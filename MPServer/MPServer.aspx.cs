using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Xml;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.MPServer
{

    /// <summary>
    /// 微信调用接口
    /// </summary>
    public partial class MPServer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //1.微信消息处理
            string sDel = Request["del"];
            switch (Request["f"])
            {
                case "xml":
                    MPWXReturn();
                    if (!string.IsNullOrWhiteSpace(Request["kfid"]))
                    {
                        WeiXinMsgInfo.FakeId = Request["kfid"];
                    }
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(Request["openid"]))
                    {
                        WeiXinMsgInfo.OpenId = Request["openid"];
                    }
                    else if (!string.IsNullOrWhiteSpace(Request["from"]))
                    {
                        WeiXinMsgInfo.OpenId = Request["from"];
                    }
                    else
                    {
                        WritePage("不合法的请求！请不要窃取他人劳动成果，如有问题请联系QQ：5420470");
                    }
                    if (!string.IsNullOrWhiteSpace(Request["content"]))
                    {
                        WeiXinMsgInfo.Text = Request["content"];
                    }
                    if (!string.IsNullOrWhiteSpace(Request["kfid"]))
                    {
                        WeiXinMsgInfo.FakeId = Request["kfid"];
                    }
                    WeiXinMsgInfo.ToUser = Request["to"];
                    break;
            }
            ReturnInitialization();
            WeiXinMsgInfo.OriginalId = Request["OriginalId"];
            if (!WeiXinTool.OriginalIdIsRegUser(WeiXinMsgInfo.OriginalId))
            {
                WritePage("无效的Key值，请在管理后台查看完整API地址!");
            }
            //2.回复微信消息
            WritePage(MsgServer.MsgFeedback());
        }

        /// <summary>
        /// 微信调用
        /// </summary>
        private void MPWXReturn()
        {
            if (Request.HttpMethod.ToLower() == "post")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string sWXml = Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(sWXml))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(sWXml);
                    XmlNodeList list = doc.GetElementsByTagName("xml");
                    XmlNode xn = list[0];
                    WeiXinMsgInfo.OpenId = xn.SelectSingleNode("//FromUserName").InnerText;
                    WeiXinMsgInfo.ToUser = xn.SelectSingleNode("//ToUserName").InnerText;
                    string sType = xn.SelectSingleNode("//MsgType").InnerText;
                    switch (sType.ToLower())
                    {
                        case "text":
                            WeiXinMsgInfo.Text = xn.SelectSingleNode("//Content").InnerText;
                            break;
                        default:
                            WeiXinMsgInfo.Text = string.Empty;
                            break;
                    }
                    return;
                }
            }
            else
            {
                WritePage("不合法的请求！");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void ReturnInitialization()
        {
            string sHostUrl = Request.Url.Host;
            if (!Registration.IsRegistration(sHostUrl))
            {
                WritePage("客服系统API尚未注册，请登录http://" + sHostUrl + "/MPServer/Register.aspx完成注册！");
            }
        }

        /// <summary>
        /// 向Page打印消息
        /// </summary>
        /// <param name="sRetText">消息文本</param>
        public void WritePage(string sRetText,bool bType = true)
        {
            try
            {
                string sBingKeyWord = string.Empty;
                Page.Controls.Clear();
                if (sRetText.Length >= 1500)
                {
                    sRetText = sRetText.Substring(0, 1500);
                }
                if (!string.IsNullOrWhiteSpace(WeiXinMsgInfo.OpenId))
                {

                }
                Response.Write(XmlFormat(sRetText));
                if (!string.IsNullOrWhiteSpace(sBingKeyWord))
                {
                    
                }
                if (bType)
                {
                    Response.End();
                }
                else
                {
                    Response.Flush();
                    Response.Close(); 
                }
            }
            catch
            { }
        }

        private string XmlFormat(string sText)
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

        private string Timestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}