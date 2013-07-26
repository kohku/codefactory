using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class PersistableModuleWebPart : ModuleWebPart
    {
        protected Module _module;

        protected PersistableModuleWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            this._module = Module.Load(this.Key);

            if (this._module == null)
            {
                this._module = new Module(this.Key);
                this._module.Title = this.Title;
            }
        }

        [Category("Custom"), Personalizable(false), Description("Content"), WebDisplayName("DisplayName"), DefaultValue("Content"), Themeable(false)]
        public override string Content
        {
            get
            {
                return (((string)this.ViewState["Content"]) ?? this._module.Content);
            }
            set
            {
                this.ViewState["Content"] = this._module.Content = value;
                this._module.Save();
            }
        }

        public override bool EnableViewState
        {
            get { return false; }
            set { }
        }

        public Guid Key
        {
            get { return base.GetType().GUID; }
        }
    }
}
