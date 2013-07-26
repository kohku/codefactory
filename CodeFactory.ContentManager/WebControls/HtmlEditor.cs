using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CodeFactory.ContentManager.WebControls
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level=AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Text"), ToolboxData("<{0}:HtmlEditor runat=\"server\"> </{0}:HtmlEditor>")]
    public class HtmlEditor : WebControl
    {
        private TextBox content;

        private Unit heigth = Unit.Empty;
        private Unit width = Unit.Empty;

        // Configuration
        private string mode = "exact";
        private string theme = "advanced";
        private string language = "en";
        private string plugins = "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template";
        // Theme options
        private string themeAdvancedButtons1 = "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect";
        private string themeAdvancedButtons2 = "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor";
        private string themeAdvancedButtons3 = "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen";
        private string themeAdvancedButtons4 = "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak";
        private string themeAdvancedToolbarLocation = "top";
        private string themeAdvancedToolbarAlign = "left";
        private string themeAdvancedStatusbarLocation = "bottom";
        private bool themeAdvancedResizing = true;
        // Content css
        private string contentCss = "css/content.css";
        // Drop lists for link/image/media/template dialogs
        private string templateExternalListUrl = "lists/template_list.js";
        private string externalLinkListUrl = "lists/link_list.js";
        private string externalImageListUrl = "lists/image_list.js";
        private string mediaExternalListUrl = "lists/media_list.js";

        public HtmlEditor()
        {
        }

        [Bindable(true), Category("Settings"), Description("Mode."), DefaultValue("exact")]
        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        [Bindable(true), Category("Settings"), Description("Theme."), DefaultValue("advanced")]
        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }

        [Bindable(true), Category("Settings"), Description("Language."), DefaultValue("en")]
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        [Bindable(true), Category("Settings"), Description("Plugins."), DefaultValue("safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template")]
        public string Plugins
        {
            get { return plugins; }
            set { plugins = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedButtons1."), DefaultValue("save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect")]
        public string ThemeAdvancedButtons1
        {
            get { return themeAdvancedButtons1; }
            set { themeAdvancedButtons1 = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedButtons2."), DefaultValue("cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor")]
        public string ThemeAdvancedButtons2
        {
            get { return themeAdvancedButtons2; }
            set { themeAdvancedButtons2 = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedButtons3."), DefaultValue("tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen")]
        public string ThemeAdvancedButtons3
        {
            get { return themeAdvancedButtons3; }
            set { themeAdvancedButtons3 = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedButtons4."), DefaultValue("insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak")]
        public string ThemeAdvancedButtons4
        {
            get { return themeAdvancedButtons4; }
            set { themeAdvancedButtons4 = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedToolbarLocation."), DefaultValue("top")]
        public string ThemeAdvancedToolbarLocation
        {
            get { return themeAdvancedToolbarLocation; }
            set { themeAdvancedToolbarLocation = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedToolbarAlign."), DefaultValue("left")]
        public string ThemeAdvancedToolbarAlign
        {
            get { return themeAdvancedToolbarAlign; }
            set { themeAdvancedToolbarAlign = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedStatusbarLocation."), DefaultValue("bottom")]
        public string ThemeAdvancedStatusbarLocation
        {
            get { return themeAdvancedStatusbarLocation; }
            set { themeAdvancedStatusbarLocation = value; }
        }

        [Bindable(true), Category("Settings"), Description("ThemeAdvancedResizing."), DefaultValue(true)]
        public bool ThemeAdvancedResizing
        {
            get { return themeAdvancedResizing; }
            set { themeAdvancedResizing = value; }
        }

        [Bindable(true), Category("Settings"), Description("ContentCss."), DefaultValue("css/content.css")]
        public string ContentCss
        {
            get { return contentCss; }
            set { contentCss = value; }
        }

        [Bindable(true), Category("Settings"), Description("TemplateExternalListUrl."), DefaultValue("lists/template_list.js")]
        public string TemplateExternalListUrl
        {
            get { return templateExternalListUrl; }
            set { templateExternalListUrl = value; }
        }

        [Bindable(true), Category("Settings"), Description("ExternalLinkListUrl."), DefaultValue("lists/link_list.js")]
        public string ExternalLinkListUrl
        {
            get { return externalLinkListUrl; }
            set { externalLinkListUrl = value; }
        }

        [Bindable(true), Category("Settings"), Description("ExternalImageListUrl."), DefaultValue("lists/image_list.js")]
        public string ExternalImageListUrl
        {
            get { return externalImageListUrl; }
            set { externalImageListUrl = value; }
        }

        [Bindable(true), Category("Settings"), Description("MediaExternalListUrl."), DefaultValue("lists/media_list.js")]
        public string MediaExternalListUrl
        {
            get { return mediaExternalListUrl; }
            set { mediaExternalListUrl = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(""), Description("The Html content."),
        Localizable(true)]
        public virtual string Content
        {
            get { return this.content != null ? this.content.Text : string.Empty; }
            set { if (this.content != null) this.content.Text = value; }
        }

        [Bindable(true), Category("Layout"), Description("Height.")]
        public override Unit Height
        {
            get 
            {
                return heigth;
            }
            set 
            {
                if (this.content != null) 
                    this.content.Height = value;
                heigth = value;
            }
        }

        [Bindable(true), Category("Layout"), Description("Width.")]
        public override Unit Width
        {
            get
            {
                return width;
            }
            set 
            { 
                if (this.content != null) 
                    this.content.Width = value;
                width = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            this.content = new TextBox();
            this.content.Height = this.Height;
            this.content.TextMode = TextBoxMode.MultiLine;
            this.content.Width = this.Width;

            this.Controls.Add(this.content);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("TinyMCE"))
                Page.ClientScript.RegisterClientScriptInclude(typeof(Page), "TinyMCE",
                    ResolveClientUrl("~/js/tiny_mce/tiny_mce.js"));

            StringBuilder init = new StringBuilder();

            init.AppendLine("tinyMCE.init({");
            init.AppendLine("// General options");
            init.AppendFormat("mode : \"{0}\", ", this.Mode);
            init.AppendFormat("elements : \"{0}\", ", content.ClientID);
            init.AppendFormat("theme : \"{0}\", ", this.Theme);
            init.AppendFormat("language : \"{0}\", ", this.Language);
            init.AppendFormat("plugins : \"{0}\", ", this.Plugins);

            init.AppendLine("// Theme options");
            init.AppendFormat("theme_advanced_buttons1 : \"{0}\", ", this.ThemeAdvancedButtons1);
            init.AppendFormat("theme_advanced_buttons2 : \"{0}\", ", this.ThemeAdvancedButtons2);
            init.AppendFormat("theme_advanced_buttons3 : \"{0}\", ", this.ThemeAdvancedButtons3);
            init.AppendFormat("theme_advanced_buttons4 : \"{0}\", ", this.ThemeAdvancedButtons4);
            init.AppendFormat("theme_advanced_toolbar_location : \"{0}\", ", this.ThemeAdvancedToolbarLocation);
            init.AppendFormat("theme_advanced_toolbar_align : \"{0}\", ", this.ThemeAdvancedToolbarAlign);
            init.AppendFormat("theme_advanced_statusbar_location : \"{0}\", ", this.ThemeAdvancedStatusbarLocation);
            init.AppendFormat("theme_advanced_resizing : {0}, ", this.ThemeAdvancedResizing.ToString().ToLowerInvariant());

            init.AppendLine("// Example content CSS (should be your site CSS)");
            init.AppendFormat("content_css : \"{0}\", ", this.ContentCss);

            init.AppendLine("// Drop lists for link/image/media/template dialogs");
            init.AppendFormat("template_external_list_url : \"{0}\", ", this.TemplateExternalListUrl);
            init.AppendFormat("external_link_list_url : \"{0}\", ", this.ExternalLinkListUrl);
            init.AppendFormat("external_image_list_url : \"{0}\", ", this.ExternalImageListUrl);
            init.AppendFormat("media_external_list_url : \"{0}\", ", this.MediaExternalListUrl);

            init.AppendLine("// Replace values for the template plugin");
            init.AppendLine("template_replace_values : {");
            init.AppendLine("username : \"Some User\",");
            init.AppendLine("staffid : \"991234\"");
            init.AppendLine("}");
            init.AppendLine("});");

            if (!Page.ClientScript.IsStartupScriptRegistered("TinyMCE_Init"))
                Page.ClientScript.RegisterStartupScript(typeof(Page), "TinyMCE_Init",
                    init.ToString(), true);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            // Determines whether the server control contains child controls.
            // If it does not, it creates child controls.
            EnsureChildControls();

            this.content.RenderControl(writer);
        }
    }
}