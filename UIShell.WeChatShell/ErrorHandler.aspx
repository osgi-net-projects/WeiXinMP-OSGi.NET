<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorHandler.aspx.cs" Inherits="UIShell.WeChatShell.ErrorHandler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>系统出错了</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        对不起，系统出现异常，请点击以下按钮来重启系统。<br><br>
        <asp:Button ID="restartButton" runat="server" Text="重启系统" Width="200px" 
            onclick="btnRestart_Click" />
            
        <asp:Label ID="errorLabel" runat="server" ForeColor="White" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
