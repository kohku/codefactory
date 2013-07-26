<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageCategories.aspx.cs" Inherits="admin_manageCategories" %>

<%@ Register assembly="CodeFactory.ContentManager" namespace="CodeFactory.ContentManager.WebControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="CategoryTreeView" runat="server" 
            DataSourceID="CategoryTreeSource" ShowLines="True" ExpandDepth="0" 
            onselectednodechanged="CategoryTreeView_SelectedNodeChanged" >
        </asp:TreeView>
        <cc1:CategoryDataSource ID="CategoryTreeSource" runat="server">
        </cc1:CategoryDataSource>
        <asp:TextBox ID="PathTextBox" runat="server" Width="700px"></asp:TextBox>
        <asp:Button ID="FindPathButton" runat="server" onclick="FindPathButton_Click" 
            Text="Find" />
        <asp:DetailsView ID="CategoryDetailsView" runat="server" 
            AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="CategoryDataSource" 
            DefaultMode="Insert" Height="50px" Width="125px">
            <Fields>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:CommandField ShowInsertButton="True" ShowCancelButton="False" />
            </Fields>
        </asp:DetailsView>
        <asp:GridView ID="CategoryGridView" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ID" DataSourceID="CategoryDataSource" AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" 
                    SortExpression="DateCreated" ReadOnly="True" />
                <asp:BoundField DataField="LastUpdated" HeaderText="LastUpdated" 
                    SortExpression="LastUpdated" ReadOnly="True" />
                <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="CategoryDataSource" runat="server" DeleteMethod="DeleteCategory" 
            InsertMethod="InsertCategory" 
            SelectMethod="GetChildCategories" TypeName="CategoriesSource" 
            UpdateMethod="UpdateCategory" ondeleted="CategoryDataSource_Deleted" 
            oninserted="CategoryDataSource_Inserted" 
            onupdated="CategoryDataSource_Updated" SelectCountMethod="TotalChildCount" 
            EnablePaging="True" oninserting="CategoryDataSource_Inserting" 
            onobjectcreating="CategoryDataSource_ObjectCreating">
            <UpdateParameters>
                <asp:Parameter DbType="Guid" Name="id" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter DbType="Guid" Name="parentId" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter DbType="Guid" Name="parentId" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
