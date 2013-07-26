<%@ Page Title="Preferencias" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="Preferences.aspx.cs" Inherits="Preferences" StyleSheetTheme="Default" %>

<asp:Content ID="TheHeadContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="TheMainContent" ContentPlaceHolderID="MainPlaceHolder" Runat="Server">
    <br />
    <br />
    <asp:Label ID="SecurityLabel" runat="server" Text="Seguridad" 
        CssClass="subtitles_yellow"></asp:Label><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Label ID="UsersLabel" runat="server" Text="Usuario" 
        CssClass="content_bold"></asp:Label><br />
    &nbsp;&nbsp;&nbsp;
    <asp:Image ID="ChangePasswordBulletImage" runat="server" ImageUrl="~/images/bullet.gif" /><asp:LinkButton ID="ChangePasswordLinkButton" runat="server" 
        PostBackUrl="~/authentication/ChangePassword.aspx" CssClass="content">Cambiar Contraseña</asp:LinkButton><br />
    &nbsp;&nbsp;&nbsp;
</asp:Content>

