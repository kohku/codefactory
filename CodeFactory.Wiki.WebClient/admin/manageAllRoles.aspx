<%@ Page Title="Wiki - Administración de roles" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="manageAllRoles.aspx.cs" Inherits="admin_Roles" StyleSheetTheme="Default" Theme="Default" %>

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
        <asp:Label ID="TitleLabel" runat="server" Text="Administración de Roles" 
            CssClass="subtitles_yellow"></asp:Label>
        <br />
        <br />
        <asp:Label ID="AddRoleLabel" runat="server" Text="Rol"></asp:Label>&nbsp;
    <asp:TextBox ID="AddRoleTextBox" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
    &nbsp;<asp:Button ID="AddRoleButton" runat="server" Text="Agregar" 
        onclick="AddRoleButton_Click" CssClass="buttons_blue" />
        <br />
    <br />
    <asp:GridView ID="RolesGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="RolesDataSource" onrowdatabound="RolesGridView_RowDataBound" 
        onrowdeleting="RolesGridView_RowDeleting" Width="350px" 
        onrowcommand="RolesGridView_RowCommand" BorderColor="#CCCCCC">
        <RowStyle CssClass="DHTR_Grid_Row" />
        <Columns>
            <asp:TemplateField HeaderText="Rol">
                <ItemTemplate>
                    <asp:Label ID="RolenameLabel" runat="server"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="150px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Agregar/Eliminar Usuarios">
                <ItemTemplate>
                    <asp:LinkButton ID="ManageLinkButton" runat="server" CausesValidation="False" 
                        CommandName="Manage" Text="Administrar"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="175px" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen roles.
        </EmptyDataTemplate>
        <HeaderStyle BackColor="#F8F8F8" CssClass="content_bold" />
    </asp:GridView>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" DeleteMethod="DeleteRole" SelectMethod="GetRoles" 
        TypeName="RolesResult">
        <DeleteParameters>
            <asp:Parameter Name="rolename" Type="String" />
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

