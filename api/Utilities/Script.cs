using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Utilities
{
    public class Script
    {
        ///// <summary>
        ///// 弹出提示框，然后跳转到制定的Url（无UpdatePanel）
        ///// </summary>
        ///// <param name="page">Page对象</param>
        ///// <param name="url">跳转的Url</param>
        ///// <param name="content">提示内容</param>
        //public static void Alert(Page page, string url, string content)
        //{
        //    page.ClientScript.RegisterClientScriptBlock(page.GetType(),"script", "<script>window.alert('" + content + "');location.href='" + url + "';</script>");
        //}

        ///// <summary>
        ///// 弹出提示框（无UpdatePanel）
        ///// </summary>
        ///// <param name="page">Page对象</param>
        ///// <param name="content">提示内容</param>
        //public static void Alert(Page page, string content)
        //{
        //    page.ClientScript.RegisterClientScriptBlock(page.GetType(), "script", "<script>window.alert('" + content + "');</script>");
        //}

        /// <summary>
        /// 弹出提示框（在UpdatePanel中）
        /// </summary>
        /// <param name="control">Page对象</param>
        /// <param name="content">提示内容</param>
        public static void Alert(Control control, string content)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "", "alert('" + content + "');", true);
        }

        /// <summary>
        /// 弹出提示框，然后跳转到制定的Url（在UpdatePanel中）
        /// </summary>
        /// <param name="control">Page对象</param>
        /// <param name="url">跳转的Url</param>
        /// <param name="content">提示内容</param>
        public static void Alert(Control control, string url, string content)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "", "alert('" + content + "');location.href='" + url + "';", true);
        }

        /// <summary>
        /// 弹出提示框，然后跳转到制定的Url（在UpdatePanel中）
        /// </summary>
        /// <param name="control">Page对象</param>
        /// <param name="script">Script脚本</param>
        public static void Execute(Control control,string script)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "", script, true);
        }
    }
}
