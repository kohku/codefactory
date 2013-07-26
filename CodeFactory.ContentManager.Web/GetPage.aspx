<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GetPage.aspx.cs" Inherits="GetPage"
    StylesheetTheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="ContentManager" Assembly="CodeFactory.ContentManager" Namespace="CodeFactory.ContentManager.WebControls.WebParts" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 80px;
            text-align: right;
        }
        .style3
        {
            width: 280px;
        }
        .style4
        {
            width: 50px;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="TheForm" runat="server">
    <asp:WebPartManager ID="TheWebPartManager" runat="server">
        <Personalization InitialScope="Shared" />
    </asp:WebPartManager>
    <asp:ScriptManager ID="TheScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="LayoutHelper" runat="server" />
    <div id="__page_main">
        <asp:DropDownList ID="DisplayMode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DisplayMode_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:Panel ID="__page_properties_title" runat="server">
            <asp:Label ID="PagePropertiesTitle" runat="server" Text="Title"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="__page_properties" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="style1">
                        &nbsp;
                        <asp:Label ID="TitleLabel" runat="server" Text="Title"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TitleTextBox" runat="server" Width="260px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TitleTextBox"
                            Display="Dynamic" ErrorMessage="*">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="style1">
                        <asp:Label ID="SlugLabel" runat="server" Text="Slug"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="SlugTextBox" runat="server" Width="260px"></asp:TextBox>
                    </td>
                    <td class="style4">
                        <asp:Label ID="IsVisibleLabel" runat="server" Text="Is visible"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="IsVisibleCheckBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;
                        <asp:Label ID="DescriptionLabel" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="DescriptionTextBox" runat="server" Rows="3" TextMode="MultiLine"
                            Width="260px"></asp:TextBox>
                    </td>
                    <td class="style1">
                        <asp:Label ID="KeywordsLabel" runat="server" Text="Keywords"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="KeywordsTextBox" runat="server" Rows="3" TextMode="MultiLine" Width="260px"></asp:TextBox>
                    </td>
                    <td class="style4">
                        <asp:Label ID="RolesLabel" runat="server" Text="Roles"></asp:Label>
                    </td>
                    <td>
                        <asp:ListBox ID="RolesList" runat="server" Width="100px"></asp:ListBox>
                        &nbsp;<asp:Button ID="SelectRolesButton" runat="server" Text="Select" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;
                        <asp:Label ID="LayoutLabel" runat="server" Text="Layout"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:DropDownList ID="LayoutList" runat="server" Width="200px" AutoPostBack="True"
                            OnSelectedIndexChanged="LayoutList_SelectedIndexChanged">
                            <asp:ListItem Value="~/settings/layout/defaultlayout.ascx">Default</asp:ListItem>
                            <asp:ListItem Value="~/settings/layout/emptylayout.ascx">Empty</asp:ListItem>
                            <asp:ListItem Value="~/settings/layout/twocolumns1layout.ascx">Two Columns 1</asp:ListItem>
                            <asp:ListItem Value="~/settings/layout/twocolumns2layout.ascx">Two Columns 2</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style1">
                        <asp:Label ID="Label6" runat="server" Text="Section"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="SectionTextBox" runat="server" Width="200px"></asp:TextBox>
                        &nbsp;<asp:Button ID="SelectSectionButton" runat="server" Text="Select" />
                    </td>
                    <td class="style4">
                        <asp:Label ID="IsDefaultLabel" runat="server" Text="Is default"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="IsDefaultCheckBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:Label ID="AuthorLabel" runat="server" Text="Author"></asp:Label>
                    </td>
                    <td class="style3">
                        &nbsp;
                        <asp:Label ID="Author" runat="server"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:Label ID="DateCreatedLabel" runat="server" Text="Date created"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="DateCreated" runat="server"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:Label ID="LastUpdatedByLabel" runat="server" Text="Last updated by"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="LastUpdatedBy" runat="server"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:Label ID="LastUpdatedLabel" runat="server" Text="Last updated"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="LastUpdated" runat="server"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <cc2:CollapsiblePanelExtender ID="__page_properties_CollapsiblePanelExtender" runat="server" TargetControlID="__page_properties"
            TextLabelID="PagePropertiesTitle" CollapsedText="Expand" ExpandedText="Collapse" ExpandControlID="PagePropertiesTitle" 
            CollapseControlID="PagePropertiesTitle">
        </cc2:CollapsiblePanelExtender>
        <div id="__content">
            <asp:UpdatePanel ID="PreviewUpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="AcceptButton" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="LayoutPanel" runat="server">
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Panel ID="__page_catalog_title" runat="server">
            <asp:Label ID="PageCatalogTitle" runat="server" Text="Title"></asp:Label>
        </asp:Panel>
        <asp:Panel runat="server" ID="__page_catalog">
            <asp:CatalogZone ID="TheCatalogZone" runat="server">
                <ZoneTemplate>
                    <ContentManager:ModulesCatalogPart ID="TheModulesCatalogPart" runat="server" Title="Modules Catalog" />
                    <asp:PageCatalogPart ID="ThePageCatalogPart" runat="server" />
                </ZoneTemplate>
            </asp:CatalogZone>
        </asp:Panel>
        <cc2:CollapsiblePanelExtender ID="__page_catalog_CollapsiblePanelExtender" runat="server" TargetControlID="__page_catalog"
            TextLabelID="PageCatalogTitle" CollapsedText="Expand" ExpandedText="Collapse" ExpandControlID="PageCatalogTitle" 
            CollapseControlID="PageCatalogTitle">
        </cc2:CollapsiblePanelExtender>
        <asp:Panel ID="__page_actions" runat="server">
            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" CausesValidation="false" />
            <asp:Button ID="DeleteButton" runat="server" Text="Delete" OnClick="DeleteButton_Click" CausesValidation="false" />
            <cc2:ConfirmButtonExtender ID="DeleteConfirmButtonExtender" TargetControlID="DeleteButton" ConfirmText="Are you sure?" runat="server">
            </cc2:ConfirmButtonExtender>
        </asp:Panel>
        <div id="__toolbar" style="display: none">
            <asp:Label ID="ContentLabel" runat="server" BorderStyle="Dashed" BorderWidth="1px"
                Height="100px" Width="400px">Contenido editable...</asp:Label>
            <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click" />
            <asp:UpdateProgress ID="PreviewUpdateProgress" runat="server">
                <ProgressTemplate>
                    <asp:Image ImageUrl="~/images/ajax-loader.gif" ID="PreviewProgressImage" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Button ID="__ShowButton" runat="server" Text="Show" />
            <cc2:ModalPopupExtender ID="ContentEditorModalPopupExtender" runat="server" CancelControlID="EditCancelButton"
                OkControlID="__AcceptButton" PopupControlID="__content_panel" TargetControlID="__ShowButton"
                PopupDragHandleControlID="__content_title" BackgroundCssClass="__modal_editor">
            </cc2:ModalPopupExtender>
        </div>
        <asp:Panel ID="__content_panel" runat="server" Style="display: none">
            <div id="__content_title" runat="server">
                <asp:Label ID="ContentTitleLabel" runat="server" Text="Label" />
            </div>
            <asp:UpdatePanel ID="__editor_update_panel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="EditButton" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <cc1:Editor ID="__content_editor" runat="server" Height="400px" HtmlPanelCssClass="custom_html_editor"
                        CssClass="custom_editor" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="__content_actions">
                <asp:Button ID="AcceptButton" runat="server" Text="Accept" OnClick="AcceptButton_Click" />
                <asp:Button ID="__AcceptButton" runat="server" Text="Accept" />
                <asp:Button ID="EditCancelButton" runat="server" Text="Edit Cancel" />
            </div>
            <div id="__content_progress">
                <asp:UpdateProgress ID="EditorUpdateProgress" runat="server">
                    <ProgressTemplate>
                        <asp:Image ImageUrl="~/images/ajax-loader.gif" ID="EditorProgressImage" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
