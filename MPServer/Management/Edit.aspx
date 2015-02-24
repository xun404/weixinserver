<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Weixin_Server.MPServer.Management.Edit" %>

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
        <asp:Label ID="Label3" runat="server" Text="原始ID："></asp:Label>
        <asp:TextBox ID="TBYsid" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label4" runat="server" Text="客服ID："></asp:Label>
        <asp:TextBox ID="TBKfid" runat="server"></asp:TextBox>
        <br />
        <br />
        <br />
        <asp:Button ID="ButEdit" runat="server" OnClick="ButEdit_Click" Text="修  改" />
        <asp:Button ID="ButBack" runat="server" OnClick="ButBack_Click" Text="返  回" />
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
