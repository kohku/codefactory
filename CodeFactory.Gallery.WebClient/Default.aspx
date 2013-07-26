<%@ Page Language="C#" MasterPageFile="~/themes/CodeFactoryDefault/CodeFactoryDefault.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Src="Gallery.ascx" TagName="Gallery" TagPrefix="uc1" %>

<asp:Content ID="Main" ContentPlaceHolderID="Main" Runat="Server">
<table id="table_main" style="width: 967px; vertical-align: top;" cellpadding="0" cellspacing="10">
    <tbody>
        <tr>
            <td style="text-align: left;" valign="top">
                <div id="project_title" style="width: 626px">
                    <div style="width: 294px; margin-left: 20px;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/proyecto.jpg" /></div>
                </div>
                <div id="gallery" style="width: 626px">
                    <uc1:Gallery ID="Gallery1" runat="server" />
                </div>
                <div id="history" style="width: 626px">
                </div>
            </td>
            <td valign="top">
                <div id="comment_bar" style="width: 297px; height: 450px">
                    <div id="comments_title" style="width: 296px; height: 39px;"></div>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 296px">
                        <tr>
                            <td style="width: 81px">
                            </td>
                            <td style="text-align: center">
                                <asp:Button ID="NewButton" runat="server" OnClick="NewButton_Click" Text="<%$ Resources:NewProject.Text %>"
                                    Width="120px" /></td>
                            <td style="width: 81px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 81px">
                            </td>
                            <td style="text-align: center">
                                <asp:Button ID="UsersRolesButton" runat="server" OnClick="UsersRolesButton_Click"
                                    Text="<%$ Resources:UsersRoles.Text %>" Width="120px" /></td>
                            <td style="width: 81px">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </tbody>
</table>
</asp:Content>
