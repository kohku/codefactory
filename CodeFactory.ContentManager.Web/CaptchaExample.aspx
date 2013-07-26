<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaptchaExample.aspx.cs" Inherits="CaptchaExample" Culture="es-MX" uiCulture="es-MX" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var RecaptchaOptions = {
            theme: 'white',
            tabindex: 2
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label Visible="false" ID="lblResult" runat="server" />
        <recaptcha:RecaptchaControl ID="recaptcha" runat="server"
            PublicKey="6LficAYAAAAAAL6I7UyvWp6qXiDfOSibZ6Dl-pTU" 
            PrivateKey="6LficAYAAAAAAIDr52IXCoL-xtnPkFHt5sZVSHlp" Language="es" 
            Theme="clean" />
            <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
