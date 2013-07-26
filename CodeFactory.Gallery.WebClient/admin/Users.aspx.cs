using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_Users : System.Web.UI.Page
{
    private string username = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Label4.Text = string.Empty;

        if (ViewState["username"] != null)
            username = (string)ViewState["username"];
    }

    protected void MembershipUserDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        TextBox password = MembershipUserDetailsView.FindControl("PasswordTextBox") as TextBox;

        if (password != null)
            e.Values.Add("Password", password.Text);
    }

    protected void Page_Error(object sender, EventArgs e)
    {
    }

    protected void MembershipUserDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        if (! UsersResult.Message.Equals(String.Empty))
            Label4.Text = UsersResult.Message;
        else
            Label4.Text = string.Empty;
    }
    protected void MembershipUsersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        e.OldValues.Clear();

    }
    protected void MembershipUserDetailsView_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {

    }
    protected void MembershipUsersGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label usernameLabel = MembershipUsersGridView.Rows[MembershipUsersGridView.SelectedIndex].FindControl(
            "UserNameLabel") as Label;

        if (usernameLabel != null && !string.IsNullOrEmpty(usernameLabel.Text))
        {
            ViewState["username"] = username = usernameLabel.Text;
            UsersRoleGridView.DataSourceID = "RolesDataSource";
            UsersRoleGridView.DataBind();
        }
    }

    protected void UsersRoleGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox role = e.Row.FindControl("RoleCheckBox") as CheckBox;

            if (role != null && !string.IsNullOrEmpty(username))
            {
                role.Checked = Roles.IsUserInRole(username, role.Text);
            }
        }
    }

    protected void RoleCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox role = sender as CheckBox;

        if (role != null)
        {
            if (role.Checked)
            {
                if (!Roles.IsUserInRole(username, role.Text))
                    Roles.AddUserToRole(username, role.Text);
            }
            else
            {
                if (Roles.IsUserInRole(username, role.Text))
                    Roles.RemoveUserFromRole(username, role.Text);
            }
        }
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.ApplicationPath + "/Default.aspx");
    }
}
