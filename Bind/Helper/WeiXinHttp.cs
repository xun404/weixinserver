using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Weixin_Server.Bind.Helper
{
    public partial class HttpHelper
    {
        /// <summary>
        /// 模拟登录默认UserAgent
        /// </summary>
        private static string DefaultUserAgent
        {
            get
            {
                return "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.107 Safari/537.36";
            }
        }

        /// <summary>
        /// Http Request Post Data
        /// </summary>
        /// <param name="url">目标Url</param>
        /// <param name="postData">Post数据</param>
        /// <param name="headers">请求体头</param>
        /// <param name="postencoding">请求编码</param>
        /// <returns>返回Http响应</returns>
        public static HttpWebResponse HttpWebRequestPostData(string url, Dictionary<string, string> postData, Dictionary<string, string> headers, Encoding postencoding)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url不能为空，参数:url");
            }
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
            }
            Uri uri = new Uri(url);
            request.UserAgent = DefaultUserAgent;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Host = uri.Host;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (item.Key.Equals("Referer"))
                    {
                        request.Referer = item.Value;
                    }
                    else if (item.Key.Equals("User-Agent", StringComparison.InvariantCultureIgnoreCase))
                    {
                        request.UserAgent = item.Value;
                    }
                    else
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
            }
            request.CookieContainer = LoginInfo.LoginCookie;
            if (postData != null && postData.Count > 0)
            {
                byte[] buffer = postencoding.GetBytes(string.Join("&", postData.Select(item => string.Format("{0}={1}", item.Key, item.Value))));
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response;

        }

        /// <summary>
        /// 得到Http响应 
        /// </summary>
        /// <param name="url">目标Url</param>
        /// <param name="postData">提交数据体</param>
        /// <param name="headers">请求体头</param>
        /// <param name="postencoding">请求编码</param>
        /// <returns>返回Http响应</returns>
        public static HttpWebResponse GetHttpWebResponse(string url, Dictionary<string, string> postData, Dictionary<string, string> headers, Encoding postencoding)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url不能为空，参数:url");
            }
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
            }
            string hostname = new Uri(url).Host;

            request.UserAgent = DefaultUserAgent;

            request.Method = "GET";
            request.Host = hostname;

            request.CookieContainer = LoginInfo.LoginCookie;
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    //if (headers["User-Agent"] != null)

                    if (item.Key.Equals("Referer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        request.Referer = item.Value;
                    }
                    else if (item.Key.Equals("User-Agent", StringComparison.InvariantCultureIgnoreCase))
                    {
                        request.UserAgent = item.Value;
                    }
                    else
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
            }


            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //CookieManager.SaveCookieToFile(response);
            return response;
        }

        /// <summary>
        /// http上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <param name="files"></param>
        /// <param name="postencoding"></param>
        /// <returns></returns>
        public static HttpWebResponse PostMulitFormData(string url, Dictionary<string, string> postData, Dictionary<string, string> headers, Dictionary<string, string> files, Encoding postencoding)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url不能为空，参数:url");
            }
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = HttpWebRequest.Create(url) as HttpWebRequest;
            }
            Uri uri = new Uri(url);
            request.UserAgent = DefaultUserAgent;
            string boundaryString = "-----------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes(string.Format("{0}--{1}{0}", Environment.NewLine, boundaryString));
            request.Method = "POST";
            request.ContentType = "multipart/form-data;boundary=" + boundaryString;
            request.Host = uri.Host;

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (item.Key.Equals("Referer"))
                    {
                        request.Referer = item.Value;
                    }
                    else if (item.Key.Equals("User-Agent", StringComparison.InvariantCultureIgnoreCase))
                    {
                        request.UserAgent = item.Value;
                    }
                    else
                    {
                        request.Headers[item.Key] = item.Value;
                    }
                }
            }

            request.CookieContainer = LoginInfo.LoginCookie;
            using (Stream stream = request.GetRequestStream())
            {
                if (postData != null && postData.Count > 0)
                {
                    foreach (var item in postData) //posfile
                    {
                        //bondary
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string postStr = string.Format("Content-Disposition: form-data; name=\"{1}\"{0}{0}{2}", Environment.NewLine, item.Key, item.Value);
                        byte[] dataBytes = postencoding.GetBytes(postStr);
                        stream.Write(dataBytes, 0, dataBytes.Length);
                    }
                }
                if (files != null && files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string postHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", item.Value, Path.GetFileName(item.Key), "application/x-msdownload");
                        byte[] headerBytes = postencoding.GetBytes(postHeader);
                        stream.Write(headerBytes, 0, headerBytes.Length);
                        using (FileStream fileStream = File.OpenRead(item.Key))
                        {
                            byte[] buffer = new byte[1024];
                            int readeLength = 0;
                            while ((readeLength = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                stream.Write(buffer, 0, readeLength);
                            }
                        }
                    }
                }
                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundaryString + "--\r\n");
                stream.Write(trailer, 0, trailer.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //CookieManager.SaveCookieToFile(response);
            return response;
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        public static HttpWebResponse HttpGetEx(string url, Encoding encoding)
        {
            return HttpHelper.GetHttpWebResponse(url, null, null, encoding);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="header">附加头</param>
        public static void DownloadFile(string url, string savePath, Dictionary<string, string> header)
        {
            using (HttpWebResponse response = HttpHelper.GetHttpWebResponse(url, null, header, Encoding.UTF8))
            {
                using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {


                    using (Stream stream = response.GetResponseStream())
                    {
                        int length = 0;
                        do
                        {
                            byte[] bytes = new byte[1024];
                            length = stream.Read(bytes, 0, bytes.Length);
                            if (length > 0)
                            {
                                fs.Write(bytes, 0, length);
                            }
                            else
                            {
                                break;
                            }
                        } while (true);

                    }
                }
            }
        }

        public static HttpWebResponse PostData(string url, Dictionary<string, string> postData, Encoding encoding)
        {
            return HttpHelper.HttpWebRequestPostData(url, postData, null, encoding);
        }

        public static string HttpGetString(string url, Dictionary<string, string> header, Encoding encoding)
        {
            using (HttpWebResponse respose = HttpHelper.GetHttpWebResponse(url, null, header, encoding))
            {
                using (StreamReader reader = new StreamReader(respose.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string GetTextByPost(string url)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("t", "123");
            return GetTextByPost(url, dic, Encoding.UTF8);
        }

        public static string GetTextByPost(string url, Dictionary<string, string> postData, Encoding encoding)
        {
            using (HttpWebResponse response = HttpHelper.PostData(url, postData, encoding))
            {

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static string GetTextByPost(string url, Dictionary<string, string> postData, Dictionary<string, string> header, Encoding encoding)
        {
            using (HttpWebResponse response = HttpHelper.HttpWebRequestPostData(url, postData, header, encoding))
            {

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}