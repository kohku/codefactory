﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true" CodeFile="AuthorizeWiki.aspx.cs" Inherits="AuthorizeWiki" StyleSheetTheme="Default" Theme="Default" %>
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
    
    .MessagesBoard
    {
    	border: solid 1px #3399FF;
    	display:block; 
    	float: left; 
    	text-align: center; 
    	width: 533px; 
    	background-color: #FFFFCC; 
    	color: #0000FF; 
    	padding: 5px;
    	margin-bottom: 10px;
    }
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainPlaceHolder" ID="MainHolder" runat="server">
    <div id="wiki">
    <div id="controls">
        <div class="buttons_blue" style="display: block; float: left; height: 18px; width: 90px; text-align: center;">Contenido</div>
        <div style="display: block; float: left; width: 4px;">&nbsp;</div>
        <div id="EditButton" runat="server" class="buttons_blue" style="display: block; float: left; height: 18px; width: 60px; text-align: center; visibility: hidden;">Editar</div>
    </div>
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
                            <asp:Label ID="ContentLabel" runat="server"></asp:Label>
                        </p>
                        <div id="MessagesBoard" runat="server" class="MessagesBoard">
                            <asp:Label ID="MessagesBoardLabel" runat="server"></asp:Label>
                        </div>
                        <div style="display:block; float: left; text-align: center; width: 533px;">
                            <asp:Button ID="AuthorizeButton" runat="server" SkinID="Button_Standard" 
                                Text="Autorizar" Width="86px" onclick="AuthorizeButton_Click" />&nbsp;
                            <asp:Button 
                                    ID="RejectButton" runat="server" SkinID="Button_Standard" 
                                Text="Rechazar" Width="86px" onclick="RejectButton_Click" 
                                ValidationGroup="Reject" />
                        </div>
                        <div style="display:block; float: left; text-align: left; width: 533px">
                            <asp:Label ID="CommentsLabel" runat="server">Comentarios:</asp:Label>
                            <asp:TextBox ID="CommentsTextBox" runat="server" TextMode="MultiLine" 
                                Width="520px" ValidationGroup="Reject" SkinID="TextBox_Standard"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RejectValidator" runat="server" 
                                ControlToValidate="CommentsTextBox" Display="Dynamic" ErrorMessage="*" 
                                ValidationGroup="Reject">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                <td id="middle">&nbsp;</td>
                <td id="right">&nbsp;</td>
                </tr>
            </table></div>
        <div id="bottom">
            <asp:Image ID="ContentBottomImage" runat="server" ImageUrl="~/images/fondo_tabla_bott.jpg" /></div>
    </div>
</div>
</asp:Content>
