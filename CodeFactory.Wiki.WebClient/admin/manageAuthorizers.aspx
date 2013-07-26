<%@ Page Title="Wiki - Administración de autorizadores" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true"
    CodeFile="manageAuthorizers.aspx.cs" Inherits="admin_manageAuthorizers" StylesheetTheme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style5
        {
            width: 225px;
        }
        .style6
        {
            width: 355px;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <table width="100%">
        <tr>
            <td class="style5">
                &nbsp;
            </td>
            <td class="style6">
                &nbsp;&nbsp;
            </td>
            <td class="style5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style5">
            </td>
            <td class="style6">
                <br />
                <asp:Label ID="TitleLabel" runat="server" Text="Administración de Autorizadores" 
                    CssClass="subtitles_yellow"></asp:Label>
                <br />
                <br />
                <asp:Label ID="AddRoleLabel0" runat="server" Text="Categoría"></asp:Label>&nbsp;<asp:DropDownList 
                    ID="CategoryList" runat="server" SkinID="DropDownList_Standard"
                    Width="200px" DataSourceID="CategoriesDataSource" DataTextField="Key" DataValueField="Value">
                </asp:DropDownList>
                <br />
                &nbsp;
                <asp:Label ID="AddUserLabel" runat="server" Text="Usuario"></asp:Label>&nbsp;
                <asp:DropDownList ID="AuthorizersList" runat="server" SkinID="DropDownList_Standard"
                    Width="200px" DataSourceID="TheAuthorizersSource" DataTextField="Name" 
                    DataValueField="Name">
                </asp:DropDownList>
                &nbsp;<asp:Button ID="AddRuleButton" runat="server" Text="Agregar" OnClick="AddRoleButton_Click"
                    CssClass="buttons_blue" />
                <br />
                <br />
                <asp:GridView ID="TheWikiAuthorizersGridView" runat="server" AutoGenerateColumns="False"
                    DataSourceID="TheWikiAuthorizersSource" Width="300px" 
                    BorderColor="#CCCCCC" AllowPaging="True" DataKeyNames="Key">
                    <RowStyle CssClass="DHTR_Grid_Row" />
                    <Columns>
                        <asp:BoundField DataField="Key" HeaderText="Categoría" ReadOnly="True" 
                            SortExpression="Key" />
                        <asp:BoundField DataField="Value" HeaderText="Usuario" ReadOnly="True" 
                            SortExpression="Value" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                    <EmptyDataTemplate>
                        No existen autorizadores.
                    </EmptyDataTemplate>
                    <HeaderStyle BackColor="#F8F8F8" CssClass="content_bold" />
                </asp:GridView>
                <br />
                <asp:Button ID="BackButton" runat="server" OnClick="BackButton_Click" Text="Regresar"
                    CssClass="buttons_blue" />
            </td>
            <td class="style5">
            </td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;
            </td>
            <td class="style6">
                &nbsp;&nbsp;
            </td>
            <td class="style5">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="TheWikiAuthorizersSource" runat="server"
        SelectMethod="GetWikiAuthorizers" TypeName="WikiAuthorizersResult" EnablePaging="True"
        SelectCountMethod="TotalCount" DeleteMethod="DeleteWikiAuthorizer">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="TheAuthorizersSource" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAuthorizers" 
        TypeName="MemberAuthorizersResults"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="CategoriesDataSource" runat="server" 
                    DeleteMethod="DeleteCategory" OldValuesParameterFormatString="original_{0}" 
                    SelectMethod="GetCategories" TypeName="WikiCategoriesResult" 
        onobjectcreating="CategoriesDataSource_ObjectCreating">
                    <DeleteParameters>
                        <asp:Parameter Name="category" Type="String" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                </asp:Content>
