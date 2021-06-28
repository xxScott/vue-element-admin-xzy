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
    public class AssessController : Controller
    {

        public JsonResult Query(int groupId, int storeId, int peroidId, string keyword, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryAssessService service = ServiceFactory.Factory.SalaryAssessService;
                int totalCount = 0;
                DataView dv = service.QueryEmp(storeId, peroidId, keyword, pageNumber, pageSize, out totalCount).DefaultView;
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
                dat.Columns.Add("Description", typeof(string));
                dat.Columns.Add("Sex", typeof(Int16));
                dat.Columns.Add("SexName", typeof(string));
                dat.Columns.Add("State", typeof(int));
                dat.Columns.Add("StateName", typeof(string));
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
                    if (dv[i]["Sex"].ToString() != "")
                    {
                        row["Sex"] = dv[i]["Sex"];
                        row["SexName"] = DAL.Util.GetDisplayName((Enum_Sex)dv[i]["Sex"]);
                    }
                    if (dv[i]["State"].ToString() != "")
                    {
                        row["State"] = dv[i]["State"];
                        row["StateName"] = DAL.Util.GetDisplayName((Enum_EmployeeState)dv[i]["State"]);
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

                    row["Description"] = dv[i]["Description"];
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


        public JsonResult LoadSalaryProject(int groupId, int storeId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryChangeService service = ServiceFactory.Factory.SalaryChangeService;

                DataView dv = service.Query(groupId, storeId).DefaultView;
                //固定薪资
                dv.RowFilter = "ProjectProperty=" + Convert.ToInt16(Enum_SalaryProjectProperty.Increase);
                double total = 0.00; string type = "固定薪资";
                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("ProjectType", typeof(string));
                dat.Columns.Add("salaryId", typeof(Int16));
                dat.Columns.Add("ProjectCode", typeof(string));
                dat.Columns.Add("ProjectName", typeof(string));
                dat.Columns.Add("ProjectProperty", typeof(Int16));
                dat.Columns.Add("ProjectPropertyName", typeof(string));
                dat.Columns.Add("ProjectSource", typeof(Int16));
                dat.Columns.Add("ProjectSourceName", typeof(string));
                dat.Columns.Add("SalaryAmount", typeof(string));
                dat.Columns.Add("Description", typeof(string));
                dat.Columns.Add("DataState", typeof(bool));
                for (int i = 0; i < dv.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dv[i]["ID"];
                    row["ProjectType"] = type;

                    row["ProjectCode"] = dv[i]["ProjectCode"];
                    row["ProjectName"] = dv[i]["ProjectName"];
                    if (dv[i]["ProjectProperty"] != null)
                    {
                        row["ProjectProperty"] = dv[i]["ProjectProperty"];
                        row["ProjectPropertyName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectProperty)dv[i]["ProjectProperty"]);
                    }
                    row["SalaryAmount"] = "0.00";
                    row["Description"] = "";
                    row["DataState"] = false;
                    dat.Rows.Add(row);
                }
                AddNewRow(ref dat, total, type, "固定薪资小计");

                //奖罚薪资
                double totalAward = 0.00; type = "奖罚薪资";
                AddNewRow(ref dat, totalAward, type, "奖励金额");

                double totalPunish = 0.00;
                AddNewRow(ref dat, totalPunish, type, "处罚金额");

                total = totalAward + totalPunish;
                AddNewRow(ref dat, total, type, "奖罚薪资小计");

                //考评薪资
                dv.RowFilter = string.Empty;
                dv.RowFilter = "ProjectProperty=" + Convert.ToInt16(Enum_SalaryProjectProperty.Deduct);
                total = 0; type = "考评薪资";
                for (int i = 0; i < dv.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dv[i]["ID"];
                    row["ProjectType"] = type;

                    row["ProjectCode"] = dv[i]["ProjectCode"];
                    row["ProjectName"] = dv[i]["ProjectName"];
                    if (dv[i]["ProjectProperty"] != null)
                    {
                        row["ProjectProperty"] = dv[i]["ProjectProperty"];
                        row["ProjectPropertyName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectProperty)dv[i]["ProjectProperty"]);
                    }
                    row["SalaryAmount"] = "0.00";
                    row["Description"] = "";
                    row["DataState"] = false;
                    dat.Rows.Add(row);
                }
                AddNewRow(ref dat, total, type, "考评薪资小计");




                json.Data = JsonUtil.GetSuccessForObject(dat);


            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult QuerySalaryProject(int empId, int peroidId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryChangeService service = ServiceFactory.Factory.SalaryChangeService;

                DataView dv = service.QueryIncrease(empId, Convert.ToInt16(Enum_SalaryProjectProperty.Increase)).DefaultView;
                //固定薪资
                double total = 0; string type = "固定薪资";
                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("ProjectType", typeof(string));
                dat.Columns.Add("DataID", typeof(Int16));
                dat.Columns.Add("PeroidID", typeof(Int16));
                dat.Columns.Add("AssessID", typeof(Int16));
                dat.Columns.Add("ProjectName", typeof(string));
                dat.Columns.Add("SalaryAmount", typeof(string));
                dat.Columns.Add("DataState", typeof(bool));
                dat.Columns.Add("EditState", typeof(bool));
                for (int i = 0; i < dv.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dv[i]["ID"];
                    row["ProjectType"] = type;
                    row["ProjectName"] = dv[i]["ProjectName"];
                    row["SalaryAmount"] = dv[i]["SalaryAmount"];
                    if (dv[i]["SalaryAmount"] != null && dv[i]["SalaryAmount"].ToString() != "")
                    {
                        total += Convert.ToDouble(dv[i]["SalaryAmount"]);
                    }
                    else
                    {
                        row["SalaryAmount"] = "0.00";
                    }


                    dat.Rows.Add(row);
                }
                AddNewRow(ref dat, total, type, "固定薪资小计");

                //奖罚薪资
                IEmployeeAwardService serviceAward = ServiceFactory.Factory.EmployeeAwardService;
                List<EmployeeAwardView> award = serviceAward.Search(new SimpleCondition("EmployeeID", empId));
                List<EmployeeAwardView> awardDt = award.Where(a => a.AwardType == Convert.ToInt16(Enum_AwardType.Cite) || a.AwardType == Convert.ToInt16(Enum_AwardType.MaxCite) || a.AwardType == Convert.ToInt16(Enum_AwardType.MinCite)).ToList();
                double totalAward = awardDt.Sum(a => a.AwardAmount); type = "奖罚薪资";
                AddNewRow(ref dat, totalAward, type, "奖励金额");



                List<EmployeeAwardView> punishDt = award.Where(a => a.AwardType == Convert.ToInt16(Enum_AwardType.MaxFault) || a.AwardType == Convert.ToInt16(Enum_AwardType.MinFault) || a.AwardType == Convert.ToInt16(Enum_AwardType.Warning)).ToList();
                double totalPunish = punishDt.Sum(a => a.AwardAmount); type = "奖罚薪资";
                AddNewRow(ref dat, totalPunish * -1, type, "处罚金额");

                total = totalAward + totalPunish;
                AddNewRow(ref dat, total, type, "奖罚薪资小计");

                //考评薪资

                DataView dvDe = service.QueryEvaluation(empId, Convert.ToInt16(Enum_SalaryProjectProperty.Deduct), peroidId).DefaultView;
                total = 0; type = "考评薪资";
                for (int i = 0; i < dvDe.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dvDe[i]["ID"];
                    row["ProjectType"] = type;
                    row["DataID"] = dvDe[i]["DataID"];
                    row["PeroidID"] = dvDe[i]["PeroidID"];
                    row["AssessID"] = dvDe[i]["AssessID"];
                    row["ProjectName"] = dvDe[i]["ProjectName"];

                    row["SalaryAmount"] = dvDe[i]["SalaryAmount"];
                    if (dvDe[i]["SalaryAmount"] != null && dvDe[i]["SalaryAmount"].ToString() != "")
                    {
                        total += Convert.ToDouble(dvDe[i]["SalaryAmount"]);
                    }
                    else
                    {
                        row["SalaryAmount"] = "0.00";
                    }
                    row["EditState"] = true;
                    if (dvDe[i]["AssessState"].ToString() != "")
                    {
                        if (Convert.ToInt16(dvDe[i]["AssessState"]) == Convert.ToInt16(Enum_AssessState.Plan))
                        {
                            row["EditState"] = true;
                        }
                        else
                        {
                            row["EditState"] = false;
                        }
                    }
                    //else
                    //{
                    //    row["EditState"] = false;
                    //}
                    row["DataState"] = true;
                    dat.Rows.Add(row);
                }
                AddNewRow(ref dat, total * -1, type, "考评薪资小计");



                json.Data = JsonUtil.GetSuccessForObject(dat);


            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult SaveSalaryProject(Models.Data.SalaryAssessView entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryAssessService service = ServiceFactory.Factory.SalaryAssessService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));

                //查询数据
                SalaryAssess old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    SalaryAssess new_entity = new SalaryAssess();
                    new_entity.GroupID = entity.GroupID;
                    new_entity.StoreID = entity.StoreID;
                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.PeroidID = entity.PeroidID;
                    if (new_entity.PeroidID != 0)
                    {
                        Periods period = ServiceFactory.Factory.PeriodService.SearchOne(new SimpleCondition("ID", new_entity.PeroidID));
                        new_entity.StartDate = period.StartDate;
                        new_entity.EndDate = period.EndDate;
                    }
                    new_entity.TotalAmount = entity.TotalAmount;
                    new_entity.DeductAmount = entity.DeductAmount;
                    new_entity.RealAmount = entity.RealAmount;
                    new_entity.CreateTime = DateTime.Now;
                    new_entity.Creator = Utilities.Security.CurrentUser.UserName;
                    new_entity.CreateIP = Network.ClientIP;
                    new_entity.UpdateTime = DateTime.Now;
                    new_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    new_entity.UpdateIP = Network.ClientIP;
                    new_entity.State = Convert.ToInt16(Enum_AssessState.Plan);

                    SalaryAssessData data = new SalaryAssessData();


                    data.SalaryProjectID = entity.AssessData.SalaryProjectID;
                    if (data.SalaryProjectID != 0)
                    {
                        SalaryProjects salaryPro = ServiceFactory.Factory.SalaryProjectsService.SearchOne(new SimpleCondition("ID", data.SalaryProjectID));
                        data.SalaryProjectName = salaryPro.ProjectName;
                        data.SalaryProjectProperty = salaryPro.ProjectProperty;
                        data.SalaryProjectSource = salaryPro.ProjectSource;
                        data.SalaryDesignFormulas = salaryPro.DesignFormulas;
                        data.SalaryProjectOrder = salaryPro.ProjectOrder;
                    }
                    data.SalaryAmount = entity.AssessData.SalaryAmount;

                    service.SaveAssess(new_entity, data);

                    json.Data = JsonUtil.GetSuccessForString("新增已完成");
                }
                else
                {
                    SalaryAssessData data = ServiceFactory.Factory.SalaryAssessDataService.SearchOne(new SimpleCondition("ID", entity.AssessData.ID));
                    if (data != null)
                    {
                        data.SalaryAmount = entity.AssessData.SalaryAmount;
                        data.UpdateTime = DateTime.Now;
                        data.Updater = Utilities.Security.CurrentUser.UserName;
                        data.UpdateIP = Network.ClientIP;
                    }
                    else
                    {
                        data = new SalaryAssessData();
                        data.SalaryProjectID = entity.AssessData.SalaryProjectID;
                        if (data.SalaryProjectID != 0)
                        {
                            SalaryProjects salaryPro = ServiceFactory.Factory.SalaryProjectsService.SearchOne(new SimpleCondition("ID", data.SalaryProjectID));
                            data.SalaryProjectName = salaryPro.ProjectName;
                            data.SalaryProjectProperty = salaryPro.ProjectProperty;
                            data.SalaryProjectSource = salaryPro.ProjectSource;
                            data.SalaryDesignFormulas = salaryPro.DesignFormulas;
                            data.SalaryProjectOrder = salaryPro.ProjectOrder;
                        }
                        data.SalaryAmount = entity.AssessData.SalaryAmount;
                    }
                    old_entity.TotalAmount = entity.TotalAmount;
                    old_entity.DeductAmount = entity.DeductAmount;
                    old_entity.RealAmount = entity.RealAmount;
                    old_entity.UpdateTime = DateTime.Now;
                    old_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    old_entity.UpdateIP = Network.ClientIP;

                    service.UpdateAssess(old_entity, data);

                    json.Data = JsonUtil.GetSuccessForString("修改已完成");
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);
            }
            return json;
        }

        public JsonResult QueryPeriod(string companyId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {

                IPeriodService service = ServiceFactory.Factory.PeriodService;
                DataView dv = service.Query(Convert.ToInt16(companyId)).DefaultView;


                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("PeriodName", typeof(string));
                dat.Columns.Add("StartDate", typeof(string));
                dat.Columns.Add("EndDate", typeof(string));
                dat.Columns.Add("State", typeof(int));
                dat.Columns.Add("StateName", typeof(int));
                dat.Columns.Add("EditState", typeof(bool));
                for (int i = 0; i < dv.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dv[i]["ID"];
                    row["PeriodName"] = dv[i]["PeriodName"];
                    row["StartDate"] = DateTime.Parse(dv[i]["StartDate"].ToString()).ToString("yyyy-MM-dd");
                    row["EndDate"] = DateTime.Parse(dv[i]["EndDate"].ToString()).ToString("yyyy-MM-dd");
                    if (dv[i]["State"] != null)
                    {
                        row["State"] = dv[i]["State"];
                        row["StateName"] = DAL.Util.GetDisplayName((Enum_MakeClassState)dv[i]["State"]);
                        // row["EditState"] = dv[i]["State"] == Convert.ToInt16(Enum_MakeClassState.MakeOver);
                    }
                    dat.Rows.Add(row);
                }
                json.Data = JsonUtil.GetSuccessForObject(dat);

            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Submit(int id)
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
                    if (assess.State == Convert.ToInt16(Enum_AssessState.Plan))
                    {
                        assess.State = Convert.ToInt16(Enum_AssessState.Wait);
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

        private static void AddNewRow(ref DataTable dat, double amount, string type, string projectName)
        {
            DataRow row = dat.NewRow();
            row["ID"] = 0;
            row["ProjectType"] = type;
            row["ProjectName"] = projectName;
            row["SalaryAmount"] = amount.ToString("0.00");
            row["DataState"] = false;
            dat.Rows.Add(row);
        }
    }
}