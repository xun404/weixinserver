using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Weixin_Server.MPServer.Helper
{
    public class Registration
    {
        public static bool IsRegistration(string sUrlHost)
        {
            //注册校验代码，涉及加密算法问题，已删除
            return true;
        }

        public static void DelRegistration()
        {
            string sSql = "DELETE FROM `mpserver_registration`";
            CDBAccess.MySqlDt(sSql);
        }
    }
}