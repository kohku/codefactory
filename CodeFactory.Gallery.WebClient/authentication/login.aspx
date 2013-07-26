<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="authentication_login" UICulture="Auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal runat="server" Text="<%$ Resources:TitleText %>" /></title>
    <link type="text/css" rel="Stylesheet" href="login.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="header">
                <asp:Image ID="LogoImage" runat="server" ImageUrl="images/logoCodeFactory.jpg" />
            </div>
            <div class="login_layout_row" style="height: 35px;">
                <div class="login_layout_row_title" style="width: 350px;">
                </div>
                <div class="login_layout_row_field">&nbsp;
                </div>
            </div>
            <div class="login_layout_row">
                <div class="login_layout_row_title" style="width: 350px; height: 35px;">&nbsp;
                </div>
                <div class="login_layout_row_field" style="height: 35px;">
                    <asp:Label ID="LoginLabel" runat="server" CssClass="whitetext" Text="<%$ Resources:LoginTitleText %>"></asp:Label>
                </div>
            </div>
            <asp:Login ID="LoginControl"  runat="server" BorderStyle="None" BorderWidth="0px" FailureText="<%$ Resources:FailureText %>">
                <LayoutTemplate>
                    <div class="login_layout_row">
                        <div class="login_layout_row_title" style="width: 310px">
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="whitetext" Text="<%$ Resources:UserNameText %>"></asp:Label>
                        </div>
                        <div class="login_layout_row_field">
                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                ErrorMessage="<%$ Resources:UserNameErrorMessage %>" ToolTip="User Name is required.">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="login_layout_row">
                        <div class="login_layout_row_title" style="width: 310px">
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="whitetext" Text="<%$ Resources:PasswordText %>"></asp:Label>
                        </div>
                        <div class="login_layout_row_field">
                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                ErrorMessage="<%$ Resources:PasswordErrorMessage %>" ToolTip="Password is required.">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="login_layout_row">
                        <div class="login_layout_row_title" style="width: 310px">&nbsp;
                        </div>
                        <div class="login_layout_row_field">
                            <asp:CheckBox ID="RememberMe" runat="server" Text="<%$ Resources:RememberMeText %>" CssClass="whitetext" /><
                        </div>
                    </div>
                    <div class="login_layout_row">
                        <div class="login_layout_row_title" style="width: 310px">&nbsp;
                        </div>
                        <div class="login_layout_row_field">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False" Text="<%$ Resources:FailureText %>"></asp:Literal>
                        </div>
                    </div>
                    <div class="login_layout_row" style="height: 18px;">
                        <div class="login_layout_row_title" style="width: 350px;">
                        </div>
                        <div class="login_layout_row_field">&nbsp;
                        </div>
                    </div>
                    <div class="login_layout_row">
                        <div class="login_layout_row_title" style="width: 320px">&nbsp;
                        </div>
                        <div class="login_layout_row_field">
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="<%$ Resources:LoginButtonText %>" Width="96px" />
                        </div>
                    </div>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </form>
</body>
</html>
