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

public partial class authentication_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void WikiLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (Membership.ValidateUser(WikiLogin.UserName, WikiLogin.Password))
        {
            e.Authenticated = true;
            return;
        }
        else if (Membership.Providers["XmlMembershipProvider"].ValidateUser(WikiLogin.UserName, WikiLogin.Password))
        {
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");

            if (!Roles.IsUserInRole(WikiLogin.UserName, "Administrator"))
                Roles.AddUserToRole(WikiLogin.UserName, "Administrator");

            if (!Roles.RoleExists("Authorizer"))
                Roles.CreateRole("Authorizer");

            e.Authenticated = true;
            return;
        }
        else if (FormsAuthentication.Authenticate(WikiLogin.UserName, WikiLogin.Password))
        {
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");

            if (!Roles.IsUserInRole(WikiLogin.UserName, "Administrator"))
                Roles.AddUserToRole(WikiLogin.UserName, "Administrator");

            if (!Roles.RoleExists("Authorizer"))
                Roles.CreateRole("Authorizer");

            e.Authenticated = true;
            return;
        }

        e.Authenticated = false;
    }

    protected void WikiLogin_LoggedIn(object sender, EventArgs e)
    {
        Response.Redirect(FormsAuthentication.DefaultUrl);
    }
}
