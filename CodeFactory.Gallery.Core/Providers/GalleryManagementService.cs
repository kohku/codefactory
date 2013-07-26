using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Reflection;
using System.Text;
using CodeFactory.Web.PostOffice;
using CodeFactory.Web.Storage;
using System.Diagnostics;
using System.Net.Mail;

namespace CodeFactory.Gallery.Core.Providers
{
    /// <summary>
    /// The proxy class for communication between
    /// the business objects and the providers.
    /// </summary>
    public sealed class GalleryManagementService
    {
        private static object syncRoot = new Object();

        private static GalleryManagementServiceSettings _settings;
        private static GalleryProvider _defaultProvider;
        private static GalleryManagementProviderCollection _providers;

        static GalleryManagementService()
        {
            if (_defaultProvider == null)
            {
                lock (syncRoot)
                {
                    if (_defaultProvider == null)
                    {
                        LoadProviders();
                    }
                }
            }
        }

        public static GalleryManagementServiceSettings Settings
        {
            [DebuggerStepThrough]
            get { return _settings; }
        }

        /// <summary>
        /// Gets the default gallery management provider.
        /// </summary>
        public static GalleryProvider Provider
        {
            [DebuggerStepThrough]
            get { return _defaultProvider; }
        }

        /// <summary>
        /// Gets a collection of gallery management providers.
        /// </summary>
        public static GalleryManagementProviderCollection Providers
        {
            [DebuggerStepThrough]
            get { return _providers; }
        }

        /// <summary>
        /// Load providers specified in web.config.
        /// </summary>
        private static void LoadProviders()
        {
            try
            {
                _settings = (GalleryManagementServiceSettings)WebConfigurationManager.GetSection(
                    "galleryManagement/generalSettings");

                GalleryManagementSettings galleryManagement = (GalleryManagementSettings)
                    WebConfigurationManager.GetSection("galleryManagement/galleryManagementSettings");

                // Load registered providers and point _provider
                // to the default provider
                _providers = new GalleryManagementProviderCollection();
                ProvidersHelper.InstantiateProviders(
                    galleryManagement.Providers, _providers, typeof(GalleryProvider));
                _defaultProvider = _providers[galleryManagement.DefaultProvider];

                if (_defaultProvider == null)
                    throw new ProviderException("Unable to load default gallery management provider.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error loading cache from config.");
                throw ex;
            }
        }

        /// <summary>
        /// Returns a gallery based on the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Gallery SelectGallery(Guid id)
        {
            return _defaultProvider.SelectGallery(id);
        }

        /// <summary>
        /// Updates the gallery in the current management provider.
        /// </summary>
        /// <param name="gallery"></param>
        public static void UpdateGallery(Gallery gallery)
        {
            _defaultProvider.UpdateGallery(gallery);

            try
            {
                if (string.IsNullOrEmpty(_settings.Galleries.NotificationTemplate) ||
                    !VirtualPathUtility.IsAppRelative(_settings.Galleries.NotificationTemplate))
                    return;
                
                string fullyQualifiedPath = HttpContext.Current.Server.MapPath(
                    VirtualPathUtility.Combine(HttpContext.Current.Request.ApplicationPath,
                     _settings.Galleries.NotificationTemplate));

                if (!File.Exists(fullyQualifiedPath))
                    return;

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                using (Stream xsl = new FileStream(fullyQualifiedPath, FileMode.Open))
                {
                    try
                    {
                        HtmlMailMessage message = PostOfficeService.Create(gallery, xsl, parameters);

                        foreach (string user in gallery.Users)
                        {
                            MembershipUser recipient = Membership.GetUser(user);

                            if (recipient == null)
                                continue;

                            if (!string.IsNullOrEmpty(recipient.Email))
                                message.To.Add(new MailAddress(recipient.Email));
                        }

                        PostOfficeService.SendMessage(message);
                    }
                    finally
                    {
                        xsl.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        [DebuggerStepThrough]
        public static void UpdateGallery(Gallery gallery, bool updateComments)
        {
            _defaultProvider.UpdateGallery(gallery, updateComments);
        }

        [DebuggerStepThrough]
        public static void UpdateGallery(Gallery gallery, bool updateComments, bool updateUsers)
        {
            _defaultProvider.UpdateGallery(gallery, updateComments, updateUsers);
        }

        [DebuggerStepThrough]
        public static void UpdateGallery(Gallery gallery, bool updateComments, bool updateUsers, bool updateFiles)
        {
            _defaultProvider.UpdateGallery(gallery, updateComments, updateUsers, updateFiles);
        }

        /// <summary>
        /// Inserts a new gallery into the current gallery management provider.
        /// </summary>
        /// <param name="gallery"></param>
        [DebuggerStepThrough]
        public static void InsertGallery(Gallery gallery)
        {
            _defaultProvider.InsertGallery(gallery);
        }

        /// <summary>
        /// Deletes a gallery from the current gallery management provider.
        /// </summary>
        /// <param name="gallery"></param>
        [DebuggerStepThrough]
        public static void DeleteGallery(Gallery gallery)
        {
            _defaultProvider.DeleteGallery(gallery);
        }

        [DebuggerStepThrough]
        public static List<Guid> GetGalleryList()
        {
            return _defaultProvider.GetGalleryList();
        }

        [DebuggerStepThrough]
        public static List<Guid> GetGalleryList(Guid? id, string author, bool? visible, string lastUpdatedBy, string title, GalleryStatus? status,
            int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetGalleryList(id, author, visible, lastUpdatedBy, title, status, pageSize, pageIndex, out totalCount);
        }

        [DebuggerStepThrough]
        public static List<Guid> GetGalleryList(
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
            return _defaultProvider.GetGalleryList(id, author, initialDateCreated, finalDateCreated, description,
                isVisible, keywords, initialLastUpdated, finalLastUpdated, lastUpdatedBy, slug, title, status, 
                pageSize, pageIndex, out totalCount);
        }

        /// <summary>
        /// Returns all galleries.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static List<Gallery> GetGalleries()
        {
            return _defaultProvider.GetGalleries();
        }

        [DebuggerStepThrough]
        public static List<Gallery> GetGalleries(List<Guid> identifiers, bool includeComments)
        {
            return _defaultProvider.GetGalleries(identifiers, includeComments);
        }

        [DebuggerStepThrough]
        public static List<Gallery> GetGalleries(List<Guid> identifiers, bool includeComments, bool includeFiles)
        {
            return _defaultProvider.GetGalleries(identifiers, includeComments, includeFiles);
        }
    }
}
