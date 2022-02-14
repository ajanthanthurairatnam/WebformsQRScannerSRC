<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QrCodeReader.ascx.cs" Inherits="WebApplication1.QrCodeReader" %>
 <div id="container">
      <h1>QR Code Scanner</h1>

      <a id="btn-scan-qr">
          <img src="QRReaderSrc/src/qr01.png" />
      <a/>

      <canvas hidden="" id="qr-canvas"></canvas>

      <div id="qr-result" hidden="">
         <span id="qroutputData"></span>
      </div>
      <div id="qr-close">
          <input id="btnCloseQrReader" type="button" value="Close" hidden />
      </div>
  </div>
<script src="QRReaderSrc/src/qrCodeScanner.js"></script>
<link href="QRReaderSrc/src/qrCodeScanner.css" rel="stylesheet" />
