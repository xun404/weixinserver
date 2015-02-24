using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Weixin_Server.MPServer.Helper;
using Weixin_Server.Push.Action;

namespace Weixin_Server.Push
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sDo = context.Request["do"];
            string sOriginalId = context.Request["originalid"];
            string sHostUrl = context.Request.Url.Host;
            if (!Registration.IsRegistration(sHostUrl))
            {
                context.Response.Write("系统API尚未注册，请登录http://" + sHostUrl + "/MPServer/Register.aspx完成注册！");
            }
            if (!ChecksOriginalId(sOriginalId))
            {
                sDo = "exit";
            }
            string sHtml = string.Empty;
            switch (sDo)
            { 
                case "rec":
                    sHtml = RecordAction.AddRec(context);
                    break;
                case "edit":
                    sHtml = RecordAction.EditRec(context);
                    break;
                case "send":
                    sHtml = RecordAction.SendRec(context);
                    break;
                case "list":
                    sHtml = RecordAction.ListRec(context);
                    break;
                case "exit":
                    sHtml = "0";
                    break;
                default:
                    break;
            }
            context.Response.Write(sHtml);
            context.Response.Flush();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public bool ChecksOriginalId(string sOriginalId)
        {
            string sSql = string.Format("SELECT COUNT(1) FROM `mpserver_mpweixin_login` WHERE `OriginalId` = '{0}'", sOriginalId);
            DataTable dt = CDBAccess.MySqlDt(sSql);
            if (dt.Rows.Count < 0)
            {
                return false;
            }
            else
            {
                return Convert.ToInt16(dt.Rows[0][0]) > 0;
            }
        }
    }
}