#define SHORTER_FILE_NAMES
#define ROOT_FILES_FIX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Web;
using System.Security.Permissions;
using System.Xml;
using System.IO;
using System.Web.Hosting;
using System.Threading;

namespace CodeFactory.Web.Storage
{
    public class XmlUploadStorageProvider : UploadStorageProvider
    {
        private string _dataStorePath;
        private ReaderWriterLockSlim _protectionLock;

        public string DataStorePath
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _dataStorePath; }
        }

        public XmlUploadStorageProvider()
        {
            _protectionLock = new ReaderWriterLockSlim();
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (string.IsNullOrEmpty(config["dataStorePath"]))
                throw new ProviderException("dataStorePath config attribute is required.");

            this._dataStorePath = config["dataStorePath"];

            if (!VirtualPathUtility.IsAppRelative(this._dataStorePath))
                throw new ArgumentException("dataStorePath must be app-relative");

            string fullyQualifiedPath = VirtualPathUtility.Combine(
                VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath),
                this._dataStorePath);

            this._dataStorePath = HostingEnvironment.MapPath(fullyQualifiedPath);

            config.Remove("dataStorePath");

            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            if (!Directory.Exists(this._dataStorePath))
                Directory.CreateDirectory(this._dataStorePath);

            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, this._dataStorePath);
            permission.Demand();

            if (config.Count > 0)
                throw new ProviderException(string.Format("Unknown config attribute '{0}'", config.GetKey(0)));
        }

        public override UploadedFile SelectFile(Guid id)
        {
            return SelectFile(id, true);
        }

        public override UploadedFile SelectFile(Guid id, bool includeData)
        {
            UploadedFile file = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;

            Guid parentId = Guid.Empty;

            _protectionLock.EnterReadLock();

            try
            {
                using (XmlReader reader = XmlReader.Create(File.OpenRead(PrepareFileName(id, null)), settings))
                {
                    reader.Read();

                    file = new UploadedFile(id);

                    reader.ReadStartElement("UploadedFile");
                    file.ApplicationName = reader.ReadElementString("ApplicationName");
                    file.ParentID = new Guid(reader.ReadElementString("ParentID"));
                    file.FileName = reader.ReadElementString("FileName");
                    file.Description = reader.ReadElementString("Description");

                    reader.ReadStartElement("Data");
                    if (includeData)
                    {
                        int readBytes = 0;
                        byte[] buffer = new byte[1024];
                        MemoryStream stream = new MemoryStream();
                        BinaryWriter writer = new BinaryWriter(stream);

                        try
                        {
                            //reader.ReadStartElement("Data");
                            while ((readBytes = reader.ReadContentAsBinHex(buffer, 0, 1024)) > 0)
                            {
                                writer.Write(buffer);
                            }
                            file.Data = stream.ToArray();
                            //reader.ReadEndElement();
                        }
                        finally
                        {
                            stream.Close();
                        }
                    }
                    else
                    {
                        reader.Skip();
                    }
                    reader.ReadEndElement();

                    reader.ReadStartElement("DateCreated");
                    file.DateCreated = reader.ReadContentAsDateTime();
                    reader.ReadEndElement();
                    reader.ReadStartElement("LastUpdated");
                    DateTime lastUpdated;
                    if (reader.HasValue && DateTime.TryParse(reader.Value, out lastUpdated))
                    {
                        file.LastUpdated = reader.ReadContentAsDateTime();
                        reader.ReadEndElement();
                    }
                    file.ContentType = reader.ReadElementString("ContentType");
                    reader.ReadStartElement("ContentLength");
                    file.ContentLength = reader.ReadContentAsInt();
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                }
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            return file;
        }

        public override void UpdateFile(UploadedFile file)
        {
            InsertFile(file);
        }

        public override void InsertFile(UploadedFile file)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            _protectionLock.EnterWriteLock();

            try
            {

                using (XmlWriter writer = XmlWriter.Create(PrepareFileName(file.ID, file.ParentID), settings))
                {
                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("UploadedFile");
                    writer.WriteElementString("ApplicationName", this.ApplicationName);
                    writer.WriteElementString("ParentID", file.ParentID.ToString());
                    writer.WriteElementString("FileName", file.FileName);
                    writer.WriteElementString("Description", file.Description);
                    writer.WriteStartElement("Data");
                    if (file.Data != null)
                        writer.WriteBinHex(file.Data, 0, file.Data.Length);
                    writer.WriteEndElement(); // closes Data
                    writer.WriteStartElement("DateCreated");
                    writer.WriteValue(DateTime.Now);
                    writer.WriteEndElement(); // closes DateCreated
                    writer.WriteStartElement("LastUpdated");
                    if (file.LastUpdated.HasValue)
                        writer.WriteValue(file.LastUpdated);
                    writer.WriteEndElement(); // closes LastUpdated
                    writer.WriteElementString("ContentType", file.ContentType);
                    writer.WriteStartElement("ContentLength");
                    writer.WriteValue(file.ContentLength);
                    writer.WriteEndElement(); // closes ContentLength
                    writer.WriteEndElement(); // closes UploadedFile
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        private string PrepareFileName(Guid id, Guid? parentId)
        {

#if ROOT_FILES_FIX
            if (!parentId.HasValue)
#else
            // cambio hecho porque no marca un error cuando parentId.Value == Guid.Empty. 
            // El código es para el select recibe null en el parentId porque lo recupera del archivo.
            if (!parentId.HasValue || parentId.Value == Guid.Empty)
#endif
            {
                string[] files = Directory.GetFiles(this.DataStorePath, GenerateSearchPattern(id, parentId),
                    SearchOption.AllDirectories);

                if (files.Length != 1)
                    throw new ProviderException(string.Format(
                        "Directory.GetFiles found zero more than one results with the pattern '{0}'.",
                        GenerateSearchPattern(id, parentId)));

                return files[0];
            }

            string parentDirectory = Path.Combine(this.DataStorePath, parentId.Value.ToString());

            if (!Directory.Exists(parentDirectory))
                Directory.CreateDirectory(parentDirectory);

            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, parentDirectory);
            permission.Demand();

            return Path.Combine(parentDirectory, string.Format("{0}.xml", id));
        }

        private string GenerateSearchPattern(Guid? id, Guid? parentId)
        {
            return string.Format("{0}{1}.xml",
                parentId.HasValue && parentId.Value != Guid.Empty ?
                parentId.Value.ToString() + Path.DirectorySeparatorChar : string.Empty,
                id.HasValue && id.Value != Guid.Empty ? id.Value.ToString() : "*");
        }

        public override void DeleteFile(UploadedFile file)
        {
            _protectionLock.EnterWriteLock();

            try
            {
                File.Delete(PrepareFileName(file.ID, file.ParentID));

                string parentDirectory = Path.Combine(this.DataStorePath, file.ParentID.ToString());

                if (Directory.Exists(parentDirectory) && Directory.GetFiles(parentDirectory).Length == 0)
                    Directory.Delete(parentDirectory);
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override List<UploadedFile> GetFiles(Guid parentId)
        {
            List<UploadedFile> uploadedFiles = new List<UploadedFile>();

            string parentDirectory = Path.Combine(this.DataStorePath, parentId.ToString());

            if (Directory.Exists(parentDirectory))
            {
                string[] files = Directory.GetFiles(this.DataStorePath, GenerateSearchPattern(null, parentId), SearchOption.AllDirectories);

                foreach (string item in files)
                {
                    try
                    {
                        string filename = Path.GetFileNameWithoutExtension(item);

                        Guid id = new Guid(filename.Replace(
                            string.Format("{0}.{1}.", this.ApplicationName, parentId), string.Empty));

                        uploadedFiles.Add(SelectFile(id));
                    }
                    catch
                    {
                        continue;
                    }
                }

                uploadedFiles.Sort(delegate(UploadedFile x, UploadedFile y)
                {
                    return x.FileName.CompareTo(y.FileName);
                });
            }

            return uploadedFiles;
        }

        public override List<UploadedFile> GetFiles(Guid? id, Guid? parentId, string fileName,
            DateTime? initialDateCreated, DateTime? finalDateCreated,
            DateTime? initialLastUpdated, DateTime? finalLastUpdated,
            string contentType, bool includeData,
            int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;

            List<UploadedFile> uploadedFiles = new List<UploadedFile>();

            string parentDirectory = Path.Combine(this.DataStorePath, parentId.ToString());

            if (Directory.Exists(parentDirectory))
            {
                string[] files = Directory.GetFiles(this.DataStorePath, GenerateSearchPattern(id, parentId), SearchOption.AllDirectories);

                foreach (string item in files)
                {
                    try
                    {
                        string filename = Path.GetFileNameWithoutExtension(item);

                        Guid _id = new Guid(filename.Replace(
                            string.Format("{0}.{1}.", this.ApplicationName, parentId), string.Empty));

                        uploadedFiles.Add(SelectFile(_id, includeData));
                    }
                    catch
                    {
                        continue;
                    }
                }

                uploadedFiles.FindAll(delegate(UploadedFile match)
                {
                    if (!string.IsNullOrEmpty(fileName) && match.FileName != fileName)
                        return false;
                    if (initialDateCreated.HasValue && finalDateCreated.HasValue &&
                        (match.DateCreated < initialDateCreated && match.DateCreated > finalDateCreated))
                        return false;
                    if (initialLastUpdated.HasValue && finalLastUpdated.HasValue &&
                        match.LastUpdated.HasValue &&
                        (match.LastUpdated < initialDateCreated && match.LastUpdated > finalDateCreated))
                        return false;
                    if (!string.IsNullOrEmpty(contentType) && match.ContentType != contentType)
                        return false;
                    return true;
                });

                totalCount = uploadedFiles.Count;

                uploadedFiles.Sort(delegate(UploadedFile x, UploadedFile y)
                {
                    return x.FileName.CompareTo(y.FileName);
                });
            }

            return uploadedFiles.GetRange(
                pageIndex * pageSize,
                pageSize + (pageIndex * pageSize) > uploadedFiles.Count ?
                uploadedFiles.Count - (pageIndex * pageSize) : pageSize);
        }
    }
}
