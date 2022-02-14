<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="WebApplication1.Default2" %>

<%@ Register Src="~/QRReaderSrc/QrCodeReader.ascx" TagPrefix="uc1" TagName="QrCodeReader" %>


<!DOCTYPE html>

<!-- saved from url=(0039)https://dev.aladn.com.au/APMS_LocWatch/ -->
<html xmlns="http://www.w3.org/1999/xhtml" class="fullscreen boxsizing">
    <head><meta http-equiv="Content-Type" content="text/html; charset=UTF-8"><title>
	AAP - Location Watch
</title><meta http-equiv="Cache-Control" content="no-store"><meta http-equiv="Expires" content="0">
    <link rel="StyleSheet" href="CSS/Default.css" type="text/css">
    <link rel="StyleSheet" href="CSS/DefaultFonts.css" type="text/css">
    <link rel="StyleSheet" href="CSS/aap.css" type="text/css">
    <script language="javascript" type="text/javascript">

        /////////////////////////////////
        // Launch QR Reader ....
        /////////////////////////////////
        function LaunchQRReader() {

            alert("Launch QR Reader");

            window.open("camera.html");
        }

        ////////////////////////////////
        // First Thing 
        ////////////////////////////////
        function Window_Onload() {

            if(eText != "")
            {
                alert(eText);
            }
        }
        
        
    </script>
    
    
<style id="THY_WPF">
@font-face {
font-family:'Roboto-Regular' ;
src: url('chrome-extension://mfpddejbpnbjkjoaicfedaljnfeollkh/fonts/font/Roboto-Regular.woff') ;
}

@font-face {
font-family:'Roboto-Medium' ;
src: url('chrome-extension://mfpddejbpnbjkjoaicfedaljnfeollkh/fonts/font/Roboto-Medium.woff') ;
}
</style><link type="image/x-icon" rel="shortcut icon"></head>
<body class="fullscreen boxsizing" onload="return Window_Onload();">

     <script>
         function qrcodecallback(response) {
             //alert(response);
             document.getElementById("txt_ContractNo").value = response;
         }
         function qrcodeClosecallback(response) {
             //alert(response);
         }

</script>
    <form name="form1" method="post" action="https://dev.aladn.com.au/APMS_LocWatch/Default.aspx" id="form1" class="fullscreen boxsizing">
<div>
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwULLTE3NzE1ODExMDkPZBYCAgMPZBYCAgEPD2QWAh4Kb25rZXlwcmVzcwUdcmV0dXJuIGZpbHRlcklucHV0KHRoaXMsJ24nKTtkZLfuVMGTh54W2LODGI/4Zi42nyXq">
</div>

<div>

	<input type="hidden" name="__VIEWSTATEGENERATOR" id="__VIEWSTATEGENERATOR" value="7C4011F2">
	<input type="hidden" name="__EVENTVALIDATION" id="__EVENTVALIDATION" value="/wEWBALnoLSpCQKfj6/+CQLb0PXrCwKln/PuCt8hNySBuVi1yJB1ji2RNUabVAoZ">
</div>
    
    <div class="qr_box boxsizing">
    
        <table style="width:300px" border="0" align="center" class="qr_Fields" cellpadding="0" cellspacing="0">
        
            <tbody><tr>
                <td style="width:120px"></td>
                <td style="width:130px"></td>
                <td style="width:50px"></td>
            </tr>
        
            <tr class="trFields">
                <td class="label">Contract&nbsp;#</td>
                <td><input name="txt_ContractNo" type="text" id="txt_ContractNo" onkeypress="return filterInput(this,&#39;n&#39;);" style="width:120px;"></td>
                <td>
                    
                  
                </td>
            </tr>
                 <tr class="trFields">
                     <td colspan="3">
                          <uc1:QrCodeReader runat="server" ID="QrCodeReader" />

                     </td>
                     </tr>
             
        
            <tr class="trFields">
                <td class="label">Car&nbsp;Registration</td>
                <td><input name="txt_CarRego" type="text" id="txt_CarRego" style="width:120px;"></td>
                <td></td>
            </tr>
            
            <tr>
                <td></td>
                <td><input type="submit" name="btnSearch" value="Search" id="btnSearch" style="width:80px;"></td>
                <td></td>
            </tr>
        
        </tbody></table>
    </div>

    

<script type="text/javascript">
//<![CDATA[
var txt_ContractNo = document.all ? document.all["txt_ContractNo"]: document.getElementById("txt_ContractNo") ;
var txt_CarRego = document.all ? document.all["txt_CarRego"]: document.getElementById("txt_CarRego") ;
var eText = '';
//]]>
</script>
</form>
    
    
    
    



