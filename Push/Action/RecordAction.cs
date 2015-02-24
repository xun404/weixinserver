using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Weixin_Server.MPServer.Helper;

namespace Weixin_Server.Push.Action
{
    public class RecordAction
    {
        /// <summary>
        /// 触发记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string AddRec(HttpContext context)
        {
            WeiXinMsgInfo.OpenId = context.Request["openid"];
            WeiXinMsgInfo.OriginalId = context.Request["originalid"];
            WeiXinMsgInfo.ToUser = context.Request["to"];
            WeiXinMsgInfo.Text = context.Request["text"];
            return BindAction.IsBindFakeId();
        }

        /// <summary>
        /// 遍历48小时内发送消息的用户
        /// </summary>
        /// <returns></returns>
        public static string ListRec(HttpContext context)
        {
            string sHtml = "Id    OpenId    DateTime";
            string sOriginalId = context.Request["OriginalId"];
            DataTable dt = ListAction.ShowSendUser(sOriginalId);
            if (dt == null)
            {
                return "暂无数据";
            }
            foreach (DataRow t in dt.Rows)
            {
                sHtml += string.Format("\n<br>{0}    {1}    {2}", t[0], t[1], t[2]); 
            }
            return sHtml;
        }

        internal static string EditRec(HttpContext context)
        {
            if (context.Request.HttpMethod.ToLower() == "post")
            {
                EditAction.AddEdit(context);
                return "<script type=\"text/JavaScript\">alert(\"ADD Success；\");window.location.href='handler.ashx?do=edit&OriginalId=" + context.Request["OriginalId"] + "';</script>";
            }
            else
            { 
                DataTable dt = EditAction.ShowEdit(context);
                if (dt == null)
                {
                    var J1 = new { tw = "", Text = "" };
                    var J2 = new { tw = "", Text = "" };
                    var datas = new { J1, J2 };
                    return CommonHelper.RenderHtml("edit.html",datas);
                }
                else
                { 
                var j1 = new { tw = dt.Rows[0]["type"].ToString()=="1"?"selected=\"selected\"":"", Text = dt.Rows[0]["text"] };
                var j2 = new { tw = dt.Rows[1]["type"].ToString() == "1" ? "selected=\"selected\"" : "", Text = dt.Rows[1]["text"] };
                var data = new { j1, j2 };
                return CommonHelper.RenderHtml("edit.html",data);
            }
            }
        }

        public static string SendRec(HttpContext context)
        {
            WeiXinMsgInfo.OpenId = context.Request["openid"];
            WeiXinMsgInfo.OriginalId = context.Request["originalid"];
            WeiXinMsgInfo.ToUser = context.Request["to"];
            WeiXinMsgInfo.Text = context.Request["text"];
            return SendAction.SendMessage(context);
        }
    }
    public class CommonHelper
    {
        /// <summary>
        /// 模版引擎
        /// </summary>
        /// <param name="name">模版名</param>
        /// <param name="data">数据集合体</param>
        /// <returns></returns>
        public static string RenderHtml(string name, object data = null)
        {
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/push/tpl"));//模板文件所在的文件夹
            vltEngine.Init();
            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("Model", data);
            Template vltTemplate = vltEngine.GetTemplate(name);
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);
            return vltWriter.GetStringBuilder().ToString();
        }
    }
}