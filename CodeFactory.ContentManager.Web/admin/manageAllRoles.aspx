<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageAllRoles.aspx.cs" Inherits="admin_Roles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Default Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="AddRoleLabel" runat="server" Text="Rol"></asp:Label>
    <asp:TextBox ID="AddRoleTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="AddRoleButton" runat="server" Text="Agregar" 
        onclick="AddRoleButton_Click" />
    <br />
    <asp:GridView ID="RolesGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="RolesDataSource" onrowdatabound="RolesGridView_RowDataBound" 
        onrowdeleting="RolesGridView_RowDeleting" Width="350px" 
        onrowcommand="RolesGridView_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Rol Name">
                <ItemTemplate>
                    <asp:Label ID="RolenameLabel" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add/Remove Users">
                <ItemTemplate>
                    <asp:LinkButton ID="ManageLinkButton" runat="server" CausesValidation="False" 
                        CommandName="Manage" Text="Manage"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="125px" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" DeleteMethod="DeleteRole" SelectMethod="GetRoles" 
        TypeName="RolesResult">
        <DeleteParameters>
            <asp:Parameter Name="rolename" Type="String" />
        </DeleteParameters>
    </asp:ObjectDataSource>
        <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
            Text="Back" />
    </div>
    </form>
</body>
</html>
