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
    public class ClassMakeTaskController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {

                IMakeClassPlanService service = ServiceFactory.Factory.MakeClassPlanService;
                ConditionSet condition = new ConditionSet();

                //start /query /groupid&storeid /20190405
                GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                //end

                //condition.Add(new SimpleCondition("StoreID", storeId));
                condition.Add(new SimpleCondition("DeptID", ConditionOperator.Equals, 0, true));
                if (keyword != null && keyword != string.Empty)
                {
                    ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                    _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                    condition.Add(_condition);
                }
                //查询记录总数
                int totalCount = service.Count(condition);
                //查询分页数据
                List<MakeClassPlanView> makeClassPlan = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                //数据转换，查询枚举描述
                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int16));
                dat.Columns.Add("PeriodName", typeof(string));
                dat.Columns.Add("DeptName", typeof(string));
                dat.Columns.Add("UserName", typeof(string));
                dat.Columns.Add("State", typeof(int));
                dat.Columns.Add("UpdateTime", typeof(string));
                dat.Columns.Add("Description", typeof(string));
                dat.Columns.Add("StateName", typeof(string));
                dat.Columns.Add("EditState", typeof(bool));
                for (int i = 0; i < makeClassPlan.Count; i++)
                {
                    DataRow row = dat.NewRow();
                    row["ID"] = makeClassPlan[i].ID;
                    row["PeriodName"] = makeClassPlan[i].PeriodName;
                    row["DeptName"] = makeClassPlan[i].DeptName;
                    row["UserName"] = makeClassPlan[i].UserName;
                    if (makeClassPlan[i].State != null)
                    {
                        row["State"] = makeClassPlan[i].State;
                        row["StateName"] = DAL.Util.GetDisplayName((Enum_MakeClassState)makeClassPlan[i].State);
                        row["EditState"] = makeClassPlan[i].State == Convert.ToInt16(Enum_MakeClassState.Plan);
                    }
                    row["UpdateTime"] = makeClassPlan[i].UpdateTime;
                    row["Description"] = makeClassPlan[i].Description;
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


        public JsonResult Get(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IMakeClassPlanService service = ServiceFactory.Factory.MakeClassPlanService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                MakeClassPlan makeClass = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(makeClass);
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
                json.Data = ServiceFactory.Factory.MakeClassPlanService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Save(MakeClassPlan entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IMakeClassPlanService service = ServiceFactory.Factory.MakeClassPlanService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                MakeClassPlan old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    MakeClassPlan new_entity = new MakeClassPlan();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    //new_entity.GroupID = entity.GroupID;
                    //new_entity.StoreID = entity.StoreID;
                    new_entity.PeriodID = entity.PeriodID;
                    new_entity.DeptID = entity.DeptID;
                    new_entity.UserID = entity.UserID;
                    new_entity.State = Convert.ToInt16(Enum_MakeClassState.Plan);



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

                    old_entity.PeriodID = entity.PeriodID;
                    old_entity.DeptID = entity.DeptID;
                    old_entity.UserID = entity.UserID;

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