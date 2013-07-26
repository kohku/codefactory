using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using CodeFactory.Utilities;
using System.Globalization;
using System.Collections.Specialized;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.IO;
using System.Threading;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    public class FileBasedPersonalizationProvider : System.Web.UI.WebControls.WebParts.PersonalizationProvider
    {
        private string _applicationName;
        private string _directoryName = "~/App_Data/";

        public override string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(this._applicationName))
                    this._applicationName = SecUtility.GetDefaultAppName();

                return this._applicationName;
            }
            set
            {
                if ((value != null) && (value.Length > 0x100))
                {
                    object[] args = new object[] { 0x100.ToString(CultureInfo.CurrentCulture) };
                    throw new ProviderException(ResourceStringLoader.GetResourceString("PersonalizationProvider_ApplicationNameExceedMaxLength", args));
                }

                this._applicationName = value;
            }
        }

        public override void Initialize(string name, NameValueCollection configSettings)
        {
            if (configSettings == null)
                throw new ArgumentNullException("configSettings");

            if (string.IsNullOrEmpty(name))
                name = "PersonalizationProvider";

            if (string.IsNullOrEmpty(configSettings["description"]))
            {
                configSettings.Remove("description");
                configSettings.Add("description", ResourceStringLoader.GetResourceString(
                    "PersonalizationProvider_Description"));
            }

            base.Initialize(name, configSettings);

            this._applicationName = configSettings["applicationName"];

            if (this._applicationName != null)
            {
                configSettings.Remove("applicationName");
                if (this._applicationName.Length > 0x100)
                {
                    object[] args = new object[] { 0x100.ToString(CultureInfo.CurrentCulture) };
                    throw new ProviderException(ResourceStringLoader.GetResourceString(
                        "PersonalizationProvider_ApplicationNameExceedMaxLength", args));
                }
            }

            if (!string.IsNullOrEmpty(configSettings["directoryName"]))
                this._directoryName = configSettings["directoryName"];

            configSettings.Remove("directoryName");

            if (!VirtualPathUtility.IsAppRelative(this._directoryName))
                throw new ProviderException(ResourceStringLoader.GetResourceString(
                    "PersonalizationProvider_DirectoryNameNotRelative"));

            this._directoryName = VirtualPathUtility.AppendTrailingSlash(this._directoryName);

            if (configSettings.Count > 0)
            {
                string key = configSettings.GetKey(0);
                throw new ProviderException(ResourceStringLoader.GetResourceString(
                    "PersonalizationProvider_UnknownProp", new object[] { key, name }));
            }
        }

        protected override void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName, ref byte[] sharedDataBlob, ref byte[] userDataBlob)
        {
            string fileName = null;

            if (HttpContext.Current.Request.QueryString.Count > 0)
                path = path + "?" + HttpContext.Current.Request.QueryString.ToString();

            sharedDataBlob = null;
            userDataBlob = null;

            fileName = HttpContext.Current.Server.MapPath(ConstructAllUsersDataFileName(path));

            try
            {
                // lock on the fileName in case two clients try accessing the
                // same file concurrently - note we lock on the interned fileName
                // string, which will always return the same objref for identical 
                // strings
                //
                if (Monitor.TryEnter(string.Intern(fileName), 5000))
                {
                    if (File.Exists(fileName))
                        sharedDataBlob = File.ReadAllBytes(fileName);
                }
                else
                {
                    throw new ApplicationException("Monitor timed out");
                }
            }
            finally
            {
                Monitor.Exit(string.Intern(fileName));
            }

            fileName = HttpContext.Current.Server.MapPath(ConstructUserDataFileName(userName, path));

            try
            {
                if (Monitor.TryEnter(string.Intern(fileName), 5000))
                {
                    if (File.Exists(fileName) && !string.IsNullOrEmpty(userName))
                        userDataBlob = File.ReadAllBytes(fileName);
                }
                else
                {
                    throw new ApplicationException("Monitor timed out");
                }
            }
            finally
            {
                Monitor.Exit(string.Intern(fileName));
            }
        }

        protected override void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName, byte[] dataBlob)
        {
            string fileName = null;

            if (HttpContext.Current.Request.QueryString.Count > 0)
                path = path + "?" + HttpContext.Current.Request.QueryString.ToString();

            if (!string.IsNullOrEmpty(userName))
                fileName = ConstructUserDataFileName(userName, path);
            else
                fileName = ConstructAllUsersDataFileName(path);

            // lock on the filename in case two clients try accessing the same
            // file concurrently

            try
            {
                if (Monitor.TryEnter(string.Intern(fileName), 5000))
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(fileName), dataBlob);
                else
                    throw new ApplicationException("Failed to acquire lock on the file to write data");
            }
            finally
            {
                Monitor.Exit(string.Intern(fileName));
            }

        }

        /// <summary>
        /// Helper funciont for creating a unique filename for a particular user based on a path
        /// </summary>
        /// <param name="webPartManager"></param>
        /// <param name="path"></param>
        /// <param name="userName"></param>
        private string ConstructUserDataFileName(string userName, string path)
        {
            string pathConvertedToFileName = path.Replace('/', '_');
            pathConvertedToFileName = pathConvertedToFileName.Replace('?', '_');
            pathConvertedToFileName = pathConvertedToFileName.Replace('~', '_');
            pathConvertedToFileName = pathConvertedToFileName.Replace('.', '_');

            return VirtualPathUtility.Combine(this._directoryName, userName + pathConvertedToFileName + ".bin");
        }

        /// <summary>
        /// Helper function for creating a unique filename for all users based on a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ConstructAllUsersDataFileName(string path)
        {
            string pathConvertedToFileName = path.Replace('/', '_');
            pathConvertedToFileName = pathConvertedToFileName.Replace('?', '_');
            pathConvertedToFileName = pathConvertedToFileName.Replace('~', '_');
            pathConvertedToFileName = pathConvertedToFileName.Replace('.', '_');

            return VirtualPathUtility.Combine(this._directoryName, "allusers" + pathConvertedToFileName + ".bin");
        }

        public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
        {
            throw new NotImplementedException();
        }

        protected override void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName)
        {
            throw new NotImplementedException();
        }

        public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }
    }
}
