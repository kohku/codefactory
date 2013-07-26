using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeFactory.Gallery.Core;
using CodeFactory.Gallery.Core.Providers;
using CodeFactory.Web.Storage;

public partial class Modules_Orders_OrdersItemSearchContainer : System.Web.UI.Page
{
    protected ObjectDataSource datasource;
//    protected QueryItemParameters input;
    protected TipoCaptura tipoCaptura = TipoCaptura.Busqueda;

    public enum TipoCaptura
    {
        Busqueda,
        CargaPorBloques,
        CartaPorExcel
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    input = new QueryItemParameters();
        //    input.PageSize = ResultsGridView.PageSize;

        //    ViewState["QueryItemParameters"] = input;

        //    UpdateView();
        //}
        //else
        //{
        //    input = (QueryItemParameters)ViewState["QueryItemParameters"];
        //}
    }

    private void UpdateView()
    {
        Page.DataBind();

        #region HEADER PALETTES
        Image step1 = Master.FindControl("Step1Image") as Image;
        step1.ImageUrl = "~/Modules/Orders/images/pd_01c.gif";

        Image step2 = Master.FindControl("Step2Image") as Image;
        step2.ImageUrl = "~/Modules/Orders/images/pd_02c.gif";

        Image step3 = Master.FindControl("Step3Image") as Image;
        step3.ImageUrl = "~/Modules/Orders/images/pd_03b.gif";

        Image step4 = Master.FindControl("Step4Image") as Image;
        step4.ImageUrl = "~/Modules/Orders/images/pd_04.gif";

        Image step5 = Master.FindControl("Step5Image") as Image;
        step5.ImageUrl = "~/Modules/Orders/images/pd_05.gif";

        Image step6 = Master.FindControl("Step6Image") as Image;
        step6.ImageUrl = "~/Modules/Orders/images/pd_06.gif";

        Image left = Master.FindControl("LeftImage") as Image;
        left.ImageUrl = "~/Modules/Orders/images/fondo2.gif";
        #endregion 

        SelectionPanel.Visible = (tipoCaptura == TipoCaptura.Busqueda) && (ResultsGridView.Rows.Count > 0);
        ResultsGridView.Visible = (tipoCaptura == TipoCaptura.Busqueda);
        BloquesPanel.Visible = (tipoCaptura == TipoCaptura.CargaPorBloques);

        ItemIndexLabel.Text = string.Format("Mostrando del {0} al {1}.",
            ResultsGridView.PageIndex * ResultsGridView.PageSize + 1,
            ResultsGridView.PageIndex * ResultsGridView.PageSize + ResultsGridView.Rows.Count);
        TotalCountLabel.Text = string.Format("{0} productos encontrados.",
            HttpContext.Current.Items["ItemsResult_TotalCount"]);

        //if (input.Line != string.Empty)
        //    LineList.SelectedIndex = LineList.Items.IndexOf(LineList.Items.FindByValue(input.Line));

        //if (input.Brand != string.Empty)
        //    BrandList.SelectedIndex = BrandList.Items.IndexOf(BrandList.Items.FindByValue(input.Brand));

        //if (input.Capacity != Decimal.Zero)
        //    CapacityList.SelectedIndex = CapacityList.Items.IndexOf(
        //        CapacityList.Items.FindByValue(input.Capacity.ToString()));
    }

    private void PerformSearch()
    {
        datasource = new ObjectDataSource();
        datasource.EnablePaging = true;
        //datasource.TypeName = typeof(ItemsResult).FullName;
        datasource.ObjectCreating += new ObjectDataSourceObjectEventHandler(datasource_ObjectCreating);
        datasource.SelectMethod = "GetResults";
        datasource.SelectCountMethod = "TotalCount";

        ResultsGridView.DataSource = datasource;
        //ResultsGridView.PageIndex = input.PageIndex;
    }

    private void datasource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        //e.ObjectInstance = new ItemsResult(input);
    }

    protected void ResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Item item = e.Row.DataItem as Item;

        //    Label code = e.Row.FindControl("CodeLabel") as Label;

        //    if (code != null && item != null)
        //        code.Text = item.Code;

        //    Label description = e.Row.FindControl("DescriptionLabel") as Label;

        //    if (description != null && item != null)
        //        description.Text = item.Name;

        //    Label messages = e.Row.FindControl("MessagesLabel") as Label;

        //    Label presentation = e.Row.FindControl("PresentationLabel") as Label;

        //    if (presentation != null && item != null)
        //        presentation.Text = item.Presentation;

        //    Label weight = e.Row.FindControl("WeightLabel") as Label;

        //    if (weight != null && item != null)
        //        weight.Text = item.Weigth.ToString();

        //    Label price = e.Row.FindControl("PriceLabel") as Label;

        //    if (price != null && item != null)
        //        price.Text = String.Format("{0:C}", 0);

        //    TextBox quantity = e.Row.FindControl("QuantityTextBox") as TextBox;

        //    if (quantity != null)
        //        quantity.Attributes.Add("AUTOCOMPLETE", "OFF");
        //}
    }

    protected void ResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //input.PageIndex = e.NewPageIndex;

        PerformSearch();

        UpdateView();
    }

    protected void BuscarButton_Click(object sender, ImageClickEventArgs e)
    {
        //input.PageIndex = 0;
        //input.ProductCode = ProductCodeTextBox.Text.ToUpperInvariant();
        //input.ProductName = ProductNameTextBox.Text.ToUpperInvariant();
        //input.Line = LineList.SelectedValue.ToUpperInvariant();
        //input.Brand = BrandList.SelectedValue.ToUpperInvariant();

        //try
        //{
        //    if (CapacityList.SelectedValue != string.Empty)
        //        input.Capacity = Convert.ToDecimal(CapacityList.SelectedValue);
        //}
        //catch (FormatException)
        //{
        //    // Ignore exception
        //}

        //ViewState["QueryItemParameters"] = input;

        PerformSearch();

        tipoCaptura = TipoCaptura.Busqueda;

        UpdateView();
    }

    protected void CargaBloqueButton_Click(object sender, ImageClickEventArgs e)
    {
        tipoCaptura = TipoCaptura.CargaPorBloques;

        UpdateView();
    }

    protected void LimpiarButton_Click(object sender, ImageClickEventArgs e)
    {
        tipoCaptura = TipoCaptura.Busqueda;

        //input = new QueryItemParameters();
        //input.PageSize = ResultsGridView.PageSize;

        //ViewState["QueryItemParameters"] = input;

        UpdateView();
    }

    protected void BackButton_Click(object sender, ImageClickEventArgs e)
    {
        //OrderSettingsHandler settings = 
        //    (OrderSettingsHandler)System.Configuration.ConfigurationManager.GetSection(
        //    "comexnet/orderSettings");

        //Server.Transfer(Request.ApplicationPath + settings.navigation.urlOrderCapture);
    }

    protected void LoadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        if (ExcelFileUpload.PostedFile.ContentLength > 0)
        {
            UploadedFile file = new UploadedFile();
            file.FileName = ExcelFileUpload.PostedFile.FileName;
            file.ContentType = ExcelFileUpload.PostedFile.ContentType;
            file.ContentLength = ExcelFileUpload.PostedFile.ContentLength;
            file.InputStream = ExcelFileUpload.PostedFile.InputStream;

            file.Save();
        }
    }
}
