using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Security;

public partial class admin_Roles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RolesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label rolename = e.Row.FindControl("RolenameLabel") as Label;

        if (rolename != null)
            rolename.Text = e.Row.DataItem.ToString();

        LinkButton delete = e.Row.FindControl("DeleteLinkButton") as LinkButton;

        if (delete != null)
        {
            if (!ClientScript.IsClientScriptBlockRegistered("DeleteRoleWarning"))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("function deleteRole(rolename){");
                builder.Append("return confirm(\"¿Estás seguro que deseas eliminar el rol '\" + rolename + \"'?\");");
                builder.Append("}");

                ClientScript.RegisterClientScriptBlock(GetType(), "DeleteRoleWarning", builder.ToString(), true);
            }

            delete.Attributes.Add("onclick", string.Format("javascript: return deleteRole('{0}');",
                e.Row.DataItem.ToString()));
            delete.CommandArgument = e.Row.DataItem.ToString();
        }

        LinkButton manage = e.Row.FindControl("ManageLinkButton") as LinkButton;

        if (manage != null)
            manage.CommandArgument = e.Row.DataItem.ToString();
    }

    protected void RolesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LinkButton item = RolesGridView.Rows[e.RowIndex].FindControl("DeleteLinkButton") as LinkButton;

        if (item != null)
            e.Keys.Add("rolename", item.CommandArgument);
    }

    protected void AddRoleButton_Click(object sender, EventArgs e)
    {
        string rolename = AddRoleTextBox.Text.Trim();

        if (string.IsNullOrEmpty(rolename) || Roles.RoleExists(rolename))
            return;

        Roles.CreateRole(rolename);

        RolesGridView.DataBind();

        AddRoleTextBox.Text = string.Empty;
    }

    protected void RolesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Manage")
            Response.Redirect(string.Format("~/admin/manageSingleRol.aspx?rolename={0}",
                Server.UrlEncode(e.CommandArgument.ToString())));
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }
}
