<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileManager.aspx.cs" Inherits="tools_FileManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, #__the_form
        {
            margin: 0;
            padding: 0;
            width: 100%;
            height: 100%;
            background: #cccccc;
        }
        div#__file_manager
        {
            position: relative;
            margin: 0 auto;
            width: auto;
            
            height: auto !important; /* real browsers */
            height: 100%;
            min-height: 100%; /* real browsers */
        }
        div#__address_bar
        {
            padding: 1em;
            border-bottom: 6px double gray;
            background: #fcfcfc;
        }
        div#__tree
        {
        }
        div#__content
        {
            padding: 1em 1em 5em; /* bottom padding for footer */
            background: #f0f0f0;
            
            height: auto !important; /* real browsers */
            height: 100%;
            min-height: 100%; /* real browsers */
        }
        div#__tools
        {
        }
        div#__status_bar
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
    <form id="__the_form" runat="server">
    <div id="__file_manager">
        <div id="__address_bar">
            <div style="border: dashed 1px #cccccc;">
                Address bar</div>
        </div>
        <!--
        <div id="__tree">
            <div style="border: dashed 1px #cccccc;">Tree</div>
        </div>
        -->
        <div id="__content">
            Container
        </div>
        <!--
        <div id="__tools">
            <div style="border: dashed 1px #cccccc;">Tools</div>
        </div>
        -->
        <div id="__status_bar">
            <div style="border: dashed 1px #cccccc;">
                Status bar</div>
        </div>
    </div>
    </form>
</body>
</html>
