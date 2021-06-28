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


namespace HrApp.Api
{
    public class EntryController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, string adapter, int? pageNumber, int? pageSize, string state)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                ConditionSet condition = new ConditionSet();
                //condition.Add(new SimpleCondition("GroupID", groupId));
                condition.Add(new SimpleCondition("StoreID", storeId));
                if (state != null && state != "")
                {
                    condition.Add(new SimpleCondition("State", state));
                }
                if (keyword != null && keyword != string.Empty)
                {
                    ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                    //start /query /groupid&storeid /20190405
                    GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                    //end

                    _condition.Add(new SimpleCondition("EmployeeCode", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("IDNumber", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Birthday", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("NativePlace", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Height", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Weight", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("MobilePhone", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Address", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("MajorName", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("DiseaseNotes", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("DefectNotes", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("SalaryBankName", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("SalaryCardNo", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("EmergencyContact", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("EmergencyPhone", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("EmergencyRelation", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                    condition.Add(_condition);
                }
                //查询记录总数
                int totalCount = service.Count(condition);
                //查询分页数据
                List<EmployeeBaseView> empBasees = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

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
                dat.Columns.Add("EditState", typeof(bool));
                for (int i = 0; i < empBasees.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = empBasees[i].ID;
                    row["EmployeeCode"] = empBasees[i].EmployeeCode;
                    row["EmployeeName"] = empBasees[i].EmployeeName;
                    if (empBasees[i].Sex != null)
                    {
                        row["Sex"] = empBasees[i].Sex;
                        row["SexName"] = DAL.Util.GetDisplayName((Enum_Sex)empBasees[i].Sex);
                    }
                    if (empBasees[i].State != null)
                    {
                        row["State"] = empBasees[i].State;
                        if (empBasees[i].State == Convert.ToInt16(Enum_EmployeeState.Ready))
                        {
                            row["EditState"] = true;
                        }
                        else
                        {
                            row["EditState"] = false;
                        }
                        row["StateName"] = DAL.Util.GetDisplayName((Enum_EmployeeState)empBasees[i].State);
                    }
                    row["IDNumber"] = empBasees[i].IDNumber;
                    row["DeptName"] = empBasees[i].DeptName;
                    row["StationName"] = empBasees[i].StationName;
                    row["MobilePhone"] = empBasees[i].MobilePhone;
                    row["UpdateTime"] = empBasees[i].UpdateTime;

                    row["Description"] = empBasees[i].Description;
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

        public JsonResult Delete(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                json.Data = ServiceFactory.Factory.EmployeeBaseService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Get(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeBaseView empbase = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(empbase);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Save(Models.Data.EntryDataView entity, string state)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                EmployeeBase old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    EmployeeBase new_entity = new EmployeeBase();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.State = Convert.ToInt16(state);
                    new_entity.GroupID = entity.GroupID == 0 ? -1 : entity.GroupID;
                    new_entity.StoreID = entity.StoreID;
                    new_entity.EmployeePic = entity.EmployeePic;
                    new_entity.EmployeeCode = entity.EmployeeCode;
                    new_entity.EmployeeName = entity.EmployeeName;
                    new_entity.IDNumber = entity.IDNumber;
                    new_entity.Sex = entity.Sex;
                    new_entity.Birthday = entity.Birthday;
                    new_entity.Nation = entity.Nation;
                    new_entity.NativePlace = entity.NativePlace;
                    new_entity.PoliticalStatus = entity.PoliticalStatus;
                    new_entity.Height = entity.Height;
                    new_entity.Weight = entity.Weight;
                    new_entity.MobilePhone = entity.MobilePhone;
                    new_entity.Address = entity.Address;
                    new_entity.EducationDegree = entity.EducationDegree;
                    new_entity.MajorName = entity.MajorName;
                    new_entity.MarriageStatus = entity.MarriageStatus;
                    new_entity.HasDisease = entity.HasDisease;
                    new_entity.DiseaseNotes = entity.DiseaseNotes;
                    new_entity.HasDefect = entity.HasDefect;
                    new_entity.DefectNotes = entity.DefectNotes;
                    new_entity.DeptID = entity.DeptID;
                    new_entity.WorkStationID = entity.WorkStationID;
                    new_entity.SalaryBankName = entity.SalaryBankName;
                    new_entity.SalaryCardNo = entity.SalaryCardNo;
                    new_entity.EmergencyContact = entity.EmergencyContact;
                    new_entity.EmergencyPhone = entity.EmergencyPhone;
                    new_entity.EmergencyRelation = entity.EmergencyRelation;


                    new_entity.Description = entity.Description;
                    new_entity.CreateTime = DateTime.Now;
                    new_entity.Creator = Utilities.Security.CurrentUser.UserName;
                    new_entity.CreateIP = Network.ClientIP;
                    new_entity.UpdateTime = DateTime.Now;
                    new_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    new_entity.UpdateIP = Network.ClientIP;

                    //薪资预设
                    List<SalaryBase> list = new List<SalaryBase>();
                    if (entity.Salarys != null)
                    {
                        for (int i = 0; i < entity.Salarys.Count; i++)
                        {
                            SalaryBase salarybase = new SalaryBase();

                            salarybase.CompanyID = entity.Salarys[i].CompanyID;
                            salarybase.EmployeeID = entity.Salarys[i].EmployeeID;
                            salarybase.SalaryProjectID = entity.Salarys[i].SalaryProjectID;
                            salarybase.SalaryAmount = entity.Salarys[i].SalaryAmount;
                            salarybase.Description = entity.Salarys[i].Description;
                            salarybase.State = Convert.ToInt16(Enum_AssessState.Plan);
                            list.Add(salarybase);
                        }
                    }
                    service.SaveEmpBase(new_entity, list);


                    json.Data = JsonUtil.GetSuccessForString("新增已完成");

                }
                else
                {

                    //start /update /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    old_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    old_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    old_entity.EmployeePic = entity.EmployeePic;
                    old_entity.EmployeeCode = entity.EmployeeCode;
                    old_entity.EmployeeName = entity.EmployeeName;
                    old_entity.IDNumber = entity.IDNumber;
                    old_entity.Sex = entity.Sex;
                    old_entity.Birthday = entity.Birthday;
                    old_entity.Nation = entity.Nation;
                    old_entity.NativePlace = entity.NativePlace;
                    old_entity.PoliticalStatus = entity.PoliticalStatus;
                    old_entity.Height = entity.Height;
                    old_entity.Weight = entity.Weight;
                    old_entity.MobilePhone = entity.MobilePhone;
                    old_entity.Address = entity.Address;
                    old_entity.EducationDegree = entity.EducationDegree;
                    old_entity.MajorName = entity.MajorName;
                    old_entity.MarriageStatus = entity.MarriageStatus;
                    old_entity.HasDisease = entity.HasDisease;
                    old_entity.DiseaseNotes = entity.DiseaseNotes;
                    old_entity.HasDefect = entity.HasDefect;
                    old_entity.DefectNotes = entity.DefectNotes;
                    old_entity.DeptID = entity.DeptID;
                    old_entity.WorkStationID = entity.WorkStationID;
                    old_entity.SalaryBankName = entity.SalaryBankName;
                    old_entity.SalaryCardNo = entity.SalaryCardNo;
                    old_entity.EmergencyContact = entity.EmergencyContact;
                    old_entity.EmergencyPhone = entity.EmergencyPhone;
                    old_entity.EmergencyRelation = entity.EmergencyRelation;

                    old_entity.Description = entity.Description;
                    old_entity.UpdateTime = DateTime.Now;
                    old_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    old_entity.UpdateIP = Network.ClientIP;

                    //薪资预设
                    List<SalaryBase> list = new List<SalaryBase>();
                    if (entity.Salarys != null)
                    {
                        for (int i = 0; i < entity.Salarys.Count; i++)
                        {
                            SalaryBase salarybase = new SalaryBase();
                            salarybase.ID = entity.Salarys[i].ID;
                            salarybase.CompanyID = entity.Salarys[i].CompanyID;
                            salarybase.EmployeeID = entity.Salarys[i].EmployeeID;
                            salarybase.SalaryProjectID = entity.Salarys[i].SalaryProjectID;
                            salarybase.SalaryAmount = entity.Salarys[i].SalaryAmount;
                            salarybase.Description = entity.Salarys[i].Description;

                            list.Add(salarybase);
                        }
                    }

                    service.UpdateEmpBase(old_entity, list);
                    json.Data = JsonUtil.GetSuccessForString("修改已完成");

                }
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
                IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeBaseView empbase = service.SearchOne(condition);
                if (empbase != null)
                {
                    if (empbase.State == Convert.ToInt16(Enum_EmployeeState.Ready))
                    {
                        empbase.State = Convert.ToInt16(Enum_EmployeeState.Wait);
                        service.Update(empbase);
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

        public JsonResult Check(int id, int checkState)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeBaseView empbase = service.SearchOne(condition);
                if (empbase != null)
                {
                    if (empbase.State == Convert.ToInt16(Enum_EmployeeState.Wait))
                    {
                        empbase.State = checkState;
                        service.Update(empbase);
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


        public JsonResult GetSalaryData(int empId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                ISalaryBaseService service = ServiceFactory.Factory.SalaryBaseService;

                DataTable dt = service.QueryEmpSalary(empId);
                DataView dv = dt.DefaultView;
                dv.RowFilter = "ProjectProperty=" + Convert.ToInt16(Enum_SalaryProjectProperty.Increase);
                if (dv.Count > 0)
                {
                    DataTable dat = new DataTable();
                    dat.Columns.Add("ID", typeof(Int16));
                    dat.Columns.Add("salaryId", typeof(Int16));
                    dat.Columns.Add("ProjectCode", typeof(string));
                    dat.Columns.Add("ProjectName", typeof(string));
                    dat.Columns.Add("ProjectProperty", typeof(Int16));
                    dat.Columns.Add("ProjectPropertyName", typeof(string));
                    dat.Columns.Add("ProjectSource", typeof(Int16));
                    dat.Columns.Add("ProjectSourceName", typeof(string));
                    dat.Columns.Add("SalaryAmount", typeof(string));
                    dat.Columns.Add("Description", typeof(string));
                    for (int i = 0; i < dv.Count; i++)
                    {
                        DataRow row = dat.NewRow();
                        row["ID"] = dv[i]["ID"];
                        row["salaryId"] = dv[i]["salaryId"];
                        row["ProjectCode"] = dv[i]["ProjectCode"];
                        row["ProjectName"] = dv[i]["ProjectName"];
                        if (dv[i]["ProjectProperty"] != null)
                        {
                            row["ProjectProperty"] = dv[i]["ProjectProperty"];
                            row["ProjectPropertyName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectProperty)dv[i]["ProjectProperty"]);
                        }
                        if (dt.Rows[i]["ProjectSource"] != null)
                        {
                            row["ProjectSource"] = dv[i]["ProjectSource"];
                            row["ProjectSourceName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectSource)dv[i]["ProjectSource"]);
                        }
                        row["SalaryAmount"] = dv[i]["SalaryAmount"];
                        row["Description"] = dv[i]["salaryDescription"];
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
        public JsonResult QuerySalaryProject(string groupId, string storeId, int projectProperty)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                if (groupId != "-1")
                {
                    storeId = "-1";
                }
                ISalaryProjectsService service = ServiceFactory.Factory.SalaryProjectsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("GroupID", groupId == "0" ? "-1" : groupId));
                condition.Add(new SimpleCondition("StoreID", storeId));
                condition.Add(new SimpleCondition("ProjectProperty", projectProperty));

                //查询分页数据
                List<SalaryProjects> projects = service.Search(condition);
                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("ProjectCode", typeof(string));
                dat.Columns.Add("ProjectName", typeof(string));
                dat.Columns.Add("ProjectProperty", typeof(Int16));
                dat.Columns.Add("ProjectPropertyName", typeof(string));
                dat.Columns.Add("ProjectSource", typeof(Int16));
                dat.Columns.Add("ProjectSourceName", typeof(string));
                dat.Columns.Add("Description", typeof(string));
                for (int i = 0; i < projects.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = projects[i].ID;
                    row["ProjectCode"] = projects[i].ProjectCode;
                    row["ProjectName"] = projects[i].ProjectName;
                    if (projects[i].ProjectProperty != null)
                    {
                        row["ProjectProperty"] = projects[i].ProjectProperty;
                        row["ProjectPropertyName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectProperty)projects[i].ProjectProperty);
                    }
                    if (projects[i].ProjectSource != null)
                    {
                        row["ProjectSource"] = projects[i].ProjectSource;
                        row["ProjectSourceName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectSource)projects[i].ProjectSource);
                    }
                    row["Description"] = projects[i].Description;
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
    }

}