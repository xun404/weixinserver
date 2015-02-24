<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Weixin_Server.MPServer.Management.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:LinkButton ID="LButAdd" runat="server" OnClick="LButAdd_Click">添加用户</asp:LinkButton>
        <br />
    
    </div>
    </form>

            <table width="100%" border="0" cellpadding="1" cellspacing="0">
            <tr>
                <td>ID</td>
                <td>帐号</td>
                <td>密码</td>
                <td>API密钥</td>
                <td>客服ID</td>
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
                <td><%=dtUserList.Rows[i]["MPUser"].ToString()%></td>
                <td>******</td>
                <td><%=dtUserList.Rows[i]["MPKey"].ToString()%></td>
                <td><%=dtUserList.Rows[i]["ServerId"].ToString()%></td>
                <td>
                    <li><a href="API.aspx?Key=<%=dtUserList.Rows[i]["MPKey"].ToString()%>">查看API地址</a></li>
                    <li><a href="Edit.aspx?ID=<%=dtUserList.Rows[i]["ID"].ToString()%>">修改</a></li>
                    <li><a href="Del.aspx?ID=<%=dtUserList.Rows[i]["ID"].ToString()%>">删除</a></li>
                </td>
            </tr>
            <%
                        }
                    }
            %>
        </table>
</body>
</html>
