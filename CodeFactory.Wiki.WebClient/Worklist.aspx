<%@ Page Title="" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="Worklist.aspx.cs" Inherits="Worklist" StyleSheetTheme="Default" Theme="Default" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <div style="display: block; float: left; width: 770px; background-color: #efefef">
        <asp:Label runat="server" ID="AuthorizationTitles">Autorizaciones Pendientes</asp:Label>
    </div>
    <asp:GridView ID="TheGridView" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="ID" DataSourceID="TheDataSource" Width="770px" 
        onrowdatabound="TheGridView_RowDataBound" AllowPaging="True" 
        BorderStyle="Solid">
    <Columns>
        <asp:TemplateField HeaderText="Contenido">
            <ItemTemplate>
                <asp:HyperLink ID="WikiLink" runat="server">[WikiLink]</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Autor">
            <ItemTemplate>
                <asp:Label ID="AuthorLabel" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Fecha Creación" SortExpression="DateCreated">
            <ItemTemplate>
                <asp:Label ID="CreationDateLabel" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="170px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Expira">
            <ItemTemplate>
                <asp:Label ID="ExpirationDateLabel" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="170px" />
        </asp:TemplateField>
    </Columns>
        <EmptyDataTemplate>
            <asp:Label ID="EmptyLabel" runat="server" Text="No hay actividades pendientes"></asp:Label>
        </EmptyDataTemplate>
        <HeaderStyle BackColor="#33CCCC" ForeColor="SteelBlue" />
        <AlternatingRowStyle BackColor="#C9F1F1" />
</asp:GridView>
<asp:ObjectDataSource ID="TheDataSource" runat="server" 
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetWorkList" 
    TypeName="WorkList" EnablePaging="True" SelectCountMethod="TotalCount"></asp:ObjectDataSource>
<br />
</asp:Content>
