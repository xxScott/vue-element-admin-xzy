using Com.Caimomo.Common;
using DAL;
using HrApp.TC.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace HrApp.TC.Handlers
{
    public class TcMealManagementAdd : IAjaxHandler
    {
        public class tc_categorySource
        {
            /// <summary>
            /// 餐别名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 订餐开始时间
            /// </summary>
            public string ReserveStart { get; set; }
            /// <summary>
            /// 订餐结束时间
            /// </summary>
            public DateTime? ReserveOver { get; set; }
            /// <summary>
            /// 取餐开始时间
            /// </summary>
            public DateTime? FetchStartTime { get; set; }
            /// <summary>
            /// 取餐结束时间
            /// </summary>
            public DateTime? FetchEndTime { get; set; }
        }
        /// <summary>
        /// 餐别添加
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
                        tc_categorySource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_categorySource>(postContent);

                        TC_Category tc_category = new TC_Category();
                        tc_category.ID = Guid.NewGuid().ToString();
                        tc_category.Name = source.Name;
                        tc_category.ReserveStart = source.ReserveStart;
                        tc_category.ReserveOver = source.ReserveOver;
                        tc_category.FetchStartTime = source.FetchStartTime;
                        tc_category.FetchEndTime = source.FetchEndTime;
                        tc_category.IsEnable = true;
                        tc_category.GroupID = currentUser.GroupID;
                        tc_category.StoreID = currentUser.StoreID;
                        tc_category.AddTime = System.DateTime.Now;
                        tc_category.AddUserID = currentUser.UserID;
                        tc_category.AddUser = currentUser.UserName;
                        tc_category.UpdateTime = tc_category.AddTime;
                        tc_category.UpdateUserID = tc_category.AddUserID;
                        tc_category.UpdateUser = tc_category.AddUser;
                        context.TC_Category.InsertOnSubmit(tc_category);
                        context.SubmitChanges();

                    }
                }

                return new HandleResult { ResultCode = 0, Message = "新增成功", Data = null };
            }
            catch (Exception ex)
            {
                Log.WriteLog("新增失败:" + ex);
                return new HandleResult { ResultCode = 1, Message = "新增失败:" + ex, Data = null };
            }

        }
    }


    public class TcMealManagementDelete : IAjaxHandler
    {
        /// <summary>
        /// 餐别删除
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

                    TC_Category tc_category = context.TC_Category.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_category, null))
                    {
                        tc_category.IsEnable = false;
                        tc_category.UpdateTime = System.DateTime.Now;
                        tc_category.UpdateUserID = currentUser.UserID;
                        tc_category.UpdateUser = currentUser.UserName;
                        context.SubmitChanges();
                    }
                }

                return new HandleResult { ResultCode = 0, Message = "删除成功", Data = null };
            }
            catch (Exception ex)
            {
                Log.WriteLog("删除出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "删除出错:" + ex, Data = null };
            }

        }
    }


    public class TcMealManagementUpdate : IAjaxHandler
    {
        public class tc_categorySource
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
            /// 集团号
            /// </summary>
            public int GroupID { get; set; }
            /// <summary>
            /// 门店号
            /// </summary>
            public int StoreID { get; set; }
            /// <summary>
            /// 餐别名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 订餐开始时间
            /// </summary>
            public string ReserveStart { get; set; }
            /// <summary>
            /// 订餐结束时间
            /// </summary>
            public DateTime? ReserveOver { get; set; }
            /// <summary>
            /// 取餐开始时间
            /// </summary>
            public DateTime? FetchStartTime { get; set; }
            /// <summary>
            /// 取餐结束时间
            /// </summary>
            public DateTime? FetchEndTime { get; set; }
        }
        /// <summary>
        /// 餐别修改
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

                    tc_categorySource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_categorySource>(postContent);

                    TC_Category tc_category = context.TC_Category.SingleOrDefault(g => g.ID == queryString["ID"]);


                    if (!object.Equals(postContent, null))
                    {
                        if (!string.IsNullOrEmpty(source.Name))
                        {
                            tc_category.Name = source.Name;
                        }
                        if (!string.IsNullOrEmpty(source.ReserveStart))
                        {
                            tc_category.ReserveStart = source.ReserveStart;
                        }
                        tc_category.ReserveOver = source.ReserveOver;
                        tc_category.FetchStartTime = source.FetchStartTime;
                        tc_category.FetchEndTime = source.FetchEndTime;
                        tc_category.UpdateTime = System.DateTime.Now;
                        tc_category.UpdateUserID = currentUser.UserID;
                        tc_category.UpdateUser = currentUser.UserName;
                        context.SubmitChanges();
                    }
                }

                return new HandleResult { ResultCode = 0, Message = "修改成功", Data = null };
            }
            catch (Exception ex)
            {
                Log.WriteLog("修改失败:" + ex);
                return new HandleResult { ResultCode = 1, Message = "修改失败:" + ex, Data = null };
            }

        }
    }


    public class TcMealManagementQueryList : IAjaxHandler
    {
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
        public class tc_categoryResponse
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
            /// 集团号
            /// </summary>
            public int GroupID { get; set; }
            /// <summary>
            /// 门店号
            /// </summary>
            public int StoreID { get; set; }
            /// <summary>
            /// 餐别名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 订餐开始时间
            /// </summary>
            public string ReserveStart { get; set; }
            /// <summary>
            /// 订餐结束时间
            /// </summary>
            public DateTime? ReserveOver { get; set; }
            /// <summary>
            /// 取餐开始时间
            /// </summary>
            public DateTime? FetchStartTime { get; set; }
            /// <summary>
            /// 取餐结束时间
            /// </summary>
            public DateTime? FetchEndTime { get; set; }
        }

        /// <summary>
        /// 餐别列表查询
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
                    var query = from cg in context.TC_Category
                                where cg.IsEnable == true && cg.StoreID == storeid
                                select new tc_categoryResponse
                                {
                                    ID = cg.ID,
                                    Name = cg.Name,
                                    ReserveStart = cg.ReserveStart,
                                    ReserveOver = cg.ReserveOver,
                                    FetchStartTime = cg.FetchStartTime,
                                    FetchEndTime = cg.FetchEndTime,
                                    AddTime = cg.AddTime,
                                    AddUserID = cg.AddUserID,
                                    AddUser = cg.AddUser,
                                    UpdateTime = cg.UpdateTime,
                                    UpdateUserID = cg.UpdateUserID,
                                    UpdateUser = cg.UpdateUser,
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
                        query = from cg in context.TC_Category
                                where cg.IsEnable == true && cg.GroupID == groudid
                                select new tc_categoryResponse
                                {
                                    ID = cg.ID,
                                    Name = cg.Name,
                                    ReserveStart = cg.ReserveStart,
                                    ReserveOver = cg.ReserveOver,
                                    FetchStartTime = cg.FetchStartTime,
                                    FetchEndTime = cg.FetchEndTime,
                                    AddTime = cg.AddTime,
                                    AddUserID = cg.AddUserID,
                                    AddUser = cg.AddUser,
                                    UpdateTime = cg.UpdateTime,
                                    UpdateUserID = cg.UpdateUserID,
                                    UpdateUser = cg.UpdateUser,
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

                    List<tc_categoryResponse> lstCategoryResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<tc_categoryResponse>();
                    return new MyHandleResult { ResultCode = 0, Message = "查询成功", total_count = pageCount, Data = lstCategoryResponse };

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }

    }

    public class TcMealManagementQueryTree : IAjaxHandler
    {
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

        public class tc_categoryResponse
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime AddTime { get; set; }
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
            public DateTime UpdateTime { get; set; }
            /// <summary>
            /// 更新人员
            /// </summary>
            public string UpdateUser { get; set; }
            /// <summary>
            /// 更新人员ID
            /// </summary>
            public string UpdateUserID { get; set; }
            /// <summary>
            /// 集团号
            /// </summary>
            public int GroupID { get; set; }
            /// <summary>
            /// 门店号
            /// </summary>
            public int StoreID { get; set; }
            /// <summary>
            /// 餐别名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 订餐开始时间
            /// </summary>
            public string ReserveStart { get; set; }
            /// <summary>
            /// 订餐结束时间
            /// </summary>
            public DateTime ReserveOver { get; set; }
            /// <summary>
            /// 取餐开始时间
            /// </summary>
            public DateTime FetchStartTime { get; set; }
            /// <summary>
            /// 取餐结束时间
            /// </summary>
            public DateTime FetchEndTime { get; set; }
        }

        /// <summary>
        /// 餐别列表查询
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
                    var query = from cg in context.TC_Category
                                where cg.IsEnable == true && cg.StoreID == storeid
                                select new tc_categoryResponse
                                {
                                    ID = cg.ID,
                                    ReserveStart = cg.ReserveStart,
                                    //ReserveOver = cg.ReserveOver,
                                    //FetchStartTime = cg.FetchStartTime,
                                    //FetchEndTime = cg.FetchEndTime,
                                    //AddTime = cg.AddTime,
                                    //AddUserID = cg.AddUserID,
                                    //AddUser = cg.AddUser,
                                    //UpdateTime = cg.UpdateTime,
                                    UpdateUserID = cg.UpdateUserID,
                                    UpdateUser = cg.UpdateUser,
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
                        query = from cg in context.TC_Category
                                where cg.IsEnable == true && cg.GroupID == groudid
                                select new tc_categoryResponse
                                {
                                    ID = cg.ID,
                                    ReserveStart = cg.ReserveStart,
                                    //ReserveOver = cg.ReserveOver,
                                    //FetchStartTime = cg.FetchStartTime,
                                    //FetchEndTime = cg.FetchEndTime,
                                    //AddTime = cg.AddTime,
                                    //AddUserID = cg.AddUserID,
                                    //AddUser = cg.AddUser,
                                    //UpdateTime = cg.UpdateTime,
                                    UpdateUserID = cg.UpdateUserID,
                                    UpdateUser = cg.UpdateUser,
                                };
                    }

                    //该账号下管理的门店列表
                    List<StoreResponse> lstStoreResponse = query_s.OrderByDescending(r => r.UID).ToList<StoreResponse>();

                    #endregion


                    List<tc_categoryResponse> lstCategoryResponse = query.OrderByDescending(r => r.UpdateTime).ToList<tc_categoryResponse>();
                    return new HandleResult { ResultCode = 0, Message = "查询成功", Data = lstCategoryResponse };

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("列表查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }

    }

}