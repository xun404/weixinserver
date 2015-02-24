<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Weixin_Server.MPServer.Management.User" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="添加用户"></asp:Label>
    
        <br />
    
    </div>
    </form>

            <table width="100%" border="0" cellpadding="1" cellspacing="0">
            <tr>
                <td>ID</td>
                <td>帐号</td>
                <td>密码</td>
                <td>操作</td>
            </tr>
            <%
                if (dtUserList.Rows.Count == 0)
                { 
            %>
            <tr>
                <td colspan="8">暂无用户，请先添加！</td>
            </tr>
            <%
                    }
                    else
                    {
                        for (int i = 0; i < dtUserList.Rows.Count; i++)
                        {
            %>
            <tr>
                <td><%=(i+1).ToString()%></td>
                <td><%=dtUserList.Rows[i]["User"].ToString()%></td>
                <td>******</td>
                <td>

                </td>
            </tr>
            <%
                        }
                    }
            %>
        </table>
</body>
</html>
