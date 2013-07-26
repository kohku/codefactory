using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Core;
using CodeFactory.Web.Storage;
using System.ComponentModel;
using CodeFactory.Utilities;
using System.Web;
using CodeFactory.Web;

namespace CodeFactory.Wiki
{
    [Serializable]
    public class Wiki : BusinessBase<Wiki, Guid>, IWiki
    {
        private string _title;
        private string _description;
        private string _content;
        private string _author;
        private bool _isVisible;
        private bool _editable = true;

        private string _editor;
        private string _departmentArea;

        private string _slug;
        private string _category;
        private string _keywords;
        private ReachLevel _reachLevel = ReachLevel.Intranet;
        private string _lastUpdatedBy;

        private List<UploadedFile> _files;

        public Wiki()
            : this(Guid.NewGuid())
        {
            this.ID = Guid.NewGuid();
            _files = new List<UploadedFile>();
        }

        public Wiki(Guid id)
            : base(id)
        {
            this.ID = id;
            _files = new List<UploadedFile>();
        }

        #region IWiki Members

        public void AddFile(CodeFactory.Web.Storage.UploadedFile item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            lock (_files)
            {
                if (_files.Contains(item))
                    return;

                CancelEventArgs e = new CancelEventArgs();
                OnAddingFile(item, e);

                if (!e.Cancel)
                {
                    _files.Add(item);
                    item.Parent = this;

                    MarkChanged("Files");
                    OnFileAdded(item);
                }
            }
        }

        public string Category
        {
            get { return _category; }
            set
            {
                if (_category != value)
                    MarkChanged("Category");
                _category = value;
            }
        }

        public string DepartmentArea
        {
            get { return _departmentArea; }
            set
            {
                if (_departmentArea != value)
                    MarkChanged("DepartmentArea");
                _departmentArea = value;
            }
        }

        public bool Editable
        {
            get { return _editable; }
            set
            {
                if (_editable != value)
                    MarkChanged("Editable");
                _editable = value;
            }
        }

        public string Editor
        {
            get { return _editor; }
            set
            {
                if (_editor != value)
                    MarkChanged("Editor");
                _editor = value;
            }
        }

        public List<CodeFactory.Web.Storage.UploadedFile> Files
        {
            get { return _files; }
        }

        public List<CodeFactory.Web.Storage.UploadedFile> Images
        {
            get
            {
                return this.Files.FindAll(delegate(UploadedFile match)
                {
                    return match.Media == UploadedFile.MediaType.image;
                });
            }
        }

        public string Keywords
        {
            get { return _keywords; }
            set
            {
                if (_keywords != value)
                    MarkChanged("Keywords");
                _keywords = value;
            }
        }

        public ReachLevel ReachLevel
        {
            get { return _reachLevel; }
            set
            {
                if (_reachLevel != value)
                    MarkChanged("ReachLevel");
                _reachLevel = value;
            }
        }

        public void RemoveFile(UploadedFile item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            lock (_files)
            {
                if (!_files.Contains(item))
                    return;

                CancelEventArgs e = new CancelEventArgs();
                OnRemovingFile(item, e);

                if (!e.Cancel)
                {
                    //_files.Remove(item);
                    OnFileRemoved(item);
                    item.Delete();

                    MarkChanged("Files");
                }
            }
        }

        public string Slug
        {
            get { return _slug; }
            set
            {
                if (_slug != value)
                    MarkChanged("Slug");
                _slug = value;
            }
        }

        #endregion

        #region IPublishable<Guid> Members

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                    MarkChanged("Title");
                _title = value;
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                    MarkChanged("Content");
                _content = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                    MarkChanged("Description");
                _description = value;
            }
        }

        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                    MarkChanged("Author");
                _author = value;
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                    MarkChanged("IsVisible");
                _isVisible = value;
            }
        }

        public string LastUpdatedBy
        {
            get { return _lastUpdatedBy; }
            set
            {
                if (_lastUpdatedBy != value)
                    MarkChanged("LastUpdatedBy");
                _lastUpdatedBy = value;
            }
        }

        public string RelativeLink
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Slug))
                    return string.Format("{0}Wiki/{1}.aspx", Utils.RelativeWebRoot, this.Slug);

                return string.Format("{0}Wiki.aspx?id={1}", Utils.RelativeWebRoot, this.ID);
            }
        }

        public Uri AbsoluteLink
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Slug))
                    return new Uri(string.Format("{0}Wiki/{1}.aspx", Utils.AbsoluteWebRoot.ToString(), this.Slug));

                return new Uri(string.Format("{0}Wiki.aspx?id={1}", Utils.AbsoluteWebRoot.ToString(), this.ID));
            }
        }

        #endregion

        protected override void ValidationRules()
        {
            if (string.IsNullOrEmpty(this.Content))
                this.Content = "[Sin contenido]";

            AddRule("Title", ResourceStringLoader.GetResourceString("WIKI_TITLE_REQUIRED"),
                string.IsNullOrEmpty(this.Title));
            AddRule("Content", ResourceStringLoader.GetResourceString("WIKI_CONTENT_REQUIRED"),
                string.IsNullOrEmpty(this.Content));
            AddRule("Category", ResourceStringLoader.GetResourceString("WIKI_CATEGORY_REQUIRED"),
                string.IsNullOrEmpty(this.Category));
            AddRule("Description", ResourceStringLoader.GetResourceString("WIKI_DESCRIPTION_REQUIRED"),
                string.IsNullOrEmpty(this.Description));
            if (!HttpContext.Current.User.IsInRole("Administrator"))
                AddRule("Restricted Slug", ResourceStringLoader.GetResourceString("WIKI_RESTRICTED_SLUG"),
                    this.IsNew && WikiService.RestrictedSlugs.Contains(this.Slug.ToLowerInvariant()));
        }

        protected override Wiki DataSelect(Guid id)
        {
            return (Wiki)WikiService.SelectWiki(id);
        }

        protected override void DataUpdate()
        {
            foreach (UploadedFile item in this.Files)
                item.Save();

            WikiService.UpdateWiki(this);

            for (int i = this.Files.Count - 1; i >= 0; i--)
                if (this.Files[i].IsDeleted)
                    this.Files.RemoveAt(i);
        }

        protected override void DataInsert()
        {
            foreach (UploadedFile item in this.Files)
                item.Save();

            WikiService.InsertWiki(this);
        }

        protected override void DataDelete()
        {
            foreach (UploadedFile item in this.Files)
            {
                item.Delete();
                item.Save();
            }

            WikiService.DeleteWiki(this);
        }

        /// <summary>
        /// Occurs before a new file is added.
        /// </summary>
        public static event EventHandler<CancelEventArgs> AddingFile;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="item">File</param>
        /// <param name="e">Cancel event parameter.</param>
        protected virtual void OnAddingFile(UploadedFile item, CancelEventArgs e)
        {
            if (AddingFile != null)
                AddingFile(item, e);
        }

        /// <summary>
        /// Occurss when a file is added.
        /// </summary>
        public static event EventHandler<EventArgs> FileAdded;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="item">File</param>
        protected virtual void OnFileAdded(UploadedFile item)
        {
            if (FileAdded != null)
                FileAdded(item, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs before a file is removed.
        /// </summary>
        public static event EventHandler<CancelEventArgs> RemovingFile;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="e">Cancel event argument.</param>
        protected virtual void OnRemovingFile(UploadedFile item, CancelEventArgs e)
        {
            if (RemovingFile != null)
                RemovingFile(item, e);
        }

        /// <summary>
        /// Occurs when a file is removed.
        /// </summary>
        public static event EventHandler<EventArgs> FileRemoved;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="file"></param>
        protected virtual void OnFileRemoved(UploadedFile item)
        {
            if (FileRemoved != null)
                FileRemoved(item, EventArgs.Empty);
        }

        public static IWiki GetArticleBySlug(string slug)
        {
            int totalCount = 0;

            List<Guid> articles = WikiService.GetWiki(null, null, null, null, null,
                Utils.RemoveIllegalCharacters(slug), null, null, int.MaxValue, 0, out totalCount);

            // Ok, return first occurrence.
            if (articles.Count > 0)
                return Wiki.Load(articles[0]);

            return null;
        }

        public static IWiki GetRelatedWiki(IWiki wiki)
        {
            string keyword;

            return GetRelatedWiki(wiki, out keyword);
        }

        public static IWiki GetRelatedWiki(IWiki wiki, out string keyword)
        {
            if (wiki == null)
                throw new ArgumentNullException("wiki");

            if (string.IsNullOrEmpty(wiki.Keywords))
            {
                keyword = null;
                return null;
            }

            List<string> keywords = new List<string>(wiki.Keywords.Split(new string[] { ";" },
                StringSplitOptions.RemoveEmptyEntries));

            keyword = keywords[new Random().Next(keywords.Count)];

            return GetRelatedWiki(keyword);
        }

        public static IWiki GetRelatedWiki(string keyword)
        {
            return WikiService.GetRelatedWiki(keyword);
        }

        public static IWiki GetRandomWiki()
        {
            return WikiService.GetRandomWiki();
        }
    }
}
