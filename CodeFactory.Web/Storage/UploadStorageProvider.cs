using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text;
using System.Web.Hosting;
using System.Diagnostics;

namespace CodeFactory.Web.Storage
{
    public abstract class UploadStorageProvider : ProviderBase
    {
        private string _applicationName;
        private int _maxFileSize;
        private List<string> _contentTypeAllowed;
        private List<string> _compressContentType;

        protected UploadStorageProvider()
        {
            _contentTypeAllowed = new List<string>();
            _compressContentType = new List<string>();
        }

        public virtual string ApplicationName
        {
            get { return _applicationName; }
        }

        /// <summary>
        /// Maximum length allowed for gallery files.
        /// </summary>
        public virtual int MaxFileSize
        {
            get { return _maxFileSize; }
        }

        /// <summary>
        /// List of content type allowed.
        /// </summary>
        public virtual List<string> ContentTypeAllowed
        {
            get { return _contentTypeAllowed; }
        }

        /// <summary>
        /// List of content type that can be compressed.
        /// </summary>
        public virtual List<string> CompressContentType
        {
            get { return _compressContentType; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (!string.IsNullOrEmpty(config["applicationName"]))
            {
                this._applicationName = config["applicationName"];
                config.Remove("applicationName");
            }
            else
                this._applicationName = HostingEnvironment.ApplicationVirtualPath;

            this._maxFileSize = Convert.ToInt32(config["maxFileSize"]);
            config.Remove("maxFileSize");

            if (!string.IsNullOrEmpty(config["saveContentType"]))
            {
                string[] contentTypeAllowed = config["saveContentType"].Split(
                    new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string contentType in contentTypeAllowed)
                    this.ContentTypeAllowed.Add(contentType.Replace("\r", string.Empty).Replace(
                        "\n", string.Empty).Replace("\t", string.Empty).Trim());

                config.Remove("saveContentType");
            }

            if (!string.IsNullOrEmpty(config["compressContentType"]))
            {
                string[] compressContentType = config["compressContentType"].Split(
                    new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string contentType in compressContentType)
                    this.CompressContentType.Add(contentType.Replace("\r", string.Empty).Replace(
                        "\n", string.Empty).Replace("\t", string.Empty).Trim());

                config.Remove("compressContentType");
            }
        }

        public abstract UploadedFile SelectFile(Guid id);
        public abstract UploadedFile SelectFile(Guid id, bool includeData);

        public abstract void UpdateFile(UploadedFile file);
        public abstract void InsertFile(UploadedFile file);
        public abstract void DeleteFile(UploadedFile file);

        public virtual UploadedFile CreateFile4Storage(string contentType)
        {
            return CreateFile4Storage(Guid.NewGuid(), contentType);
        }

        public virtual UploadedFile CreateFile4Storage(Guid id, string contentType)
        {
            UploadedFile file = null;

            try
            {
                UploadedFile.MediaType media = (UploadedFile.MediaType)Enum.Parse(typeof(UploadedFile.MediaType),
                            contentType.Substring(0, contentType.IndexOf("/")).ToLower());

                string coincidenceAllowed = this.ContentTypeAllowed.Find(delegate(string match)
                {
                    return match.Equals(contentType, StringComparison.InvariantCultureIgnoreCase);
                });

                if (string.IsNullOrEmpty(coincidenceAllowed))
                    throw new ProviderException(string.Format("The content type {0} is not allowed.", contentType));

                string coincidenceCompressed = this.CompressContentType.Find(delegate(string match)
                {
                    return match.Equals(contentType, StringComparison.InvariantCultureIgnoreCase);
                });

                if (!string.IsNullOrEmpty(coincidenceCompressed))
                    file = new DeflateUploadedFile(id);
                else
                    file = new UploadedFile(id);

                file.ContentType = contentType;
            }
            catch (ProviderException ex)
            {
                Trace.WriteLine(string.Format(
                    "DefaultUploadStorageProvider.CreateFile4Storage error. Description: {0}", ex.Message));

                throw ex;
            }

            return file;
        }

        public abstract List<UploadedFile> GetFiles(Guid parentId);
        public abstract List<UploadedFile> GetFiles(Guid? id, Guid? parentId, string fileName,
            DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated, 
            string contentType, bool includeData, int pageSize, int pageIndex, out int totalCount);
    }
}
