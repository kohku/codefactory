<%@ Page Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="CategoryIndex.aspx.cs" Inherits="CategoryIndex" Title="Wiki - Índice de Categorías" StyleSheetTheme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

.titles_blue {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-style: normal;
	font-weight: lighter;
	color: #00ADEF;
	font-size: 16px;
	line-height: 18px;
	text-transform: none;
}
</style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <table width="770" align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td background="images/fondo_tabla.jpg">
            <img src="images/fondo_tabla_top.jpg" width="770" height="15" /></td>
    </tr>
    <tr>
        <td width="766" background="images/fondo_tabla.jpg">
            <table width="731" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td height="35" colspan="3" bgcolor="#f8f8f8">
                        <label class="titles_green">
                        Indice de categorias:</label></td>
                </tr>
                <tr>
                    <td width="534" valign="top">
                        <asp:GridView ID="CategoriresGridView" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" DataSourceID="CategoriesDataSource" PageSize="20" 
                            Width="532px" GridLines="None" 
                            onrowdatabound="CategoriresGridView_RowDataBound" ShowHeader="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Categoría">
                                    <ItemTemplate>
                                        <br /><asp:HyperLink id="CategoryLink" runat="server" CssClass="titles_blue" 
                                            NavigateUrl="~/WikiSearch.aspx">[CategoryLink]</asp:HyperLink><br /><br />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="CategoriesDataSource" runat="server" 
                            SelectMethod="GetCategories" 
                            TypeName="CodeFactory.Wiki.WikiService"></asp:ObjectDataSource>
                    </td>
                    <td width="17" valign="top">
                        &nbsp;</td>
                    <td width="180" valign="top" bgcolor="#F8F8F8">
                        <table width="180" border="0" cellspacing="0" cellpadding="5">
                            <tr>
                                <td width="170" bgcolor="#F8F8F8">
                                <p>
                                    <asp:Label ID="TitleRelatedLabel" runat="server" CssClass="titles_blue">Lorelm ipsum dolor</asp:Label></p>
                                    <asp:Label ID="ContentRelatedLabel" runat="server">Content</asp:Label>
                                <p>
                                    <asp:HyperLink ID="RelatedWikiLink" runat="server">Ver contenido</asp:HyperLink></p>
                            </tr>
                        </table>
                        <p>
                            &nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        &nbsp;</td>
                </tr>
            </table>
            <br />
        </td>
    </tr>
    <tr>
        <td background="images/fondo_tabla.jpg">
            <img src="images/fondo_tabla_bott.jpg" width="770" height="15" /></td>
    </tr>
</table>
</asp:Content>

