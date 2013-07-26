using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = (string)GetLocalResourceObject("Title");

        Session["masterGraphic"] = true;
        Session.Remove("guid");

        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        NewButton.Visible = HttpContext.Current.User.IsInRole("Administrator");
        UsersRolesButton.Visible = HttpContext.Current.User.IsInRole("Administrator");
    }

    protected void NewButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProjectWizard.aspx");
    }

    protected void UsersRolesButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Admin/Users.aspx");
    }
}
