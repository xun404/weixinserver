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
    public partial class Admin : System.Web.UI.Page
    {
        public DataTable dtUserList = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)Session["User"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            string sAgents = (string)Session["User"];
            dtUserList = GetUserList(sAgents);
        }

        private DataTable GetUserList(string sUser)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_mpweixin_login` WHERE `User` = '{0}'", sUser);
            return CDBAccess.MySqlDt(sSql);
        }

        protected void LButAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Add.aspx");
        }
    }
}