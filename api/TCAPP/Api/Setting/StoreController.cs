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

namespace HrApp.Api.Setting
{
    public class StoreController : Controller
    {
        public JsonResult Query(string keyword, string adapter, int? pageNumber, int? pageSize, string groupId)
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
                    IStoresService service = ServiceFactory.Factory.StoresService;
                    ConditionSet condition = new ConditionSet();

                    //start /query /groupid&storeid /20190405
                    //GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                    condition.Add(new SimpleCondition("GroupID", groupId));
                    //end

                    //condition.Add(new SimpleCondition("GroupID", groupId == "0" ? "-1" : groupId));
                    if (keyword != null && keyword != string.Empty)
                    {
                        ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                        _condition.Add(new SimpleCondition("StoreCode", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("StoreName", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                        condition.Add(_condition);
                    }
                    //查询记录总数
                    int totalCount = service.Count(condition);
                    //查询分页数据
                    List<Stores> roles = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                    json.Data = JsonUtil.GetSuccessForObject(roles, totalCount);
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
                IStoresService service = ServiceFactory.Factory.StoresService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                Stores store = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(store);
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
                json.Data = ServiceFactory.Factory.StoresService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Stores entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IStoresService service = ServiceFactory.Factory.StoresService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                Stores old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    Stores new_entity = new Stores();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    //new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.UID = entity.UID;
                    new_entity.StoreName = entity.StoreName;
                    //new_entity.GroupID = entity.GroupID;
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
                    //old_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    old_entity.UID = entity.UID;
                    old_entity.StoreName = entity.StoreName;
                    //old_entity.GroupID = entity.GroupID;
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