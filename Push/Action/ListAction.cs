using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.Push.Action
{
    public class ListAction
    {
        public static DataTable ShowSendUser(string sOriginalId)
        {
            string sSql = string.Format("SELECT `Id`,`OpenId`,`DateTime` FROM `push_logs` WHERE `DateTime` >= NOW() - INTERVAL 2 DAY AND `OriginalId` = '{0}' GROUP BY `OpenId`", sOriginalId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return dt;
            }
        }
    }
}