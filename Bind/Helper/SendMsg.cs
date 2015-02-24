using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Weixin_Server.Bind.Helper
{
    public class SendMsg
    {
        /// <summary>
        /// 推送文本消息
        /// </summary>
        /// <param name="sText">消息内容</param>
        /// <param name="sFakeId">接受FakeId</param>
        /// <returns>返回发送状态</returns>
        public static string SendMessageText(string sMessage, string sFakeId, string sImageCode = "")
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            Dictionary<string, string> header = new Dictionary<string, string>();
            postData.Add("content", sMessage);
            postData.Add("imgcode", sImageCode);
            postData.Add("tofakeid", sFakeId);
            postData.Add("token", LoginInfo.Token);
            postData.Add("type", "1");
            postData.Add("lang", "zh_CN");
            postData.Add("t", "ajax-response");
            postData.Add("random", new Random().NextDouble().ToString());
            header.Add("Referer", string.Format("https://mp.weixin.qq.com/cgi-bin/singlesendpage?t=message/send&action=index&tofakeid={0}&token={1}&lang=zh_CN", sFakeId, LoginInfo.Token));
            return HttpHelper.GetTextByPost("https://mp.weixin.qq.com/cgi-bin/singlesend", postData, header, Encoding.UTF8);
        }

        /// <summary>
        /// 推送图文消息
        /// </summary>
        /// <param name="sFid">图文消息ID</param>
        /// <param name="sFakeId">FakeId</param>
        /// <param name="sImageCode">验证码</param>
        /// <returns></returns>
        public static string SendTuWen(string sFid, string sFakeId, string sImageCode = "")
        {
            string sToken = LoginInfo.Token;
            string sUrl = "https://mp.weixin.qq.com/cgi-bin/singlesend";
            Dictionary<string, string> postData = new Dictionary<string, string>();
            Dictionary<string, string> header = new Dictionary<string, string>();
            postData.Add("type", "10");
            postData.Add("app_id", sFid);
            postData.Add("tofakeid", sFakeId);
            postData.Add("appmsgid", sFid);
            postData.Add("imgcode", sImageCode);
            postData.Add("token", sToken);
            postData.Add("lang", "zh_CN");
            postData.Add("random", new Random().NextDouble().ToString());
            postData.Add("t", "ajax-response");
            header.Add("Referer", string.Format("https://mp.weixin.qq.com/cgi-bin/singlesendpage?t=message/send&action=index&tofakeid={0}&token={1}&lang=zh_CN", sFakeId, LoginInfo.Token));
            string context = HttpHelper.GetTextByPost(sUrl, postData, header, Encoding.UTF8);
            return context;
        }
    }
}