using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace CodeFactory.ContentManager
{
    [Table(Name = "Modules")]
    public class LinqModule : CodeFactory.ContentManager.IModule
    {
        private Guid _id;
        private string _title;
        private byte[] _content;
        private string _applicationName;
        private DateTime _dateCreated;
        private DateTime? _lastUpdated;

        public LinqModule()
        {
        }

        public LinqModule(Guid id)
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

        #region IModule

        [Column(Storage = "_title", DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Title
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._title; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._title = value; }
        }

        [Column(Name = "Content", Storage = "_content", DbType = "VarBinary(MAX)", CanBeNull = true)]
        public byte[] ContentRaw
        {
            get { return _content; }
            set { _content = value; }
        }

        [Column(Storage = "_applicationName", DbType = "NVarChar(512)", CanBeNull = false)]
        public string ApplicationName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _applicationName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _applicationName = value; }
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

        #endregion
    }
}
