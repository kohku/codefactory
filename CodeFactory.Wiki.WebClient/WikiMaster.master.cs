using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Web;

public partial class WikiMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ThePreferencesLink.Visible = HttpContext.Current.User.Identity.IsAuthenticated;
        Page.Form.DefaultButton = SearchButton.UniqueID;
    }

    protected void SearchButton_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(SearchTextBox.Text.Trim()))
        {
            Response.Redirect(string.Format("~/WikiSearch.aspx?q={0}", HttpUtility.HtmlEncode(SearchTextBox.Text)));
        }
    }

    protected void LogoWikiImage_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect(Utils.RelativeWebRoot);
    }

    protected void CreateContentLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Entry.aspx");
    }
}
