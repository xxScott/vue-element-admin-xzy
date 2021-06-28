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
    public class ClassController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, string adapter, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                Enum_Adapter data_adapter = Enum_Adapter.None;
                if (adapter != null)
                    data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);

                if (data_adapter == Enum_Adapter.None)
                {
                    IClassesService service = ServiceFactory.Factory.ClassesService;
                    ConditionSet condition = new ConditionSet();

                    //start /query /groupid&storeid /20190405
                    GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                    //end

                    //condition.Add(new SimpleCondition("GroupID", groupId));
                    //condition.Add(new SimpleCondition("StoreID", storeId));
                    if (keyword != null && keyword != string.Empty)
                    {
                        ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                        _condition.Add(new SimpleCondition("DeptName", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("ClassCode", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("ClassName", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("StartHour", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("StartMinute", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("OverHour", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("OverMinute", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("OverHour", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("RestTypeID", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("RestDays", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                        condition.Add(_condition);
                    }
                    //查询记录总数
                    int totalCount = service.Count(condition);
                    //查询分页数据
                    List<ClassesView> classes = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);


                    //数据转换，查询枚举描述
                    DataTable dat = new DataTable();
                    dat.Columns.Add("ID", typeof(Int16));
                    dat.Columns.Add("DeptName", typeof(string));
                    dat.Columns.Add("ClassCode", typeof(string));
                    dat.Columns.Add("ClassName", typeof(string));
                    dat.Columns.Add("StartHour", typeof(int));
                    dat.Columns.Add("StartMinute", typeof(int));
                    dat.Columns.Add("StartTime", typeof(string));
                    dat.Columns.Add("OverHour", typeof(int));
                    dat.Columns.Add("OverMinute", typeof(int));
                    dat.Columns.Add("OverTime", typeof(string));
                    dat.Columns.Add("RestTypeID", typeof(int));
                    dat.Columns.Add("RestType", typeof(string));
                    dat.Columns.Add("RestDays", typeof(string));
                    dat.Columns.Add("Description", typeof(string));
                    for (int i = 0; i < classes.Count; i++)
                    {
                        DataRow row = dat.NewRow();
                        row["ID"] = classes[i].ID;
                        row["DeptName"] = classes[i].DeptName;
                        row["ClassCode"] = classes[i].ClassCode;
                        row["ClassName"] = classes[i].ClassName;
                        if (classes[i].RestTypeID != null)
                        {
                            row["RestTypeID"] = classes[i].RestTypeID;
                            row["RestType"] = DAL.Util.GetDisplayName((Enum_RestType)classes[i].RestTypeID);
                        }
                        row["StartHour"] = classes[i].StartHour;
                        row["StartMinute"] = classes[i].StartMinute;
                        row["StartTime"] = classes[i].StartHour + " : " + classes[i].StartMinute;
                        row["OverHour"] = classes[i].OverHour;
                        row["OverMinute"] = classes[i].OverMinute;
                        row["OverTime"] = classes[i].OverHour + " : " + classes[i].OverMinute;
                        row["RestDays"] = classes[i].RestDays;

                        row["Description"] = classes[i].Description;
                        dat.Rows.Add(row);
                    }
                    json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
                }
                else if (data_adapter == Enum_Adapter.Combobox)
                {
                    JArray array = new ComboboxAdapter().GetClass(groupId, storeId);
                    json.Data = JsonUtil.GetSuccessForObject(array);
                }
                else
                {
                    json.Data = JsonUtil.GetFailForString("数据加载失败");
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
                IClassesService service = ServiceFactory.Factory.ClassesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                Classes stations = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(stations);
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
                json.Data = ServiceFactory.Factory.ClassesService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Classes entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IClassesService service = ServiceFactory.Factory.ClassesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                Classes old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    Classes new_entity = new Classes();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    //new_entity.GroupID = entity.GroupID;
                    //new_entity.StoreID = entity.StoreID;
                    new_entity.DeptID = entity.DeptID;
                    new_entity.ClassCode = entity.ClassCode;
                    new_entity.ClassName = entity.ClassName;
                    new_entity.StartHour = entity.StartHour;
                    new_entity.StartMinute = entity.StartMinute;
                    new_entity.OverHour = entity.OverHour;
                    new_entity.OverMinute = entity.OverMinute;
                    new_entity.RestTypeID = entity.RestTypeID;
                    new_entity.RestDays = entity.RestDays;

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

                    old_entity.DeptID = entity.DeptID;
                    old_entity.ClassCode = entity.ClassCode;
                    old_entity.ClassName = entity.ClassName;
                    old_entity.StartHour = entity.StartHour;
                    old_entity.StartMinute = entity.StartMinute;
                    old_entity.OverHour = entity.OverHour;
                    old_entity.OverMinute = entity.OverMinute;
                    old_entity.RestTypeID = entity.RestTypeID;
                    old_entity.RestDays = entity.RestDays;
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