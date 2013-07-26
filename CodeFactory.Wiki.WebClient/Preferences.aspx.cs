using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Preferences : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ChangePasswordLinkButton.Enabled = !User.Identity.Name.Contains(Path.DirectorySeparatorChar.ToString());
        }
    }
}
