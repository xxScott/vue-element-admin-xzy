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
using Newtonsoft.Json.Linq;
using Utilities;
using Adapters;

namespace HrApp.Api.Setting
{
    public class PeriodController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, string adapter, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json._dateFormat = "yyyy-MM-dd";
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
                        IPeriodService service = ServiceFactory.Factory.PeriodService;
                        ConditionSet condition = new ConditionSet();
                        
                        //if (groupId != "-1")
                        //    storeId = "-1";
                        condition.Add(new SimpleCondition("GroupID", groupId));
                        condition.Add(new SimpleCondition("StoreID", storeId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                            _condition.Add(new SimpleCondition("PeriodName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("StartDate", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("EndDate", ConditionOperator.Like, "%" + keyword + "%"));
                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<Periods> roles = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                        json.Data = JsonUtil.GetSuccessForObject(roles, totalCount);

                    }
                    else if (data_adapter == Enum_Adapter.Combobox)
                    {
                        JArray array = new ComboboxAdapter().GetPeriod(groupId, storeId);
                        json.Data = JsonUtil.GetSuccessForObject(array);
                    }
                    else
                    {
                        json.Data = JsonUtil.GetFailForString("数据加载失败");
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
            json._dateFormat = "yyyy-MM-dd";
            json.ContentType = "text/plain";

            try
            {
                IPeriodService service = ServiceFactory.Factory.PeriodService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                Periods period = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(period);
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
                json.Data = ServiceFactory.Factory.PeriodService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Periods entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IPeriodService service = ServiceFactory.Factory.PeriodService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                Periods old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    Periods new_entity = new Periods();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    //new_entity.GroupID = entity.GroupID;
                    //new_entity.StoreID = entity.StoreID;
                    new_entity.PeriodName = entity.PeriodName;
                    new_entity.StartDate = (DateTime)entity.StartDate;
                    new_entity.EndDate = (DateTime)entity.EndDate;

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

                    old_entity.PeriodName = entity.PeriodName;
                    old_entity.StartDate = entity.StartDate;
                    old_entity.EndDate = entity.EndDate;
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