using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CodeFactory.ContentManager;
using System.Web.UI.HtmlControls;
using System.Text;
using CodeFactory.Web;
using System.Web.Security;

public partial class GetPage : System.Web.UI.Page
{
    protected CodeFactory.ContentManager.Page _page;

    protected void Page_Init(object sender, EventArgs e)
    {
        // Viene de la liga de página nueva? Redireccionando
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            Response.Redirect(string.Format("~/GetPage.aspx?id={0}", Guid.NewGuid()), true);

        // Intentando recuperar información de la página.
        if (!Page.IsPostBack && !Page.IsCallback && !string.IsNullOrEmpty(Request.QueryString["id"]))
            _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));

        // TODO: Checar si podemos cambiar LayoutHelper.Value por ViewState["Layout"]
        // Mostrando la distribución existente o la default para la página y almacenándola.
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
        // Estableciendo el título para la página
        if (!Page.IsPostBack && !Page.IsCallback)
            Page.Title = Server.HtmlEncode(_page != null ? _page.Title : "Nueva página");

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

            // Cambiando al modo de personalización compartida 
            // si se encuentra en el modo de personalización por usuario
            //if (currentWebPartManager.Personalization.CanEnterSharedScope &&
            //    currentWebPartManager.Personalization.Scope == PersonalizationScope.User)
            //    currentWebPartManager.Personalization.ToggleScope();

            // Llenando el combo de los modos de visualización soportados
            foreach (WebPartDisplayMode mode in currentWebPartManager.SupportedDisplayModes)
                DisplayMode.Items.Add(mode.Name);

            // Selecciona el modo inicial de visualización
            if (!IsPostBack)
            {
                // Página nueva, modo catálogo
                if (_page == null && currentWebPartManager.SupportedDisplayModes.Contains(WebPartManager.DesignDisplayMode))
                    currentWebPartManager.DisplayMode = WebPartManager.DesignDisplayMode;
                // Página existente, modo navegación
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

            Author.Text = User.Identity.Name;
            DateCreated.Text = DateTime.Now.ToString();
        }

        // Solo visible si el usuario está autentificado y es del rol administrador o editor.
        DisplayMode.Visible = User.Identity.IsAuthenticated && (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Editor"));

        // Propiedades de la página
        __page_properties_title.Visible =  __page_properties.Visible = User.Identity.IsAuthenticated && 
            (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Editor")) && 
            !currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode);

        // Catálogo de controles
        __page_catalog_title.Visible = __page_catalog.Visible = User.Identity.IsAuthenticated && 
            (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Editor")) && 
            currentWebPartManager.DisplayMode.Equals(WebPartManager.CatalogDisplayMode);

        // Botones de acciones
        __page_actions.Visible = User.Identity.IsAuthenticated && (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Editor")) &&
            !currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode);

        // Botón de eliminar
        DeleteButton.Visible = User.Identity.IsAuthenticated && (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Editor")) && 
            _page != null && !_page.IsNew && !currentWebPartManager.DisplayMode.Equals(WebPartManager.BrowseDisplayMode);
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

    private void UpdatePage(CodeFactory.ContentManager.Page page)
    {
        if (page == null)
            return;

        if (page.IsNew)
            page.Author = User.Identity.Name;
        page.Title = TitleTextBox.Text;
        page.Slug = Utils.RemoveIllegalCharacters(SlugTextBox.Text);
        page.Description = DescriptionTextBox.Text;
        // page.IsDefault
        page.IsVisible = IsVisibleCheckBox.Checked;
        page.Keywords = KeywordsTextBox.Text;
        page.Layout = LayoutList.SelectedValue;
        // page.Roles
        // page.Section

        // Al final, para evitar que se guarde sin cambios.
        if (!page.IsNew && page.IsChanged)
            page.LastUpdatedBy = User.Identity.Name;
    }

    /// <summary>
    /// Adds the meta tags and title to the HTML header.
    /// </summary>
    private void AddMetaTags()
    {
        if (this._page == null)
            return;

        //Page.Title = Server.HtmlEncode(this.Page.Title);
        AddMetaTagBase("keywords", Server.HtmlEncode(this._page.Keywords));
        AddMetaTagBase("description", Server.HtmlEncode(this._page.Description));
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
        //if ((this.TheWebPartManager.DisplayMode == WebPartManager.EditDisplayMode) &&
        //    (this.TheWebPartManager.SelectedWebPart != null))
        //    this.TheMultiView.ActiveViewIndex = 1;
        //else
        //    this.TheMultiView.ActiveViewIndex = 0;
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

    protected void LayoutList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
            UpdateView();
    }

    protected void EditButton_Click(object sender, EventArgs e)
    {
        __content_editor.Content = ContentLabel.Text;
        ContentEditorModalPopupExtender.Show();
    }

    protected void AcceptButton_Click(object sender, EventArgs e)
    {
        ContentLabel.Text = __content_editor.Content;
        ContentEditorModalPopupExtender.Hide();
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));

        if (_page == null)
            _page = new CodeFactory.ContentManager.Page(new Guid(Request.QueryString["id"]));

        UpdatePage(_page);

        _page.AcceptChanges();

        WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
        
        Response.Redirect(_page.RelativeLink);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

        _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));

        if (_page != null)
            Response.Redirect(_page.RelativeLink, false);
        else
            Response.Redirect("~/default.aspx", false);

        // esets personalization data for the current page, scope, and user in the underlying data store. 
        // As a side effect of the reset, the currently executing page is re-executed by a Transfer call.
        if (currentWebPartManager.Personalization.HasPersonalizationState)
            currentWebPartManager.Personalization.ResetPersonalizationState();
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

        _page = CodeFactory.ContentManager.Page.Load(new Guid(Request.QueryString["id"]));

        if (_page == null)
            return;

        _page.Delete();

        _page.AcceptChanges();

        if (_page != null)
            Response.Redirect("~/default.aspx", false);

        // esets personalization data for the current page, scope, and user in the underlying data store. 
        // As a side effect of the reset, the currently executing page is re-executed by a Transfer call.
        if (currentWebPartManager.Personalization.HasPersonalizationState)
            currentWebPartManager.Personalization.ResetPersonalizationState();
    }
}
