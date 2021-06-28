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

    public class TcOrderDetailsManagerDelete : IAjaxHandler
    {
        /// <summary>
        /// 订单明细删除
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

                    TC_OrderDetailed tc_OrderDetailed = context.TC_OrderDetailed.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_OrderDetailed, null))
                    {
                        tc_OrderDetailed.IsEnable = false;
                        tc_OrderDetailed.UpdateTime = System.DateTime.Now;
                        tc_OrderDetailed.UpdateUserID = currentUser.UserID;
                        tc_OrderDetailed.UpdateUser = currentUser.UserName;
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


    public class TcOrderDetailsManagerUpdate : IAjaxHandler
    {

        public class tc_orderDetailedSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 订单id
            /// </summary>
            public string OrderID { get; set; }
            /// <summary>
            /// 部门id
            /// </summary>
            public string DepartmentID { get; set; }
            /// <summary>
            /// 餐别id
            /// </summary>
            public string CategoryID { get; set; }
            /// <summary>
            /// 菜品id
            /// </summary>
            public string FoodID { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal? Price { get; set; }
            /// <summary>
            /// 实付
            /// </summary>
            public decimal? Practical { get; set; }
            /// <summary>
            /// 评星
            /// </summary>
            public string Stars { get; set; }
            /// <summary>
            /// 取餐1.已取2.未取
            /// </summary>
            public string Fetch { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }
        }
        /// <summary>
        /// 订单明细修改
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

                    tc_orderDetailedSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_orderDetailedSource>(postContent);

                    TC_OrderDetailed tc_OrderDetailed = context.TC_OrderDetailed.SingleOrDefault(g => g.ID == queryString["ID"]);


                    if (!object.Equals(postContent, null))
                    {
                        if (!string.IsNullOrEmpty(source.OrderID))
                        {
                            tc_OrderDetailed.OrderID = source.OrderID;
                        }
                        if (!string.IsNullOrEmpty(source.DepartmentID))
                        {
                            tc_OrderDetailed.DepartmentID = source.DepartmentID;
                        }
                        if (!string.IsNullOrEmpty(source.CategoryID))
                        {
                            tc_OrderDetailed.CategoryID = source.CategoryID;
                        }
                        if (!string.IsNullOrEmpty(source.FoodID))
                        {
                            tc_OrderDetailed.FoodID = source.FoodID;
                        }
                        if (!string.IsNullOrEmpty(source.Stars))
                        {
                            tc_OrderDetailed.Stars = source.Stars;
                        }
                        if (!string.IsNullOrEmpty(source.Fetch))
                        {
                            tc_OrderDetailed.Fetch = source.Fetch;
                        }
                        if (!string.IsNullOrEmpty(source.Remark))
                        {
                            tc_OrderDetailed.Remark = source.Remark;
                        }
                        tc_OrderDetailed.Price = source.Price;
                        tc_OrderDetailed.Practical = source.Practical;
                        tc_OrderDetailed.UpdateTime = System.DateTime.Now;
                        tc_OrderDetailed.UpdateUserID = currentUser.UserID;
                        tc_OrderDetailed.UpdateUser = currentUser.UserName;
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


    public class TcOrderDetailsManagerQueryList : IAjaxHandler
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
        public class tc_orderDetailedSource
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
            /// 订单id
            /// </summary>
            public string OrderID { get; set; }
            /// <summary>
            /// 部门id
            /// </summary>
            public string DepartmentID { get; set; }
            /// <summary>
            /// 餐别id
            /// </summary>
            public string CategoryID { get; set; }
            /// <summary>
            /// 菜品id
            /// </summary>
            public string FoodID { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal? Price { get; set; }
            /// <summary>
            /// 实付
            /// </summary>
            public decimal? Practical { get; set; }
            /// <summary>
            /// 评星
            /// </summary>
            public string Stars { get; set; }
            /// <summary>
            /// 取餐1.已取2.未取
            /// </summary>
            public string Fetch { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }
        }

        /// <summary>
        /// 订单明细查询
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
                    var query = from am in context.TC_OrderDetailed
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_orderDetailedSource
                                {
                                    ID = am.ID,
                                    OrderID = am.OrderID,
                                    CategoryID = am.CategoryID,
                                    FoodID = am.FoodID,
                                    Price = am.Price,
                                    Practical = am.Practical,
                                    Fetch = am.Fetch,
                                    Stars = am.Stars,
                                    Remark = am.Remark,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser
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
                        query = from am in context.TC_OrderDetailed
                                where am.IsEnable == true && am.GroupID == groudid
                                select new tc_orderDetailedSource
                                {
                                    ID = am.ID,
                                    OrderID = am.OrderID,
                                    CategoryID = am.CategoryID,
                                    FoodID = am.FoodID,
                                    Price = am.Price,
                                    Practical = am.Practical,
                                    Fetch = am.Fetch,
                                    Stars = am.Stars,
                                    Remark = am.Remark,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser
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

                    List<tc_orderDetailedSource> lstCategoryResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<tc_orderDetailedSource>();
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

}