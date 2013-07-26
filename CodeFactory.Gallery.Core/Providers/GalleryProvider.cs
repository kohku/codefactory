using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text;
using System.Web.Hosting;

namespace CodeFactory.Gallery.Core.Providers
{
    public abstract class GalleryProvider : ProviderBase
    {
        private string _applicationName;

        protected GalleryProvider()
        {
        }

        public string ApplicationName
        {
            get { return _applicationName; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (string.IsNullOrEmpty(config["applicationName"]))
                throw new ProviderException("applicationName attribute must be specified.");

            if (!string.IsNullOrEmpty(config["applicationName"]))
            {
                this._applicationName = config["applicationName"];
                config.Remove("applicationName");
            }
            else
                this._applicationName = HostingEnvironment.ApplicationVirtualPath;
        }

        /// <summary>
        /// Selects a gallery with comments and files id's
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Gallery SelectGallery(Guid id)
        {
            return SelectGallery(id, true, true);
        }

        /// <summary>
        /// Selects a gallery specifying if including comments, including files.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeComments"></param>
        /// <returns></returns>
        public virtual Gallery SelectGallery(Guid id, bool includeComments)
        {
            return SelectGallery(id, includeComments, false);
        }

        public abstract Gallery SelectGallery(Guid id, bool includeComments, bool includeFiles);

        public virtual void UpdateGallery(Gallery gallery)
        {
            UpdateGallery(gallery, true, true, true);
        }

        public virtual void UpdateGallery(Gallery gallery, bool updateComments)
        {
            UpdateGallery(gallery, updateComments, false, false);
        }

        public virtual void UpdateGallery(Gallery gallery, bool updateComments, bool updateUsers)
        {
            UpdateGallery(gallery, updateComments, updateUsers, false);
        }

        public abstract void UpdateGallery(Gallery gallery, bool updateComments, bool updateUsers, bool updateFiles);

        public abstract void InsertGallery(Gallery gallery);

        public abstract void DeleteGallery(Gallery gallery);

        public List<Guid> GetGalleryList()
        {
            int totalCount;

            return GetGalleryList(null, null, null, null, null, null, int.MaxValue, 0, out totalCount);
        }

        public List<Guid> GetGalleryList(
            Guid? id,
            string author,
            bool? visible,
            string lastUpdatedBy,
            string title,
            GalleryStatus? status,
            int pageSize,
            int pageIndex,
            out int totalCount)
        {
            return GetGalleryList(id, author, null, null, null, visible, null, null, null, lastUpdatedBy,
                null, title, status, pageSize, pageIndex, out totalCount);
        }
        
        public abstract List<Guid> GetGalleryList(
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
            out int totalCount);

        public virtual List<Gallery> GetGalleries()
        {
            return GetGalleries(GetGalleryList(), true, true);
        }

        public virtual List<Gallery> GetGalleries(List<Guid> identifiers, bool includeComments)
        {
            return GetGalleries(identifiers, true, true);
        }

        public virtual List<Gallery> GetGalleries(List<Guid> identifiers, bool includeComments, bool includeFiles)
        {
            List<Gallery> galleries = new List<Gallery>();

            foreach (Guid id in identifiers)
                galleries.Add(SelectGallery(id, includeComments, includeFiles));

            return galleries;
        }

        public abstract void GetComments(Gallery gallery);
        public abstract void UpdateComments(Gallery gallery);
        public abstract void DeleteComments(Gallery gallery);

        public abstract void GetUsers(Gallery gallery);
        public abstract void DeleteUsers(Gallery gallery);
        public abstract void UpdateUsers(Gallery gallery);
    }
}
