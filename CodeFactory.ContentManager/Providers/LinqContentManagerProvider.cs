using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using CodeFactory.ContentManager.Settings;
using System.Web.Hosting;
using System.Data.Linq;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager.Providers
{
    public class LinqContentManagerProvider : ContentManagementProvider
    {
        private string _connectionStringName;

        public string ConnectionStringName
        {
            get { return _connectionStringName; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (config == null)
                return;

            if (!string.IsNullOrEmpty(config["connectionStringName"]))
            {
                this._connectionStringName = config["connectionStringName"];
                config.Remove("connectionStringName");
            }

            if (config.Count > 0)
                throw new ProviderException(string.Format("Unknown config attribute '{0}'", config.GetKey(0)));
        }

        #region ICategoryRepository

        public override ICategory GetCategory(Guid id)
        {
            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                try
                {
                    Table<LinqCategory> categories = db.GetTable<LinqCategory>();

                    return categories.FirstOrDefault(category => category.ID.Equals(id));
                }

                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The category with id '{0}' was not found.", id));
                    return null;
                }
            }
        }

        public override void UpdateCategory(ICategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            ICategory c = GetCategory(category.ID);

            if (c == null)
                throw new ChangeConflictException("Category not found or deleted");

            LinqCategory item = new LinqCategory()
            {
                ApplicationName = this.ApplicationName,
                DateCreated = category.DateCreated,
                ID = category.ID,
                LastUpdated = c.LastUpdated,
                Name = category.Name,
                ParentID = category.Parent != null ? category.Parent.ID : default(Nullable<Guid>)
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqCategory> categories = db.GetTable<LinqCategory>();

                // Assume that "page" has been sent by client.
                // Attach with "true" to the change tracker to consider the entity modified
                // and it can be checked for optimistic concurrency because
                // it has a column that is marked with "RowVersion" attribute
                categories.Attach(item, true);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override void InsertCategory(ICategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            LinqCategory c = new LinqCategory()
            {
                ApplicationName = this.ApplicationName,
                DateCreated = category.DateCreated,
                ID = category.ID,
                LastUpdated = category.LastUpdated,
                Name = category.Name,
                ParentID = category.Parent != null ? category.Parent.ID : default(Nullable<Guid>)
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqCategory> categories = db.GetTable<LinqCategory>();

                categories.InsertOnSubmit(c);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override void DeleteCategory(ICategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            ICategory c = GetCategory(category.ID);

            if (c == null)
                return;

            LinqCategory item = new LinqCategory()
            {
                ApplicationName = this.ApplicationName,
                DateCreated = category.DateCreated,
                ID = category.ID,
                LastUpdated = category.LastUpdated,
                Name = category.Name,
                ParentID = category.Parent != null ? category.Parent.ID : default(Nullable<Guid>)
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqCategory> categories = db.GetTable<LinqCategory>();

                // Set to false to indicate that the object does not have a timestamp (RowVersion)
                categories.Attach(item, true);

                categories.DeleteOnSubmit(item);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override ICategory GetCategory(string path)
        {
            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString))
            {
                string[] names = path.Split(
                    new string[] { System.IO.Path.DirectorySeparatorChar.ToString() },
                    StringSplitOptions.RemoveEmptyEntries);

                if (names.Length == 0)
                    return null;

                string name2Find = names[names.Length - 1];

                Table<LinqCategory> categories = db.GetTable<LinqCategory>();

                List<Category> search = new List<Category>();

                try
                {
                    var coincidences = from c in categories
                                       where c.Name.Equals(name2Find, StringComparison.OrdinalIgnoreCase)
                                       select c.ID;

                    foreach (Guid id in coincidences)
                        search.Add(Category.Load(id));

                    return search.Find(match => match.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The category with path '{0}' was not found.", path));
                    return null;
                }
            }
        }

        public override List<ICategory> GetCategories(Guid? id, string name, Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            List<ICategory> items = new List<ICategory>();

            totalCount = 0;

            int? _totalCount = null;

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                try
                {
                    var query = db.GetCategories(this.ApplicationName, id, name, parentId,
                        pageIndex * pageSize, (pageIndex * pageSize) + pageSize, ref _totalCount);

                    foreach (GetCategoriesResult item in query)
                        items.Add(Category.Load(item.ID));

                    if (_totalCount.HasValue)
                        totalCount = _totalCount.Value;

                }
                catch (ArgumentNullException)
                {
                    return new List<ICategory>();
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The category with id '{0}' was not found.", id));
                    return new List<ICategory>();
                }
            }

            return items;
        }

        public override List<ICategory> GetChildCategories(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return GetCategories(null, null, parentId, pageSize, pageIndex, out totalCount);
        }

        #endregion

        #region ISectionRepository

        public override ISection GetSection(Guid id)
        {
            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                try
                {
                    Table<LinqSection> sections = db.GetTable<LinqSection>();

                    return sections.FirstOrDefault(section => section.ID.Equals(id));
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The category with id '{0}' was not found.", id));
                    return null;
                }
            }
        }

        public override void UpdateSection(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            ISection s = GetSection(section.ID);

            if (s == null)
                throw new ChangeConflictException("Section not found or deleted");

            LinqSection item = new LinqSection()
            {
                ApplicationName = this.ApplicationName,
                DateCreated = section.DateCreated,
                Keywords = section.Keywords,
                ID = section.ID,
                Index = section.Index,
                IsVisible = section.IsVisible,
                LastUpdated = s.LastUpdated,
                Name = section.Name,
                ParentID = section.Parent != null ? section.Parent.ID : default(Nullable<Guid>),
                Slug = section.Slug
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqSection> sections = db.GetTable<LinqSection>();

                // Assume that "page" has been sent by client.
                // Attach with "true" to the change tracker to consider the entity modified
                // and it can be checked for optimistic concurrency because
                // it has a column that is marked with "RowVersion" attribute
                sections.Attach(item, true);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override void InsertSection(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            LinqSection s = new LinqSection()
            {
                ApplicationName = this.ApplicationName,
                DateCreated = section.DateCreated,
                Keywords = section.Keywords,
                ID = section.ID,
                Index = section.Index,
                IsVisible = section.IsVisible,
                LastUpdated = section.LastUpdated,
                Name = section.Name,
                ParentID = section.Parent != null ? section.Parent.ID : default(Nullable<Guid>),
                Slug = section.Slug
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqSection> sections = db.GetTable<LinqSection>();

                sections.InsertOnSubmit(s);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override void DeleteSection(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            DeleteRoles(section);

            ISection s = GetSection(section.ID);

            if (s == null)
                return;

            LinqSection item = new LinqSection()
            {
                ApplicationName = this.ApplicationName,
                DateCreated = section.DateCreated,
                Keywords = section.Keywords,
                ID = section.ID,
                Index = section.Index,
                IsVisible = section.IsVisible,
                LastUpdated = s.LastUpdated,
                Name = section.Name,
                ParentID = section.Parent != null ? section.Parent.ID : default(Nullable<Guid>),
                Slug = section.Slug
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqSection> sections = db.GetTable<LinqSection>();

                // Set to false to indicate that the object does not have a timestamp (RowVersion)
                sections.Attach(item, true);

                sections.DeleteOnSubmit(item);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
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

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString))
            {
                try
                {
                    Table<LinqSection> sections = db.GetTable<LinqSection>();

                    var coincidences = from s in sections
                                       where s.Name == name2Find
                                       select s.ID;

                    List<Section> search = new List<Section>();

                    foreach (Guid id in coincidences)
                        search.Add(Section.Load(id));

                    return search.Find(match => match.Path.Equals(path));
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The section with path '{0}' was not found.", path));
                    return null;
                }
            }
        }

        public override List<ISection> GetChildSections(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            List<ISection> children = new List<ISection>();

            totalCount = 0;

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                try
                {
                    Table<LinqSection> sections = db.GetTable<LinqSection>();

                    var query = from s in sections
                                where s.ApplicationName == this.ApplicationName
                                where s.ParentID.GetValueOrDefault() == parentId.GetValueOrDefault()
                                orderby s.Index ascending
                                select s.ID;

                    totalCount = query.Count();

                    foreach (Guid id in query.Skip(pageSize * pageIndex).Take(pageSize))
                        children.Add(Section.Load(id));
                }
                catch (ArgumentNullException)
                {
                    return new List<ISection>();
                }
                catch (InvalidOperationException)
                {
                    return new List<ISection>();
                }
            }

            return children;
        }

        public override List<ISection> GetSections(Guid? id, string name, string slug, bool? isVisible, Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            List<ISection> items = new List<ISection>();

            totalCount = 0;

            int? _totalCount = null;

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                try
                {
                    var query = db.GetSections(this.ApplicationName, id, name, slug, isVisible, parentId,
                        pageIndex * pageSize, (pageIndex * pageSize) + pageSize, ref _totalCount);

                    foreach (GetSectionsResult item in query)
                        items.Add(Section.Load(item.ID));

                    if (_totalCount.HasValue)
                        totalCount = _totalCount.Value;
                }
                catch (ArgumentNullException)
                {
                    return new List<ISection>();
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The category with id '{0}' was not found.", id));
                    return new List<ISection>();
                }
            }

            return items;
        }

        public override List<string> GetRoles(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            List<string> roles = new List<string>();

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<SectionsInRole> roleset = db.GetTable<SectionsInRole>();

                var query = from r in roleset
                            where r.sectionId == section.ID
                            select r;

                foreach (SectionsInRole role in query)
                    roles.Add(role.roleName);

                return roles;
            }
        }

        public override void UpdateRoles(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            DeleteRoles(section);

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<SectionsInRole> roleset = db.GetTable<SectionsInRole>();

                var roles = from r in section.Roles
                            select new SectionsInRole()
                            {
                                sectionId = section.ID,
                                roleName = r
                            };

                roleset.InsertAllOnSubmit(roles);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        private void DeleteRoles(ISection section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<SectionsInRole> roleset = db.GetTable<SectionsInRole>();

                var roles = from r in roleset
                            where r.sectionId == section.ID
                            select r;

                roleset.DeleteAllOnSubmit(roles);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        #endregion

        #region Page

        public override IPage GetPage(Guid id)
        {
            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqPage> pages = db.GetTable<LinqPage>();

                try
                {
                    return Page.ConvertFrom(pages.FirstOrDefault(page => page.ID.Equals(id)));
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The page with id '{0}' was not found.", id));
                    return null;
                }
            }
        }

        public override void InsertPage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            LinqPage p = new LinqPage()
            {
                ApplicationName = this.ApplicationName,
                Author = page.Author,
                DateCreated = page.DateCreated,
                Description = page.Description,
                ID = page.ID,
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

            using(ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqPage> pages = db.GetTable<LinqPage>();

                pages.InsertOnSubmit(p);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override void UpdatePage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            IPage p = GetPage(page.ID);

            if (p == null)
                throw new ChangeConflictException("Page not found or deleted");

            LinqPage item = new LinqPage()
            {
                ApplicationName = this.ApplicationName,
                Author = page.Author,
                DateCreated = page.DateCreated,
                Description = page.Description,
                ID = page.ID,
                IsVisible = page.IsVisible,
                Keywords = page.Keywords,
                LastUpdated = p.LastUpdated,
                LastUpdatedBy = page.LastUpdatedBy,
                Layout = page.Layout,
                ParentID = page.Parent != null ? page.Parent.ID : default(Nullable<Guid>),
                SectionID = page.Section != null ? page.Section.ID : default(Nullable<Guid>),
                Slug = page.Slug,
                Title = page.Title
            };

            using(ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqPage> pages = db.GetTable<LinqPage>();

                // Assume that "page" has been sent by client.
                // Attach with "true" to the change tracker to consider the entity modified
                // and it can be checked for optimistic concurrency because
                // it has a column that is marked with "RowVersion" attribute
                pages.Attach(item, true);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                
                }
            }
        }

        public override void DeletePage(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            DeleteRoles(page);

            IPage p = GetPage(page.ID);

            if (p == null)
                return;

            LinqPage item = new LinqPage()
            {
                ApplicationName = this.ApplicationName,
                Author = page.Author,
                DateCreated = page.DateCreated,
                Description = page.Description,
                ID = page.ID,
                IsVisible = page.IsVisible,
                Keywords = page.Keywords,
                LastUpdated = p.LastUpdated,
                LastUpdatedBy = page.LastUpdatedBy,
                Layout = page.Layout,
                ParentID = page.Parent != null ? page.Parent.ID : default(Nullable<Guid>),
                Slug = page.Slug,
                Title = page.Title
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqPage> pages = db.GetTable<LinqPage>();

                // Set to false to indicate that the object does not have a timestamp (RowVersion)
                pages.Attach(item, true);

                pages.DeleteOnSubmit(item);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override List<IPage> GetChildPages(Guid? parentId, int pageSize, int pageIndex, out int totalCount)
        {
            return GetPages(null, parentId, null, null, true, pageSize, pageIndex, out totalCount);
        }

        public override List<IPage> GetPages(Guid? id, Guid? parentId, Guid? sectionId, string slug, bool? isVisible, int pageSize, int pageIndex, out int totalCount)
        {
            List<IPage> results = new List<IPage>();

            totalCount = 0;

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                try
                {
                    Table<LinqPage> pages = db.GetTable<LinqPage>();

                    var query = from p in pages
                                where p.ApplicationName == this.ApplicationName
                                select p;

                    if (id.HasValue)
                        query = from p in query
                                where p.ID == id.Value
                                select p;

                    if (parentId.HasValue)
                        query = from p in query
                                where p.ParentID == parentId
                                select p;

                    if (sectionId.HasValue)
                        query = from p in query
                                where p.SectionID == sectionId.Value
                                select p;

                    //if (!string.IsNullOrEmpty(slug))
                    //{
                    //    List<string> slugs = new List<string>(slug.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));

                    //    if (slugs.Count > 0)
                    //    {
                    //        query = from p in query
                    //                where p.Slug.ToLower() == slugs[slugs.Count - 1].ToLower()
                    //                select p;
                    //    }
                    //}
                    if (!string.IsNullOrEmpty(slug))
                        query = from p in query
                                where p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase)
                                select p;


                    if (isVisible.HasValue)
                        query = from p in query
                                where p.IsVisible == isVisible.Value
                                select p;

                    totalCount = query.Count();

                    foreach (LinqPage p in query.Skip(pageSize * pageIndex).Take(pageSize))
                        results.Add(Page.ConvertFrom(p));
                }
                catch (ArgumentNullException)
                {
                    return new List<IPage>();
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The page with parentId '{0}' was not found.", parentId));
                    return new List<IPage>();
                }
            }

            return results;
        }

        public override List<string> GetRoles(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            List<string> roles = new List<string>();

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<PagesInRole> roleset = db.GetTable<PagesInRole>();

                var query = from r in roleset
                            where r.pageId == page.ID
                            select r;

                foreach (PagesInRole role in query)
                    roles.Add(role.roleName);
            }

            return roles;
        }

        public override void UpdateRoles(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            DeleteRoles(page);

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<PagesInRole> roleset = db.GetTable<PagesInRole>();

                var roles = from r in page.Roles
                            select new PagesInRole()
                            {
                                pageId = page.ID,
                                roleName = r
                            };

                roleset.InsertAllOnSubmit(roles);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        private void DeleteRoles(IPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<PagesInRole> roleset = db.GetTable<PagesInRole>();

                var roles = from r in roleset
                            where r.pageId == page.ID
                            select r;

                roleset.DeleteAllOnSubmit(roles);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override List<IPage> GetSectionPages(Guid sectionId)
        {
            int totalCount;

            return GetPages(null, null, sectionId, null, true, int.MaxValue, 0, out totalCount);
        }

        #endregion

        #region Module

        public override IModule GetModule(Guid id)
        {
            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqModule> modules = db.GetTable<LinqModule>();

                try
                {
                    return Module.ConvertFrom(modules.FirstOrDefault(module => module.ID.Equals(id)));
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    Trace.WriteLine(string.Format("The module with id '{0}' was not found.", id));
                    return null;
                }
            }
        }

        public override void InsertModule(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            LinqModule m = new LinqModule()
            {
                ApplicationName = this.ApplicationName,
                ContentRaw = module.ContentRaw,
                DateCreated = module.DateCreated,
                ID = module.ID,
                LastUpdated = module.LastUpdated,
                Title = module.Title
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqModule> modules = db.GetTable<LinqModule>();

                modules.InsertOnSubmit(m);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        public override void UpdateModule(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            IModule m = GetModule(module.ID);

            if (m == null)
                throw new ChangeConflictException("Page not found or deleted");

            LinqModule item = new LinqModule()
            {
                ApplicationName = this.ApplicationName,
                ContentRaw = module.ContentRaw,
                DateCreated = module.DateCreated,
                ID = module.ID,
                LastUpdated = m.LastUpdated,
                Title = module.Title
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqModule> modules = db.GetTable<LinqModule>();

                // Assume that "module" has been sent by client.
                // Attach with "true" to the change tracker to consider the entity modified
                // and it can be checked for optimistic concurrency because
                // it has a column that is marked with "RowVersion" attribute
                modules.Attach(item, true);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);

                }
            }
        }

        public override void DeleteModule(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            IModule m = GetModule(module.ID);

            if (m == null)
                return;

            LinqModule item = new LinqModule()
            {
                ApplicationName = this.ApplicationName,
                ContentRaw = module.ContentRaw,
                DateCreated = module.DateCreated,
                ID = module.ID,
                LastUpdated = m.LastUpdated,
                Title = module.Title
            };

            using (ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString))
            {
                Table<LinqModule> modules = db.GetTable<LinqModule>();

                // Set to false to indicate that the object does not have a timestamp (RowVersion)
                modules.Attach(item, true);

                modules.DeleteOnSubmit(item);

                try
                {
                    db.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Trace.TraceError(ex.Message);

                    // All database values overwrite current values.
                    foreach (ObjectChangeConflict occ in db.ChangeConflicts)
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        #endregion

        //#region Publication

        //public override IPublishable<Guid> GetPublication(Guid id)
        //{
        //    try
        //    {
        //        ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //        Table<LinqPublication> publications = db.GetTable<LinqPublication>();

        //        return publications.FirstOrDefault(publication => publication.ID.Equals(id));
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        return null;
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        Trace.WriteLine(string.Format("The publication with id '{0}' was not found.", id));
        //        return null;
        //    }
        //}

        //public override void UpdatePublication(IPublishable<Guid> publication)
        //{
        //    ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //    Table<LinqPublication> publications = db.GetTable<LinqPublication>();

        //    // Assume that "page" has been sent by client.
        //    // Attach with "true" to the change tracker to consider the entity modified
        //    // and it can be checked for optimistic concurrency because
        //    // it has a column that is marked with "RowVersion" attribute
        //    publications.Attach(publication, true);

        //    try
        //    {
        //        db.SubmitChanges(ConflictMode.ContinueOnConflict);
        //    }
        //    catch (ChangeConflictException ex)
        //    {
        //        Trace.TraceError(ex.Message);

        //        // All database values overwrite current values.
        //        foreach (ObjectChangeConflict occ in db.ChangeConflicts)
        //            occ.Resolve(RefreshMode.OverwriteCurrentValues);
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //    }
        //}

        //public override void InsertPublication(IPublishable<Guid> publication)
        //{
        //    publication.ApplicationName = this.ApplicationName;

        //    ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //    Table<LinqPublication> publications = db.GetTable<LinqPublication>();

        //    publications.InsertOnSubmit(publication);

        //    try
        //    {
        //        db.SubmitChanges(ConflictMode.ContinueOnConflict);
        //    }
        //    catch (ChangeConflictException ex)
        //    {
        //        Trace.TraceError(ex.Message);

        //        // All database values overwrite current values.
        //        foreach (ObjectChangeConflict occ in db.ChangeConflicts)
        //            occ.Resolve(RefreshMode.OverwriteCurrentValues);
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //    }
        //}

        //public override void DeletePublication(IPublishable<Guid> publication)
        //{
        //    ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //    Table<LinqPublication> publications = db.GetTable<LinqPublication>();

        //    // Set to false to indicate that the object does not have a timestamp (RowVersion)
        //    publications.Attach(publication, true);

        //    publications.DeleteOnSubmit(publication);

        //    try
        //    {
        //        db.SubmitChanges(ConflictMode.ContinueOnConflict);
        //    }
        //    catch (ChangeConflictException ex)
        //    {
        //        Trace.TraceError(ex.Message);

        //        // All database values overwrite current values.
        //        foreach (ObjectChangeConflict occ in db.ChangeConflicts)
        //            occ.Resolve(RefreshMode.OverwriteCurrentValues);
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //    }
        //}

        //#endregion

        //#region Post

        //public override IPublishable<Guid> GetPost(Guid id)
        //{
        //    try
        //    {
        //        ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //        Table<LinqPost> posts = db.GetTable<LinqPost>();

        //        return posts.FirstOrDefault(post => post.ID.Equals(id));
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        return null;
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        Trace.WriteLine(string.Format("The post with id '{0}' was not found.", id));
        //        return null;
        //    }
        //}

        //public override void UpdatePost(IPublishable<Guid> post)
        //{
        //    ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //    Table<LinqPost> posts = db.GetTable<LinqPost>();

        //    // Assume that "post" has been sent by client.
        //    // Attach with "true" to the change tracker to consider the entity modified
        //    // and it can be checked for optimistic concurrency because
        //    // it has a column that is marked with "RowVersion" attribute
        //    posts.Attach(post, true);

        //    try
        //    {
        //        db.SubmitChanges(ConflictMode.ContinueOnConflict);
        //    }
        //    catch (ChangeConflictException ex)
        //    {
        //        Trace.TraceError(ex.Message);

        //        // All database values overwrite current values.
        //        foreach (ObjectChangeConflict occ in db.ChangeConflicts)
        //            occ.Resolve(RefreshMode.OverwriteCurrentValues);
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //    }
        //}

        //public override void InsertPost(IPublishable<Guid> post)
        //{
        //    post.ApplicationName = this.ApplicationName;

        //    ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //    Table<LinqPost> posts = db.GetTable<LinqPost>();

        //    posts.InsertOnSubmit(post);

        //    try
        //    {
        //        db.SubmitChanges(ConflictMode.ContinueOnConflict);
        //    }
        //    catch (ChangeConflictException ex)
        //    {
        //        Trace.TraceError(ex.Message);

        //        // All database values overwrite current values.
        //        foreach (ObjectChangeConflict occ in db.ChangeConflicts)
        //            occ.Resolve(RefreshMode.OverwriteCurrentValues);
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //    }
        //}

        //public override void DeletePost(IPublishable<Guid> post)
        //{
        //    ContentManagerDataContext db = new ContentManagerDataContext(ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString);

        //    Table<LinqPost> posts = db.GetTable<LinqPost>();

        //    // Set to false to indicate that the object does not have a timestamp (RowVersion)
        //    posts.Attach(post, true);

        //    posts.DeleteOnSubmit(post);

        //    try
        //    {
        //        db.SubmitChanges(ConflictMode.ContinueOnConflict);
        //    }
        //    catch (ChangeConflictException ex)
        //    {
        //        Trace.TraceError(ex.Message);

        //        // All database values overwrite current values.
        //        foreach (ObjectChangeConflict occ in db.ChangeConflicts)
        //            occ.Resolve(RefreshMode.OverwriteCurrentValues);
        //    }
        //    catch (System.Data.Common.DbException ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //    }
        //}

        //#endregion

    }
}
