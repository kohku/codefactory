<%@ Page Title="Wiki - Editar usuario" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="editUser.aspx.cs" Inherits="admin_editUser" StyleSheetTheme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style5
        {
            width: 200px;
        }
        .style6
        {
            width: 82px;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <br />
    <asp:Label ID="TitleLabel" runat="server" Text="Editar Usuario" 
        CssClass="subtitles_yellow"></asp:Label>
    <br />
    <table width="100%">
    <tr>
    <td class="style5">
    </td>
    <td class="style6">
    </td>
    <td class="style5">
        &nbsp;</td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6">
    <asp:Label ID="UserNameLabel" runat="server" Text="User Name"></asp:Label>
    </td>
    <td class="style5">
    <asp:TextBox ID="UserNameTextBox" runat="server" SkinID="TextBox_Standard" 
            Width="190px" ReadOnly="True"></asp:TextBox>
    </td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6">
    <asp:Label ID="EmailLabel" runat="server" Text="Email"></asp:Label>
    </td>
    <td class="style5">
    <asp:TextBox ID="EmailTextBox" runat="server" SkinID="TextBox_Standard" 
            Width="190px"></asp:TextBox>
    </td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6">
    <asp:Label ID="CommentsLabel" runat="server" Text="Description"></asp:Label>
    </td>
    <td class="style5">
    <asp:TextBox ID="DescriptionTextBox" runat="server" SkinID="TextBox_Standard" 
            Width="190px"></asp:TextBox>
    </td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6" valign="top">
        &nbsp;</td>
    <td class="style5" valign="top">
        &nbsp;</td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6" valign="top">
    <asp:CheckBox ID="ActiveUserCheckBox" runat="server" Text="Active user" />
        </td>
    <td class="style5" valign="top">
    <asp:GridView ID="RolesGridView" runat="server" AutoGenerateColumns="False" 
                        onrowdatabound="RolesGridView_RowDataBound" 
        DataSourceID="RolesDataSource" Width="190px">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:CheckBox ID="IsUserInRoleCheckBox" runat="server" 
                                CommandName="IsUserInRole">
                    </asp:CheckBox>
                </ItemTemplate>
                <ItemStyle Width="25px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rol">
                <ItemTemplate>
                    <asp:Label ID="RoleNameLabel" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle BackColor="#F8F8F8" CssClass="content_bold" />
    </asp:GridView>
    </td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6">
        &nbsp;</td>
    <td class="style5">
        &nbsp;&nbsp;</td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6">
        &nbsp;</td>
    <td class="style5">
    <asp:Button ID="SaveButton" runat="server" Text="Guardar" 
        onclick="SaveButton_Click" CssClass="buttons_blue" />
        &nbsp;<asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
        Text="Cancelar" CssClass="buttons_blue" />
        </td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    <tr>
    <td class="style5">
        &nbsp;</td>
    <td class="style6">
        &nbsp;</td>
    <td class="style5">
        &nbsp;&nbsp;</td>
    <td class="style5">
        &nbsp;</td>
    </tr>
    </table>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" SelectMethod="GetRoles" 
                        TypeName="RolesResult"></asp:ObjectDataSource>
    <br />
</asp:Content>
