using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.ContentManager;
using System.Text;
using System.Web.Hosting;

public partial class Page : System.Web.UI.Page
{
    protected CodeFactory.ContentManager.Page _page;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback && !string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));
        }

        if (!IsPostBack)
            LayoutHelper.Value = _page != null ? _page.Layout : ContentManagementService.Settings.DefaultLayout;
        else
            LayoutHelper.Value = Request.Params["LayoutHelper"];

        try
        {
            this.LayoutPanel.Controls.Add(LoadControl(LayoutHelper.Value));
        }
        catch (HttpException)
        {
            this.LayoutPanel.Controls.Add(LoadControl(ContentManagementService.Settings.DefaultLayout));
        }
    }

    protected void Page_InitComplete(object sender, EventArgs e)
    {
        Page.Title = Server.HtmlEncode(_page != null ? _page.Title : "Nueva página");

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

            if (currentWebPartManager.Personalization.CanEnterSharedScope &&
                (currentWebPartManager.Personalization.Scope == PersonalizationScope.User))
                currentWebPartManager.Personalization.ToggleScope();

            foreach (WebPartDisplayMode mode in currentWebPartManager.SupportedDisplayModes)
                DisplayMode.Items.Add(mode.Name);

            if (!IsPostBack)
            {
                if (_page == null && currentWebPartManager.SupportedDisplayModes.Contains(WebPartManager.CatalogDisplayMode))
                    currentWebPartManager.DisplayMode = WebPartManager.CatalogDisplayMode;
                else
                    currentWebPartManager.DisplayMode = WebPartManager.BrowseDisplayMode;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            UpdateView();

        AddMetaTags();
    }

    private void UpdateView()
    {
        WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

        if (User.Identity.IsAuthenticated)
        {
            int selectedMode = DisplayMode.Items.IndexOf(DisplayMode.Items.FindByText(
                currentWebPartManager.DisplayMode.Name));

            if (selectedMode >= 0)
                DisplayMode.SelectedIndex = selectedMode;

            BindPage(_page);
           
            if (!currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode) &&
                !ClientScript.IsClientScriptBlockRegistered(GetType(), "LayoutHelper"))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("function updateLayout(layoutList){");
                builder.AppendFormat("var layoutHelper = document.getElementById(\"{0}\");", LayoutHelper.ClientID);
                builder.Append("layoutHelper.value = layoutList.options[layoutList.selectedIndex].value;");
                builder.Append("}");

                ClientScript.RegisterClientScriptBlock(GetType(), "LayoutHelper", builder.ToString(), true);
                LayoutList.Attributes.Add("onchange", "updateLayout(this);");
            }

            AuthorData.Text = User.Identity.Name;
            DateCreatedData.Text = DateTime.Now.ToString();
        }

        DisplayMode.Visible = User.Identity.IsAuthenticated;
        ToolBarPanel.Visible = !currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode);
        ActionsPanel.Visible = !currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode);
        DeleteButton.Visible = !currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode);
    }

    private void BindPage(CodeFactory.ContentManager.Page page)
    {
        if (page == null)
            return;

        TitleTextBox.Text = page.Title;
        SlugTextBox.Text = page.Slug;
        DescriptionTextBox.Text = page.Description;
        KeywordsTextBox.Text = page.Keywords;
        IsVisibleCheckBox.Checked = page.IsVisible;

        int layoutIndex = LayoutList.Items.IndexOf(LayoutList.Items.FindByValue(page.Layout));

        if (layoutIndex >= 0)
            LayoutList.SelectedIndex = layoutIndex;
    }

    /// <summary>
    /// Adds the meta tags and title to the HTML header.
    /// </summary>
    private void AddMetaTags()
    {
        if (this._page != null)
        {
            //Page.Title = Server.HtmlEncode(this.Page.Title);
            AddMetaTagBase("keywords", Server.HtmlEncode(this._page.Keywords));
            AddMetaTagBase("description", Server.HtmlEncode(this._page.Description));
        }
    }

    /// <summary>
    /// Add a meta tag to the page's header.
    /// </summary>
    protected virtual void AddMetaTagBase(string name, string value)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            return;

        HtmlMeta meta = new HtmlMeta();
        meta.Name = name;
        meta.Content = value;
        Page.Header.Controls.Add(meta);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if ((this.TheWebPartManager.DisplayMode == WebPartManager.EditDisplayMode) &&
            (this.TheWebPartManager.SelectedWebPart != null))
            this.TheMultiView.ActiveViewIndex = 1;
        else
            this.TheMultiView.ActiveViewIndex = 0;
    }

    protected void DisplayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
            WebPartDisplayMode mode = currentWebPartManager.SupportedDisplayModes[DisplayMode.SelectedValue];

            if (mode != null)
                currentWebPartManager.DisplayMode = mode;

            UpdateView();
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));

            if (_page == null)
                _page = new CodeFactory.ContentManager.Page(new Guid(Request.QueryString["id"]));

            _page.Title = TitleTextBox.Text;
            _page.Slug = SlugTextBox.Text;
            _page.Description = DescriptionTextBox.Text;
            _page.Keywords = KeywordsTextBox.Text;
            _page.Layout = LayoutHelper.Value;
            _page.Author = User.Identity.Name;
            _page.IsVisible = IsVisibleCheckBox.Checked;

            if (IsDefaultCheckBox.Checked)
            {
                ContentManagementService.Settings.DefaultPage = _page.RelativeLink;
                ContentManagementService.SaveSettings();
            }

            _page.Save();
        }

        WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
        currentWebPartManager.DisplayMode = WebPartManager.BrowseDisplayMode;
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        // TODO: Si es una página nueva debemos entonces eliminar lo almacenado por el web part manager.
        Response.Redirect("~/default.aspx");
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            // TODO: Debemos eliminar el contenido almacenado por el web part manager para este url.
            _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));

            if (_page != null)
            {
                _page.Delete();
                _page.Save();
            }
        }

        Response.Redirect("~/default.aspx");
    }

    protected void LayoutList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            UpdateView();
        }
    }
}
