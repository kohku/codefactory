<%@ Page Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="Wiki - Portada" StylesheetTheme="Default"
    Theme="Default" EnableSessionState="False" EnableViewState="False" %>
    
<%@ OutputCache Duration="60" VaryByParam="*" VaryByCustom="userId" %>

<%@ Register src="widgets/TagCloud.ascx" tagname="TagCloud" tagprefix="uc1" %>

<asp:Content ContentPlaceHolderID="head" ID="header" runat="server">
    <style type="text/css">
        #tools
        {
            display: block; float: none; padding-top: 20px; padding-bottom: 10px;
        }
        #tools #content
        {
            background-image: url(<%= CodeFactory.Web.Utils.RelativeWebRoot +  "images/fondo_herramientas.jpg" %>);
            background-repeat: repeat-x;
        }
        #keywordOfDay
        {
        	display: block; float: none; width: 750px; height: 24px; background-color: #f8f8f8; padding-top: 10px; padding-left: 18px;
        	border-top: solid 1px #e4e4e4; border-left: solid 1px #e4e4e4; border-right: solid 1px #e4e4e4; vertical-align: middle;
        }
        #cover
        {
    	    display:block; float: none; text-align: left; padding-bottom: 10px;
        }
        #cover #top
        {
    	    display: block; float: none; width: 770px;
        }
        #cover #main
        {
            display: block; float: none; width: 770px; 
            background-image: url(<%= CodeFactory.Web.Utils.RelativeWebRoot + "images/fondo_tabla.jpg"%>);
            background-repeat: repeat-y; padding-top: 10px; padding-left: 18px;    
        }
        #cover #bottom
        {
    	    display: block; float: none; width: 770px;
        }
        #cover #main #container
        {
    	    width: 731px; text-align: left;
        }
        #cover #main #container #left
        {
    	    width: 355px; vertical-align: top;
        }
        #cover #main #container #middle
        {
    	    width: 31px;
        }
        #cover #main #container #right
        {
    	    width: 350px; vertical-align: top;
        }
        .style1
        {
            height: 28px;
        }
        .style2
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 10px;
            color: #999999;
            line-height: 16px;
            text-align: justify;
            word-spacing: normal;
            height: 28px;
        }

        .style5
        {
            height: 28px;
        }
        .style6
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-style: normal;
            font-weight: bold;
            color: #FAA634;
            font-size: 16px;
            line-height: 16px;
            text-transform: none;
            height: 28px;
        }

        .style7
        {
            height: 28px;
            width: 9px;
        }
        .style8
        {
            width: 9px;
        }

        .style9
        {
            height: 28px;
            width: 12px;
        }
        .style10
        {
            width: 12px;
        }

    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <div id="tools">
        <div id="content">
            <table width="731" border="0" align="center" cellpadding="2" cellspacing="0">
                <tr>
                    <td width="131" rowspan="5">
                        <div align="center">
                            <asp:Image ID="ContributeImage" ImageUrl="~/images/participa.jpg" runat="server" /></div>
                    </td>
                    <td class="style9">
                        </td>
                    <td width="193" class="style6">
                        &nbsp;</td>
                    <td width="125" rowspan="5">
                        <div align="center">
                            <asp:Image ID="QueryImage" ImageUrl="~/images/lupa.gif" runat="server" /></div>
                    </td>
                    <td class="style7">
                        </td>
                    <td width="196" class="style5">
                        <asp:Label ID="CurrentDateLabel" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="style10">
                        &nbsp;
                    </td>
                    <td width="193" class="titles_yellow">
                        <asp:Label ID="ContributeLabel" runat="server" Text="Participa"></asp:Label>
                    </td>
                    <td class="style8">
                        &nbsp;
                    </td>
                    <td width="196" class="titles_yellow">
                        <asp:Label ID="QueryLabel" runat="server" Text="Consulta"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style10">
                        <asp:Image ID="BulletImage0" ImageUrl="~/images/bullet.gif" runat="server" />
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink1" CssClass="content" runat="server" 
                            NavigateUrl="~/wiki/como-colaborar.aspx">¿Cómo colaborar?</asp:HyperLink><br />
                    </td>
                    <td class="style8">
                        <asp:Image ID="BulletImage1" ImageUrl="~/images/bullet.gif" runat="server" />
                    </td>
                    <td>
                        <asp:HyperLink ID="CategoriesLink" CssClass="content" runat="server" 
                            NavigateUrl="~/CategoryIndex.aspx">Índice de categorías</asp:HyperLink><br />
                    </td>
                </tr>
                <tr>
                    <td class="style10">
                        <asp:Image ID="BulletImage2" ImageUrl="~/images/bullet.gif" runat="server" />
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink3" CssClass="content" runat="server" 
                            NavigateUrl="~/wiki/primeros-pasos.aspx">Primeros pasos</asp:HyperLink>
                    </td>
                    <td class="style8">
                        <asp:Image ID="WorklistBullet" ImageUrl="~/images/bullet.gif" runat="server" />
                    </td>
                    <td>
                        <asp:HyperLink ID="WorklistLink" CssClass="content" runat="server" 
                            NavigateUrl="~/worklist.aspx">Lista de Tareas</asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="style10">
                        <asp:Image ID="BulletImage4" ImageUrl="~/images/bullet.gif" runat="server" />
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink6" CssClass="content" runat="server" 
                            NavigateUrl="~/wiki/tutorial.aspx">Tutorial</asp:HyperLink>
                    </td>
                    <td class="style8">
                        <asp:Image ID="AdministrationBullet" ImageUrl="~/images/bullet.gif" 
                            runat="server" />
                    </td>
                    <td>
                        <asp:HyperLink ID="AdministrationLink" CssClass="content" runat="server" 
                            NavigateUrl="~/admin/default.aspx">Administración</asp:HyperLink></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="keywordOfDay">
        <asp:Label ID="KeywordOfDayLabel" runat="server" CssClass="titles_green" Text="Tema del día:&nbsp;"></asp:Label>
        <asp:Label ID="ContentLabel" runat="server" CssClass="content_bold" 
            Font-Bold="False" Font-Size="16px"></asp:Label>
    </div>
    <div id="cover">
    <!--
        <div id="top">
            <asp:Image ID="ContentTopImage" runat="server" ImageUrl="~/images/fondo_tabla_top.jpg" /></div>-->
        <div id="main">
            <table id="container" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="left" valign="top">                        
                        <p>
                            <asp:HyperLink ID="TitleLabel1" runat="server" CssClass="titles_blue" 
                                Text="Lorelm ipsum dolor"></asp:HyperLink></p>
                        <p>
                            <asp:Label ID="ContentLabel1" runat="server" Text="Content"></asp:Label>
                        </p></td>
                <td id="middle">&nbsp;</td>
                <td id="right" valign="top">
                    <uc1:TagCloud ID="TheTagCloud" runat="server" /></td>
                </tr>
            </table></div>
        <div id="bottom">
            <asp:Image ID="ContentBottomImage" runat="server" ImageUrl="~/images/fondo_tabla_bott.jpg" /></div>
    </div>
</asp:Content>
