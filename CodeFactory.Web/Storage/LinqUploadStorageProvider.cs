#define GETFILES_W_SPROC
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Data.Linq;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Web.Hosting;

namespace CodeFactory.Web.Storage
{
    public class LinqUploadStorageProvider : UploadStorageProvider
    {
        private string _connectionStringName;

        public LinqUploadStorageProvider()
        {
        }

        public string ConnectionStringName
        {
            get { return _connectionStringName; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (string.IsNullOrEmpty(config["connectionStringName"]))
                throw new ProviderException("connectionStringName config attribute must be specified.");

            this._connectionStringName = config["connectionStringName"];
            config.Remove("connectionStringName");

            if (config.Count > 0)
                throw new ProviderException(string.Format("Unknown config attribute '{0}'", config.GetKey(0)));
        }

        public override void InsertFile(UploadedFile file)
        {
            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqUploadedFile> files = db.GetTable<LinqUploadedFile>();

            file.ApplicationName = this.ApplicationName;

            files.InsertOnSubmit(new LinqUploadedFile(file));

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

        public override void UpdateFile(UploadedFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqUploadedFile> files = db.GetTable<LinqUploadedFile>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            UploadedFile f = SelectFile(file.ID, false);

            if (f != null)
                file.LastUpdated = f.LastUpdated;

            // Assume that "file" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            files.Attach(new LinqUploadedFile(file), true);

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

        public override void DeleteFile(UploadedFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            LinqUploadedFile f = new LinqUploadedFile(file);

            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqUploadedFile> files = db.GetTable<LinqUploadedFile>();

            files.Attach(f);

            files.DeleteOnSubmit(f);

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

        public override UploadedFile SelectFile(Guid id)
        {
            return SelectFile(id, true);
        }

        public override UploadedFile SelectFile(Guid id, bool includeData)
        {
            UploadedFile file = null;

            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqUploadedFile> files = db.GetTable<LinqUploadedFile>();

            LinqUploadedFile row = files.SingleOrDefault<LinqUploadedFile>(
                f => f.ApplicationName == this.ApplicationName && f.ID == id);

            if (row == null)
                return null;

            file = new UploadedFile(row.ID)
            {
                ApplicationName = this.ApplicationName,
                ParentID = row.ParentID,
                FileName = row.FileName,
                Description = row.Description,
                DateCreated = row.DateCreated,
                LastUpdated = row.LastUpdated,
                ContentType = row.ContentType,
                ContentLength = row.ContentLength,
                Data = includeData ? row.Data : null
            };

            return file;
        }

        public override List<UploadedFile> GetFiles(Guid parentId)
        {
            List<UploadedFile> results = new List<UploadedFile>();

            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqUploadedFile> files = db.GetTable<LinqUploadedFile>();

            var query =
                from file in files
                where file.ApplicationName == this.ApplicationName
                where file.ParentID == parentId
                orderby file.FileName ascending
                select file;

            foreach (var row in query)
            {
                results.Add(new UploadedFile(row.ID)
                {
                    ApplicationName = this.ApplicationName,
                    ParentID = row.ParentID,
                    FileName = row.FileName,
                    Description = row.Description,
                    DateCreated = row.DateCreated,
                    LastUpdated = row.LastUpdated,
                    ContentType = row.ContentType,
                    ContentLength = row.ContentLength,
                    Data = row.Data
                });
                break;
            } 

            return results;
        }

        public override List<UploadedFile> GetFiles(Guid? id, Guid? parentId, string fileName,
            DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated,
            string contentType, bool includeData, int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;

            List<UploadedFile> results = new List<UploadedFile>();

#if GETFILES_W_SPROC
            int? _totalCount = null;

            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            var query = 
                db.GetFiles(this.ApplicationName, id, contentType, initialDateCreated, finalDateCreated, fileName,
                initialLastUpdated, finalLastUpdated, parentId, pageIndex * pageSize,
                (pageIndex * pageSize) + pageSize, ref _totalCount);

            foreach (GetFilesResult item in query)
                results.Add(SelectFile(item.ID, includeData));
#else
            UploadStorageDataContext db = new UploadStorageDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<PersistableUploadedFile> files = db.GetTable<PersistableUploadedFile>();

            var query =
                from file in files
                where file.ApplicationName == this.ApplicationName
                where file.ID == (id.HasValue && id.Value != Guid.Empty ? id.Value : file.ID)
                where file.ParentID == (parentId.HasValue && parentId.Value != Guid.Empty ? parentId.Value : file.ParentID)
                where file.FileName == (!string.IsNullOrEmpty(fileName) ? fileName : file.FileName)
                where file.DateCreated >= (initialDateCreated.HasValue && initialDateCreated != DateTime.MinValue ? initialDateCreated.Value : file.DateCreated)
                where file.DateCreated <= (finalDateCreated.HasValue && finalDateCreated != DateTime.MinValue ? finalDateCreated.Value : file.DateCreated)
                where file.LastUpdated >= (initialLastUpdated.HasValue && initialLastUpdated != DateTime.MinValue ? initialLastUpdated.Value : file.LastUpdated)
                where file.LastUpdated <= (initialLastUpdated.HasValue && initialLastUpdated != DateTime.MinValue ? initialLastUpdated.Value : file.LastUpdated)
                where file.ContentType == (!string.IsNullOrEmpty(contentType) ? contentType : file.ContentType)
                orderby file.FileName ascending
                select file;

            foreach (PersistableUploadedFile item in query.Skip(pageSize * pageIndex).Take(pageSize))
                results.Add(new UploadedFile(item));

            totalCount = query.Count();
#endif

            return results;
        }
    }
}
