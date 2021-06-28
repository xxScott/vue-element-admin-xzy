using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Utilities;


namespace HrApp.Api
{
    public class UnCheckController : Controller
    {
        public JsonResult Query(int storeId, int peroidId, string keyword, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryAssessService service = ServiceFactory.Factory.SalaryAssessService;
                int totalCount = 0;
                DataView dv = service.QueryAssess(storeId, Convert.ToInt16(Enum_AssessState.Pass), peroidId, keyword, pageNumber, pageSize, out totalCount).DefaultView;
                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("EmployeeCode", typeof(string));
                dat.Columns.Add("EmployeeName", typeof(string));
                dat.Columns.Add("IDNumber", typeof(string));
                dat.Columns.Add("DeptName", typeof(string));
                dat.Columns.Add("StationName", typeof(string));
                dat.Columns.Add("MobilePhone", typeof(string));
                dat.Columns.Add("UpdateTime", typeof(string));
                dat.Columns.Add("Sex", typeof(Int16));
                dat.Columns.Add("SexName", typeof(string));
                dat.Columns.Add("AssessID", typeof(int));
                dat.Columns.Add("AssessState", typeof(int));
                dat.Columns.Add("AssessStateName", typeof(string));
                dat.Columns.Add("EditState", typeof(bool));
                for (int i = 0; i < dv.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dv[i]["ID"];
                    row["AssessID"] = dv[i]["AssessID"];
                    row["EmployeeCode"] = dv[i]["EmployeeCode"];
                    row["EmployeeName"] = dv[i]["EmployeeName"];
                    if (dv[i]["Sex"] != null)
                    {
                        row["Sex"] = dv[i]["Sex"];
                        row["SexName"] = DAL.Util.GetDisplayName((Enum_Sex)dv[i]["Sex"]);
                    }

                    if (dv[i]["AssessState"].ToString() != "")
                    {
                        row["AssessState"] = dv[i]["AssessState"];
                        if (Convert.ToInt16(dv[i]["AssessState"]) == Convert.ToInt16(Enum_AssessState.Plan))
                        {
                            row["EditState"] = true;
                        }
                        else
                        {
                            row["EditState"] = false;
                        }
                        row["AssessStateName"] = DAL.Util.GetDisplayName((Enum_AssessState)dv[i]["AssessState"]);
                    }
                    else
                    {
                        row["AssessStateName"] = "未考评";
                    }
                    row["IDNumber"] = dv[i]["IDNumber"];
                    row["DeptName"] = dv[i]["DeptName"];
                    row["StationName"] = dv[i]["StationName"];
                    row["MobilePhone"] = dv[i]["MobilePhone"];
                    row["UpdateTime"] = dv[i]["UpdateTime"];
                    dat.Rows.Add(row);
                }
                json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult Check(int id, int checkState)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                ISalaryAssessService service = ServiceFactory.Factory.SalaryAssessService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                SalaryAssess assess = service.SearchOne(condition);
                if (assess != null)
                {
                    if (assess.State == Convert.ToInt16(Enum_AssessState.Pass))
                    {
                        assess.State = checkState;
                        assess.UpdateTime = DateTime.Now;
                        assess.Updater = Utilities.Security.CurrentUser.UserName;
                        assess.UpdateIP = Network.ClientIP;
                        service.Update(assess);
                        json.Data = JsonUtil.GetSuccessForString("提交审核成功");
                    }
                    else
                    {
                        json.Data = JsonUtil.GetSuccessForString("数据状态错误");
                    }
                }
                else
                {
                    json.Data = JsonUtil.GetSuccessForString("未找到数据");
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