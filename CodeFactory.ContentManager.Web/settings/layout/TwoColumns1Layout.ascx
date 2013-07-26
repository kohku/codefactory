<%@ Control Language="C#" AutoEventWireup="true" %>
<div id="__layout" class="twoColumnsA">
    <div id="__header">
        <asp:WebPartZone ID="HeaderZone" runat="server" Width="100%" SkinID="Publishable">
        </asp:WebPartZone>
    </div>
    <div id="__main">
        <div id="__left">
            <asp:WebPartZone ID="LeftZone" runat="server" Width="100%" Height="100%" SkinID="Publishable">
            </asp:WebPartZone>
        </div>
        <div id="__right">
            <asp:WebPartZone ID="MainZone" runat="server" Width="100%" Height="100%" SkinID="Publishable">
            </asp:WebPartZone>
        </div>
    </div>
    <div id="__footer">
        <asp:WebPartZone ID="FooterZone" runat="server" Width="100%" SkinID="Publishable">
        </asp:WebPartZone>
    </div>
</div>