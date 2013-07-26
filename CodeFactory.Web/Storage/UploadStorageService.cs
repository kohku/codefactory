using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace CodeFactory.Web.Storage
{
    public sealed class UploadStorageService
    {
        private static object syncRoot = new Object();

        private static UploadStorageServiceSettings _settings;

        private static UploadStorageProvider _defaultProvider;
        private static UploadStorageProviderCollection _uploadStorageProviders;


        static UploadStorageService()
        {
            LoadProviders();
        }

        public static UploadStorageProvider Provider
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _defaultProvider; }
        }

        public static UploadStorageServiceSettings Settings
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _settings; }
        }

        public static UploadStorageProviderCollection Providers
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _uploadStorageProviders; }
        }

        /// <summary>
        /// Load providers specified in web.config.
        /// </summary>
        private static void LoadProviders()
        {
            if (_defaultProvider == null)
            {
                lock (syncRoot)
                {
                    if (_defaultProvider == null)
                    {
                        _settings = (UploadStorageServiceSettings)
                            WebConfigurationManager.GetSection("uploadStorageSettings");

                        _uploadStorageProviders = new UploadStorageProviderCollection();

                        ProvidersHelper.InstantiateProviders(
                            _settings.Providers, _uploadStorageProviders, typeof(UploadStorageProvider));
                        _defaultProvider = _uploadStorageProviders[_settings.DefaultProvider];
                    }
                }
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<UploadedFile> GetFiles(Guid groupId)
        {
            return _defaultProvider.GetFiles(groupId);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static UploadedFile SelectFile(Guid id)
        {
            return _defaultProvider.SelectFile(id);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void UpdateFile(UploadedFile file)
        {
            _defaultProvider.UpdateFile(file);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void InsertFile(UploadedFile file)
        {
            _defaultProvider.InsertFile(file);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void DeleteFile(UploadedFile file)
        {
            _defaultProvider.DeleteFile(file);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static UploadedFile CreateFile4Storage(string contentType)
        {
            return _defaultProvider.CreateFile4Storage(contentType);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static List<UploadedFile> GetFiles(Guid? id, Guid? parentId, string fileName,
            DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated,
            string contentType, bool includeData, int pageSize, int pageIndex, out int totalCount)
        {
            return _defaultProvider.GetFiles(id, parentId, fileName, initialDateCreated, finalDateCreated,
                initialLastUpdated, finalLastUpdated, contentType,
                includeData, pageSize, pageIndex, out totalCount);
        }

        public static void Clean(System.Web.HttpContext httpContext)
        {
        }
    }
}
