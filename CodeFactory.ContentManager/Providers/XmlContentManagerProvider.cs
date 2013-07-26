using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Security.Permissions;
using System.Web;
using System.Xml;
using System.Globalization;
using System.Threading;

namespace CodeFactory.ContentManager.Providers
{
    public class XmlContentManagerProvider : ContentManagementProvider
    {
        private bool _isInitialized;
        private string _xmlFileName;

        private object syncRoot = new object();

        private ReaderWriterLockSlim _protectionLock;

        private Dictionary<Guid, LinqCategory> _categories;
        private Dictionary<Guid, LinqSection> _sections;
        private Dictionary<Guid, LinqPage> _pages;
        private Dictionary<Guid, LinqModule> _modules;
        private NameValueCollection _roles;

        public XmlContentManagerProvider()
        {
            _protectionLock = new ReaderWriterLockSlim();
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (config == null)
                return;

            // Initialize _xmlFileName and make sure the path is app-relative
            _xmlFileName = config["xmlFileName"];

            if (string.IsNullOrEmpty(_xmlFileName))
                throw new ProviderException("Data store file is not specified");

            if (HttpContext.Current != null)
            {
                if (!VirtualPathUtility.IsAppRelative(_xmlFileName))
                    throw new ArgumentException("xmlFileName must be app-relative");

                string fullyQualifiedPath = VirtualPathUtility.Combine(
                    VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath),
                    _xmlFileName);

                _xmlFileName = HostingEnvironment.MapPath(fullyQualifiedPath);
            }

            config.Remove("xmlFileName");

            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, _xmlFileName);
            permission.Demand();

            if (config.Count > 0)
                throw new ProviderException(string.Format("Unknown config attribute '{0}'", config.GetKey(0)));

            _isInitialized = true;
        }

        private void ReadContentManagerStore()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Provider has not been initialized");

            if (_categories == null)
            {
                lock (syncRoot)
                {
                    if (_categories == null)
                    {
                        _categories = new Dictionary<Guid, LinqCategory>();
                        _sections = new Dictionary<Guid, LinqSection>();
                        _pages = new Dictionary<Guid, LinqPage>();
                        _roles = new NameValueCollection();
                        _modules = new Dictionary<Guid, LinqModule>();

                        XmlDocument doc = new XmlDocument();
                        doc.Load(_xmlFileName);

                        #region Categories

                        XmlNodeList nodes = doc.GetElementsByTagName("Category");

                        foreach (XmlNode node in nodes)
                        {
                            LinqCategory category = null;

                            Guid id = new Guid(node["ID"].InnerText);

                            if (_categories.ContainsKey(id))
                            {
                                category = _categories[id];
                            }
                            else
                            {
                                category = new LinqCategory(id);
                                _categories.Add(id, category);
                            }

                            category.Name = node["Name"].InnerText;

                            if (!string.IsNullOrEmpty(node["DateCreated"].InnerText))
                                category.DateCreated = DateTime.Parse(node["DateCreated"].InnerText);
                            if (!string.IsNullOrEmpty(node["LastUpdated"].InnerText))
                                category.LastUpdated = DateTime.Parse(node["LastUpdated"].InnerText);

                            if (!string.IsNullOrEmpty(node["ParentID"].InnerText))
                                category.ParentID = new Guid(node["ParentID"].InnerText);
                        }

                        #endregion

                        #region Sections

                        nodes = doc.GetElementsByTagName("Section");

                        foreach (XmlNode node in nodes)
                        {
                            LinqSection section = null;

                            Guid id = new Guid(node["ID"].InnerText);

                            if (_sections.ContainsKey(id))
                            {
                                section = _sections[id];
                            }
                            else
                            {
                                section = new LinqSection(id);
                                _sections.Add(id, section);
                            }

                            section.Name = node["Name"].InnerText;
                            section.Index = Int32.Parse(node["Index"].InnerText);
                            section.Slug = node["Slug"].InnerText;
                            if (!string.IsNullOrEmpty(node["DateCreated"].InnerText))
                                section.DateCreated = DateTime.Parse(node["DateCreated"].InnerText);
                            if (!string.IsNullOrEmpty(node["LastUpdated"].InnerText))
                                section.LastUpdated = DateTime.Parse(node["LastUpdated"].InnerText);
                            section.IsVisible = bool.Parse(node["IsVisible"].InnerText);

                            if (!string.IsNullOrEmpty(node["ParentID"].InnerText))
                                section.ParentID = new Guid(node["ParentID"].InnerText);

                            XmlNodeList roles = node.SelectNodes("./Roles/Role");

                            foreach (XmlNode role in roles)
                                _roles.Add(section.ID.ToString(), role.InnerText);
                        }

                        #endregion

                        #region Pages

                        nodes = doc.GetElementsByTagName("Page");

                        foreach (XmlNode node in nodes)
                        {
                            LinqPage page = null;

                            Guid id = new Guid(node["ID"].InnerText);

                            if (_pages.ContainsKey(id))
                            {
                                page = _pages[id];
                            }
                            else
                            {
                                page = new LinqPage(id);
                                _pages.Add(id, page);
                            }

                            page.Title = node["Title"].InnerText;
                            page.Slug = node["Slug"].InnerText;
                            page.Description = node["Description"].InnerText;
                            page.Keywords = node["Keywords"].InnerText;
                            page.Layout = node["Layout"].InnerText;
                            if (!string.IsNullOrEmpty(node["DateCreated"].InnerText))
                                page.DateCreated = DateTime.Parse(node["DateCreated"].InnerText);
                            if (!string.IsNullOrEmpty(node["LastUpdated"].InnerText))
                                page.LastUpdated = DateTime.Parse(node["LastUpdated"].InnerText);
                            page.IsVisible = bool.Parse(node["IsVisible"].InnerText);
                            page.Author = node["Author"].InnerText;
                            page.LastUpdatedBy = node["LastUpdatedBy"].InnerText;

                            if (!string.IsNullOrEmpty(node["ParentID"].InnerText))
                                page.ParentID = new Guid(node["ParentID"].InnerText);

                            if (!string.IsNullOrEmpty(node["SectionID"].InnerText))
                                page.SectionID = new Guid(node["SectionID"].InnerText);

                            XmlNodeList roles = node.SelectNodes("./Roles/Role");

                            foreach (XmlNode role in roles)
                                _roles.Add(page.ID.ToString(), role.InnerText);
                        }

                        #endregion
                    }
                }
            }
        }

        #region Category Repository

        public override ICategory GetCategory(Guid id)
        {
            ReadContentManagerStore();

            var query = from c in _categories
                        where c.Key == id
                        select c.Value;

            _protectionLock.EnterReadLock();

            try
            {
                return Category.ConvertFrom(query.FirstOrDefault());
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        public override void UpdateCategory(ICategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterWriteLock();

            try
            {
                doc.Load(_xmlFileName);

                foreach (XmlNode node in doc.GetElementsByTagName("Category"))
                {
                    if (node["ID"].InnerText.Equals(category.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        node["Name"].InnerText = category.Name;
                        if (category.Parent != null)
                            node["ParentID"].InnerText = category.Parent.ID.ToString();
                        node["DateCreated"].InnerText = category.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        if (category.LastUpdated.HasValue)
                            node["LastUpdated"].InnerText = category.LastUpdated.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        doc.Save(_xmlFileName);
                        _categories[category.ID] = new LinqCategory(category.ID)
                        {
                            Name = category.Name,
                            ParentID = category.Parent != null ? category.Parent.ID : default(Nullable<Guid>),
                            DateCreated = category.DateCreated,
                            LastUpdated = category.LastUpdated
                        };
                        break;
                    }
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override void InsertCategory(ICategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterWriteLock();

            try
            {
                doc.Load(_xmlFileName);

                XmlNode xmlCategoryRoot = doc.CreateElement("Category");
                XmlNode xmlID = doc.CreateElement("ID");
                XmlNode xmlName = doc.CreateElement("Name");
                XmlNode xmlParentID = doc.CreateElement("ParentID");
                XmlNode xmlDateCreated = doc.CreateElement("DateCreated");
                XmlNode xmlLastUpdated = doc.CreateElement("LastUpdated");

                xmlID.InnerText = category.ID.ToString();
                xmlName.InnerText = category.Name;
                xmlParentID.InnerText = category.Parent != null ? category.Parent.ID.ToString() : null;
                xmlDateCreated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                xmlLastUpdated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                xmlCategoryRoot.AppendChild(xmlID);
                xmlCategoryRoot.AppendChild(xmlName);
                xmlCategoryRoot.AppendChild(xmlParentID);
                xmlCategoryRoot.AppendChild(xmlDateCreated);
                xmlCategoryRoot.AppendChild(xmlLastUpdated);

                XmlNode categoryRoot = doc.SelectSingleNode("//ContentManager/Categories");

                if (categoryRoot == null)
                    throw new InvalidOperationException("There is no category node within catalog");

                categoryRoot.AppendChild(xmlCategoryRoot);

                if (_categories.ContainsKey(category.ID))
                    return;

                doc.Save(_xmlFileName);
                _categories.Add(category.ID, new LinqCategory(category.ID)
                    {
                        Name = category.Name,
                        ParentID = category.Parent != null ? category.Parent.ID : default(Nullable<Guid>),
                        DateCreated = category.DateCreated,
                        LastUpdated = category.LastUpdated
                    });
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override void DeleteCategory(ICategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterWriteLock();

            try
            {
                doc.Load(_xmlFileName);

                foreach (XmlNode node in doc.GetElementsByTagName("Category"))
                {
                    if (node["ID"].InnerText.Equals(category.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        XmlNode categoryRoot = doc.SelectSingleNode("//ContentManager/Categories");

                        if (categoryRoot == null)
                            throw new InvalidOperationException("There is not categories node within catalog");

                        categoryRoot.RemoveChild(node);
                        doc.Save(_xmlFileName);
                        _categories.Remove(category.ID);
                        break;
                    }
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override ICategory GetCategory(string path)
        {
            string[] names = path.Split(
                    new string[] { System.IO.Path.DirectorySeparatorChar.ToString() },
                    StringSplitOptions.RemoveEmptyEntries);

            if (names.Length == 0)
                return null;

            string name2Find = names[names.Length - 1];

            int totalCount;

            foreach (Category match in GetCategories(null, name2Find, null, int.MaxValue, 0, out totalCount))
            {
                if (match.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                    return match;
            }

            return null;
        }

        public override List<ICategory> GetChildCategories(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return GetCategories(null, null, parentId, pageSize, pageIndex, out totalCount);
        }

        public override List<ICategory> GetCategories(Guid? id, string name, Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;

            ReadContentManagerStore();

            List<ICategory> results = new List<ICategory>();

            var query = from c in _categories
                        select c.Value;

            if (id.HasValue)
                query = from c in query
                        where c.ID == id.Value
                        select c;

            if (!string.IsNullOrEmpty(name))
                query = from c in query
                        where c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                        select c;

            if (parentId.HasValue)
            {
                if (!parentId.Value.Equals(Guid.Empty))
                    query = from c in query
                            where c.ParentID == parentId.Value
                            select c;
                else
                    query = from c in query
                            where c.ParentID == null
                            select c;
            }

            _protectionLock.EnterReadLock();

            try
            {
                totalCount = query.Count();

                foreach (LinqCategory item in query.Skip(pageSize * pageIndex).Take(pageSize))
                    results.Add(Category.ConvertFrom(item));
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            return results;
        }

        #endregion

        #region Section Repository

        public override ISection GetSection(Guid id)
        {
            ReadContentManagerStore();

            var query = from s in _sections
                        where s.Key == id
                        select s.Value;

            _protectionLock.EnterReadLock();

            try
            {
                return Section.ConvertFrom(query.FirstOrDefault());
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        public override void UpdateSection(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterReadLock();

            try
            {
                doc.Load(_xmlFileName);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            foreach (XmlNode node in doc.GetElementsByTagName("Section"))
            {
                if (node["ID"].InnerText.Equals(section.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    node["Name"].InnerText = section.Name;
                    node["Index"].InnerText = section.Index.ToString();
                    node["Slug"].InnerText = section.Slug;
                    node["Keywords"].InnerText = section.Keywords;
                    node["DateCreated"].InnerText = section.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (section.LastUpdated.HasValue)
                        node["LastUpdated"].InnerText = section.LastUpdated.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    node["IsVisible"].InnerText = section.IsVisible.ToString();
                    if (section.Parent != null)
                        node["ParentID"].InnerText = section.Parent.ID.ToString();

                    node["Roles"].RemoveAll();

                    foreach (string rol in section.Roles)
                    {
                        XmlNode role = doc.CreateElement("Rol");
                        role.InnerText = rol;
                        node["Roles"].AppendChild(role);
                    }

                    _protectionLock.EnterWriteLock();

                    try
                    {
                        if (!_sections.ContainsKey(section.ID))
                            return;

                        doc.Save(_xmlFileName);
                        _sections[section.ID] = new LinqSection(section.ID)
                        {
                            DateCreated = section.DateCreated,
                            Index = section.Index,
                            IsVisible = section.IsVisible,
                            LastUpdated = section.LastUpdated,
                            Name = section.Name,
                            ParentID = section.Parent != null ? section.Parent.ID : default(Nullable<Guid>),
                            Slug = section.Slug
                        };

                        if (_roles.GetValues(section.ID.ToString()) != null)
                            _roles.Remove(section.ID.ToString());

                        foreach (string rol in section.Roles)
                            _roles.Add(section.ID.ToString(), rol);

                    }
                    finally
                    {
                        _protectionLock.ExitWriteLock();
                    }
                    break;
                }
            }
        }

        public override void InsertSection(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterReadLock();

            try
            {
                doc.Load(_xmlFileName);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            XmlNode xmlSection = doc.CreateElement("Section");
            XmlNode xmlID = doc.CreateElement("ID");
            XmlNode xmlName = doc.CreateElement("Name");
            XmlNode xmlIndex = doc.CreateElement("Index");
            XmlNode xmlSlug = doc.CreateElement("Slug");
            XmlNode xmlKeywords = doc.CreateElement("Keywords");
            XmlNode xmlParentID = doc.CreateElement("ParentID");
            XmlNode xmlDateCreated = doc.CreateElement("DateCreated");
            XmlNode xmlLastUpdated = doc.CreateElement("LastUpdated");
            XmlNode xmlIsVisible = doc.CreateElement("IsVisible");
            XmlNode xmlRoles = doc.CreateElement("Roles");

            xmlID.InnerText = section.ID.ToString();
            xmlName.InnerText = section.Name;

            xmlDateCreated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            xmlLastUpdated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            xmlParentID.InnerText = section.Parent != null ? section.Parent.ID.ToString() : null;

            xmlSection.AppendChild(xmlID);
            xmlSection.AppendChild(xmlName);
            xmlSection.AppendChild(xmlIndex);
            xmlSection.AppendChild(xmlSlug);
            xmlSection.AppendChild(xmlKeywords);
            xmlSection.AppendChild(xmlParentID);
            xmlSection.AppendChild(xmlDateCreated);
            xmlSection.AppendChild(xmlLastUpdated);
            xmlSection.AppendChild(xmlIsVisible);
            xmlSection.AppendChild(xmlRoles);

            foreach (string rol in section.Roles)
            {
                XmlNode role = doc.CreateElement("Rol");
                role.InnerText = rol;
                xmlRoles.AppendChild(role);
            }

            XmlNode sectionRoot = doc.SelectSingleNode("//ContentManager/Sections");

            if (sectionRoot == null)
                throw new InvalidOperationException("There is no sections node within catalog");

            sectionRoot.AppendChild(xmlSection);

            _protectionLock.EnterWriteLock();

            try
            {
                if (_sections.ContainsKey(section.ID))
                    return;

                doc.Save(_xmlFileName);

                _sections.Add(section.ID, new LinqSection(section.ID)
                {
                    DateCreated = section.DateCreated,
                    Keywords = section.Keywords,
                    Index = section.Index,
                    IsVisible = section.IsVisible,
                    LastUpdated = section.LastUpdated,
                    Name = section.Name,
                    ParentID = section.Parent != null ? section.Parent.ID : default(Nullable<Guid>),
                    Slug = section.Slug
                });

                if (_roles.GetValues(section.ID.ToString()) != null)
                    _roles.Remove(section.ID.ToString());

                foreach (string rol in section.Roles)
                    _roles.Add(section.ID.ToString(), rol);

            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override void DeleteSection(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterWriteLock();

            try
            {
                doc.Load(_xmlFileName);

                foreach (XmlNode node in doc.GetElementsByTagName("Section"))
                {
                    if (node["ID"].InnerText.Equals(section.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        XmlNode sectionRoot = doc.SelectSingleNode("//ContentManager/Sections");

                        if (sectionRoot == null)
                            throw new InvalidOperationException("There is not section node within catalog");

                        sectionRoot.RemoveChild(node);
                        doc.Save(_xmlFileName);
                        _sections.Remove(section.ID);
                        _roles.Remove(section.ID.ToString());
                        break;
                    }
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override ISection GetSection(string path)
        {
            string[] names = path.Split(
                    new string[] { System.IO.Path.DirectorySeparatorChar.ToString() },
                    StringSplitOptions.RemoveEmptyEntries);

            if (names.Length == 0)
                return null;

            string name2Find = names[names.Length - 1];

            int totalCount;

            foreach (Section match in GetSections(null, name2Find, null, true, null, int.MaxValue, 0, out totalCount))
            {
                if (match.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                    return match;
            }

            return null;
        }

        public override List<ISection> GetChildSections(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return GetSections(null, null, null, true, parentId, pageSize, pageIndex, out totalCount);
        }

        public override List<ISection> GetSections(Guid? id, string name, string slug, bool? isVisible, Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;

            ReadContentManagerStore();

            List<ISection> results = new List<ISection>();

            var query = from s in _sections
                        select s.Value;

            if (id.HasValue)
                query = from s in query
                        where s.ID == id.Value
                        select s;

            if (!string.IsNullOrEmpty(name))
                query = from s in query
                        where s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                        select s;

            if (!string.IsNullOrEmpty(slug))
                query = from s in query
                        where s.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase)
                        select s;

            if (isVisible.HasValue)
                query = from s in query
                        where s.IsVisible == isVisible
                        select s;

            if (parentId.HasValue)
            {
                if (!parentId.Value.Equals(Guid.Empty))
                    query = from s in query
                            where s.ParentID == parentId.Value
                            select s;
                else
                    query = from s in query
                            where s.ParentID == null
                            select s;
            }

            _protectionLock.EnterReadLock();

            try
            {
                totalCount = query.Count();

                foreach (LinqSection item in query.Skip(pageSize * pageIndex).Take(pageSize))
                    results.Add(Section.ConvertFrom(item));
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            return results;
        }

        public override List<string> GetRoles(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            ReadContentManagerStore();

            _protectionLock.EnterReadLock();

            try
            {
                string[] roles = _roles.GetValues(section.ID.ToString());

                return roles != null ? new List<string>(roles) : new List<string>();
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        public override void UpdateRoles(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            UpdateSection(section);
        }

        #endregion

        #region Page Repository

        public override IPage GetPage(Guid id)
        {
            ReadContentManagerStore();

            var query = from p in _pages
                        where p.Key == id
                        select p.Value;

            _protectionLock.EnterReadLock();

            try
            {
                return Page.ConvertFrom(query.FirstOrDefault());
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        public override void InsertPage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterReadLock();

            try
            {
                doc.Load(_xmlFileName);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            XmlNode xmlPage = doc.CreateElement("Page");
            XmlNode xmlID = doc.CreateElement("ID");
            XmlNode xmlTitle = doc.CreateElement("Title");
            XmlNode xmlSlug = doc.CreateElement("Slug");
            XmlNode xmlDescription = doc.CreateElement("Description");
            XmlNode xmlKeywords = doc.CreateElement("Keywords");
            XmlNode xmlLayout = doc.CreateElement("Layout");
            XmlNode xmlSectionID = doc.CreateElement("SectionID");
            XmlNode xmlParentID = doc.CreateElement("ParentID");
            XmlNode xmlDateCreated = doc.CreateElement("DateCreated");
            XmlNode xmlLastUpdated = doc.CreateElement("LastUpdated");
            XmlNode xmlAuthor = doc.CreateElement("Author");
            XmlNode xmlLastUpdatedBy = doc.CreateElement("LastUpdatedBy");
            XmlNode xmlIsVisible = doc.CreateElement("IsVisible");
            XmlNode xmlRoles = doc.CreateElement("Roles");

            xmlID.InnerText = page.ID.ToString();
            xmlTitle.InnerText = page.Title;
            xmlSlug.InnerText = page.Slug;
            xmlDescription.InnerText = page.Description;
            xmlKeywords.InnerText = page.Keywords;
            xmlLayout.InnerText = page.Layout;
            xmlSectionID.InnerText = page.Section != null ? page.Section.ID.ToString() : null;
            xmlParentID.InnerText = page.Parent != null ? page.Parent.ID.ToString() : null;
            xmlDateCreated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            xmlLastUpdated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            xmlAuthor.InnerText = page.Author;
            xmlLastUpdatedBy.InnerText = page.LastUpdatedBy;
            xmlIsVisible.InnerText = page.IsVisible.ToString();

            xmlPage.AppendChild(xmlID);
            xmlPage.AppendChild(xmlTitle);
            xmlPage.AppendChild(xmlSlug);
            xmlPage.AppendChild(xmlDescription);
            xmlPage.AppendChild(xmlKeywords);
            xmlPage.AppendChild(xmlLayout);
            xmlPage.AppendChild(xmlSectionID);
            xmlPage.AppendChild(xmlParentID);
            xmlPage.AppendChild(xmlDateCreated);
            xmlPage.AppendChild(xmlLastUpdated);
            xmlPage.AppendChild(xmlAuthor);
            xmlPage.AppendChild(xmlLastUpdatedBy);
            xmlPage.AppendChild(xmlIsVisible);
            xmlPage.AppendChild(xmlRoles);

            foreach (string rol in page.Roles)
            {
                XmlNode role = doc.CreateElement("Rol");
                role.InnerText = rol;
                xmlRoles.AppendChild(role);
            }

            XmlNode pageRoot = doc.SelectSingleNode("//ContentManager/Pages");

            if (pageRoot == null)
                throw new InvalidOperationException("There is no pages node within catalog");

            pageRoot.AppendChild(xmlPage);

            _protectionLock.EnterWriteLock();

            try
            {
                if (_pages.ContainsKey(page.ID))
                    return;

                doc.Save(_xmlFileName);
                _pages.Add(page.ID, new LinqPage(page.ID)
                {
                    Author = page.Author,
                    DateCreated = page.DateCreated,
                    Description = page.Description,
                    IsVisible = page.IsVisible,
                    Keywords = page.Keywords,
                    LastUpdated = page.LastUpdated,
                    LastUpdatedBy = page.LastUpdatedBy,
                    Layout = page.Layout,
                    ParentID = page.Parent != null ? page.Parent.ID : default(Nullable<Guid>),
                    SectionID = page.Section != null ? page.Section.ID : default(Nullable<Guid>),
                    Slug = page.Slug,
                    Title = page.Title
                });

                if (_roles.GetValues(page.ID.ToString()) != null)
                    _roles.Remove(page.ID.ToString());

                foreach (string rol in page.Roles)
                    _roles.Add(page.ID.ToString(), rol);
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override void UpdatePage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterReadLock();

            try
            {
                doc.Load(_xmlFileName);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            foreach (XmlNode node in doc.GetElementsByTagName("Page"))
            {
                if (node["ID"].InnerText.Equals(page.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    node["Title"].InnerText = page.Title;
                    node["Slug"].InnerText = page.Slug;
                    node["Description"].InnerText = page.Description;
                    node["Keywords"].InnerText = page.Keywords;
                    node["Layout"].InnerText = page.Layout;
                    if (page.Section != null)
                        node["SectionID"].InnerText = page.Section.ID.ToString();
                    if (page.Parent != null)
                        node["ParentID"].InnerText = page.Parent.ID.ToString();
                    node["DateCreated"].InnerText = page.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (page.LastUpdated.HasValue)
                        node["LastUpdated"].InnerText = page.LastUpdated.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    node["Author"].InnerText = page.Author;
                    node["LastUpdatedBy"].InnerText = page.LastUpdatedBy;
                    node["IsVisible"].InnerText = page.IsVisible.ToString();

                    node["Roles"].RemoveAll();

                    foreach (string rol in page.Roles)
                    {
                        XmlNode role = doc.CreateElement("Rol");
                        role.InnerText = rol;
                        node["Roles"].AppendChild(role);
                    }

                    _protectionLock.EnterWriteLock();

                    try
                    {
                        if (!_pages.ContainsKey(page.ID))
                            return;

                        doc.Save(_xmlFileName);
                        _pages[page.ID] = new LinqPage(page.ID)
                        {
                            Author = page.Author,
                            DateCreated = page.DateCreated,
                            Description = page.Description,
                            IsVisible = page.IsVisible,
                            Keywords = page.Keywords,
                            LastUpdated = page.LastUpdated,
                            LastUpdatedBy = page.LastUpdatedBy,
                            Layout = page.Layout,
                            ParentID = page.Parent != null ? page.Parent.ID : default(Nullable<Guid>),
                            SectionID = page.Section != null ? page.Section.ID : default(Nullable<Guid>),
                            Slug = page.Slug,
                            Title = page.Title
                        };

                        if (_roles.GetValues(page.ID.ToString()) != null)
                            _roles.Remove(page.ID.ToString());

                        foreach (string rol in page.Roles)
                            _roles.Add(page.ID.ToString(), rol);
                    }
                    finally
                    {
                        _protectionLock.ExitWriteLock();
                    }
                    break;
                }
            }
        }

        public override void DeletePage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterWriteLock();

            try
            {
                doc.Load(_xmlFileName);

                foreach (XmlNode node in doc.GetElementsByTagName("Page"))
                {
                    if (node["ID"].InnerText.Equals(page.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        XmlNode pagesRoot = doc.SelectSingleNode("//ContentManager/Pages");

                        if (pagesRoot == null)
                            throw new InvalidOperationException("There is no pages node within catalog");

                        pagesRoot.RemoveChild(node);
                        doc.Save(_xmlFileName);
                        _pages.Remove(page.ID);
                        _roles.Remove(page.ID.ToString());
                        break;
                    }
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override List<IPage> GetChildPages(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return GetPages(null, parentId, null, null, true, pageSize, pageIndex, out totalCount);
        }

        public override List<IPage> GetPages(Guid? id, Guid? parentId, Guid? sectionId, string slug, bool? isVisible, int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;

            ReadContentManagerStore();

            List<IPage> results = new List<IPage>();

            var query = from p in _pages
                        select p.Value;

            if (id.HasValue)
                query = from p in query
                        where p.ID == id.Value
                        select p;

            if (parentId.HasValue)
            {
                if (!parentId.Value.Equals(Guid.Empty))
                    query = from s in query
                            where s.ParentID == parentId.Value
                            select s;
                else
                    query = from s in query
                            where s.ParentID == null
                            select s;
            }

            if (sectionId.HasValue)
            {
                if (!sectionId.Value.Equals(Guid.Empty))
                    query = from p in query
                            where p.SectionID == sectionId.Value
                            select p;
                else
                    query = from p in query
                            where p.SectionID == null
                            select p;
            }

            if (!string.IsNullOrEmpty(slug))
                query = from p in query
                        where p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase)
                        select p;

            if (isVisible.HasValue)
                query = from p in query
                        where p.IsVisible == isVisible
                        select p;

            _protectionLock.EnterReadLock();

            try
            {
                totalCount = query.Count();

                foreach (LinqPage page in query.Skip(pageSize * pageIndex).Take(pageSize))
                    results.Add(Page.ConvertFrom(page));
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            return results;
        }

        public override List<string> GetRoles(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            ReadContentManagerStore();

            _protectionLock.EnterReadLock();

            try
            {
                string[] roles = _roles.GetValues(page.ID.ToString());

                return roles != null ? new List<string>(roles) : new List<string>();
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        public override void UpdateRoles(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            UpdatePage(page);
        }

        public override List<IPage> GetSectionPages(Guid sectionId)
        {
            int totalCount;

            return GetPages(null, null, sectionId, null, true, int.MaxValue, 0, out totalCount);
        }

        #endregion

        #region Module Repository

        public override IModule GetModule(Guid id)
        {
            ReadContentManagerStore();

            var query = from m in _modules
                        where m.Key == id
                        select m.Value;

            _protectionLock.EnterReadLock();

            try
            {
                return Module.ConvertFrom(query.FirstOrDefault());
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }
        }

        public override void InsertModule(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterReadLock();

            try
            {
                doc.Load(_xmlFileName);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            XmlNode xmlModule = doc.CreateElement("Module");
            XmlNode xmlID = doc.CreateElement("ID");
            XmlNode xmlTitle = doc.CreateElement("Title");
            XmlNode xmlContentRaw = doc.CreateElement("ContentRaw");
            XmlNode xmlDateCreated = doc.CreateElement("DateCreated");
            XmlNode xmlLastUpdated = doc.CreateElement("LastUpdated");

            xmlID.InnerText = module.ID.ToString();
            xmlTitle.InnerText = module.Title;
            xmlContentRaw.InnerText = module.ContentRaw.ToString();
            xmlDateCreated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            xmlLastUpdated.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            xmlModule.AppendChild(xmlID);
            xmlModule.AppendChild(xmlTitle);
            xmlModule.AppendChild(xmlContentRaw);
            xmlModule.AppendChild(xmlDateCreated);
            xmlModule.AppendChild(xmlLastUpdated);

            XmlNode moduleRoot = doc.SelectSingleNode("//ContentManager/Modules");

            if (moduleRoot == null)
                throw new InvalidOperationException("There is no modules node within catalog");

            moduleRoot.AppendChild(xmlModule);

            _protectionLock.EnterWriteLock();

            try
            {
                if (_modules.ContainsKey(module.ID))
                    return;

                doc.Save(_xmlFileName);
                _modules.Add(module.ID, new LinqModule(module.ID)
                {
                    ContentRaw = module.ContentRaw,
                    DateCreated = module.DateCreated,
                    LastUpdated = module.LastUpdated,
                    Title = module.Title
                });
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        public override void UpdateModule(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterReadLock();

            try
            {
                doc.Load(_xmlFileName);
            }
            finally
            {
                _protectionLock.ExitReadLock();
            }

            foreach (XmlNode node in doc.GetElementsByTagName("Module"))
            {
                if (node["ID"].InnerText.Equals(module.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    node["Title"].InnerText = module.Title;
                    node["ContentRaw"].InnerText = module.ContentRaw.ToString();
                    node["DateCreated"].InnerText = module.DateCreated.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (module.LastUpdated.HasValue)
                        node["LastUpdated"].InnerText = module.LastUpdated.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    _protectionLock.EnterWriteLock();

                    try
                    {
                        if (!_modules.ContainsKey(module.ID))
                            return;

                        doc.Save(_xmlFileName);
                        _modules[module.ID] = new LinqModule(module.ID)
                        {
                            ContentRaw = module.ContentRaw,
                            DateCreated = module.DateCreated,
                            LastUpdated = module.LastUpdated,
                            Title = module.Title
                        };
                    }
                    finally
                    {
                        _protectionLock.ExitWriteLock();
                    }
                    break;
                }
            }
        }

        public override void DeleteModule(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            ReadContentManagerStore();

            XmlDocument doc = new XmlDocument();

            _protectionLock.EnterWriteLock();

            try
            {
                doc.Load(_xmlFileName);

                foreach (XmlNode node in doc.GetElementsByTagName("Module"))
                {
                    if (node["ID"].InnerText.Equals(module.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        XmlNode modulesRoot = doc.SelectSingleNode("//ContentManager/Modules");

                        if (modulesRoot == null)
                            throw new InvalidOperationException("There is no modules node within catalog");

                        modulesRoot.RemoveChild(node);
                        doc.Save(_xmlFileName);
                        _modules.Remove(module.ID);
                        _roles.Remove(module.ID.ToString());
                        break;
                    }
                }
            }
            finally
            {
                _protectionLock.ExitWriteLock();
            }
        }

        #endregion
    }
}
