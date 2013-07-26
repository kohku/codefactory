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
using CodeFactory.Wiki.Statistics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using CodeFactory.Wiki;

public partial class _Wiki : System.Web.UI.Page
{
    private IWiki wiki;
    private IWiki relatedWiki;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            BindWikiContent();
    }

    private void BindWikiContent()
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            throw new InvalidOperationException("A valid identifier is requested.");

        wiki = Wiki.Load(new Guid(Request.QueryString["id"]));

        if (wiki == null)
            return;

        // Tracing on first request.
        if (!Page.IsPostBack)
            TraceWiki(wiki);

        Page.Title = string.Format("Wiki - {0}", wiki.Title);
        TitleLabel.Text = wiki.Title;
        EditorLabel.Text = string.Format("Autor: {0}{1}", !string.IsNullOrEmpty(wiki.Editor) ? wiki.Editor : "ND",
            !string.IsNullOrEmpty(wiki.DepartmentArea) ? string.Format(" - {0}", wiki.DepartmentArea) : string.Empty);
        ContentLabel.Text = wiki.Content;
        EditButton.Visible = wiki.Editable || User.IsInRole("Administrator");

        relatedWiki = Wiki.GetRelatedWiki(wiki);

        if (relatedWiki != null)
        {
            int max = 3;

            while (max-- > 0 && (relatedWiki == null || relatedWiki.Equals(wiki)))
            relatedWiki = Wiki.GetRandomWiki();

            TitleRelatedLabel.Text = relatedWiki.Title;
            ContentRelatedLabel.Text = relatedWiki.Description;
            RelatedWikiLink.NavigateUrl = TitleRelatedLabel.NavigateUrl = relatedWiki.RelativeLink;
        }
    }

    private void TraceWiki(IWiki wiki)
    {
        if (wiki == null)
            return;

        LogEntry entry = new LogEntry();

        entry.Categories.Clear();
        entry.Categories.Add("Bitacora de consultas");
        entry.Priority = 5;
        entry.Severity = TraceEventType.Information;
        entry.Message = "Wiki consultado";
        entry.ExtendedProperties.Add("id", wiki.ID);
        entry.ExtendedProperties.Add("title", wiki.Title);
        entry.ExtendedProperties.Add("urlRequested", HttpContext.Current != null ?
            HttpContext.Current.Request.Url.PathAndQuery : string.Empty);
        entry.ExtendedProperties.Add("username", HttpContext.Current != null ?
            HttpContext.Current.User.Identity.Name : string.Empty);
        entry.ExtendedProperties.Add("type", wiki.GetType().ToString());

        Logger.Write(entry);
    }

    protected void EditButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            Response.Redirect(string.Format("{0}Entry.aspx?id={1}&ischanged=false", Utils.RelativeWebRoot, 
                Request.QueryString["id"]));
    }
}
