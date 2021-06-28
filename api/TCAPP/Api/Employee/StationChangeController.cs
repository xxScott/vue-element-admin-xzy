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
    public class StationChangeController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, string adapter, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json._dateFormat = "yyyy-MM-dd";
            json.ContentType = "text/plain";

            try
            {
                IEmployeeChangeStationService service = ServiceFactory.Factory.EmployeeChangeStationService;

                //查询记录总数
                int totalCount = 0;
                DataView dv = service.Query(Convert.ToInt32(Enum_EmployeeState.Pass), groupId, storeId, keyword, pageNumber, pageSize, out totalCount).DefaultView;

                ////查询分页数据
                //List<EmployeeChangeStation> empStationChanges = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);


                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("EmployeeCode", typeof(string));
                dat.Columns.Add("EmployeeName", typeof(string));
                dat.Columns.Add("EmployeeID", typeof(string));
                dat.Columns.Add("IDNumber", typeof(string));
                dat.Columns.Add("MobilePhone", typeof(string));
                dat.Columns.Add("SourceDeptName", typeof(string));
                dat.Columns.Add("SourceStationName", typeof(string));
                dat.Columns.Add("DeptName", typeof(string));
                dat.Columns.Add("StationName", typeof(string));
                dat.Columns.Add("ChangeDate", typeof(string));
                dat.Columns.Add("Description", typeof(string));
                dat.Columns.Add("Sex", typeof(Int16));
                dat.Columns.Add("SexName", typeof(string));

                for (int i = 0; i < dv.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = dv[i]["ID"];
                    row["EmployeeCode"] = dv[i]["EmployeeCode"];
                    row["EmployeeName"] = dv[i]["EmployeeName"];
                    row["EmployeeID"] = dv[i]["EmployeeID"];
                    if (dv[i]["Sex"].ToString() != "")
                    {
                        row["Sex"] = dv[i]["Sex"];
                        row["SexName"] = DAL.Util.GetDisplayName((Enum_Sex)dv[i]["Sex"]);
                    }

                    row["IDNumber"] = dv[i]["IDNumber"];

                    row["SourceDeptName"] = dv[i]["SourceDeptName"];
                    row["SourceStationName"] = dv[i]["SourceStationName"];

                    row["DeptName"] = dv[i]["DeptName"];
                    row["StationName"] = dv[i]["StationName"];
                    row["MobilePhone"] = dv[i]["MobilePhone"];
                    row["ChangeDate"] = dv[i]["ChangeDate"];

                    row["Description"] = dv[i]["Description"];
                    dat.Rows.Add(row);
                }
                json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);

                ////else
                ////{
                ////    IEmployeeBaseService servicebBase = ServiceFactory.Factory.EmployeeBaseService;
                ////    ConditionSet conditionBase = new ConditionSet();
                ////    conditionBase.Add(new SimpleCondition("CompanyID", companyId));
                ////    conditionBase.Add(new SimpleCondition("State", Convert.ToInt32(Enum_EmployeeState.Pass)));


                ////    //查询记录总数
                ////    totalCount = service.Count(conditionBase);
                ////    //查询分页数据
                ////    List<EmployeeBaseView> empBasees = servicebBase.SearchSection(conditionBase, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                ////    //数据转换，查询枚举描述
                ////    DataTable dat = new DataTable();
                ////    dat.Columns.Add("ID", typeof(Int16));
                ////    dat.Columns.Add("EmployeeCode", typeof(string));
                ////    dat.Columns.Add("EmployeeName", typeof(string));
                ////    dat.Columns.Add("EmployeeID", typeof(string));
                ////    dat.Columns.Add("IDNumber", typeof(string));
                ////    dat.Columns.Add("MobilePhone", typeof(string));
                ////    dat.Columns.Add("SourceDeptName", typeof(string));
                ////    dat.Columns.Add("SourceStationName", typeof(string));
                ////    dat.Columns.Add("DeptName", typeof(string));
                ////    dat.Columns.Add("StationName", typeof(string));
                ////    dat.Columns.Add("ChangeDate", typeof(string));
                ////    dat.Columns.Add("Description", typeof(string));
                ////    dat.Columns.Add("Sex", typeof(Int16));
                ////    dat.Columns.Add("SexName", typeof(string));

                ////    for (int i = 0; i < empBasees.Count; i++)
                ////    {
                ////        DataRow row = dat.NewRow();
                ////        row["ID"] = "";
                ////        row["EmployeeCode"] = empBasees[i].EmployeeCode;
                ////        row["EmployeeName"] = empBasees[i].EmployeeName;
                ////        row["EmployeeID"] = empBasees[i].ID;
                ////        if (empBasees[i].Sex != null)
                ////        {
                ////            row["Sex"] = empBasees[i].Sex;
                ////            row["SexName"] = DAL.Util.GetDisplayName((Enum_Sex)empBasees[i].Sex);
                ////        }
                ////        row["IDNumber"] = empBasees[i].IDNumber;
                ////        row["SourceDeptName"] = empBasees[i].DeptName;
                ////        row["SourceStationName"] = empBasees[i].StationName;
                ////        row["DeptName"] = "";
                ////        row["StationName"] = "";
                ////        row["MobilePhone"] = empBasees[i].MobilePhone;
                ////        row["ChangeDate"] = "";

                ////        row["Description"] = empBasees[i].Description;
                ////        dat.Rows.Add(row);
                ////    }
                //    json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
                //}
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
                IEmployeeChangeStationService service = ServiceFactory.Factory.EmployeeChangeStationService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeChangeStationView empChangeStation = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(empChangeStation);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult Save(EmployeeChangeStation entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IEmployeeChangeStationService service = ServiceFactory.Factory.EmployeeChangeStationService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                EmployeeChangeStation old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    EmployeeChangeStation new_entity = new EmployeeChangeStation();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.DeptID = entity.DeptID;
                    new_entity.WorkStationID = entity.WorkStationID;
                    new_entity.SourceDeptID = entity.SourceDeptID;
                    new_entity.SourceWorkStationID = entity.SourceWorkStationID;
                    new_entity.ChangeDate = entity.ChangeDate;

                    new_entity.Description = entity.Description;
                    new_entity.CreateTime = DateTime.Now;
                    new_entity.Creator = Utilities.Security.CurrentUser.UserName;
                    new_entity.CreateIP = Network.ClientIP;
                    new_entity.UpdateTime = DateTime.Now;
                    new_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    new_entity.UpdateIP = Network.ClientIP;
                    service.Insert(new_entity);
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

                    old_entity.DeptID = entity.DeptID;
                    old_entity.WorkStationID = entity.WorkStationID;
                    old_entity.SourceDeptID = entity.SourceDeptID;
                    old_entity.SourceWorkStationID = entity.SourceWorkStationID;
                    old_entity.ChangeDate = entity.ChangeDate;


                    old_entity.Description = entity.Description;
                    old_entity.UpdateTime = DateTime.Now;
                    old_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    old_entity.UpdateIP = Network.ClientIP;
                    service.Update(old_entity);
                }

                EmployeeBase empBase = ServiceFactory.Factory.EmployeeBaseService.SearchOne(new SimpleCondition("ID", entity.EmployeeID));
                if (empBase != null)
                {
                    empBase.WorkStationID = entity.WorkStationID;
                    empBase.DeptID = entity.DeptID;
                    empBase.UpdateTime = DateTime.Now;
                    empBase.Updater = Utilities.Security.CurrentUser.UserName;
                    empBase.UpdateIP = Network.ClientIP;
                    ServiceFactory.Factory.EmployeeBaseService.Update(empBase);
                    json.Data = JsonUtil.GetSuccessForString("调整已完成");
                }
                else
                {
                    json.Data = JsonUtil.GetFailForString("调整失败，员工数据未找到");
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