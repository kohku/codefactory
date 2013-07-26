using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_EstadisticasConsulta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FechaInicioTextBox.Text = DateTime.Now.ToShortDateString();
            FechaFinTextBox.Text = DateTime.Now.ToShortDateString(); 
            TheGridView.PageSize = Convert.ToInt32(PageSizeList.SelectedValue);
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        TheGridView.PageIndex = 0;
        TheGridView.DataBind();
    }

    protected void TheDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        DateTime fechaInicio = DateTime.MinValue;
        DateTime fechaFin = DateTime.MinValue;

        if (!string.IsNullOrEmpty(FechaInicioTextBox.Text))
            fechaInicio = DateTime.Parse(FechaInicioTextBox.Text);

        if (!string.IsNullOrEmpty(FechaFinTextBox.Text))
            fechaFin = DateTime.Parse(FechaFinTextBox.Text).AddDays(1);

        e.ObjectInstance = new StatisticTraceReport(new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day),
            new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day), string.Empty, string.Empty, null, string.Empty, string.Empty);
    }

    protected void PageSizeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TheGridView.PageIndex = 0;
        TheGridView.PageSize = Convert.ToInt32(PageSizeList.SelectedValue);
        TheGridView.DataBind();
    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/default.aspx");
    }

    protected void TheGridView_DataBound(object sender, EventArgs e)
    {
        options.Visible = TheGridView.Rows.Count > 0;
        TotalCountLabel.Text = (int)HttpContext.Current.Items["StatisticTraceReport_TotalCount"] > 0 ?
            string.Format("La búsqueda arrojó {0} resultados. Mostrando del {1} al {2}",
                HttpContext.Current.Items["StatisticTraceReport_TotalCount"],
            TheGridView.PageIndex * TheGridView.PageSize + 1,
            (int)HttpContext.Current.Items["StatisticTraceReport_TotalCount"] <= TheGridView.PageIndex * TheGridView.PageSize + TheGridView.PageSize ?
            (int)HttpContext.Current.Items["StatisticTraceReport_TotalCount"] : TheGridView.PageIndex * TheGridView.PageSize + TheGridView.PageSize) :
                "La búsqueda no arrojo resultados.";

    }

    protected void ExportExcelButton_Click(object sender, EventArgs e)
    {
        DateTime fechaInicio = DateTime.MinValue;
        DateTime fechaFin = DateTime.MinValue;

        if (!string.IsNullOrEmpty(FechaInicioTextBox.Text))
            fechaInicio = DateTime.Parse(FechaInicioTextBox.Text);

        if (!string.IsNullOrEmpty(FechaFinTextBox.Text))
            fechaFin = DateTime.Parse(FechaFinTextBox.Text).AddDays(1);

        Response.Redirect(string.Format(
            "~/RequestExcelReport.ashx?request={0}&sd={1}&fd={2}",
            new object[] { "statisticsTrace", fechaInicio.ToBinary(), fechaFin.ToBinary()}));
    }
}
