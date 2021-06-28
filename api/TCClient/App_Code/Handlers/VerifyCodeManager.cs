using Com.Caimomo.Common;
using Com.Caimomo.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using DAL;

namespace HrApp.TCClient.Handlers
{
    public class TcVerifyCodeManagementGetVerifySms : IAjaxHandler
    {
        /// <summary>
        /// 获取验证码
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


                    string strOpenID = "strOpenID";
                    string strPhoneNumber = queryString["PhoneNumber"];
                    string strMsg = "";

                    if (string.IsNullOrEmpty(strPhoneNumber)) { return new HandleResult { ResultCode = 1, Message = "手机号无效", Data = null }; }


                    //获取用户手机验证绑定状态
                    if (openidHasPhone(strOpenID, strPhoneNumber, ref strMsg))
                    {
                        //如果微信已绑定手机，则退出
                        return new HandleResult { ResultCode = 1, Message = strMsg, Data = null };
                    }
                    else
                    {
                        //如果微信未绑定手机，则生成验证码并发送
                        if (insertVerifyCode(strOpenID, strPhoneNumber, ref strMsg))
                        {
                            return new HandleResult { ResultCode = 0, Message = "获取验证码成功", Data = null };
                        }
                        else
                        {
                            return new HandleResult { ResultCode = 1, Message = "获取验证码出错", Data = null };
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("获取验证码出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "获取验证码出错:" + ex, Data = null };
            }
        }

        /// <summary>
        /// 判断微信是否已绑定手机
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private bool openidHasPhone(string openid, string strPhoneNumber, ref string strmsg)
        {
            //手机号不正确
            //手机号不存在
            //手机号存在但已绑定微信
            return false;
        }

        /// <summary>
        /// 生成并发送验证码
        /// </summary>
        /// <param name="strOpenid"></param>
        /// <param name="strPhoneNumber"></param>
        /// <param name="strmsg"></param>
        /// <returns></returns>
        private bool insertVerifyCode(string strOpenid, string strPhoneNumber, ref string strmsg)
        {
            try
            {
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    TC_VerifyCode tC_VerifyCode = new TC_VerifyCode();

                    tC_VerifyCode.ID = Guid.NewGuid().ToString();
                    tC_VerifyCode.ReceiverName = strPhoneNumber;
                    tC_VerifyCode.ReceiverType = "1";//手机
                    tC_VerifyCode.CodeSN = strOpenid;

                    Random rd = new Random();
                    int num = rd.Next(100000, 1000000);

                    tC_VerifyCode.VerifyCode = num.ToString();
                    tC_VerifyCode.IsUsed = "0";

                    tC_VerifyCode.AddTime = System.DateTime.Now;
                    //tC_VerifyCode.AddUserID = currentUser.UserID;
                    //tC_VerifyCode.AddUser = currentUser.UserName;
                    tC_VerifyCode.UpdateTime = tC_VerifyCode.AddTime;
                    tC_VerifyCode.UpdateUserID = tC_VerifyCode.AddUserID;
                    tC_VerifyCode.UpdateUser = tC_VerifyCode.AddUser;

                    tC_VerifyCode.SendTime = tC_VerifyCode.AddTime;

                    context.TC_VerifyCode.InsertOnSubmit(tC_VerifyCode);
                    context.SubmitChanges();

                    if (SmsTool.SendMsg(strPhoneNumber, num.ToString())) return true;
                    else return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("方法insertVerifyCode出错:" + ex);
                return false;
            }
        }

    }

    public class TcVerifyCodeManagementVerifySms : IAjaxHandler
    {
        public class TC_UserAuthsResponse
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
            /// 用户ID
            /// </summary>
            public string UserID { get; set; }
            /// <summary>
            /// 账户类型（phone,email,wechat,qq,weibo）
            /// </summary>
            public string IdentityType { get; set; }
            /// <summary>
            /// 账户名
            /// </summary>
            public string Identifier { get; set; }
            /// <summary>
            /// 凭证
            /// </summary>
            public string Credential { get; set; }
            /// <summary>
            /// 最近登录时间
            /// </summary>
            public DateTime LastLoginTime { get; set; }
            /// <summary>
            /// 验证时间
            /// </summary>
            public DateTime VerifiedTime { get; set; }
        }

        public class TC_VerifyCodeResponse
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
            /// 接收端名称（手机或邮箱）
            /// </summary>
            public string ReceiverName { get; set; }
            /// <summary>
            /// 接收端类型（sms, email）
            /// </summary>
            public string ReceiverType { get; set; }
            /// <summary>
            /// 短信序号
            /// </summary>
            public string CodeSN { get; set; }
            /// <summary>
            /// 验证码
            /// </summary>
            public string VerifyCode { get; set; }
            /// <summary>
            /// 是否已使用
            /// </summary>
            public string IsUsed { get; set; }
            /// <summary>
            /// 调用方法名称（'register、retrieve、change'）
            /// </summary>
            public string MethodName { get; set; }
            /// <summary>
            /// 发送时间
            /// </summary>
            public DateTime SendTime { get; set; }
            /// <summary>
            /// 使用时间
            /// </summary>
            public DateTime UsedTime { get; set; }
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
        /// 校验验证码
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
                    string strOpenID = "strOpenID";
                    string strPhoneNumber = queryString["PhoneNumber"];
                    string strVerifyCode = queryString["VerifyCode"];
                    string strMsg = "";


                    var query = from vc in context.TC_VerifyCode
                                where vc.CodeSN == strOpenID && vc.ReceiverName == strPhoneNumber && vc.VerifyCode == strVerifyCode && vc.IsUsed == "0"
                                select new TC_VerifyCodeResponse
                                {
                                    ID = vc.ID.ToString(),
                                    SendTime = (DateTime)vc.SendTime,

                                };
                    List<TC_VerifyCodeResponse> lstTC_VerifyCodeResponse = query.OrderByDescending(r => r.SendTime).ToList<TC_VerifyCodeResponse>();
                    if (lstTC_VerifyCodeResponse.Count > 0) {

                        string strUserID = getUserID(strPhoneNumber,ref strMsg);
                        if (!string.IsNullOrEmpty(strUserID))
                        {
                            TC_VerifyCode tC_VerifyCode = context.TC_VerifyCode.SingleOrDefault(g => g.ID == lstTC_VerifyCodeResponse[0].ID);
                            tC_VerifyCode.IsUsed = "1";
                            context.SubmitChanges();

                            #region 删除后新增
                            var queryUserAuths = from ua in context.TC_UserAuths
                                                 where ua.UserID == strUserID && ua.Credential == strPhoneNumber && ua.Identifier == strOpenID
                                                 select new TC_UserAuthsResponse
                                                 {
                                                     ID = ua.ID
                                                 };
                            List<TC_UserAuthsResponse> lstTC_UserAuthsResponse = queryUserAuths.ToList<TC_UserAuthsResponse>();
                            //删除
                            for (var i = 0; i < lstTC_UserAuthsResponse.Count(); i++)
                            {
                                if (lstTC_UserAuthsResponse[i].ID != "")
                                {
                                    TC_UserAuths deltC_UserAuths = context.TC_UserAuths.SingleOrDefault(g => g.ID == lstTC_UserAuthsResponse[i].ID);
                                    context.TC_UserAuths.DeleteOnSubmit(deltC_UserAuths);
                                }
                            }
                            context.SubmitChanges();

                            TC_UserAuths tC_UserAuths = new TC_UserAuths();
                            tC_UserAuths.ID = Guid.NewGuid().ToString();
                            tC_UserAuths.UserID = strUserID;
                            tC_UserAuths.Credential = strPhoneNumber;
                            tC_UserAuths.Identifier = strOpenID;
                            tC_UserAuths.Verified = true;
                            tC_UserAuths.IdentityType = "1";//手机短信验证
                            tC_UserAuths.AddTime = System.DateTime.Now;
                            //tC_UserAuths.AddUserID = currentUser.UserID;
                            //tC_UserAuths.AddUser = currentUser.UserName;
                            tC_UserAuths.UpdateTime = tC_UserAuths.AddTime;
                            tC_UserAuths.UpdateUserID = tC_UserAuths.AddUserID;
                            tC_UserAuths.UpdateUser = tC_UserAuths.AddUser;
                            tC_UserAuths.VerifiedTime = tC_UserAuths.UpdateTime;

                            context.TC_UserAuths.InsertOnSubmit(tC_UserAuths);
                            context.SubmitChanges();

                            #endregion


                            return new HandleResult { ResultCode = 0, Message = "校验验证码成功", Data = null };
                        }
                        else
                        {
                            return new HandleResult { ResultCode = 1, Message = strMsg, Data = null };
                        }
                    }
                    else
                    {
                        return new HandleResult { ResultCode = 1, Message = "校验验证码失败", Data = null };
                    }

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("校验验证码出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "校验验证码出错:" + ex, Data = null };
            }
        }

        private string getUserID(string strPhoneNumber,ref string strMsg)
        {
            try
            {
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    var query = from a in context.TC_AccountManagement
                                where a.IsEnable==true && a.Phone == strPhoneNumber
                                select new tc_accountManagementResponse
                                {
                                    ID = a.ID.ToString(),
                                    UpdateTime = (DateTime)a.UpdateTime,
                                };
                    List<tc_accountManagementResponse> lsttc_accountManagementResponse = query.OrderByDescending(r => r.UpdateTime).ToList<tc_accountManagementResponse>();
                    if (lsttc_accountManagementResponse.Count > 0)
                    {
                        return lsttc_accountManagementResponse[0].ID;
                    }
                    else
                    {
                        strMsg = "该手机号用户未在后台配置";
                        return "";
                    }

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("方法getUserID出错:" + ex);
                strMsg = "获取用户ID出错";
                return "";
            }
        }
    }
}