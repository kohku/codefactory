using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class admin_manageSingleRol : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["rolename"]))
            Response.Redirect("manageAllRoles.aspx");

        if (!IsPostBack)
        {
            string rolename = Server.UrlDecode(Request.QueryString["rolename"]);

            ViewState["rolename"] = rolename;
            RoleNameLabel.Text = string.Format("Role {0}", rolename);
        }
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        ViewState["rolename"] = string.Empty;
        MembershipGridView.DataBind();
    }

    protected void MembershipDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new MembershipResult(SearchSelection.SelectedValue == "username" ?
            SearchTextBox.Text : string.Empty, SearchSelection.SelectedValue == "email" ?
            SearchTextBox.Text : string.Empty, ViewState["rolename"].ToString());
    }

    protected void IsInRoleCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox item = sender as CheckBox;
        string rolename = Server.UrlDecode(Request.QueryString["rolename"]);

        if (item == null)
            return;

        foreach (GridViewRow row in MembershipGridView.Rows)
        {
            CheckBox i = row.FindControl(item.ID) as CheckBox;

            if (i == null || !i.Equals(item))
                continue;

            Label usernameLabel = row.FindControl("UserNameLabel") as Label;

            if (usernameLabel == null)
                continue;

            if (i.Checked)
                Roles.AddUserToRole(usernameLabel.Text, rolename);
            else
                Roles.RemoveUserFromRole(usernameLabel.Text, rolename);

            break;
        }
    
        MembershipGridView.DataBind();
    }

    protected void MembershipGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        MembershipUser user = e.Row.DataItem as MembershipUser;
        string rolename = Server.UrlDecode(Request.QueryString["rolename"]);

        CheckBox isUserInRole = e.Row.FindControl("IsInRoleCheckBox") as CheckBox;

        if (isUserInRole == null)
            return;
        
        isUserInRole.Checked = Roles.IsUserInRole(user.UserName, rolename);
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/manageAllRoles.aspx");
    }
}
