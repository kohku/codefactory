<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editUser.aspx.cs" Inherits="admin_editUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Default Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="UserNameLabel" runat="server" Text="User Name"></asp:Label><asp:TextBox ID="UserNameTextBox" runat="server"></asp:TextBox><br />
    <asp:Label ID="EmailLabel" runat="server" Text="Email"></asp:Label><asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox><br />
    <asp:Label ID="CommentsLabel" runat="server" Text="Description"></asp:Label><asp:TextBox ID="DescriptionTextBox" runat="server"></asp:TextBox><br />
    <asp:CheckBox ID="ActiveUserCheckBox" runat="server" Text="Active user" /><br />
    <asp:Button ID="SaveButton" runat="server" Text="Save" 
        onclick="SaveButton_Click" />
    <asp:GridView ID="RolesGridView" runat="server" AutoGenerateColumns="False" 
                        onrowdatabound="RolesGridView_RowDataBound" 
        DataSourceID="RolesDataSource">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:CheckBox ID="IsUserInRoleCheckBox" runat="server" 
                                CommandName="IsUserInRole">
                    </asp:CheckBox>
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
                        TypeName="RolesResult"></asp:ObjectDataSource>
        <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
            Text="Back" />
    </div>
    </form>
</body>
</html>
