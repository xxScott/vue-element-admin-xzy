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

    public class TcDepartmentUserManagementDelete : IAjaxHandler
    {
        /// <summary>
        /// 部门人员关系删除
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

                    TC_DepartmentUser tc_DepartmentUser = context.TC_DepartmentUser.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_DepartmentUser, null))
                    {
                        tc_DepartmentUser.IsEnable = false;
                        tc_DepartmentUser.UpdateTime = System.DateTime.Now;
                        tc_DepartmentUser.UpdateUserID = currentUser.UserID;
                        tc_DepartmentUser.UpdateUser = currentUser.UserName;
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

    public class TcDepartmentUserManagementQueryList : IAjaxHandler
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
        public class TC_DepartmentUserResponse
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
            /// 部门ID
            /// </summary>
            public string DepartmentID { get; set; }
            /// <summary>
            /// 部门编号
            /// </summary>
            public string DepartmentCode { get; set; }
            /// <summary>
            /// 部门名称
            /// </summary>
            public string DepartmentName { get; set; }
            /// <summary>
            /// 部门地点
            /// </summary>
            public string DepartmentLoc { get; set; }
            /// <summary>
            /// 人员ID
            /// </summary>
            public string UserID { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string UserPassword { get; set; }
            /// <summary>
            /// 手机号
            /// </summary>
            public string UserPhone { get; set; }
            /// <summary>
            /// 用户编号
            /// </summary>
            public string Code { get; set; }
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
                    var query = from am in context.TC_AccountManagement
                                where am.IsEnable == true && am.StoreID == storeid
                                select new TC_DepartmentUserResponse
                                {
                                    ID = am.ID,
                                    UserName = am.UserName,
                                    Code = am.Code,
                                    DepartmentName = string.Empty,
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
                                select new TC_DepartmentUserResponse
                                {
                                        ID = am.ID,
                                        UserName = am.UserName,
                                        Code = am.Code,
                                        DepartmentName = string.Empty,
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

                    //所有用户列表
                    List<TC_DepartmentUserResponse> lstTC_DepartmentUserResponse = query.OrderByDescending(r => r.ID).ToList<TC_DepartmentUserResponse>();

                    #region 根据用户查询所属部门
                    foreach (var departmentuserresponse in lstTC_DepartmentUserResponse)
                    {

                        #region 查询部门
                        var queryDepartment = from am in context.TC_DepartmentUser
                                              from d in context.TC_Department
                                              where d.IsEnable == true && am.IsEnable == true && d.ID == am.DepartmentID && am.UserID == departmentuserresponse.ID
                                              select new tc_departmentResponse
                                              {
                                                  Name = d.Name,
                                              };


                        List<tc_departmentResponse> lsttc_departmentResponse = queryDepartment.ToList<tc_departmentResponse>();
                        foreach (var deaprtment in lsttc_departmentResponse)
                        {
                            departmentuserresponse.DepartmentName += deaprtment.Name + ",";
                        }

                        if (departmentuserresponse.DepartmentName != null && departmentuserresponse.DepartmentName.EndsWith(","))
                        {
                            departmentuserresponse.DepartmentName = departmentuserresponse.DepartmentName.Substring(0, departmentuserresponse.DepartmentName.Length - 1);
                        }
                        #endregion

                    }

                    #endregion

                    int pageCount = 0;
                    if (pageNumber > 0)
                    {
                        pageCount = query.ToList().Count;
                    }

                    return new MyHandleResult { ResultCode = 0, Message = "查询成功", total_count = pageCount, Data = lstTC_DepartmentUserResponse };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "列表查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcDepartmentUserManagementAdd : IAjaxHandler
    {
        /// <summary>
        /// 部门配置--响应
        /// </summary>
        public class DepartmentResponse
        {
            /// <summary>
            /// 部门编号
            /// </summary>
            public string ID { get; set; }
        }

        public class TC_DepartmentUserSource
        {
            /// <summary>
            /// 部门编号
            /// </summary>
            public List<DepartmentResponse> DepartmentID { get; set; }
            /// <summary>
            /// 人员编号
            /// </summary>
            public string UserID { get; set; }

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
                        TC_DepartmentUserSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_DepartmentUserSource>(postContent);

                        TC_DepartmentUser tC_DepartmentUser = new TC_DepartmentUser();
                        List<DepartmentResponse> DepartmentID =  source.DepartmentID;
                        String departId = "";
                        foreach (var dId in DepartmentID) {
                            if (!"".Equals(departId)) {
                                departId = departId + ",";
                            }
                             departId = departId + dId.ID ;   
                        }
                        tC_DepartmentUser.DepartmentID = departId;
                        tC_DepartmentUser.ID = Guid.NewGuid().ToString();
                        tC_DepartmentUser.UserID = source.UserID;
                        tC_DepartmentUser.StoreID = currentUser.StoreID;
                        tC_DepartmentUser.IsEnable = true;
                        tC_DepartmentUser.AddTime = System.DateTime.Now;
                        tC_DepartmentUser.AddUserID = currentUser.UserID;
                        tC_DepartmentUser.AddUser = currentUser.UserName;
                        tC_DepartmentUser.UpdateTime = tC_DepartmentUser.AddTime;
                        tC_DepartmentUser.UpdateUserID = tC_DepartmentUser.AddUserID;
                        tC_DepartmentUser.UpdateUser = tC_DepartmentUser.AddUser;

                        context.TC_DepartmentUser.InsertOnSubmit(tC_DepartmentUser);
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

    public class TcDepartmentUserManagementUpdate : IAjaxHandler
    {
        /// <summary>
        /// 部门配置--响应
        /// </summary>
        public class DepartmentResponse
        {
            /// <summary>
            /// 部门编号
            /// </summary>
            public string ID { get; set; }
        }

        public class TC_DepartmentUserSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 部门编号
            /// </summary>
            public List<DepartmentResponse> DepartmentID { get; set; }
            /// <summary>
            /// 人员编号
            /// </summary>
            public string UserID { get; set; }

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

                    TC_DepartmentUserSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_DepartmentUserSource>(postContent);


                    #region 更新Inspection_DepartmentConfig

                    TC_DepartmentUser tC_DepartmentUser = context.TC_DepartmentUser.SingleOrDefault(g => g.ID == source.ID); ;

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        if (!string.IsNullOrEmpty(source.UserID))
                        {
                            tC_DepartmentUser.UserID = source.UserID;
                        }
                        List<DepartmentResponse> DepartmentID = source.DepartmentID;
                        tC_DepartmentUser.UpdateTime = System.DateTime.Now;
                        tC_DepartmentUser.UpdateUserID = currentUser.UserID;
                        tC_DepartmentUser.UpdateUser = currentUser.UserName;
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