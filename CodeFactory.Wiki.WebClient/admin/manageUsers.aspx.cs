using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

public partial class admin_manageUsers : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        MembershipGridView.DataBind();
    }

    protected void MembershipDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new MembershipResult(SearchSelection.SelectedValue == "username" ?
            SearchTextBox.Text : string.Empty, SearchSelection.SelectedValue == "email" ?
            SearchTextBox.Text : string.Empty);
    }

    protected void MembershipGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        MembershipUser user = e.Row.DataItem as MembershipUser;

        LinkButton editUser = e.Row.FindControl("EditUserLinkButton") as LinkButton;

        if (user != null && editUser != null)
        {
            editUser.CommandArgument = user.UserName;
            editUser.PostBackUrl = string.Format("~/admin/editUser.aspx?username={0}", Server.UrlEncode(user.UserName));
        }

        LinkButton deleteUser = e.Row.FindControl("DeleteUserLinkButton") as LinkButton;

        if (user != null && deleteUser != null)
        {
            if (!ClientScript.IsClientScriptBlockRegistered("DeleteUserWarning"))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("function deleteUser(username){");
                builder.Append("return confirm(\"¿Estás seguro que deseas eliminar el usuario '\" + username + \"'?\");");
                builder.Append("}");

                ClientScript.RegisterClientScriptBlock(GetType(), "DeleteRoleWarning", builder.ToString(), true);
            }

            deleteUser.Attributes.Add("onclick", string.Format("javascript: return deleteUser('{0}');",
                user.UserName));
            deleteUser.CommandArgument = user.UserName;
        }

        LinkButton editRoles = e.Row.FindControl("EditRolesLinkButton") as LinkButton;

        if (user != null && editRoles != null)
            editRoles.CommandArgument = user.UserName;
    }

    protected void RolesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label usernameLabel = MembershipGridView.Rows[MembershipGridView.SelectedIndex].FindControl("UserNameLabel") as Label;

        string rolename = e.Row.DataItem as string;

        CheckBox isUserInRole = e.Row.FindControl("IsUserInRoleCheckBox") as CheckBox;

        Label rolenameLabel = e.Row.FindControl("RoleNameLabel") as Label;

        if (!string.IsNullOrEmpty(rolename) && isUserInRole != null && usernameLabel != null)
            isUserInRole.Checked = Roles.IsUserInRole(usernameLabel.Text, rolename);

        if (!string.IsNullOrEmpty(rolename) && rolenameLabel != null)
            rolenameLabel.Text = rolename;
    }

    protected void MembershipGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        if (MembershipGridView.SelectedIndex >= 0)
        {
            using (GridViewRow currentMembershipRow = MembershipGridView.Rows[MembershipGridView.SelectedIndex])
            {
                LinkButton editRoles = currentMembershipRow.FindControl("EditRolesLinkButton") as LinkButton;
                GridView lastRoles = currentMembershipRow.FindControl("RolesGridView") as GridView;

                if (editRoles != null)
                    editRoles.Visible = true;

                if (lastRoles != null)
                {
                    lastRoles.DataSourceID = string.Empty;
                    lastRoles.DataBind();
                }
            }
        }

        using (GridViewRow newRow = MembershipGridView.Rows[e.NewSelectedIndex])
        {
            LinkButton editRoles = newRow.FindControl("EditRolesLinkButton") as LinkButton;
            GridView roles = newRow.FindControl("RolesGridView") as GridView;

            if (editRoles != null)
                editRoles.Visible = false;

            if (roles != null)
            {
                MembershipGridView.SelectedIndex = e.NewSelectedIndex;
                roles.DataSourceID = "RolesDataSource";
                roles.DataBind();
            }
        }
    }

    protected void IsUserInRoleCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        using (GridViewRow currentMembershipRow = MembershipGridView.Rows[MembershipGridView.SelectedIndex])
        {
            CheckBox item = sender as CheckBox;

            Label usernameLabel = currentMembershipRow.FindControl("UserNameLabel") as Label;
            GridView roles = currentMembershipRow.FindControl("RolesGridView") as GridView;

            if (item == null || usernameLabel == null || roles == null)
                return;

            foreach (GridViewRow row in roles.Rows)
            {
                CheckBox i = row.FindControl(item.ID) as CheckBox;
                Label rolenameLabel = row.FindControl("RoleNameLabel") as Label;

                if (i == null || !i.Equals(item) || rolenameLabel == null)
                    continue;

                if (i.Checked)
                    Roles.AddUserToRole(usernameLabel.Text, rolenameLabel.Text);
                else
                    Roles.RemoveUserFromRole(usernameLabel.Text, rolenameLabel.Text);

                break;
            }

            roles.DataBind();
        }
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }
}
