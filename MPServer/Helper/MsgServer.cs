using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Weixin_Server.Bind.Helper;

namespace Weixin_Server.MPServer.Helper
{
    public class MsgServer
    {

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <returns></returns>
        public static string MsgFeedback()
        {          
            bool bGetFans = WeiXinTool.OpenIdToFans(WeiXinMsgInfo.OpenId);
            string[] aMPInfo = WeiXinTool.GetMPUserInfo(WeiXinMsgInfo.OriginalId);
            string[] aServerId = WeiXinTool.GetMPServerId(WeiXinMsgInfo.OriginalId);
            WeiXinBindInfo.MPUser = aMPInfo[0];
            WeiXinBindInfo.MPPass = aMPInfo[1];
            WeiXinLogin.ExecLogin(WeiXinBindInfo.MPUser, WeiXinBindInfo.MPPass);
            if (bGetFans)
            { 
                foreach(string sServerId in aServerId)
                {
                    if (sServerId == WeiXinFans.sFakeId)
                    {
                        return ServerBack();
                    }
                }
            }
            if (aMPInfo == null)
            {
                return "客服系统配置出现问题，如果你是本系统管理员请进入后台完成相关设置。";
            }
            WeiXinBindInfo.BreakMsg = Guid.NewGuid().ToString().Substring(0,5) ;
            if (!bGetFans)
            {
                string sOutText = string.Format("您是首次使用“微客服”平台，将会为您自动注册到客服系统，如果在5秒内收到绑定成功信息则代表注册成功！<a href=\"http:////RMBZ.Net\\{0}\"> </a>", WeiXinBindInfo.BreakMsg);
                WritePage(sOutText,false);
                Thread.Sleep(2500);
                string[] aFakeId = Bind.MPBind.BindFakeId(WeiXinBindInfo.BreakMsg, WeiXinBindInfo.MPUser, WeiXinBindInfo.MPPass);
                if (aFakeId.Length == 2)
                {
                    WeiXinFans.sFakeId = aFakeId[0];
                    string sSql = string.Format("INSERT INTO `mpserver_bridge` (`openid`, `fakeid`, `name`, `time`) VALUES ('{0}', '{1}', '{2}', '{3}')", WeiXinMsgInfo.OpenId, aFakeId[0], WeiXinTool.Base64Code(aFakeId[1]), DateTime.Now);
                    CDBAccess.MySqlDt(sSql);
                    SendMsg.SendMessageText(aFakeId[1] + " 绑定成功，现在您就可以和客服联系啦。", WeiXinFans.sFakeId);
                }
                else
                {
                    //WriteFile.Write("log.txt", WeiXinMsgInfo.OpenId + ":NoFoundFakeId.\n");
                }
                return "OK";
            }
            else
            {
                if (aServerId == null)
                {
                    return "无法联系到客服！\n\n本客服系统尚未设置客服人员ID，请管理员尽快设置！";
                }
                else
                {
                    string sServerId = aServerId[0];
                    string MsgText = string.Format("来自 {0} 的消息：\n\n{1}\n\n回复此消息请回复“{2}#回复内容”",WeiXinFans.sNickName,WeiXinMsgInfo.Text,WeiXinFans.sFansId);
                    SendMsg.SendMessageText(MsgText, sServerId);
                    return "消息已经送达至客服，我们会尽快为您做答复！\n回复“退出”可退出“微客服”平台。";
                }
            }
        }

        /// <summary>
        /// 服务器返回处理
        /// </summary>
        /// <returns></returns>
        public static string ServerBack()
        {
            WeiXinMsgInfo.Text = WeiXinMsgInfo.Text.Replace("＃","#");
            string[] aSendMsgInfo = WeiXinMsgInfo.Text.Split('#');
            if (aSendMsgInfo.Length != 2)
            {
                return "回复格式错误，标准格式为“ID#回复内容”，如“121#这个问题已经给你提交，稍后给您答复。”";
            }
            else
            {
                string sFakeId = WeiXinTool.FansIdToFakeId(aSendMsgInfo[0]);
                if (sFakeId == "0")
                {
                    return "此ID无效，没有找到此ID对应的用户信息！请确认ID正确后再回复。";
                }
                else
                {
                    string sSendMsg = string.Format("来自客服的回复：\n{0}\n回复“退出”可退出“微客服”平台。", aSendMsgInfo[1]);
                    SendMsg.SendMessageText(sSendMsg, sFakeId);
                    return "已回复！";
                }
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
                //Page.Controls.Clear();
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

        /// <summary>
        /// XML
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private static string Timestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}