<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrdersQueryRequests.aspx.cs" Inherits="Modules_Orders_OrdersQueryRequests" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gallery</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="SearchDropDownList" runat="server">
            <asp:ListItem Value="userId">LastUpdatedBy</asp:ListItem>
            <asp:ListItem Value="author">Author</asp:ListItem>
            <asp:ListItem Value="title">Title</asp:ListItem>
            <asp:ListItem Value="keywords">Keywords</asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="SearchTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" /><br />
        <br />
        <asp:GridView ID="ProjectGridView" runat="server" AllowPaging="True" OnPageIndexChanging="ProjectGridView_PageIndexChanging" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanging="ProjectGridView_SelectedIndexChanging" PageSize="25">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
        <asp:Label ID="TotalResults" runat="server" Text="Total {0}, mostrando de {1} al {2}."></asp:Label>
    </div>
    </form>
</body>
</html>
