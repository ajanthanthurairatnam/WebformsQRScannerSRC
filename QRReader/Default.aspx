<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" class="fullscreen boxsizing" >
<head runat="server">
    <title>AAP - Location Watch</title>
    
    <meta http-equiv="Cache-Control" content="no-store" />
    <meta http-equiv="Expires" content="0" />
    <link rel="StyleSheet" href="CSS/Default.css" type="text/css" />
    <link rel="StyleSheet" href="CSS/DefaultFonts.css" type="text/css" />
    <link rel="StyleSheet" href="CSS/aap.css" type="text/css" />
    
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
    
    
</head>
<body class="fullscreen boxsizing"  onload="return Window_Onload();">
    <form id="form1" runat="server" class="fullscreen boxsizing">
    
    <div class="qr_box boxsizing"  >
    
        <table style="width:300px" border="0" align="center" class="qr_Fields" cellpadding="0" cellspacing="0">
        
            <tr>
                <td style="width:120px"></td>
                <td style="width:130px"></td>
                <td style="width:50px"></td>
            </tr>
        
            <tr class="trFields">
                <td class="label">Contract&nbsp;#</td>
                <td><asp:TextBox ID="txt_ContractNo" runat="server" style="width:120px;"></asp:TextBox></td>
                <td><img src="images/qr01.png" alt="QR Icon" width="20px" onclick="LaunchQRReader();" /></td>
            </tr>
        
            <tr class="trFields">
                <td class="label">Car&nbsp;Registration</td>
                <td><asp:TextBox ID="txt_CarRego" runat="server" style="width:120px;"></asp:TextBox></td>
                <td></td>
            </tr>
            
            <tr>
                <td></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Search" Width="80px" 
                        onclick="btnSearch_Click" /></td>
                <td></td>
            </tr>
        
        </table>
    </div>

    </form>
    
    
    
    
</body>
</html>
