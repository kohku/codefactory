using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeFactory.Web;

namespace CodeFactory.ContentManager
{
    public class Module : CodeFactory.Web.Core.BusinessBase<Module, Guid>, IModule
    {
        private string _title;
        private byte[] _contentRaw;

        public Module()
            : this(Guid.NewGuid())
        {
        }

        public Module(Guid id) 
            : base(id)
        {
        }

        #region IModule Members

        public string Title
        {
            get { return _title; }
            set
            {
                if (this._title != value)
                {
                    this.OnPropertyChanging("Title");
                    this._title = value;
                    this.MarkChanged("Title");
                }
            }
        }

        byte[] IModule.ContentRaw
        {
            get { return _contentRaw; }
            set { this._contentRaw = value; }
        }

        #endregion

        #region Module

        public string Content
        {
            get
            {
                if (_contentRaw == null || _contentRaw.Length == 0)
                    return string.Empty;

                return Encoding.Unicode.GetString(_contentRaw);
            }
            set
            {
                if (this.Content != value)
                {
                    this.OnPropertyChanging("Content");
                    ((IModule)this).ContentRaw = !string.IsNullOrEmpty(value) ?
                            Encoding.Unicode.GetBytes(value) : new byte[0];
                    this.MarkChanged("Content");
                }
            }
        }

        #endregion

        protected override void ValidationRules()
        {
        }

        protected override Module DataSelect(Guid id)
        {
            IModule m = ContentManagementService.GetModule(id);

            if (m == null)
                return null;

            if (m is Module)
                return (Module)m;

            return Module.ConvertFrom(m);
        }

        protected override void DataInsert()
        {
            ContentManagementService.InsertModule(this);
        }

        protected override void DataUpdate()
        {
            ContentManagementService.UpdateModule(this);
        }

        protected override void DataDelete()
        {
            ContentManagementService.DeleteModule(this);
        }

        public static Module ConvertFrom(IModule module)
        {
            if (module == null)
                return null;

            return new Module(module.ID)
            {
                Title = module.Title,
                _contentRaw = module.ContentRaw,
                DateCreated = module.DateCreated,
                LastUpdated = module.LastUpdated,
            };
        }
    }
}
