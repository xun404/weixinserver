using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Weixin_Server.Bind.Helper
{
    public class LoginInfo
    {

        public static string CurrentCode = string.Empty;
        /// <summary>
        /// 登录后得到的令牌
        /// </summary>        
        public static string Token { get; set; }
        /// <summary>
        /// 登录后得到的cookie
        /// </summary>
        public static CookieContainer LoginCookie { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public static DateTime CreateDate { get; set; }

    }
}