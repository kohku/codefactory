using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_addUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void TheCreateUserWizard_CreatedUser(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }
}
