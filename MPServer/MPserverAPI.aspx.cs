using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.MPServer
{
    public partial class MPserverAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sOpenId = Request["OpenId"];
            string sFakeId = Request["FakeId"];
            string sDo = Request["Do"];
            string sSession = Request["Session"];
            if (string.IsNullOrWhiteSpace(sOpenId) || string.IsNullOrWhiteSpace(sFakeId))
            {
                Response.Write("null");
                Response.End();
            }
            switch (sDo)
            { 
                case "Add":
                    DelSession(sOpenId);
                    AddSession(sOpenId,sFakeId,sSession);
                    Response.Write("add");
                    break;

                case "Del":
                    DelSession(sOpenId);
                    Response.Write("del");
                    break;

                case "Check":
                    if (CheckSession(sOpenId, sSession))
                    {
                        Response.Write("yse");
                    }
                    else
                    {
                        Response.Write("no");
                    }
                    break;
            }
            Response.End();
        }

        private void AddSession(string sOpenId,string sFakeId,string sSession)
        {
            string sSql = string.Format("INSERT INTO `mpserver_session` (`session`, `time`, `user`, `other`) VALUES ('{0}', '{1}', '{2}', '{3}')",
                sSession,DateTime.Now,sOpenId,sFakeId);
            CDBAccess.MySqlDt(sSql);
        }

        private void DelSession(string sOpenId)
        {
            string sSql = string.Format("DELETE FROM `mpserver_session` WHERE (`User`='{0}')",sOpenId);
            CDBAccess.MySqlDt(sSql);
        }

        private bool CheckSession(string sOpenId,string sSession)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_session` WHERE `User` = '{0}' AND `session` = '{1}'", sOpenId,sSession);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                return false;
            }
            else
            {
                DateTime dtTime = (DateTime)dt.Rows[0]["time"];
                if ((DateTime.Now - dtTime).TotalSeconds <= 300)
                {
                    return true;
                }
                else
                {
                    DelSession(sOpenId);
                    return true;
                }
            }
        }
    }
}