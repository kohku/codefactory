using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;
using CodeFactory.Gallery.Core.Providers;

public partial class Modules_Orders_OrdersQueryRequests : System.Web.UI.Page
{
    protected ObjectDataSource datasource;
    protected int pageIndex;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PerformSearch();
            UpdateView();
        }
    }

    private void PerformSearch()
    {
        datasource = new ObjectDataSource();
        datasource.EnablePaging = true;
        datasource.TypeName = typeof(GalleriesResult).FullName;
        datasource.ObjectCreating += new ObjectDataSourceObjectEventHandler(datasource_ObjectCreating);
        datasource.SelectMethod = "GetProjects";
        datasource.SelectCountMethod = "TotalCount";

        ProjectGridView.DataSource = datasource;
        ProjectGridView.PageIndex = pageIndex;
    }

    private void UpdateView()
    {
        Page.DataBind();

        TotalResults.Text = string.Format("Total {0}, mostrando de {1} al {2}.",
            HttpContext.Current.Items["ProjectsResult_TotalCount"],
            ProjectGridView.PageIndex * ProjectGridView.PageSize + 1,
            (ProjectGridView.PageIndex * ProjectGridView.PageSize + ProjectGridView.Rows.Count >
            (int)HttpContext.Current.Items["ProjectsResult_TotalCount"] ?
            (int)HttpContext.Current.Items["ProjectsResult_TotalCount"] :
            ProjectGridView.PageIndex * ProjectGridView.PageSize + ProjectGridView.Rows.Count));
    }

    private void datasource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new GalleriesResult(null, null, null, null, null, null, null, null);
    }

    protected void ProjectGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        pageIndex = e.NewPageIndex;

        PerformSearch();

        UpdateView();
    }

    protected void ProjectGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView grid = (sender) as GridView;

        if (grid != null)
        {
            GridViewRow row = grid.Rows[e.NewSelectedIndex];
        }
    }
}
