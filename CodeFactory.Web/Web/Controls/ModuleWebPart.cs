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

namespace CodeFactory.Web.Controls
{
    /// <summary>
    /// Serves as a base class for a web part content that can be published
    /// </summary>
    public abstract class ModuleWebPart : WebPart
    {
        public ModuleWebPart()
        {
        }

        [Personalizable(PersonalizationScope.Shared), WebDisplayName("DisplayName"),
        DefaultValue("Content"), Themeable(false), Category("Custom"), Description("Content")]
        public virtual string Content
        {
            get { return (string)ViewState["Content"] ?? string.Empty; }
            set { ViewState["Content"] = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            // Won't use HttpUtility.HtmlEncode
            writer.Write(this.Content);
        }

        public override EditorPartCollection CreateEditorParts()
        {
            List<EditorPart> customEditorPartCollection = new List<EditorPart>();

            ModuleEditorPart contentEditorPart = new ModuleEditorPart();
            contentEditorPart.ID = "ContentEditorPart1";
            customEditorPartCollection.Add(contentEditorPart);

            EditorPartCollection editorPartCollection = new EditorPartCollection(customEditorPartCollection);
            return editorPartCollection;
        }

        public override object WebBrowsableObject
        {
            get { return this; }
        }
    }
}