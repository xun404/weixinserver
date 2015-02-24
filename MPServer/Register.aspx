<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Weixin_Server.MPServer.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <h4>感谢使用MPServe微客服平台</h4> 
    
    </div>
        <p>
            SN：<asp:TextBox ID="TBSN" runat="server" MaxLength="36" Width="357px"></asp:TextBox>
            <asp:Button ID="ButRegistration" runat="server" OnClick="ButRegistration_Click" Text="注册平台" />
        </p>
        <p>
            <asp:Label ID="LabDomainName" runat="server" ForeColor="Blue"></asp:Label>
        </p>
    </form>
</body>
</html>
