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
    public class TcFoodManagementDelete : IAjaxHandler
    {
        /// <summary>
        ///  菜品删除
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
                    {
                        int? storeid = currentUser.StoreID;
                        int? groudid = currentUser.GroupID;
                    }

                    TC_Food tc_Food = context.TC_Food.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_Food, null))
                    {
                        tc_Food.IsEnable = false;
                        tc_Food.UpdateTime = System.DateTime.Now;
                        tc_Food.UpdateUserID = currentUser.UserID;
                        tc_Food.UpdateUser = currentUser.UserName;
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

    public class TcFoodManagementQueryList : IAjaxHandler
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

        public class tc_foodResponse
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
            /// 日期
            /// </summary>
            public DateTime? Date { get; set; }
            /// <summary>
            /// 菜名
            /// </summary>
            public string Menu { get; set; }
            /// <summary>
            /// 套餐名称
            /// </summary>
            public string SetMeal { get; set; }
            /// <summary>
            /// 菜品简介
            /// </summary>
            public string Brief { get; set; }
            /// <summary>
            /// 菜品状态1.已售空2.未售空
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 菜品类型1.大荤2.小炒3.素菜4.套餐
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 时间1.本周2.下周
            /// </summary>
            public string Time { get; set; }
            /// <summary>
            /// 库存
            /// </summary>
            public int? Inventory { get; set; }
            /// <summary>
            /// 已订
            /// </summary>
            public int? Intended { get; set; }
            /// <summary>
            /// 限订
            /// </summary>
            public int? Restrict { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal? Price { get; set; }
        }

        /// <summary>
        ///  菜品列表
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

                    //默认门店账户
                    var query_s = from id in context.B_Store
                                  where id.UID == storeid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                    var query = from am in context.TC_Food
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_foodResponse
                                {
                                    ID = am.ID,
                                    Date = am.Date,
                                    Menu = am.Menu,
                                    SetMeal = am.SetMeal,
                                    Brief = am.Brief,
                                    Status = am.Status,
                                    Type = am.Type,
                                    Time = am.Time,
                                    Inventory = am.Inventory,
                                    Intended = am.Intended,
                                    Restrict = am.Restrict,
                                    Price = am.Price,
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
                        query = from am in context.TC_Food
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_foodResponse
                                {
                                    ID = am.ID,
                                    Date = am.Date,
                                    Menu = am.Menu,
                                    SetMeal = am.SetMeal,
                                    Brief = am.Brief,
                                    Status = am.Status,
                                    Type = am.Type,
                                    Time = am.Time,
                                    Inventory = am.Inventory,
                                    Intended = am.Intended,
                                    Restrict = am.Restrict,
                                    Price = am.Price,
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

                    List<tc_foodResponse> lstFoodResponses = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<tc_foodResponse>();
                    return new MyHandleResult { ResultCode = 0, Message = "查询成功", total_count = pageCount, Data = lstFoodResponses };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcFoodManagementAdd : IAjaxHandler
    {
        private class tc_foodSource
        {
            /// <summary>
            /// 日期
            /// </summary>
            public DateTime? Date { get; set; }
            /// <summary>
            /// 日期
            /// </summary>
            public string Menu { get; set; }
            /// <summary>
            /// 套餐名称
            /// </summary>
            public string SetMeal { get; set; }
            /// <summary>
            /// 菜品简介
            /// </summary>
            public string Brief { get; set; }
            /// <summary>
            /// 菜品状态1.已售空2.未售空
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 菜品类型1.大荤2.小炒3.素菜4.套餐
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 时间1.本周2.下周
            /// </summary>
            public string Time { get; set; }
            /// <summary>
            /// 库存
            /// </summary>
            public int? Inventory { get; set; }
            /// <summary>
            /// 已订
            /// </summary>
            public int? Intended { get; set; }
            /// <summary>
            /// 限订
            /// </summary>
            public int? Restrict { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal? Price { get; set; }
        }
        /// <summary>
        ///  菜品新增
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
                        tc_foodSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_foodSource>(postContent);

                        TC_Food tc_Food = new TC_Food();
                        tc_Food.ID = Guid.NewGuid().ToString();
                        tc_Food.Date = source.Date;
                        tc_Food.Menu = source.Menu;
                        tc_Food.SetMeal = source.SetMeal;
                        tc_Food.Brief = source.Brief;
                        tc_Food.Status = source.Status;
                        tc_Food.Type = source.Type;
                        tc_Food.Time = source.Time;
                        tc_Food.Inventory = source.Inventory;
                        tc_Food.Intended = source.Intended;
                        tc_Food.Restrict = source.Restrict;
                        tc_Food.Price = source.Price;
                        tc_Food.IsEnable = true;
                        tc_Food.StoreID = currentUser.StoreID;
                        tc_Food.AddTime = System.DateTime.Now;
                        tc_Food.AddUserID = currentUser.UserID;
                        tc_Food.AddUser = currentUser.UserName;
                        tc_Food.UpdateTime = tc_Food.AddTime;
                        tc_Food.UpdateUserID = tc_Food.AddUserID;
                        tc_Food.UpdateUser = tc_Food.AddUser;
                        context.TC_Food.InsertOnSubmit(tc_Food);
                        context.SubmitChanges();
                    };
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

    public class TcFoodManagementUpdate : IAjaxHandler
    {
        private class tc_foodSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 日期
            /// </summary>
            public DateTime? Date { get; set; }
            /// <summary>
            /// 菜名
            /// </summary>
            public string Menu { get; set; }
            /// <summary>
            /// 套餐名称
            /// </summary>
            public string SetMeal { get; set; }
            /// <summary>
            /// 菜品简介
            /// </summary>
            public string Brief { get; set; }
            /// <summary>
            /// 菜品状态1.已售空2.未售空
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 菜品类型1.大荤2.小炒3.素菜4.套餐
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 时间1.本周2.下周
            /// </summary>
            public string Time { get; set; }
            /// <summary>
            /// 库存
            /// </summary>
            public int? Inventory { get; set; }
            /// <summary>
            /// 已订
            /// </summary>
            public int? Intended { get; set; }
            /// <summary>
            /// 限订
            /// </summary>
            public int? Restrict { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal? Price { get; set; }
        }
        /// <summary>
        ///  菜品修改
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

                    tc_foodSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<tc_foodSource>(postContent);

                    #region 更新Inspection_FoodConfig

                    TC_Food tc_Food = context.TC_Food.SingleOrDefault(g => g.ID == source.ID);
                    if (!string.IsNullOrEmpty(postContent))
                    {
                        if (!string.IsNullOrEmpty(source.Menu))
                        {
                            tc_Food.Menu = source.Menu;
                        }
                        if (!string.IsNullOrEmpty(source.SetMeal))
                        {
                            tc_Food.SetMeal = source.SetMeal;
                        }
                        if (!string.IsNullOrEmpty(source.Brief))
                        {
                            tc_Food.Brief = source.Brief;
                        }
                        if (!string.IsNullOrEmpty(source.Status))
                        {
                            tc_Food.Status = source.Status;
                        }
                        if (!string.IsNullOrEmpty(source.Type))
                        {
                            tc_Food.Type = source.Type;
                        }
                        if (!string.IsNullOrEmpty(source.Time))
                        {
                            tc_Food.Time = source.Time;
                        }
                        tc_Food.Date = source.Date;
                        tc_Food.Inventory = source.Inventory;
                        tc_Food.Intended = source.Intended;
                        tc_Food.Restrict = source.Restrict;
                        tc_Food.Price = source.Price;
                        tc_Food.UpdateTime = System.DateTime.Now;
                        tc_Food.UpdateUserID = currentUser.UserID;
                        tc_Food.UpdateUser = currentUser.UserName;
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

    public class TcFoodManagementQueryTree : IAjaxHandler
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

        public class tc_foodResponse
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
            /// 日期
            /// </summary>
            public DateTime? Date { get; set; }
            /// <summary>
            /// 菜名
            /// </summary>
            public string Menu { get; set; }
            /// <summary>
            /// 套餐名称
            /// </summary>
            public string SetMeal { get; set; }
            /// <summary>
            /// 菜品简介
            /// </summary>
            public string Brief { get; set; }
            /// <summary>
            /// 菜品状态1.已售空2.未售空
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 菜品类型1.大荤2.小炒3.素菜4.套餐
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 时间1.本周2.下周
            /// </summary>
            public string Time { get; set; }
            /// <summary>
            /// 库存
            /// </summary>
            public int? Inventory { get; set; }
            /// <summary>
            /// 已订
            /// </summary>
            public int? Intended { get; set; }
            /// <summary>
            /// 限订
            /// </summary>
            public int? Restrict { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal? Price { get; set; }
        }

        /// <summary>
        ///  菜品列表
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

                    //默认门店账户
                    var query_s = from id in context.B_Store
                                  where id.UID == storeid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                    var query = from am in context.TC_Food
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_foodResponse
                                {
                                    ID = am.ID,
                                    Date = am.Date,
                                    Menu = am.Menu,
                                    SetMeal = am.SetMeal,
                                    Brief = am.Brief,
                                    Status = am.Status,
                                    Type = am.Type,
                                    Time = am.Time,
                                    Inventory = am.Inventory,
                                    Intended = am.Intended,
                                    Restrict = am.Restrict,
                                    Price = am.Price,
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
                        query = from am in context.TC_Food
                                where am.IsEnable == true && am.StoreID == storeid
                                select new tc_foodResponse
                                {
                                    ID = am.ID,
                                    Date = am.Date,
                                    Menu = am.Menu,
                                    SetMeal = am.SetMeal,
                                    Brief = am.Brief,
                                    Status = am.Status,
                                    Type = am.Type,
                                    Time = am.Time,
                                    Inventory = am.Inventory,
                                    Intended = am.Intended,
                                    Restrict = am.Restrict,
                                    Price = am.Price,
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
                    //List<string> scoreList = new List<string>();
                    //tc_foodResponse tfr = new tc_foodResponse();
                    //Dictionary<string, string> dic = new Dictionary<string, string>();
                    //Dictionary<decimal?, string> dicprice = new Dictionary<decimal?, string>();
                    //dic.Add(tfr.Menu,"name");
                    //dicprice.Add(tfr.Price, "2");
                    //scoreList.Add(dic);
                    List<tc_foodResponse> lstFoodResponses = query.OrderByDescending(r => r.UpdateTime).ToList<tc_foodResponse>();
                    return new HandleResult { ResultCode = 0, Message = "查询成功", Data = lstFoodResponses };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }
    }
}