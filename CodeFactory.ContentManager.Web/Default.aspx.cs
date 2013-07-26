using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.ContentManager;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // TODO: Set default page in web site settings.
        string defaultPage = ContentManagementService.Settings.DefaultPage;

        if (!VirtualPathUtility.IsAppRelative(defaultPage))
            defaultPage = !defaultPage.ToLowerInvariant().Contains(Request.ApplicationPath.ToLowerInvariant()) ?
                VirtualPathUtility.AppendTrailingSlash(Request.ApplicationPath) + defaultPage : defaultPage;
        else
            defaultPage = VirtualPathUtility.ToAbsolute(defaultPage);

        if (Request.RawUrl.ToLowerInvariant() != defaultPage.ToLowerInvariant())
            Response.Redirect(defaultPage);

        if (!IsPostBack)
        {
            ResizeImageLinkButton.PostBackUrl = string.Format("~/resizeImage.axd?imageUrl={0}&scaleFactor={1}",
                Server.UrlEncode("~/images/noThumbnailAvailable.jpg"), "2");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ContentAdministrationLinkButton.Visible = User.Identity.IsAuthenticated && (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Editor"));

            RegistrationHyperLink.Visible = !User.Identity.IsAuthenticated;
            ChangePasswordHyperLink.Visible = User.Identity.IsAuthenticated;
        }
    }
}
