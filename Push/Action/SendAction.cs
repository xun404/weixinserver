using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using Weixin_Server.Bind.Helper;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.Push.Action
{
    public class SendAction
    {
        public static string SendMessage(HttpContext context)
        {
            if (Login() == "0")
            {
                DataTable dt = EditAction.ShowEdit(context);
                if (dt == null)
                {
                    return "请先编辑群发内容列表！";
                }
                else
                {
                    var vSendList = new { j1 = dt.Rows[0]["type"], t1 = dt.Rows[0]["text"], j2 = dt.Rows[1]["type"], t2 = dt.Rows[1]["text"] };
                    DataTable dtList = FakeList(context.Request["originalid"]);
                    System.Web.HttpContext.Current.Response.Write("发送托管服务已启动，已开始发送！");
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.Close();
                    for (int i = 0;  i < dtList.Rows.Count;i++)
                    {
                        if (!string.IsNullOrWhiteSpace(vSendList.t1.ToString()))
                        {
                            if (vSendList.j1.ToString() == "0")
                            {
                                SendMsg.SendMessageText(vSendList.t1.ToString(), dtList.Rows[i]["fakeid"].ToString());
                            }
                            else
                            {
                                SendMsg.SendTuWen(vSendList.t1.ToString(), dtList.Rows[i]["fakeid"].ToString());
                            }
                        }
                        Thread.Sleep(2000);
                        if (!string.IsNullOrWhiteSpace(vSendList.t2.ToString()))
                        {
                            if (vSendList.j2.ToString() == "0")
                            {
                                SendMsg.SendMessageText(vSendList.t2.ToString(), dtList.Rows[i]["fakeid"].ToString());
                            }
                            else
                            {
                                SendMsg.SendTuWen(vSendList.t2.ToString(), dtList.Rows[i]["fakeid"].ToString());
                            }
                        }
                        Thread.Sleep(2000);
                    }
                    return "0";
                }
            }
            else
            {
                return "MP登录失败，请确定帐号密码有效以及未开启短信验证！";
            }
        }

        public static string Login()
        {
            bool bGetFans = Weixin_Server.MPServer.Helper.WeiXinTool.OpenIdToFans(WeiXinMsgInfo.OpenId);
            string[] aMPInfo = Weixin_Server.MPServer.Helper.WeiXinTool.GetMPUserInfo(WeiXinMsgInfo.OriginalId);
            WeiXinBindInfo.MPUser = aMPInfo[0];
            WeiXinBindInfo.MPPass = aMPInfo[1];
            Weixin_Server.Bind.Helper.WeiXinLogin.ExecLogin(WeiXinBindInfo.MPUser, WeiXinBindInfo.MPPass);
            if (aMPInfo == null)
            {
                return "-10011";
            }
            else
            {
                return "0";
            }
        }

        public static DataTable FakeList(string sOriginalId)
        {
            string sSql = string.Format(@"SELECT
	`push_logs`.`OpenId`,
	`mpserver_bridge`.`fakeid`
FROM
	`push_logs`
LEFT JOIN `mpserver_bridge` ON `push_logs`.`OpenId` = `mpserver_bridge`.`openid`
WHERE
	`DateTime` >= NOW() - INTERVAL 2 DAY
AND `OriginalId` = '{0}'
GROUP BY
	`push_logs`.`OpenId`", sOriginalId);
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