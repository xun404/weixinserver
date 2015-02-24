using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.Bind.Helper
{
    public class WeiXinLogin
    {
        /// <summary>
        /// 登录微信公众平台
        /// </summary>
        /// <param name="sName">公众平台帐号</param>
        /// <param name="sPass">公众平台密码</param>
        /// <returns>登录结果</returns>
        public static bool ExecLogin(string sName, string sPass)
        {
            if (ReadCookies(WeiXinMsgInfo.OriginalId))
            {
                return true;
            }
            bool result = false;
            string sPassWord = GetMd5Str32(sPass).ToUpper();
            string sPostData = "username=" + sName + "&pwd=" + sPassWord + "&imgcode=&f=json";
            string sUrl = "http://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";
            try
            {
                CookieContainer ccLoginCookies = new CookieContainer();
                byte[] byteArray = Encoding.UTF8.GetBytes(sPostData); 
                HttpWebRequest hwrLogin = (HttpWebRequest)WebRequest.Create(sUrl);
                hwrLogin.Referer = "http://mp.weixin.qq.com/cgi-bin/loginpage?lang=zh_CN&t=wxm2-login";
                hwrLogin.CookieContainer = ccLoginCookies;
                hwrLogin.Method = "POST";
                hwrLogin.ContentType = "application/x-www-form-urlencoded"; 
                hwrLogin.ContentLength = byteArray.Length;
                Stream newStream = hwrLogin.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                HttpWebResponse response2 = (HttpWebResponse)hwrLogin.GetResponse();
                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);
                string sText = sr2.ReadToEnd();
                WeiXinRetInfo retinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiXinRetInfo>(sText);
                string sToken = string.Empty;
                if (retinfo.redirect_url.Length > 0)
                {
                    sToken = retinfo.redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();
                    LoginInfo.LoginCookie = ccLoginCookies;
                    LoginInfo.CreateDate = DateTime.Now;
                    LoginInfo.Token = sToken;
                    WriteCookies();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// MD5-32加密
        /// </summary>
        /// <param name="sPass">明文密码</param>
        /// <returns>MD5加密的密码</returns>
        private static string GetMd5Str32(string sPass)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider(); 
            char[] temp = sPass.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 校验Cookies是否有效
        /// </summary>
        /// <returns>有效返回True，否则返回False</returns>
        private static bool CheckLoginCookies()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Referer", "https://mp.weixin.qq.com/cgi-bin/indexpage?t=wxm-index&lang=zh_CN&token=" + LoginInfo.Token);
            string url = "https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=" + LoginInfo.Token + "&f=json";
            string context = HttpHelper.HttpGetString(url, header, Encoding.UTF8);
            return !context.Contains("\"err_msg\":\"invalid session\"");
        }

        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="sOriginalId">原始ID</param>
        /// <returns></returns>
        private static bool ReadCookies(string sOriginalId)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_cookies` WHERE OriginalId = '{0}'", sOriginalId);
            DataTable dt = MPServer.Helper.CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                return false;
            }
            string sCookies = dt.Rows[0]["Cookies"].ToString();
            LoginInfo.Token = dt.Rows[0]["Token"].ToString();
            LoginInfo.CreateDate = (DateTime)dt.Rows[0]["Time"];
            string[] aCookies = sCookies.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            CookieContainer cc = new CookieContainer();
            foreach (string c in aCookies)
            {
                string[] ccc = c.Split(";".ToCharArray());
                Cookie ck = new Cookie(ccc[1], ccc[5], ccc[2], ccc[0]);
                cc.Add(ck);
            }
            LoginInfo.LoginCookie = cc;
            //校验Cookies是否有效
            return CheckLoginCookies();
        }

        /// <summary>
        /// 写入Cookies
        /// </summary>
        private static void WriteCookies()
        {
            //删除同OriginalId的记录
            string sSql = string.Format("DELETE FROM `mpserver_cookies` WHERE OriginalId = '{0}'", WeiXinMsgInfo.OriginalId);
            CDBAccess.MySqlDt(sSql);
            //插入新OriginalId记录
            string sCookies = GetAllCookies(LoginInfo.LoginCookie);
            sSql = string.Format("INSERT INTO `mpserver_cookies` (`Cookies`, `Token`, `OriginalId`, `Time`) VALUES ('{0}', '{1}', '{2}', '{3}')",
                sCookies,LoginInfo.Token,WeiXinMsgInfo.OriginalId,LoginInfo.CreateDate);
            CDBAccess.MySqlDt(sSql);
        }

        /// <summary>
        /// Cookies序列化
        /// </summary>
        /// <param name="cc">CookieContainer</param>
        /// <returns>Cookies字符串序列</returns>
        private static string GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            //return lstCookies;
            StringBuilder sbc = new StringBuilder();
            foreach (Cookie cookie in lstCookies)
            {
                sbc.AppendFormat("{0};{1};{2};{3};{4};{5}\r\n",
                cookie.Domain, cookie.Name, cookie.Path, cookie.Port,
                cookie.Secure.ToString(), cookie.Value);
            }
            return sbc.ToString();
        }
    }
}