using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CodeFactory.Utilities;

namespace CodeFactory.ContentManager
{
    [Serializable]
    public class Category : CodeFactory.Web.Core.BusinessBase<Category, Guid>, ICategory
    {
        private object syncRoot = new object();
        public static readonly string Root = new string(new char[]{
            System.IO.Path.DirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar});

        private string _name;
        private Guid? _parentId;
        private List<ICategory> _childs;

        public Category()
            : base(Guid.NewGuid())
        {
        }

        public Category(Guid id)
            : base(id)
        {
        }

        #region ICategory Members

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Contains(System.IO.Path.DirectorySeparatorChar.ToString()))
                    throw new ArgumentException(
                        ResourceStringLoader.GetResourceString("invalid_character",
                        System.IO.Path.DirectorySeparatorChar.ToString()));

                if (_name != value)
                {
                    this.OnPropertyChanging("Name");
                    _name = value;
                    this.MarkChanged("Name");
                }
            }
        }

        public ICategory Parent
        {
            get
            {
                if (_parentId.HasValue)
                    return Category.Load(_parentId.Value);

                return null;
            }
            set
            {
                if (value != null)
                {
                    this.OnPropertyChanging("Parent");
                    _parentId = value.ID;
                    this.MarkChanged("Parent");
                }
            }
        }

        public string Path
        {
            get
            {
                return this.Parent != null ?
                    System.IO.Path.Combine(this.Parent.Path, this.Name) :
                    string.Format("{0}{1}", Category.Root, this.Name);
            }
        }

        public List<ICategory> Childs
        {
            get
            {
                lock (syncRoot)
                {
                    if (_childs != null)
                        return _childs;

                    _childs = ContentManagementService.GetChildCategories(this.ID);
                }

                return _childs;
            }
        }

        protected override void ValidationRules()
        {
        }

        #endregion

        #region Data Access

        protected override Category DataSelect(Guid id)
        {
            ICategory c = ContentManagementService.GetCategory(id);

            if (c == null)
                return null;

            if (c is Category)
                return (Category)c;

            return Category.ConvertFrom(c);
        }

        protected override void DataUpdate()
        {
            ContentManagementService.UpdateCategory(this);
        }

        protected override void DataInsert()
        {
            ContentManagementService.InsertCategory(this);
        }

        protected override void DataDelete()
        {
            ContentManagementService.DeleteCategory(this);

            foreach (Category child in ContentManagementService.GetChildCategories(this.ID))
            {
                child.Delete();
                child.Save();
            }
        }

        #endregion

        #region Operations

        public static event EventHandler<CancelEventArgs> AddingChild;

        protected virtual void OnAddingChild(ICategory child, CancelEventArgs e)
        {
            if (AddingChild != null)
                AddingChild(child, e);
        }

        public static event EventHandler<EventArgs> ChildAdded;

        protected virtual void OnChildAdded(ICategory child)
        {
            if (ChildAdded != null)
                ChildAdded(child, EventArgs.Empty);
        }

        public void AddChild(ICategory child)
        {
            if (child == null)
                throw new ArgumentNullException("item");

            lock (this.Childs)
            {
                if (this.Childs.Contains(child))
                    return;

                CancelEventArgs e = new CancelEventArgs();

                OnAddingChild(child, e);

                if (!e.Cancel)
                {
                    child.Parent = this;
                    this.Childs.Add(child);

                    MarkChanged("Childs");
                    OnChildAdded(child);
                }
            }
        }

        /// <summary>
        /// Occurs before a item is removed.
        /// </summary>
        public static event EventHandler<CancelEventArgs> RemovingChild;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="item">File.</param>
        /// <param name="e">Cancel event argument.</param>
        protected virtual void OnRemovingChild(ICategory child, CancelEventArgs e)
        {
            if (RemovingChild != null)
                RemovingChild(child, e);
        }

        /// <summary>
        /// Occurs when a item is removed.
        /// </summary>
        public static event EventHandler<EventArgs> ChildRemoved;

        /// <summary>
        /// Raises the event in a safe way.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnChildRemoved(ICategory child)
        {
            if (ChildRemoved != null)
                ChildRemoved(child, EventArgs.Empty);
        }

        public void RemoveChild(ICategory child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            lock (this.Childs)
            {
                if (!this.Childs.Contains(child))
                    return;

                CancelEventArgs e = new CancelEventArgs();
                OnRemovingChild(child, e);

                if (!e.Cancel)
                {
                    OnChildRemoved(child);
                    if (child is Category)
                        ((Category)child).Delete();

                    MarkChanged("Childs");
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return this.Path;
        }

        public static Category ConvertFrom(ICategory category)
        {
            if (category == null)
                return null;

            Category c = new Category(category.ID)
            {
                Name = category.Name,
                _parentId = category.Parent != null ? category.Parent.ID : default(Nullable<Guid>),
                DateCreated = category.DateCreated,
                LastUpdated = category.LastUpdated
            };

            if (!c.IsValid)
                throw new InvalidOperationException("Category does not satisfy validation rules");

            return c;
        }
    }
}
