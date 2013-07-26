using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_pendingsOfAuthorization : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void TheWorkWikiItemsSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new WorkWikiItemResultSet(Guid.Empty, TitleTextBox.Text, DescriptionTextBox.Text, string.Empty, AuthorTextBox.Text,
            string.Empty, null, string.Empty, string.Empty,
            !string.IsNullOrEmpty(DateCreatedTextBox.Text) ? DateTime.Parse(DateCreatedTextBox.Text) : DateTime.MinValue,
            !string.IsNullOrEmpty(DateCreatedTextBox.Text) ? DateTime.Parse(DateCreatedTextBox.Text).AddDays(1) : DateTime.MinValue,
            !string.IsNullOrEmpty(DateModifiedTextBox.Text) ? DateTime.Parse(DateModifiedTextBox.Text) : DateTime.MinValue,
            !string.IsNullOrEmpty(DateModifiedTextBox.Text) ? DateTime.Parse(DateModifiedTextBox.Text).AddDays(1) : DateTime.MinValue,
            !string.IsNullOrEmpty(ExpirationDateTextBox.Text) ? DateTime.Parse(ExpirationDateTextBox.Text) : DateTime.MinValue,
            !string.IsNullOrEmpty(ExpirationDateTextBox.Text) ? DateTime.Parse(ExpirationDateTextBox.Text).AddDays(1) : DateTime.MinValue,
            LastModifiedByTextBox.Text);
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }
    protected void TheWorkWikiItemsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        CodeFactory.Wiki.Workflow.IWorkWikiItem wiki = e.Row.DataItem as CodeFactory.Wiki.Workflow.IWorkWikiItem;

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

        Label expirationDateTitle = e.Row.FindControl("ExpirationDateTitle") as Label;
        Label expirationDate = e.Row.FindControl("ExpirationDate") as Label;

        if (expirationDate != null && wiki != null && wiki.ExpirationDate.Equals(DateTime.MinValue))
        {
            if (expirationDateTitle != null)
                expirationDateTitle.Visible = false;
            expirationDate.Text = string.Empty;
        }
    }

    protected void TheWorkWikiItemsGridView_DataBound(object sender, EventArgs e)
    {
        StatusLabel.Visible = HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"] != null &&
            (int)HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"] > 0;

        StatusLabel.Text = string.Format("{0} registros encontrados. Mostrando del {1} al {2}.",
            HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"],
            TheWorkWikiItemsGridView.PageIndex * TheWorkWikiItemsGridView.PageSize + 1,
            (int)HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"] <= TheWorkWikiItemsGridView.PageIndex * TheWorkWikiItemsGridView.PageSize + TheWorkWikiItemsGridView.PageSize ?
            (int)HttpContext.Current.Items["WorkWikiItemResultSet_TotalCount"] : TheWorkWikiItemsGridView.PageIndex * TheWorkWikiItemsGridView.PageSize + TheWorkWikiItemsGridView.PageSize);
    }

    protected void FilterButton_Click(object sender, EventArgs e)
    {
        TheWorkWikiItemsGridView.DataBind();
    }

    protected void PageSizeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheWorkWikiItemsGridView.PageIndex = 0;
        TheWorkWikiItemsGridView.PageSize = Convert.ToInt32(PageSizeList.SelectedValue);
        TheWorkWikiItemsGridView.DataBind();
    }
}
