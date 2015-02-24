using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weixin_Server.Bind.Helper;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.MPServer.Management
{
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)Session["User"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }

        protected void ButEdit_Click(object sender, EventArgs e)
        {
            string sMPKey = Guid.NewGuid().ToString().Substring(1, 5);
            string sAddUser = (string)Session["User"];
            string sSql = string.Format("INSERT INTO `mpserver_mpweixin_login` (`MPUser`, `MPPass`, `OriginalId`, `MPKey`, `User`, `ServerId`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                TBMPUser.Text, TBPass.Text, TBYuanShiId.Text, sMPKey, sAddUser, TBKfid.Text);
            CDBAccess.MySqlDt(sSql);
            Response.Redirect("Admin.aspx");
        }

        protected void ButBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");
        }
    }
}