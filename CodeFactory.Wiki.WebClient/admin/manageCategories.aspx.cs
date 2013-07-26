using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CodeFactory.Wiki;

public partial class admin_manageCategories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void AddCategoryButton_Click(object sender, EventArgs e)
    {
        string category = AddCategoryTextBox.Text.Trim();

        if (string.IsNullOrEmpty(category))
            return;

        WikiService.InsertCategory(category);

        CategoriesGridView.DataBind();

        AddCategoryTextBox.Text = string.Empty;
    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }

    protected void CategoriesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LinkButton item = CategoriesGridView.Rows[e.RowIndex].FindControl("DeleteLinkButton") as LinkButton;

        if (item != null)
            e.Keys.Add("category", item.CommandArgument);
    }

    protected void CategoriesDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new WikiCategoriesResult(true);
    }

    protected void CategoriesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!(e.Row.DataItem is KeyValuePair<string, string>))
            return;

        string category = ((KeyValuePair<string, string>)e.Row.DataItem).Key;

        LinkButton delete = e.Row.FindControl("DeleteLinkButton") as LinkButton;

        if (!string.IsNullOrEmpty(category) && delete != null)
        {
            if (!ClientScript.IsClientScriptBlockRegistered("DeleteCategoryWarning"))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("function deleteRole(category){");
                builder.AppendFormat("return confirm(\"¿Estás seguro que deseas eliminar la categoría '{0}'?\");", category);
                builder.Append("}");

                ClientScript.RegisterClientScriptBlock(GetType(), "DeleteCategoryWarning", builder.ToString(), true);
            }

            delete.Attributes.Add("onclick", string.Format("javascript: return deleteRole('{0}');", category));
            delete.CommandArgument = category;
        }

        LinkButton manage = e.Row.FindControl("ManageLinkButton") as LinkButton;

        if (manage != null)
            manage.CommandArgument = e.Row.DataItem.ToString();
    }
}
