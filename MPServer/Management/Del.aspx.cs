using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.MPServer.Management
{
    public partial class Del : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)Session["User"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            string sUserId = Request["id"];
            if (string.IsNullOrWhiteSpace(sUserId))
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                string sSql = string.Format("DELETE FROM `mpserver_mpweixin_login` WHERE (`Id`='{0}')",sUserId);
                CDBAccess.MySqlDt(sSql);
                Response.Redirect("Admin.aspx");
            }
        }
    }
}