<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordRecovery.aspx.cs"
    Inherits="authentication_PasswordRecovery" StylesheetTheme="Default" Theme="Default" %>

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
            width: 300px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="width: 100%; margin: 0 auto;">
        <div id="body" style="width: 100%; margin: 0 auto;">
            <div id="header">
                <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/images/logo_investiga_wiki.jpg" />
                <asp:Image ID="Image2" runat="server" ImageAlign="Middle" ImageUrl="~/images/logo_comex.jpg" />
            </div>
            <div id="content">
                <br />
                <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" SkinID="TextBox_Standard">
                    <MailDefinition BodyFileName="~/authentication/PasswordRecoveryTemplate.htm" IsBodyHtml="True"
                        Priority="High" Subject="Tu contraseña">
                    </MailDefinition>
                    <QuestionTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                Identity Confirmation
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                Answer the following question to receive your password.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                User Name:
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="UserName" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Question:
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="Question" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Answer:</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="Answer" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer"
                                                    ErrorMessage="Answer is required." ToolTip="Answer is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" CssClass="buttons_blue"
                                                    Text="Submit" ValidationGroup="PasswordRecovery1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </QuestionTemplate>
                    <UserNameTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td align="center" colspan="2">
                                                Forgot Your Password?
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                Enter your User Name to receive your password.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UserName" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" CssClass="buttons_blue"
                                                    Text="Submit" ValidationGroup="PasswordRecovery1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </UserNameTemplate>
                    <SuccessTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                Your password has been sent to you.<br />
                                                <br />
                                                <asp:Button ID="ContinueButton" runat="server" CssClass="buttons_blue" PostBackUrl="~/authentication/login.aspx"
                                                    Text="Continuar" />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </SuccessTemplate>
                </asp:PasswordRecovery>
                <br />
                <asp:Label ID="footer" runat="server" Text="Copyright © Derechos Reservados Consorcio CodeFactory, S.A. de C.V" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
