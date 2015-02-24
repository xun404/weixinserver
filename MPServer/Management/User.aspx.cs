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
    public partial class User : System.Web.UI.Page
    {
        public DataTable dtUserList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)Session["User"]) || (string)Session["User"]!= "admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }
            string sAgents = (string)Session["User"];
            dtUserList = GetUserList(sAgents);
        }
        private DataTable GetUserList(string sUser)
        {
            string sSql = "SELECT * FROM `mpserver_server_user`";
            return CDBAccess.MySqlDt(sSql);
        }
    }
}