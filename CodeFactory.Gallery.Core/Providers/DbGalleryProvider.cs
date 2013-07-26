using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using CodeFactory.DataAccess;
using CodeFactory.DataAccess.Transactions;
using CodeFactory.Web.Storage;
using System.Configuration.Provider;
using System.Web.Hosting;

namespace CodeFactory.Gallery.Core.Providers
{
    public class DbGalleryProvider : GalleryProvider
    {
        public DbGalleryProvider()
        {
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);           

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

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                IDataCommand cmd = dataSource.GetCommand("GetGalleries");

                cmd.Parameters["applicationName"].Value = this.ApplicationName;

                if (id.HasValue && id.Value != Guid.Empty)
                    cmd.Parameters["id"].Value = id;
                if (!string.IsNullOrEmpty(author))
                    cmd.Parameters["author"].Value = author;
                if (initialDateCreated.HasValue && initialDateCreated.Value != DateTime.MinValue &&
                    finalDateCreated.HasValue && finalDateCreated.Value != DateTime.MinValue)
                {
                    cmd.Parameters["initialDateCreated"].Value = initialDateCreated;
                    cmd.Parameters["finalDateCreated"].Value = finalDateCreated;
                }
                if (!string.IsNullOrEmpty(description))
                    cmd.Parameters["description"].Value = description;
                if (!(HttpContext.Current != null && HttpContext.Current.User.IsInRole("Administrator")) &&
                    isVisible.HasValue)
                    cmd.Parameters["isVisible"].Value = isVisible;
                if (!string.IsNullOrEmpty(keywords))
                    cmd.Parameters["keywords"].Value = keywords;
                if (initialLastUpdated.HasValue && initialLastUpdated.Value != DateTime.MinValue &&
                    finalLastUpdated.HasValue && finalLastUpdated.Value != DateTime.MinValue)
                {
                    cmd.Parameters["initialLastUpdated"].Value = initialLastUpdated;
                    cmd.Parameters["finalLastUpdated"].Value = finalLastUpdated;
                }
                if (!string.IsNullOrEmpty(lastUpdatedBy))
                    cmd.Parameters["lastUpdatedBy"].Value = lastUpdatedBy;
                if (!string.IsNullOrEmpty(slug))
                    cmd.Parameters["slug"].Value = slug;
                if (!string.IsNullOrEmpty(title))
                    cmd.Parameters["title"].Value = title;
                if (status.HasValue)
                    cmd.Parameters["status"].Value = status.ToString();
                cmd.Parameters["firstIndex"].Value = pageIndex * pageSize;
                cmd.Parameters["lastIndex"].Value = (pageIndex * pageSize) + pageSize;

                IDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        list.Add(new Guid(reader["id"].ToString()));
                    }
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                totalCount = Convert.ToInt32(cmd.Parameters["totalCount"].Value);
            }

            return list;
        }
        
        public override Gallery SelectGallery(Guid id, bool includeComments, bool includeFiles)
        {
            Gallery gallery = null;

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Supported))
            {
                IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                IDataCommand cmd = dataSource.GetCommand("SelectGallery");

                cmd.Parameters["applicationName"].Value = this.ApplicationName;
                cmd.Parameters["id"].Value = id;

                IDataReader reader = cmd.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        gallery = new Gallery(id);

                        FillGalleryCmd(reader, gallery);
                    }
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }

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

        private void FillGalleryCmd(IDataReader reader, Gallery gallery)
        {
            gallery.ApplicationName = this.ApplicationName;
            gallery.Author = reader["author"].ToString();
            if (reader["content"] != DBNull.Value)
                gallery.Content = reader["content"].ToString();
            gallery.DateCreated = Convert.ToDateTime(reader["dateCreated"]);
            if (reader["description"] != DBNull.Value)
                gallery.Description = reader["description"].ToString();
            gallery.IsVisible = Convert.ToBoolean(reader["isVisible"]);
            if (reader["keywords"] != DBNull.Value)
                gallery.Keywords = reader["keywords"].ToString();
            if (reader["lastUpdated"] != DBNull.Value)
                gallery.LastUpdated = Convert.ToDateTime(reader["lastUpdated"]);
            if (reader["lastUpdatedBy"] != DBNull.Value)
                gallery.LastUpdatedBy = reader["lastUpdatedBy"].ToString();
            if (reader["slug"] != DBNull.Value)
                gallery.Slug = reader["slug"].ToString();
            gallery.Title = reader["title"].ToString();
            gallery.Status = (GalleryStatus)Enum.Parse(typeof(GalleryStatus), reader["status"].ToString());
        }

        public override void UpdateGallery(Gallery gallery, bool updateComments, bool updateUsers, bool updateFiles)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("UpdateGallery");

                    ExecuteGalleryCmd(cmd, gallery);

                    if (updateComments)
                        UpdateComments(gallery);

                    if (updateUsers)
                        UpdateUsers(gallery);

                    if (updateFiles)
                        foreach (UploadedFile file in gallery.Files)
                            file.Save();

                    context.VoteCommit();
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        public override void InsertGallery(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("InsertGallery");

                    ExecuteGalleryCmd(cmd, gallery);

                    context.VoteCommit();

                    UpdateComments(gallery);

                    foreach (UploadedFile file in gallery.Files)
                        file.Save();

                    UpdateUsers(gallery);
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        private void ExecuteGalleryCmd(IDataCommand cmd, Gallery gallery)
        {
            cmd.Parameters["applicationName"].Value = this.ApplicationName;
            cmd.Parameters["id"].Value = gallery.ID;
            cmd.Parameters["author"].Value = gallery.Author;
            if (!string.IsNullOrEmpty(gallery.Content))
                cmd.Parameters["content"].Value = gallery.Content;
            cmd.Parameters["dateCreated"].Value = gallery.DateCreated;
            if (!string.IsNullOrEmpty(gallery.Description))
                cmd.Parameters["description"].Value = gallery.Description;
            cmd.Parameters["isVisible"].Value = gallery.IsVisible;
            if (!string.IsNullOrEmpty(gallery.Keywords))
                cmd.Parameters["keywords"].Value = gallery.Keywords;
            if (gallery.LastUpdated.HasValue && gallery.LastUpdated != DateTime.MinValue)
                cmd.Parameters["lastUpdated"].Value = gallery.LastUpdated;
            if (!string.IsNullOrEmpty(gallery.LastUpdatedBy))
                cmd.Parameters["lastUpdatedBy"].Value = gallery.LastUpdatedBy;
            if (!string.IsNullOrEmpty(gallery.Slug))
                cmd.Parameters["slug"].Value = gallery.Slug;
            cmd.Parameters["title"].Value = gallery.Title;
            cmd.Parameters["status"].Value = gallery.Status.ToString();

            int affectedRows = cmd.ExecuteNonQuery();
        }

        public override void DeleteGallery(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Supported))
            {
                try
                {
                    DeleteComments(gallery);

                    DeleteUsers(gallery);

                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("DeleteGallery");

                    cmd.Parameters["applicationName"].Value = this.ApplicationName;
                    cmd.Parameters["id"].Value = gallery.ID;

                    int affectedRows = cmd.ExecuteNonQuery();

                    context.VoteCommit();
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        public override void DeleteComments(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("DeleteComments");

                    cmd.Parameters["galleryId"].Value = gallery.ID;

                    int affectedRows = cmd.ExecuteNonQuery();

                    context.VoteCommit();
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        public override void UpdateComments(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    DeleteComments(gallery);

                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("InsertComment");

                    foreach (Comment comment in gallery.Comments)
                    {
                        cmd.Parameters["galleryId"].Value = gallery.ID;
                        cmd.Parameters["id"].Value = comment.ID;
                        cmd.Parameters["author"].Value = comment.Author;
                        cmd.Parameters["content"].Value = comment.Content;
                        cmd.Parameters["dateCreated"].Value = comment.DateCreated;
                        cmd.Parameters["title"].Value = comment.Title;
                        cmd.Parameters["ipAddress"].Value = comment.IPAddress;

                        int affectedRows = cmd.ExecuteNonQuery();
                    }

                    context.VoteCommit();
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        public override void GetComments(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                IDataCommand cmd = dataSource.GetCommand("GetComments");

                cmd.Parameters["galleryId"].Value = gallery.ID;

                IDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        Comment item = new Comment(new Guid(reader["id"].ToString()));

                        item.Author = reader["author"].ToString();
                        item.Content = reader["content"].ToString();
                        item.DateCreated = Convert.ToDateTime(reader["dateCreated"]);
                        item.Title = reader["title"].ToString();
                        item.IPAddress = reader["ipAddress"].ToString();

                        gallery.AddComment(item);
                    }
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        public override void DeleteUsers(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("DeleteUsers");

                    cmd.Parameters["galleryId"].Value = gallery.ID;

                    int affectedRows = cmd.ExecuteNonQuery();

                    context.VoteCommit();
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        public override void UpdateUsers(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    DeleteUsers(gallery);

                    IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                    IDataCommand cmd = dataSource.GetCommand("UpdateUsers");

                    foreach (string userId in gallery.Users)
                    {
                        cmd.Parameters["galleryId"].Value = gallery.ID;
                        cmd.Parameters["userId"].Value = userId;

                        int affectedRows = cmd.ExecuteNonQuery();
                    }

                    context.VoteCommit();
                }
                catch (Exception ex)
                {
                    context.VoteRollback();
                    throw ex;
                }
            }
        }

        public override void GetUsers(Gallery gallery)
        {
            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Supported))
            {
                IDataSource dataSource = DataSourceFactory.GetDataSource("DbGalleryProvider");

                IDataCommand cmd = dataSource.GetCommand("GetUsers");

                cmd.Parameters["galleryId"].Value = gallery.ID;

                IDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                        gallery.AddUser(reader["userId"].ToString());
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }
    }
}
