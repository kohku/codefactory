<%@ Page Title="Wiki - Administración de usuarios" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="manageUsers.aspx.cs" Inherits="admin_manageUsers" StyleSheetTheme="Default" Theme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <br />
    <asp:Label ID="TitleLabel" runat="server" Text="Administración de Usuarios" 
        CssClass="subtitles_yellow"></asp:Label>
    <br />
    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/bullet.gif" />
    <asp:LinkButton ID="CreateUserLinkButton" runat="server" 
        PostBackUrl="~/admin/addUser.aspx">Crear usuario nuevo</asp:LinkButton>
    <br />
    <br />
    <asp:Label ID="SearchLabel" runat="server" Text="Búsqueda"></asp:Label>
    &nbsp;<asp:TextBox ID="SearchTextBox"
        runat="server" SkinID="TextBox_Standard"></asp:TextBox>
    &nbsp;<asp:DropDownList ID="SearchSelection" runat="server" 
        SkinID="DropDownList_Standard">
        <asp:ListItem Selected="True" Value="username">Usuario</asp:ListItem>
        <asp:ListItem Value="email">Email</asp:ListItem>
    </asp:DropDownList>
    &nbsp;<asp:Button ID="SearchButton" runat="server" Text="Buscar" 
        onclick="SearchButton_Click" CssClass="buttons_blue" />
    <br />
    <br />
    <asp:GridView ID="MembershipGridView" runat="server" Width="770px" AllowPaging="True" 
        AutoGenerateColumns="False" DataSourceID="MembershipDataSource" 
        onrowdatabound="MembershipGridView_RowDataBound" 
        onselectedindexchanging="MembershipGridView_SelectedIndexChanging" 
        DataKeyNames="username">
        <Columns>
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Bloqueado" 
                ReadOnly="True"/>
            <asp:TemplateField HeaderText="Usuario">
                <ItemTemplate>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="EditUserLinkButton" CommandName="Editar" runat="server">Editar Usuario</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteUserLinkButton" CommandName="Delete" CommandArgument='<%# Bind("UserName") %>' runat="server">Eliminar Usuario</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="EditRolesLinkButton" CommandName="Select" runat="server">Editar Roles</asp:LinkButton>
                    <asp:GridView ID="RolesGridView" runat="server" AutoGenerateColumns="False" 
                        onrowdatabound="RolesGridView_RowDataBound">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="IsUserInRoleCheckBox" runat="server" 
                                CommandName="IsUserInRole" AutoPostBack="True" 
                                    oncheckedchanged="IsUserInRoleCheckBox_CheckedChanged"></asp:CheckBox>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rol">
                            <ItemTemplate>
                                <asp:Label ID="RoleNameLabel" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="RolesDataSource" runat="server" SelectMethod="GetRoles" 
                        TypeName="RolesResult">
                    </asp:ObjectDataSource>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen usuarios registrados.
        </EmptyDataTemplate>
        <HeaderStyle BackColor="#F8F8F8" CssClass="content_bold" />
    </asp:GridView>
    <asp:ObjectDataSource ID="MembershipDataSource" runat="server" 
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" 
        SelectCountMethod="TotalCount" SelectMethod="GetMembers" 
        TypeName="MembershipResult" 
        onobjectcreating="MembershipDataSource_ObjectCreating" 
        DeleteMethod="DeleteUser">
        <DeleteParameters>
            <asp:Parameter Name="username" Type="String" />
            <asp:Parameter Name="original_username" Type="String" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
    <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
        Text="Regresar" CssClass="buttons_blue" />
    <br />
    <br />
</asp:Content>

