<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModalPopupExtender.aspx.cs"
    Inherits="ModalPopupExtender" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="TheForm" runat="server">
    <div>
        <asp:ScriptManager ID="TheScriptManager" runat="server">
        </asp:ScriptManager>
        <asp:Button ID="ShowButton" runat="server" Text="Show" />
        <cc1:ModalPopupExtender ID="TheModalPopupExtender" runat="server" 
            TargetControlID="ShowButton" CancelControlID="HideButton" 
            OkControlID="HideButton" PopupControlID="ThePanel" >
        </cc1:ModalPopupExtender>
        <asp:Panel ID="ThePanel" runat="server" style="display: none;">
            Here we go!
        <asp:Button ID="HideButton" runat="server" Text="Hide" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
