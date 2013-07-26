<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageSections.aspx.cs" Inherits="admin_manageSections" %>

<%@ Register assembly="CodeFactory.ContentManager" namespace="CodeFactory.ContentManager.WebControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TreeView ID="SectionTreeView" runat="server" 
            DataSourceID="SectionTreeSource" ShowLines="True" ExpandDepth="0"
            onselectednodechanged="SectionTreeView_SelectedNodeChanged" >
        </asp:TreeView>
        <cc1:SectionDataSource ID="SectionTreeSource" runat="server">
        </cc1:SectionDataSource>
        <asp:TextBox ID="PathTextBox" runat="server" Width="700px"></asp:TextBox>
        <asp:Button ID="FindPathButton" runat="server" onclick="FindPathButton_Click" 
            Text="Find" />
    
        <asp:DetailsView ID="SectionDetailsView" runat="server" 
            AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="SectionDataSource" 
            DefaultMode="Insert" Height="50px" Width="125px">
            <Fields>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Slug" HeaderText="Slug" SortExpression="Slug" />
                <asp:BoundField DataField="Index" HeaderText="Index" SortExpression="Index" />
                <asp:CheckBoxField DataField="IsVisible" HeaderText="IsVisible" 
                    SortExpression="IsVisible" />
                <asp:CommandField ShowInsertButton="True" ShowCancelButton="False" />
            </Fields>
        </asp:DetailsView>
        <asp:GridView ID="SectionGridView" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="ID" DataSourceID="SectionDataSource" AllowPaging="True" 
            onrowcommand="SectionGridView_RowCommand">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Slug" HeaderText="Slug" SortExpression="Slug" />
                <asp:BoundField DataField="Index" HeaderText="Index" SortExpression="Index" />
                <asp:ButtonField Text="Up" CommandName="Up" />
                <asp:CheckBoxField DataField="IsVisible" HeaderText="IsVisible" 
                    SortExpression="IsVisible" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" 
                    SortExpression="DateCreated" ReadOnly="True" />
                <asp:BoundField DataField="LastUpdated" HeaderText="LastUpdated" 
                    SortExpression="LastUpdated" ReadOnly="True" />
                <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="SectionDataSource" runat="server" DeleteMethod="DeleteSection" 
            InsertMethod="InsertSection" 
            SelectMethod="GetChildSections" TypeName="SectionsSource" 
            UpdateMethod="UpdateSection" ondeleted="SectionDataSource_Deleted" 
            oninserted="SectionDataSource_Inserted" 
            onupdated="SectionDataSource_Updated" SelectCountMethod="TotalChildCount" 
            EnablePaging="True" oninserting="SectionDataSource_Inserting"
            onobjectcreating="SectionDataSource_ObjectCreating" 
            OldValuesParameterFormatString="{0}">
            <UpdateParameters>
                <asp:Parameter DbType="Guid" Name="id" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="slug" Type="String" />
                <asp:Parameter Name="index" Type="Int32" />
                <asp:Parameter DbType="Guid" Name="parentId" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="slug" Type="String" />
                <asp:Parameter Name="index" Type="Int32" />
                <asp:Parameter DbType="Guid" Name="parentId" />
            </InsertParameters>
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
