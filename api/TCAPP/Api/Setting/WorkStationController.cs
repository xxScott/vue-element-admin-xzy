using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;

using Utilities;
using Adapters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HrApp.Api
{
    public class WorkStationController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, string adapter, int? pageNumber, int? pageSize)
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
                        IWorkStationsService service = ServiceFactory.Factory.WorkStationsService;
                        ConditionSet condition = new ConditionSet();

                        //start /query /groupid&storeid /20190405
                        GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                        //end

                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                            _condition.Add(new SimpleCondition("StationCode", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("StationName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<WorkStations> stations = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                        json.Data = JsonUtil.GetSuccessForObject(stations, totalCount);

                    }
                    else if (data_adapter == Enum_Adapter.Combobox)
                    {
                        JArray array = new ComboboxAdapter().GetWorkStation(groupId,storeId);
                        json.Data = JsonUtil.GetSuccessForObject(array);
                    }
                    else
                    {
                        json.Data = JsonUtil.GetFailForString("数据加载失败");
                    }
                }
                else
                {
                    JObject data = new JObject();
                    string[] req_data_array = adapter.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < req_data_array.Length; i++)
                    {
                        Enum_Adapter data_adapter = Enum_Adapter.None;
                        if (adapter != null)
                            data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);

                        if (data_adapter == Enum_Adapter.None)
                        {
                            IWorkStationsService service = ServiceFactory.Factory.WorkStationsService;
                            ConditionSet condition = new ConditionSet();

                            //start /query /groupid&storeid /20190405
                            GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                            //end

                            //condition.Add(new SimpleCondition("GroupID", groupId));
                            //condition.Add(new SimpleCondition("StoreID", storeId));
                            if (keyword != null && keyword != string.Empty)
                            {
                                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                                _condition.Add(new SimpleCondition("StationCode", ConditionOperator.Like, "%" + keyword + "%"));
                                _condition.Add(new SimpleCondition("StationName", ConditionOperator.Like, "%" + keyword + "%"));
                                _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                                condition.Add(_condition);
                            }

                            //查询分页数据
                            List<WorkStations> stations = service.Search(condition);
                            data["DeptData" + (int)data_adapter] = JArray.Parse(JsonConvert.SerializeObject(stations));

                        }
                        else if (data_adapter == Enum_Adapter.Combobox)
                        {
                            JArray array = new ComboboxAdapter().GetWorkStation(groupId,storeId);
                            data["DeptData" + (int)data_adapter] = array;
                        }
                        else
                        {
                            json.Data = JsonUtil.GetFailForString("数据加载失败");
                        }
                    }
                    json.Data = JsonUtil.GetSuccessForObject(data);
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
                IWorkStationsService service = ServiceFactory.Factory.WorkStationsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                WorkStations stations = service.SearchOne(condition);
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
                json.Data = ServiceFactory.Factory.WorkStationsService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(WorkStations entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IWorkStationsService service = ServiceFactory.Factory.WorkStationsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                WorkStations old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    WorkStations new_entity = new WorkStations();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.StationCode = entity.StationCode;
                    new_entity.StationName = entity.StationName;
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

                    old_entity.StationCode = entity.StationCode;
                    old_entity.StationName = entity.StationName;
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