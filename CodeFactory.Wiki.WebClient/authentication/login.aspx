<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="authentication_login" Theme="Default" StyleSheetTheme="Default" EnableSessionState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Wiki - Autenticación</title>
    <style type="text/css">
        #body
        {
        	width: 810px;
        }
        #header
        {
	        text-align: center;
	        width: 100%;
        }
        #content
        {
	        text-align: center;
        	width: 200px;
        }
    </style>
</head>
<body style="margin: 0px 0px 0px 0px; background-color: #e9e9e9;">
    <form id="form1" runat="server">
    <div align="center" style="width: 100%; margin: 0 auto;">
        <div align="center" style="width: 810px; margin: 0 auto; background-color: #ffffff;">
            <div id="header">
                <asp:Image ID="Image1" runat="server" ImageAlign="Middle" 
                    ImageUrl="~/images/logo_investiga_wiki.jpg" />
                <asp:Image ID="Image2" runat="server" ImageAlign="Middle" 
                    ImageUrl="~/images/logo_comex.jpg" />
            </div>
            <div id="content">
                <br />
            <asp:Login ID="WikiLogin" runat="server"  
                    onauthenticate="WikiLogin_Authenticate" onloggedin="WikiLogin_LoggedIn">
                <CheckBoxStyle CssClass="content" />
                <TextBoxStyle CssClass="content" />
                <LoginButtonStyle CssClass="buttons_blue" />
            </asp:Login>
                <br />
                <asp:HyperLink ID="RecoveryPasswordHyperLink" runat="server" 
                    NavigateUrl="~/authentication/PasswordRecovery.aspx">[Olvidé mi contraseña]</asp:HyperLink>
                <br />
                <br />
            <asp:Label ID="footer" runat="server" Text="CodeFactory 2009" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
