using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (this.Page.Session["OpenId"] == null || this.Page.Session["OpenId"].ToString() == string.Empty)
        {
            string response_type = "code";//    返回类型，此时固定为：code
            string scope = "snsapi_userinfo";//应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
            string state = "state";//随便写，微信提供给我们的自定义参数。重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect", Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId, "http://wxdemo.smarttips.org.cn/WeixinRedirect.aspx", response_type, scope, state);

            var response = base.Response;
            response.Redirect(url, true);//跳转到微信授权
        }
        else
        {
            this.txtOpenId.Text = this.Page.Session["OpenId"].ToString();
        }

       
    }

   

}