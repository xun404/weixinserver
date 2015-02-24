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
    public partial class Edit : System.Web.UI.Page
    {
        string sUserID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)Session["User"]))
            {
                Response.Redirect("Login.aspx");
                return;
            }
            sUserID = Request["ID"];
            if (string.IsNullOrWhiteSpace(sUserID))
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                string sSql = string.Format("SELECT * FROM `mpserver_mpweixin_login` WHERE `Id` = {0}",sUserID);
                DataTable dt = CDBAccess.MySqlDt(sSql);
                if (dt.Rows.Count <= 0)
                {
                    Response.Redirect("Admin.aspx");
                }
                else
                {
                    TBMPUser.Text = dt.Rows[0]["MPUser"].ToString();
                    TBPass.Text = dt.Rows[0]["MPPass"].ToString();
                    TBYsid.Text = dt.Rows[0]["OriginalId"].ToString();
                    TBKfid.Text = dt.Rows[0]["ServerId"].ToString();
                }
            }
        }

        protected void ButEdit_Click(object sender, EventArgs e)
        {
            string sSql = string.Format("UPDATE `mpserver_mpweixin_login` SET `MPUser`='{0}', `MPPass`='{1}', `OriginalId`='{2}', `MPKey`='{3}', `User`='{4}', `ServerId`='{5}' WHERE (`Id`='{6}')",
                TBMPUser.Text,TBPass.Text,TBYsid.Text,TBKfid.Text, sUserID);
            CDBAccess.MySqlDt(sSql);
            Response.Redirect("Admin.aspx");
        }

        protected void ButBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");
        }
    }
}