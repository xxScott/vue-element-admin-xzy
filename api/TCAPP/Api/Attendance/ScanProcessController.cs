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
using System.Data.SqlClient;
namespace HrApp.Api.Attendance
{
    public class ScanProcessController : Controller
    {
        public JsonResult SendSMS(string tel)
        {

            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {


            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult SignIn(string tel, string companyCode, int scanType)
        {

            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("MobilePhone", tel));
                condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));

                //获取员工信息
                EmployeeBase empBase = service.SearchOne(condition);
                if (empBase != null)
                {
                    //获取公司考勤设置
                    List<AttendParam> list = ServiceFactory.Factory.AttendParamService.Search(null);
                    List<AttendParam> attendParams = list.Where(p => p.GroupID == empBase.GroupID).ToList();

                    AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                    AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                    AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                    AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));
                    if (server != null && userId != null && userPwd != null && storage != null)
                    {
                        string CatalogName = string.Empty; int n = 0; string sql = string.Empty;
                        companyCode = companyCode.Replace("-", "");

                        //判断数据保存类型 - 年
                        if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                        {
                            CatalogName = "CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year;
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
                      + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + "'";
                            SqlCommand comd = new SqlCommand(sql, conn);
                            SqlDataReader sda = comd.ExecuteReader();
                            if (sda.Read())
                                n = Convert.ToInt32(sda["Count"].ToString());
                            sda.Close();
                            if (n > 0)
                            {

                                sql = "insert Scan_" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + "(CompanyID,DeptID,EmployeeID,ScanTime,ScanType) values(" + empBase.GroupID + "," + empBase.DeptID + "," + empBase.ID + ",'" + DateTime.Now + "'," + scanType + ")";
                                comd = new SqlCommand(sql, conn);
                                n = comd.ExecuteNonQuery();
                                JObject obj = new JObject();

                                obj["message"] = "签到成功";
                                obj["scanTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                obj["scanType"] = DAL.Util.GetDisplayName((Enum_StartOrOver)scanType);
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