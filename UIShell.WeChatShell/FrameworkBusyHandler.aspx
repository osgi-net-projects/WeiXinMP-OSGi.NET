<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameworkBusyHandler.aspx.cs" Inherits="UIShell.WeChatShell.FrameworkBusyHandler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>系统忙或已停止</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="hintLabel" runat="server"></asp:Label><br><br>
        <asp:Button ID="reloadButton" runat="server" Width="200px" Text="返回首页" 
            onclick="reloadButton_Click" />
    </div>
    </form>
</body>
</html>
