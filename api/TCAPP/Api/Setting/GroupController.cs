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

namespace HrApp.Api
{
    public class GroupController : Controller
    {
        public JsonResult Query(string keyword, string adapter, int? pageNumber, int? pageSize)
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
                    IGroupsService service = ServiceFactory.Factory.GroupsService;
                    ConditionSet condition = new ConditionSet();

                    //start /query /groupid&storeid /20190405
                    GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                    //end

                    if (keyword != null && keyword != string.Empty)
                    {
                        ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                        _condition.Add(new SimpleCondition("CompanyCode", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("CompanyName", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                        condition.Add(_condition);
                    }
                    //查询记录总数
                    int totalCount = service.Count(condition);
                    //查询分页数据
                    List<Groups> roles = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                    json.Data = JsonUtil.GetSuccessForObject(roles, totalCount);
                }
                else if (data_adapter == Enum_Adapter.DataList)
                {
                    JArray array = new DataListAdapter().GetGroup();
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    JObject obj = array[0] as JObject;
                    if (obj != null)
                    {
                        obj["storeid"] = storeid.HasValue ? storeid.Value.ToString() : string.Empty;
                        obj["groudid"] = groudid.HasValue ? groudid.Value.ToString() : string.Empty;
                    }

                    json.Data = JsonUtil.GetSuccessForObject(array);
                }
                else if (data_adapter == Enum_Adapter.Tree)
                {
                    JArray array = new TreeAdapter().GetGroupAndStore();
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    JObject obj = array[0] as JObject;
                    if (obj != null)
                    {
                        obj["storeid"] = storeid.HasValue ? storeid.Value.ToString() : string.Empty;
                        obj["groudid"] = groudid.HasValue ? groudid.Value.ToString() : string.Empty;
                    }
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

        public JsonResult QueryAdmin()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {

                JArray array = new DataListAdapter().GetGroupAdmin();
                json.Data = JsonUtil.GetSuccessForObject(array);
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
                IGroupsService service = ServiceFactory.Factory.GroupsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                Groups company = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(company);
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
                json.Data = ServiceFactory.Factory.GroupsService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Groups entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IGroupsService service = ServiceFactory.Factory.GroupsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                Groups old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    Groups new_entity = new Groups();

                    //start /insert /groupid&storeid /20190405
                    //Users currentUser = Utilities.Security.CurrentUser;
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;
                    //new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    //new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.UID = entity.UID;
                    new_entity.GroupName = entity.GroupName;
                  
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
                    //Users currentUser = Utilities.Security.CurrentUser;
                    //int? storeid = currentUser.StoreID;
                    //int? groudid = currentUser.GroupID;
                    //old_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    //old_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    old_entity.UID = entity.UID;
                    old_entity.GroupName = entity.GroupName;
                   
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