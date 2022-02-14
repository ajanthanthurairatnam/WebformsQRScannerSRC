<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<%@ Register Src="~/QRReaderSrc/QrCodeReader.ascx" TagPrefix="uc1" TagName="QrCodeReader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script>
        function qrcodecallback(response) {
            //something you want to do with the response
        }
        function qrcodeClosecallback(response) {
            //something you want to do when click close
        }
        
    </script>
     <form id="form1" runat="server">
        <div>
            <uc1:QrCodeReader runat="server" ID="QrCodeReader" />
        </div>
    </form>
</body>
</html>
