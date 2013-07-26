using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Collections.Specialized;
using CodeFactory.Web.Core;
using System.Xml;
using System.Web.Hosting;

namespace CodeFactory.ContentManager.Providers
{
    public abstract class ContentManagementProvider : ProviderBase, ICategoryRepository, ISectionRepository, IPageRepository, IModuleRepository
        //IPublicationRepository, , IPostRepository
    {
        private string _applicationName;
        private string _settingsFile;

        public string ApplicationName
        {
            get { return _applicationName; }
        }

        public string SettingsFile
        {
            get { return _settingsFile; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (config == null)
                return;

            if (!string.IsNullOrEmpty(config["applicationName"]))
            {
                this._applicationName = config["applicationName"];
                config.Remove("applicationName");
            }
            else
                this._applicationName = HostingEnvironment.ApplicationVirtualPath;

            if (!string.IsNullOrEmpty(config["settingsFile"]))
            {
                this._settingsFile = config["settingsFile"];
                if (System.Web.HttpContext.Current != null && !System.Web.VirtualPathUtility.IsAppRelative(this._settingsFile))
                    throw new ProviderException("settingsFile path must be app relative");

                config.Remove("settingsFile");
            }
        }

        public virtual StringDictionary LoadSettings()
        {
            StringDictionary dic = new StringDictionary();

            string filename = System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.Server.MapPath(this.SettingsFile) :
                this.SettingsFile;

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            foreach (XmlNode settingsNode in doc.SelectSingleNode("ContentManagerSettings").ChildNodes)
            {
                string name = settingsNode.Name;
                string value = settingsNode.InnerText;

                dic.Add(name, value);
            }

            return dic;
        }

        public virtual void SaveSettings(StringDictionary settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            string filename = System.Web.HttpContext.Current.Server.MapPath(this.SettingsFile);

            XmlWriterSettings writerSettings = new XmlWriterSettings();

            writerSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filename, writerSettings))
            {
                writer.WriteStartElement("ContentManagerSettings");

                foreach (string key in settings.Keys)
                    writer.WriteElementString(key, settings[key]);

                writer.WriteEndElement();
            }
        }

        public abstract ICategory GetCategory(Guid id);
        public abstract void UpdateCategory(ICategory category);
        public abstract void InsertCategory(ICategory category);
        public abstract void DeleteCategory(ICategory category);

        public abstract ICategory GetCategory(string path);
        public abstract List<ICategory> GetCategories(Guid? id, string name, Guid? parentId, int pageSize, int pageIndex, out int totalCount);
        public abstract List<ICategory> GetChildCategories(Guid? parentId, int pageSize, int pageIndex, out int totalCount);

        public abstract ISection GetSection(Guid id);
        public abstract void InsertSection(ISection section);
        public abstract void UpdateSection(ISection section);
        public abstract void DeleteSection(ISection section);

        public abstract ISection GetSection(string path);
        public abstract List<ISection> GetSections(Guid? id, string name, string slug, bool? isVisible, Guid? parentId, int pageSize, int pageIndex, out int totalCount);
        public abstract List<ISection> GetChildSections(Guid? parentId, int pageSize, int pageIndex, out int totalCount);

        public abstract List<string> GetRoles(ISection section);
        public abstract void UpdateRoles(ISection section);

        public abstract IPage GetPage(Guid id);
        public abstract void InsertPage(IPage page);
        public abstract void UpdatePage(IPage page);
        public abstract void DeletePage(IPage page);

        public IPage GetPageBySlug(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return null;

            List<string> slugs = new List<string>(slug.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));

            if (slugs.Count == 0)
                return null;

            int totalCount;

            var query = from p in GetPages(null, null, null, slugs[slugs.Count - 1].ToLower(), true, int.MaxValue, 0, out totalCount)
                        select p;

            foreach (Page match in query)
            {
                if (match.AbsoluteSlug.Equals(slug, StringComparison.OrdinalIgnoreCase))
                    return match;
            }

            return null;
        }

        public abstract List<IPage> GetChildPages(Guid? parentId, int pageSize, int pageIndex, out int totalCount);
        public abstract List<IPage> GetPages(Guid? id, Guid? parentId, Guid? sectionId, string slug, bool? isVisible, int pageSize, int pageIndex, out int totalCount);

        public abstract List<string> GetRoles(IPage page);
        public abstract void UpdateRoles(IPage page);

        public abstract List<IPage> GetSectionPages(Guid sectionId);

        public abstract IModule GetModule(Guid id);
        public abstract void InsertModule(IModule module);
        public abstract void UpdateModule(IModule module);
        public abstract void DeleteModule(IModule module);
        
        //public abstract IPublishable<Guid> GetPublication(Guid id);
        //public abstract void UpdatePublication(IPublishable<Guid> publication);
        //public abstract void InsertPublication(IPublishable<Guid> publication);
        //public abstract void DeletePublication(IPublishable<Guid> publication);

        //public abstract IPublishable<Guid> GetPost(Guid id);
        //public abstract void UpdatePost(IPublishable<Guid> post);
        //public abstract void InsertPost(IPublishable<Guid> post);
        //public abstract void DeletePost(IPublishable<Guid> post);

    }
}
