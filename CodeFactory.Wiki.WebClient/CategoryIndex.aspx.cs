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
using CodeFactory.Wiki;

public partial class CategoryIndex : System.Web.UI.Page
{
    private IWiki randomWiki;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            randomWiki = Wiki.GetRandomWiki();

            if (randomWiki != null)
            {
                TitleRelatedLabel.Text = randomWiki.Title;
                ContentRelatedLabel.Text = randomWiki.Description;
                RelatedWikiLink.NavigateUrl = randomWiki.RelativeLink;
            }
        }
    }
    protected void CategoriresGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink category = e.Row.FindControl("CategoryLink") as HyperLink;

            if (category != null)
            {
                category.Text = e.Row.DataItem.ToString();
                category.NavigateUrl = string.Format("~/WikiSearch.aspx?cat={0}", 
                    HttpUtility.UrlEncode(e.Row.DataItem.ToString()));
            }
        }
    }
}
