using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using CodeFactory.Gallery.Core.Providers;
using CodeFactory.Web.Storage;
using CodeFactory.Web.Core;

namespace CodeFactory.Gallery.Core
{
    [Serializable]
    public class Gallery : BusinessBase<Gallery, Guid>, IComparable<Gallery>, IGallery
    {
        private string _applicationName;
        private string _author;
        private string _content;
        private string _description;
        private bool _isVisible;
        private string _keywords;
        private string _lastUpdatedBy;
        private string _slug;
        private string _title;
        private GalleryStatus _status = GalleryStatus.Design;

        public Gallery()
            : this(Guid.NewGuid())
        {
        }

        public Gallery(Guid id)
            : base(id)
        {
            _comments = new List<Comment>();
            _files = new List<UploadedFile>();
            _users = new List<string>();
        }

        #region IPublishable Members


        [XmlIgnore()]
        public Uri AbsoluteLink
        {
            get { return new Uri(RelativeLink); }
        }

        public string Author
        {
            get { return this._author; }
            set
            {
                if (this._author != value)
                {
                    this.OnPropertyChanging("Author");
                    MarkChanged("Author");
                }
                this._author = value;
            }

        }

        public string Content
        {
            get { return this._content; }
            set 
            {
                if (this._content != value)
                {
                    this.OnPropertyChanging("Content");
                    MarkChanged("Content");
                }
                this._content = value; 
            }
        }

        /// <summary>
        /// Gets or sets the description of the instance.
        /// </summary>
        public string Description
        {
            get { return this._description; }
            set
            {
                if (this._description != value)
                {
                    this.OnPropertyChanging("Description");
                    MarkChanged("Description");
                }
                this._description = value;
            }
        }

        public bool IsVisible
        {
            get { return this._isVisible; }
            set
            {
                if (this._isVisible != value)
                {
                    this.OnPropertyChanging("IsVisible");
                    MarkChanged("IsVisible");
                }
                this._isVisible = value;
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
                    MarkChanged("Keywords");
                }
                this._keywords = value;
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
                    MarkChanged("LastUpdatedBy");
                }
                this._lastUpdatedBy = value;
            }
        }

        public string RelativeLink
        {
            get
            {
                return string.Format("{0}?guid={1}",
              GalleryManagementService.Settings.Galleries.PageUrl, this.ID);
            }
            set { }
        }

        public string Slug
        {
            get { return this._slug; }
            set
            {
                if (this._slug != value)
                {
                    this.OnPropertyChanging("Slug");
                    MarkChanged("Slug");
                }
                this._slug = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the instance.
        /// </summary>
        public string Title
        {
            get { return this._title; }
            set
            {
                if (this._title != value)
                {
                    this.OnPropertyChanging("Title"); 
                    MarkChanged("Title");
                }
                this._title = value;
            }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return new List<string>(); }
        }

        #endregion

        #region IGallery Members

        public string ApplicationName
        {
            get { return this._applicationName; }
            set
            {
                if (this._applicationName != value)
                {
                    this.OnPropertyChanging("ApplicationName");
                    MarkChanged("ApplicationName");
                }
                this._applicationName = value;
            }
        }

        private readonly List<Comment> _comments;

        [XmlIgnore()]
        public List<Comment> Comments
        {
            get { return _comments; }
        }

        private readonly List<UploadedFile> _files;

        [XmlIgnore()]
        public List<UploadedFile> Files
        {
            get { return _files; }
        }

        public GalleryStatus Status
        {
            get { return this._status; }
            set
            {
                if (this._status != value)
                {
                    this.OnPropertyChanging("Status");
                    MarkChanged("Status");
                }
                this._status = value;
            }
        }

        private readonly List<string> _users;

        [XmlIgnore()]
        public List<string> Users
        {
            get { return _users; }
        }

        #endregion

        /// <summary>
        /// Gets an unsorted list of all galleries.
        /// </summary>
        public static List<Gallery> Projects
        {
            get
            {
                return GalleryManagementService.GetGalleries();
            }
        }

        /// <summary>
        /// Search a gallery that matches the id, and returns the first occurrence.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Gallery GetGallery(Guid id)
        {
            return Projects.Find(delegate(Gallery match)
            {
                return match.ID == id;
            });
        }

        /// <summary>
        /// Reinforces the business rules by adding additional rules to the
        /// broken rules collection.
        /// </summary>
        protected override void ValidationRules()
        {
            AddRule("Title", "El proyecto no cuenta con Título.", string.IsNullOrEmpty(Title));
            //AddRule("Files", "El proyecto no cuenta con Master graphic.", Files.Count <= 0);
        }

        protected override Gallery DataSelect(Guid id)
        {
            return GalleryManagementService.SelectGallery(id);
        }

        protected override void DataUpdate()
        {
            GalleryManagementService.UpdateGallery(this,
                this.ChangedProperties.Contains("Comments"),
                this.ChangedProperties.Contains("Users"),
                this.ChangedProperties.Contains("Files"));
        }

        protected override void DataInsert()
        {
            GalleryManagementService.InsertGallery(this);
        }

        protected override void DataDelete()
        {
            foreach (UploadedFile file in Files)
            {
                file.Delete();
                file.Save();
            }

            GalleryManagementService.DeleteGallery(this);
        }

        public override void MarkOld(){
            foreach (UploadedFile file in Files)
                file.MarkOld();

            base.MarkOld();
        }

        #region IComparable<Project> Members

        public int CompareTo(Gallery other)
        {
            return Title.CompareTo(other.Title);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs before a new comment is added.
        /// </summary>
        public static event EventHandler<CancelEventArgs> AddingComment;
        /// <summary>
        /// Raises the event in a safe way
        /// </summary>
        protected virtual void OnAddingComment(Comment comment, CancelEventArgs e)
        {
            if (AddingComment != null)
                AddingComment(comment, e);
        }

        /// <summary>
        /// Occurs when a comment is added.
        /// </summary>
        public static event EventHandler<EventArgs> CommentAdded;
        /// <summary>
        /// Raises the event in a safe way
        /// </summary>
        protected virtual void OnCommentAdded(Comment comment)
        {
            if (CommentAdded != null)
                CommentAdded(comment, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs before comment is removed.
        /// </summary>
        public static event EventHandler<CancelEventArgs> RemovingComment;
        /// <summary>
        /// Raises the event in a safe way
        /// </summary>
        protected virtual void OnRemovingComment(Comment comment, CancelEventArgs e)
        {
            if (RemovingComment != null)
                RemovingComment(comment, e);
        }

        /// <summary>
        /// Occurs when a comment has been removed.
        /// </summary>
        public static event EventHandler<EventArgs> CommentRemoved;
        /// <summary>
        /// Raises the event in a safe way
        /// </summary>
        protected virtual void OnCommentRemoved(Comment comment)
        {
            if (CommentRemoved != null)
                CommentRemoved(comment, EventArgs.Empty);
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

        public static event EventHandler<EventArgs> AddingUser;

        protected virtual void OnAddingUser(string user, CancelEventArgs e)
        {
            if (AddingUser != null)
                AddingUser(user, e);
        }

        public static event EventHandler<EventArgs> UserAdded;

        protected virtual void OnUserAdded(string user)
        {
            if (UserAdded != null)
                UserAdded(user, EventArgs.Empty);
        }

        public static event EventHandler<EventArgs> RemovingUser;

        protected virtual void OnRemovingUser(string user, CancelEventArgs e)
        {
            if (RemovingUser != null)
                RemovingUser(user, e);
        }

        public static event EventHandler<EventArgs> UserRemoved;

        protected virtual void OnUserRemoved(string user)
        {
            if (UserRemoved != null)
                UserRemoved(user, EventArgs.Empty);
        }

#endregion

        public override string ToString()
        {
            return Title;
        }

        public void AddComment(Comment item)
        {
            Comment comment = Comments.Find(delegate(Comment match)
            {
                return match.DateCreated.Equals(item.DateCreated);
            });

            if (comment != null)
                return;

            CancelEventArgs e = new CancelEventArgs();

            OnAddingComment(item, e);

            if (!e.Cancel)
            {
                item.Parent = this;
                Comments.Add(item);

                // Sorting items
                Comments.Sort(delegate(Comment x, Comment y)
                {
                    return y.CompareTo(x);
                });

                MarkChanged("Comments");

                OnCommentAdded(item);
            }
        }

        public void UpdateComment(Comment item)
        {
            int affectedRows = Comments.RemoveAll(delegate(Comment match)
            {
                return match.DateCreated.Equals(item.DateCreated);
            });

            if (affectedRows > 0)
                AddComment(item);
        }

        public void RemoveComment(Comment item)
        {
            Comment comment = Comments.Find(delegate(Comment match)
            {
                return match.DateCreated.Equals(item.DateCreated);
            });

            if (comment == null)
                return;

            CancelEventArgs e = new CancelEventArgs();

            if (!e.Cancel)
            {
                if (!Comments.Remove(comment))
                    return;

                // There's no need to re-sort.
                MarkChanged("Comments");

                OnCommentRemoved(comment);
            }
        }

        public void ClearComments()
        {
            foreach (Comment item in Comments)
                RemoveComment(item);

            MarkChanged("Comments");
        }

        public void AddFile(UploadedFile file)
        {
            CancelEventArgs e = new CancelEventArgs();

            OnAddingFile(file, e);

            if (!e.Cancel)
            {
                file.Parent = this;
                Files.Add(file);
                MarkChanged("Files");

                OnFileAdded(file);
            }
        }

        public void RemoveFile(UploadedFile file)
        {
            CancelEventArgs e = new CancelEventArgs();

            if (!e.Cancel)
            {
                if (!Files.Remove(file))
                    return;

                MarkChanged("Files"); 

                OnFileRemoved(file);
            }
        }

        public void ClearFiles()
        {
            foreach (UploadedFile file in Files)
                RemoveFile(file);
        
            MarkChanged("Files");
        }

        public void AddUser(string user)
        {
            if (Users.Contains(user))
                return;

            CancelEventArgs e = new CancelEventArgs();

            OnAddingUser(user, e);

            if (!e.Cancel)
            {
                Users.Add(user);

                MarkChanged("Users");

                OnUserAdded(user);
            }
        }

        public void RemoveUser(string user)
        {
            if (!Users.Contains(user))
                return;

            CancelEventArgs e = new CancelEventArgs();

            if (!e.Cancel)
            {
                if (!Users.Remove(user))
                    return;

                MarkChanged("Users");

                OnUserRemoved(user);
            }
        }

        public void ClearUsers()
        {
            foreach (string user in Users)
                RemoveUser(user);

            MarkChanged("Users");
        }
    }

    public enum GalleryStatus
    {
        Design,
        Changes,
        Authorized
    }
}
