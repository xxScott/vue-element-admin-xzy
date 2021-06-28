using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Caimomo.Common
{
    /// <summary>
    /// WeixinInit 的摘要说明
    /// </summary>
    public class WeixinInit
    {
        public WeixinInit()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }

        public static void Init()
        {

            /* CO2NET 全局注册开始
                 * 建议按照以下顺序进行注册
                 */

            //设置全局 Debug 状态
            var isGLobalDebug = true;
            var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);

            //CO2NET 全局注册，必须！！
            IRegisterService register = RegisterService.Start(senparcSetting)
                     .UseSenparcGlobal(false, null);

            /* 微信配置开始
                 * 建议按照以下顺序进行注册
                 */

            //设置微信 Debug 状态
            var isWeixinDebug = true;
            var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);

            //微信全局注册，必须！！
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);
        }
    }
}