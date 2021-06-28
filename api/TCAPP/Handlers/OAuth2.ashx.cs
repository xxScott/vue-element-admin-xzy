using DAL.Business;
using MyOrm.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities;

namespace HrApp.Handlers
{
    /// <summary>
    /// OAuth2 的摘要说明
    /// </summary>
    public class OAuth2 : IHttpHandler
    {
        Log log = new Log();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string code = context.Request.Params["code"].ToString().Trim();
            string page = context.Request.Params["page"].ToString().Trim();
            string result = new HttpUtil().HttpGet(string.Format(WeChat.URL_OAUTH2_ACCESS_TOKEN, code));
            log.Info(result);

            JObject jsonObject = JObject.Parse(result);
            //返回值
            string openid = jsonObject["openid"].ToString();
            string access_token = jsonObject["access_token"].ToString();
            string refresh_token = jsonObject["refresh_token"].ToString();
            context.Session["openid"] = openid;

            string req_data = new HttpUtil().HttpGet(string.Format(WeChat.URL_APP_USERINFO, WeChat.GetAccessToken(), openid));
            log.Info("req_data:" + req_data);

            JObject _jsonObject = JObject.Parse(req_data);
            string nickname = _jsonObject["nickname"].ToString();
            string sex = _jsonObject["sex"].ToString();
            string province = _jsonObject["province"].ToString();
            string city = _jsonObject["city"].ToString();
            string headimgurl = _jsonObject["headimgurl"].ToString();

            DAL.Account acct = ServiceFactory.Factory.AccountService.SearchOne(new SimpleCondition("OpenId", openid));
            if (acct == null)
            {
                DAL.Account account = new DAL.Account();
                account.OpenId = openid;
                account.ProvinceName = province;
                account.CityName = city;
                account.NickName = nickname;
                account.Sex = Convert.ToInt16(sex);
                account.HeadImgUrl = headimgurl;
                account.Enabled = true;
                account.CreateTime = DateTime.Now;
                account.Creator = "oauth2";
                account.CreateIP = Network.ClientIP;
                account.UpdateTime = DateTime.Now;
                account.Updater = "oauth2";
                account.UpdateIP = Network.ClientIP;
                ServiceFactory.Factory.AccountService.Insert(account);
            }

            //存在cookie里,方便sell.htm各页面取到,经测试,可以取到
            HttpCookie cookie = new HttpCookie("openid", openid);
            context.Response.Cookies.Add(cookie);

            string url = page + "?ver=" + new Random().Next();
            log.Info(url);
            context.Response.Redirect(url, false);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}