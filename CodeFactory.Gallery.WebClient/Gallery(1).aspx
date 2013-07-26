<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gallery(1).aspx.cs" Inherits="_Gallery_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gallery</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0"
            width="600" height="161">
            <param name="movie" value="gallery.swf">
            <param name="quality" value="high">
            <embed src="XML_dynamic_gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer"
                type="application/x-shockwave-flash" width="600" height="161">
            </embed>
        </object>
    </div>
    </form>
</body>
</html>
