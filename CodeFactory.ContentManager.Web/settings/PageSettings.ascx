<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageSettings.ascx.cs" Inherits="settings_PageSettings" %>
<div style="width: 270px; background-color: #b1c3d9; border-right: background 1px solid; border-top: background 1px solid; font-size: 7pt; border-left: background 1px solid; border-bottom: background 1px solid; font-family: Arial;">
    <div style="height: 16px; display: block; float: left;">
    <asp:Image ID="ExpandCollapseImage" runat="server" />
    <asp:Label ID="PageTitleLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="7pt" Font-Underline="True" Height="15px" Width="200px">Page</asp:Label>
    </div>
    <asp:Panel ID="PageSettingsPanel" runat="server" Height="100%" Width="100%" ToolTip="Page Settings">
        <asp:Panel ID="GeneralTabPanel" runat="server">
            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 75px; text-align: right">
                        <asp:Label ID="LayoutTemplateLabel" runat="server" meta:resourceKey="LayoutTemplateLabel"
                            SkinID="Label_Small" Text="Layout:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="LayoutTemplatesList" runat="server" AutoPostBack="True" SkinID="DropDownList_Small" Width="175px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                        <asp:Label ID="TitleLabel" runat="server" SkinID="Label_Small" Text="Title:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TitleTextBox" runat="server" SkinID="TextBox_Small" Width="170px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                        <asp:Label ID="ContentLabel" runat="server" SkinID="Label_Small" Text="Content:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ContentTextBox" runat="server" SkinID="TextBox_Small" Rows="2" TextMode="MultiLine" Width="170px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                        <asp:Label ID="DescriptionLabel" runat="server" SkinID="Label_Small" Text="Description:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="DescriptionTextBox" runat="server" SkinID="TextBox_Small" Rows="2" TextMode="MultiLine" Width="170px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                        <asp:Label ID="KeywordsLabel" runat="server" SkinID="Label_Small" Text="Keywords:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="KeywordsTextBox" runat="server" SkinID="TextBox_Small" Width="170px" Rows="2" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                        <asp:Label ID="IsPublishedLabel" runat="server" SkinID="Label_Small" Text="Is Published:"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="IsPublishedCheckBox" runat="server" SkinID="CheckBox_Small" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 75px; text-align: right">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="AdvancedTabPanel" runat="server">
                Advanced<br />
                <br />
                Here is the content for the superior tab container.
                <br />
                <br />
                The text within this container is just used to complete a series of test that shows
                the utility for the tab.
        </asp:Panel>
    </asp:Panel>
</div>
