using DAL.Business;
using MyOrm.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class WeChat
    {
        public static string MP_PAY_API_KEY = "&key=VXjihRCAV3e5UWnkWW5MdUWjiDjK5aVK";

        public static string MP_ORI_ID = ConfigurationManager.AppSettings["MP_ORI_ID"].ToString().Trim();
        public static string MP_APPID = ConfigurationManager.AppSettings["MP_APPID"].ToString().Trim();
        public static string MP_APPSECRET = ConfigurationManager.AppSettings["MP_APPSECRET"].ToString().Trim();
        public static string MP_PAY_MCH_ID = ConfigurationManager.AppSettings["MP_PAY_MCH_ID"].ToString().Trim();

        /// <summary>
        /// 得到code后,用code获取access_token和refresh_token
        /// </summary>
        public static string URL_OAUTH2_ACCESS_TOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + MP_APPID + "&secret=" + MP_APPSECRET + "&code={0}&grant_type=authorization_code";

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public static string URL_APP_USERINFO = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        public static string KEY_ACCESS_TOKEN = "Access_Token";
        public static string KEY_JSAPI_TICKET = "jsapi_ticket";

        /// <summary>
        /// 设置AccessToken的缓存
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            string CacheKey = "DataCache-" + KEY_ACCESS_TOKEN;
            object objModel = Cache.GetFromCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    DAL.AccessToken token = ServiceFactory.Factory.AccessTokenService.SearchOne(new SimpleCondition("ParamType", KEY_ACCESS_TOKEN));
                    if (token != null)
                    {
                        objModel = token.Access_Token.ToString();
                        Cache.AddToCache(CacheKey, objModel, DateTime.Now.AddMinutes(4), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            return objModel.ToString();
        }

        /// <summary>
        /// 设置jsapi_tickt的缓存
        /// </summary>
        /// <returns></returns>
        public static string GetJsApiTicket()
        {
            string CacheKey = "DataCache-" + KEY_JSAPI_TICKET;
            object objModel = Cache.GetFromCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    DAL.AccessToken token = ServiceFactory.Factory.AccessTokenService.SearchOne(new SimpleCondition("ParamType", KEY_JSAPI_TICKET));
                    if (token != null)
                    {
                        objModel = token.Access_Token.ToString();
                        Cache.AddToCache(CacheKey, objModel, DateTime.Now.AddMinutes(4), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            return objModel.ToString();
        }
    }
}
