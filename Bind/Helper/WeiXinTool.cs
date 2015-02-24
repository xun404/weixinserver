using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Weixin_Server.Bind.Helper
{
    public class WeiXinTool
    {
        /// <summary>
        /// 获得相应页面的Json
        /// </summary>
        /// <param name="sJsonUrl"></param>
        /// <returns></returns>
        //public static List<MessageListItems> GetCurrentFans(string sJsonUrl)
        //{
        //    Dictionary<string, string> header = new Dictionary<string, string>();
        //    header.Add("Referer", "https://mp.weixin.qq.com/cgi-bin/indexpage?t=wxm-index&lang=zh_CN&token=" + LoginInfo.Token);
        //    string url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&token=" + LoginInfo.Token + "&lang=zh_CN&pagesize=10000&pageidx=0&type=0&groupid=" + groupid;
        //    string context = HttpHelper.HttpGetString(url, header, Encoding.UTF8);
        //    Regex regex = new Regex(@"\{""contacts"":(\[.*?\])\}", RegexOptions.Singleline);
        //    Match match = regex.Match(context);
        //    if (match.Success)
        //    {
        //        return JSON.GetObject<List<WeiXinFans>>(match.Groups[1].Value);
        //    }
        //    return new List<WeiXinFans>();
        //    //\<script id=\"json-friendList\" type="json\/text"\>(.*)<\/script\>
        //    //File.WriteAllText("f:/a.html", context);
        //}
    }
}