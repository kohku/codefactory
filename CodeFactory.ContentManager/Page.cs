#define GET_PAGE_ASPX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web;
using System.Web;
using System.ComponentModel;

namespace CodeFactory.ContentManager
{
    public class Page : CodeFactory.Web.Core.BusinessBase<Page, Guid>, IPage
    {
        private object syncRoot = new object();

        private string _title;
        private string _slug;
        private string _description;
        private string _keywords;
        private string _layout;
        private Guid? _parentId;
        private bool _isVisible;
        private string _author;
        private string _lastUpdatedBy;
        private Guid? _sectionId;
        private List<IPage> _childs;
        private Dictionary<string, Guid> _roles;

        public Page()
            : this(Guid.NewGuid())
        {
        }

        public Page(Guid id)
            : base(id)
        {
        }

        #region IPublishable Members

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (this._title != value)
                {
                    this.OnPropertyChanging("Title");
                    this._title = value;
                    this.MarkChanged("Title");
                }
            }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Content
        {
            get { return null; }
            set { }
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

        public string Description
        {
            get { return this._description; }
            set
            {
                if (this._description != value)
                {
                    this.OnPropertyChanging("Description");
                    this._description = value;
                    this.MarkChanged("Description");
                }
            }
        }

        public string Keywords
        {
            get { return this._keywords; }
            set
            {
                if (this._keywords != value)
                {
                    this.OnPropertyChanging("Keywords");
                    this._keywords = value;
                    this.MarkChanged("Keywords");
                }
            }
        }

        /// <summary>
        /// Is published.
        /// </summary>
        public bool IsVisible
        {
            get { return this._isVisible; }
            set
            {
                if (_isVisible != value)
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

                    _roles = new Dictionary<string,Guid>();

                    ISection s = this.Section;

                    if (s is Section)
                        foreach (KeyValuePair<string, Guid> role in _roles.Concat(((Section)s).Roles))
                            if (!_roles.ContainsKey(role.Key))
                                _roles.Add(role.Key, role.Value);

                    IPage p = this.Parent;

                    if (p is Page)
                        foreach (KeyValuePair<string, Guid> role in _roles.Concat(((Page)p).Roles))
                            if (!_roles.ContainsKey(role.Key))
                                _roles.Add(role.Key, role.Value);

                    foreach (string role in ContentManagementService.GetRoles(this))
                        if (!_roles.ContainsKey(role))
                            _roles.Add(role, this.ID);
                }

                return _roles;
            }
        }

        public string Author
        {
            get { return this._author; }
            set
            {
                if (this._author != value)
                {
                    this.OnPropertyChanging("Author");
                    this._author = value;
                    this.MarkChanged("Author");
                }
            }
        }

        public string LastUpdatedBy
        {
            get { return this._lastUpdatedBy; }
            set
            {
                if (this._lastUpdatedBy != value)
                {
                    this.OnPropertyChanging("LastUpdatedBy");
                    this._lastUpdatedBy = value;
                    this.MarkChanged("LastUpdatedBy");
                }
            }
        }

        public string RelativeLink
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Slug))
                    return string.Format("{0}{1}.aspx", Utils.RelativeWebRoot, this.AbsoluteSlug);

#if GET_PAGE_ASPX
                if (!this.ID.Equals(Guid.Empty))
                    return string.Format("{0}getpage.aspx?id={1}", Utils.RelativeWebRoot, this.ID);
#else
                if (!this.ID.Equals(Guid.Empty))
                    return string.Format("{0}page.aspx?id={1}", Utils.RelativeWebRoot, this.ID);
#endif

                return Utils.RelativeWebRoot;
            }
        }

        public Uri AbsoluteLink
        {
            get { return new Uri(VirtualPathUtility.ToAbsolute(this.RelativeLink, HttpContext.Current.Request.ApplicationPath)); }
        }

        #endregion

        #region IPage Members

        public string Layout
        {
            get { return this._layout; }
            set
            {
                if (this._layout != value)
                {
                    this.OnPropertyChanged("Layout");
                    this._layout = value;
                    this.MarkChanged("Layout");
                }
            }
        }

        public ISection Section
        {
            get
            {
                return this._sectionId.HasValue ? CodeFactory.ContentManager.Section.Load(_sectionId.Value) : null;                    
            }
            set
            {
                if (value != null)
                {
                    this.OnPropertyChanging("Section");
                    _sectionId = value.ID;
                    this.MarkChanged("Section");
                }
            }
        }

        public IPage Parent
        {
            get
            {
                return _parentId.HasValue ? Page.Load(_parentId.Value) : null;
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

        public string AbsoluteSlug
        {
            get
            {
                ISection s = this.Section;

                return s is ISection ?
                    string.Format("{0}{1}", UrlPath.AppendTrailingSlash(this.Section.AbsoluteSlug), this.Slug) :
                    this.Slug;
            }
        }

        public List<IPage> Childs
        {
            get
            {
                lock (syncRoot)
                {
                    if (_childs != null)
                        return _childs;

                    _childs = ContentManagementService.GetChildPages(this.ID);
                }

                return _childs;
            }
        }

        #endregion

        #region Validation

        protected override void ValidationRules()
        {
            this.AddRule("Title", "Title must not be null or empty", string.IsNullOrEmpty(this.Title));
            this.AddRule("Layout", "Layout must not be null or empty", string.IsNullOrEmpty(this.Layout));
        }

        #endregion

        #region Data Access

        protected override Page DataSelect(Guid id)
        {
            IPage p = ContentManagementService.GetPage(id);

            return p is Page ? (Page)p : null;
        }

        protected override void DataInsert()
        {
            ContentManagementService.InsertPage(this);

            ContentManagementService.UpdateRoles(this);
        }

        protected override void DataUpdate()
        {
            ContentManagementService.UpdatePage(this);

            ContentManagementService.UpdateRoles(this);
        }

        protected override void DataDelete()
        {
            ContentManagementService.DeletePage(this);
        }

        #endregion

        #region Operations

        public static event EventHandler<CancelEventArgs> AddingChild;

        protected virtual void OnAddingChild(IPage child, CancelEventArgs e)
        {
            if (AddingChild != null)
                AddingChild(child, e);
        }

        public static event EventHandler<EventArgs> ChildAdded;

        protected virtual void OnChildAdded(IPage child)
        {
            if (ChildAdded != null)
                ChildAdded(child, EventArgs.Empty);
        }

        public void AddChild(IPage child)
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
        protected virtual void OnRemovingChild(IPage child, CancelEventArgs e)
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
        protected virtual void OnChildRemoved(IPage child)
        {
            if (ChildRemoved != null)
                ChildRemoved(child, EventArgs.Empty);
        }

        public void RemoveChild(IPage child)
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

        #endregion

        public override string ToString()
        {
            return !string.IsNullOrEmpty(this.Title) ? this.Title : this.ID.ToString();
        }

        public static Page GetPageBySlug(string slug)
        {
            IPage p = ContentManagementService.GetPageBySlug(slug);

            return p is Page ? (Page)p : null;
        }

        public static Page ConvertFrom(LinqPage page)
        {
            if (page == null)
                return default(Page);

            Page p = new Page(page.ID)
            {
                Title = page.Title,
                Slug = page.Slug,
                Description = page.Description,
                Keywords = page.Keywords,
                Layout = page.Layout,
                _sectionId = page.SectionID,
                _parentId = page.ParentID,
                DateCreated = page.DateCreated,
                LastUpdated = page.LastUpdated,
                Author = page.Author,
                LastUpdatedBy = page.LastUpdatedBy,
                IsVisible = page.IsVisible
            };

            if (!p.IsValid)
                throw new InvalidOperationException("Page does not satisfy validation rules");

            return p;
        }
    }
}
