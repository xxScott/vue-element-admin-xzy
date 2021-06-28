using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Caimomo.Entity;
using System.Web.Script.Serialization;

namespace Com.Caimomo.Common
{
    /// <summary>
    /// TCSecurity 的摘要说明
    /// </summary>
    public class TCSecurity
    {
        public static readonly string CURRENTUSERNAME = "EasyTimeStudio_CurrentUser";
        public static readonly string CURRENTOPENID = "EasyTimeStudio_OpenId";

        public static bool IsLogin
        {
            get
            {
                return !object.Equals(CurrentUser, null);
            }
        }

        public static void SaveOpenIdToCookie(string openid)
        {
            HttpCookie cookie = new HttpCookie(CURRENTOPENID, HttpContext.Current.Server.UrlEncode(openid));
            
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCurrentUserOpenId()
        {
            string openId = string.Empty;
            if (!object.Equals(HttpContext.Current, null) && HttpContext.Current.Request.Cookies.AllKeys.Contains(CURRENTOPENID))
            {
                openId = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[CURRENTOPENID].Value);
            }
            return "strOpenID";
            //return openId;
        }

        /// <summary>
        /// 当前登录OpenID是否绑定手机号
        /// </summary>
        public static bool IsCurrentUserBindMoble
        {
            get
            {
                bool result = false;
                TC_AccountManagement currentUser = CurrentUser;
                if (currentUser != null)
                {
                    foreach (TC_UserAuths ua in currentUser.LstTC_UserAuths)
                    {
                        if (!string.IsNullOrEmpty(ua.Credential) && ua.Identifier == GetCurrentUserOpenId())
                        {
                            result = true;
                            break;
                        }
                    }
                   
                }
                return result;
            }
          

        }

        public static void ClearCurrentUserCookie()
        {
            if (!object.Equals(HttpContext.Current, null) && HttpContext.Current.Request.Cookies.AllKeys.Contains(CURRENTUSERNAME))
            {
                HttpContext.Current.Request.Cookies.Remove(CURRENTUSERNAME);
            }
        }

    
        public static TC_AccountManagement CurrentUser
        {
            get
            {
                TC_AccountManagement currentUser = null;
                string userString = null;
                if (!object.Equals(HttpContext.Current, null) && HttpContext.Current.Request.Cookies.AllKeys.Contains(CURRENTUSERNAME))
                {
                    userString = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[CURRENTUSERNAME].Value);
                }
                if (string.IsNullOrEmpty(userString) || object.Equals(userString, null) || userString.Length <= 0)
                {
                    try
                    {

                        using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                        {
                            TC_UserAuths ua = context.TC_UserAuths.SingleOrDefault(g => g.Identifier == GetCurrentUserOpenId());
                            if (ua != null)
                            {
                                if (!string.IsNullOrEmpty(ua.UserID))
                                {
                                    currentUser = context.TC_AccountManagement.SingleOrDefault(g => g.ID == ua.UserID);
                                    if (!object.Equals(currentUser, null))
                                    {
                                        currentUser.GroupID = ua.GroupID;
                                        currentUser.StoreID = ua.StoreID;
                                        currentUser.OpenID = ua.Identifier;
                                        currentUser.Phone = ua.Credential;
                                        currentUser.LstTC_UserAuths = context.TC_UserAuths.Where(g => g.UserID == currentUser.ID).ToList();
                                    }
                                }
                                else
                                {
                                    currentUser = new TC_AccountManagement();
                                    currentUser.GroupID = ua.GroupID;
                                    currentUser.StoreID = ua.StoreID;
                                    currentUser.OpenID = ua.Identifier;
                                }

                                HttpCookie cookie = new HttpCookie(CURRENTUSERNAME, HttpContext.Current.Server.UrlEncode(Newtonsoft.Json.JsonConvert.SerializeObject(currentUser)));

                                HttpContext.Current.Response.Cookies.Add(cookie);

                                //HttpContext.Current.Session.Add("CurrentUser", currentUser);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("获取当前登录人出错:" + ex + "|" + ex.StackTrace);

                    }
                }
                else
                {
                    try
                    {
                        currentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_AccountManagement>(userString);

                       
                        return currentUser;
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog(ex.Message + ":\r\n" + ex.StackTrace);
                        Log.WriteLog(userString);
                        return null;
                    }
                }
                return currentUser;


                //TC_AccountManagement currentUser = HttpContext.Current.Session["CurrentUser"] as TC_AccountManagement;
                //if (currentUser == null)
                //{
                //    try
                //    {

                //        using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                //        {


                //            TC_AccountManagement tc_CurreUser = context.TC_AccountManagement.SingleOrDefault(g => g.OpenID == HttpContext.Current.Session["OpenId"].ToString());
                //            if (!object.Equals(tc_CurreUser, null))
                //            {
                //                tc_CurreUser.LstTC_UserAuths = context.TC_UserAuths.Where(g => g.UserID == tc_CurreUser.ID).ToList();
                //            }

                //            HttpContext.Current.Session.Add("CurrentUser",currentUser);

                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.WriteLog("获取当前登录人出错:" + ex + "|" + ex.StackTrace);

                //    }
                //}
                //return currentUser;

               
            }

        }

      
    }
}