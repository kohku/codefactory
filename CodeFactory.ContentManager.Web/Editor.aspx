<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Editor.aspx.cs" Inherits="Editor"
    StylesheetTheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="TheForm" runat="server">
    <asp:ScriptManager ID="TheScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="__page_main">
        <div id="__content">
            <asp:UpdatePanel ID="PreviewUpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="AcceptButton" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="ContentLabel" runat="server" BorderStyle="Dashed" BorderWidth="1px"
                        Height="100px" Width="400px">Contenido editable...</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="__toolbar">
            <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click" />
            <asp:UpdateProgress ID="PreviewUpdateProgress" runat="server">
                <ProgressTemplate>
                    <asp:Image ImageUrl="~/images/ajax-loader.gif" ID="PreviewProgressImage" runat="server" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Button ID="__ShowButton" runat="server" Text="Show" />
            <cc2:ModalPopupExtender ID="ContentEditorModalPopupExtender" runat="server" CancelControlID="CancelButton"
                OkControlID="__AcceptButton" PopupControlID="__content_panel" TargetControlID="__ShowButton"
                PopupDragHandleControlID="__content_title" BackgroundCssClass="__modal_editor">
            </cc2:ModalPopupExtender>
        </div>
        <asp:Panel ID="__content_panel" runat="server" Style="display: none">
            <div id="__content_title" runat="server">
                <asp:Label ID="ContentTitleLabel" runat="server" Text="Label" />
            </div>
            <asp:UpdatePanel ID="EditorUpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="EditButton" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <cc1:Editor ID="ContentEditor" runat="server" Height="400px" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="__content_actions">
                <asp:Button ID="AcceptButton" runat="server" Text="Accept" OnClick="AcceptButton_Click" />
                <asp:Button ID="__AcceptButton" runat="server" Text="Accept" />
                <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
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
