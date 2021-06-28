using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;

/// <summary>
///HttpUtil 的摘要说明
/// </summary>
namespace Utilities
{
    public class HttpUtil
    {
        public HttpUtil()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public string HttpPost(string Url, string postDataStr)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();

                byte[] postData = encoding.GetBytes(postDataStr);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();

                return retString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string HttpGet(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.KeepAlive = false;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();

                return retString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
