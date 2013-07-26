using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager.WebControls
{
    public class Publication : CodeFactory.Web.Core.BusinessBase<Publication, Guid>, IPublishable<Guid>
    {
        private LinqPublication _publication;

        public Publication()
            : this(new LinqPublication(Guid.NewGuid()))
        {
        }

        public Publication(LinqPublication publication)
            : base(publication.ID)
        {
            if (publication == null)
                throw new ArgumentNullException("module");

            _publication = publication;
        }

        #region IPublishable Members

        public string Title
        {
            get { return _publication.Title; }
            set
            {
                if (this._publication.Title != value)
                {
                    this.OnPropertyChanging("Title");
                    this._publication.Title = value;
                    this.MarkChanged("Title");
                }
            }
        }

        public string Description
        {
            get { return _publication.Description; }
            set
            {
                if (this._publication.Description != value)
                {
                    this.OnPropertyChanging("Description");
                    this._publication.Description = value;
                    this.MarkChanged("Description");
                }
            }
        }

        public string Content
        {
            get { return _publication.Content; }
            set
            {
                Regex regEx = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>",
                    RegexOptions.Singleline);

                string tagless = regEx.Replace(value, string.Empty);

                if (this._publication.Content != tagless)
                {
                    this.OnPropertyChanging("Content");
                    this._publication.Content = tagless;
                    this.MarkChanged("Content");
                }
            }
        }

        public string Category
        {
            get { return _publication.Category; }
            set 
            {
                if (this._publication.Category != value)
                {
                    this.OnPropertyChanging("Category");
                    _publication.Category = value;
                    this.MarkChanged("Category");
                }
            }
        }

        public string Author
        {
            get { return _publication.Author; }
            set
            {
                if (this._publication.Author != value)
                {
                    this.OnPropertyChanging("Author");
                    this._publication.Author = value;
                    this.MarkChanged("Author");
                }
            }
        }

        public bool IsVisible
        {
            get { return _publication.IsVisible; }
            set
            {
                if (this._publication.IsVisible != value)
                {
                    this.OnPropertyChanging("IsVisible");
                    this._publication.IsVisible = value;
                    this.MarkChanged("IsVisible");
                }
            }
        }

        List<string> CodeFactory.Web.Core.IPublishable<Guid>.Roles
        {
            get { return null; }
        }

        public string RelativeLink
        {
            get { return _publication.RelativeLink; }
            set
            {
                if (this._publication.RelativeLink != value)
                {
                    this.OnPropertyChanging("RelativeLink");
                    this._publication.RelativeLink = value;
                    this.MarkChanged("RelativeLink");
                }
            }
        }

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

        public override DateTime DateCreated
        {
            get { return _publication.DateCreated; }
        }

        public override DateTime? LastUpdated
        {
            get { return _publication.LastUpdated; }
        }

        protected override void  ValidationRules()
        {
        }

        protected override Publication DataSelect(Guid id)
        {
            //LinqPublication publication = ContentManagementService.GetPublication(id);

            //if (publication == null)
            return null;

            //return new Publication(publication);
        }

        protected override void  DataUpdate()
        {
            //ContentManagementService.UpdatePublication(this._publication);
        }

        protected override void  DataInsert()
        {
            //ContentManagementService.InsertPublication(this._publication);
        }

        protected override void  DataDelete()
        {
            //ContentManagementService.DeletePublication(this._publication);
        }

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
