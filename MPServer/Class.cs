using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Weixin_Server.MPServer
{
    /// <summary>
    /// 系统配置操作类
    /// </summary>
    public class Config                                                         
    {
        //DBAccess
        public static string DBIP { get { return ConfigurationManager.AppSettings["DB_IP"].ToString(); } }
        public static string DBPort { get { return ConfigurationManager.AppSettings["DB_Port"].ToString(); } }
        public static string DBUser { get { return ConfigurationManager.AppSettings["DB_User"].ToString(); } }
        public static string DBPass { get { return ConfigurationManager.AppSettings["DB_Pass"].ToString(); } }
        public static string DBDatabase { get { return ConfigurationManager.AppSettings["DB_Database"].ToString(); } }
        public static string[] DBConnString { get { return new string[] { DBIP, DBPort, DBUser, DBPass, DBDatabase }; } }
    }
}