<%@ Page Title="Wiki - Administración de Categorías" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="manageCategories.aspx.cs" Inherits="admin_manageCategories" StyleSheetTheme="Default" Theme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style7
        {
            width: 190px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <table width="100%">
    <tr>
    <td class="style7"></td>
    <td>
        <br />
        <asp:Label ID="TitleLabel" runat="server" Text="Administración de Categorías" 
            CssClass="subtitles_yellow"></asp:Label>
        <br />
        <br />
        <asp:Label ID="AddCategoryLabel" runat="server" Text="Categoría"></asp:Label>&nbsp;
    <asp:TextBox ID="AddCategoryTextBox" runat="server" SkinID="TextBox_Standard" 
            MaxLength="50"></asp:TextBox>
    &nbsp;<asp:Button ID="AddCategoryButton" runat="server" Text="Agregar" 
        onclick="AddCategoryButton_Click" CssClass="buttons_blue" />
        <br />
    <br />
    <asp:GridView ID="CategoriesGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="CategoriesDataSource" 
        onrowdeleting="CategoriesGridView_RowDeleting" Width="350px" 
            BorderColor="#CCCCCC" onrowdatabound="CategoriesGridView_RowDataBound">
        <RowStyle CssClass="DHTR_Grid_Row" />
        <Columns>
            <asp:TemplateField HeaderText="Categoría" SortExpression="Key">
                <ItemTemplate>
                    <asp:Label ID="CategoryLabel" runat="server" Text='<%# Bind("Key") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen categorías.
        </EmptyDataTemplate>
        <HeaderStyle BackColor="#F8F8F8" CssClass="content_bold" />
    </asp:GridView>
    <asp:ObjectDataSource ID="CategoriesDataSource" runat="server" 
            DeleteMethod="DeleteCategory" SelectMethod="GetCategories" 
        TypeName="WikiCategoriesResult" 
            onobjectcreating="CategoriesDataSource_ObjectCreating">
        <DeleteParameters>
            <asp:Parameter Name="category" Type="String" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <br />
    <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
        Text="Regresar" CssClass="buttons_blue" />
        <br />
    <br />
    <br />
    </td>
    <td class="style7">&nbsp;</td>
    </tr>
    </table>
    
</asp:Content>

