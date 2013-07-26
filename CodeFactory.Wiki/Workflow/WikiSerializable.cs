using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.Wiki.Workflow
{
    [Serializable]
    public class WikiSerializable : IWorkWikiItem
    {
        private IWorkWikiItem _item;

        private Guid _id;
        private string _title;
        private string _content;

        public WikiSerializable()
        {
        }

        public WikiSerializable(IWorkWikiItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _item = item;
            _id = item.ID;
            _title = item.Title;
            _content = item.Content;
        }

        #region IWorkWikiItem Members

        CodeFactory.Web.Core.SaveAction IWorkWikiItem.Action
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IWorkWikiItem.Authorizer
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

        string IWorkWikiItem.Messages
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

        WikiStatus IWorkWikiItem.Status
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

        Guid IWorkWikiItem.TrackingNumber
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

        string IWorkWikiItem.TrackingLink
        {
            get { throw new NotImplementedException(); }
        }

        DateTime? IWorkWikiItem.ExpirationDate
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

        CodeFactory.Web.Core.SaveAction IWorkWikiItem.Save()
        {
            throw new NotImplementedException();
        }

        string IWorkWikiItem.IPAddress
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

        #region IWiki Members

        void IWiki.AddFile(CodeFactory.Web.Storage.UploadedFile item)
        {
            throw new NotImplementedException();
        }

        string IWiki.Category
        {
            get { throw new NotImplementedException(); }
        }

        string IWiki.DepartmentArea
        {
            get { throw new NotImplementedException(); }
        }

        bool IWiki.Editable
        {
            get { throw new NotImplementedException(); }
        }

        string IWiki.Editor
        {
            get { throw new NotImplementedException(); }
        }

        List<CodeFactory.Web.Storage.UploadedFile> IWiki.Files
        {
            get { throw new NotImplementedException(); }
        }

        List<CodeFactory.Web.Storage.UploadedFile> IWiki.Images
        {
            get { throw new NotImplementedException(); }
        }

        ReachLevel IWiki.ReachLevel
        {
            get { throw new NotImplementedException(); }
        }

        void IWiki.RemoveFile(CodeFactory.Web.Storage.UploadedFile item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPublishable Members

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Title
        {
            get { return _item.Title; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Content
        {
            get { return _item.Content; }
        }

        DateTime CodeFactory.Web.Core.IPublishable<Guid>.DateCreated
        {
            get { return _item.DateCreated; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.RelativeLink
        {
            get { return _item.RelativeLink; }
        }

        Uri CodeFactory.Web.Core.IPublishable<Guid>.AbsoluteLink
        {
            get { return _item.AbsoluteLink; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Description
        {
            get { return _item.Description; }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Author
        {
            get { return _item.Author; }
        }

        bool CodeFactory.Web.Core.IPublishable<Guid>.IsVisible
        {
            get { return _item.IsVisible; }
        }

        #endregion

        #region IUniqueIdentifier Members

        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region IPublishable<Guid> Members


        string CodeFactory.Web.Core.IPublishable<Guid>.Slug
        {
            get { throw new NotImplementedException(); }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.Keywords
        {
            get { throw new NotImplementedException(); }
        }

        DateTime? CodeFactory.Web.Core.IPublishable<Guid>.LastUpdated
        {
            get { throw new NotImplementedException(); }
        }

        string CodeFactory.Web.Core.IPublishable<Guid>.LastUpdatedBy
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IIdentifiable<Guid> Members

        Guid CodeFactory.Web.Core.IIdentifiable<Guid>.ID
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
