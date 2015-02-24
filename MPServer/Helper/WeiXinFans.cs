using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin_Server.MPServer.Helper
{
    public class WeiXinFans
    {
        public static string sFakeId { get; set; }

        public static string sNickName { get { return WeiXinTool.Base64Decode(sBaseNikeName); } }

        public static string sBaseNikeName { get; set; }

        public static string sFansId{ get; set; }
    }
}