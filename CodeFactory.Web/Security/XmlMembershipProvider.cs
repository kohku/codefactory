using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.Security;
using System.Xml;
using System.Threading;

namespace CodeFactory.Web.Security
{
    public class XmlMembershipProvider : MembershipProvider
    {
        private Dictionary<string, MembershipUser> _users;
        private string _xmlFileName;

        private ReaderWriterLockSlim _protectionLock;

        private bool _enablePasswordRetrieval;
        private bool _enablePasswordReset = true;
        private bool _requiresQuestionAndAnswer = true;
        private bool _requiresUniqueEmail = true;
        private MembershipPasswordFormat _passwordFormat = MembershipPasswordFormat.Clear;
        private int _maxInvalidPasswordAttempts = 5;
        private int _minRequiredPasswordLength = 7;
        private int _minRequiredNonalphanumericCharacters = 1;
        private int _passwordAttemptWindow = 10;
        private string _passwordStrengthRegularExpression;

        public XmlMembershipProvider()
        {
            _protectionLock = new ReaderWriterLockSlim();
        }

        #region Properties

        // MembershipProvider Properties
        /// <summary>
        /// 
        /// </summary>
        public override string ApplicationName
        {
            get { return Membership.ApplicationName; }
            set { Membership.ApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return this._enablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return this._enablePasswordRetrieval;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return this._maxInvalidPasswordAttempts;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return this._minRequiredNonalphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return this._minRequiredPasswordLength;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return this._passwordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return this._passwordFormat;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return this._passwordStrengthRegularExpression;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return this._requiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return this._requiresUniqueEmail;
            }
        }

        #endregion

        #region Supported methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "XmlMembershipProvider";

            if (Type.GetType("Mono.Runtime") != null)
            {
                // Mono dies with a "Unrecognized attribute: description" if a description is part of the config.
                if (!string.IsNullOrEmpty(config["description"]))
                {
                    config.Remove("description");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(config["description"]))
                {
                    config.Remove("description");
                    config.Add("description", "XML membership provider");
                }
            }

            base.Initialize(name, config);

            this._enablePasswordRetrieval = SecUtility.GetBooleanValue(config, "enablePasswordRetrieval", false);
            config.Remove("enablePasswordRetrieval");
            this._enablePasswordReset = SecUtility.GetBooleanValue(config, "enablePasswordReset", true);
            config.Remove("enablePasswordReset");
            this._requiresQuestionAndAnswer = SecUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            config.Remove("requiresQuestionAndAnswer");
            this._requiresUniqueEmail = SecUtility.GetBooleanValue(config, "requiresUniqueEmail", true);
            config.Remove("requiresUniqueEmail");
            this._maxInvalidPasswordAttempts = SecUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            config.Remove("maxInvalidPasswordAttempts");
            this._passwordAttemptWindow = SecUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            config.Remove("passwordAttemptWindow");
            this._minRequiredPasswordLength = SecUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, 0x80);
            config.Remove("minRequiredPasswordLength");
            this._minRequiredNonalphanumericCharacters = SecUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 0x80);
            config.Remove("minRequiredNonalphanumericCharacters");
            this._passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            config.Remove("passwordStrengthRegularExpression");
            if (!string.IsNullOrEmpty(config["passwordFormat"]))
                this._passwordFormat = (MembershipPasswordFormat)Enum.Parse(typeof(MembershipPasswordFormat),
                    config["passwordFormat"]);
            config.Remove("passwordFormat");
            if (this.PasswordFormat == MembershipPasswordFormat.Hashed && this.EnablePasswordRetrieval)
                throw new ProviderException("Provider can not retrieve hashed password");

            // Initialize _xmlFileName and make sure the path is app-relative
            string path = config["xmlFileName"];

            if (String.IsNullOrEmpty(path))
                path = @"~\App_Data\users.xml";

            if (!VirtualPathUtility.IsAppRelative(path))
                throw new ArgumentException("xmlFileName must be app-relative");

            string fullyQualifiedPath = VirtualPathUtility.Combine(
                VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath), 
                path);

            _xmlFileName = HostingEnvironment.MapPath(fullyQualifiedPath);
            config.Remove("xmlFileName");

            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, _xmlFileName);
            permission.Demand();

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }
        }

        /// <summary>
        /// Returns true if the username and password match an exsisting user.
        /// </summary>
        public override bool ValidateUser(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                return false;

            try
            {
                ReadMembershipDataStore();

                // Validate the user name and password
                MembershipUser user;

                if (_users.TryGetValue(username, out user))
                {
                    if (user.Comment == password) // Case-sensitive
                    {
                        user.LastLoginDate = DateTime.Now;

                        UpdateUser(user);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves a user based on his/hers username.
        /// the userIsOnline parameter is ignored.
        /// </summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (String.IsNullOrEmpty(username))
                return null;

            ReadMembershipDataStore();

            // Retrieve the user from the data source
            MembershipUser user;

            if (_users.TryGetValue(username, out user))
                return user;

            return null;
        }

        /// <summary>
        /// Retrieves a collection of all the users.
        /// This implementation ignores pageIndex and pageSize,
        /// and it doesn't sort the MembershipUser objects returned.
        /// </summary>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            ReadMembershipDataStore();

            MembershipUserCollection users = new MembershipUserCollection();

            foreach (KeyValuePair<string, MembershipUser> pair in _users)
            {
                users.Add(pair.Value);
            }

            totalRecords = users.Count;
            return users;
        }

        /// <summary>
        /// Changes a users password.
        /// </summary>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFileName);
            XmlNodeList nodes = doc.GetElementsByTagName("User");
            foreach (XmlNode node in nodes)
            {
                if (node["UserName"].InnerText.Equals(username, StringComparison.OrdinalIgnoreCase)
                  || node["Password"].InnerText.Equals(oldPassword, StringComparison.OrdinalIgnoreCase))
                {
                    node["Password"].InnerText = newPassword;
                    doc.Save(_xmlFileName);

                    _users = null;

                    ReadMembershipDataStore();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a new user store he/she in the XML file
        /// </summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFileName);

            XmlNode xmlUserRoot = doc.CreateElement("User");
            XmlNode xmlUserName = doc.CreateElement("UserName");
            XmlNode xmlPassword = doc.CreateElement("Password");
            XmlNode xmlEmail = doc.CreateElement("Email");
            XmlNode xmlLastLoginTime = doc.CreateElement("LastLoginTime");

            xmlUserName.InnerText = username;
            xmlPassword.InnerText = password;
            xmlEmail.InnerText = email;
            xmlLastLoginTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            xmlUserRoot.AppendChild(xmlUserName);
            xmlUserRoot.AppendChild(xmlPassword);
            xmlUserRoot.AppendChild(xmlEmail);
            xmlUserRoot.AppendChild(xmlLastLoginTime);

            doc.SelectSingleNode("Users").AppendChild(xmlUserRoot);
            doc.Save(_xmlFileName);

            status = MembershipCreateStatus.Success;
            MembershipUser user = new MembershipUser(Name, username, username, email, passwordQuestion, password, isApproved, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.MaxValue);
            _users.Add(username, user);
            return user;
        }

        /// <summary>
        /// Deletes the user from the XML file and 
        /// removes him/her from the internal cache.
        /// </summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFileName);

            foreach (XmlNode node in doc.GetElementsByTagName("User"))
            {
                if (node.ChildNodes[0].InnerText.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    doc.SelectSingleNode("Users").RemoveChild(node);
                    doc.Save(_xmlFileName);
                    _users.Remove(username);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get a user based on the username parameter.
        /// the userIsOnline parameter is ignored.
        /// </summary>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null)
                throw new ArgumentNullException("providerUserKey");

            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFileName);

            foreach (XmlNode node in doc.SelectNodes("//User"))
            {
                if (node.ChildNodes[0].InnerText.Equals(providerUserKey.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userName = node.ChildNodes[0].InnerText;
                    string password = node.ChildNodes[1].InnerText;
                    string email = node.ChildNodes[2].InnerText;
                    DateTime lastLoginTime = DateTime.Parse(node.ChildNodes[3].InnerText, CultureInfo.InvariantCulture);

                    return new MembershipUser(Name, providerUserKey.ToString(), providerUserKey, email, string.Empty, password, true, false, DateTime.Now, lastLoginTime, DateTime.Now, DateTime.Now, DateTime.MaxValue);
                }
            }

            return default(MembershipUser);
        }

        /// <summary>
        /// Retrieves a username based on a matching email.
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            if (email == null)
                throw new ArgumentNullException("email");

            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFileName);

            foreach (XmlNode node in doc.GetElementsByTagName("User"))
            {
                if (node.ChildNodes[2].InnerText.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return node.ChildNodes[0].InnerText;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates a user. The username will not be changed.
        /// </summary>
        public override void UpdateUser(MembershipUser user)
        {
            _protectionLock.EnterWriteLock();

            try
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlFileName);

                foreach (XmlNode node in doc.GetElementsByTagName("User"))
                {
                    if (node.ChildNodes[0].InnerText.Equals(user.UserName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (user.Comment.Length > 30)
                        {
                            node.ChildNodes[1].InnerText = user.Comment;
                        }
                        node.ChildNodes[2].InnerText = user.Email;
                        node.ChildNodes[3].InnerText = user.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                }

                doc.Save(_xmlFileName);
                _users[user.UserName] = user;
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Builds the internal cache of users.
        /// </summary>
        private void ReadMembershipDataStore()
        {
            _protectionLock.EnterReadLock();

            try
            {

                if (_users == null)
                {
                    _users = new Dictionary<string, MembershipUser>(16, StringComparer.OrdinalIgnoreCase);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(_xmlFileName);
                    XmlNodeList nodes = doc.GetElementsByTagName("User");

                    foreach (XmlNode node in nodes)
                    {
                        MembershipUser user = new MembershipUser(
                            Name,                       // Provider name
                            node["UserName"].InnerText, // Username
                            node["UserName"].InnerText, // providerUserKey
                            node["Email"].InnerText,    // Email
                            String.Empty,               // passwordQuestion
                            node["Password"].InnerText, // Comment
                            true,                       // isApproved
                            false,                      // isLockedOut
                            DateTime.Now,               // creationDate
                            DateTime.Parse(node["LastLoginTime"].InnerText, CultureInfo.InvariantCulture), // lastLoginDate
                            DateTime.Now,               // lastActivityDate
                            DateTime.Now, // lastPasswordChangedDate
                            new DateTime(1980, 1, 1)    // lastLockoutDate
                        );

                        _users.Add(user.UserName, user);
                    }
                }
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        ///// <summary>
        ///// Encrypts a string using the SHA256 algorithm.
        ///// </summary>
        //private static string Encrypt(string plainMessage)
        //{
        //  byte[] data = Encoding.UTF8.GetBytes(plainMessage);
        //  using (HashAlgorithm sha = new SHA256Managed())
        //  {
        //    byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);
        //    return Convert.ToBase64String(sha.Hash);
        //  }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            ReadMembershipDataStore();

            MembershipUserCollection users = new MembershipUserCollection();

            List<MembershipUser> allUsers = new List<MembershipUser>(_users.Values).FindAll(delegate(MembershipUser match)
            {
                return match.UserName.Equals(emailToMatch);
            });

            totalRecords = users.Count;

            if (pageIndex >= allUsers.Count)
                throw new ArgumentOutOfRangeException("pageIndex");

            for (int i = pageIndex; i < pageIndex + pageSize && i < allUsers.Count; i++)
            {
                users.Add(allUsers[i]);
            }

            return users;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            ReadMembershipDataStore();

            MembershipUserCollection users = new MembershipUserCollection();

            List<MembershipUser> allUsers = new List<MembershipUser>(_users.Values).FindAll(delegate(MembershipUser match)
            {
                return match.UserName.Equals(usernameToMatch);
            });

            if (pageIndex >= allUsers.Count)
                throw new ArgumentOutOfRangeException("pageIndex");

            for (int i = pageIndex; i < pageIndex + pageSize && i < allUsers.Count; i++)
            {
                users.Add(allUsers[i]);
            }

            totalRecords = users.Count;

            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("Membership password retrieval not supported");
        }

        #endregion
    }
}
