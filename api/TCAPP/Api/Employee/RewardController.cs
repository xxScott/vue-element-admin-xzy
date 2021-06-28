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
    public class RewardController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string employeeId, string keyword, string adapter, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                if (adapter.IndexOf("|") == -1)
                {
                    Enum_Adapter data_adapter = Enum_Adapter.None;
                    if (adapter != null)
                        data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);

                    if (data_adapter == Enum_Adapter.None)
                    {
                        IEmployeeAwardService service = ServiceFactory.Factory.EmployeeAwardService;
                        ConditionSet condition = new ConditionSet();

                        //start /query /groupid&storeid /20190405
                        GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                        //end

                        condition.Add(new SimpleCondition("EmployeeID", employeeId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                            _condition.Add(new SimpleCondition("EmployeeCode", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("AwardDate", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("AwardAmount", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Reason", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Decision", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<EmployeeAwardView> empAwardes = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                        //数据转换，查询枚举描述
                        DataTable dat = new DataTable();
                        dat.Columns.Add("ID", typeof(Int16));
                        dat.Columns.Add("AwardType", typeof(string));
                        dat.Columns.Add("TypeName", typeof(string));
                        dat.Columns.Add("EmployeeCode", typeof(string));
                        dat.Columns.Add("EmployeeName", typeof(string));
                        dat.Columns.Add("AwardDate", typeof(DateTime));
                        dat.Columns.Add("AwardAmount", typeof(double));
                        dat.Columns.Add("Reason", typeof(string));
                        dat.Columns.Add("Decision", typeof(string));
                        dat.Columns.Add("Description", typeof(string));
                        dat.Columns.Add("UpdateTime", typeof(string));

                        for (int i = 0; i < empAwardes.Count; i++)
                        {
                            DataRow row = dat.NewRow();
                            row["ID"] = empAwardes[i].ID;
                            if (empAwardes[i].AwardType != null)
                            {
                                row["AwardType"] = empAwardes[i].AwardType;
                                row["TypeName"] = DAL.Util.GetDisplayName((Enum_AwardType)empAwardes[i].AwardType);
                            }
                            row["EmployeeCode"] = empAwardes[i].EmployeeCode;
                            row["EmployeeName"] = empAwardes[i].EmployeeName;
                            row["AwardDate"] = empAwardes[i].AwardDate;
                            row["AwardAmount"] = empAwardes[i].AwardAmount;
                            row["Reason"] = empAwardes[i].Reason;
                            row["Decision"] = empAwardes[i].Decision;
                            row["UpdateTime"] = empAwardes[i].UpdateTime;
                            row["Description"] = empAwardes[i].Description;
                            dat.Rows.Add(row);
                        }
                        json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
                    }
                    if (data_adapter == Enum_Adapter.TreeGrid)
                    {
                        int totalCount = 0;
                        JArray array = new TreeGridAdapter().GetEmpAward(Convert.ToInt16(storeId), keyword, pageNumber, pageSize, out totalCount);
                        json.Data = JsonUtil.GetSuccessForObject(array, totalCount);
                    }
                }
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
                IEmployeeAwardService service = ServiceFactory.Factory.EmployeeAwardService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeAward empAward = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(empAward);
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
                json.Data = ServiceFactory.Factory.EmployeeAwardService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Save(EmployeeAward entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IEmployeeAwardService service = ServiceFactory.Factory.EmployeeAwardService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                EmployeeAward old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    EmployeeAward new_entity = new EmployeeAward();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.AwardType = entity.AwardType;
                    new_entity.AwardDate = entity.AwardDate;
                    new_entity.AwardAmount = entity.AwardAmount;
                    new_entity.Reason = entity.Reason;
                    new_entity.Decision = entity.Decision;

                    new_entity.Description = entity.Description;
                    new_entity.CreateTime = DateTime.Now;
                    new_entity.Creator = Utilities.Security.CurrentUser.UserName;
                    new_entity.CreateIP = Network.ClientIP;
                    new_entity.UpdateTime = DateTime.Now;
                    new_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    new_entity.UpdateIP = Network.ClientIP;
                    service.Insert(new_entity);
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

                    old_entity.AwardType = entity.AwardType;
                    old_entity.AwardDate = entity.AwardDate;
                    old_entity.AwardAmount = entity.AwardAmount;
                    old_entity.Reason = entity.Reason;
                    old_entity.Decision = entity.Decision;

                    old_entity.Description = entity.Description;
                    old_entity.UpdateTime = DateTime.Now;
                    old_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    old_entity.UpdateIP = Network.ClientIP;
                    service.Update(old_entity);
                    json.Data = JsonUtil.GetSuccessForString("修改已完成");
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