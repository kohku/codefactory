using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web.Core;
using CodeFactory.Web.Storage;
using CodeFactory.Web;

namespace CodeFactory.Wiki.Workflow
{
    [Serializable]
    public class WorkWikiItem : CodeFactory.Web.Core.BusinessBase<WorkWikiItem, Guid>, IWorkWikiItem
    {
        private CodeFactory.Wiki.IWiki _item;
        private Guid _trackingNumber;
        private SaveAction _action = SaveAction.None;
        private WikiStatus _status = WikiStatus.Created;
        private string _authorizer;
        private string _messages;
        private DateTime? _expirationDate;
        private string _ipAddress;

        public WorkWikiItem(CodeFactory.Wiki.IWiki item)
            : this(item.ID)
        {
            _item = item;
        }

        public WorkWikiItem()
            : this(Guid.NewGuid())
        {
        }

        public WorkWikiItem(Guid id)
            : base(id)
        {
        }

        #region IWorkWikiItem Members

        public DateTime? ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                if (_expirationDate != value)
                    MarkChanged("ExpirationDate");
                _expirationDate = value;
            }
        }

        public Guid TrackingNumber
        {
            get { return _trackingNumber; }
            set
            {
                if (_trackingNumber != value)
                    MarkChanged("WorkflowInstanceId");
                _trackingNumber = value;
            }
        }

        public string TrackingLink
        {
            get
            {
                return string.Format("{0}AuthorizeWiki.aspx?trackingNumber={1}", Utils.RelativeWebRoot,
                    this.TrackingNumber);
            }
        }


        public SaveAction Action
        {
            get { return _action; }
            set
            {
                if (_action != value)
                    MarkChanged("Action");
                _action = value;
            }
        }

        public WikiStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                    MarkChanged("Status");
                _status = value;
            }
        }

        public string Authorizer
        {
            get { return _authorizer; }
            set
            {
                if (_authorizer != value)
                    MarkChanged("Authorizer");
                _authorizer = value;
            }
        }

        public string Messages
        {
            get { return _messages; }
            set
            {
                if (_messages != value)
                    MarkChanged("Messages");
                _messages = value;
            }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                if (_ipAddress != value)
                    MarkChanged("IPAddress");
                _ipAddress = value;
            }
        }

        #endregion

        #region IWiki Members

        public void AddFile(UploadedFile item)
        {
            _item.AddFile(item);
        }

        public string Category
        {
            get { return _item.Category; }

        }

        public string DepartmentArea
        {
            get { return _item.DepartmentArea; }

        }

        public bool Editable
        {
            get { return _item.Editable; }

        }

        public string Editor
        {
            get { return _item.Editor; }

        }

        public List<UploadedFile> Files
        {
            get { return _item.Files; }
        }

        public List<UploadedFile> Images
        {
            get { return _item.Images; }
        }

        public string Keywords
        {
            get { return _item.Keywords; }

        }

        public string LastUpdatedBy
        {
            get { return _item.LastUpdatedBy; }

        }

        public ReachLevel ReachLevel
        {
            get { return _item.ReachLevel; }
        }

        public void RemoveFile(UploadedFile item)
        {
            _item.RemoveFile(item);
        }

        public string Slug
        {
            get { return _item.Slug; }
        }

        #endregion

        #region IPublishable Members

        public new DateTime DateCreated
        {
            get { return _item.DateCreated; }
        }

        public new DateTime? LastUpdated
        {
            get { return _item.LastUpdated; }
        }

        public string Title
        {
            get { return _item.Title; }
        }

        public string Content
        {
            get { return _item.Content; }
        }

        public string RelativeLink
        {
            get { return _item.RelativeLink; }
        }

        public Uri AbsoluteLink
        {
            get { return _item.AbsoluteLink; }
        }

        public string Description
        {
            get { return _item.Description; }
        }

        public string Author
        {
            get { return _item.Author; }
        }

        public bool IsVisible
        {
            get { return _item.IsVisible; }
        }

        #endregion

        protected override void ValidationRules()
        {
        }

        protected override WorkWikiItem DataSelect(Guid id)
        {
            return (WorkWikiItem)WikiService.SelectWorkWikiItem(id);
        }

        protected override void DataUpdate()
        {
            WikiService.UpdateWorkWikiItem(this);
        }

        protected override void DataInsert()
        {
            WikiService.InsertWorkWikiItem(this);
        }

        protected override void DataDelete()
        {
            WikiService.DeleteWorkWikiItem(this);
        }
    }
}
