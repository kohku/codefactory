using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Wiki.Workflow;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Diagnostics;
using System.Threading;
using System.Configuration.Provider;
using System.Workflow.Activities;
using System.Configuration;
using System.Data.Linq;
using System.Collections.Specialized;
using System.Data.Common;
using CodeFactory.Web.Core;
using System.Data;
using System.Web;

namespace CodeFactory.Wiki.Providers
{
    public class LinqWikiProvider : WikiProvider, IDisposable
    {
        private bool _synchronousExecution;
        private string _connectionStringName = "SqlServices";
        private string _applicationName = "CodeFactory.Wiki";

        private string _defaultStartTime = "08:00";
        private string _defaultEndTime = "07:30";
        private TimeSpan _timeToExpire = new TimeSpan(8, 0, 0);
        private bool _includeWeekends;
        private string _requestSchemaPath;
        private string _responseSchemaPath;

        public override string ApplicationName
        {
            get { return _applicationName; }
        }

        public override string DefaultStartTime
        {
            get { return _defaultStartTime; }
        }

        public override string DefaultEndTime
        {
            get { return _defaultEndTime; }
        }

        public override TimeSpan TimeToExpire
        {
            get { return _timeToExpire; }
        }

        public override bool IncludeWeekends
        {
            get { return _includeWeekends; }
        }

        public override string RequestSchemaPath
        {
            get { return _requestSchemaPath; }
        }

        public override string ResponseSchemaPath
        {
            get { return _responseSchemaPath; }
        }

        public string ConnectionStringName
        {
            get { return _connectionStringName; }
        }

        public bool SynchronousExecution
        {
            get { return _synchronousExecution; }
        }

        private static WorkflowRuntime theWorkflowRuntime;
        private static ServiceProviderHelper theServiceProvider;
        private ManualWorkflowSchedulerService theSchedulerService;


        public LinqWikiProvider()
        {
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            if (!string.IsNullOrEmpty(config["applicationName"]))
            {
                this._applicationName = config["applicationName"];
                config.Remove("applicationName");
            }

            if (!string.IsNullOrEmpty(config["connectionStringName"]))
            {
                this._connectionStringName = config["connectionStringName"];
                config.Remove("connectionStringName");
            }

            if (!string.IsNullOrEmpty(config["synchronousExecution"]))
            {
                this._synchronousExecution = Convert.ToBoolean(config["synchronousExecution"]);
                config.Remove("synchronousExecution");
            }

            if (!string.IsNullOrEmpty(config["defaultStartTime"]))
            {
                this._defaultEndTime = config["defaultStartTime"];
                config.Remove("defaultStartTime");
            }

            if (!string.IsNullOrEmpty(config["defaultEndTime"]))
            {
                this._defaultEndTime = config["defaultEndTime"];
                config.Remove("defaultEndTime");
            }

            if (!string.IsNullOrEmpty(config["timeToExpire"]))
            {
                this._timeToExpire = TimeSpan.Parse(config["timeToExpire"]);
                config.Remove("timeToExpire");
            }

            if (!string.IsNullOrEmpty(config["includeWeekends"]))
            {
                this._includeWeekends = Convert.ToBoolean(config["includeWeekends"]);
                config.Remove("includeWeekends");
            }

            if (!string.IsNullOrEmpty(config["requestSchemaPath"]))
            {
                this._requestSchemaPath = config["requestSchemaPath"];
                config.Remove("requestSchemaPath");
            }

            if (!string.IsNullOrEmpty(config["responseSchemaPath"]))
            {
                this._responseSchemaPath = config["responseSchemaPath"];
                config.Remove("responseSchemaPath");
            }

            if (config.Count > 0)
                throw new ProviderException(string.Format("Unknown config attribute '{0}'", config.GetKey(0)));

            #region Workflow Runtime Initialization

            if (theWorkflowRuntime == null)
            {
                lock (this)
                {
                    if (theWorkflowRuntime == null)
                    {
                        theWorkflowRuntime = new WorkflowRuntime();

                        theWorkflowRuntime.WorkflowLoaded += new EventHandler<WorkflowEventArgs>(theWorkflowRuntime_WorkflowLoaded);
                        theWorkflowRuntime.WorkflowIdled += new EventHandler<WorkflowEventArgs>(theWorkflowRuntime_WorkflowIdled);
                        theWorkflowRuntime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(theWorkflowRuntime_WorkflowCompleted);
                        theWorkflowRuntime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(theWorkflowRuntime_WorkflowTerminated);
                        theWorkflowRuntime.WorkflowSuspended += new EventHandler<WorkflowSuspendedEventArgs>(theWorkflowRuntime_WorkflowSuspended);

                        // Used for synchronous execution of workflow instances.
                        if (this.SynchronousExecution)
                            theSchedulerService = theWorkflowRuntime.GetService<ManualWorkflowSchedulerService>();

                        // Add the external data service
                        ExternalDataExchangeService dataService = new ExternalDataExchangeService();
                        theWorkflowRuntime.AddService(dataService);

                        // Add custom wiki management service.
                        theServiceProvider = new ServiceProviderHelper();
                        dataService.AddService(theServiceProvider);

                        // Add system SQL state service.
                        SqlWorkflowPersistenceService stateService = new SqlWorkflowPersistenceService(
                            ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);
                        theWorkflowRuntime.AddService(stateService);

                        try
                        {
                            Trace.TraceInformation("Starting workflow engine.");
                            // Start
                            theWorkflowRuntime.StartRuntime();
                            Trace.TraceInformation("Workflow engine was sucessfully started.");
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(string.Format(
                                "Error while starting the workflow engine. Description: {0}", ex.Message));
                            theWorkflowRuntime.Dispose();
                            throw;
                        }
                    }
                }
            }

            #endregion
        }

        #region Workflow engine operations

        private void theWorkflowRuntime_WorkflowSuspended(object sender, WorkflowSuspendedEventArgs e)
        {
            Trace.TraceInformation(string.Format(
                "Workflow report: The instance {0} was suspended.", e.WorkflowInstance));
        }

        // This method is called when the workflow terminates and does not complete
        // It is good practice to include a handler for this event 
        // so the host application can manage workflows that are
        // unexpectedly terminated (e.g. unhandled workflow exception).
        // waitHandle is set so the main thread can continue
        private void theWorkflowRuntime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            WorkWikiItem workitem = WorkWikiItem.Load(e.WorkflowInstance.InstanceId);

            if (workitem != null)
            {
                workitem.Messages = e.Exception.Message;
                workitem.Save();
            }

            Trace.TraceWarning(string.Format("Workflow report: The instance {0} was unexpectdly terminated. Message: {1}",
                e.WorkflowInstance.InstanceId, e.Exception.Message));
        }

        // This method will be called when a workflow instance is completed; since we have started only a single
        // instance we are ignoring the event args and signaling the waitHandle so the main thread can continue
        private void theWorkflowRuntime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            Trace.TraceInformation(string.Format(
                "Workflow report: The instance {0} completed sucessfully.", e.WorkflowInstance.InstanceId));
        }

        private void theWorkflowRuntime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            Trace.TraceInformation(string.Format(
                "Workflow report: The instance {0} is going idled.", e.WorkflowInstance.InstanceId));
            ThreadPool.QueueUserWorkItem(UnloadInstance, e.WorkflowInstance);
        }

        static void UnloadInstance(object workflowInstance)
        {
            try
            {
                ((WorkflowInstance)workflowInstance).TryUnload();
            }
            // There is no worflow instance with id registered in the persistence service.
            catch (InvalidOperationException)
            {
            }
        }

        // Fired when the workflow is loaded from storage
        private void theWorkflowRuntime_WorkflowLoaded(object sender, WorkflowEventArgs e)
        {
            Trace.TraceInformation(string.Format(
                "Workflow report: The instance {0} is loading from storage.", e.WorkflowInstance.InstanceId));
        }

        public void Shutdown()
        {
            Trace.TraceInformation("Stoping the workflow engine.");
            if ((theWorkflowRuntime != null) && theWorkflowRuntime.IsStarted)
                theWorkflowRuntime.StopRuntime();
            Trace.TraceInformation("Workflow engine was stopped.");
        }

        #endregion

        public override IWiki SelectWiki(Guid id)
        {
            Wiki wiki = null;

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            LinqWiki row = wikis.SingleOrDefault<LinqWiki>(
                w => w.ApplicationName == this.ApplicationName && w.ID == id);

            if (row == null)
                return null;

            wiki = new Wiki(row.ID)
            {
                Author = row.Author,
                Category = row.Category,
                Content = row.Content,
                DateCreated = row.DateCreated,
                DepartmentArea = row.DepartmentArea,
                Description = row.Description,
                Editable = row.Editable,
                Editor = row.Editor,
                IsVisible = row.IsVisible,
                Keywords = row.Keywords,
                LastUpdated = row.LastUpdated,
                LastUpdatedBy = row.LastUpdatedBy,
                ReachLevel = (ReachLevel)row.ReachLevel,
                Slug = row.Slug,
                Title = row.Title
            };

            return wiki;
        }

        public override void InsertWiki(IWiki item)
        {
            WorkWikiItem workitem = new WorkWikiItem(item);
            workitem.Action = SaveAction.Insert;

            RequestAuthorization(workitem);
        }

        public override void InsertPublishedWiki(IWiki item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            wikis.InsertOnSubmit(new LinqWiki()
            {
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ID = item.ID,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Title = item.Title
            });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceWarning(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
                return;
            }
        }

        public override void UpdateWiki(IWiki item)
        {
            WorkWikiItem workitem = new WorkWikiItem(item);
            workitem.Action = SaveAction.Update;

            RequestAuthorization(workitem);
        }

        public override void UpdatePublishedWiki(IWiki item)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            IWiki w = SelectWiki(item.ID);

            // Assume that "wiki" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            wikis.Attach(new LinqWiki(){
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ID = item.ID,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = w.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Title = item.Title
            }, true);

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException)
            {
                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override void DeleteWiki(IWiki item)
        {
            WorkWikiItem workitem = new WorkWikiItem(item);
            workitem.Action = SaveAction.Delete;

            RequestAuthorization(workitem);
        }

        public override void DeletePublishedWiki(IWiki item)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            LinqWiki w = new LinqWiki()
            {
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ID = item.ID,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Title = item.Title
            };

            // Assume that "wiki" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            wikis.Attach(w);

            wikis.DeleteOnSubmit(w);

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceError(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override IWiki GetRandomWiki()
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            var query = db.GetRandomWiki(this.ApplicationName);

            GetRandomWikiResult result = query.SingleOrDefault();

            if (result == null)
                return null;

            return SelectWiki(result.id);
        }

        public override IWiki GetRelatedWiki(string keyword)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            var query = db.GetRelatedWiki(this.ApplicationName, keyword);

            GetRelatedWikiResult result = query.SingleOrDefault();

            if (result == null)
                return null;

            return SelectWiki(result.id);
        }

        public override List<string> GetCategories()
        {
            List<string> categories = new List<string>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<WikiCategory> wikiCategories = db.GetTable<WikiCategory>();

            var query = from c in wikiCategories
                        where c.ApplicationName == this.ApplicationName
                        select c;

            foreach (WikiCategory item in query)
            {
                categories.Add(item.ApplicationName);
            }

            return categories;
        }

        public override void InsertCategory(string category)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<WikiCategory> categories = db.GetTable<WikiCategory>();

            categories.InsertOnSubmit(new WikiCategory() { ApplicationName = this.ApplicationName, CategoryName = category });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceWarning(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
                return;
            }
        }

        public override bool DeleteCategory(string category)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<WikiCategory> categories = db.GetTable<WikiCategory>();

            categories.DeleteOnSubmit(new WikiCategory() { ApplicationName = this.ApplicationName, CategoryName = category });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceWarning(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }

            return true;
        }

        public override List<Guid> GetWiki(Guid? id, string title, string description, string content, string author,
            string slug, bool? isVisible, string category, string keywords, ReachLevel? level, 
            DateTime? initialDateCreated, DateTime? finalDateCreated, DateTime? initialLastUpdated, DateTime? finalLastUpdated, 
            string lastUpdatedBy, int pageSize, int pageIndex, out int totalCount)
        {
            List<Guid> records = new List<Guid>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            var query = from w in wikis
                        where w.ApplicationName == this.ApplicationName
                        where w.Author.Contains(!string.IsNullOrEmpty(author) ? author : string.Empty)
                        where w.Category.Contains(!string.IsNullOrEmpty(category) ? category : string.Empty)
                        where w.Content.Contains(!string.IsNullOrEmpty(content) ? content : string.Empty)
                        where w.DateCreated >= (initialDateCreated.HasValue && initialDateCreated != DateTime.MinValue ? initialDateCreated : w.DateCreated)
                        where w.DateCreated <= (finalDateCreated.HasValue && finalDateCreated != DateTime.MinValue ? finalDateCreated : w.DateCreated)
                        where (description != null && w.Description != null ? w.Description : string.Empty) == (description != null ? description : string.Empty)
                        where w.ID == (id.HasValue && id != Guid.Empty ? id : w.ID)
                        where w.IsVisible == (isVisible.HasValue ? isVisible : w.IsVisible)
                        where (keywords != null && w.Keywords != null ? w.Keywords : string.Empty) == (keywords != null ? keywords : string.Empty)
                        where w.LastUpdated >= (initialLastUpdated.HasValue && initialLastUpdated != DateTime.MinValue ? initialLastUpdated : w.LastUpdated)
                        where w.LastUpdated <= (finalLastUpdated.HasValue && finalLastUpdated != DateTime.MinValue ? finalLastUpdated : w.LastUpdated)
                        where (lastUpdatedBy != null && w.LastUpdatedBy != null ? w.LastUpdatedBy : string.Empty) == (lastUpdatedBy != null ? lastUpdatedBy : string.Empty)
                        where (ReachLevel)w.ReachLevel == (level.HasValue ? level : (ReachLevel)w.ReachLevel)
                        where w.Slug.Contains(!string.IsNullOrEmpty(slug) ? slug : string.Empty)
                        where w.Title.Contains(!string.IsNullOrEmpty(title) ? title : string.Empty)
                        select w.ID;

            totalCount = query.Count();

            foreach (Guid itemId in query.Skip(pageSize * pageIndex).Take(pageSize))
                records.Add(itemId);

            return records;
        }

        private class SearchWikiResult
        {
            private Guid _id;
            private string _author;
            private bool _isVisible;
            private DateTime _dateCreated;
            private ReachLevel _reachLevel;
            private DateTime? _lastUpdated;
            private string _lastUpdatedBy;
            private int _rate;

            public SearchWikiResult()
            {
            }

            public Guid ID
            {
                get { return _id; }
                set { _id = value; }
            }
            public string Author
            {
                get { return _author; }
                set { _author = value; }
            }
            public bool IsVisible
            {
                get { return _isVisible; }
                set { _isVisible = value; }
            }
            public DateTime DateCreated
            {
                get { return _dateCreated; }
                set { _dateCreated = value; }
            }
            public ReachLevel ReachLevel 
            {
                get { return _reachLevel; }
                set { _reachLevel = value; } 
            }
            public DateTime? LastUpdated
            {
                get { return _lastUpdated; }
                set { _lastUpdated = value; }
            }
            public string LastUpdatedBy
            {
                get { return _lastUpdatedBy; }
                set { _lastUpdatedBy = value; }
            }
            public int Rate
            {
                get { return _rate; }
                set { _rate = value; }
            }
        }

        public override List<Guid> SearchWiki(string title, string description, string content, string author,
            string slug, bool? isVisible, string category, string keywords, ReachLevel? level, 
            DateTime? initialDateCreated, DateTime? finalDateCreated, DateTime? initialLastUpdated, DateTime? finalLastUpdated, 
            string lastUpdatedBy, int pageSize, int pageIndex, out int totalCount)
        {
            List<Guid> records = new List<Guid>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            var q0 = (from w in wikis
                      where w.ApplicationName == this.ApplicationName
                      select new SearchWikiResult()
                      {
                          ID = Guid.Empty,
                          Author = w.Author,
                          IsVisible = w.IsVisible,
                          DateCreated = w.DateCreated,
                          ReachLevel = (ReachLevel)w.ReachLevel,
                          LastUpdated = w.LastUpdated,
                          LastUpdatedBy = w.LastUpdatedBy,
                          Rate = 10
                      }).Distinct();

            var q1 = from w in wikis
                     where w.ApplicationName == this.ApplicationName
                     where w.Slug.Contains(slug)
                     select new SearchWikiResult()
                     { 
                         ID = w.ID, 
                         Author = w.Author, 
                         IsVisible = w.IsVisible,
                         DateCreated = w.DateCreated,
                         ReachLevel = (ReachLevel)w.ReachLevel,
                         LastUpdated = w.LastUpdated, 
                         LastUpdatedBy = w.LastUpdatedBy, 
                         Rate = 100
                     };

            var q2 = from w in wikis
                     where w.ApplicationName == this.ApplicationName
                     where w.Title.Contains(title)
                     select new SearchWikiResult()
                     {
                         ID = w.ID,
                         Author = w.Author,
                         IsVisible = w.IsVisible,
                         DateCreated = w.DateCreated,
                         ReachLevel = (ReachLevel)w.ReachLevel,
                         LastUpdated = w.LastUpdated,
                         LastUpdatedBy = w.LastUpdatedBy,
                         Rate = 90
                     };

            var q3 = from w in wikis
                     where w.ApplicationName == this.ApplicationName
                     where w.Keywords.Contains(keywords)
                     select new SearchWikiResult()
                     {
                         ID = w.ID,
                         Author = w.Author,
                         IsVisible = w.IsVisible,
                         DateCreated = w.DateCreated,
                         ReachLevel = (ReachLevel)w.ReachLevel,
                         LastUpdated = w.LastUpdated,
                         LastUpdatedBy = w.LastUpdatedBy,
                         Rate = 80
                     };

            var q4 = from w in wikis
                     where w.ApplicationName == this.ApplicationName
                     where w.Category.Contains(category)
                     select new SearchWikiResult()
                     {
                         ID = w.ID,
                         Author = w.Author,
                         IsVisible = w.IsVisible,
                         DateCreated = w.DateCreated,
                         ReachLevel = (ReachLevel)w.ReachLevel,
                         LastUpdated = w.LastUpdated,
                         LastUpdatedBy = w.LastUpdatedBy,
                         Rate = 70
                     };

            var q5 = from w in wikis
                     where w.ApplicationName == this.ApplicationName
                     where w.Description.Contains(description)
                     select new SearchWikiResult()
                     {
                         ID = w.ID,
                         Author = w.Author,
                         IsVisible = w.IsVisible,
                         DateCreated = w.DateCreated,
                         ReachLevel = (ReachLevel)w.ReachLevel,
                         LastUpdated = w.LastUpdated,
                         LastUpdatedBy = w.LastUpdatedBy,
                         Rate = 60
                     };

            var q6 = from w in wikis
                     where w.ApplicationName == this.ApplicationName
                     where w.Content.Contains(content)
                     select new SearchWikiResult()
                     {
                         ID = w.ID,
                         Author = w.Author,
                         IsVisible = w.IsVisible,
                         DateCreated = w.DateCreated,
                         ReachLevel = (ReachLevel)w.ReachLevel,
                         LastUpdated = w.LastUpdated,
                         LastUpdatedBy = w.LastUpdatedBy,
                         Rate = 50
                     };

            var q7 = q0;

            if (!string.IsNullOrEmpty(slug))
                q7 = q0.Union(q1);

            if (!string.IsNullOrEmpty(title))
                q7 = q7.Union(q2);

            if (!string.IsNullOrEmpty(keywords))
                q7 = q7.Union(q3);

            if (!string.IsNullOrEmpty(category))
                q7 = q7.Union(q4);

            if (!string.IsNullOrEmpty(description))
                q7 = q7.Union(q5);

            if (!string.IsNullOrEmpty(content))
                q7 = q7.Union(q6);

            var query = (from w in q7
                        where w.ID != Guid.Empty
                        where w.IsVisible == (isVisible.HasValue ? isVisible : w.IsVisible)
                        where w.DateCreated >= (initialDateCreated.HasValue && initialDateCreated != DateTime.MinValue ? initialDateCreated : w.DateCreated)
                        where w.DateCreated <= (finalDateCreated.HasValue && finalDateCreated != DateTime.MinValue ? finalDateCreated : w.DateCreated)
                        where (ReachLevel)w.ReachLevel >= (level.HasValue ? level : (ReachLevel)w.ReachLevel)
                        where w.LastUpdated >= (initialLastUpdated.HasValue && initialLastUpdated != DateTime.MinValue ? initialLastUpdated : w.LastUpdated)
                        where w.LastUpdated <= (finalLastUpdated.HasValue && finalLastUpdated != DateTime.MinValue ? finalLastUpdated : w.LastUpdated)
                        where (lastUpdatedBy != null && w.LastUpdatedBy != null ? w.LastUpdatedBy : string.Empty) == (lastUpdatedBy != null ? lastUpdatedBy : string.Empty)
                        orderby w.Rate descending
                        select w.ID
                        ).Distinct();

            totalCount = query.Count();

            foreach (Guid id in query.Skip(pageSize * pageIndex).Take(pageSize))
                records.Add(id);

            return records;
        }

        public override List<Guid> SearchWiki(string alphabeticalIndex, int pageSize, int pageIndex, out int totalCount)
        {
            List<Guid> records = new List<Guid>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWiki> wikis = db.GetTable<LinqWiki>();

            var query = (
                        from w in wikis
                        where w.ApplicationName == this.ApplicationName
                        where w.Keywords.StartsWith(alphabeticalIndex)
                        select w.ID
                        ).Union(
                        from w in wikis
                        where w.ApplicationName == this.ApplicationName
                        where w.Title.StartsWith(alphabeticalIndex)
                        select w.ID
                        );

            totalCount = query.Count();

            foreach (Guid id in query)
            {
                records.Add(id);
            }

            return records;
        }

        public override Dictionary<string, int> GetTagCloud()
        {
            Dictionary<string, int> tagCloud = new Dictionary<string, int>();

            AlternateWikiDataContext db = new AlternateWikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            var query = db.GetTagCloud(this.ApplicationName);

            foreach (TagCloudResult tag in query)
                tagCloud.Add(tag.Keyword, tag.Hits);

            return tagCloud;
        }

        public override IWorkWikiItem SelectWorkWikiItem(Guid id)
        {
            WorkWikiItem wiki = null;

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWorkWikiItem> wikis = db.GetTable<LinqWorkWikiItem>();

            LinqWorkWikiItem row = wikis.SingleOrDefault<LinqWorkWikiItem>(
                w => w.ApplicationName == this.ApplicationName && w.TrackingNumber == id);

            if (row == null)
                return null;

            wiki = new WorkWikiItem(new Wiki(row.ID)
            {
                Author = row.Author,
                Category = row.Category,
                Content = row.Content,
                DateCreated = row.DateCreated,
                DepartmentArea = row.DepartmentArea,
                Description = row.Description,
                Editable = row.Editable,
                Editor = row.Editor,
                IsVisible = row.IsVisible,
                Keywords = row.Keywords,
                LastUpdated = row.LastUpdated,
                LastUpdatedBy = row.LastUpdatedBy,
                ReachLevel = (ReachLevel)row.ReachLevel,
                Slug = row.Slug,
                Title = row.Title,
            })
            {
                Action = (SaveAction)row.Action,
                Authorizer = row.Authorizer,
                ExpirationDate = row.ExpirationDate,
                IPAddress = row.IpAddress,
                Messages = row.Messages,
                Status = (WikiStatus)row.Status,
                TrackingNumber = row.TrackingNumber
            };

            return wiki;
        }

        public override void InsertWorkWikiItem(IWorkWikiItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWorkWikiItem> wikis = db.GetTable<LinqWorkWikiItem>();

            wikis.InsertOnSubmit(new LinqWorkWikiItem()
            {
                Action = (int)item.Action,
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Authorizer = item.Authorizer,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ExpirationDate = item.ExpirationDate,
                ID = item.ID,
                IpAddress = item.IPAddress,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                Messages = item.Messages,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Status = (int)item.Status,
                Title = item.Title,
                TrackingNumber = item.TrackingNumber
            });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceWarning(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
                return;
            }
        }

        public override void UpdateWorkWikiItem(IWorkWikiItem item)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWorkWikiItem> wikis = db.GetTable<LinqWorkWikiItem>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            IWiki w = SelectWorkWikiItem(item.TrackingNumber);

            // Assume that "wiki" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            wikis.Attach(new LinqWorkWikiItem()
            {
                Action = (int)item.Action,
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Authorizer = item.Authorizer,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ExpirationDate = item.ExpirationDate,
                ID = item.ID,
                IpAddress = item.IPAddress,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = w.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                Messages = item.Messages,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Status = (int)item.Status,
                Title = item.Title,
                TrackingNumber = item.TrackingNumber
            }, true);

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException)
            {
                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override void DeleteWorkWikiItem(IWorkWikiItem item)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWorkWikiItem> wikis = db.GetTable<LinqWorkWikiItem>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            IWiki wi = SelectWorkWikiItem(item.TrackingNumber);

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            LinqWorkWikiItem w = new LinqWorkWikiItem()
            {
                Action = (int)item.Action,
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Authorizer = item.Authorizer,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ExpirationDate = item.ExpirationDate,
                ID = item.ID,
                IpAddress = item.IPAddress,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = wi.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                Messages = item.Messages,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Status = (int)item.Status,
                Title = item.Title,
                TrackingNumber = item.TrackingNumber
            };

            // Assume that "wiki" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            wikis.Attach(w);

            wikis.DeleteOnSubmit(w);

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceError(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override IWorkWikiItem SelectWikiHistory(Guid id)
        {
            WikiHistory wiki = null;

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWikiHistory> wikis = db.GetTable<LinqWikiHistory>();

            LinqWikiHistory row = wikis.SingleOrDefault<LinqWikiHistory>(
                w => w.ApplicationName == this.ApplicationName && w.TrackingNumber == id);

            if (row == null)
                return null;

            wiki = new WikiHistory(new Wiki(row.ID)
            {
                Author = row.Author,
                Category = row.Category,
                Content = row.Content,
                DateCreated = row.DateCreated,
                DepartmentArea = row.DepartmentArea,
                Description = row.Description,
                Editable = row.Editable,
                Editor = row.Editor,
                IsVisible = row.IsVisible,
                Keywords = row.Keywords,
                LastUpdated = row.LastUpdated,
                LastUpdatedBy = row.LastUpdatedBy,
                ReachLevel = (ReachLevel)row.ReachLevel,
                Slug = row.Slug,
                Title = row.Title
            })
            {
                Action = (SaveAction)row.Action,
                Authorizer = row.Authorizer,
                ExpirationDate = row.ExpirationDate,
                IPAddress = row.IpAddress,
                Messages = row.Messages,
                Status = (WikiStatus)row.Status,
                TrackingNumber = row.TrackingNumber
            };

            return wiki;
        }

        public override void InsertWikiHistory(IWorkWikiItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWikiHistory> wikis = db.GetTable<LinqWikiHistory>();

            wikis.InsertOnSubmit(new LinqWikiHistory()
            {
                Action = (int)item.Action,
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Authorizer = item.Authorizer,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ExpirationDate = item.ExpirationDate,
                ID = item.ID,
                IpAddress = item.IPAddress,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                Messages = item.Messages,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Status = (int)item.Status,
                Title = item.Title,
                TrackingNumber = item.TrackingNumber
            });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceWarning(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
                return;
            }
        }

        public override void UpdateWikiHistory(IWorkWikiItem item)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWikiHistory> wikis = db.GetTable<LinqWikiHistory>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            IWiki w = SelectWikiHistory(item.TrackingNumber);

            // Assume that "wiki" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            wikis.Attach(new LinqWikiHistory()
            {
                Action = (int)item.Action,
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Authorizer = item.Authorizer,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ExpirationDate = item.ExpirationDate,
                ID = item.ID,
                IpAddress = item.IPAddress,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                Messages = item.Messages,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Status = (int)item.Status,
                Title = item.Title,
                TrackingNumber = item.TrackingNumber
            }, true);

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException)
            {
                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override void DeleteWikiHistory(IWorkWikiItem item)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWikiHistory> wikis = db.GetTable<LinqWikiHistory>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            LinqWikiHistory w = new LinqWikiHistory()
            {
                Action = (int)item.Action,
                ApplicationName = this.ApplicationName,
                Author = item.Author,
                Authorizer = item.Authorizer,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                ExpirationDate = item.ExpirationDate,
                ID = item.ID,
                IpAddress = item.IPAddress,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                Messages = item.Messages,
                ReachLevel = (int)item.ReachLevel,
                Slug = item.Slug,
                Status = (int)item.Status,
                Title = item.Title,
                TrackingNumber = item.TrackingNumber
            };

            // Assume that "wiki" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            wikis.Attach(w);

            wikis.DeleteOnSubmit(w);

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceError(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override List<Guid> GetWikiHistory(Guid? trackingNumber, Guid? id, string title, string description,
            string content, string author, string slug, bool? isVisible, string category, string keywords, ReachLevel? level, 
            DateTime? initialDateCreated, DateTime? finalDateCreated, DateTime? initialLastUpdated, DateTime? finalLastUpdated, 
            DateTime? initialExpirationDate, DateTime? finalExpirationDate, string lastUpdatedBy, 
            int pageSize, int pageIndex, out int totalCount)
        {
            List<Guid> records = new List<Guid>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWikiHistory> history = db.GetTable<LinqWikiHistory>();

            var query = from h in history
                        where h.ApplicationName == this.ApplicationName
                        where h.Author.Contains(!string.IsNullOrEmpty(author) ? author : string.Empty)
                        where h.Category.Contains(!string.IsNullOrEmpty(category) ? category : string.Empty)
                        where h.Content.Contains(!string.IsNullOrEmpty(content) ? content : string.Empty)
                        where h.DateCreated >= (initialDateCreated.HasValue && initialDateCreated != DateTime.MinValue ? initialDateCreated : h.DateCreated)
                        where h.DateCreated <= (finalDateCreated.HasValue && finalDateCreated != DateTime.MinValue ? finalDateCreated : h.DateCreated)
                        where (description != null && h.Description != null ? h.Description : string.Empty) == (description != null ? description : string.Empty)
                        where initialExpirationDate != null && h.ExpirationDate >= (initialExpirationDate.HasValue && initialExpirationDate != DateTime.MinValue ? initialExpirationDate.Value : h.ExpirationDate)
                        where finalExpirationDate != null && h.ExpirationDate <= (finalExpirationDate.HasValue && finalExpirationDate != DateTime.MinValue ? finalExpirationDate.Value : h.ExpirationDate)
                        where h.ID == (id.HasValue && id != Guid.Empty ? id : h.ID)
                        where h.IsVisible == (isVisible.HasValue ? isVisible : h.IsVisible)
                        where (keywords != null && h.Keywords != null ? h.Keywords : string.Empty) == (keywords != null ? keywords : string.Empty)
                        where h.LastUpdated >= (initialLastUpdated.HasValue && initialLastUpdated != DateTime.MinValue ? initialLastUpdated : h.LastUpdated)
                        where h.LastUpdated <= (finalLastUpdated.HasValue && finalLastUpdated != DateTime.MinValue ? finalLastUpdated : h.LastUpdated)
                        where (lastUpdatedBy != null && h.LastUpdatedBy != null ? h.LastUpdatedBy : string.Empty) == (lastUpdatedBy != null ? lastUpdatedBy : string.Empty)
                        where (ReachLevel)h.ReachLevel == (level.HasValue ? level : (ReachLevel)h.ReachLevel)
                        where h.Slug.Contains(!string.IsNullOrEmpty(slug) ? slug : string.Empty)
                        where h.Title.Contains(!string.IsNullOrEmpty(title) ? title : string.Empty)
                        where h.TrackingNumber == (trackingNumber.HasValue ? trackingNumber : h.TrackingNumber)
                        select h.TrackingNumber;

            totalCount = query.Count();

            foreach (Guid historyId in query.Skip(pageSize * pageIndex).Take(pageSize))
                records.Add(historyId);

            return records;
        }

        public override List<Guid> GetWorkWikiItem(Guid? trackingNumber, Guid? id, string title, string description,
            string content, string author, string slug, bool? isVisible, string category, string keywords, ReachLevel? level, 
            DateTime? initialDateCreated, DateTime? finalDateCreated, DateTime? initialLastUpdated, DateTime? finalLastUpdated, 
            DateTime? initialExpirationDate, DateTime? finalExpirationDate, string lastUpdatedBy, 
            int pageSize, int pageIndex, out int totalCount)
        {
            List<Guid> records = new List<Guid>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWorkWikiItem> workitems = db.GetTable<LinqWorkWikiItem>();

            var query = from w in workitems
                        where w.ApplicationName == this.ApplicationName
                        where w.Author.Contains(!string.IsNullOrEmpty(author) ? author : string.Empty)
                        where w.Category.Contains(!string.IsNullOrEmpty(category) ? category : string.Empty)
                        where w.Content.Contains(!string.IsNullOrEmpty(content) ? content : string.Empty)
                        where w.DateCreated >= (initialDateCreated.HasValue && initialDateCreated != DateTime.MinValue ? initialDateCreated : w.DateCreated)
                        where w.DateCreated <= (finalDateCreated.HasValue && finalDateCreated != DateTime.MinValue ? finalDateCreated : w.DateCreated)
                        where (description != null && w.Description != null ? w.Description : string.Empty) == (description != null ? description : string.Empty)
                        where initialExpirationDate != null && w.ExpirationDate >= (initialExpirationDate.HasValue && initialExpirationDate != DateTime.MinValue ? initialExpirationDate.Value : w.ExpirationDate)
                        where finalExpirationDate != null && w.ExpirationDate <= (finalExpirationDate.HasValue && finalExpirationDate != DateTime.MinValue ? finalExpirationDate.Value : w.ExpirationDate)
                        where w.ID == (id.HasValue && id != Guid.Empty ? id : w.ID)
                        where w.IsVisible == (isVisible.HasValue ? isVisible : w.IsVisible)
                        where (keywords != null && w.Keywords != null ? w.Keywords : string.Empty) == (keywords != null ? keywords : string.Empty)
                        where w.LastUpdated >= (initialLastUpdated.HasValue && initialLastUpdated != DateTime.MinValue ? initialLastUpdated : w.LastUpdated)
                        where w.LastUpdated <= (finalLastUpdated.HasValue && finalLastUpdated != DateTime.MinValue ? finalLastUpdated : w.LastUpdated)
                        where (lastUpdatedBy != null && w.LastUpdatedBy != null ? w.LastUpdatedBy : string.Empty) == (lastUpdatedBy != null ? lastUpdatedBy : string.Empty)
                        where (ReachLevel)w.ReachLevel == (level.HasValue ? level : (ReachLevel)w.ReachLevel)
                        where w.Slug.Contains(!string.IsNullOrEmpty(slug) ? slug : string.Empty)
                        where w.Title.Contains(!string.IsNullOrEmpty(title) ? title : string.Empty)
                        where w.TrackingNumber == (trackingNumber.HasValue ? trackingNumber : w.TrackingNumber)
                        select w.TrackingNumber;

            totalCount = query.Count();

            foreach (Guid workitemId in query.Skip(pageSize * pageIndex).Take(pageSize))
                records.Add(workitemId);

            return records;
        }

        public override void RequestAuthorization(IWorkWikiItem item)
        {
            try
            {
                // Fill the parameters collection for this instance of the workflow
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("WikiID", item.ID);

                Type type = typeof(AuthorizeEntries);

                WorkflowInstance instance = theWorkflowRuntime.CreateWorkflow(type, parameters);

                item.Status = WikiStatus.Processing;
                item.TrackingNumber = instance.InstanceId;
                item.IPAddress = HttpContext.Current.Request.UserHostAddress;
                item.Save();

                instance.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("PRIMARY KEY"))
                    throw new InvalidConstraintException("InsertWiki failed.", ex);
                else
                    throw ex;
            }
        }

        public override void AuthorizeWiki(IWorkWikiItem item)
        {
            try
            {
                WorkflowInstance instance = theWorkflowRuntime.GetWorkflow(item.TrackingNumber);
                instance.Load();

                item.Status = WikiStatus.Processing;

                item.Save();

                theServiceProvider.AcceptAuthorization(item.TrackingNumber, item.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void RejectAuthorization(IWorkWikiItem item)
        {
            try
            {
                WorkflowInstance instance = theWorkflowRuntime.GetWorkflow(item.TrackingNumber);
                instance.Load();

                item.Status = WikiStatus.Processing;

                item.Save();

                theServiceProvider.RejectAuthorization(item.TrackingNumber, item.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<IWorkWikiItem> GetPendingAuthorizations(string user, int pageSize, int pageIndex, out int totalCount)
        {
            List<IWorkWikiItem> pending = new List<IWorkWikiItem>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqWorkWikiItem> workitems = db.GetTable<LinqWorkWikiItem>();

            var query = from w in workitems
                        where w.ApplicationName == this.ApplicationName
                        where w.Status == (int)WikiStatus.AuthorizationRequested
                        select w;

            totalCount = query.Count();

            foreach (LinqWorkWikiItem item in query.Skip(pageSize * pageIndex).Take(pageSize))
            {
                pending.Add(new WorkWikiItem(new Wiki(item.ID)
            {
                Author = item.Author,
                Category = item.Category,
                Content = item.Content,
                DateCreated = item.DateCreated,
                DepartmentArea = item.DepartmentArea,
                Description = item.Description,
                Editable = item.Editable,
                Editor = item.Editor,
                IsVisible = item.IsVisible,
                Keywords = item.Keywords,
                LastUpdated = item.LastUpdated,
                LastUpdatedBy = item.LastUpdatedBy,
                ReachLevel = (ReachLevel)item.ReachLevel,
                Slug = item.Slug,
                Title = item.Title,
            })
            {
                Action = (SaveAction)item.Action,
                Authorizer = item.Authorizer,
                ExpirationDate = item.ExpirationDate,
                IPAddress = item.IpAddress,
                Messages = item.Messages,
                Status = (WikiStatus)item.Status,
                TrackingNumber = item.TrackingNumber
            });
            }             

            return pending;
        }

        public override Dictionary<string, string> GetAuthorizersByCategory(int pageSize, int pageIndex, out int totalCount)
        {
            Dictionary<string, string> authorizers = new Dictionary<string, string>();

            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<WikiAuthorizer> categories = db.GetTable<WikiAuthorizer>();

            var query = from c in categories
                        where c.ApplicationName == this.ApplicationName
                        select c;

            totalCount = query.Count();

            foreach (WikiAuthorizer authorizer in query.Skip(pageIndex * pageSize).Take(pageSize))
                authorizers.Add(authorizer.Category, authorizer.UserName);

            return authorizers;
        }

        public override void InsertAuthorizerByCategory(string category, string username)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<WikiAuthorizer> categories = db.GetTable<WikiAuthorizer>();

            categories.InsertOnSubmit(new WikiAuthorizer()
            {
                ApplicationName = this.ApplicationName,
                Category = category,
                UserName = username
            });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceError(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public override void DeleteAuthorizerByCategory(string category)
        {
            WikiDataContext db = new WikiDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<WikiAuthorizer> categories = db.GetTable<WikiAuthorizer>();

            categories.DeleteOnSubmit(new WikiAuthorizer()
            {
                ApplicationName = this.ApplicationName,
                Category = category
            });

            try
            {
                db.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
            catch (ChangeConflictException ex)
            {
                Trace.TraceError(ex.Message);

                // All database values overwrite current values.
                foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                    occ.Resolve(RefreshMode.OverwriteCurrentValues);
            }
            catch (DbException ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Shutdown();
        }

        #endregion
    }
}
