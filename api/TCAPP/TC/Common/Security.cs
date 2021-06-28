using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Caimomo.Entity;
using System.Web.Script.Serialization;

namespace Com.Caimomo.Common
{
    /// <summary>
    /// Security 的摘要说明
    /// </summary>
    public class Security
    {
        public static readonly string CURRENTUSERNAME = "EasyTimeStudio_CurrentUser";
        public static bool IsLogin
        {
            get
            {
                return !object.Equals(CurrentUser, null);
            }
        }

        static bool ConvertBool(object boolVal)
        {
            if (boolVal is bool)
                return (bool)boolVal;
            else if (object.Equals(boolVal, null))
                return false;
            else
            {
                string bval = boolVal.ToString().ToLower();
                if ("true".Equals(bval) || "1".Equals(bval))
                    return true;
                else
                    return false;
            }
        }

        public static SysGroupUser CurrentUser
        {
            get
            {
                string userString = null;
                if (!object.Equals(HttpContext.Current, null) && HttpContext.Current.Request.Cookies.AllKeys.Contains(CURRENTUSERNAME))
                {
                    userString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[CURRENTUSERNAME].Value);
                }
                if (string.IsNullOrEmpty(userString))
                    return null;
                if (object.Equals(userString, null) || userString.Length <= 0)
                    return null;
                else
                {
                    try
                    {
                        //return StringTool.ToObject<SysGroupUser>(userString);

                        JavaScriptSerializer jserizor = new JavaScriptSerializer();
                        Dictionary<string, object> user = jserizor.DeserializeObject(userString) as Dictionary<string, object>;

                        try
                        {
                            user["AllStore"] = ConvertBool(user["AllStore"]);
                            user["IsManager"] = ConvertBool(user["IsManager"]);
                            user["IsEnable"] = ConvertBool(user["IsEnable"]);
                            user["IsGuestManager"] = ConvertBool(user["IsGuestManager"]);
                        }
                        catch { }

                        SysGroupUser ret = jserizor.ConvertToType<SysGroupUser>(user);

                        return ret;
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog(ex.Message + ":\r\n" + ex.StackTrace);
                        Log.WriteLog(userString);
                        return null;
                    }
                }
            }
            set
            {
                if (value != null)
                {
                    value.Password = "";
                    value.PasswordBak = "";
                    value.StoreList = "";
                }
                if (HttpContext.Current.Request.Cookies.AllKeys.Contains(CURRENTUSERNAME))
                {
                    HttpContext.Current.Request.Cookies[CURRENTUSERNAME].Value = HttpContext.Current.Server.UrlEncode(StringTool.ToJsonString(value));
                    HttpContext.Current.Response.Cookies[CURRENTUSERNAME].Value = HttpContext.Current.Server.UrlEncode(StringTool.ToJsonString(value));
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(CURRENTUSERNAME, HttpContext.Current.Server.UrlEncode(StringTool.ToJsonString(value)));
                    //cookie.Path = "/";
                    //cookie.Domain = ".caimomo.com";
                    //HttpContext.Current.Request.Cookies.Add();
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }

        public static void RemberUser()
        {
            HttpContext.Current.Response.Cookies[CURRENTUSERNAME].Expires = DateTime.Now.AddYears(10);
        }

        public static bool CheckRight(SysGroupUser user, int rightid)
        {
            if (object.Equals(user, null))
                return false;
            if ((bool)user.IsManager)
                return true;
            using (EasyDataContext context = DataContextFactory.GetDataContext<EasyDataContext>(user.GroupID))
            {
                SysGroupUser us = (from m in context.SysGroupUser where m.UID == user.UID select m).SingleOrDefault();

                BaseUserRight right = context.BaseUserRight.FirstOrDefault(
                    u => u.StoreID == us.StoreID && u.UserID == user.UID && u.RightID == rightid);
                if (object.Equals(right, null))
                    return false;
                return right.IsAllow;
            }
        }
    }
}