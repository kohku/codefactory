<%@ Page Title="Wiki - Agregar usuario" Language="C#" MasterPageFile="~/WikiMaster.master" AutoEventWireup="true"
    CodeFile="addUser.aspx.cs" Inherits="admin_addUser" StylesheetTheme="Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style5
        {
            width: 150px;
        }
        .style6
        {
        	text-align: left;
            width: 299px;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainPlaceHolder" runat="Server">
    <br />
    <table width="100%" style="text-align: right">
        <tr>
            <td class="style5">
            </td>
            <td class="style6">
                <asp:CreateUserWizard ID="TheCreateUserWizard" runat="server" 
                    OnCreatedUser="TheCreateUserWizard_CreatedUser" CancelButtonText="Cancelar" 
                    CreateUserButtonText="Crear Usuario">
                    <CreateUserButtonStyle CssClass="buttons_blue" />
                    <NavigationStyle HorizontalAlign="Left" />
                    <WizardSteps>
                        <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                            <ContentTemplate>
                                <table border="0">
                                    <tr>
                                        <td align="center" colspan="2" class="subtitles_yellow">
                                            Registra el nuevo usuario</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuario:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="TheCreateUserWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Contraseña:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" SkinID="TextBox_Standard"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="TheCreateUserWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirma Contraseña:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" SkinID="TextBox_Standard"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                                ValidationGroup="TheCreateUserWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Email" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                                ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="TheCreateUserWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Pregunta Secreta:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Question" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" ControlToValidate="Question"
                                                ErrorMessage="Security question is required." ToolTip="Security question is required."
                                                ValidationGroup="TheCreateUserWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Respuesta Secreta:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Answer" runat="server" SkinID="TextBox_Standard"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer"
                                                ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                                                ValidationGroup="TheCreateUserWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="La contraseña y su confirmación deben de coincidir."
                                                ValidationGroup="TheCreateUserWizard"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="color: Red;">
                                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CreateUserWizardStep>
                        <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                            <ContentTemplate>
                                <table border="0">
                                    <tr>
                                        <td align="center" colspan="2">
                                            Completado
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tu cuenta ha sido creada exitósamente.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
                                                Text="Continuar" ValidationGroup="TheCreateUserWizard" 
                                                CssClass="buttons_blue" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                </asp:CreateUserWizard>
            </td>
            <td valign="bottom">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td class="style6">
    &nbsp;&nbsp;
    <asp:Button ID="BackButton" runat="server" OnClick="BackButton_Click" Text="Regresar"
        CssClass="buttons_blue" />&nbsp;&nbsp;&nbsp;
            </td>
            <td valign="bottom">
                &nbsp;</td>
        </tr>
    </table>
    <br />
    <br />
</asp:Content>
