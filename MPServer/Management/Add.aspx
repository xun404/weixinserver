<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Weixin_Server.MPServer.Management.Add" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="MPUser："></asp:Label>
        <asp:TextBox ID="TBMPUser" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label2" runat="server" Text="MPPass："></asp:Label>
        <asp:TextBox ID="TBPass" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label5" runat="server" Text="原始ID："></asp:Label>
        <asp:TextBox ID="TBYuanShiId" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label4" runat="server" Text="客服ID："></asp:Label>
        <asp:TextBox ID="TBKfid" runat="server"></asp:TextBox>
        <br />
        <br />
        <br />
        <asp:Button ID="ButEdit" runat="server" Text="添  加" style="height: 21px" OnClick="ButEdit_Click" />
        <asp:Button ID="ButBack" runat="server" Text="返  回" OnClick="ButBack_Click" />
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
