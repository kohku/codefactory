using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.Wiki;
using CodeFactory.Web;

public partial class Preview : System.Web.UI.Page
{
    private Wiki wiki;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
            BindWikiContent();
    }

    private void BindWikiContent()
    {
        if (string.IsNullOrEmpty(Request.QueryString["id"]))
            return;

        wiki = string.IsNullOrEmpty(Request.QueryString["ischanged"]) || !Convert.ToBoolean(Request.QueryString["ischanged"]) ?
            Wiki.Load(new Guid(Request.QueryString["id"])) : (Wiki)Session[Request.QueryString["id"]];

        if (wiki == null)
            return;

        Page.Title = string.Format("Wiki - {0}", wiki.Title);
        TitleLabel.Text = wiki.Title;
        EditorLabel.Text = string.Format("Autor: {0}{1}", !string.IsNullOrEmpty(wiki.Editor) ? wiki.Editor : "ND",
            !string.IsNullOrEmpty(wiki.DepartmentArea) ? string.Format(" - {0}", wiki.DepartmentArea) : string.Empty);
        ContentLabel.Text = wiki.Content;
        EditButton.Visible = wiki.Editable || User.IsInRole("Administrator");
    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("{0}Entry.aspx?id={1}&ischanged={2}", Utils.RelativeWebRoot,
            Request.QueryString["id"], Request.QueryString["ischanged"]));
    }
}
