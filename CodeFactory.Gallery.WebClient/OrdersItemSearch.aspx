 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrdersItemSearch.aspx.cs" Inherits="Modules_Orders_OrdersItemSearchContainer"%>

<html>
<head>
</head>
<body>
<div class="header1_row">
    <div class="header1_icon">
        <asp:Image ID="HeaderImage" runat="server" ImageUrl="~/Modules/Orders/images/ico_numsoli.gif" />
    </div>
    <div class="header1_message">
        <asp:Label ID="HeaderLabel" runat="server" Text="Tu número de solicitud es: {0}" CssClass="linkLightBlue"></asp:Label>
    </div>
</div>
<div class="alerts_row">
    <div class="alerts_block">
        <div class="alerts_icon">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Modules/Orders/images/ico_alerta.gif" />
        </div>
        <div class="alerts_message">
            <asp:Label ID="AlertMessage" runat="server" Text="Mensaje de Alerta"></asp:Label>
        </div>
    </div>
</div>

<div class="part_block">
    <div class="part_block_white_header">
        <asp:Label ID="AccountHeaderLabel" runat="server" Text="Número de Cuenta: {0}" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_blue_header">
        <asp:Label ID="ProductSearchLabel" runat="server" Text="Búsqueda por producto" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:Label ID="LineLabel" runat="server" Text="Línea" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:DropDownList ID="LineList" runat="server" Width="200px" DataSourceID="lines" DataTextField="Key" DataValueField="Value">
        </asp:DropDownList><asp:ObjectDataSource ID="lines" runat="server" SelectMethod="GetProductLines"
            TypeName="CodeFactory.GalleryManagement.OrderManagement"></asp:ObjectDataSource>
    </div>
    <div class="part_block_white_header">
        <asp:Label ID="BrandLabel" runat="server" Text="Marca" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:DropDownList ID="BrandList" runat="server" Width="200px" DataSourceID="brands" DataTextField="Key" DataValueField="Value">
        </asp:DropDownList><asp:ObjectDataSource ID="brands" runat="server" SelectMethod="GetProductBrands"
            TypeName="CodeFactory.GalleryManagement.OrderManagement"></asp:ObjectDataSource>
    </div>
    <div class="part_block_white_header">
        <asp:Label ID="CapacityLabel" runat="server" Text="Capacidad" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:DropDownList ID="CapacityList" runat="server" Width="200px" DataSourceID="capacities" DataTextField="Key" DataValueField="Value">
            </asp:DropDownList><asp:ObjectDataSource ID="capacities" runat="server" SelectMethod="GetProductCapacities"
            TypeName="CodeFactory.GalleryManagement.OrderManagement"></asp:ObjectDataSource>
    </div>
</div>
<div class="part_block">
    <div class="part_block_blue_header">
        <asp:Label ID="AnotherSearchLabel" runat="server" Text="Otras Búsquedas" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:Label ID="CodeLabel" runat="server" Text="Código" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:TextBox ID="ProductCodeTextBox" runat="server" Width="200px"></asp:TextBox>
    </div>
    <div class="part_block_white_header">
        <asp:Label ID="DescriptionHeaderLabel" runat="server" Text="Descripción" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div class="part_block_white_header">
        <asp:TextBox ID="ProductNameTextBox" runat="server" Width="200px"></asp:TextBox>
    </div>
</div>
<div class="part_block" style="text-align: center;">
    <asp:ImageButton ID="BuscarButton" runat="server" ImageUrl="~/Modules/Orders/images/bot_buscar.gif" OnClick="BuscarButton_Click" />
    <asp:ImageButton ID="CargaBloqueButton" runat="server" ImageUrl="~/Modules/Orders/images/bot_bloque.jpg" OnClick="CargaBloqueButton_Click" />
    <asp:ImageButton ID="LimpiarButton" runat="server" ImageUrl="~/Modules/Orders/images/bot_limpiar.gif" OnClick="LimpiarButton_Click" />
</div>
<div class="part_block" style="width: 462px; margin-left: 3px;">
    <div class="part_block_blue_header" style="text-align: center;">
         <asp:Label ID="ExcelCaptureLabel" runat="server" Text="Archivo de Excel" CssClass="linkLightBlue"></asp:Label>
    </div>
    <div style="display: inline; float: left; width: 235px; padding-left: 2px;"><asp:FileUpload ID="ExcelFileUpload" runat="server" Width="230px" /></div>
    <div style="display: inline; float: left; width: 120px; text-align: center;"><asp:ImageButton ID="LoadImageButton" runat="server" ImageUrl="~/Modules/Orders/images/bot_cargar.gif" OnClick="LoadImageButton_Click" /></div>
    <div style="display: inline; float: left; text-align: center;"><asp:HyperLink ID="ExcelHyperLink" runat="server">Ejemplo de Excel</asp:HyperLink></div>
</div>
<!-- #region REGION DE INFORMACION //-->
<div style="float:left; display: block; width: 470px; vertical-align: middle; height: 40px; background-color: #e0f2f8;  border: 1px solid #00A8DE;">
    <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width: 40px">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Modules/Orders/images/ico_info.gif" /></td>
            <td style="width: 400px; text-align: left;"><asp:Label ID="Label1" runat="server" Text="Este nuevo sistema de filtros te ayudará a encontrar los productos de una manera fácil y rápida."></asp:Label></td>
        </tr>    
    </table>
</div>
<div id="" style="display: block; width: 470px; float: none; height: 15px;">
</div>
<div style="display: block; float: left; vertical-align: middle; width: 470px; height: 40px">
    <div style="display: inline; float: left; width: 150px; text-align: left">
        </div>
    <div style="display: inline; float: left; width: 169px; text-align: center">
        </div>
    <div style="display: inline; float: left; width: 150px; text-align: right">
        <asp:ImageButton ID="BackButton" runat="server" ImageUrl="~/Modules/Orders/images/bot_regresar.gif" OnClick="BackButton_Click" />
    </div>
</div>
<!-- #endregion REGION DE INFORMACION //-->
    <asp:Panel ID="SelectionPanel" runat="server" Width="686px" Height="31px">
        <asp:Label ID="TotalCountLabel" runat="server" Text="{0} productos encontrados." Font-Names="Verdana" Font-Size="10px" ForeColor="#3993CA" Font-Bold="True"></asp:Label><br />
        <asp:Label ID="ItemIndexLabel" runat="server" Text="Mostrando del {0} al {1}." Font-Names="Verdana" Font-Size="10px" ForeColor="#3993CA" Font-Bold="True"></asp:Label></asp:Panel>
    <asp:GridView ID="ResultsGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        PageSize="25" Width="686px" CellPadding="0" OnRowDataBound="ResultsGridView_RowDataBound" OnPageIndexChanging="ResultsGridView_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="C&#243;digo" HeaderStyle-Width="110">
                <ItemTemplate>
                    <asp:Label id="CodeLabel" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripci&#243;n">
                <ItemTemplate>
                    <asp:Label id="DescriptionLabel" runat="server"></asp:Label><br/>
				    <asp:Label id="MessagesLabel" runat="server" CssClass="alerts"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Presentaci&#243;n" HeaderStyle-Width="120">
                <ItemTemplate>
                    <asp:Label id="PresentationLabel" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Peso Kgs" HeaderStyle-Width="75">
                <ItemTemplate>
                    <asp:Label id="WeightLabel" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Precio Lista" HeaderStyle-Width="75" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label id="PriceLabel" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Width="65" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:TextBox id="QuantityTextBox" runat="server" CssClass="standardTextBox" Width="50" MaxLength="5"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings Mode="NextPrevious" NextPageText="Siguiente" Position="TopAndBottom"
            PreviousPageText="Anterior" />
        <PagerStyle Font-Bold="True" Font-Names="Verdana" Font-Size="11px" ForeColor="SteelBlue"
            HorizontalAlign="Right" />
        <RowStyle Font-Names="Verdana" Font-Size="10px" ForeColor="#4F556A" />
        <HeaderStyle BackColor="#5BB322" Font-Names="Verdana" Font-Size="10px" ForeColor="White" />
    </asp:GridView>
    <asp:Panel ID="BloquesPanel" runat="server" Width="647px" HorizontalAlign="Center">
        <table id="CargaPorBloquesTable" border="0" cellpadding="3" cellspacing="0" style="width: 360px">
            <tr class="resultsHeaderBlue">
                <td align="center" class="linkLightBlue">
                    Código</td>
                <td align="center" class="linkLightBlue">
                    Cantidad</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code1" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity1" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code2" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity2" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code3" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity3" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code4" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity4" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code5" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity5" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code6" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity6" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code7" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity7" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code8" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity8" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code9" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity9" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code10" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity10" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code11" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity11" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code12" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity12" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code13" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity13" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="code14" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center" class="resultsAlternateItemsBlue">
                    <asp:TextBox ID="quantity14" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="code15" runat="server" CssClass="standardTextBox" MaxLength="8"
                        Width="100"></asp:TextBox></td>
                <td align="center">
                    <asp:TextBox ID="quantity15" runat="server" CssClass="standardTextBox" MaxLength="5"
                        Width="100"></asp:TextBox></td>
            </tr>
        </table>
    </asp:Panel>

</body>
</html>