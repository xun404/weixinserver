using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.MPServer.Management
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sHostUrl = Request.Url.Host;
            if (!Registration.IsRegistration(sHostUrl))
            {
                Response.Clear();
                Response.Write("<script LANGUAGE='javascript'>alert('微客服平台尚未注册！');</script>");
                Response.Redirect("../Register.aspx");
                return; 
            }
        }

        protected void ButLogin_Click(object sender, EventArgs e)
        {
            string sUser = TBUser.Text;
            string sPass = TBPass.Text;
            if (string.IsNullOrWhiteSpace(sUser))
            {
                Tips.Text = "用户名不能为空";
                return;
            }
            if (string.IsNullOrWhiteSpace(sPass))
            {
                Tips.Text = "用户名不能为空";
                return;
            }
            string sSql = string.Format("SELECT * FROM `mpserver_server_user` WHERE `user`='{0}'",sUser);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                Tips.Text = "帐号或密码错误！";
                return;
            }
            else
            {
                if (dt.Rows[0]["pass"].ToString() != sPass)
                {
                    Tips.Text = "帐号或密码错误！";
                    return;
                }
                else
                {
                    Session.Add("User", sUser);
                    Response.Redirect("admin.aspx");
                }
            }
        }
    }
}