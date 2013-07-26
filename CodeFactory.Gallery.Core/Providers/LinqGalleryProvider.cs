using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Data.Linq;
using CodeFactory.Web.Storage;
using System.Diagnostics;
using System.Data.Common;
using CodeFactory.DataAccess.Transactions;

namespace CodeFactory.Gallery.Core.Providers
{
    public class LinqGalleryProvider : GalleryProvider
    {
        private string _connectionStringName;

        public LinqGalleryProvider()
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

        public override List<Guid> GetGalleryList(
                    Guid? id,
                    string author,
                    DateTime? initialDateCreated,
                    DateTime? finalDateCreated,
                    string description,
                    bool? isVisible,
                    string keywords,
                    DateTime? initialLastUpdated,
                    DateTime? finalLastUpdated,
                    string lastUpdatedBy,
                    string slug,
                    string title,
                    GalleryStatus? status,
                    int pageSize,
                    int pageIndex,
                    out int totalCount)
        {
            totalCount = 0;

            List<Guid> list = new List<Guid>();

            int? _totalCount = null;

            GalleryDataContext db = new GalleryDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            var query =
                db.GetGalleries(this.ApplicationName, id, author, initialDateCreated, finalDateCreated, description, isVisible, keywords,
                initialLastUpdated, finalLastUpdated, lastUpdatedBy, slug, title, status.HasValue ? status.ToString() : null,
                pageIndex * pageSize, (pageIndex * pageSize) + pageSize, ref _totalCount);

            foreach (GetGalleriesResult item in query)
                list.Add(item.ID);

            if (_totalCount.HasValue)
                totalCount = _totalCount.Value;

            return list;
        }
        
        public override Gallery SelectGallery(Guid id, bool includeComments, bool includeFiles)
        {
            Gallery gallery = null;

            GalleryDataContext db = new GalleryDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqGallery> galleries = db.GetTable<LinqGallery>();

            LinqGallery row = galleries.SingleOrDefault<LinqGallery>(
                g => g.ApplicationName == this.ApplicationName && g.ID == id);

            if (row == null)
                return null;

            gallery = new Gallery(row.ID)
            {
                ApplicationName = this.ApplicationName,
                Author = row.Author,
                Content = row.Content,
                DateCreated = row.DateCreated,
                Description = row.Description,
                IsVisible = row.IsVisible,
                Keywords = row.Keywords,
                LastUpdated = row.LastUpdated,
                LastUpdatedBy = row.LastUpdatedBy,
                Slug = row.Slug,
                Status = row.Status,
                Title = row.Title
            };

            if (gallery == null)
                return null;

            if (includeComments)
                GetComments(gallery);

            if (includeFiles)
            {
                foreach (UploadedFile file in UploadStorageService.GetFiles(gallery.ID))
                {
                    gallery.AddFile(file);
                }
            }

            GetUsers(gallery);

            return gallery;
        }

        public override void UpdateGallery(Gallery gallery, bool updateComments, bool updateUsers, bool updateFiles)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            // TODO: Verificar el contexto de la transacción. Si se puede expandir a otros métodos, como es el caso de esta función.
            GalleryDataContext db = new GalleryDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqGallery> galleries = db.GetTable<LinqGallery>();

            // BusinessBase inherited clases will have a downside effect with a ChangeConflictException 
            // as it has changed LastUpdated row version in the call stack.
            Gallery g = SelectGallery(gallery.ID, false, false);

            if (g != null)
                gallery.LastUpdated = g.LastUpdated;

            // Assume that "gallery" has been sent by client.
            // Attach with "true" to the change tracker to consider the entity modified
            // and it can be checked for optimistic concurrency because
            // it has a column that is marked with "RowVersion" attribute
            galleries.Attach(new LinqGallery(gallery), true);

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

            if (updateComments)
                UpdateComments(gallery);
            
            if (updateUsers)
                UpdateUsers(gallery);

            if (updateFiles)
                foreach (UploadedFile file in gallery.Files)
                    file.Save();
        }

        public override void InsertGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            GalleryDataContext db = new GalleryDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqGallery> galleries = db.GetTable<LinqGallery>();

            gallery.ApplicationName = this.ApplicationName;

            galleries.InsertOnSubmit(new LinqGallery(gallery));

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

            UpdateComments(gallery);

            UpdateUsers(gallery);

            foreach (UploadedFile file in gallery.Files)
                file.Save();
        }

        public override void DeleteGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            LinqGallery g = new LinqGallery(gallery);

            GalleryDataContext db = new GalleryDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<LinqGallery> galleries = db.GetTable<LinqGallery>();

            galleries.Attach(g);

            galleries.DeleteOnSubmit(g);

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

        public override void GetComments(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            GalleryDataContext db = new GalleryDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<Comment> comments = db.GetTable<Comment>();

            var query =
                from comment in comments
                where comment.ParentID == gallery.ID
                orderby comment.DateCreated descending
                select comment;

            foreach (var row in query)
            {
                gallery.AddComment(new Comment(row.ID)
                {
                    Author = row.Author,
                    Content = row.Content,
                    DateCreated = row.DateCreated,
                    IPAddress = row.IPAddress,
                    Title = row.Title
                });
            }
        }

        public override void UpdateComments(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            DeleteComments(gallery);

            GalleryDataContext db = new GalleryDataContext(
                ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<Comment> comments = db.GetTable<Comment>();

            comments.InsertAllOnSubmit<Comment>(gallery.Comments);

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

        public override void DeleteComments(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            GalleryDataContext db = new GalleryDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<Comment> comments = db.GetTable<Comment>();

            Gallery g = SelectGallery(gallery.ID, true);

            if (g == null)
                return;

            comments.AttachAll<Comment>(g.Comments);

            comments.DeleteAllOnSubmit<Comment>(g.Comments);

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

        public override void GetUsers(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            GalleryDataContext db = new GalleryDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<GalleryUser> users = db.GetTable<GalleryUser>();

            var query =
                from user in users
                where user.GalleryID == gallery.ID
                orderby user.UserID ascending
                select user;

            foreach (var row in query)
            {
                gallery.AddUser(row.UserID);
            }
        }

        public override void DeleteUsers(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            GalleryDataContext db = new GalleryDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<GalleryUser> users = db.GetTable<GalleryUser>();

            Gallery g = SelectGallery(gallery.ID, true);

            if (g == null)
                return;

            List<GalleryUser> galleryUsers = CreateGalleryUserCollection(g);

            users.AttachAll<GalleryUser>(galleryUsers);

            users.DeleteAllOnSubmit<GalleryUser>(galleryUsers);

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

        public override void UpdateUsers(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            DeleteUsers(gallery);

            GalleryDataContext db = new GalleryDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

            Table<GalleryUser> users = db.GetTable<GalleryUser>();

            users.InsertAllOnSubmit<GalleryUser>(CreateGalleryUserCollection(gallery));

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

        private List<GalleryUser> CreateGalleryUserCollection(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("gallery");

            List<GalleryUser> users = new List<GalleryUser>();

            foreach (string userId in gallery.Users)
            {
                users.Add(new GalleryUser()
                {
                    GalleryID = gallery.ID,
                    UserID = userId
                });
            }

            return users;
        }
    }
}
