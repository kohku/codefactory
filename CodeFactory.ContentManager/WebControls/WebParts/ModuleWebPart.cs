using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Permissions;

namespace CodeFactory.ContentManager.WebControls.WebParts
{
    /// <summary>
    /// Serves as a base class for a web part content that can be published.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ModuleWebPart : WebPart
    {
        public override EditorPartCollection CreateEditorParts()
        {
            List<EditorPart> customEditorPartCollection = new List<EditorPart>();
            ModuleEditorPart contentEditorPart = new ModuleEditorPart
            {
                ID = "ContentEditorPart1"
            };
            customEditorPartCollection.Add(contentEditorPart);
            return new EditorPartCollection(customEditorPartCollection);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(this.Content);
        }

        // Properties
        [WebDisplayName("DisplayName"), Category("Custom"), Personalizable(true), Description("Content"), Themeable(false), DefaultValue("Content")]
        public virtual string Content
        {
            get
            {
                return (((string)this.ViewState["Content"]) ?? string.Empty);
            }
            set
            {
                this.ViewState["Content"] = value;
            }
        }

        public override object WebBrowsableObject
        {
            get { return this; }
        }
    }
}