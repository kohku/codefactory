<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manageSingleRol.aspx.cs" Inherits="admin_manageSingleRol" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Default Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="RoleNameLabel" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:Label ID="SearchLabel" runat="server" Text="Búsqueda"></asp:Label><asp:TextBox ID="SearchTextBox"
        runat="server"></asp:TextBox>
    <asp:DropDownList ID="SearchSelection" runat="server">
        <asp:ListItem Selected="True" Value="username">Usuario</asp:ListItem>
        <asp:ListItem Value="email">Email</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="SearchButton" runat="server" Text="Buscar" 
        onclick="SearchButton_Click" />
    <asp:GridView ID="MembershipGridView" runat="server" Width="770px" AllowPaging="True" 
        AutoGenerateColumns="False" DataSourceID="MembershipDataSource" 
        onrowdatabound="MembershipGridView_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="UserName">
                <ItemTemplate>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
            <asp:TemplateField HeaderText="Is user in role">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox ID="IsInRoleCheckBox" runat="server" AutoPostBack="True" 
                        oncheckedchanged="IsInRoleCheckBox_CheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="MembershipDataSource" runat="server" 
        EnablePaging="True" OldValuesParameterFormatString="original_{0}" 
        SelectCountMethod="TotalCount" SelectMethod="GetMembers" 
        TypeName="MembershipResult" 
        onobjectcreating="MembershipDataSource_ObjectCreating">
        <SelectParameters>
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
        <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
            Text="Back" />
    </div>
    </form>
</body>
</html>
