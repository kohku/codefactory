using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CodeFactory.Web;
using CodeFactory.Wiki.Workflow;

public partial class admin_wikiHistory : System.Web.UI.Page
{
    private IWorkWikiItem wiki;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["trackingNumber"]))
                BindWikiContent();

            UpdateView();
        }
    }

    private void BindWikiContent()
    {
        if (string.IsNullOrEmpty(Request.QueryString["trackingNumber"]))
            throw new InvalidOperationException("A valid identifier is requested.");

        wiki = WikiHistory.Load(new Guid(Request.QueryString["trackingNumber"]));

        if (wiki == null)
            return;

        Page.Title = string.Format("Wiki - {0}", wiki.Title);
        TitleLabel.Text = string.Format("{0} ({1})", wiki.Title, wiki.Status == WikiStatus.AuthorizationAccepted ?
            "Autorizado" : "Rechazado");
        EditorLabel.Text = string.Format("Autor: {0}{1}", !string.IsNullOrEmpty(wiki.Editor) ? wiki.Editor : "ND",
            !string.IsNullOrEmpty(wiki.DepartmentArea) ? string.Format(" - {0}", wiki.DepartmentArea) : string.Empty);
        ContentLabel.Text = wiki.Content;
        BackButton.Visible = wiki.Editable || User.IsInRole("Administrator");
    }

    private void UpdateView()
    {
    }
}
