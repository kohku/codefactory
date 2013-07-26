using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeFactory.Wiki.Workflow;

public partial class Worklist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void TheGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType.Equals(DataControlRowType.DataRow))
        {
            WorkWikiItem item = e.Row.DataItem as WorkWikiItem;

            HyperLink wikiLink = e.Row.FindControl("WikiLink") as HyperLink;

            if (item != null && wikiLink != null)
            {
                wikiLink.Text = item.Title;
                wikiLink.NavigateUrl = string.Format("~/AuthorizeWiki.aspx?trackingNumber={0}", item.TrackingNumber);
            }

            Label author = e.Row.FindControl("AuthorLabel") as Label;

            if (item != null && author != null)
                author.Text = item.Author;

            Label creationDate = e.Row.FindControl("CreationDateLabel") as Label;

            if (item != null && creationDate != null)
                creationDate.Text = !item.LastUpdated.Equals(DateTime.MinValue) ? item.LastUpdated.ToString() :
                    item.DateCreated.ToString();

            Label expirationDate = e.Row.FindControl("ExpirationDateLabel") as Label;

            if (item != null && expirationDate != null && !item.ExpirationDate.Equals(DateTime.MinValue))
                expirationDate.Text = item.ExpirationDate.ToString();
        }
    }
}
