using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Data.Linq.Mapping;

namespace CodeFactory.Gallery.Core
{
    [Serializable]
    [Table(Name = "GalleryComments")]
    public class Comment : IComparable<Comment>, CodeFactory.Web.Core.IPublishable<Guid>
    {    
        public Comment() 
            : this(Guid.NewGuid())
        {
            _dateCreated = DateTime.Now;
        }

        public Comment(Guid id)
        {
            _id = id;
        }

        #region IPublishable Members

        private string _title = null;

        [Column(Storage = "_title", DbType = "NVarChar(512)", CanBeNull = false)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _content = null;

        [Column(Storage = "_content", DbType = "NVarChar(1024)", CanBeNull = false)]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private DateTime _dateCreated = DateTime.MinValue;

        [Column(Storage = "_dateCreated", DbType = "DateTime", CanBeNull = false)]
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        [XmlIgnore()]
        public DateTime? LastUpdated
        {
            get { return null; }
            set { }
        }

        private Guid _id;

        [Column(Storage = "_id", DbType = "UniqueIdentifier NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlIgnore()]
        public string Description
        {
            get { return null; }
        }

        private string _author = null;

        [Column(Storage = "_author", DbType = "NVarChar(512)", CanBeNull = false)]
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        [XmlIgnore()]
        public bool IsVisible
        {
            get { return true; }
            set { }
        }

        [XmlIgnore()]
        string CodeFactory.Web.Core.IPublishable<Guid>.Slug
        {
            get { throw new NotImplementedException(); }
        }

        [XmlIgnore()]
        string CodeFactory.Web.Core.IPublishable<Guid>.Keywords
        {
            get { throw new NotImplementedException(); }
        }

        [XmlIgnore()]
        string CodeFactory.Web.Core.IPublishable<Guid>.LastUpdatedBy
        {
            get { return null; }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return new List<string>(); }
        }

        public string RelativeLink
        {
            get { throw new NotImplementedException(); }
        }

        public Uri AbsoluteLink
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        private CodeFactory.Web.Core.IPublishable<Guid> _parent;

        [XmlIgnore()]
        public CodeFactory.Web.Core.IPublishable<Guid> Parent
        {
            get { return _parent; }
            set {
                if (value != null)
                    _parentId = value.ID;
                _parent = value;
            }
        }

        private Guid _parentId;

        [Column(Name = "GalleryID", DbType = "UniqueIdentifier", CanBeNull = false)]
        internal Guid ParentID
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private string _ipAddress = null;

        [Column(Storage = "_ipAddress", DbType = "NVarChar(50)", CanBeNull = false)]
        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        #region IComparable<Comment> Members

        public int CompareTo(Comment other)
        {
            return DateCreated.CompareTo(other.DateCreated);
        }

        #endregion
    }
}
