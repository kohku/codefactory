<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" StyleSheetTheme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Default Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:LoginView ID="TheLoginView" runat="server">
            <AnonymousTemplate>
                <asp:Label ID="AnonymousLabel" runat="server" Text="You are anonymous."></asp:Label>&nbsp;
                <asp:LoginStatus ID="TheLoginStatus" runat="server" />
            </AnonymousTemplate>
            <LoggedInTemplate>
                <asp:Label ID="WelcomeLabel" runat="server" Text="Welcome"></asp:Label>&nbsp;
                <asp:LoginName ID="TheLoginName" runat="server" />&nbsp;
                <asp:LoginStatus ID="TheLoginStatus" runat="server" />
            </LoggedInTemplate>
        </asp:LoginView>
        <br />
        <asp:HyperLink ID="ChangePasswordHyperLink" runat="server" 
            NavigateUrl="~/authentication/changePassword.aspx">Change password</asp:HyperLink>
        <br />
        <asp:HyperLink ID="RegistrationHyperLink" runat="server" 
            NavigateUrl="~/authentication/registration.aspx">Registration</asp:HyperLink>
        <br />
        Yout don&#39;t have a start page.<br />
        <asp:LinkButton ID="ContentAdministrationLinkButton" runat="server" 
            PostBackUrl="~/admin/manageContent.aspx">Content Administration</asp:LinkButton>
        <br />
        <asp:LinkButton ID="SiteMapLinkButton" runat="server" 
            PostBackUrl="~/SiteMap.aspx">Site Map</asp:LinkButton>
        <br />
        <br />
        <br />
        <asp:LinkButton ID="ResizeImageLinkButton" runat="server">Resize Image</asp:LinkButton>
    </div>
    </form>
</body>
</html>
