using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_wikiReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void TheWikiDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new WikiResult(TitleTextBox.Text, DescriptionTextBox.Text, string.Empty, AuthorTextBox.Text, string.Empty,
            null, string.Empty, string.Empty, 
            !string.IsNullOrEmpty(DateCreatedTextBox.Text) ? DateTime.Parse(DateCreatedTextBox.Text) : DateTime.MinValue,
            !string.IsNullOrEmpty(DateCreatedTextBox.Text) ? DateTime.Parse(DateCreatedTextBox.Text).AddDays(1) : DateTime.MinValue,
            !string.IsNullOrEmpty(DateModifiedTextBox.Text) ? DateTime.Parse(DateModifiedTextBox.Text) : DateTime.MinValue,
            !string.IsNullOrEmpty(DateModifiedTextBox.Text) ? DateTime.Parse(DateModifiedTextBox.Text).AddDays(1) : DateTime.MinValue,
            LastModifiedByTextBox.Text);
    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }

    protected void TheWikiGridView_DataBound(object sender, EventArgs e)
    {
        StatusLabel.Visible = HttpContext.Current.Items["WikiResultSet_TotalCount"] != null &&
            (int)HttpContext.Current.Items["WikiResultSet_TotalCount"] > 0;

        StatusLabel.Text = string.Format("{0} registros encontrados. Mostrando del {1} al {2}.",
            HttpContext.Current.Items["WikiResultSet_TotalCount"], 
            TheWikiGridView.PageIndex * TheWikiGridView.PageSize + 1,
            (int)HttpContext.Current.Items["WikiResultSet_TotalCount"] <= TheWikiGridView.PageIndex * TheWikiGridView.PageSize + TheWikiGridView.PageSize ?
            (int)HttpContext.Current.Items["WikiResultSet_TotalCount"] : TheWikiGridView.PageIndex * TheWikiGridView.PageSize + TheWikiGridView.PageSize);
    }

    protected void TheWikiGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        CodeFactory.Wiki.IWiki wiki = e.Row.DataItem as CodeFactory.Wiki.IWiki;

        Label author = e.Row.FindControl("AuthorTitle") as Label;
        Label lastModifiedBy = e.Row.FindControl("LastModifiedByTitle") as Label;

        if (wiki != null && string.IsNullOrEmpty(wiki.Author))
            author.Text = "No disponible";

        if (wiki != null && string.IsNullOrEmpty(wiki.LastUpdatedBy))
            lastModifiedBy.Visible = false;

        Label dateCreatedTitle = e.Row.FindControl("DateCreatedTitle") as Label;
        Label dateCreated = e.Row.FindControl("DateCreated") as Label;

        if (dateCreated != null && wiki != null && wiki.DateCreated.Equals(DateTime.MinValue))
        {
            if (dateCreatedTitle != null)
                dateCreatedTitle.Visible = false;
            dateCreated.Text = string.Empty;
        }

        Label dateModifiedTitle = e.Row.FindControl("DateModifiedTitle") as Label;
        Label dateModified = e.Row.FindControl("LastUpdated") as Label;

        if (dateModified != null && wiki != null && wiki.LastUpdated.Equals(DateTime.MinValue))
        {
            if (dateModifiedTitle != null)
                dateModifiedTitle.Visible = false;
            dateModified.Text = string.Empty;
        }
    }

    protected void FilterButton_Click(object sender, EventArgs e)
    {
        TheWikiGridView.DataBind();
    }

    protected void PageSizeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheWikiGridView.PageIndex = 0;
        TheWikiGridView.PageSize = Convert.ToInt32(PageSizeList.SelectedValue);
        TheWikiGridView.DataBind();
    }
}
