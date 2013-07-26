<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteMap.aspx.cs" Inherits="SiteMap" StyleSheetTheme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <asp:TreeView ID="TheSiteMap" runat="server" 
        DataSourceID="TheSiteMapDataSource" Width="900px">
    </asp:TreeView>
    <asp:SiteMapDataSource ID="TheSiteMapDataSource" runat="server" />
    </form>
</body>
</html>
