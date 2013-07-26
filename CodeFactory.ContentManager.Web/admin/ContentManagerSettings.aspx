<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContentManagerSettings.aspx.cs" Inherits="admin_ContentManagerSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Content Manager Settings</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="DefaultPageLabel" runat="server" Text="Default Page:"></asp:Label>
        <asp:TextBox ID="DefaultPageTextBox" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="DefaultLayoutLabel" runat="server" Text="Default Layout:"></asp:Label>
        <asp:TextBox ID="DefaultLayoutTextBox" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="SaveButton" runat="server" Text="Save" 
            onclick="SaveButton_Click" />
    
    </div>
    </form>
</body>
</html>
