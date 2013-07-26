using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager
{
    [Table(Name="Publications")]
    public class LinqPublication : IPublishable<Guid>
    {
        private Guid _id;
        private string _title;
        private string _description;
        private string _content;
        private string _category;
        private DateTime _dateCreated;
        private DateTime? _lastUpdated;
        private string _author;
        private bool _isVisible;
        private string _relativeLink;
        private string _applicationName;

        #region IPublishable Members

        public LinqPublication()
        {
        }

        public LinqPublication(Guid id)
        {
            _id = id;
        }

        [Column(Storage = "_title", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Title
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _title; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _title = value; }
        }

        [Column(Storage = "_description", DbType = "NVarChar(50) NOT NULL", CanBeNull = true)]
        public string Description
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _description; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _description = value; }
        }

        [Column(Storage = "_content", DbType = "NVarChar(MAX) NOT NULL", CanBeNull = false)]
        public string Content
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _content; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _content = value; }
        }

        [Column(Storage = "_category", DbType = "NVarChar(512)", CanBeNull = true)]
        public string Category
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _category; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _category = value; }
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

        [Column(Storage = "_author", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Author
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._author; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._author = value; }
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

        [Column(Storage = "_relativeLink", DbType = "NVarChar(1024) NOT NULL", CanBeNull = false)]
        public string RelativeLink
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _relativeLink; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _relativeLink = value; }
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

        [Column(Storage = "_applicationName", DbType = "NVarChar(512)", CanBeNull = false)]
        public string ApplicationName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _applicationName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _applicationName = value; }
        }

        #region IPublishable<Guid> Members


        public string Slug
        {
            get { throw new NotImplementedException(); }
        }

        public string Keywords
        {
            get { throw new NotImplementedException(); }
        }

        public string LastUpdatedBy
        {
            get { throw new NotImplementedException(); }
        }

        public Uri AbsoluteLink
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IPublishable<Guid> Members


        string IPublishable<Guid>.Slug
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IPublishable<Guid>.Keywords
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IPublishable<Guid>.LastUpdatedBy
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
