using Net.Rmbz.Core.MySqlDBAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Weixin_Server.MPServer.Helper
{
    public class CDBAccess
    {
        /// <summary>
        /// MySQLAccess
        /// </summary>
        /// <param name="sSql">SQL语句</param>
        /// <returns>返回DataTables查询数据</returns>
        public static DataTable MySqlDt(string sSql)
        {
            return DBAccess.SqlExceDataTable(Config.DBConnString, sSql);
        }
    }
}