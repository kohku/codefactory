using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager
{
    public class Post : CodeFactory.Web.Core.BusinessBase<Post, Guid>, CodeFactory.Web.Core.IPublishable<Guid>
    {
        private LinqPost _post;

        private List<Comment> _comments;
        private List<Category> _categories;
        private List<string> _tags;

        public Post()
            : this(new LinqPost(Guid.NewGuid()))
        {
        }

        public Post(Guid id)
            : this(new LinqPost(id))
        {
        }

        public Post(LinqPost post)
            : base(post.ID)
        {
            if (post == null)
                throw new ArgumentNullException("post");

            this._post = post;
            _comments = new List<Comment>();
            _categories = new List<Category>();
            _tags = new List<string>();
        }

        public List<Comment> Comments
        {
            get { return _comments; }
        }

        public List<Category> Categories
        {
            get { return _categories; }
        }

        public List<string> Tags
        {
            get { return _tags; }
        }

        #region IPublishable<Guid> Members

        public string Title
        {
            get
            {
                return this._post.Title;
            }
            set
            {
                if (this._post.Title != value)
                {
                    this.OnPropertyChanging("Title");
                    this._post.Title = value;
                    this.MarkChanged("Title");
                }
            }
        }

        public string Content
        {
            get
            {
                return this._post.Content;
            }
            set
            {
                if (this._post.Content != value)
                {
                    this.OnPropertyChanging("Content");
                    this._post.Content = value;
                    this.MarkChanged("Content");
                }
            }
        }

        public string Slug
        {
            get
            {
                return this._post.Slug;
            }
            set
            {
                if (this._post.Slug != value)
                {
                    this.OnPropertyChanging("Slug");
                    this._post.Slug = value;
                    this.MarkChanged("Slug");
                }
            }
        }

        public string Keywords
        {
            get
            {
                return this._post.Keywords;
            }
            set
            {
                if (this._post.Keywords != value)
                {
                    this.OnPropertyChanging("Keywords");
                    this._post.Keywords = value;
                    this.MarkChanged("Keywords");
                }
            }
        }

        public string Description
        {
            get
            {
                return this._post.Description;
            }
            set
            {
                if (this._post.Description != value)
                {
                    this.OnPropertyChanging("Description");
                    this._post.Description = value;
                    this.MarkChanged("Description");
                }
            }
        }

        public string Author
        {
            get
            {
                return this._post.Author;
            }
            set
            {
                if (this._post.Author != value)
                {
                    this.OnPropertyChanging("Author");
                    this._post.Author = value;
                    this.MarkChanged("Author");
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return this._post.IsVisible;
            }
            set
            {
                if (this._post.IsVisible != value)
                {
                    this.OnPropertyChanging("IsVisible");
                    this._post.IsVisible = value;
                    this.MarkChanged("IsVisible");
                }
            }
        }

        public List<string> Roles
        {
            get { throw new NotImplementedException(); }
        }

        public string LastUpdatedBy
        {
            get
            {
                return this._post.LastUpdatedBy;
            }
            set
            {
                if (this._post.LastUpdatedBy != value)
                {
                    this.OnPropertyChanging("LastUpdatedBy");
                    this._post.LastUpdatedBy = value;
                    this.MarkChanged("LastUpdatedBy");
                }
            }
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

        protected override void ValidationRules()
        {
        }

        protected override Post DataSelect(Guid id)
        {
            //LinqPost post = ContentManagementService.GetPost(id);

            //if (post == null)
            return null;

            //return new Post(post);
        }

        protected override void DataUpdate()
        {
            //ContentManagementService.UpdatePost(this._post);
        }

        protected override void DataInsert()
        {
            //ContentManagementService.InsertPost(this._post);
        }

        protected override void DataDelete()
        {
            //ContentManagementService.DeletePost(this._post);
        }
    }
}
