<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Wiki.aspx.cs" Inherits="_Wiki"
    MasterPageFile="~/WikiMaster.master" Theme="Default" StylesheetTheme="Default"
    Title="Wiki - Contenido" EnableSessionState="False" EnableViewState="false" %>

<%@ OutputCache Duration="60" VaryByParam="*" VaryByCustom="userId" %>

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
    #wiki #content #main #container #left
    {
    	width: 533px; vertical-align: top;
    }
    #wiki #content #main #container #middle
    {
    	width: 18px;
    }
    #wiki #content #main #container #right
    {
    	width: 180px; vertical-align: top; background-color: #F0F0F0; padding: 5px 5px 5px 5px;
    }
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainPlaceHolder" ID="MainHolder" runat="server">
    <div id="wiki">
    <div id="controls">
        <asp:Button ID="ArticleButton" runat="server" CssClass="buttons_blue" Text="Contenido" />
        <asp:Button ID="EditButton" runat="server" CssClass="buttons_blue" 
            Text="Editar" onclick="EditButton_Click" /></div>
    <div id="content">
        <div id="top">
            <asp:Image ID="ContentTopImage" runat="server" ImageUrl="~/images/fondo_tabla_top.jpg" /></div>
        <div id="main">
            <table id="container" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="left">                        
                        <p>
                            <asp:Label ID="TitleLabel" runat="server" CssClass="titles_blue" Text="Lorelm ipsum dolor"></asp:Label>
                            <asp:Image ID="TitleImage" runat="server" ImageUrl="~/images/linea.jpg" 
                                Width="533px" />
                            <asp:Label ID="EditorLabel" runat="server" Font-Italic="True">Author: {0}</asp:Label></p>
                        <p>
                            <asp:Label ID="ContentLabel" runat="server"></asp:Label></p></td>
                <td id="middle">&nbsp;</td>
                <td id="right">
                    <p>
                        <asp:HyperLink ID="TitleRelatedLabel" runat="server" CssClass="titles_blue"></asp:HyperLink></p>
                        <asp:Label ID="ContentRelatedLabel" runat="server"></asp:Label>
                    <p>
                        <asp:HyperLink ID="RelatedWikiLink" runat="server">Ver contenido</asp:HyperLink></p>
                    </td>
                </tr>
            </table></div>
        <div id="bottom">
            <asp:Image ID="ContentBottomImage" runat="server" ImageUrl="~/images/fondo_tabla_bott.jpg" /></div>
    </div>
</div>
</asp:Content>
