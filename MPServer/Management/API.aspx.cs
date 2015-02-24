using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Weixin_Server.MPServer.Management
{
    public partial class API : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sMPKey = Request["key"];
            Tips.Text = "http://" + Request.Url.Authority + "/MPServer/MPServer.aspx?OriginalId=" + sMPKey + "&f=xml";
            if (string.IsNullOrWhiteSpace(sMPKey))
            {
                Response.Redirect("Admin.aspx");
            }
        }
    }
}