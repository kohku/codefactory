<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Settings" %>
<%@ Register Src="settings/PageSettings.ascx" TagName="PageSettings" TagPrefix="uc1" %>
<%@ Register Src="settings/DisplayModeSettings.ascx" TagName="DisplayModeSettings"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:WebPartManager ID="TheWebPartManager" runat="server">
    </asp:WebPartManager>
    <div>
        <asp:MultiView ID="TheMultiView" runat="server">
            <asp:View ID="LayoutView" runat="server">
                <div style="width: 1235px;">
                    <asp:Panel ID="SettingsPanel" runat="server">
                        <div style="display: block; float: left; width: 275px;">
                            <uc2:DisplayModeSettings ID="TheDisplayMode" runat="server" />
                            <uc1:PageSettings ID="ThePageSettings" runat="server" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="LayoutPanel" runat="server" Width="957px">
                        <asp:WebPartZone ID="WebPartZone1" runat="server">
                        </asp:WebPartZone>
                    </asp:Panel>
                </div>
            </asp:View>
            <asp:View ID="EditorView" runat="server">
                <asp:EditorZone ID="TheEditorZone" runat="server" Width="957px" BackColor="#F7F6F3" BorderColor="#CCCCCC" BorderWidth="1px" Font-Names="Verdana" Padding="6" HeaderText="Editor">
                    <HeaderVerbStyle Font-Bold="False" Font-Size="7pt" Font-Underline="False" ForeColor="DimGray" />
                    <PartTitleStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="7pt" ForeColor="White" />
                    <FooterStyle BackColor="#E2DED6" HorizontalAlign="Right" />
                    <PartChromeStyle BorderColor="#E2DED6" BorderStyle="Solid" BorderWidth="1px" />
                    <InstructionTextStyle Font-Size="7pt" ForeColor="DimGray" />
                    <LabelStyle Font-Size="7pt" ForeColor="DimGray" />
                    <VerbStyle Font-Names="Arial" Font-Size="7pt" ForeColor="DimGray" />
                    <HeaderStyle BackColor="#E2DED6" Font-Bold="True" Font-Size="7pt" ForeColor="DimGray" />
                    <EditUIStyle Font-Names="Arial" Font-Size="7pt" ForeColor="DimGray" />
                    <PartStyle BorderColor="#F7F6F3" BorderWidth="5px" />
                    <EmptyZoneTextStyle Font-Size="7pt" ForeColor="DimGray" />
                </asp:EditorZone>
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
