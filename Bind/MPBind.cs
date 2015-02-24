using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using Weixin_Server.Bind.Helper;

namespace Weixin_Server.Bind
{
    public class MPBind
    {
        
        /// <summary>
        /// FakeIdBind
        /// </summary>
        /// <param name="sBreakMsg">关键字</param>
        /// <param name="sMPUser">MP帐号</param>
        /// <param name="sMPPass">MP密码</param>
        /// <param name="sCookies">MPCookies</param>
        /// <returns>返回FakeId</returns>
        public static string[] BindFakeId(string sBreakMsg,string sMPUser,string sMPPass)
        {
            if (!WeiXinLogin.ExecLogin(sMPUser, sMPPass))
            {
                return new string[]{"客服暂时繁忙，无法为您解决问题。\nERROR CODE:ERROR_MPSERVER_LOGIN_FAILURE"};
            }
            else
            {
                string[] aFakeIdInfo = GetRecentMessageListFakeId(sBreakMsg);
                if (aFakeIdInfo == null)
                {
                    return new string[] { "ERROR_NO_FAND_FAKEID" };
                }
                return aFakeIdInfo;
            }
        }

        /// <summary>
        /// 寻找BreakMsg所在的FakeId，实现全自动绑定
        /// </summary>
        /// <param name="sBreakMsg">关键字值</param>
        /// <returns>返回FakeId</returns>
        private static string[] GetRecentMessageListFakeId(string sBreakMsg)
        {
            //获得最近的消息列表及用户FakeId
            List<MessageListItems> LsMlitem = new List<MessageListItems>();
            string sMsgListUrl = string.Format("https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count=20&day=0&token={0}&f=json",LoginInfo.Token);
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Referer", "https://mp.weixin.qq.com/cgi-bin/indexpage?t=wxm-index&lang=zh_CN&token=" + LoginInfo.Token);
            string sMsgListJson = HttpHelper.HttpGetString(sMsgListUrl, header, Encoding.UTF8);
            if (sMsgListJson.Contains("\"err_msg\":\"ok\""))
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(sMsgListJson);
                jo = (JObject)JsonConvert.DeserializeObject(jo["msg_items"].ToString());
                LsMlitem = JsonConvert.DeserializeObject<List<MessageListItems>>(jo["msg_item"].ToString());
            }
            else
            {
                //获取失败则直接返回空值
                return null;
            }
            if (LsMlitem == null || LsMlitem.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (var ls in LsMlitem)
                {
                    if (IsBreakMessage(sBreakMsg, ls.fakeid))
                    {
                        return new string[]{ ls.fakeid,ls.nick_name};
                    }
                }
            }
            return null;
        }

        private static bool IsBreakMessage(string sBreakMsg, string sFakeId)
        {
            //获取FakeId下的消息内容列表
            string sFakeIdContentUrl = string.Format("https://mp.weixin.qq.com/cgi-bin/singlesendpage?tofakeid={0}&t=message/send&action=index&token={1}&lang=zh_CN&f=json",sFakeId,LoginInfo.Token);
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Referer", "https://mp.weixin.qq.com/cgi-bin/indexpage?t=wxm-index&lang=zh_CN&token=" + LoginInfo.Token);
            string sMsgListJson = HttpHelper.HttpGetString(sFakeIdContentUrl, header, Encoding.UTF8);
            return sMsgListJson.Contains(sBreakMsg);
        }

    }
}