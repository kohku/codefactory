<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeFile="Page.aspx.cs"
    Inherits="Page" Theme="" StyleSheetTheme="Default" %>

<%@ Register TagPrefix="ContentManager" Assembly="CodeFactory.ContentManager" Namespace="CodeFactory.ContentManager.WebControls.WebParts" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 250px;
        }
        .style3
        {
            width: 90px;
            text-align: right;
        }
        .style4
        {
            width: 249px;
        }
        .style5
        {
            width: 80px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="LayoutHelper" runat="server" />
    <asp:WebPartManager ID="TheWebPartManager" runat="server">
        <Personalization InitialScope="Shared" />
    </asp:WebPartManager>
    <asp:MultiView ID="TheMultiView" runat="server">
        <asp:View ID="LayoutView" runat="server">
            <asp:DropDownList ID="DisplayMode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DisplayMode_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Panel ID="ToolBarPanel" runat="server">
                <table style="width: 955px;">
                    <tr>
                        <td class="style3">
                            <asp:Label ID="TitleLabel" runat="server" Text="Title"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TitleTextBox" runat="server" Width="250px"></asp:TextBox>
                        </td>
                        <td class="style3">
                            <asp:Label ID="SlugLabel" runat="server" Text="Slug"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="SlugTextBox" runat="server" Width="250px"></asp:TextBox>
                        </td>
                        <td class="style5" style="text-align: right">
                            &nbsp;</td>
                        <td>
                            <asp:CheckBox ID="IsVisibleCheckBox" runat="server" Text="Is Visible" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:Label ID="DescriptionLabel" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="DescriptionTextBox" runat="server" Width="250px"></asp:TextBox>
                        </td>
                        <td class="style3">
                            &nbsp;<asp:Label ID="KeywordsLabel" runat="server" Text="Keywords"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="KeywordsTextBox" runat="server" Width="250px"></asp:TextBox>
                        </td>
                        <td class="style5" style="text-align: right">
                            Roles</td>
                        <td>
                            <asp:DropDownList ID="RolesList" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:Label ID="LayoutLabel" runat="server" Text="Layout"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:DropDownList ID="LayoutList" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="LayoutList_SelectedIndexChanged">
                                <asp:ListItem Value="~/settings/layout/defaultlayout.ascx">Default</asp:ListItem>
                                <asp:ListItem Value="~/settings/layout/emptylayout.ascx">Empty</asp:ListItem>
                                <asp:ListItem Value="~/settings/layout/twocolumns1layout.ascx">Two Columns 1</asp:ListItem>
                                <asp:ListItem Value="~/settings/layout/twocolumns2layout.ascx">Two Columns 2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style3">
                            Parent</td>
                        <td class="style2">
                            <asp:DropDownList ID="ParentsList" runat="server">
                            </asp:DropDownList>
                            &nbsp;</td>
                        <td class="style5" style="text-align: right">
                            &nbsp;</td>
                        <td>
                            <asp:CheckBox ID="IsDefaultCheckBox" runat="server" Text="Is Default Page" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:Label ID="AuthorLabel" runat="server" Text="Author"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style4">
                            &nbsp;<asp:Label ID="AuthorData" runat="server"></asp:Label>
                        </td>
                        <td class="style3">
                            &nbsp;<asp:Label ID="DateCreatedLabel" runat="server" Text="DateCreated"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:Label ID="DateCreatedData" runat="server"></asp:Label>
                        </td>
                        <td class="style5" style="text-align: right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            <asp:Label ID="LastUpdatedByLabel" runat="server" Text="Last updated by"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="LastUpdatedByData" runat="server"></asp:Label>
                        </td>
                        <td class="style3">
                            &nbsp;</td>
                        <td class="style2">
                            &nbsp;</td>
                        <td class="style5" style="text-align: right">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="LayoutPanel" runat="server">
            </asp:Panel>
            <asp:CatalogZone ID="TheCatalogZone" runat="server">
                <ZoneTemplate>
                    <ContentManager:ModulesCatalogPart ID="TheModulesCatalogPart" runat="server" Title="Modules Catalog" />
                    <asp:PageCatalogPart ID="ThePageCatalogPart" runat="server" />
                </ZoneTemplate>
            </asp:CatalogZone>
            <asp:Panel ID="ActionsPanel" runat="server">
                <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" Text="Save" />
                <asp:Button ID="CancelButton" runat="server" CausesValidation="False" OnClick="CancelButton_Click"
                    Text="Cancel" />
                <asp:Button ID="DeleteButton" runat="server" onclick="DeleteButton_Click" 
                    Text="Delete" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="EditorView" runat="server">
            <asp:EditorZone ID="TheEditorZone" runat="server" Width="957px" BackColor="#F7F6F3"
                BorderColor="#CCCCCC" BorderWidth="1px" Font-Names="Verdana" Padding="6" HeaderText="Editor">
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
    </form>
</body>
</html>
