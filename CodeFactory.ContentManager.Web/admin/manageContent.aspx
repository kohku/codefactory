<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageContent.aspx.cs" Inherits="admin_manageContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Content Administration</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Content Administration<br />
        <asp:LinkButton ID="NewPageLinkButton" runat="server" 
            onclick="NewPageLinkButton_Click">New Page</asp:LinkButton>
        <br />
        <asp:LinkButton ID="SiteMapLinkButton" runat="server" 
            PostBackUrl="~/SiteMap.aspx">Site map</asp:LinkButton>
    
        <br />
        <asp:HyperLink ID="SectionsHyperLink" runat="server" 
            NavigateUrl="~/admin/manageSections.aspx">Sections</asp:HyperLink>
        <br />
        <asp:HyperLink ID="CategoriesHyperLink" runat="server" 
            NavigateUrl="~/admin/manageCategories.aspx">Categories</asp:HyperLink>
    
        <br />
        Security<br />
        <asp:LinkButton ID="ManageSecurityLinkButton" runat="server" 
            PostBackUrl="~/admin/default.aspx">Manage Security</asp:LinkButton></div>
    </form>
</body>
</html>
