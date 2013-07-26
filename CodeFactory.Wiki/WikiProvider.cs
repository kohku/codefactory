using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using CodeFactory.Wiki.Workflow;
using CodeFactory.Web.Core;

namespace CodeFactory.Wiki
{
    [Serializable]
    public abstract class WikiProvider : ProviderBase
    {
        public abstract string ApplicationName { get; }
        public abstract string DefaultStartTime { get; }
        public abstract string DefaultEndTime { get; }
        public abstract TimeSpan TimeToExpire { get; }
        public abstract bool IncludeWeekends { get; }
        public abstract string RequestSchemaPath { get; }
        public abstract string ResponseSchemaPath { get; }

        public WikiProvider()
        {
        }

        public abstract IWiki SelectWiki(Guid id);

        public abstract void InsertWiki(IWiki item);

        public abstract void UpdateWiki(IWiki item);

        public abstract void DeleteWiki(IWiki item);

        public abstract IWiki GetRandomWiki();

        public abstract IWiki GetRelatedWiki(string keyword);

        public abstract List<string> GetCategories();

        public abstract void InsertCategory(string category);

        public abstract bool DeleteCategory(string category);

        public virtual List<Guid> GetWiki(Guid? id, string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return GetWiki(id, title, description, content, author, slug, null, category, keywords, null, null, null, null, null,
                null, pageSize, pageIndex, out totalCount);
        }

        public abstract List<Guid> GetWiki(Guid? id, string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level, DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, string lastUpdatedBy,
            int pageSize, int pageIndex, out int totalCount);

        public virtual List<Guid> SearchWiki(string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return SearchWiki(title, description, content, author, slug,
                null, category, keywords, null, null,
                null, null, null,
                null, pageSize, pageIndex, out totalCount);
        }

        public virtual List<Guid> SearchWiki(string title, string description, string content, string author, string slug,
            string category, string keywords, ReachLevel? level, int pageSize, int pageIndex, out int totalCount)
        {
            return SearchWiki(title, description, content, author, slug,
                null, category, keywords, level, null,
                null, null, null,
                null, pageSize, pageIndex, out totalCount);
        }

        public abstract List<Guid> SearchWiki(string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level,
            DateTime? initialDateCreated, DateTime? finalDateCreated, DateTime? initialLastUpdated, DateTime? finalLastUpdated,
            string lastUpdatedBy, int pageSize, int pageIndex, out int totalCount);

        public abstract List<Guid> SearchWiki(string alphabeticalIndex, int pageSize, int pageIndex, out int totalCount);

        public abstract Dictionary<string, int> GetTagCloud();

        public abstract IWorkWikiItem SelectWorkWikiItem(Guid id);

        public abstract void InsertWorkWikiItem(IWorkWikiItem item);

        public abstract void UpdateWorkWikiItem(IWorkWikiItem item);

        public abstract void DeleteWorkWikiItem(IWorkWikiItem item);

        public abstract void InsertPublishedWiki(IWiki item);

        public abstract void UpdatePublishedWiki(IWiki item);

        public abstract void DeletePublishedWiki(IWiki item);

        public abstract IWorkWikiItem SelectWikiHistory(Guid id);

        public abstract void InsertWikiHistory(IWorkWikiItem item);

        public abstract void UpdateWikiHistory(IWorkWikiItem item);

        public abstract void DeleteWikiHistory(IWorkWikiItem item);

        public virtual List<Guid> GetWikiHistory(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return GetWikiHistory(trackingNumber, id, title, description, content, author, slug,
                null, category, keywords, null, null,
                null, null, null, null, null,
                null, pageSize, pageIndex, out totalCount);
        }

        public abstract List<Guid> GetWikiHistory(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level, DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, DateTime? initialExpirationDate, DateTime? finalExpirationDate,
            string lastUpdatedBy, int pageSize, int pageIndex, out int totalCount);

        public virtual List<Guid> GetWorkWikiItem(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            string category, string keywords, int pageSize, int pageIndex, out int totalCount)
        {
            return GetWorkWikiItem(trackingNumber, id, title, description, content, author, slug,
                null, category, keywords, null, null,
                null, null, null, null, null, null,
                pageSize, pageIndex, out totalCount);
        }

        public abstract List<Guid> GetWorkWikiItem(Guid? trackingNumber, Guid? id, string title, string description, string content, string author, string slug,
            bool? isVisible, string category, string keywords, ReachLevel? level, DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, DateTime? initialExpirationDate, DateTime? finalExpirationDate, string lastUpdatedBy,
            int pageSize, int pageIndex, out int totalCount);

        public abstract void RequestAuthorization(IWorkWikiItem item);

        public virtual void AuthorizationReceived(Guid trackingNumber)
        {
            WorkWikiItem item = WorkWikiItem.Load(trackingNumber);

            Dictionary<string, string> authorizers = this.GetAuthorizersByCategory();

            string authorizer = null;

            if (!authorizers.TryGetValue(item.Category, out authorizer))
            {
                item.Authorizer = authorizer;
            }

            item.Status = WikiStatus.AuthorizationRequested;

            item.Save();
        }

        public virtual void CompleteAuthorization(Guid trackingNumber, string authorizer)
        {
            WorkWikiItem item = WorkWikiItem.Load(trackingNumber);

            item.Status = WikiStatus.AuthorizationAccepted;

            // Set the authorizer if a distinct authorizer complete the task.
            if (item.Authorizer != authorizer)
                item.Authorizer = authorizer;

            item.Save();

            switch (item.Action)
            {
                case SaveAction.Delete:
                    this.DeletePublishedWiki(item);
                    break;
                case SaveAction.Insert:
                    this.InsertPublishedWiki(item);
                    break;
                case SaveAction.Update:
                    this.UpdatePublishedWiki(item);
                    break;
                default:
                    break;
            }

            this.InsertWikiHistory(item);
            item.Delete();
            item.Save();

        }

        public virtual void CompleteRejection(Guid trackingNumber, string authorizer)
        {
            WorkWikiItem item = WorkWikiItem.Load(trackingNumber);

            item.Status = WikiStatus.AuthorizationRejected;

            // Set the authorizer if a distinct authorizer complete the task.
            if (item.Authorizer != authorizer)
                item.Authorizer = authorizer;

            item.Save();

            this.InsertWikiHistory(item);

            item.Delete();
            item.Save();
        }

        public virtual void CompleteExpiration(Guid trackingNumber)
        {
            WorkWikiItem item = WorkWikiItem.Load(trackingNumber);

            item.Status = WikiStatus.AuthorizationExpired;
            item.Authorizer = "WikiSystem";
            item.Save();

            this.InsertWikiHistory(item);

            item.Delete();
            item.Save();
        }

        public abstract void AuthorizeWiki(IWorkWikiItem item);

        public abstract void RejectAuthorization(IWorkWikiItem item);

        public virtual List<IWorkWikiItem> GetPendingAuthorizations(string user)
        {
            int totalCount = 0;

            return GetPendingAuthorizations(user, int.MaxValue, 0, out totalCount);
        }

        public abstract List<IWorkWikiItem> GetPendingAuthorizations(string user, int pageSize, int pageIndex, out int totalCount);

        public virtual Dictionary<string, string> GetAuthorizersByCategory()
        {
            int totalCount = 0;
            return GetAuthorizersByCategory(int.MaxValue, 0, out totalCount);
        }

        public abstract Dictionary<string, string> GetAuthorizersByCategory(int pageSize, int pageIndex, out int totalCount);

        public abstract void InsertAuthorizerByCategory(string category, string username);

        public abstract void DeleteAuthorizerByCategory(string category);
    }
}
