using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace CodeFactory.ContentManager
{
    [Serializable]
    [Table(Name = "Posts")]
    public class LinqPost : CodeFactory.Web.Core.IPublishable<Guid>
    {
        private string _applicationName;
        private Guid? _parentId;
        private bool _isCommentsEnabled;

        private string _title;
        private string _content;
        private string _slug;
        private string _keywords;
        private DateTime _dateCreated;
        private DateTime? _lastUpdated;
        private string _description;
        private string _author;
        private bool _isVisible;
        private string _lastUpdatedBy;

        private Guid _id;

        public LinqPost()
        {
        }

        public LinqPost(Guid id)
        {
            _id = id;
        }

        [Column(Storage = "_applicationName", DbType = "NVarChar(512)", CanBeNull = false)]
        public string ApplicationName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _applicationName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _applicationName = value; }
        }

        [Column(Storage = "_parentId", DbType = "UniqueIdentifier", CanBeNull = true)]
        public Guid? ParentID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _parentId; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _parentId = value; }
        }

        [Column(Storage = "_isCommentsEnabled", DbType = "Bit NOT NULL", CanBeNull = false)]
        public bool IsCommentsEnabled
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _isCommentsEnabled; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _isCommentsEnabled = value; }
        }

        #region IPublishable<Guid> Members

        [Column(Storage = "_title", DbType = "NVarChar(512)", CanBeNull = true)]
        public string Title
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _title; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _title = value; }
        }

        [Column(Storage = "_content", DbType = "NVarChar(1024) NOT NULL", CanBeNull = true)]
        public string Content
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _content; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _content = value; }
        }

        [Column(Storage = "_slug", DbType = "NVarChar(512) NOT NULL", CanBeNull = true)]
        public string Slug
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _slug; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _slug = value; }
        }

        [Column(Storage = "_keywords", DbType = "NVarChar(1024)", CanBeNull = true)]
        public string Keywords
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _keywords; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _keywords = value; }
        }

        [Column(Storage = "_dateCreated", DbType = "DATETIME NOT NULL", CanBeNull = false, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public DateTime DateCreated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _dateCreated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _dateCreated = value; }
        }

        [Column(Storage = "_lastUpdated", DbType = "DATETIME NOT NULL", CanBeNull = false, IsVersion = true)]
        public DateTime? LastUpdated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _lastUpdated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _lastUpdated = value; }
        }

        [Column(Storage = "_description", DbType = "NVarChar(512)", CanBeNull = true)]
        public string Description
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _description; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _description = value; }
        }

        [Column(Storage = "_author", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Author
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _author; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _author = value; }
        }

        [Column(Storage = "_isVisible", DbType = "Bit NOT NULL", CanBeNull = false)]
        public bool IsVisible
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _isVisible; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _isVisible = value; }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return null; }
        }

        [Column(Storage = "_lastUpdatedBy", DbType = "NVarChar(512) NOT NULL", CanBeNull = true)]
        public string LastUpdatedBy
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _lastUpdatedBy; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _lastUpdatedBy = value; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.RelativeLink
        {
            get { return null; }
        }

        Uri CodeFactory.Web.Core.IPublishable<Guid>.AbsoluteLink
        {
            get { return null; }
        }

        #endregion

        #region IIdentifiable<Guid> Members

        [Column(Storage = "_id", DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _id; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _id = value; }
        }

        #endregion
    }
}
