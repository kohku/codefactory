<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wikiHistory.aspx.cs" Inherits="admin_wikiHistory"
    MasterPageFile="~/WikiMaster.master" Theme="Default" StylesheetTheme="Default"
    Title="Wiki - Historia" %>
<asp:Content ContentPlaceHolderID="head" ID="header" runat="server">
    <style type="text/css">
    #wiki
    {
    	display: block; float: none; padding-top: 10px; padding-bottom: 10px;
    }
    #wiki #controls
    {
    	display:block; float: none; width: 731px; padding-left: 18px;
    }
    #wiki #content
    {
    	display:block; float: none; text-align: left;
    }
    #wiki #content #top
    {
    	display: block; float: none; width: 770px;
    }
    #wiki #content #main
    {
        display: block; float: none; width: 770px; 
        background-image: url(<%= CodeFactory.Web.Utils.RelativeWebRoot + "images/fondo_tabla.jpg"%>);
        background-repeat: repeat-y; padding-left: 18px;    
    }
    #wiki #content #bottom
    {
    	display: block; float: none; width: 770px;
    }
    #wiki #content #main #container
    {
    	width: 731px; text-align: left;
    }
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainPlaceHolder" ID="MainHolder" runat="server">
    <div id="wiki">
    <div id="controls">
        <asp:Button ID="BackButton" runat="server" CssClass="buttons_blue" 
            Text="Regresar" onclientclick="window.history.back();return false;" /></div>
    <div id="content">
        <div id="top">
            <asp:Image ID="ContentTopImage" runat="server" ImageUrl="~/images/fondo_tabla_top.jpg" /></div>
        <div id="main">
            <table id="container" cellpadding="0" cellspacing="0">
                <tr>
                    <td>                        
                        <p>
                            <asp:Label ID="TitleLabel" runat="server" CssClass="titles_blue" Text="Lorelm ipsum dolor"></asp:Label>
                            <asp:Image ID="TitleImage" runat="server" ImageUrl="~/images/linea.jpg" 
                                Width="533px" />
                            <asp:Label ID="EditorLabel" runat="server" Font-Italic="True">Author: {0}</asp:Label></p>
                        <p>
                            <asp:Label ID="ContentLabel" runat="server"></asp:Label></p></td>
                </tr>
            </table></div>
        <div id="bottom">
            <asp:Image ID="ContentBottomImage" runat="server" ImageUrl="~/images/fondo_tabla_bott.jpg" /></div>
    </div>
</div>
</asp:Content>
