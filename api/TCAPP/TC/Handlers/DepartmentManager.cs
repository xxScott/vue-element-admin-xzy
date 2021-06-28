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

    public class TcDepartmentManagementDelete : IAjaxHandler
    {
        /// <summary>
        /// 部门删除
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
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录" , Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    TC_Department tc_Department = context.TC_Department.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_Department, null))
                    {
                        tc_Department.IsEnable = false;
                        tc_Department.UpdateTime = System.DateTime.Now;
                        tc_Department.UpdateUserID = currentUser.UserID;
                        tc_Department.UpdateUser = currentUser.UserName;
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

    public class TcDepartmentManagementQueryList : IAjaxHandler
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

        public class tc_departmentResponse
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
            /// 部门编号
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 部门名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 部门地点
            /// </summary>
            public string Loc { get; set; }

        }
        /// <summary>
        /// 部门查询列表
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
                    var query = from am in context.TC_Department
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_departmentResponse
                                {
                                    ID = am.ID,
                                    Code = am.Code,
                                    Name = am.Name,
                                    Loc = am.Loc,
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
                        query = from am in context.TC_Department
                                where am.IsEnable == true && am.GroupID == groudid
                                select new tc_departmentResponse
                                {
                                    ID = am.ID,
                                    Code = am.Code,
                                    Name = am.Name,
                                    Loc = am.Loc,
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

                    List<tc_departmentResponse> lstDepartmentResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<tc_departmentResponse>();
                    return new MyHandleResult { ResultCode = 0, Message = "查询成功", total_count = pageCount, Data = lstDepartmentResponse };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "列表查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcDepartmentManagementQueryTree : IAjaxHandler
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

        public class tc_departmentResponse
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
            /// 部门编号
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 部门名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 部门地点
            /// </summary>
            public string Loc { get; set; }

        }
        /// <summary>
        /// 部门查询列表
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
                    var query = from am in context.TC_Department
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_departmentResponse
                                {
                                    ID = am.ID,
                                    Code = am.Code,
                                    Name = am.Name,
                                    Loc = am.Loc,
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
                        query = from am in context.TC_Department
                                where am.IsEnable == true && am.GroupID == groudid
                                select new tc_departmentResponse
                                {
                                    ID = am.ID,
                                    Code = am.Code,
                                    Name = am.Name,
                                    Loc = am.Loc,
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

                    List<tc_departmentResponse> lstDepartmentResponse = query.OrderByDescending(r => r.UpdateTime).ToList<tc_departmentResponse>();
                    return new HandleResult { ResultCode = 0, Message = "查询成功", Data = lstDepartmentResponse };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "列表查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcDepartmentManagementAdd : IAjaxHandler
    {
        public class tc_departmentSource
        {
            /// <summary>
            /// 部门编号
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 部门名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 部门地点
            /// </summary>
            public string Loc { get; set; }


        }
        /// <summary>
        /// 部门新增
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
                        tc_departmentSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_departmentSource>(postContent);

                        TC_Department tC_Department = new TC_Department();
                        tC_Department.ID = Guid.NewGuid().ToString();
                        tC_Department.Code = source.Code;
                        tC_Department.Name = source.Name;
                        tC_Department.Loc = source.Loc;
                        tC_Department.IsEnable = true;
                        tC_Department.StoreID = currentUser.StoreID;
                        tC_Department.AddTime = System.DateTime.Now;
                        tC_Department.AddUserID = currentUser.UserID;
                        tC_Department.AddUser = currentUser.UserName;
                        tC_Department.UpdateTime = tC_Department.AddTime;
                        tC_Department.UpdateUserID = tC_Department.AddUserID;
                        tC_Department.UpdateUser = tC_Department.AddUser;

                        context.TC_Department.InsertOnSubmit(tC_Department);
                        context.SubmitChanges();
                    }

                    return new HandleResult { ResultCode = 0, Message = "新增成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("新增出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "新增出错:" + ex, Data = null };
            }
        }
    }

    public class TcDepartmentManagementUpdate : IAjaxHandler
    {
        public class tc_departmentSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 部门编号
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 部门名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 部门地点
            /// </summary>
            public string Loc { get; set; }
        }
        /// <summary>
        /// 账号修改
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

                    tc_departmentSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_departmentSource>(postContent);


                    #region 更新Inspection_DepartmentConfig

                    TC_Department tc_department = context.TC_Department.SingleOrDefault(g => g.ID == source.ID); ;

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        if (!string.IsNullOrEmpty(source.Code))
                        {
                            tc_department.Code = source.Code;
                        }
                        if (!string.IsNullOrEmpty(source.Name))
                        {
                            tc_department.Name = source.Name;
                        }
                        if (!string.IsNullOrEmpty(source.Loc))
                        {
                            tc_department.Loc = source.Loc;
                        }
                        tc_department.UpdateTime = System.DateTime.Now;
                        tc_department.UpdateUserID = currentUser.UserID;
                        tc_department.UpdateUser = currentUser.UserName;
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
}