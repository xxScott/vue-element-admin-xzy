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
    public class SalaryChangeController : Controller
    {
        public JsonResult Save(Models.Data.SalaryChangeData entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryChangeService service = ServiceFactory.Factory.SalaryChangeService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));

                //查询数据
                SalaryChange old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    SalaryChange new_entity = new SalaryChange();
                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.EffectDate = entity.EffectDate;
                    new_entity.Reason = entity.Reason;
                    new_entity.Description = entity.Description;
                    new_entity.CreateTime = DateTime.Now;
                    new_entity.Creator = Utilities.Security.CurrentUser.UserName;
                    new_entity.CreateIP = Network.ClientIP;
                    new_entity.UpdateTime = DateTime.Now;
                    new_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    new_entity.UpdateIP = Network.ClientIP;


                    List<SalaryChangeProject> list = new List<SalaryChangeProject>();
                    for (int i = 0; i < entity.changes.Count; i++)
                    {
                        SalaryChangeProject pro = new SalaryChangeProject();
                        pro.SourceProjectID = entity.changes[i].SourceProjectID;
                        pro.SalaryProjectID = entity.changes[i].SourceProjectID;
                        pro.SalaryAmount = entity.changes[i].SourceAmount;
                        list.Add(pro);
                    }

                    ServiceFactory.Factory.SalaryChangeProjectService.Insert(new_entity, list);

                    json.Data = JsonUtil.GetSuccessForString("新增已完成");
                }
                else
                {
                    old_entity.EmployeeID = entity.EmployeeID;
                    old_entity.EffectDate = entity.EffectDate;
                    old_entity.Reason = entity.Reason;
                    old_entity.Description = entity.Description;
                    old_entity.UpdateTime = DateTime.Now;
                    old_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    old_entity.UpdateIP = Network.ClientIP;

                    List<SalaryChangeProject> list = new List<SalaryChangeProject>();
                    for (int i = 0; i < entity.changes.Count; i++)
                    {
                        SalaryChangeProject pro = new SalaryChangeProject();
                        pro.SourceProjectID = entity.changes[i].SourceProjectID;
                        pro.SalaryProjectID = entity.changes[i].SourceProjectID;
                        pro.SalaryAmount = entity.changes[i].SourceAmount;
                        list.Add(pro);
                    }

                    ServiceFactory.Factory.SalaryChangeProjectService.Update(old_entity, list);
                    json.Data = JsonUtil.GetSuccessForString("修改已完成");
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);
            }
            return json;
        }

        public JsonResult Get(int empId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                ISalaryChangeService service = ServiceFactory.Factory.SalaryChangeService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("EmployeeID", empId));
                SalaryChange empbase = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(empbase);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult GetSalaryData(int empId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                ISalaryChangeService service = ServiceFactory.Factory.SalaryChangeService;

                DataTable dt = service.QueryChange(empId);
                if (dt.Rows.Count > 0)
                {
                    DataTable dat = new DataTable();
                    dat.Columns.Add("ID", typeof(Int16));
                    dat.Columns.Add("salaryId", typeof(Int16));

                    dat.Columns.Add("ProjectName", typeof(string));
                    dat.Columns.Add("SalaryAmount", typeof(string));
                    dat.Columns.Add("SourceAmount", typeof(string));
                    dat.Columns.Add("Description", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        DataRow row = dat.NewRow();
                        row["ID"] = dt.Rows[i]["ID"];
                        row["salaryId"] = dt.Rows[i]["salaryId"];
                        row["ProjectName"] = dt.Rows[i]["ProjectName"];
                        row["SalaryAmount"] = dt.Rows[i]["SalaryAmount"];
                        row["SourceAmount"] = dt.Rows[i]["SourceAmount"];
                        row["Description"] = dt.Rows[i]["salaryDescription"];
                        dat.Rows.Add(row);
                    }
                    json.Data = JsonUtil.GetSuccessForObject(dat);
                }
                else
                {
                    json.Data = JsonUtil.GetFailForString("暂无薪资科目");
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