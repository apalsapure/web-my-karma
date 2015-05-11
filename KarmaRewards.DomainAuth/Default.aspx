<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KarmaRewards.DomainAuth.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tavisca Domain Authentication</title>
</head>
<body style="background-color: #416DA5; color: #6373AD; font-family: Segoe UI, Tahoma, Arial, Verdana;
    font-size: 13px; margin: 0px auto; padding: 0px; text-align: center;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="script" runat="server">
    </asp:ScriptManager>
    <div id="updatingStatus" style="position: absolute; background-color: White; width: 400px;
        text-align: center; font-weight: bold; height: 50px; padding: 15px 0px;">
        Authenticating and Redirecting Back....
        <div style="padding-top: 8px;">
            <asp:Image ID="image" runat="server" ImageUrl="~/Images/ajax-loader-1.gif" /></div>
        <asp:Timer ID="timer" runat="server">
        </asp:Timer>
    </div>
    </form>

    <script language="javascript" type="text/javascript">
        //Get height
        var myWidth = 0, myHeight = 0, progDivWidth = 0;
        if (typeof (window.innerWidth) == 'number') {
            myWidth = window.innerWidth; myHeight = window.innerHeight;
        } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
            myWidth = document.documentElement.clientWidth; myHeight = document.documentElement.clientHeight;
        } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
            myWidth = document.body.clientWidth; myHeight = document.body.clientHeight;
        }
        progDivWidth = myWidth;
        var yWithScroll = 0;
        if (window.innerHeight && window.scrollMaxY) {// Firefox
            yWithScroll = window.innerHeight + window.scrollMaxY;
        } else if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac
            yWithScroll = document.body.scrollHeight;
        } else { // works in Explorer 6 Strict, Mozilla (not FF) and Safari
            yWithScroll = document.body.offsetHeight;
        }

        document.getElementById('updatingStatus').style.top = myHeight / 2 - 80 + 'px';
        document.getElementById('updatingStatus').style.left = progDivWidth / 2 - 200 + 'px';
    </script>

</body>
</html>
