using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Newtonsoft.Json.Linq;
using Utilities;
using Adapters;
using System.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.ComponentModel;

namespace HrApp.Api
{
    public class BarCodeController : Controller
    {
        const string URL_Sign = "/Pages/Attendance/ScanProcessForm.aspx";


        /// <summary>
        /// 生成上班打卡二维码
        /// </summary>
        /// <param name="companyCode">公司代码</param>
        /// <param name="deptCode">部门代码</param>
        /// <returns>二维码数据对象</returns>
        public JsonResult GenStartCode(string uid, string deptCode)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {

                Stores store = ServiceFactory.Factory.StoresService.SearchOne(new SimpleCondition("UID", uid));
                if (store == null)
                {
                    json.Data = JsonUtil.GetFailForString("门店UID错误");
                }
                else
                {
                    ConditionSet condition = new ConditionSet();
                    if (store.GroupID != -1)
                    {
                        condition.Add(new SimpleCondition("GroupID", store.GroupID));
                        condition.Add(new SimpleCondition("StoreID", -1));
                    }
                    else
                    {
                        condition.Add(new SimpleCondition("GroupID", -1));
                        condition.Add(new SimpleCondition("StoreID", store.ID));
                    }

                    BarCode barCode = ServiceFactory.Factory.BarCodeService.SearchOne(new ConditionSet() { new SimpleCondition("StoreID", store.ID),  new SimpleCondition("StartOrOver", (int)DAL.Enum_StartOrOver.Start) });
                    if (barCode == null)
                    {
                        var barCodeData = new { StoreUID = store.UID,  StartOrOver = (int)DAL.Enum_StartOrOver.Start, RandomCode = Utilities.Char.GetRandomCode(8) };

                        barCode = new BarCode();
                        barCode.StoreID = store.ID;
                        barCode.StartOrOver = barCodeData.StartOrOver;
                        barCode.RandomCode = barCodeData.RandomCode;
                        barCode.BarCodeData = this.Request.Url.AbsoluteUri.Remove(this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath), this.Request.Url.ToString().Length - this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath)) + URL_Sign + "?token=" + Utilities.Encrypt.Instance.EncryptString(JsonConvert.SerializeObject(barCodeData));
                        ServiceFactory.Factory.BarCodeService.Insert(barCode);
                    }
                    else
                    {
                        var barCodeData = new { StoreUID = store.UID,StartOrOver = (int)DAL.Enum_StartOrOver.Start, RandomCode = Utilities.Char.GetRandomCode(8) };


                        barCode.StoreID = store.ID;
                        barCode.StartOrOver = barCodeData.StartOrOver;
                        barCode.RandomCode = barCodeData.RandomCode;
                        barCode.BarCodeData = this.Request.Url.AbsoluteUri.Remove(this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath), this.Request.Url.ToString().Length - this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath)) + URL_Sign + "?token=" + Utilities.Encrypt.Instance.EncryptString(JsonConvert.SerializeObject(barCodeData));
                        ServiceFactory.Factory.BarCodeService.Update(barCode);
                    }
                    json.Data = JsonUtil.GetSuccessForString(barCode.BarCodeData);
                }

            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult GenOverCode(string uid, string deptCode)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                Stores store = ServiceFactory.Factory.StoresService.SearchOne(new SimpleCondition("UID", uid));
                if (store == null)
                {
                    json.Data = JsonUtil.GetFailForString("门店UID错误");
                }
                else
                {

                    ConditionSet condition = new ConditionSet();
                    if (store.GroupID != -1)
                    {
                        condition.Add(new SimpleCondition("GroupID", store.GroupID));
                        condition.Add(new SimpleCondition("StoreID", -1));
                    }
                    else
                    {
                        condition.Add(new SimpleCondition("GroupID", -1));
                        condition.Add(new SimpleCondition("StoreID", store.ID));
                    }
                    condition.Add(new SimpleCondition("DeptCode", deptCode));
                    Depts dept = ServiceFactory.Factory.DeptsService.SearchOne(condition);
                    if (dept == null)
                    {
                        json.Data = JsonUtil.GetFailForString("部门代码错误");
                    }
                    else
                    {
                        BarCode barCode = ServiceFactory.Factory.BarCodeService.SearchOne(new ConditionSet() { new SimpleCondition("StoreID", store.ID), new SimpleCondition("DeptID", dept.ID), new SimpleCondition("StartOrOver", (int)DAL.Enum_StartOrOver.Over) });
                        if (barCode == null)
                        {
                            Models.Data.BarCodeDataView barCodeData = new Models.Data.BarCodeDataView() { StoreUID = store.UID.ToString(), DeptCode = dept.DeptCode, StartOrOver = (int)DAL.Enum_StartOrOver.Over, RandomCode = Utilities.Char.GetRandomCode(8) };

                            barCode = new BarCode();

                            barCode.StoreID = store.ID;
                            barCode.DeptID = dept.ID;
                            barCode.StartOrOver = barCodeData.StartOrOver;
                            barCode.RandomCode = barCodeData.RandomCode;
                            barCode.BarCodeData = this.Request.Url.AbsoluteUri.Remove(this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath), this.Request.Url.ToString().Length - this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath)) + URL_Sign + "?token=" + Utilities.Encrypt.Instance.EncryptString(JsonConvert.SerializeObject(barCodeData));
                            ServiceFactory.Factory.BarCodeService.Insert(barCode);
                        }
                        else
                        {
                            Models.Data.BarCodeDataView barCodeData = new Models.Data.BarCodeDataView() { StoreUID = store.UID.ToString(), DeptCode = dept.DeptCode, StartOrOver = (int)DAL.Enum_StartOrOver.Over, RandomCode = Utilities.Char.GetRandomCode(8) };


                            barCode.StoreID = store.ID;
                            barCode.DeptID = dept.ID;
                            barCode.StartOrOver = barCodeData.StartOrOver;
                            barCode.RandomCode = barCodeData.RandomCode;
                            barCode.BarCodeData = this.Request.Url.AbsoluteUri.Remove(this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath), this.Request.Url.ToString().Length - this.Request.Url.ToString().IndexOf(this.Request.Url.AbsolutePath)) + URL_Sign + "?token=" + Utilities.Encrypt.Instance.EncryptString(JsonConvert.SerializeObject(barCodeData));
                            ServiceFactory.Factory.BarCodeService.Update(barCode);
                        }
                        json.Data = JsonUtil.GetSuccessForString(barCode.BarCodeData);
                    }
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobilePhone">手机号码</param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public JsonResult SendVerifyCode(string mobilePhone)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IVerifyCodesService service = ServiceFactory.Factory.VerifyCodesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("MobilePhone", mobilePhone));
                VerifyCodes old_entity = service.SearchOne(condition);

                if (old_entity != null)
                {
                    if (DateTime.Parse(old_entity.ExpiredTime.ToString()).AddMinutes(-4) > DateTime.Now)
                        throw new Exception("两次发送时间小于1分钟");
                }
                string message = string.Empty;
                string verifyCode = Utilities.Char.GetRandomCode(6);
                if (!new SMS().SendByChuangLan(mobilePhone, verifyCode, ref message))
                {
                    json.Data = JsonUtil.GetFailForString("验证码发送失败，" + message);
                }
                else
                {
                    if (old_entity != null)
                    {
                        old_entity.VerifyCode = verifyCode;
                        old_entity.ExpiredTime = DateTime.Now.AddMinutes(5);
                        old_entity.State = (int)Enum_VerifyCodeState.Unused;
                        ServiceFactory.Factory.VerifyCodesService.Update(old_entity);
                    }
                    else
                    {
                        VerifyCodes entity = new VerifyCodes();
                        entity.MobilePhone = mobilePhone;
                        entity.VerifyCode = verifyCode;
                        entity.ExpiredTime = DateTime.Now.AddMinutes(5);
                        entity.State = (int)Enum_VerifyCodeState.Unused;
                        ServiceFactory.Factory.VerifyCodesService.Insert(entity);

                    }
                    json.Data = JsonUtil.GetSuccess();
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);
            }
            return json;
        }


        public JsonResult CompareVerifyCode(string mobilePhone, string verifyCode)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IVerifyCodesService service = ServiceFactory.Factory.VerifyCodesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_VerifyCodeState.Unused)));
                condition.Add(new SimpleCondition("MobilePhone", mobilePhone));
                VerifyCodes entity = service.SearchOne(condition);

                if (entity != null)
                {
                    if (DateTime.Parse(entity.ExpiredTime.ToString()) < DateTime.Now)
                    {
                        json.Data = JsonUtil.GetFailForString("该验证码已失效，请点击重新获取");
                    }
                    else
                    {
                        if (entity.VerifyCode.Trim() != verifyCode) json.Data = json.Data = JsonUtil.GetFailForString("输入验证码错误");
                        else
                        {
                            entity.State = (int)Enum_VerifyCodeState.Used;
                            service.Update(entity);
                            json.Data = JsonUtil.GetSuccess();
                        }
                    }
                }
                else
                {
                    json.Data = JsonUtil.GetFailForString("该验证码已失效，请点击重新获取");
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);
            }
            return json;
        }

        /// <summary>
        /// 扫描二维码
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="companyCode"></param>
        /// <param name="deptCode"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonResult ScanCode(string token, string mobilePhone)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                string data = Utilities.Encrypt.Instance.DecryptString(token);
                Models.Data.BarCodeDataView barCodeData = (Models.Data.BarCodeDataView)JsonConvert.DeserializeObject(data, typeof(Models.Data.BarCodeDataView));
                Stores store = ServiceFactory.Factory.StoresService.SearchOne(new SimpleCondition("UID", barCodeData.StoreUID));
                if (store == null) throw new Exception("未找到签到门店信息");

                Groups group = ServiceFactory.Factory.GroupsService.SearchOne(new SimpleCondition("ID", store.GroupID));



                ConditionSet conditionBar = new ConditionSet();
                conditionBar.Add(new SimpleCondition("StoreID", store.ID));
                conditionBar.Add(new SimpleCondition("StartOrOver", barCodeData.StartOrOver));
                BarCode barCode = ServiceFactory.Factory.BarCodeService.SearchOne(conditionBar);
                if (barCode == null)
                {
                    json.Data = JsonUtil.GetFailForString("二维码非法");
                }
                else
                {
                    //校验随机码
                    if (barCodeData.RandomCode.Trim() != barCode.RandomCode.Trim()) throw new Exception("二维码已失效");

                    IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                    ConditionSet condition = new ConditionSet();
                    condition.Add(new SimpleCondition("MobilePhone", mobilePhone));
                    condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));

                    //获取员工信息
                    EmployeeBase empBase = service.SearchOne(condition);
                    if (empBase != null)
                    {
                        //校验员工签到二维码信息
                        if (empBase.StoreID != barCode.StoreID) throw new Exception("员工对应签到门店错误");


                        //获取公司考勤设置
                        string sql = string.Empty, companyCode = string.Empty, CatalogName = string.Empty; int n = 0;
                        List<AttendParam> list = ServiceFactory.Factory.AttendParamService.Search(null);
                        List<AttendParam> attendParams = new List<AttendParam>();
                        ConditionSet conditionP = new ConditionSet();
                        if (group != null)
                        {
                            conditionP.Add(new SimpleCondition("GroupID", group.ID));
                            conditionP.Add(new SimpleCondition("StoreID", -1));

                            attendParams = list.Where(p => p.GroupID == group.ID).ToList();
                            companyCode = group.UID.ToString();
                        }
                        else
                        {
                            conditionP.Add(new SimpleCondition("GroupID", -1));
                            conditionP.Add(new SimpleCondition("StoreID", store.ID));

                            attendParams = list.Where(p => p.StoreID == store.ID).ToList();
                            companyCode = "DMD";
                        }
                        //获取员工签到对应期间的月、年
                        List<Periods> period = ServiceFactory.Factory.PeriodService.Search(conditionP);
                        string Month = string.Empty, Year = string.Empty;
                        for (int i = 0; i < period.Count; i++)
                        {
                            if (DateTime.Compare(Convert.ToDateTime(period[i].StartDate), DateTime.Now) < 0 && DateTime.Compare(Convert.ToDateTime(period[i].EndDate), DateTime.Now) > 0)
                            {
                                Month = Convert.ToDateTime(period[i].StartDate).Month.ToString("00");
                                Year = Convert.ToDateTime(period[i].StartDate).Year.ToString();
                            }
                        }


                        AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                        AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                        AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                        AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));

                        if (server != null && userId != null && userPwd != null && storage != null)
                        {
                            //判断数据保存类型 - 年
                            if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                            {
                                CatalogName = "CaiMoMo_HR_" + companyCode + "_" + Year;
                            }
                            else
                            {
                                throw new Exception("数据保存类型错误");
                            }
                            //数据库不存在则抛异常
                            using (SqlConnection conn = new SqlConnection("Data Source=" + server.ParamValue + ";Initial Catalog=" + CatalogName + ";User ID=" + userId.ParamValue + ";Password=" + userPwd.ParamValue))
                            {
                                conn.Open();
                                //判断对应表是否存在
                                sql = " use " + CatalogName + " "
                          + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + store.UID + "_" + Year + Month + "'";
                                SqlCommand comd = new SqlCommand(sql, conn);
                                SqlDataReader sda = comd.ExecuteReader();
                                if (sda.Read())
                                    n = Convert.ToInt32(sda["Count"].ToString());
                                sda.Close();
                                if (n > 0)
                                {
                                    sql = "insert Scan_" + store.UID + "_" + Year + Month + "(StoreID,DeptID,EmployeeID,ScanTime,ScanType) values(" + barCode.StoreID + "," + barCode.DeptID + "," + empBase.ID + ",'" + DateTime.Now + "'," + barCode.StartOrOver + ")";
                                    comd = new SqlCommand(sql, conn);
                                    n = comd.ExecuteNonQuery();
                                    JObject obj = new JObject();

                                    obj["message"] = "签到成功";
                                    obj["scanTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    obj["scanType"] = DAL.Util.GetDisplayName((Enum_StartOrOver)barCode.StartOrOver);
                                    obj["employeeName"] = empBase.EmployeeName;
                                    if (n > 0)
                                    {
                                        json.Data = JsonUtil.GetSuccessForObject(obj);
                                    }
                                    else
                                    {
                                        json.Data = JsonUtil.GetFailForString("签到失败");
                                    }
                                }
                                conn.Close();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

    }
}