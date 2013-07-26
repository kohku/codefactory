using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.IO;

namespace CodeFactory.Web.Storage
{
    [Table(Name = "UploadedFiles")]
    public class LinqUploadedFile : CodeFactory.Web.Storage.IUploadedFile
    {
        UploadedFile _file;

        public LinqUploadedFile()
        {
            _file = new UploadedFile();
        }

        public LinqUploadedFile(UploadedFile file)
        {
            _file = file;
        }

        #region IUniqueIdentifier

        [Column(DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.ID; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.ID = value; }
        }

        #endregion

        #region IUploadedFile

        [Column(DbType = "NVarChar(512)", CanBeNull = false)]
        public string ApplicationName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.ApplicationName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.ApplicationName = value; }
        }

        [Column(DbType = "UniqueIdentifier NOT NULL", CanBeNull = false)]
        public Guid ParentID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.ParentID; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.ParentID = value; }
        }

        [Column(DbType = "NVarChar(1024) NOT NULL", CanBeNull = false)]
        public string FileName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.FileName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.FileName = value; }
        }

        [Column(DbType = "NVarChar(1024)", CanBeNull = true)]
        public string Description
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.Description; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.Description = value; }
        }

        [Column(DbType = "VARBINARY(MAX) NOT NULL", CanBeNull = false)]
        public byte[] Data
        {
            get { return _file.Data; }
            set { _file.Data = value; }
        }

        [Column(DbType = "DATETIME NOT NULL", CanBeNull = false, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public DateTime DateCreated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.DateCreated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.DateCreated = value; }
        }

        [Column(DbType = "DATETIME", CanBeNull = true, IsVersion = true)]
        public DateTime? LastUpdated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.LastUpdated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.LastUpdated = value; }
        }

        [Column(DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string ContentType
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.ContentType; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _file.ContentType = value; }
        }

        [Column(DbType = "Int NOT NULL", CanBeNull = false)]
        public int ContentLength
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _file.ContentLength; }
            set { _file.ContentLength = value; }
        }

        #endregion
    }
}