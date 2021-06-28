using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAL;
using MyOrm.Common;
using DAL.Business;

namespace Utilities
{
    public class Security
    {
        public static string cookiename = "caimomo_hr_admin";
        public static string cookienamegroup = "caimomo_hr_admin_group";

        public static bool IsLogin
        {
            get
            {
                return !object.Equals(null, HttpContext.Current.Request.Cookies[cookiename]);
            }
        }

        public static Users CurrentUser
        {
            get
            {
                if (IsLogin)
                {

                    string userID = HttpUtility.UrlEncode(HttpContext.Current.Request.Cookies[cookiename]["userid"].ToString());
                    //sy_User user = Tools.GetFromCache(userID) as sy_User;
                    //if (object.Equals(user, null))
                    //{ && c.EnterpriseID.Equals(enterpriseId)
                    Users u = ServiceFactory.Factory.UsersService.SearchOne(new SimpleCondition("ID", userID));
                    Cache.AddToCache(u.ID.ToString().Trim(), u);
                    return u;
                    //}
                    //else
                    //{
                    //    return user;
                    //}
                }
                else
                {
                    return null;
                }
            }
        }

        
        /// <summary>
        /// 当前集团是否有集团账户
        /// </summary>
        public static bool HaveGroupAccount
        {
            get
            {
                string haveGroupAccount = HttpUtility.UrlEncode(HttpContext.Current.Request.Cookies[cookienamegroup]["haveGroupAccount"].ToString());
                if (haveGroupAccount != null && haveGroupAccount == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                string haveGroupAccount = "0";
                if (value)
                {
                    haveGroupAccount = "1";
                }
                HttpCookie cookie = new HttpCookie(cookienamegroup);
                cookie.Values.Add("haveGroupAccount", haveGroupAccount);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            
        }

        /// <summary>
        /// 当前用户是否是集团账户
        /// </summary>
        public static bool IsGroupAccount
        {
            get
            {
                bool result = false;
                Users user = CurrentUser;
                if (user != null && user.StoreID.HasValue && (user.StoreID.Value.ToString().EndsWith("00") || user.StoreID.Value.ToString().EndsWith("-1")))
                {
                    result = true;
                }
                return result;
            }
            

        }

        public static Roles CurrentUserRole
        {
            get
            {
                if (IsLogin)
                {
                    string userID = HttpUtility.UrlEncode(HttpContext.Current.Request.Cookies[cookiename]["userid"].ToString());
                    //sy_User user = Tools.GetFromCache(userID) as sy_User;
                    //if (object.Equals(user, null))
                    //{ && c.EnterpriseID.Equals(enterpriseId)
                    UserInRole r = ServiceFactory.Factory.UserInRoleService.SearchOne(new SimpleCondition("UserID", userID));
                    if (r != null)
                    {
                        Roles role = ServiceFactory.Factory.RolesService.SearchOne(new SimpleCondition("ID", r.RoleID));
                        Cache.AddToCache(r.ID.ToString().Trim(), r);
                        return role;
                    }
                    else
                        return null;
                    //}
                    //else
                    //{
                    //    return user;
                    //}
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="message">执行信息</param>
        /// <returns></returns>
        public static bool Login(string userID, string password, string uId, ref string message)
        {
            try
            {
                int Id = ServiceFactory.Factory.UsersService.Login(uId, userID, password);
                HttpCookie cookie = new HttpCookie(cookiename);
                cookie.Values.Add("userid", HttpUtility.UrlEncode(Id.ToString()));
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="message">执行信息</param>
        /// <returns></returns>
        public static bool Login(string userID, string password, ref string message)
        {
            try
            {
                int Id = ServiceFactory.Factory.UsersService.Login(userID, password);
                HttpCookie cookie = new HttpCookie(cookiename);
                cookie.Values.Add("userid", HttpUtility.UrlEncode(Id.ToString()));
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Logout()
        {
            HttpCookie cookie = new HttpCookie(cookiename);
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
            Cache.RemoveCache(CurrentUser.UserID);
        }
    }
}
