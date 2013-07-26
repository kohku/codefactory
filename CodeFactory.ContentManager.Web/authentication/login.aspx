<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="authentication_login" StyleSheetTheme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="TheForm" runat="server">
    <div>
    
    </div>
    <asp:Login ID="TheLogin" runat="server">
    </asp:Login>
    <asp:HyperLink ID="PasswordRecoveryHyperLink" runat="server" 
        NavigateUrl="~/authentication/passwordRecovery.aspx">Password recovery</asp:HyperLink>
    </form>
</body>
</html>
