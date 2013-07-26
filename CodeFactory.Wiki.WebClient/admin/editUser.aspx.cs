using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration.Provider;

public partial class admin_editUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["username"]))
            Response.Redirect("~/admin/manageUsers.aspx");

        if (!IsPostBack)
            BindUser();
    }

    private void BindUser()
    {
        string username = Server.UrlDecode(Request.QueryString["username"]);

        MembershipUser user = Membership.GetUser(username);

        if (user == null)
            Response.Redirect("~/admin/manageUsers.aspx");

        UserNameTextBox.Text = user.UserName;
        EmailTextBox.Text = user.Email;
        DescriptionTextBox.Text = user.Comment;
        ActiveUserCheckBox.Checked = !user.IsLockedOut;
    }

    protected void RolesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string rolename = e.Row.DataItem as string;
        string username = Server.UrlDecode(Request.QueryString["username"]);

        CheckBox isUserInRole = e.Row.FindControl("IsUserInRoleCheckBox") as CheckBox;

        Label rolenameLabel = e.Row.FindControl("RoleNameLabel") as Label;

        if (!string.IsNullOrEmpty(rolename) && isUserInRole != null)
            isUserInRole.Checked = Roles.IsUserInRole(username, rolename);

        if (!string.IsNullOrEmpty(rolename) && rolenameLabel != null)
            rolenameLabel.Text = rolename;
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            string username = Server.UrlDecode(Request.QueryString["username"]);

            MembershipUser user = Membership.GetUser(username);

            if (user == null)
                Response.Redirect("~/admin/manageUsers.aspx");

            user.Email = EmailTextBox.Text;
            user.Comment = DescriptionTextBox.Text;

            Membership.UpdateUser(user);

            if (ActiveUserCheckBox.Checked)
                user.UnlockUser();

            foreach (GridViewRow row in RolesGridView.Rows)
            {
                CheckBox isUserInRole = row.FindControl("IsUserInRoleCheckBox") as CheckBox;

                Label rolename = row.FindControl("RoleNameLabel") as Label;

                if (isUserInRole == null || rolename == null)
                    continue;

                if (isUserInRole.Checked && !Roles.IsUserInRole(username, rolename.Text))
                    Roles.AddUserToRole(username, rolename.Text);
                else if (!isUserInRole.Checked && Roles.IsUserInRole(username, rolename.Text))
                    Roles.RemoveUserFromRole(username, rolename.Text);
            }
        }
        catch (ProviderException)
        {
        }

        Response.Redirect("~/admin/manageUsers.aspx");
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/manageUsers.aspx");
    }
}
