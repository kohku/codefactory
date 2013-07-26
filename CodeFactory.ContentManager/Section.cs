using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Utilities;
using System.ComponentModel;
using CodeFactory.Web;
using System.Web;
using CodeFactory.Web.Storage;

namespace CodeFactory.ContentManager
{
    public class Section : CodeFactory.Web.Core.BusinessBase<Section, Guid>, ISection
    {
        private object syncRoot = new object();
        public static readonly string Root = new string(new char[] {
            System.IO.Path.DirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar});

        private string _name;
        private string _slug;
        private Guid? _parentId;
        private int _index;
        private bool _isVisible;
        private string _keywords;
        private List<ISection> _childs;
        private List<IPage> _pages;
        private Dictionary<string, Guid> _roles;
        private List<UploadedFile> _files;

        public Section()
            : base(Guid.NewGuid())
        {
        }

        public Section(Guid id)
            : base(id)
        {
        }

        #region Section Members

        public string Name
        {
            get { return this._name; }
            set
            {
                if (value.Contains(System.IO.Path.DirectorySeparatorChar.ToString()))
                    throw new ArgumentException(
                        ResourceStringLoader.GetResourceString("invalid_character",
                        System.IO.Path.DirectorySeparatorChar.ToString()));

                if (this._name != value)
                {
                    this.OnPropertyChanging("Name");
                    this._name = value;
                    this.MarkChanged("Name");
                }
            }
        }

        public string Slug
        {
            get
            {
                return this._slug;
            }
            set
            {
                if (this._slug != value)
                {
                    this.OnPropertyChanging("Slug");
                    this._slug = value;
                    this.MarkChanged("Slug");
                }
            }
        }

        public string AbsoluteSlug
        {
            get
            {
                return this.Parent != null ? 
                    string.Format("{0}{1}", UrlPath.AppendTrailingSlash(this.Parent.AbsoluteSlug), this.Slug) :
                    this.Slug;
            }
        }

        public ISection Parent
        {
            get
            {
                if (_parentId.HasValue)
                    return Section.Load(_parentId.Value);

                return null;
            }
            set
            {
                if (value != null)
                {
                    this.OnPropertyChanging("Parent");
                    _parentId = value.ID;
                    this.MarkChanged("Parent");
                }
            }
        }

        protected override void ValidationRules()
        {
        }

        public int Index
        {
            get { return _index; }
            set
            {
                if (this._index != value)
                {
                    this.OnPropertyChanging("Index");
                    this._index = value;
                    this.MarkChanged("Index");
                }
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (this._isVisible != value)
                {
                    this.OnPropertyChanging("IsVisible");
                    this._isVisible = value;
                    this.MarkChanged("IsVisible");
                }
            }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get 
            {
                var query = from r in this.Roles
                            where r.Value == this.ID
                            select r.Key;

                return new List<string>(query);
            }
        }

        public Dictionary<string, Guid> Roles
        {
            get
            {
                lock (syncRoot)
                {
                    if (_roles != null)
                        return _roles;

                    _roles = new Dictionary<string, Guid>();

                    ISection s = this.Parent;

                    if (s is Section)
                        foreach (KeyValuePair<string, Guid> role in _roles.Concat(((Section)s).Roles))
                            if (!_roles.ContainsKey(role.Key))
                                _roles.Add(role.Key, role.Value);

                    foreach (string role in ContentManagementService.GetRoles(this))
                        _roles.Add(role, this.ID);

                }

                return _roles;
            }
        }

        #endregion

        #region IPublishable Members

        string CodeFactory.Web.Core.IPublishable<Guid>.Author
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.LastUpdatedBy
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Description
        {
            get { return null; }
            set { }
        }

        public string Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Content
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Title
        {
            get { return this.Name; }
            set { }
        }

        #endregion

        #region Section Members

        public string Path
        {
            get
            {
                return this.Parent != null ? 
                    System.IO.Path.Combine(this.Parent.Path, this.Name) :
                    string.Format("{0}{1}", Section.Root, this.Name);
            }
        }

        public List<ISection> Childs
        {
            get 
            {
                lock (syncRoot)
                {
                    if (_childs != null)
                        return _childs;

                    _childs = ContentManagementService.GetChildSections(this.ID);
                }

                return _childs; 
            }
        }

        public List<IPage> Pages
        {
            get
            {
                lock (syncRoot)
                {
                    if (_pages != null)
                        return _pages;

                    _pages = ContentManagementService.GetSectionPages(this.ID);
                }

                return _pages;
            }
        }

        #endregion

        public string RelativeLink
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Slug))
                    return string.Format("{0}{1}/default.aspx", Utils.RelativeWebRoot, this.AbsoluteSlug);

                if (!this.ID.Equals(Guid.Empty))
                    return string.Format("{0}aspx?id={1}", Utils.RelativeWebRoot, this.ID);

                return Utils.RelativeWebRoot;
            }
        }

        public Uri AbsoluteLink
        {
            get { return new Uri(VirtualPathUtility.ToAbsolute(this.RelativeLink, HttpContext.Current.Request.ApplicationPath)); }
        }

        #region Data Access

        protected override Section DataSelect(Guid id)
        {
            ISection s = ContentManagementService.GetSection(id);

            return s is Section ? (Section)s : null;
        }

        protected override void DataUpdate()
        {
            ContentManagementService.UpdateSection(this);

            ContentManagementService.UpdateRoles(this);
        }

        protected override void DataInsert()
        {
            ContentManagementService.InsertSection(this);

            ContentManagementService.UpdateRoles(this);
        }

        protected override void DataDelete()
        {
            ContentManagementService.DeleteSection(this);

            List<ISection> sections = ContentManagementService.GetChildSections(this.ID);

            foreach (ISection section in sections)
            {
                Delete();
                Save();
            }

            //List<Page> pages = ContentManagementService.GetChildPages(this.ID);

            //foreach (Page page in pages)
            //{
            //    page.Delete();
            //    page.Save();
            //}
        }

        #endregion

        #region Operations

        public static event EventHandler<CancelEventArgs> AddingChild;

        protected virtual void OnAddingChild(ISection child, CancelEventArgs e)
        {
            if (AddingChild != null)
                AddingChild(child, e);
        }

        public static event EventHandler<EventArgs> ChildAdded;

        protected virtual void OnChildAdded(ISection child)
        {
            if (ChildAdded != null)
                ChildAdded(child, EventArgs.Empty);
        }

        public void AddChild(ISection child)
        {
            if (child == null)
                throw new ArgumentNullException("item");

            lock (this.Childs)
            {
                if (this.Childs.Contains(child))
                    return;

                CancelEventArgs e = new CancelEventArgs();
                OnAddingChild(child, e);

                if (!e.Cancel)
                {
                    child.Parent = this;
                    this.Childs.Add(child);

                    MarkChanged("Childs");
                    OnChildAdded(child);
                }
            }
        }

        /// <summary>
        /// Occurs before a item is removed.
        /// </summary>
        public static event EventHandler<CancelEventArgs> RemovingChild;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="item">File.</param>
        /// <param name="e">Cancel event argument.</param>
        protected virtual void OnRemovingChild(ISection child, CancelEventArgs e)
        {
            if (RemovingChild != null)
                RemovingChild(child, e);
        }

        /// <summary>
        /// Occurs when a item is removed.
        /// </summary>
        public static event EventHandler<EventArgs> ChildRemoved;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnChildRemoved(ISection child)
        {
            if (ChildRemoved != null)
                ChildRemoved(child, EventArgs.Empty);
        }

        public void RemoveChild(ISection child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            lock (this.Childs)
            {
                if (!this.Childs.Contains(child))
                    return;

                CancelEventArgs e = new CancelEventArgs();
                OnRemovingChild(child, e);

                if (!e.Cancel)
                {
                    OnChildRemoved(child);
                    if (child is Section)
                        ((Section)child).Delete();

                    MarkChanged("Childs");
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public List<UploadedFile> Files
        {
            [System.Diagnostics.DebuggerStepThrough]
            get 
            {
                lock (syncRoot)
                {
                    if (_files != null)
                        return _files;

                    _files = UploadStorageService.GetFiles(this.ID);
                }

                return _files;
            }
        }

        public void AddFile(UploadedFile file)
        {
            CancelEventArgs e = new CancelEventArgs();

            OnAddingFile(file, e);

            if (!e.Cancel)
            {
                file.Parent = this;
                this.Files.Add(file);
                MarkChanged("Files");

                OnFileAdded(file);
            }
        }

        public void RemoveFile(UploadedFile file)
        {
            CancelEventArgs e = new CancelEventArgs();

            if (!e.Cancel)
            {
                if (!this.Files.Remove(file))
                    return;

                MarkChanged("Files");

                OnFileRemoved(file);
            }
        }

        public void ClearFiles()
        {
            foreach (UploadedFile file in this.Files)
                RemoveFile(file);

            MarkChanged("Files");
        }

        public static event EventHandler<EventArgs> AddingFile;

        protected virtual void OnAddingFile(UploadedFile file, CancelEventArgs e)
        {
            if (AddingFile != null)
                AddingFile(file, e);
        }

        public static event EventHandler<EventArgs> FileAdded;

        protected virtual void OnFileAdded(UploadedFile file)
        {
            if (FileAdded != null)
                FileAdded(file, EventArgs.Empty);
        }

        public static event EventHandler<EventArgs> RemovingFile;

        protected virtual void OnRemovingFile(UploadedFile file, CancelEventArgs e)
        {
            if (RemovingFile != null)
                RemovingFile(file, e);
        }

        public static event EventHandler<EventArgs> FileRemoved;

        protected virtual void OnFileRemoved(UploadedFile file)
        {
            if (FileRemoved != null)
                FileRemoved(file, EventArgs.Empty);
        }

        #endregion

        public override string ToString()
        {
            return this.Path;
        }

        public static Section ConvertFrom(LinqSection section)
        {
            if (section == null)
                return null;

            Section s = new Section(section.ID)
            {
                Name = section.Name,
                _parentId = section.ParentID,
                Index = section.Index,
                Slug = section.Slug,
                DateCreated = section.DateCreated,
                LastUpdated = section.LastUpdated,
                IsVisible = section.IsVisible,
                Keywords = section.Keywords
            };

            if (!s.IsValid)
                throw new InvalidOperationException("Page does not satisfy validation rules");

            return s;
        }
    }
}
