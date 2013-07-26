<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DisplayModeSettings.ascx.cs" Inherits="settings_DisplayModeSettings" %>
<%@ Register TagPrefix="ContentManager" Assembly="CodeFactory.ContentManager" Namespace="CodeFactory.ContentManager.WebControls.WebParts" %>
<div id="DisplayModeContainer" runat="server" style="width: 270px; background-color: #b1c3d9; border-right: background 1px solid; border-top: background 1px solid; font-size: 7pt; border-left: background 1px solid; border-bottom: background 1px solid; font-family: Arial;">
    <div style="height: 16px; display: block; float: left;">
    <asp:Image ID="ExpandCollapseImage" runat="server" />
    <asp:Label ID="PageTitleLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="7pt" Font-Underline="True" Height="15px" Width="200px">Display Mode</asp:Label>
    </div>
    <asp:Panel ID="PageSettingsPanel" runat="server" Height="100%" Width="100%" ToolTip="Display Mode">
        <asp:Panel ID="DesignPanel" runat="server">
            <asp:Label ID="DesignPanelLabel" runat="server" />
            You can drag and drop Web Parts into different locations.
        </asp:Panel>
        <asp:Panel ID="CatalogPanel" runat="server">
            <asp:Label ID="CatalogPanelLabel" runat="server" />
            <asp:CatalogZone ID="TheCatalogZone" runat="server" Width="250px" BackColor="#F7F6F3" BorderColor="#CCCCCC" BorderWidth="1px" Font-Names="Arial" Font-Size="8pt" Padding="6" HeaderText="Catalog">
                <HeaderVerbStyle Font-Bold="False" Font-Size="7pt" Font-Underline="False" ForeColor="DimGray" />
                <PartTitleStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="7pt" ForeColor="White" />
                <FooterStyle BackColor="#E2DED6" HorizontalAlign="Right" />
                <PartChromeStyle BorderColor="#E2DED6" BorderStyle="Solid" BorderWidth="1px" />
                <PartLinkStyle Font-Size="7pt" />
                <InstructionTextStyle Font-Size="7pt" ForeColor="DimGray" />
                <LabelStyle Font-Size="7pt" ForeColor="DimGray" />
                <SelectedPartLinkStyle Font-Size="7pt" ForeColor="DimGray" />
                <VerbStyle Font-Names="Arial" Font-Size="7pt" ForeColor="DimGray" />
                <HeaderStyle BackColor="#E2DED6" Font-Bold="True" Font-Size="7pt" ForeColor="DimGray" />
                <EditUIStyle Font-Names="Arial" Font-Size="7pt" ForeColor="DimGray" />
                <PartStyle BorderColor="#F7F6F3" BorderWidth="5px" />
                <EmptyZoneTextStyle Font-Size="7pt" ForeColor="DimGray" />
                <ZoneTemplate>
                    <asp:DeclarativeCatalogPart ID="TheDeclarativeCatalogPart" runat="server">
                        <WebPartsTemplate>
                        </WebPartsTemplate>
                    </asp:DeclarativeCatalogPart>
                    <asp:PageCatalogPart ID="ThePageCatalogPart" runat="server" />
                </ZoneTemplate>
            </asp:CatalogZone>
        </asp:Panel>
        <asp:Panel ID="EditPanel" runat="server">
            <asp:Label ID="EditPanelLabel" runat="server" />
            Edit mode enable you to drag and drop Web Parts. Additionally, you can select Edit from the Web Parts menu to edit the title, size, direction, window appearance, and zone of Web Parts.
        </asp:Panel>
        <asp:Panel ID="BrowsePanel" runat="server">
            <asp:Label ID="BrowsePanelLabel" runat="server" />
        </asp:Panel>
    </asp:Panel>
</div>