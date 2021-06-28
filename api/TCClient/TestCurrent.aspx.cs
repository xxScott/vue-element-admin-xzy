using Com.Caimomo.Common;
using Com.Caimomo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestCurrent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnBindCookies_Click(object sender, EventArgs e)
    {
        TCSecurity.SaveOpenIdToCookie("1234");
    }

    protected void btnGetCurrentInfo_Click(object sender, EventArgs e)
    {
        //当前登录人
        TC_AccountManagement current = TCSecurity.CurrentUser;

        //当前登录人是否绑定过手机
        bool isBind = TCSecurity.IsCurrentUserBindMoble;
    }
}