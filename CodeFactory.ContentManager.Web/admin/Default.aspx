﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:LinkButton ID="HomeLinkButton" runat="server" 
            PostBackUrl="~/Default.aspx">Home</asp:LinkButton><br />
        Security<br />
        Users<br />
        <asp:LinkButton ID="ManageUsersLinkButton" runat="server" 
            PostBackUrl="~/admin/manageUsers.aspx">Manage Users</asp:LinkButton><br />
        <asp:LinkButton ID="CreateUserLinkButton" runat="server" 
            PostBackUrl="~/admin/addUser.aspx">Create User</asp:LinkButton><br />
        Roles<br />
        <asp:LinkButton ID="ManageRolesLinkButton" runat="server" 
            PostBackUrl="~/admin/manageAllRoles.aspx">Manage Roles</asp:LinkButton><br />
    </div>
    </form>
</body>
</html>
