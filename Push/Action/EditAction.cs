using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.Push.Action
{
    public class EditAction
    {
        public static DataTable ShowEdit(HttpContext context)
        {
            string sSql = string.Format("Select * From `Push_Messages` WHERE `OriginalId` = '{0}' LIMIT 0, 2", context.Request["OriginalId"]);
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

        internal static void AddEdit(HttpContext context)
        {
            string sSql = string.Format("DELETE FROM `Push_Messages` WHERE `OriginalId` = '{0}'", context.Request["OriginalId"]);
            CDBAccess.MySqlDt(sSql);
            string[] sData = new string[] { context.Request["j1"], context.Request["MsgText1"], context.Request["OriginalId"] };
            sSql = string.Format("INSERT INTO `Push_Messages` (`type`, `text`, `OriginalId`) VALUES ('{0}', '{1}', '{2}')", sData);
            CDBAccess.MySqlDt(sSql);
            sData = new string[] { context.Request["j2"], context.Request["MsgText2"], context.Request["OriginalId"] };
            sSql = string.Format("INSERT INTO `Push_Messages` (`type`, `text`, `OriginalId`) VALUES ('{0}', '{1}', '{2}')", sData);
            CDBAccess.MySqlDt(sSql);
        }
    }
}