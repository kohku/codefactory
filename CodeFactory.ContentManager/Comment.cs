using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web;
using System.Data.Linq.Mapping;

namespace CodeFactory.ContentManager
{
    [Table(Name="Comments")]
    public class Comment : CodeFactory.Web.Core.IPublishable<Guid>, IComparable<Comment>
    {
        private string _applicationName;

        private bool _isApproved;
        private CodeFactory.Web.Core.IPublishable<Guid> _parent;
        private Guid _parentId;
        private string _avatar;
        private string _ip;
        private string _moderatedBy;

        private string _title;
        private string _content;
        private DateTime _dateCreated;
        private string _author;

        private Guid _id;

        [Column(Storage = "_applicationName", DbType = "NVarChar(512)", CanBeNull = false)]
        public string ApplicationName
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _applicationName; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _applicationName = value; }
        }

        [Column(Storage="_isApproved",DbType="Bit",CanBeNull=false)]
        public bool IsApproved
        {
            get { return _isApproved; }
            set { _isApproved = value; }
        }

        public Guid ParentID
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public CodeFactory.Web.Core.IPublishable<Guid> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Abbreviated content
        /// </summary>
        public string Teaser
        {
            get
            {
                string ret = Utils.StripHtml(this.Content).Trim();
                if (ret.Length > 120)
                    return ret.Substring(0, 116) + " ...";
                return ret;
            }
        }

        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }

        [Column(Storage="_ip", DbType="NVarChar(50)", CanBeNull=false)]
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        [Column(Storage="_moderatedBy", DbType="NVarChar(512)", CanBeNull=true)]
        public string ModeratedBy
        {
            get { return _moderatedBy; }
            set { _moderatedBy = value; }
        }

        #region IPublishable<Guid> Members

        [Column(Storage="_title", DbType="NVarChar(512)", CanBeNull=false)]
        public string Title
        {
            get { return _title ; }
            set { _title = value; }
        }

        [Column(Storage="_content", DbType="NVarChar(1024)", CanBeNull=false)]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Slug
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Keywords
        {
            get { return null; }
            set { }
        }

        [Column(Storage="_dateCreated", DbType="DateTime", CanBeNull=false)]
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        DateTime? CodeFactory.Web.Core.IPublishable<Guid>.LastUpdated
        {
            get { return null; }
            set { }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Description
        {
            get { return null; }
            set { }
        }

        [Column(Storage="_author", DbType="NVarChar(512)", CanBeNull=false)]
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        bool CodeFactory.Web.Core.IPublishable<Guid>.IsVisible
        {
            get { return this.IsApproved; }
            set { }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return new List<string>(); }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.LastUpdatedBy
        {
            get { return null; }
            set { }
        }

        public string RelativeLink
        {
            get { return string.Format("{0}#id_", Parent.RelativeLink, this.ID); }
        }

        public Uri AbsoluteLink
        {
            get { return new Uri(string.Format("{0}#id_", Parent.AbsoluteLink, ID)); }
        }

        #endregion

        #region IIdentifiable<Guid> Members

        [Column(Storage="_id", IsPrimaryKey=true, DbType="UniqueIdentifier", CanBeNull=false)]
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region IComparable<Comment> Members

        public int CompareTo(Comment other)
        {
            return this.DateCreated.CompareTo(other.DateCreated);
        }

        #endregion
    }
}
