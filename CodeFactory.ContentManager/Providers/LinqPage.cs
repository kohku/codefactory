using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeFactory.ContentManager
{
    [Serializable]
    [Table(Name = "Pages")]
    public class LinqPage : IPage
    {
        private Guid _id;
        private string _title;
        private string _slug;
        private string _description;
        private string _keywords;
        private string _layout;
        private string _applicationName;
        private Guid? _parentId;
        private Guid? _sectionId;
        private bool _isVisible;
        private DateTime _dateCreated;
        private DateTime? _lastUpdated;
        private string _author;
        private string _lastUpdatedBy;

        public LinqPage()
        {
        }

        public LinqPage(Guid id)
        {
            _id = id;
        }

        #region IUniqueIdentifier

        [Column(Storage = "_id", DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._id; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._id = value; }
        }

        #endregion

        #region IPublishable

        string CodeFactory.Web.Core.IPublishable<Guid>.Content
        {
            get { return null; }
            set { }
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

        #region IPage

        [Column(Storage = "_title", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Title
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._title; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._title = value; }
        }

        [Column(Storage = "_slug", DbType = "NVarChar(512) NOT NULL", CanBeNull = true)]
        public string Slug
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._slug; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._slug = value; }
        }

        [Column(Storage = "_description", DbType = "NVarChar(512)", CanBeNull = true)]
        public string Description
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._description; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._description = value; }
        }

        [Column(Storage = "_keywords", DbType = "NVarChar(1024)", CanBeNull = true)]
        public string Keywords
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._keywords; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._keywords = value; }
        }

        [Column(Storage = "_layout", DbType = "NVarChar(512)", CanBeNull = false)]
        public string Layout
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _layout; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _layout = value; }
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

        IPage IPage.Parent
        {
            get { return null; }
            set { }
        }

        [Column(Storage = "_sectionId", DbType = "UniqueIdentifier", CanBeNull = true)]
        public Guid? SectionID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._sectionId; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._sectionId = value; }
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

        [Column(Storage = "_dateCreated", DbType = "DATETIME NOT NULL", CanBeNull = false, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public DateTime DateCreated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._dateCreated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._dateCreated = value; }
        }

        [Column(Storage = "_lastUpdated", DbType = "DATETIME NOT NULL", CanBeNull = false, IsVersion = true)]
        public DateTime? LastUpdated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._lastUpdated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._lastUpdated = value; }
        }

        [Column(Storage = "_author", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Author
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._author; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._author = value; }
        }

        [Column(Storage = "_lastUpdatedBy", DbType = "NVarChar(512) NOT NULL", CanBeNull = true)]
        public string LastUpdatedBy
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._lastUpdatedBy; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _lastUpdatedBy = value; }
        }

        string IPage.AbsoluteSlug
        {
            get { return null; }
        }

        ISection IPage.Section
        {
            get { return null; }
            set { }
        }

        List<IPage> IPage.Childs
        {
            get { return new List<IPage>(); }
        }

        #endregion
    }
}
