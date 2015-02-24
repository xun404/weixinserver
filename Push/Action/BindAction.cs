using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.Push.Action
{
    public class BindAction
    {
        /*ERROR CODE: 
         * -10011 MP帐号密码未设置
         * -10001 系统错误
         * 1绑定成功
         * 0记录成功
         */
        public static string IsBindFakeId()
        {
            bool bGetFans = WeiXinTool.OpenIdToFans(WeiXinMsgInfo.OpenId);
            string[] aMPInfo = WeiXinTool.GetMPUserInfo(WeiXinMsgInfo.OriginalId);
            WeiXinBindInfo.MPUser = aMPInfo[0];
            WeiXinBindInfo.MPPass = aMPInfo[1];
            Weixin_Server.Bind.Helper.WeiXinLogin.ExecLogin(WeiXinBindInfo.MPUser, WeiXinBindInfo.MPPass);
            if (aMPInfo == null)
            {
                return "-10011";
            }
            WeiXinBindInfo.BreakMsg = Guid.NewGuid().ToString().Substring(0, 5);
            //WeiXinBindInfo.BreakMsg = "cbe5c";
            if (!bGetFans)
            {
                string sOutText = string.Format("您是首次使用本平台，将会为您自动注册，如果在5秒内收到绑定成功信息则代表注册成功！<a href=\"http:////Rmbz.Net\\{0}\"> </a>", WeiXinBindInfo.BreakMsg);
                //MPServer mp = new MPServer();
                WritePage(sOutText, false);
                Thread.Sleep(2500);
                string[] aFakeId = Bind.MPBind.BindFakeId(WeiXinBindInfo.BreakMsg, WeiXinBindInfo.MPUser, WeiXinBindInfo.MPPass);
                if (aFakeId.Length == 2)
                {
                    WeiXinFans.sFakeId = aFakeId[0];
                    string sSql = string.Format("INSERT INTO `mpserver_bridge` (`openid`, `fakeid`, `name`, `time`) VALUES ('{0}', '{1}', '{2}', '{3}')", WeiXinMsgInfo.OpenId, aFakeId[0], WeiXinTool.Base64Code(aFakeId[1]), DateTime.Now);
                    CDBAccess.MySqlDt(sSql);
                    Weixin_Server.Bind.Helper.SendMsg.SendMessageText(aFakeId[1] + " 绑定成功，现在您可以体验完整功能啦。", WeiXinFans.sFakeId);
                    return "1";
                }
                else
                {
                    string sSql = string.Format("INSERT INTO `mpserver_bridge` (`openid`, `fakeid`, `name`, `time`) VALUES ('{0}', '{1}', '{2}', '{3}')", WeiXinMsgInfo.OpenId, 0, 0, DateTime.Now);
                    CDBAccess.MySqlDt(sSql);
                    return "-10001";
                }
            }
            else
            {
                string sSql = string.Format("INSERT INTO `Push_logs` (`openid`, `text`, `do`, `datetime`, `OriginalId`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", WeiXinMsgInfo.OpenId, WeiXinMsgInfo.Text, WeiXinMsgInfo.Do, DateTime.Now, WeiXinMsgInfo.OriginalId);
                CDBAccess.MySqlDt(sSql);
                return "0";
            }
        }
        /// <summary>
        /// 向Page打印消息
        /// </summary>
        /// <param name="sRetText">消息文本</param>
        public static void WritePage(string sRetText, bool bType = true)
        {
            try
            {
                string sBingKeyWord = string.Empty;
                if (sRetText.Length >= 1500)
                {
                    sRetText = sRetText.Substring(0, 1500);
                }
                System.Web.HttpContext.Current.Response.Write(XmlFormat(sRetText));
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.Close();
            }
            catch
            { }
        }

        private static string XmlFormat(string sText)
        {
            string sXml = @"
<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[{3}]]></Content>
</xml>
                ";
            return string.Format(sXml, WeiXinMsgInfo.OpenId, WeiXinMsgInfo.ToUser, Timestamp(), sText);
        }

        private static string Timestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}