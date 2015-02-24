using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin_Server.MPServer.Helper
{
    public static class WeiXinMsgInfo
    {
        public static string OpenId { get; set; }

        public static string ToUser { get; set; }

        public static string FakeId { get; set; }

        public static string Text { get; set; }

        public static string Do { get; set; }

        public static string OriginalId { get; set; }
    }
}