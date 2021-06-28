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
    public class TcOrderManagerDelete : IAjaxHandler
    {
        /// <summary>
        ///  订单删除
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

                    TC_Order tc_Order = context.TC_Order.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_Order, null))
                    {
                        tc_Order.IsEnable = false;
                        tc_Order.UpdateTime = System.DateTime.Now;
                        tc_Order.UpdateUserID = currentUser.UserID;
                        tc_Order.UpdateUser = currentUser.UserName;
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

    public class TcOrderManagerQueryList : IAjaxHandler
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

        public class TC_OrderResponse
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
            /// 订单编号
            /// </summary>
            public string OrderNumber { get; set; }
            /// <summary>
            /// 订单价格
            /// </summary>
            public decimal? OrderPrice { get; set; }
            /// <summary>
            /// 取餐状态0.未取餐1.已取餐
            /// </summary>
            public string OrderStatus { get; set; }
            /// <summary>
            /// 二维码
            /// </summary>
            public string ThinkChange { get; set; }
            /// <summary>
            /// 人员id
            /// </summary>
            public string AccountManagementID { get; set; }
            /// <summary>
            /// 订单支付状态1.已支付2.未支付
            /// </summary>
            public string FetchStatus { get; set; }
        }


        /// <summary>
        ///  订单列表
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
                    string ID = queryString["ID"];
                    string OrderNumber = queryString["OrderNumber"];

                    #region 获取该账号下管理的门店列表，并获取对应业务数据列表

                    //默认为门店账户
                    var query_s = from id in context.B_Store
                                  where id.UID == storeid
                                  select new StoreResponse
                                  {
                                      UID = id.UID.ToString(),
                                      StoreName = id.StoreName,
                                  };
                    var query = from am in context.TC_Order
                                where am.IsEnable == true && am.StoreID == storeid
                                select new TC_OrderResponse
                                {
                                    ID = am.ID,
                                    OrderNumber = am.OrderNumber,
                                    OrderPrice = am.OrderPrice,
                                    OrderStatus = am.OrderStatus,
                                    ThinkChange = am.ThinkChange,
                                    AccountManagementID = am.AccountManagementID,
                                    FetchStatus = am.FetchStatus,
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
                        query = from am in context.TC_Order
                                where am.IsEnable == true && am.GroupID == groudid
                                select new TC_OrderResponse
                                {
                                    ID = am.ID,
                                    OrderNumber = am.OrderNumber,
                                    OrderPrice = am.OrderPrice,
                                    OrderStatus = am.OrderStatus,
                                    ThinkChange = am.ThinkChange,
                                    AccountManagementID = am.AccountManagementID,
                                    FetchStatus = am.FetchStatus,
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
                    if (!string.IsNullOrEmpty(ID)) query = query.Where(g => g.ID == ID);
                    if (!string.IsNullOrEmpty(OrderNumber)) query = query.Where(g => g.OrderNumber == OrderNumber);

                    int pageCount = 0;
                    if (pageNumber > 0)
                    {
                        pageCount = query.ToList().Count;
                    }

                    List<TC_OrderResponse> lstOrderResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<TC_OrderResponse>();
                    return new MyHandleResult { ResultCode = 0, Message = "查询成功",total_count = pageCount,  Data = lstOrderResponse };
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog("查询出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "查询出错:" + ex, Data = null };
            }
        }
    }

    public class TcOrderManagerAdd : IAjaxHandler
    {
        public class TC_OrderSource
        {
            /// <summary>
            /// 订单编号
            /// </summary>
            public string OrderNumber { get; set; }
            /// <summary>
            /// 订单价格
            /// </summary>
            public decimal? OrderPrice { get; set; }
            /// <summary>
            /// 订单状态
            /// </summary>
            public string OrderStatus { get; set; }
            /// <summary>
            /// 二维码
            /// </summary>
            public string ThinkChange { get; set; }
        }

        /// <summary>
        ///  订单新增
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
                        TC_OrderSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_OrderSource>(postContent);

                        TC_Order tc_order = new TC_Order();
                        tc_order.ID = Guid.NewGuid().ToString();
                        tc_order.OrderNumber = source.OrderNumber;
                        tc_order.OrderPrice = source.OrderPrice;
                        tc_order.OrderStatus = source.OrderStatus;
                        tc_order.ThinkChange = source.ThinkChange;
                        tc_order.IsEnable = true;
                        tc_order.StoreID = currentUser.StoreID;
                        tc_order.AddTime = System.DateTime.Now;
                        tc_order.AddUserID = currentUser.UserID;
                        tc_order.AddUser = currentUser.UserName;
                        tc_order.UpdateTime = tc_order.AddTime;
                        tc_order.UpdateUserID = tc_order.AddUserID;
                        tc_order.UpdateUser = tc_order.AddUser;

                        context.TC_Order.InsertOnSubmit(tc_order);
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

    public class TcOrderManagerUpdate : IAjaxHandler
    {
        public class TC_OrderSource
        {
            /// <summary>
            /// 唯一编号
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// 订单编号
            /// </summary>
            public string OrderNumber { get; set; }
            /// <summary>
            /// 订单价格
            /// </summary>
            public decimal? OrderPrice { get; set; }
            /// <summary>
            /// 订单状态
            /// </summary>
            public string OrderStatus { get; set; }
            /// <summary>
            /// 二维码
            /// </summary>
            public string ThinkChange { get; set; }
            /// <summary>
            /// 人员id
            /// </summary>
            public string AccountManagementID { get; set; }
            /// <summary>
            /// 取餐状态1.已取餐2.未取餐
            /// </summary>
            public string FetchStatus { get; set; }
        }

        /// <summary>
        ///  订单修改
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

                    TC_OrderSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_OrderSource>(postContent);

                    #region 更新Inspection_DepartmentConfig

                    TC_Order tc_order = context.TC_Order.SingleOrDefault(g => g.ID == source.ID); ;

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        if (!string.IsNullOrEmpty(source.OrderNumber))
                        {
                            tc_order.OrderNumber = source.OrderNumber;
                        }
                        if (!string.IsNullOrEmpty(source.ThinkChange))
                        {
                            tc_order.ThinkChange = source.ThinkChange;
                        }
                        if (!string.IsNullOrEmpty(source.OrderStatus))
                        {
                            tc_order.OrderStatus = source.OrderStatus;
                        }
                        if (!string.IsNullOrEmpty(source.AccountManagementID))
                        {
                            tc_order.AccountManagementID = source.AccountManagementID;
                        }
                        if (!string.IsNullOrEmpty(source.FetchStatus))
                        {
                            tc_order.FetchStatus = source.FetchStatus;
                        }
                        tc_order.OrderPrice = source.OrderPrice;
                        tc_order.UpdateTime = System.DateTime.Now;
                        tc_order.UpdateUserID = currentUser.UserID;
                        tc_order.UpdateUser = currentUser.UserName;
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

    public class TcOrderManagerQRRead : IAjaxHandler
    {
        /// <summary>
        /// 取餐（订单扫码）
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
                    //Users currentUser = Utilities.Security.CurrentUser;
                    //if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;

                    int? storeid = 100001;
                    int? groudid = 1000;

                    TC_Order tc_Order = context.TC_Order.SingleOrDefault(g => g.ID == queryString["ID"]);

                    if (!object.Equals(tc_Order, null))
                    {
                        if (tc_Order.OrderStatus == "0")
                        {
                            tc_Order.OrderStatus = "1";//已扫码
                            context.SubmitChanges();

                            return new HandleResult { ResultCode = 0, Message = "取餐成功", Data = null };
                        }
                        else
                        {
                            return new HandleResult { ResultCode = 1, Message = "订单状态异常", Data = null };
                        }

                    }
                    else
                    {
                        return new HandleResult { ResultCode = 1, Message = "订单不存在", Data = null };
                    }

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("取餐出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "取餐出错:" + ex, Data = null };
            }
        }
    }

    ///// <summary>
    ///// QRDemoHandler 的摘要说明
    ///// </summary>
    //public class QRReadDemoHandler : IAjaxHandler
    //{
    //    public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
    //    {

    //        //地址：http://localhost:49799/AjaxHandler.ashx?methodName=QRReadDemoHandler
    //        string result = Read(@"D:\generate1.png");
    //        return new HandleResult { ResultCode = 0, Message = result, Data = null };


    //    }


    //    /// <summary>
    //    /// 读取二维码
    //    /// 读取失败，返回空字符串
    //    /// </summary>
    //    /// <param name="filename">指定二维码图片位置</param>
    //    static string Read(string filename)
    //    {
    //        BarcodeReader reader = new BarcodeReader();
    //        reader.Options.CharacterSet = "UTF-8";
    //        Bitmap map = new Bitmap(filename);
    //        Result result = reader.Decode(map);
    //        return result == null ? "" : result.Text;
    //    }
    //}
}