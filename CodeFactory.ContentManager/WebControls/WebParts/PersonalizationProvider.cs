using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using CodeFactory.DataAccess.Transactions;
using CodeFactory.DataAccess;
using System.Configuration.Provider;
using CodeFactory.Utilities;
using System.Globalization;
using System.Collections.Specialized;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class PersonalizationProvider : System.Web.UI.WebControls.WebParts.PersonalizationProvider
    {
        // Fields
        private string _applicationName;
        private int _commandTimeout;
        private const int maxStringLength = 0x100;

        // Methods
        private PersonalizationStateInfoCollection FindSharedState(string path, int pageIndex, int pageSize, out int totalRecords)
        {
            PersonalizationStateInfoCollection infos2 = null;
            totalRecords = 0;

            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_FindState");

                cmd.DbCommand.CommandTimeout = this._commandTimeout;
                cmd.Parameters["AllUsersScope"].Value = true;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;
                cmd.Parameters["PageIndex"].Value = pageIndex;
                cmd.Parameters["PageSize"].Value = pageSize;

                if (path != null)
                    cmd.Parameters["Path"].Value = path;

                IDataReader reader =  cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                try
                {
                    PersonalizationStateInfoCollection infos = new PersonalizationStateInfoCollection();

                    while (reader.Read())
                    {
                        string str = reader.GetString(0);
                        DateTime lastUpdatedDate = reader.IsDBNull(1) ? DateTime.MinValue : DateTime.SpecifyKind(reader.GetDateTime(1), DateTimeKind.Utc);
                        int size = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        int sizeOfPersonalizations = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        int countOfPersonalizations = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        infos.Add(new SharedPersonalizationStateInfo(str, lastUpdatedDate, size, sizeOfPersonalizations, countOfPersonalizations));
                    }

                    if ((cmd.Parameters["ReturnValue"].Value != null) && (cmd.Parameters["ReturnValue"].Value is int))
                        totalRecords = (int)cmd.Parameters["ReturnValue"].Value;

                    infos2 = infos;
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }

            return infos2;
        }

        public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize, out int totalRecords)
        {
            PersonalizationProviderHelper.CheckPersonalizationScope(scope);
            PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);

            if (scope == PersonalizationScope.Shared)
            {
                string str = null;
                if (query != null)
                    str = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 0x100);

                return this.FindSharedState(str, pageIndex, pageSize, out totalRecords);
            }

            string path = null;
            DateTime defaultInactiveSinceDate = DateTime.MaxValue;
            string username = null;

            if (query != null)
            {
                path = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 0x100);
                defaultInactiveSinceDate = query.UserInactiveSinceDate;
                username = StringUtil.CheckAndTrimString(query.UsernameToMatch, "query.UsernameToMatch", false, 0x100);
            }

            return this.FindUserState(path, defaultInactiveSinceDate, username, pageIndex, pageSize, out totalRecords);
        }

        private PersonalizationStateInfoCollection FindUserState(string path, DateTime inactiveSinceDate, string username, int pageIndex, int pageSize, out int totalRecords)
        {
            PersonalizationStateInfoCollection infos2 = null;
            totalRecords = 0;

            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_FindState");
                cmd.DbCommand.CommandTimeout = this._commandTimeout;

                cmd.Parameters["AllUsersScope"].Value = false;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;
                cmd.Parameters["PageIndex"].Value = pageIndex;
                cmd.Parameters["PageSize"].Value = pageSize;

                if (path != null)
                    cmd.Parameters["Path"].Value = path;

                if (username != null)
                    cmd.Parameters["UserName"].Value = username;

                if (inactiveSinceDate != DateTime.MaxValue)
                    cmd.Parameters["InactiveSinceDate"].Value = inactiveSinceDate.ToUniversalTime();

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                try
                {
                    PersonalizationStateInfoCollection infos = new PersonalizationStateInfoCollection();

                    while (reader.Read())
                    {
                        string str = reader.GetString(0);
                        DateTime lastUpdatedDate = DateTime.SpecifyKind(reader.GetDateTime(1), DateTimeKind.Utc);
                        int size = reader.GetInt32(2);
                        string str2 = reader.GetString(3);
                        DateTime lastActivityDate = DateTime.SpecifyKind(reader.GetDateTime(4), DateTimeKind.Utc);
                        infos.Add(new UserPersonalizationStateInfo(str, lastUpdatedDate, size, str2, lastActivityDate));
                    }

                    if ((cmd.Parameters["ReturnValue"].Value != null) && (cmd.Parameters["ReturnValue"].Value is int))
                        totalRecords = (int)cmd.Parameters["ReturnValue"].Value;

                    infos2 = infos;
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }

            return infos2;
        }

        private int GetCountOfSharedState(string path)
        {
            int num = 0;

            using (TransactionContextFactory.EnterContext(TransactionAffinity.Supported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_GetCountOfState");
                cmd.DbCommand.CommandTimeout = this._commandTimeout;

                cmd.Parameters["AllUsersScope"].Value = true;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;

                if (path != null)
                    cmd.Parameters["Path"].Value = path;

                int affectedRows = cmd.ExecuteNonQuery();

                if (((cmd.Parameters["Count"] != null) && (cmd.Parameters["Count"].Value != null)) && 
                    (cmd.Parameters["Count"].Value is int))
                    num = (int)cmd.Parameters["Count"].Value;
            }

            return num;
        }

        public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
        {
            PersonalizationProviderHelper.CheckPersonalizationScope(scope);

            if (scope == PersonalizationScope.Shared)
            {
                string str = null;
                if (query != null)
                    str = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 0x100);

                return this.GetCountOfSharedState(str);
            }

            string path = null;
            DateTime defaultInactiveSinceDate = DateTime.MaxValue;
            string username = null;

            if (query != null)
            {
                path = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 0x100);
                defaultInactiveSinceDate = query.UserInactiveSinceDate;
                username = StringUtil.CheckAndTrimString(query.UsernameToMatch, "query.UsernameToMatch", false, 0x100);
            }

            return this.GetCountOfUserState(path, defaultInactiveSinceDate, username);
        }

        private int GetCountOfUserState(string path, DateTime inactiveSinceDate, string username)
        {
            int num = 0;

            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_GetCountOfState");
                cmd.DbCommand.CommandTimeout = this._commandTimeout;

                cmd.Parameters["AllUsersScope"].Value = true;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;
                if (path != null)
                    cmd.Parameters["Path"].Value = path;

                if (username != null)
                    cmd.Parameters["UserName"].Value = path;

                if (inactiveSinceDate != DateTime.MaxValue)
                    cmd.Parameters["InactiveSinceDate"].Value = inactiveSinceDate.ToUniversalTime();

                int affectedRows = cmd.ExecuteNonQuery();

                if (((cmd.Parameters["Count"] != null) && (cmd.Parameters["Count"].Value != null)) &&
                    (cmd.Parameters["Count"].Value is int))
                    num = (int)cmd.Parameters["Count"].Value;
            }

            return num;
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

            string str = configSettings["connectionStringName"];

            if (string.IsNullOrEmpty(str))
                throw new ProviderException(ResourceStringLoader.GetResourceString(
                    "PersonalizationProvider_NoConnection"));

            configSettings.Remove("connectionStringName");

            this._commandTimeout = SecUtility.GetIntValue(configSettings, "commandTimeout", 60, true, 0);

            configSettings.Remove("commandTimeout");

            if (configSettings.Count > 0)
            {
                string key = configSettings.GetKey(0);
                throw new ProviderException(ResourceStringLoader.GetResourceString(
                    "PersonalizationProvider_UnknownProp", new object[] { key, name }));
            }
        }

        private byte[] LoadPersonalizationBlob(string path, string userName)
        {
            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = null;

                if (userName != null)
                    cmd = datasource.GetCommand("aspnet_PersonalizationPerUser_GetPageSettings");
                else
                    cmd = datasource.GetCommand("aspnet_PersonalizationAllUsers_GetPageSettings");

                cmd.DbCommand.CommandTimeout = this._commandTimeout;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;
                cmd.Parameters["Path"].Value = path;

                if (userName != null)
                {
                    cmd.Parameters["UserName"].Value = userName;
                    cmd.Parameters["CurrentTimeUtc"].Value = DateTime.UtcNow;
                }

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                try
                {
                    if (reader.Read())
                    {
                        int length = (int)reader.GetBytes(0, 0L, null, 0, 0);
                        byte[] buffer = new byte[length];
                        reader.GetBytes(0, 0L, buffer, 0, length);

                        return buffer;
                    }
                }
                finally
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            return null;
        }

        protected override void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName, ref byte[] sharedDataBlob, ref byte[] userDataBlob)
        {
            if (HttpContext.Current.Request.QueryString.Count > 0)
                path = path + "?" + HttpContext.Current.Request.QueryString.ToString();

            sharedDataBlob = null;
            userDataBlob = null;

            sharedDataBlob = this.LoadPersonalizationBlob(path, null);

            if (!string.IsNullOrEmpty(userName))
                userDataBlob = this.LoadPersonalizationBlob(path, userName);
        }

        private int ResetAllState(PersonalizationScope scope)
        {
            int num = 0;

            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_DeleteAllState");
                cmd.DbCommand.CommandTimeout = this._commandTimeout;

                cmd.Parameters["AllUsersScope"].Value = scope == PersonalizationScope.Shared;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;

                int affectedRows = cmd.ExecuteNonQuery();

                if (((cmd.Parameters["Count"] != null) && (cmd.Parameters["Count"].Value != null)) && 
                    (cmd.Parameters["Count"].Value is int))
                    num = (int)cmd.Parameters["Count"].Value;
            }

            return num;
        }

        protected override void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName)
        {
            if (HttpContext.Current.Request.QueryString.Count > 0)
                path = path + "?" + HttpContext.Current.Request.QueryString.ToString();

            this.ResetPersonalizationState(path, userName);
        }

        private void ResetPersonalizationState(string path, string userName)
        {
            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = null;

                if (userName != null)
                    cmd = datasource.GetCommand("aspnet_PersonalizationPerUser_ResetPageSettings");
                else
                    cmd = datasource.GetCommand("aspnet_PersonalizationAllUsers_ResetPageSettings");

                cmd.DbCommand.CommandTimeout = this._commandTimeout;

                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;
                cmd.Parameters["Path"].Value = path;

                if (userName != null)
                {
                    cmd.Parameters["UserName"].Value = userName;
                    cmd.Parameters["CurrentTimeUtc"].Value = DateTime.UtcNow;
                }

                int affectedRows = cmd.ExecuteNonQuery();
            }
        }

        private int ResetSharedState(string[] paths)
        {
            int num = 0;
            if (paths == null)
                return this.ResetAllState(PersonalizationScope.Shared);

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_ResetSharedState");
                cmd.DbCommand.CommandTimeout = this._commandTimeout;

                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;

                try
                {
                    foreach (string str in paths)
                    {
                        cmd.Parameters["Path"].Value = str;

                        int affectedRows = cmd.ExecuteNonQuery();

                        if (cmd.Parameters["Count"] != null && cmd.Parameters["Count"].Value != null &&
                            cmd.Parameters["Count"].Value is int)
                            num += (int)cmd.Parameters["Count"].Value;
                    }

                    context.VoteCommit();
                }
                catch
                {
                    context.VoteRollback();
                    throw;
                }
            }

            return num;
        }

        public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
        {
            PersonalizationProviderHelper.CheckPersonalizationScope(scope);
            paths = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(paths, "paths", false, false, 0x100);
            usernames = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(usernames, "usernames", false, true, 0x100);

            if (scope == PersonalizationScope.Shared)
            {
                PersonalizationProviderHelper.CheckUsernamesInSharedScope(usernames);
                return this.ResetSharedState(paths);
            }

            PersonalizationProviderHelper.CheckOnlyOnePathWithUsers(paths, usernames);

            return this.ResetUserState(paths, usernames);
        }

        public override int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            path = StringUtil.CheckAndTrimString(path, "path", false, 0x100);

            string[] paths = (path == null) ? null : new string[] { path };

            return this.ResetUserState(ResetUserStateMode.PerInactiveDate, userInactiveSinceDate, paths, null);
        }

        private int ResetUserState(string[] paths, string[] usernames)
        {
            bool flag = (paths != null) && (paths.Length != 0);
            bool flag2 = (usernames != null) && (usernames.Length != 0);

            if (!(flag || flag2))
                return this.ResetAllState(PersonalizationScope.User);

            if (!flag2)
                return this.ResetUserState(ResetUserStateMode.PerPaths, DateTime.MaxValue, paths, usernames);

            return this.ResetUserState(ResetUserStateMode.PerUsers, DateTime.MaxValue, paths, usernames);
        }

        private int ResetUserState(ResetUserStateMode mode, DateTime userInactiveSinceDate, string[] paths, string[] usernames)
        {
            int num = 0;

            using (TransactionContext context = TransactionContextFactory.EnterContext(TransactionAffinity.Required))
            {
                try
                {
                    IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                    IDataCommand cmd = datasource.GetCommand("aspnet_PersonalizationAdministration_ResetUserState");
                    cmd.DbCommand.CommandTimeout = this._commandTimeout;

                    cmd.Parameters["ApplicationName"].Value = this.ApplicationName;


                    string str = (paths != null && paths.Length > 0) ? paths[0] : null;

                    if (mode == ResetUserStateMode.PerInactiveDate)
                    {
                        if (userInactiveSinceDate != DateTime.MaxValue)
                            cmd.Parameters["InactiveSinceDate"].Value = userInactiveSinceDate.ToUniversalTime();

                        if (str != null)
                            cmd.Parameters["Path"].Value = str;

                        int affectedRows = cmd.ExecuteNonQuery();

                        if (((cmd.Parameters["Count"] != null) && (cmd.Parameters["Count"].Value != null)) &&
                            (cmd.Parameters["Count"].Value is int))
                            num = (int)cmd.Parameters["Count"].Value;
                    }
                    else if (mode == ResetUserStateMode.PerPaths)
                    {
                        foreach (string str2 in paths)
                        {
                            cmd.Parameters["Path"].Value = str2;

                            int affectedRows = cmd.ExecuteNonQuery();

                            if (((cmd.Parameters["Count"] != null) && (cmd.Parameters["Count"].Value != null)) &&
                                (cmd.Parameters["Count"].Value is int))
                                num += (int)cmd.Parameters["Count"].Value;
                        }
                    }
                    else
                    {
                        if (str != null)
                            cmd.Parameters["Path"].Value = str;

                        foreach (string str3 in usernames)
                        {
                            cmd.Parameters["UserName"].Value = str3;

                            int affectedRows = cmd.ExecuteNonQuery();

                            if (((cmd.Parameters["Count"] != null) && (cmd.Parameters["Count"].Value != null)) &&
                                (cmd.Parameters["Count"].Value is int))
                                num += (int)cmd.Parameters["Count"].Value;
                        }
                    }

                    context.VoteCommit();
                }
                catch
                {
                    context.VoteRollback();
                    throw;
                }
            }

            return num;
        }

        protected override void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName, byte[] dataBlob)
        {
            if (HttpContext.Current.Request.QueryString.Count > 0)
                path = path + "?" + HttpContext.Current.Request.QueryString.ToString();

            this.SavePersonalizationState(path, userName, dataBlob);
        }

        private void SavePersonalizationState(string path, string userName, byte[] state)
        {
            using (TransactionContextFactory.EnterContext(TransactionAffinity.NotSupported))
            {
                IDataSource datasource = DataSourceFactory.GetDataSource("PersonalizationProvider");
                IDataCommand cmd = null;

                if (userName != null)
                    cmd = datasource.GetCommand("aspnet_PersonalizationPerUser_SetPageSettings");
                else
                    cmd = datasource.GetCommand("aspnet_PersonalizationAllUsers_SetPageSettings");

                cmd.DbCommand.CommandTimeout = this._commandTimeout;
                cmd.Parameters["ApplicationName"].Value = this.ApplicationName;
                cmd.Parameters["Path"].Value = path;
                cmd.Parameters["PageSettings"].Value = state;
                cmd.Parameters["CurrentTimeUtc"].Value = DateTime.UtcNow;

                if (userName != null)
                    cmd.Parameters["UserName"].Value = userName;

                int affectedRows = cmd.ExecuteNonQuery();
            }
        }

        // Properties
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

        // Nested Types
        private enum ResetUserStateMode
        {
            PerInactiveDate,
            PerPaths,
            PerUsers
        }
    }
}
