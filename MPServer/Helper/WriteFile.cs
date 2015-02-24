using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Weixin_Server.MPServer.Helper
{
    public class WriteFile
    {
        public static void Write(string sFile,string sText)
        {
            if (File.Exists(sFile))
            {
                File.Delete(sFile);
            }
            FileStream fs = File.Create(sFile);
            Byte[] bContent = System.Text.Encoding.UTF8.GetBytes(sText);
            fs.Write(bContent, 0, bContent.Length);
            fs.Close();
            fs.Dispose();
        }
    }
}