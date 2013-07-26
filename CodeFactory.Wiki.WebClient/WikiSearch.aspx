<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WikiSearch.aspx.cs" Inherits="WikiSearch"
    MasterPageFile="~/WikiMaster.master" StylesheetTheme="Default" Theme="Default" Title="Wiki - Búsqueda de Artículos" %>

<asp:Content ContentPlaceHolderID="MainPlaceHolder" ID="MainHolder" runat="server">
    <table width="770" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td background="images/fondo_tabla.jpg">
                <img src="images/fondo_tabla_top.jpg" width="770" height="15" />
            </td>
        </tr>
        <tr>
            <td width="766" background="images/fondo_tabla.jpg">
                <table width="731" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="732">
                            <label>
                            </label>
                            <p class="resultados">
                                <asp:Label ID="TitleLabel" runat="server" Text="Resultados de la búsqueda"></asp:Label>
                                <br />
                                <br />
                                <img src="images/linea.jpg" width="732" height="7" /></p>
                            <asp:GridView ID="ResultsGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                DataSourceID="ResultsDataSource" Width="731px" GridLines="None" 
                                ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        <p>
                                        <asp:HyperLink ID="TitleLabel" runat="server" CssClass="subtitles_blue" 
                                                NavigateUrl='<%# Eval("RelativeLink") %>'><%# Eval("Title") %></asp:HyperLink><br />
                                        <asp:Label ID="DescriptionLabel" runat="server" CssClass="content" 
                                                Text='<%# Eval("Description") %>'></asp:Label>&nbsp;<asp:HyperLink 
                                                ID="ArticleLink" runat="server" NavigateUrl='<%# Eval("RelativeLink") %>'>[Ver contenido]</asp:HyperLink>
                                        </p>
                                        <p>&nbsp;</p>                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Lo sentimos no existen coincidencias.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td background="images/fondo_tabla.jpg">
                <img src="images/fondo_tabla_bott.jpg" width="770" height="15" />
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ResultsDataSource" runat="server" EnablePaging="True" OnObjectCreating="ResultsDataSource_ObjectCreating"
        SelectCountMethod="TotalCount" SelectMethod="GetResults" TypeName="WikiSearchResult">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
