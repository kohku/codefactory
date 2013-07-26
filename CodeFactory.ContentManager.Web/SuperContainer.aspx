<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperContainer.aspx.cs" Inherits="SuperContainer"
    StylesheetTheme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv='X-UA-Compatible' content='IE=edge' />
    <title></title>
    <style type="text/css">
        html, body, #theForm
        {
            margin: 0;
            padding: 0;
            width: 100%;
            height: 100%;
            background: #cccccc;
        }
        div#container
        {
            position: relative;
            margin: 0 auto;
            width: auto;
            background: #f0f0f0;
            height: auto !important; /* real browsers */
            height: 100%;
            min-height: 100%; /* real browsers */
        }
        div#header
        {
            padding: 1em;
            border-bottom: 6px double gray;
        }
        div#content
        {
            padding: 1em 1em 5em; /* bottom padding for footer */
        }
        div#footer
        {
            position: absolute;
            width: 100%;
            bottom: 0; /* stick to bottom */
            background: #ddd;
            border-top: 6px double gray;
        }
    </style>
</head>
<body>
    <form id="theForm" runat="server">
    <div id="container">
        <div id="header">
            Header</div>
        <div id="content">
            Content</div>
        <div id="footer">
            Content</div>
    </div>
    </form>
</body>
</html>
