using System;
using System.Collections.Generic;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Com.Caimomo.Common
{
    /// <summary>
    /// SmsTool 的摘要说明
    /// </summary>
    static class SmsTool
    {
        const string AccessKeyId = "LTAIOmEmkLy8fCrT";
        const string AccessSecret = "qvwn0hP9Xc0GIWXQsQSELN7AfhKKnX";
        const string SignName = "思特IT学院";
        const string TemplateCode = "SMS_146804705";

        private class smsResponse
        {
            public string Message { get; set; }
            public string RequestId { get; set; }
            public string BizId { get; set; }
            public string Code { get; set; }
        }

        public static bool SendMsg(string strPhoneNumbers,string strCode)
        {

            IClientProfile profile = DefaultProfile.GetProfile("default", AccessKeyId, AccessSecret);



            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            

            request.AddQueryParameters("PhoneNumbers", strPhoneNumbers);
            request.AddQueryParameters("SignName", SignName);
            request.AddQueryParameters("TemplateCode", TemplateCode);
            request.AddQueryParameters("TemplateParam", "{code:"+ strCode + "}");
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                //Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                smsResponse smsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<smsResponse>(response.Data);
                if (smsResponse.Message == "OK") return true;
                else return false;
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
                return false;
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
    }
}