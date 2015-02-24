<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Weixin_Server.MPServer.Management.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        MPServer登录<br />
        帐号：<asp:TextBox ID="TBUser" runat="server"></asp:TextBox>
        <br />
        密码：<asp:TextBox ID="TBPass" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Tips" runat="server"></asp:Label>
        <br />
        <asp:Button ID="ButLogin" runat="server" OnClick="ButLogin_Click" Text="登  录" />
    
    </div>
    </form>
</body>
</html>
