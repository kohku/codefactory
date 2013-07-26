using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Authenticated : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            Response.Redirect(Request.QueryString["ReturnUrl"], true);

        Response.Redirect(FormsAuthentication.DefaultUrl, true);
    }
}
