using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.ContentManager;

public partial class admin_ContentManagerSettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindSettings();
    }

    private void BindSettings()
    {
        DefaultPageTextBox.Text = ContentManagementService.Settings.DefaultPage;
        DefaultLayoutTextBox.Text = ContentManagementService.Settings.DefaultLayout;
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(DefaultPageTextBox.Text))
            ContentManagementService.Settings.DefaultPage = DefaultPageTextBox.Text;

        if (!string.IsNullOrEmpty(DefaultLayoutTextBox.Text))
            ContentManagementService.Settings.DefaultLayout = DefaultLayoutTextBox.Text;

        ContentManagementService.SaveSettings();
    }
}
