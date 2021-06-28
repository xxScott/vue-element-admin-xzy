using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Pages
    {
        /// <summary>
        /// 错误页面
        /// </summary>
        public static string URL_ERROR = "/Pages/Error.aspx?errorCode={#errorCode#}";

        /// <summary>
        /// 首次登录修改密码
        /// </summary>
        public static string URL_CHANGEPWD = "/Pages/Global/Pwd/ChangePwd.aspx";
    }
}
