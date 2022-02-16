<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QrCodeReader.ascx.cs" Inherits="QrCodeReader" %>
 <div id="qr-container">
      <h1>QR Code Scanner</h1>
      <canvas hidden="" id="qr-canvas"></canvas>
      <div id="qr-result" hidden="">
         <span id="qroutputData"></span>
      </div>
      <div id="qr-close">
          <input id="btnCloseQrReader" type="button" value="Close" hidden />
      </div>
  </div>
<script src="../QRResources/qrCodeScanner.js"></script>
<link href="../QRResources/qrCodeScanner.css" rel="stylesheet" />
