using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Weixin_Server.MPServer.Helper
{
    public class DataEncryptionAlgorithm
    {      
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="sEncryptString">待加密的字符串</param>
        /// <returns>
        /// 返回 DES加密后的字符串
        /// </returns>
        public static string EncryptDES(string sEncryptString)
        {
            return sEncryptString;
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="sDecryptString">待解密的字符串</param>
        /// <returns>
        /// 返回 DES解密后的字符串
        /// </returns>
        public static string DecryptDES(string sDecryptString)
        {
            return sDecryptString;
        }
    }

    public class DEAEntity
    {
        /// <summary>
        /// 加/解密Key
        /// </summary>
        public static string EncryptKey { get; set; }

        /// <summary>
        /// 初始化Key
        /// </summary>
        public static byte[] DesKeys { get; set; }
    }
}