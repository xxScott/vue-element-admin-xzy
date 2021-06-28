using Com.Caimomo.Common;
using HrApp.TC.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using DAL;

namespace HrApp.TC.Handlers
{
    public class TcAccountManagementDelete : IAjaxHandler
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
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {

                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    TC_AccountManagement tc_Account = context.TC_AccountManagement.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_Account, null))
                    {
                        tc_Account.IsEnable = false;
                        tc_Account.UpdateTime = System.DateTime.Now;
                        tc_Account.UpdateUserID = currentUser.UserID;
                        tc_Account.UpdateUser = currentUser.UserName;
                        context.SubmitChanges();
                    }

                    return new HandleResult { ResultCode = 0, Message = "删除成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("删除出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "删除出错:" + ex, Data = null };
            }

        }

    }

    public class TcAccountManagementQueryList : IAjaxHandler
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

        public class tc_accountManagementResponse
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime? AddTime { get; set; }
            /// <summary>
            /// 创建人员
            /// </summary>
            public string AddUser { get; set; }
            /// <summary>
            /// 创建人员ID
            /// </summary>
            public string AddUserID { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public DateTime? UpdateTime { get; set; }
            /// <summary>
            /// 更新人员
            /// </summary>
            public string UpdateUser { get; set; }
            /// <summary>
            /// 更新人员ID
            /// </summary>
            public string UpdateUserID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 用户编号
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 手机号
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 权限1.管理员2.用户
            /// </summary>
            public string Jurisdiction { get; set; }
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
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    int pageNumber = int.Parse(queryString["pageNumber"]);
                    int pageSize = int.Parse(queryString["pageSize"]);

                    #region 获取该账号下管理的门店列表，并获取对应业务数据列表

                    //默认为门店账户
                    var query_s = from id in context.B_Store
                                  where id.UID == storeid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                    var query = from am in context.TC_AccountManagement
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_accountManagementResponse
                                {
                                    ID = am.ID,
                                    UserName = am.UserName,
                                    Phone = am.Phone,
                                    Code = am.Code,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser,
                                };

                    //集团00账户，则重新获取
                    if (Utilities.Security.HaveGroupAccount && Utilities.Security.IsGroupAccount)
                    {
                        query_s = from id in context.B_Store
                                  where id.GroupID == groudid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                        query = from am in context.TC_AccountManagement
                                where am.IsEnable == true && am.GroupID == groudid
                                select new tc_accountManagementResponse
                                {
                                    ID = am.ID,
                                    UserName = am.UserName,
                                    Phone = am.Phone,
                                    Code = am.Code,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser,
                                };
                    }

                    //该账号下管理的门店列表
                    List<StoreResponse> lstStoreResponse = query_s.OrderByDescending(r => r.UID).ToList<StoreResponse>();

                    #endregion

                    int pageCount = 0;
                    if (pageNumber > 0)
                    {
                        pageCount = query.ToList().Count;
                    }

                    List<tc_accountManagementResponse> lstAccountManagementResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<tc_accountManagementResponse>();
                    return new MyHandleResult { ResultCode = 0, Message = "查询成功", total_count = pageCount, Data = lstAccountManagementResponse };

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcAccountManagementQueryTree : IAjaxHandler
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

        public class tc_accountManagementResponse
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime? AddTime { get; set; }
            /// <summary>
            /// 创建人员
            /// </summary>
            public string AddUser { get; set; }
            /// <summary>
            /// 创建人员ID
            /// </summary>
            public string AddUserID { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public DateTime? UpdateTime { get; set; }
            /// <summary>
            /// 更新人员
            /// </summary>
            public string UpdateUser { get; set; }
            /// <summary>
            /// 更新人员ID
            /// </summary>
            public string UpdateUserID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 手机号
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 权限1.管理员2.用户
            /// </summary>
            public string Jurisdiction { get; set; }
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
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    #region 获取该账号下管理的门店列表，并获取对应业务数据列表

                    //默认为门店账户
                    var query_s = from id in context.B_Store
                                  where id.UID == storeid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                    var query = from am in context.TC_AccountManagement
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_accountManagementResponse
                                {
                                    ID = am.ID,
                                    UserName = am.UserName,
                                    Phone = am.Phone,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser,
                                };

                    //集团00账户，则重新获取
                    if (Utilities.Security.HaveGroupAccount && Utilities.Security.IsGroupAccount)
                    {
                        query_s = from id in context.B_Store
                                  where id.GroupID == groudid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                        query = from am in context.TC_AccountManagement
                                where am.IsEnable == true && am.GroupID == groudid
                                select new tc_accountManagementResponse
                                {
                                    ID = am.ID,
                                    UserName = am.UserName,
                                    Phone = am.Phone,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser,
                                };
                    }

                    //该账号下管理的门店列表
                    List<StoreResponse> lstStoreResponse = query_s.OrderByDescending(r => r.UID).ToList<StoreResponse>();

                    #endregion

                    List<tc_accountManagementResponse> lstAccountManagementResponse = query.OrderByDescending(r => r.UpdateTime).ToList<tc_accountManagementResponse>();
                    return new HandleResult { ResultCode = 0, Message = "查询成功", Data = lstAccountManagementResponse };

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcAccountManagementAdd : IAjaxHandler
    {
        public class tc_accountManagementSource
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 电子邮件
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// 手机号
            /// </summary>
            public string Phone { get; set; }
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
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {

                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        tc_accountManagementSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_accountManagementSource>(postContent);

                        TC_AccountManagement tc_Account = new TC_AccountManagement();
                        tc_Account.ID = Guid.NewGuid().ToString();
                        tc_Account.UserName = source.UserName;
                        tc_Account.Password = source.Password;
                        tc_Account.Phone = source.Phone;
                        tc_Account.StoreID = currentUser.StoreID;
                        tc_Account.IsEnable = true;
                        tc_Account.AddTime = System.DateTime.Now;
                        tc_Account.AddUserID = currentUser.UserID;
                        tc_Account.AddUser = currentUser.UserName;
                        tc_Account.UpdateTime = tc_Account.AddTime;
                        tc_Account.UpdateUserID = tc_Account.AddUserID;
                        tc_Account.UpdateUser = tc_Account.AddUser;
                        context.TC_AccountManagement.InsertOnSubmit(tc_Account);
                        context.SubmitChanges();
                    }

                    return new HandleResult { ResultCode = 0, Message = "新增成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "新增出错:" + ex, Data = null };
            }
        }
    }

    public class TcAccountManagementUpdate : IAjaxHandler
    {

        public class tc_accountManagementSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 手机号
            /// </summary>
            public string Phone { get; set; }
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
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {

                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    tc_accountManagementSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_accountManagementSource>(postContent);


                    #region 更新Inspection_DepartmentConfig

                    TC_AccountManagement tc_Account = context.TC_AccountManagement.SingleOrDefault(g => g.ID == source.ID); ;

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        if (!string.IsNullOrEmpty(source.UserName))
                        {
                            tc_Account.UserName = source.UserName;
                        }
                        if (!string.IsNullOrEmpty(source.Password))
                        {
                            tc_Account.Password = source.Password;
                        }
                        if (!string.IsNullOrEmpty(source.Phone))
                        {
                            tc_Account.Phone = source.Phone;
                        }
                        tc_Account.UpdateTime = System.DateTime.Now;
                        tc_Account.UpdateUserID = currentUser.UserID;
                        tc_Account.UpdateUser = currentUser.UserName;
                        context.SubmitChanges();
                    }

                    #endregion

                    return new HandleResult { ResultCode = 0, Message = "修改成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("修改出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "修改出错:" + ex, Data = null };
            }
        }
    }

    public class TcAccountManagementUpdateStatus
    {
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            try
            {
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    TC_AccountManagement tc_Account = context.TC_AccountManagement.SingleOrDefault(g => g.ID == queryString["ID"]);
                    tc_Account.IsEnable = !tc_Account.IsEnable;
                    context.SubmitChanges();

                    return new HandleResult { ResultCode = 0, Message = "账号状态修改成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("账号状态修改出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "账号状态修改出错:" + ex, Data = null };
            }
        }
    }
}