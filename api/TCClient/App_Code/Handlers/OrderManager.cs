using Com.Caimomo.Common;
using Com.Caimomo.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using DAL;
using ZXing;
using ZXing.QrCode;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace HrApp.TCClient.Handlers
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
                    //Users currentUser = Utilities.Security.CurrentUser;
                    //if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;

                    int? storeid = 100001;
                    int? groudid = 1000;

                    TC_Order tc_Order = context.TC_Order.SingleOrDefault(g => g.ID == queryString["ID"]);
                    if (!object.Equals(tc_Order, null))
                    {
                        tc_Order.IsEnable = false;
                        tc_Order.UpdateTime = System.DateTime.Now;
                        //tc_Order.UpdateUserID = currentUser.UserID;
                        //tc_Order.UpdateUser = currentUser.UserName;
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
            /// 订单状态
            /// </summary>
            public string OrderStatus { get; set; }
            /// <summary>
            /// 二维码
            /// </summary>
            public string ThinkChange { get; set; }
            /// <summary>
            /// 是否取餐
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
                    //SysGroupUser currentUser = Security.CurrentUser;
                    //if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;


                    int? storeid = 100001;
                    int? groudid = 1000;


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
                    var query = from am in context.TC_Order
                                where am.IsEnable == true && am.StoreID == storeid
                                select new TC_OrderResponse
                                {
                                    ID = am.ID,
                                    OrderNumber = am.OrderNumber,
                                    OrderPrice = am.OrderPrice,
                                    OrderStatus = am.OrderStatus,
                                    ThinkChange = am.ThinkChange,
                                    AddTime = am.AddTime,
                                    AddUserID = am.AddUserID,
                                    AddUser = am.AddUser,
                                    UpdateTime = am.UpdateTime,
                                    UpdateUserID = am.UpdateUserID,
                                    UpdateUser = am.UpdateUser,
                                    FetchStatus = am.FetchStatus
                                };
                    
                    //集团00账户，则重新获取
                    //if (Utilities.Security.HaveGroupAccount && Utilities.Security.IsGroupAccount)
                    //{
                    //    query_s = from id in context.B_Store
                    //              where id.GroupID == groudid
                    //              select new StoreResponse
                    //              {
                    //                  UID = id.UID.ToString(),
                    //                  StoreName = id.StoreName,
                    //              };
                    //    query = from am in context.TC_Order
                    //            where am.IsEnable == true && am.GroupID == groudid
                    //            select new TC_OrderResponse
                    //            {
                    //                ID = am.ID,
                    //                OrderNumber = am.OrderNumber,
                    //                OrderPrice = am.OrderPrice,
                    //                OrderStatus = am.OrderStatus,
                    //                ThinkChange = am.ThinkChange,
                    //                AddTime = am.AddTime,
                    //                AddUserID = am.AddUserID,
                    //                AddUser = am.AddUser,
                    //                UpdateTime = am.UpdateTime,
                    //                UpdateUserID = am.UpdateUserID,
                    //                UpdateUser = am.UpdateUser,
                    //            };
                    //}

                    //该账号下管理的门店列表
                    List<StoreResponse> lstStoreResponse = query_s.OrderByDescending(r => r.UID).ToList<StoreResponse>();

                    #endregion

                    int pageCount = 0;
                    if (pageNumber > 0)
                    {
                        pageCount = query.ToList().Count;
                    }

                    List<TC_OrderResponse> lstOrderResponse = query.OrderByDescending(r => r.UpdateTime).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList<TC_OrderResponse>();
                    return new HandleResult { ResultCode = 0, Message = "查询成功", Data = lstOrderResponse };
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

                    TC_AccountManagement currentUser = TCSecurity.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "请在微信客户端打开链接", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    if ( !TCSecurity.IsCurrentUserBindMoble ) return new HandleResult { ResultCode = 9, Message = "请去绑定手机", Data = null };

                    string url = "";
                    string strMsg = "";
                    string orderID = Guid.NewGuid().ToString();

                    //地址：http://localhost:49799/AjaxHandler.ashx?methodName=QRGenerate1DemoHandler


                    if (!string.IsNullOrEmpty(postContent))
                    {
                        TC_OrderSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_OrderSource>(postContent);

                        if (generateQR(orderID, storeid.ToString(), groudid.ToString(), ref url, ref strMsg))
                        {
                            TC_Order tC_Order = new TC_Order();
                            tC_Order.ID = orderID;
                            tC_Order.ThinkChange = url;
                            tC_Order.OrderNumber = source.OrderNumber;
                            tC_Order.OrderPrice = source.OrderPrice;
                            tC_Order.OrderStatus = "0";//新生成订单
                            tC_Order.IsEnable = true;
                            //tC_Order.StoreID = currentUser.StoreID;
                            tC_Order.AddTime = System.DateTime.Now;
                            //tC_Order.AddUserID = currentUser.UserID;
                            //tC_Order.AddUser = currentUser.UserName;
                            tC_Order.UpdateTime = tC_Order.AddTime;
                            tC_Order.UpdateUserID = tC_Order.AddUserID;
                            tC_Order.UpdateUser = tC_Order.AddUser;

                            context.TC_Order.InsertOnSubmit(tC_Order);
                            context.SubmitChanges();

                            return new HandleResult { ResultCode = 0, Message = "生成订单成功", Data = url };
                        }
                        else
                        {
                            return new HandleResult { ResultCode = 1, Message = strMsg, Data = null };
                        }

                    }
                    else
                    {
                        return new HandleResult { ResultCode = 0, Message = "生成订单出错", Data = null };
                    }


                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("生成订单出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "生成订单出错:" + ex, Data = null };
            }
        }

        /// <summary>
        /// 生成二维码,保存成图片
        /// </summary>
        private bool generateQR(string text, string storeid, string groudid, ref string url, ref string strmsg)
        {
            try
            {
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                QrCodeEncodingOptions options = new QrCodeEncodingOptions();
                options.DisableECI = true;
                //设置内容编码
                options.CharacterSet = "UTF-8";
                //设置二维码的宽度和高度
                options.Width = 500;
                options.Height = 500;
                //设置二维码的边距,单位不是固定像素
                options.Margin = 1;
                writer.Options = options;

                if (!string.IsNullOrEmpty(text))
                {
                    Bitmap map = writer.Write(text);
                    string save_url = "/Uploads/" + groudid + "/" + storeid + "/Tc/" + DateTime.Now.ToString("yyyyMMdd");
                    string save_path = System.Web.HttpContext.Current.Server.MapPath(save_url);
                    if (!Directory.Exists(save_path)) Directory.CreateDirectory(save_path);
                    string file_name = DateTime.Now.ToString("HHmmssms") + "_" + "oqr.png";
                    url = save_url + "/" + file_name;
                    string save_name = System.Web.HttpContext.Current.Server.MapPath(url);
                    if (System.IO.File.Exists(save_name)) System.IO.File.Delete(save_name);
                    map.Save(save_name, ImageFormat.Png);
                    map.Dispose();

                    strmsg = "生成二维码图片成功";
                    return true;
                }
                else
                {
                    strmsg = "生成二维码图片出错";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("生成二维码图片出错:" + ex);
                strmsg = "生成二维码图片出错:" + ex;
                return false;
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
                    //Users currentUser = Utilities.Security.CurrentUser;
                    //if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;

                    int? storeid = 100001;
                    int? groudid = 1000;

                    TC_OrderSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_OrderSource>(postContent);

                    #region 更新Inspection_DepartmentConfig

                    TC_Order tc_order = context.TC_Order.SingleOrDefault(g => g.ID == source.ID);

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
                        tc_order.OrderPrice = source.OrderPrice;
                        tc_order.UpdateTime = System.DateTime.Now;
                        //tc_order.UpdateUserID = currentUser.UserID;
                        //tc_order.UpdateUser = currentUser.UserName;
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

    //目前只保存评分的测试接口
    public class TcOrderDetailedManagerComment : IAjaxHandler  
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

        public class TC_OrderDetailedSource
        {
            /// <summary>
            /// 订单评星
            /// </summary>
            public string Stars { get; set; }
            /// <summary>
            /// 关联订单表
            /// </summary>
            public string OrderID { get; set; }
        }

        /// <summary>
        ///  订单细节 -- 评分
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

                    TC_OrderDetailedSource source = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_OrderDetailedSource>(postContent);

                    #region 更新,如果存在OrderID相等，则删除，然后添加；如果不存在，添加///不会做删除.....                    

                    if (!string.IsNullOrEmpty(postContent))
                    {
                        TC_OrderDetailed tc_orderDetailed = Newtonsoft.Json.JsonConvert.DeserializeObject<TC_OrderDetailed>(postContent);

                        if (!string.IsNullOrEmpty(source.Stars))
                        {
                            TC_OrderDetailed tc_orderDetailedUpdate = context.TC_OrderDetailed.SingleOrDefault(g => g.OrderID == source.OrderID);

                            if (!object.Equals(tc_orderDetailedUpdate, null))
                            {
                                tc_orderDetailedUpdate.Stars = source.Stars;
                                tc_orderDetailedUpdate.UpdateTime = System.DateTime.Now;
                                context.SubmitChanges();
                            }
                            else
                            {
                                tc_orderDetailed.ID = Guid.NewGuid().ToString();
                                tc_orderDetailed.Stars = source.Stars;
                                tc_orderDetailed.OrderID = source.OrderID;

                                tc_orderDetailed.UpdateTime = System.DateTime.Now;
                                context.TC_OrderDetailed.InsertOnSubmit(tc_orderDetailed);
                                context.SubmitChanges();
                            } 
                        }
                    }
                    #endregion

                    return new HandleResult { ResultCode = 0, Message = "评论成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("评论出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "评论出错:" + ex, Data = null };
            }
        }
    }

    //根据id获取订单接口
    public class TcOrderManagerQueryListDetail : IAjaxHandler
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
            /// 订单状态
            /// </summary>
            public string OrderStatus { get; set; }
            /// <summary>
            /// 二维码
            /// </summary>
            public string ThinkChange { get; set; }
            /// <summary>
            /// 是否取餐
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
                    //SysGroupUser currentUser = Security.CurrentUser;
                    //if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;


                    int? storeid = 100001;
                    int? groudid = 1000;

                    TC_Order tc_Order = context.TC_Order.SingleOrDefault(g => g.ID == queryString["ID"]);
                   
                    return new HandleResult { ResultCode = 0, Message = "查询成功", Data = tc_Order };
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