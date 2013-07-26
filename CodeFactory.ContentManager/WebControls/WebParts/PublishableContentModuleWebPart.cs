using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class PublishableContentModuleWebPart : ModuleWebPart
    {
        private Publication _publication;

        public PublishableContentModuleWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            this._publication = Publication.Load(this.Key);

            if (this._publication == null)
            {
                this._publication = new Publication(new LinqPublication(this.Key));
                this._publication.Title = this.Title;
                this._publication.Author = HttpContext.Current.User.Identity.Name;
                this._publication.Category = null;
                this._publication.Description = this.Description;
                this._publication.IsVisible = true;
                this._publication.RelativeLink = HttpContext.Current.Request.Url.ToString();
            }
        }

        [Category("Custom"), Personalizable(true), Description("Content"), WebDisplayName("DisplayName"), DefaultValue("Content"), Themeable(false)]
        public override string Content
        {
            get
            {
                return (((string)this.ViewState["Content"]) ?? base.Content);
            }
            set
            {
                this.ViewState["Content"] = value;
                this._publication.Content = value;
                this._publication.Save();
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
