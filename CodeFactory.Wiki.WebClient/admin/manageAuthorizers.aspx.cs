using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.Wiki;

public partial class admin_manageAuthorizers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }

    protected void AddRoleButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(AuthorizersList.SelectedValue) || string.IsNullOrEmpty(CategoryList.SelectedValue))
                return;

            string category = CategoryList.SelectedValue;
            string username = AuthorizersList.SelectedValue;

            WikiService.InsertAuthorizerByCategory(category, username);
        }
        catch
        {
        }

        TheWikiAuthorizersGridView.DataBind();
    }
    protected void CategoriesDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new WikiCategoriesResult(true);
    }
}
