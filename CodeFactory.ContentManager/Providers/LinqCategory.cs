using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeFactory.ContentManager.Providers
{
    [Table(Name = "Categories")]
    public class LinqCategory : ICategory
    {
        private Guid _id;
        private string _name;
        private string _applicationName;
        private Guid? _parentId;
        private DateTime _dateCreated;
        private DateTime? _lastUpdated;

        public LinqCategory()
        {
        }

        public LinqCategory(Guid id)
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

        #region ICategory

        [Column(Storage = "_name", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
            get { return _parentId; }
            set { _parentId = value; }
        }

        string ICategory.Path { get { return null; } }

        ICategory ICategory.Parent
        {
            get
            {
                return _parentId.HasValue ? new LinqCategory(_parentId.Value) : null;
            }
            set
            {
                if (value != null)
                    _parentId = value.ID;
            }
        }

        List<ICategory> ICategory.Childs { get { return null; } }

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

        #endregion
    }
}
