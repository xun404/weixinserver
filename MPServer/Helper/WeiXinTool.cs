using System;
using System.Data;
using System.Text;

namespace Weixin_Server.MPServer.Helper
{
    public class WeiXinTool
    {
        /// <summary>
        /// OpenId转FakeId
        /// </summary>
        /// <param name="sOpenId">需要查询的OpenId</param>
        /// <returns>返回FakeId，无结果则返回0</returns>
        public static string OpenIdToFakeId(string sOpenId)
        {
            string sSql = string.Format("SELECT fakeid FROM `mpserver_bridge` WHERE openid = '{0}'", sOpenId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["fakeid"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// MPKey转OriginaId
        /// </summary>
        /// <param name="sMPKey">需查询的MPKey值</param>
        /// <returns>返回MPKey对应的OriginaId值</returns>
        public static string MPKeyToOriginaId(string sMPKey)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_mpweixin_login` WHERE MPKey = '{0}'", sMPKey);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                return string.Empty;
            }
            else
            {
                return dt.Rows[0]["OriginalId"].ToString();
            }
        }

        public static bool OriginalIdIsRegUser(string sOriginalId)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_mpweixin_login` WHERE OriginalId = '{0}'", sOriginalId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 获得公众帐号密码
        /// </summary>
        /// <param name="OriginalId">原始帐号</param>
        /// <returns>返回字符串组{MPUser,MPPass}</returns>
        public static string[] GetMPUserInfo(string OriginalId)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_mpweixin_login` WHERE OriginalId = '{0}'", OriginalId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return new string[] { dt.Rows[0]["MPUser"].ToString(), dt.Rows[0]["MPPass"].ToString() };
            }
        }

        /// <summary>
        /// 获取微信客服FakeId
        /// </summary>
        /// <param name="OriginalId">原始帐号Id</param>
        /// <returns>返回ServerId字符串组</returns>
        public static string[] GetMPServerId(string OriginalId)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_mpweixin_login` WHERE OriginalId = '{0}'", OriginalId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0]["ServerId"].ToString().Split('#');
            }
        }

        public static string OpenIdToNickName(string sOpenId)
        {
            string sSql = string.Format("SELECT name FROM `mpserver_bridge` WHERE openid = '{0}'", sOpenId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
            }
            else
            {
                return "路人甲";
            }
        }

        public static bool OpenIdToFans(string sOpenId)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_bridge` WHERE openid = '{0}'", sOpenId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count > 0)
            {
                WeiXinFans.sFakeId = dt.Rows[0]["FakeId"].ToString();
                WeiXinFans.sFansId = dt.Rows[0]["Id"].ToString();
                WeiXinFans.sBaseNikeName = dt.Rows[0]["Name"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string FansIdToFakeId(string sFansId)
        {
            string sSql = string.Format("SELECT * FROM `mpserver_bridge` WHERE id = '{0}'", sFansId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["fakeid"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string Base64Code(string Message)
        {
            byte[] bytes = Encoding.Default.GetBytes(Message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string Base64Decode(string Message)
        {
            byte[] bytes = Convert.FromBase64String(Message);
            return Encoding.Default.GetString(bytes);
        }
    }
}