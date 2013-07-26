using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Utilities;

namespace CodeFactory.Web.Controls
{
    /// <summary>
    /// Summary description for ContentEditorPart
    /// </summary>
    public class ModuleEditorPart : EditorPart
    {
        private HtmlEditor PartPropertyValue;
        private bool _displayErrorMessage;

        public ModuleEditorPart()
        {
            this.Title = ResourceStringLoader.GetResourceString("PublishableEditorPart_Title");
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            PartPropertyValue = new HtmlEditor();
            PartPropertyValue.Height = new Unit(400, UnitType.Pixel);
            PartPropertyValue.Width = new Unit(100, UnitType.Percentage);

            this.Controls.Add(PartPropertyValue);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            //writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "4");

            //writer.RenderBeginTag(HtmlTextWriterTag.Table);
            //writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            //writer.RenderBeginTag(HtmlTextWriterTag.Td);

            this.PartPropertyValue.RenderControl(writer);

            if (_displayErrorMessage)
            {
                writer.WriteBreak();
                Label errorMessage = new Label();
                errorMessage.Text = "Error converting type";
                errorMessage.ApplyStyle(this.Zone.ErrorStyle);
                errorMessage.RenderControl(writer);
            }

            //writer.RenderEndTag();  // Td
            //writer.RenderEndTag();  // Tr
            //writer.RenderEndTag();  // Table
        }

        public override bool ApplyChanges()
        {
            // Determines whether the server control contains child controls.
            // If it does not, it creates child controls.
            EnsureChildControls();

            ModuleWebPart content = WebPartToEdit as ModuleWebPart;

            if (content != null && !String.IsNullOrEmpty(PartPropertyValue.Content))
            {
                try
                {
                    content.Content = PartPropertyValue.Content;
                }
                catch
                {
                    _displayErrorMessage = true;
                    return false;
                }
            }

            return true;
        }

        public override void SyncChanges()
        {
            // Determines whether the server control contains child controls.
            // If it does not, it creates child controls.
            EnsureChildControls();

            ModuleWebPart content = WebPartToEdit as ModuleWebPart;

            if (content != null)
            {
                PartPropertyValue.Content = content.Content;
            }
        }
    }
}