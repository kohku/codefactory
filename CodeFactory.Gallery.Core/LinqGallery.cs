using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.ComponentModel;

namespace CodeFactory.Gallery.Core
{
    [Serializable]
    [Table(Name = "Galleries")]
    public class LinqGallery : IGallery
    {
        private Gallery _gallery;

        public LinqGallery()
        {
            this._gallery = new Gallery();
        }

        public LinqGallery(Gallery gallery)
        {
            this._gallery = gallery;
        }

        #region IIdentifiable<Guid> Members

        [Column(DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public Guid ID
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.ID; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.ID = value; }
        }

        #endregion

        #region IPublishable Members

        [Column(DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Author
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.Author; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.Author = value; }
        }

        [Column(DbType = "NVarChar(1024)", CanBeNull = true)]
        public string Content
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.Content; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.Content = value; }
        }

        [Column(DbType = "DATETIME NOT NULL", CanBeNull = false, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public DateTime DateCreated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.DateCreated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.DateCreated = value; }
        }

        [Column(DbType = "NVarChar(512)", CanBeNull = true)]
        public string Description
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.Description; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.Description = value; }
        }

        [Column(DbType = "Bit NOT NULL", CanBeNull = false)]
        public bool IsVisible
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.IsVisible; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.IsVisible = value; }
        }

        [Column(DbType = "NVarChar(1024) NOT NULL", CanBeNull = true)]
        public string Keywords
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.Keywords; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.Keywords = value; }
        }

        [Column(DbType = "DATETIME", CanBeNull = true, IsVersion = true)]
        public DateTime? LastUpdated
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.LastUpdated; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.LastUpdated = value; }
        }

        [Column(DbType = "NVarChar(512) NULL", CanBeNull = true)]
        public string LastUpdatedBy
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.LastUpdatedBy; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.LastUpdatedBy = value; }
        }

        [Column(DbType = "NVarChar(512) NULL", CanBeNull = true)]
        public string Slug
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.Slug; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.Slug = value; }
        }

        [Column(DbType = "NVarChar(512) NOT NULL", CanBeNull = false)]
        public string Title
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.Title; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.Title = value; }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return new List<string>(); }
        }

        #endregion

        #region IGallery Members

        [Column(DbType = "NVarChar(512)", CanBeNull = false)]
        public string ApplicationName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this._gallery.ApplicationName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { this._gallery.ApplicationName = value; }
        }

        public Uri AbsoluteLink
        {
            get { throw new NotImplementedException(); }
        }

        public List<Comment> Comments
        {
            get { throw new NotImplementedException(); }
        }

        public List<CodeFactory.Web.Storage.UploadedFile> Files
        {
            get { throw new NotImplementedException(); }
        }

        public string RelativeLink
        {
            get { throw new NotImplementedException(); }
        }

        public List<string> Users
        {
            get { throw new NotImplementedException(); }
        }

        [Column(DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public GalleryStatus Status
        {
            get { return this._gallery.Status; }
            set { this._gallery.Status = value; }
        }

        #endregion
    }
}
