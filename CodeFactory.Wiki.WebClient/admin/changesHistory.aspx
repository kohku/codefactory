<%@ Page Title="Wiki - Historial de modificaciones" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="changesHistory.aspx.cs" Inherits="admin_changesHistory" StyleSheetTheme="Default" %>

<%@ Register assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
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
            width: 770px;
            margin-top: 12px;
            padding: 4px 4px 4px 4px;
            text-align: left;
        }
        .style10
        {
            width: 100px;
            text-align: right;
        }
        .style12
        {
            width: 72px;
            text-align: right;
        }
        .style13
        {
            width: 125px;
        }
        .style14
        {
            width: 115px;
        }
        .style16
        {
            width: 105px;
        }
        .style17
        {
            width: 70px;
        }
        .style18
        {
            width: 66px;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <asp:ScriptManager ID="TheScriptManager" runat="server" 
        EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </asp:ScriptManager>
                <cc1:calendarextender ID="DateCreatedExtender" runat="server" 
        TargetControlID="DateCreatedTextBox" PopupButtonID="DateCreatedButton">
                </cc1:calendarextender>
                <cc1:calendarextender ID="DateModifiedExtender" runat="server" 
        TargetControlID="DateModifiedTextBox" PopupButtonID="DateModifiedButton">
                </cc1:calendarextender>
                <cc1:calendarextender ID="ExpirationDateExtender" runat="server" 
        TargetControlID="ExpirationDateTextBox" 
        PopupButtonID="ExpirationDateButton">
                </cc1:calendarextender>
    <asp:Label ID="TitleLabel" runat="server" Text="Historial de Modificaciones" 
        CssClass="subtitles_yellow"></asp:Label>
    <br />
    <asp:Label ID="StatusLabel" runat="server" Text="{0} registros encontrados. Mostrando del {1} al {2}."></asp:Label>
    <br />
    <table width="770" style="background-color: #EFEFEF">
        <tr>
            <td class="style12" style="text-align: right;">Título:</td>
            <td class="style13">
                <asp:TextBox ID="TitleTextBox" runat="server" SkinID="TextBox_Standard" 
                    Width="120px"></asp:TextBox>
            </td>
            <td class="style10">Autor:</td>
            <td class="style16">
                <asp:TextBox ID="AuthorTextBox" runat="server" SkinID="TextBox_Standard" 
                    Width="100px"></asp:TextBox>
            </td>
            <td style="text-align: right;" class="style14">Registrado el día:</td>
            <td class="style13">
                <asp:TextBox ID="DateCreatedTextBox" runat="server" Width="90px" 
                    SkinID="TextBox_Standard"></asp:TextBox>
                <asp:Image ID="DateCreatedButton" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style12" style="text-align: right;">Descripción:</td>
            <td class="style13">
                <asp:TextBox ID="DescriptionTextBox" runat="server" SkinID="TextBox_Standard" 
                    Width="120px"></asp:TextBox>
            </td>
            <td class="style10">Modificado por:</td>
            <td class="style16">
                <asp:TextBox ID="LastModifiedByTextBox" runat="server" SkinID="TextBox_Standard" 
                    Width="100px"></asp:TextBox>
            </td>
            <td style="text-align: right;" class="style14">Modificado el día:</td>
            <td class="style13">
                <asp:TextBox ID="DateModifiedTextBox" runat="server" Width="90px" 
                    SkinID="TextBox_Standard"></asp:TextBox>
                <asp:Image ID="DateModifiedButton" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                </td>
            <td style="text-align: center">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style12" style="text-align: right;">&nbsp;</td>
            <td class="style13">
                &nbsp;</td>
            <td class="style10">&nbsp;</td>
            <td class="style16">
                &nbsp;</td>
            <td style="text-align: right;" class="style14">Expirado el día:</td>
            <td class="style13">
                <asp:TextBox ID="ExpirationDateTextBox" runat="server" Width="90px" 
                    SkinID="TextBox_Standard"></asp:TextBox>
                <asp:Image ID="ExpirationDateButton" runat="server" 
                    ImageUrl="~/images/Calendar_scheduleHS.png" />
                </td>
            <td style="text-align: center">
                <asp:Button ID="FilterButton" runat="server" CssClass="buttons_blue" 
                    onclick="FilterButton_Click" Text="Filtrar" />
                </td>
        </tr>
        </table>
<div id="data">
    <asp:GridView ID="TheWikiHistoryGridView" runat="server" 
        AutoGenerateColumns="False" DataSourceID="TheWikiHistorySource" 
        AllowPaging="True" onrowdatabound="TheWikiHistoryGridView_RowDataBound" 
        Width="770px" BorderStyle="Solid" 
        ondatabound="TheWikiHistoryGridView_DataBound">
        <RowStyle VerticalAlign="Top" />
        <Columns>
            <asp:TemplateField HeaderText="Título" SortExpression="Title">
                <ItemTemplate>
                    <asp:HyperLink ID="TitleLabel" runat="server" Text='<%# Bind("Title") %>' NavigateUrl='<%# Bind("TrackingLink") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Descripción" ReadOnly="True" 
                SortExpression="Description" />
            <asp:TemplateField>
                <ItemTemplate>
                    <table style="width:150px;" cellpadding="0" cellspacing="1" border="0">
                        <tr>
                            <td align="right" class="style18"><asp:Label ID="AuthorTitle" runat="server" 
                                Text="Autor:"></asp:Label></td>
                            <td align="left"><asp:Label ID="Author" runat="server" 
                                Text='<%# Eval("Author") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right" class="style18"><asp:Label ID="LastModifiedByTitle" runat="server" 
                                Text="Modificado:"></asp:Label></td>
                            <td align="left"><asp:Label ID="LastModifiedBy" runat="server" 
                                Text='<%# Eval("LastModifiedBy") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right" class="style18"><asp:Label ID="AuthorizerTitle" runat="server" 
                            Text="Autorizador:"></asp:Label></td>
                            <td align="left"><asp:Label ID="Authorizer" runat="server" 
                            Text='<%# Eval("Authorizer") %>'></asp:Label></asp:Label></td>
                        </tr>
                    </table>
                </ItemTemplate>
                <ItemStyle Width="150px"/>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <table style="width:235px;" cellpadding="0" cellspacing="1" border="0">
                        <tr>
                            <td align="right" class="style17"><asp:Label ID="DateCreatedTitle" runat="server" 
                            Text="Registro:"></asp:Label></td>
                            <td align="left"><asp:Label ID="DateCreated" runat="server" 
                            Text='<%# Eval("DateCreated") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right" class="style17"><asp:Label ID="DateModifiedTitle" runat="server" 
                            Text="Modificación:"></asp:Label></td>
                            <td align="left"><asp:Label ID="LastUpdated" runat="server" 
                            Text='<%# Eval("LastUpdated") %>'></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right" class="style17"><asp:Label ID="ExpirationDateTitle" runat="server" 
                            Text="Expiración:"></asp:Label></td>
                            <td align="left"><asp:Label ID="ExpirationDate" runat="server" 
                            Text='<%# Eval("ExpirationDate") %>'></asp:Label></asp:Label></td>
                        </tr>
                    </table>
                </ItemTemplate>
                <ItemStyle Width="235px"/>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen registros.
        </EmptyDataTemplate>
        <HeaderStyle BackColor="#33CCCC" ForeColor="SteelBlue" />
        <AlternatingRowStyle BackColor="#C9F1F1" />
    </asp:GridView>
    <asp:ObjectDataSource ID="TheWikiHistorySource" runat="server" 
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" 
        onobjectcreating="TheWikiHistorySource_ObjectCreating" 
        SelectCountMethod="TotalCount" SelectMethod="GetResults" 
        TypeName="WikiHistoryResult"></asp:ObjectDataSource>
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
        <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
            Text="Regresar" CssClass="buttons_blue" />
    </div>
</asp:Content>


