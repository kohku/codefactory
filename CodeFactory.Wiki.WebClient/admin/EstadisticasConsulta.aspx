<%@ Page Title="" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="EstadisticasConsulta.aspx.cs" Inherits="admin_EstadisticasConsulta" StyleSheetTheme="Default" Theme="Default" uiCulture="es-MX" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="TheHeaderContent" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .title
        {
	        background-color: #47bce3;
	        font-family: Verdana, Arial, Helvetica, sans-serif;
	        font-size: 11px;
	        font-weight: bold;
	        color: #FFFFFF;
	        text-decoration: none;
        }
        #content #main_title{
            display: block; 
            float: none; 
            width: 770px; 
            vertical-align: middle; 
            height: 25px;
        	border-bottom: 1px solid #00a8de;
        }
        
        #content #search_options{
            display: block; 
            float: none; 
            width: 770px; 
            padding-top: 4px;
            padding-bottom: 4px;
        }
        
        #content #data_title{
            display: block; 
            float: none; 
            width: 770px; 
            padding: 4px 4px 4px 4px;
        }

        #content #data{
            display: block; 
            float: none; 
            width: 770px; 
        }
        #content #options{
            display: block; 
            float: none; 
            width: 770px; 
            padding: 4px 4px 4px 4px;
        }
        #content #actions{
            display: block; 
            float: none; 
            width: 650px;
            margin-top: 12px;
            padding: 4px 4px 4px 4px;
            text-align: left;
        }
        .style5
        {
            width: 110px;
            height: 35px;
        }
        .style6
        {
            width: 190px;
            height: 35px;
        }
        .style7
        {
            width: 80px;
            height: 35px;
        }
        .style8
        {
            height: 35px;
        }
    </style>
</asp:Content>
<asp:Content ID="TheMainContent" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <div id="main_title" class="title">
        <asp:Image ID="StatusIcon" runat="server" ImageUrl="~/images/ico_estatus.gif" />
        <asp:Label ID="TitleLabel" runat="server" Text="Estadísticas de consulta" Height="20px"></asp:Label></div>
    <div id="search_options">
        <table cellpadding="0" cellspacing="1" 
            style="width: 658px; background-color: #EFEFEF;">
            <tr>
                <td align="right" style="width: 110px">
                    <asp:Label ID="FechaInicioLabel" runat="server" Text="Fecha inicio:"></asp:Label></td>
                <td style="width: 190px">
                    <asp:TextBox ID="FechaInicioTextBox" runat="server" Width="70" 
                        CssClass="TextBox_Standard"></asp:TextBox>
                    <asp:ImageButton ID="FechaInicioButton" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        ImageAlign="Middle" /></td>
                <td align="right" style="width: 80px">
                    <asp:Label ID="FechaFinLabel" runat="server" Text="Fecha fin:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="FechaFinTextBox" runat="server" Width="70" 
                        CssClass="TextBox_Standard"></asp:TextBox>
                    <asp:ImageButton ID="FechaFinButton" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png"
                        ImageAlign="Middle" /></td>
            </tr>
            <tr>
                <td align="right" class="style5">
                    </td>
                <td class="style6">
                    </td>
                <td align="right" class="style7">
                </td>
                <td class="style8">
                    <asp:Button ID="SubmitButton" runat="server" CssClass="buttons_blue" 
                        Text="Consultar" onclick="SubmitButton_Click" />&nbsp;<asp:Button ID="ExportExcelButton" runat="server" CssClass="buttons_blue" 
                        onclick="ExportExcelButton_Click" Text="Exportar a Excel" />
                </td>
            </tr>
        </table>
        <cc1:calendarextender ID="FechaInicioCalendarExtender" runat="server" TargetControlID="FechaInicioTextBox"
            PopupButtonID="FechaInicioButton">
        </cc1:calendarextender>
        <cc1:calendarextender ID="FechaFinCalendarExtender" runat="server" TargetControlID="FechaFinTextBox"
            PopupButtonID="FechaFinButton">
        </cc1:calendarextender>
    </div>
    <div id="data_title" runat="server">
        <table style="width: 770px; border: solid 1px 1px 1px 1px #00a8de;" 
            bgcolor="#e0f3fa">
            <tr>
                <td style="width: 36px; text-align: center;">
                    <asp:Image ID="InfoIcon" runat="server" ImageAlign="Middle" ImageUrl="~/images/ico_info.gif" /></td>
                <td>
                    <asp:Label ID="TotalCountLabel" runat="server" Text="La búsqueda arrojó {0} resultados."></asp:Label>
            </tr>
        </table>
    </div>
    <div id="data">
        <asp:GridView ID="TheGridView" runat="server" AutoGenerateColumns="False" DataSourceID="TheDataSource"
            AllowPaging="True" PageSize="25" Width="770px"
            BorderColor="#00A8DE" BorderStyle="Solid" BorderWidth="1px" 
            ondatabound="TheGridView_DataBound">
            <Columns>
                <asp:BoundField DataField="Date" HeaderText="Fecha" SortExpression="Date">
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:HyperLinkField DataNavigateUrlFields="UrlRequested" DataTextField="Title" 
                    HeaderText="Título" />
                <asp:BoundField DataField="UserName" HeaderText="Usuario" 
                    SortExpression="UserName">
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Type" HeaderText="Tipo" SortExpression="Type" />
                <asp:BoundField DataField="Hits" HeaderText="Número de consultas" 
                    SortExpression="Hits" >
                <ItemStyle Width="75px" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle BackColor="#33CCCC" ForeColor="SteelBlue" />
            <AlternatingRowStyle BackColor="#C9F1F1" />
        </asp:GridView>
        <asp:ObjectDataSource ID="TheDataSource" runat="server" OldValuesParameterFormatString="original_{0}"
            OnObjectCreating="TheDataSource_ObjectCreating" SelectMethod="Select" TypeName="StatisticTraceReport"
            EnablePaging="True" SelectCountMethod="TotalCount"></asp:ObjectDataSource>
    </div>
    <div id="options" runat="server">
        <table cellpadding="0" cellspacing="1" style="width: 650px">
            <tr>
                <td style="width: 205px">
                </td>
                <td align="center">
                    <asp:Label ID="MostrarLabel" runat="server" Text="Mostrar"></asp:Label>&nbsp;<asp:DropDownList
                        ID="PageSizeList" runat="server" OnSelectedIndexChanged="PageSizeList_SelectedIndexChanged"
                        AutoPostBack="True" CssClass="DropDownList_Standard">
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                        <asp:ListItem>250</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:Label ID="RegistrosLabel" runat="server" Text="registros por página."></asp:Label></td>
                <td style="width: 205px">
                </td>
            </tr>
            <tr>
                <td style="width: 205px">
                </td>
                <td>
                    &nbsp;</td>
                <td style="width: 205px">
                    </td>
            </tr>
        </table>
    </div>
    <div id="actions">
    <asp:Button ID="BackButton0" runat="server" onclick="BackButton_Click" 
        Text="Regresar" CssClass="buttons_blue" />
    </div>
    <asp:ScriptManager id="TheScriptManager" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True">
    </asp:ScriptManager>
</asp:Content>

