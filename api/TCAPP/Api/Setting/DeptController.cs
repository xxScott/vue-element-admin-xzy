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
using Newtonsoft.Json;

namespace HrApp.Api
{
    public class DeptController : Controller
    {
        public JsonResult Query(int groupId, int storeId, string keyword, string adapter, int? pageNumber, int? pageSize)
        {
            if (groupId == -1 && storeId == -1)
            {
                
                Users currentUser = Utilities.Security.CurrentUser;
                int? storeid = currentUser.StoreID;
                int? groudid = currentUser.GroupID;

                storeId = storeid.HasValue ? storeid.Value : -1;
                groupId = groudid.HasValue ? groudid.Value : -1;

            }
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
                        ConditionSet condition = new ConditionSet();

                        //start /query /groupid&storeid /20190405
                        GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                        //end

                        //condition.Add(new SimpleCondition("GroupID", groupId));
                        //condition.Add(new SimpleCondition("StoreID", storeId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                            _condition.Add(new SimpleCondition("DeptCode", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("DeptName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                            condition.Add(_condition);
                        }
                        List<Depts> depts = ServiceFactory.Factory.DeptsService.Search(condition);
                        json.Data = JsonUtil.GetSuccessForObject(depts);
                    }
                    else if (data_adapter == Enum_Adapter.TreeGrid)
                    {
                        JArray array = new TreeGridAdapter().GetDept(groupId, storeId, keyword);
                        json.Data = JsonUtil.GetSuccessForObject(array, array.Count);
                    }
                    else if (data_adapter == Enum_Adapter.ComboTree)
                    {
                        if (Utilities.Security.HaveGroupAccount && Utilities.Security.IsGroupAccount)
                        {
                            JArray array = new ComboTreeAdapter().GetStore(groupId, storeId);
                            json.Data = JsonUtil.GetSuccessForObject(array);
                        }
                        else
                        {
                            JArray array = new ComboTreeAdapter().GetDept(groupId, storeId);
                            json.Data = JsonUtil.GetSuccessForObject(array);
                        }
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
                            data_adapter = (Enum_Adapter)Convert.ToInt16(req_data_array[i]);

                        if (data_adapter == Enum_Adapter.None)
                        {
                            ConditionSet condition = new ConditionSet();
                            condition.Add(new SimpleCondition("GroupID", groupId));
                            condition.Add(new SimpleCondition("StoreID", storeId));
                            if (keyword != null && keyword != string.Empty)
                            {
                                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                                _condition.Add(new SimpleCondition("DeptCode", ConditionOperator.Like, "%" + keyword + "%"));
                                _condition.Add(new SimpleCondition("DeptName", ConditionOperator.Like, "%" + keyword + "%"));
                                _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                                condition.Add(_condition);
                            }
                            List<Depts> depts = ServiceFactory.Factory.DeptsService.Search(condition);
                            data["DeptData" + (int)data_adapter] = JArray.Parse(JsonConvert.SerializeObject(depts));
                        }
                        else if (data_adapter == Enum_Adapter.TreeGrid)
                        {
                            JArray array = new TreeGridAdapter().GetDept(groupId, storeId, keyword);
                            data["DeptData" + (int)data_adapter] = array;
                        }
                        else if (data_adapter == Enum_Adapter.ComboTree)
                        {
                            JArray array = new ComboTreeAdapter().GetDept(groupId, storeId);
                            data["DeptData" + (int)data_adapter] = array;
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
        public JsonResult GetDept(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("id", id));
            List<Depts> depts = ServiceFactory.Factory.DeptsService.Search(condition);
            json.Data = JsonUtil.GetSuccessForObject(depts);
            return json;
        }
        public JsonResult Get(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IDeptsService service = ServiceFactory.Factory.DeptsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                //查询数据
                Depts entity = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(entity);
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
                json.Data = ServiceFactory.Factory.DeptsService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Depts entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IDeptsService service = ServiceFactory.Factory.DeptsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                Depts old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    Depts new_entity = new Depts();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue ? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    //new_entity.GroupID = entity.GroupID;
                    //new_entity.StoreID = entity.StoreID;
                    new_entity.DeptCode = entity.DeptCode;
                    new_entity.DeptName = entity.DeptName;
                    new_entity.UpperDeptID = entity.UpperDeptID;
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
                    old_entity.GroupID = groudid.HasValue ? groudid.Value : -1;
                    old_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    old_entity.DeptCode = entity.DeptCode;
                    old_entity.DeptName = entity.DeptName;
                    old_entity.UpperDeptID = entity.UpperDeptID;
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