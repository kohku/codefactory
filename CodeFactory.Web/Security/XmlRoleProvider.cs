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
    public class XmlRoleProvider : RoleProvider
    {
        #region Properties

        private List<Role> _roles = new List<Role>();
        private List<string> _userNames = new List<string>();
        private string _xmlFileName;
        readonly string[] _defaultRolesToAdd = new string[] { "Administrator" };
        private ReaderWriterLockSlim _protectionLock;

        public XmlRoleProvider()
        {
            _protectionLock = new ReaderWriterLockSlim();
        }

        ///<summary>
        ///Gets or sets the name of the application to store and retrieve role information for.
        ///</summary>
        ///
        ///<returns>
        ///The name of the application to store and retrieve role information for.
        ///</returns>
        ///
        public override string ApplicationName
        {
            get { return Roles.ApplicationName; }
            set { Roles.ApplicationName = value; }
        }

        ///<summary>
        ///Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        ///</returns>
        ///
        ///<param name="roleName">The name of the role to search for in the data source. </param>
        public override bool RoleExists(string roleName)
        {
            List<string> currentRoles = new List<string>(GetAllRoles());
            return (currentRoles.Contains(roleName)) ? true : false;
        }

        ///<summary>
        ///Gets a list of all the roles for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the roles stored in the data source for the configured applicationName.
        ///</returns>
        ///
        public override string[] GetAllRoles()
        {
            List<string> allRoles = new List<string>();
            foreach (Role role in _roles)
            {
                allRoles.Add(role.Name);
            }
            return allRoles.ToArray();
        }

        ///<summary>
        ///Gets a list of users in the specified role for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        ///</returns>
        ///
        ///<param name="roleName">The name of the role to get the list of users for. </param>
        public override string[] GetUsersInRole(string roleName)
        {
            //  ReadRoleDataStore();
            List<string> UsersInRole = new List<string>();

            foreach (Role role in _roles)
            {
                if (role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (string user in role.Users)
                    {
                        UsersInRole.Add(user.ToLowerInvariant());
                    }
                }
            }
            return UsersInRole.ToArray();
        }

        ///<summary>
        ///Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        ///</returns>
        ///
        ///<param name="username">The user name to search for.</param>
        ///<param name="roleName">The role to search in.</param>
        public override bool IsUserInRole(string username, string roleName)
        {
            foreach (Role role in _roles)
            {
                if (role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (string user in role.Users)
                    {
                        if (user == username)
                            return true;
                    }
                }
            }
            return false;
        }

        ///<summary>
        ///Gets a list of the roles that a specified user is in for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        ///</returns>
        ///
        ///<param name="username">The user to return a list of roles for.</param>
        public override string[] GetRolesForUser(string username)
        {
            //  ReadRoleDataStore();
            List<string> rolesForUser = new List<string>();

            foreach (Role role in _roles)
            {
                foreach (string user in role.Users)
                {
                    if (user.Equals(username, StringComparison.OrdinalIgnoreCase))
                        rolesForUser.Add(role.Name);
                }
            }
            return rolesForUser.ToArray();
        }

        #endregion

        #region Supported methods

        ///<summary>
        ///Gets an array of user names in a role where the user name contains the specified user name to match.
        ///</summary>
        ///
        ///<returns>
        ///A string array containing the names of all the users where the user name matches usernameToMatch and the user is a member of the specified role.
        ///</returns>
        ///
        ///<param name="usernameToMatch">The user name to search for.</param>
        ///<param name="roleName">The role to search in.</param>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            List<string> UsersInRole = new List<string>();

            if (IsUserInRole(usernameToMatch, roleName))
                UsersInRole.AddRange(_userNames);

            return UsersInRole.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                _userNames.Add(user.UserName);
            }

            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "XmlRoleProvider";

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
                    config.Add("description", "XML role provider");
                }
            }

            base.Initialize(name, config);

            // Initialize _xmlFileName and make sure the path
            // is app-relative
            string path = config["xmlFileName"];

            if (String.IsNullOrEmpty(path))
                path = @"~\App_Data\roles.xml";
            //path = settings.StorageLocation.Url + "\roles.xml";

            if (!VirtualPathUtility.IsAppRelative(path))
                throw new ArgumentException
                    ("xmlFileName must be app-relative");

            string fullyQualifiedPath = VirtualPathUtility.Combine
                (VirtualPathUtility.AppendTrailingSlash
                     (HttpRuntime.AppDomainAppVirtualPath), path);

            _xmlFileName = HostingEnvironment.MapPath(fullyQualifiedPath);
            config.Remove("xmlFileName");

            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, _xmlFileName);
            permission.Demand();

            if (!System.IO.File.Exists(_xmlFileName))
                AddUsersToRoles(_userNames.ToArray(), _defaultRolesToAdd);

            //Now that we know a xml file exists we can call it.
            ReadRoleDataStore();

            if (!RoleExists("Administrator"))
                AddUsersToRoles(_userNames.ToArray(), _defaultRolesToAdd);

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }
        }

        ///<summary>
        ///Adds the specified user names to the specified roles for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleNames">A string array of the role names to add the specified user names to. </param>
        ///<param name="usernames">A string array of user names to be added to the specified roles. </param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            List<string> currentRoles = new List<string>(GetAllRoles());
            if (usernames.Length != 0 && roleNames.Length != 0)
            {
                foreach (string _rolename in roleNames)
                {
                    if (!currentRoles.Contains(_rolename))
                    {
                        _roles.Add(new Role(_rolename, new List<string>(usernames)));
                    }
                }

                foreach (Role role in _roles)
                {
                    foreach (string _name in roleNames)
                    {
                        if (role.Name.Equals(_name, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string s in usernames)
                            {
                                if (!role.Users.Contains(s))
                                    role.Users.Add(s);
                            }
                        }
                    }
                }
            }
            Save();
        }

        ///<summary>
        ///Removes the specified user names from the specified roles for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleNames">A string array of role names to remove the specified user names from. </param>
        ///<param name="usernames">A string array of user names to be removed from the specified roles. </param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            if (usernames.Length != 0 && roleNames.Length != 0)
            {
                foreach (Role role in _roles)
                {
                    foreach (string _name in roleNames)
                    {
                        if (role.Name.Equals(_name, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string user in usernames)
                            {
                                if (role.Name.Equals("administrators", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (role.Users.Count != 1)
                                    {
                                        if (role.Users.Contains(user))
                                            role.Users.Remove(user);
                                    }
                                }
                                else
                                {
                                    if (role.Users.Contains(user))
                                        role.Users.Remove(user);
                                }
                            }

                        }
                    }
                }
            }
            Save();
        }

        ///<summary>
        ///Removes a role from the data source for the configured applicationName.
        ///</summary>
        ///
        ///<returns>
        ///true if the role was successfully deleted; otherwise, false.
        ///</returns>
        ///
        ///<param name="throwOnPopulatedRole">If true, throw an exception if roleName has one or more members and do not delete roleName.</param>
        ///<param name="roleName">The name of the role to delete.</param>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (!roleName.Equals("administrators", StringComparison.OrdinalIgnoreCase))
            {
                _roles.Remove(new Role(roleName));
                Save();
                return true;
            }
            return false;
        }

        ///<summary>
        ///Adds a new role to the data source for the configured applicationName.
        ///</summary>
        ///
        ///<param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            if (!_roles.Contains(new Role(roleName)))
            {
                _roles.Add(new Role(roleName));
                Save();
            }

        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Builds the internal cache of users.
        /// </summary>
        private void ReadRoleDataStore()
        {
            _protectionLock.EnterReadLock();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(_xmlFileName);

                XmlNodeList nodes = doc.GetElementsByTagName("Role");

                foreach (XmlNode roleNode in nodes)
                {
                    Role tempRole = new Role(roleNode.SelectSingleNode("Name").InnerText);
                    foreach (XmlNode userNode in roleNode.SelectNodes("Users/User"))
                    {
                        tempRole.Users.Add(userNode.InnerText);
                    }
                    _roles.Add(tempRole);

                }
            }
            catch (XmlException)
            {
                AddUsersToRoles(_userNames.ToArray(), _defaultRolesToAdd);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        ///<summary>
        ///</summary>
        public void Save()
        {
            _protectionLock.EnterWriteLock();

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(_xmlFileName, settings))
                {
                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("Roles");

                    foreach (Role _role in _roles)
                    {
                        writer.WriteStartElement("Role");
                        writer.WriteElementString("Name", _role.Name);
                        writer.WriteStartElement("Users");

                        foreach (string username in _role.Users)
                        {
                            writer.WriteElementString("User", username);
                        }

                        writer.WriteEndElement(); //closes users
                        writer.WriteEndElement(); //closes role
                    }
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Only so we can add users to the adminstrators role.
        /// </summary>
        //private void ReadMembershipDataStore()
        //{
        //    string fullyQualifiedPath = VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash
        //      (HttpRuntime.AppDomainAppVirtualPath), @"~\App_Data\users.xml");

        //    lock (this)
        //    {
        //        if (_userNames == null)
        //        {
        //            _userNames = new List<string>();
        //            XmlDocument doc = new XmlDocument();
        //            doc.Load(HostingEnvironment.MapPath(fullyQualifiedPath));
        //            XmlNodeList nodes = doc.GetElementsByTagName("User");

        //            foreach (XmlNode node in nodes)
        //            {
        //                _userNames.Add(node["UserName"].InnerText);
        //            }

        //        }
        //    }
        //}
        #endregion
    }
}
