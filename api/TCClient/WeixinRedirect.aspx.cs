using Com.Caimomo.Common;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeixinRedirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string code = Request.QueryString["code"];
        string state = Request.QueryString["state"];
        string returnUrl = "http://wxdemo.smarttips.org.cn/default.aspx";

        try
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("您拒绝了授权！");
            }

            //if (!state.Contains("|"))
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
            //    //实际上可以存任何想传递的数据，比如用户ID
            //    throw new Exception("验证失败！请从正规途径进入！1001");
            //}

            //通过，用code换取access_token
            var openIdResult = OAuthApi.GetAccessToken(Config.SenparcWeixinSetting.WeixinAppId, Config.SenparcWeixinSetting.WeixinAppSecret, code);
            if (openIdResult.errcode != ReturnCode.请求成功)
            {
                throw new Exception("错误：" + openIdResult.errmsg);
            }
            TCSecurity.SaveOpenIdToCookie(openIdResult.openid);
            //this.Page.Session.Add("OpenId", openIdResult.openid);

            var response = base.Response;
            response.Redirect(returnUrl, true);

        }
        catch (Exception ex)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = string.Empty;
            }
            if (string.IsNullOrEmpty(state))
            {
                state = string.Empty;
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = string.Empty;
            }
            throw new Exception(ex.Message+ "||code:"+ code+"||state:"+state+ "||returnUrl:"+ returnUrl);
        }
    }
}