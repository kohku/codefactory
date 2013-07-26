<%@ Page Title="Wiki - Administración del rol" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="manageSingleRol.aspx.cs" Inherits="admin_manageSingleRol" StyleSheetTheme="Default" Theme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <br />
    <asp:Label ID="RoleNameLabel" runat="server" Text="Role Name" 
        CssClass="subtitles_yellow"></asp:Label>
    <br />
    <br />
    <asp:Label ID="SearchLabel" runat="server" Text="Búsqueda"></asp:Label>
    &nbsp;<asp:TextBox ID="SearchTextBox"
        runat="server" SkinID="TextBox_Standard"></asp:TextBox>
    &nbsp;<asp:DropDownList ID="SearchSelection" runat="server" 
        SkinID="DropDownList_Standard">
        <asp:ListItem Selected="True" Value="username">Usuario</asp:ListItem>
        <asp:ListItem Value="email">Email</asp:ListItem>
    </asp:DropDownList>
    &nbsp;<asp:Button ID="SearchButton" runat="server" Text="Buscar" 
        onclick="SearchButton_Click" CssClass="buttons_blue" />
    <br />
    <br />
    <asp:GridView ID="MembershipGridView" runat="server" Width="770px" AllowPaging="True" 
        AutoGenerateColumns="False" DataSourceID="MembershipDataSource" 
        onrowdatabound="MembershipGridView_RowDataBound">
        <RowStyle CssClass="DHTR_Grid_Row" />
        <Columns>
            <asp:TemplateField HeaderText="Usuario">
                <ItemTemplate>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
            <asp:TemplateField HeaderText="Pertenece al Rol">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox ID="IsInRoleCheckBox" runat="server" AutoPostBack="True" 
                        oncheckedchanged="IsInRoleCheckBox_CheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen usuarios registrados en este rol.
        </EmptyDataTemplate>
        <HeaderStyle BackColor="#F8F8F8" CssClass="content_bold" />
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
    <br />
    <asp:Button ID="BackButton" runat="server" onclick="BackButton_Click" 
        Text="Regresar" CssClass="buttons_blue" />
    <br />
    <br />
    <br />
</asp:Content>

