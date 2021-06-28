using Com.Caimomo.Common;
using Com.Caimomo.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using DAL;
using Utilities;

namespace HrApp.TC.Handlers
{
    public class SysGroupUserManagementDelete : IAjaxHandler
    {
        /// <summary>
        /// 账号删除
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="queryString"></param>
        /// <param name="postContent"></param>
        /// <returns></returns>
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            try
            {
                using (EasyDataContext context = MyDataContextFactory.GetDataContext<EasyDataContext>(MyDataContextFactory.CanYinCenter))
                {

                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    SysGroupUser sysGroupUser = context.SysGroupUser.SingleOrDefault(g => g.UID == queryString["ID"]);

                    if (!object.Equals(sysGroupUser, null))
                    {
                        sysGroupUser.IsEnable = false;
                        sysGroupUser.UpdateTime = System.DateTime.Now;
                        sysGroupUser.UpdateUser = currentUser.UserID;
                        context.SubmitChanges();
                    }

                    return new HandleResult { ResultCode = 0, Message = "删除成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Com.Caimomo.Common.Log.WriteLog("删除出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "删除出错:" + ex, Data = null };
            }

        }

    }

    public class SysGroupUserManagementQueryList : IAjaxHandler
    {
        /// <summary>
        /// 门店配置--响应
        /// </summary>
        public class StoreResponse
        {
            private string m_UID = string.Empty;
            /// <summary>
            /// ID
            /// </summary>
            public string UID
            {
                get { return m_UID; }
                set
                {
                    m_UID = value;
                }
            }

            private string m_StoreName = string.Empty;
            /// <summary>
            /// 门店名称
            /// </summary>
            public string StoreName
            {
                get { return m_StoreName; }
                set
                {
                    m_StoreName = value;
                }
            }

        }

        public class SysGroupUserResponse
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string UID { get; set; }
            /// <summary>
            /// 用户登录名
            /// </summary>
            public string UserID { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string TrueName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public DateTime? UpdateTime { get; set; }
        }
        /// <summary>
        /// 账号查询列表
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="queryString"></param>
        /// <param name="postContent"></param>
        /// <returns></returns>
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            try
            {
                using (EasyDataContext context = MyDataContextFactory.GetDataContext<EasyDataContext>(MyDataContextFactory.CanYinCenter))
                {
                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    int pageNumber = int.Parse(queryString["pageNumber"]);
                    int pageSize = int.Parse(queryString["pageSize"]);

                    #region 获取该账号下管理的门店列表，并获取对应业务数据列表

                    //默认为门店账户
                    var query = from am in context.SysGroupUser
                                where am.IsEnable == true && am.StoreID == storeid
                                select new SysGroupUserResponse
                                {
                                    UID = am.UID,
                                    UserID = am.UserID,
                                    TrueName = am.TrueName,
                                    Password = "******",
                                    UpdateTime = am.UpdateTime,
                                };

                    //集团00账户，则重新获取
                    if (Utilities.Security.HaveGroupAccount && Utilities.Security.IsGroupAccount)
                    {
                        query = from am in context.SysGroupUser
                                where am.IsEnable == true && am.GroupID == groudid
                                select new SysGroupUserResponse
                                {
                                    UID = am.UID,
                                    UserID = am.UserID,
                                    TrueName = am.TrueName,
                                    Password = "******",
                                    UpdateTime = am.UpdateTime,
                                };
                    }

                    #endregion

                    int pageCount = 0;
                    if (pageNumber > 0)
                    {
                        pageCount = query.ToList().Count;
                    }

                    List<SysGroupUserResponse> lstSysGroupUserResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<SysGroupUserResponse>();
                    return new MyHandleResult { ResultCode = 0, Message = "查询成功", total_count = pageCount, Data = lstSysGroupUserResponse };

                }
            }
            catch (Exception ex)
            {
                Com.Caimomo.Common.Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }
    }

    public class SysGroupUserManagementAdd : IAjaxHandler
    {
        public class SysGroupUserSource
        {
            /// <summary>
            /// 用户登录名
            /// </summary>
            public string UserID { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string TrueName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
        }
        /// <summary>
        /// 新增账号
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="queryString"></param>
        /// <param name="postContent"></param>
        /// <returns></returns>
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            try
            {
                using (EasyDataContext context = MyDataContextFactory.GetDataContext<EasyDataContext>(MyDataContextFactory.CanYinCenter))
                {

                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        SysGroupUserSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<SysGroupUserSource>(postContent);

                        SysGroupUser sysGroupUser = new SysGroupUser();

                        sysGroupUser.UID = Guid.NewGuid().ToString();
                        sysGroupUser.UserID = source.UserID;
                        sysGroupUser.TrueName = source.TrueName;
                        sysGroupUser.Password = Encrypt.Instance.EncryptString(source.Password);
                        sysGroupUser.StoreID = (int)storeid;
                        sysGroupUser.GroupID = (int)groudid;
                        sysGroupUser.IsEnable = true;
                        sysGroupUser.AddTime = System.DateTime.Now;
                        sysGroupUser.AddUser = currentUser.UserID;
                        sysGroupUser.UpdateTime = sysGroupUser.AddTime;
                        sysGroupUser.UpdateUser = sysGroupUser.AddUser;
                        context.SysGroupUser.InsertOnSubmit(sysGroupUser);
                        context.SubmitChanges();
                    }

                    return new HandleResult { ResultCode = 0, Message = "新增成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Com.Caimomo.Common.Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "新增出错:" + ex, Data = null };
            }
        }
    }

    public class SysGroupUserManagementUpdate : IAjaxHandler
    {
        public class SysGroupUserSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string UID { get; set; }
            /// <summary>
            /// 用户登录名
            /// </summary>
            public string UserID { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string TrueName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
        }
        /// <summary>
        /// 修改账号
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="queryString"></param>
        /// <param name="postContent"></param>
        /// <returns></returns>
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            try
            {
                using (EasyDataContext context = MyDataContextFactory.GetDataContext<EasyDataContext>(MyDataContextFactory.CanYinCenter))
                {

                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    SysGroupUserSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<SysGroupUserSource>(postContent);


                    #region 更新

                    SysGroupUser sysGroupUser = context.SysGroupUser.SingleOrDefault(g => g.UID == source.UID); ;

                    if (!object.Equals(sysGroupUser, null))
                    {
                        sysGroupUser.UserID = source.UserID;
                        sysGroupUser.TrueName = source.TrueName;
                        if (!string.IsNullOrEmpty(source.Password))
                        {
                            sysGroupUser.Password = Encrypt.Instance.EncryptString(source.Password);
                            sysGroupUser.PasswordBak = source.Password;
                        }
                        sysGroupUser.StoreID = (int)storeid;
                        sysGroupUser.GroupID = (int)groudid;
                        sysGroupUser.IsEnable = true;
                        sysGroupUser.UpdateTime = System.DateTime.Now;
                        sysGroupUser.UpdateUser = currentUser.UserID;
                        context.SubmitChanges();
                    }

                    #endregion

                    return new HandleResult { ResultCode = 0, Message = "修改成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Com.Caimomo.Common.Log.WriteLog("修改出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "修改出错:" + ex, Data = null };
            }
        }
    }
}