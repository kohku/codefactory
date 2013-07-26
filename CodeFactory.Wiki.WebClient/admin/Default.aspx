<%@ Page Title="Wiki - Menú administración" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" StyleSheetTheme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style5
        {
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <br />
    <br />
    <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
        <tr>
            <td class="style5" style="vertical-align: top">    <asp:Label ID="SecurityLabel" runat="server" Text="Seguridad" 
        CssClass="subtitles_yellow"></asp:Label>
    <br />
    &nbsp;&nbsp;&nbsp;
    <asp:Label ID="UsersLabel" runat="server" Text="Users" 
        CssClass="content_bold"></asp:Label><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="BulletImage" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="ManageUsersLinkButton" runat="server" 
        PostBackUrl="~/admin/manageUsers.aspx" CssClass="content">Administración de usuarios</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="CreateUsersLinkButton" runat="server" 
        PostBackUrl="~/admin/addUser.aspx" CssClass="content">Crear usuario</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image6" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="AuthorizersLinkButton" runat="server" 
        PostBackUrl="~/admin/manageAuthorizers.aspx" CssClass="content">Autorizadores</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Label ID="RolesLabel" runat="server" Text="Roles" 
        CssClass="content_bold"></asp:Label><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="ManageRolesLinkButton" runat="server" 
        PostBackUrl="~/admin/manageAllRoles.aspx" CssClass="content">Administración de roles</asp:LinkButton>
    <br />
    <br /></td>
            <td style="vertical-align: top">
                <asp:Label ID="WikiLabel" runat="server" Text="General" 
        CssClass="subtitles_yellow"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;
    <asp:Label ID="SettingsLabel" runat="server" Text="Configuración" 
        CssClass="content_bold"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;
    <asp:Image ID="BulletImage0" runat="server" ImageUrl="~/images/bullet.gif" />
                <asp:LinkButton ID="ManageCategoriesLinkButton" runat="server" 
        PostBackUrl="~/admin/manageCategories.aspx" CssClass="content">Administración de categorías</asp:LinkButton></td>
        </tr>
        <tr>
            <td class="style5">    <asp:Label ID="WikisLabel" runat="server" Text="Reportes" 
        CssClass="subtitles_yellow"></asp:Label><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="WikiReportLinkButton" runat="server" 
        PostBackUrl="~/admin/wikiReport.aspx" CssClass="content">Reporte de contenidos</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image4" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="ChangesHistoryLinkButton" runat="server" 
        PostBackUrl="~/admin/changesHistory.aspx" CssClass="content">Historial de modificaciones a contenidos</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image5" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="PendingsOfAuthorizationLinkButton" runat="server" 
        PostBackUrl="~/admin/pendingsOfAuthorization.aspx" CssClass="content">Autorizaciones pendientes a contenidos</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="Image7" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="EstadisticasConsultaButton" runat="server" 
        PostBackUrl="~/admin/EstadisticasConsulta.aspx" CssClass="content">Estadísticas de consultas a contenidos</asp:LinkButton>
    <br />
    <br />
    <br />
</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

