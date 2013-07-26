<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageUsers.aspx.cs" Inherits="admin_manageUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Default Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="SearchLabel" runat="server" Text="Búsqueda"></asp:Label><asp:TextBox ID="SearchTextBox"
        runat="server"></asp:TextBox>
    <asp:DropDownList ID="SearchSelection" runat="server">
        <asp:ListItem Selected="True" Value="username">Usuario</asp:ListItem>
        <asp:ListItem Value="email">Email</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="SearchButton" runat="server" Text="Buscar" 
        onclick="SearchButton_Click" />
    <asp:GridView ID="MembershipGridView" runat="server" Width="770px" AllowPaging="True" 
        AutoGenerateColumns="False" DataSourceID="MembershipDataSource" 
        onrowdatabound="MembershipGridView_RowDataBound" 
        onselectedindexchanging="MembershipGridView_SelectedIndexChanging" 
        DataKeyNames="username">
        <Columns>
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Active" 
                ReadOnly="True"/>
            <asp:TemplateField HeaderText="UserName">
                <ItemTemplate>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="EditUserLinkButton" CommandName="EditUser" runat="server">Edit User</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteUserLinkButton" CommandName="Delete" runat="server">Delete User</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="EditRolesLinkButton" CommandName="Select" runat="server">Edit Roles</asp:LinkButton>
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
                        <asp:TemplateField HeaderText="Rol Name">
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
    </asp:GridView>
    <asp:ObjectDataSource ID="MembershipDataSource" runat="server" 
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" 
        SelectCountMethod="TotalCount" SelectMethod="GetMembers" 
        TypeName="MembershipResult" 
        onobjectcreating="MembershipDataSource_ObjectCreating" 
        DeleteMethod="DeleteUser">
        <DeleteParameters>
            <asp:Parameter Name="username" Type="String" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:LinkButton ID="CreateUserLinkButton" runat="server" 
        PostBackUrl="~/admin/addUser.aspx">Crear usuario nuevo</asp:LinkButton>
        <br />
        <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
            Text="Back" />
    </div>
    </form>
</body>
</html>
