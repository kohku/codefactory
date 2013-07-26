<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="authentication_ChangePassword" StyleSheetTheme="Default" Theme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        	width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="width: 100%; margins: auto;">
        <div id="body">
            <div id="header">
                <asp:Image ID="Image1" runat="server" ImageAlign="Middle" 
                    ImageUrl="~/images/logo_investiga_wiki.jpg" />
                <asp:Image ID="Image2" runat="server" ImageAlign="Middle" 
                    ImageUrl="~/images/logo_comex.jpg" />
            </div>
            <div id="content">
                <br />
                <asp:ChangePassword ID="ChangePassword1" runat="server" 
                    CancelDestinationPageUrl="~/Default.aspx" 
                    ContinueDestinationPageUrl="~/Default.aspx">
                    <ChangePasswordTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" 
                            style="border-collapse:collapse;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                Change Your Password</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="CurrentPasswordLabel" runat="server" 
                                                    AssociatedControlID="CurrentPassword">Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" 
                                                    SkinID="TextBox_Standard"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                                                    ControlToValidate="CurrentPassword" ErrorMessage="Password is required." 
                                                    ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="NewPasswordLabel" runat="server" 
                                                    AssociatedControlID="NewPassword">New Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" 
                                                    SkinID="TextBox_Standard"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                                    ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                                    ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                                    AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" 
                                                    SkinID="TextBox_Standard"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                                    ControlToValidate="ConfirmNewPassword" 
                                                    ErrorMessage="Confirm New Password is required." 
                                                    ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                                    ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                                    Display="Dynamic" 
                                                    ErrorMessage="The Confirm New Password must match the New Password entry." 
                                                    ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color:Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="ChangePasswordPushButton" runat="server" 
                                                    CommandName="ChangePassword" CssClass="buttons_blue" Text="Change Password" 
                                                    ValidationGroup="ChangePassword1" />
                                            </td>
                                            <td>
                                                <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" 
                                                    CommandName="Cancel" CssClass="buttons_blue" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ChangePasswordTemplate>
                    <SuccessTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" 
                            style="border-collapse:collapse;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                Change Password Complete</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Your password has been changed!</td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button ID="ContinuePushButton" runat="server" CausesValidation="False" 
                                                    CommandName="Continue" CssClass="buttons_blue" Text="Continue" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </SuccessTemplate>
                </asp:ChangePassword>
                <br />
            <asp:Label ID="footer" runat="server" Text="Copyright © Derechos Reservados Consorcio CodeFactory, S.A. de C.V" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
