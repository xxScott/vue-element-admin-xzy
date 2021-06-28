
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Configuration;
using System.Threading;
using DAL;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace Utilities
{
    public class SMS
    {
        Log log = new Log("ChuangLanService");

        string PostUrl = ConfigurationManager.AppSettings["WebReference.Service.PostUrl"];
        string account = "N7843581";
        string password = "jl20D1fCr";

        public bool SendByChuangLan(string mobile, string content, ref string message)
        {
            try
            {
                content = "【253云通讯】" + HttpContext.Current.Server.UrlEncode(content);

                string postJsonTpl = "\"account\":\"{0}\",\"password\":\"{1}\",\"phone\":\"{2}\",\"report\":\"false\",\"msg\":\"{3}\"";
                string jsonBody = "{" + string.Format(postJsonTpl, account, password, mobile, content) + "}";


                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                // Create NetworkCredential Object 
                NetworkCredential admin_auth = new NetworkCredential("username", "password");

                // Set your HTTP credentials in your request header
                httpWebRequest.Credentials = admin_auth;

                // callback for handling server certificates
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonBody);
                    streamWriter.Flush();
                    streamWriter.Close();
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        if (obj["code"].ToString() != "0")
                        {
                            message = obj["errorMsg"].ToString();
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("短信发送失败 " + mobile + "," + content);
                log.Error(ex);
                throw ex;
            }
        }
    }
}
