using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using log4net;

namespace Utilities
{
    public class Network
    {
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string ClientIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (null == result || result == String.Empty)
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (null == result || result == String.Empty)
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                return result;
            }
        }

        #region   GetWebRoot

        /// <summary>
        /// 获取网站根目录（网站域名）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetWebRoot(HttpContext context)
        {
            string url = context.Request.Url.ToString().IndexOf("?") == -1 ? context.Request.Url.ToString() : context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().Trim().IndexOf("?"));
            string rawUrl = context.Request.RawUrl.ToString().IndexOf("?") == -1 ? context.Request.RawUrl.ToString() : context.Request.RawUrl.ToString().Substring(0, context.Request.RawUrl.ToString().Trim().IndexOf("?"));
            string webRoot = url.Replace(rawUrl, string.Empty);
            webRoot = webRoot.EndsWith("/") ? webRoot.Remove(webRoot.Length - 1, 1) : webRoot;
            return webRoot;
        }

        /// <summary>
        /// 获取网站根目录（网站域名）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetWebRoot(Page page)
        {
            string url = page.Request.Url.ToString().IndexOf("?") == -1 ? page.Request.Url.ToString() : page.Request.Url.ToString().Substring(0, page.Request.Url.ToString().Trim().IndexOf("?"));
            string rawUrl = page.Request.RawUrl.ToString().IndexOf("?") == -1 ? page.Request.RawUrl.ToString() : page.Request.RawUrl.ToString().Substring(0, page.Request.RawUrl.ToString().Trim().IndexOf("?"));
            string webRoot = url.Replace(rawUrl, string.Empty);
            webRoot = webRoot.EndsWith("/") ? webRoot.Remove(webRoot.Length - 1, 1) : webRoot;
            return webRoot;
        }

        #endregion

        public readonly static ILog logger = LogManager.GetLogger("ServiceInterceptor");

        public string Get(string url,  string key, string value, ILog logger)
        {
            return Get(url,  new string[] { key }, new string[] { value }, logger);
        }

        public string Get(string url,string[] key, string[] value, ILog logger)
        {
            try
            {
                for (int i = 0; i < key.Length; i++)
                {
                    url = url.IndexOf("?") == -1 ? url + "?{key}={value}" : url + "&{key}={value}";
                    url = url.Replace("{key}", key[i]);
                    url = url.Replace("{value}", value[i]);
                }
                logger.Info("Begin To Request WebApi By Get,URL:" + url);

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] myData = MyWebClient.DownloadData(url);
                string data = Encoding.UTF8.GetString(myData);

                logger.Info("Request WebApi By Get Successfully,URL:" + url + ",out data:" + data);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get(string url, string data)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();

                byte[] postData = encoding.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();

                Stream newStream = request.GetRequestStream();
                newStream.Write(postData, 0, postData.Length);
                newStream.Flush();
                newStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.KeepAlive = false;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string data = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string Post(string url, string value)
        {
            try
            {
                logger.Info("Begin To Request WebApi By Post,URL:" + url);

                byte[] bs_indata = System.Text.Encoding.UTF8.GetBytes(value);
                System.Net.HttpWebRequest myRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/json";
                myRequest.ContentLength = bs_indata.Length;

                System.IO.Stream requestStream = myRequest.GetRequestStream();
                requestStream.Write(bs_indata, 0, bs_indata.Length);
                requestStream.Close();

                System.Net.HttpWebResponse myResponse = (System.Net.HttpWebResponse)myRequest.GetResponse();
                System.IO.StreamReader reader = new System.IO.StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string data = reader.ReadToEnd();

                logger.Info("Request WebApi By Post Successfully,URL:" + url + ",in data:" + value + ",out data:" + data);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RedirectAndPOST(Page page, string destinationUrl,
                                    NameValueCollection data)
        {
            string strForm = PreparePOSTForm(destinationUrl, data);
            page.Controls.Add(new LiteralControl(strForm));
        }


        private String PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key +
                               "\" value=\"" + data[key] + "\">");
            }

            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

    }
}
