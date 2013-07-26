using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace CodeFactory.ContentManager
{
    [Table(Name = "Sections")]
    public class LinqSection : ISection
    {
        private Guid _id;
        private string _name;
        private string _slug;
        private string _applicationName;
        private Guid? _parentId;
        private DateTime _dateCreated;
        private DateTime? _lastUpdated;
        private int _index;
        private bool _isVisible;
        private string _keywords;

        public LinqSection()
        {
        }

        public LinqSection(Guid id)
        {
            _id = id;
        }

        #region IIdentifiable

        [Column(Storage = "_id", DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region ISection Members

        [Column(Storage = "_name", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Column(Storage = "_slug", DbType = "NVarchar(512) NOT NULL", CanBeNull = false)]
        public string Slug
        {
            get { return _slug; }
            set { _slug = value; }
        }

        string ISection.Path
        {
            get { return null; }
        }

        [Column(Storage = "_applicationName", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
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
            get { return _parentId; }
            set { _parentId = value; }
        }

        [Column(Storage = "_index", DbType = "INT", CanBeNull = false)]
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        ISection ISection.Parent
        {
            get { return null; }
            set { }
        }

        string ISection.AbsoluteSlug
        {
            get { return null; }
        }

        List<ISection> ISection.Childs
        {
            get { return new List<ISection>(); }
        }

        #endregion

        #region IPublishable<Guid> Members

        [Column(Storage = "_isVisible", DbType = "BIT", CanBeNull = false)]
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Title
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Content
        {
            get { return null; }
            set { }
        }

        [Column(Storage = "_dateCreated", DbType = "NVarChar(1024)", CanBeNull = true)]
        public string Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
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

        string CodeFactory.Web.Core.IPublishable<Guid>.Description
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Author
        {
            get { return null; }
            set { }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return new List<string>(); }
        }

        List<IPage> ISection.Pages
        {
            get { return new List<IPage>(); }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.LastUpdatedBy
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
    }
}
