using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.MPServer
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ButRegistration_Click(object sender, EventArgs e)
        {
            string sRegSnText = TBSN.Text.Replace("'","");
            if (string.IsNullOrWhiteSpace(sRegSnText) || sRegSnText.Length != 36)
            {
                Response.Write("<script LANGUAGE='javascript'>alert('SN错误，请输入此输入平台正确的SN注册码！');</script>");
            }
            else
            {
                string sHostName = Request.Url.Host;
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('注册成功，绑定域名：{0}!');</script>", sHostName));
                Response.Redirect("Register.aspx");
            }
        }

        private void ShowRegistration()
        {

        }
    }
}