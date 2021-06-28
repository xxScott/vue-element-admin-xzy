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
    public class FormationController : Controller
    {
        public JsonResult QueryTreeData(string adapter, string groupId, string storeId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {

                Enum_Adapter data_adapter = Enum_Adapter.None;
                if (adapter != null)
                    data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);
                if (data_adapter == Enum_Adapter.Tree)
                {
                    JArray array = new TreeAdapter().GetGroupAndDept(groupId, storeId);
                    json.Data = JsonUtil.GetSuccessForObject(array, array.Count);
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

        public JsonResult Get(string deptId, string groupId, string storeId)
        {


            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IWorkStationsService service = ServiceFactory.Factory.WorkStationsService;
                ConditionSet condition = new ConditionSet();
                
                //start /query /groupid&storeid /20190405
                GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                //end

                //condition.Add(new SimpleCondition("GroupID", groupId));
                //condition.Add(new SimpleCondition("StoreID", storeId));
                //查询记录总数
                int totalCount = service.Count(condition);
                //查询分页数据
                List<WorkStations> stations = service.Search(condition);

                IFormationsService serviceStation = ServiceFactory.Factory.FormationsService;

                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("StationName", typeof(string));
                dat.Columns.Add("Count", typeof(Int16));

                for (int i = 0; i < stations.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = stations[i].ID;
                    row["StationName"] = stations[i].StationName;
                    row["Count"] = 0;

                    condition = new ConditionSet();
                    condition.Add(new SimpleCondition("DeptID", deptId));
                    condition.Add(new SimpleCondition("GroupID", groupId));
                    condition.Add(new SimpleCondition("StoreID", storeId));
                    condition.Add(new SimpleCondition("StationID", stations[i].ID));
                    Formations farmation = serviceStation.SearchOne(condition);
                    if (farmation != null)
                    {
                        row["Count"] = farmation.PersonNum;
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
        public JsonResult Save(List<Formations> entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                for (int i = 0; i < entity.Count; i++)
                {
                    IFormationsService service = ServiceFactory.Factory.FormationsService;
                    ConditionSet condition = new ConditionSet();
                    condition.Add(new SimpleCondition("DeptID", entity[i].DeptID));
                    condition.Add(new SimpleCondition("StationID", entity[i].StationID));
                    condition.Add(new SimpleCondition("GroupID", entity[i].GroupID));
                    condition.Add(new SimpleCondition("StoreID", entity[i].StoreID));
                    //查询数据
                    Formations old_entity = service.SearchOne(condition);
                    if (old_entity == null)
                    {
                        Formations new_entity = new Formations();

                        new_entity.GroupID = entity[i].GroupID;
                        new_entity.DeptID = entity[i].DeptID;
                        new_entity.StoreID = entity[i].StoreID;
                        new_entity.StationID = entity[i].StationID;
                        new_entity.PersonNum = entity[i].PersonNum;

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

                        old_entity.DeptID = entity[i].DeptID;
                        old_entity.GroupID = entity[i].GroupID;
                        old_entity.StoreID = entity[i].StoreID;
                        old_entity.StationID = entity[i].StationID;
                        old_entity.PersonNum = entity[i].PersonNum;

                        old_entity.UpdateTime = DateTime.Now;
                        old_entity.Updater = Utilities.Security.CurrentUser.UserName;
                        old_entity.UpdateIP = Network.ClientIP;
                        service.Update(old_entity);
                        json.Data = JsonUtil.GetSuccessForString("修改已完成");
                    }
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