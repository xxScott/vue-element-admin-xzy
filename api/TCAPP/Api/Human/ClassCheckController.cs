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

    public class ClassCheckController : Controller
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
                //condition.Add(new SimpleCondition("UserID", Security.CurrentUser.ID));
                condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_MakeClassState.Wait)));
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
                        row["EditState"] = makeClassPlan[i].State == Convert.ToInt16(Enum_MakeClassState.MakeOver);
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


        public JsonResult Check(int id, int state)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                MakeClassPlan plan = ServiceFactory.Factory.MakeClassPlanService.SearchOne(new SimpleCondition("ID", id));
                if (plan != null)
                {
                    if (plan.State == Convert.ToInt16(Enum_MakeClassState.Wait))
                    {
                        plan.State = state;
                        plan.UpdateTime = DateTime.Now;
                        plan.Updater = Utilities.Security.CurrentUser.UserName;
                        plan.UpdateIP = Network.ClientIP;
                        ServiceFactory.Factory.MakeClassPlanService.Update(plan);

                        //记录日志
                        MakeClassLog log = new MakeClassLog();
                        log.OccurTime = DateTime.Now;
                        log.MakeClassID = plan.ID;
                        log.SourceState = Convert.ToInt16(Enum_MakeClassState.Wait);
                        log.TargetState = state;
                        log.UserID = Security.CurrentUser.ID;
                        ServiceFactory.Factory.MakeClassLogService.Insert(log);

                        json.Data = JsonUtil.GetSuccessForString("审核成功");
                    }
                    else
                    {
                        json.Data = JsonUtil.GetSuccessForString("数据状态错误");
                    }
                }
                else
                {
                    json.Data = JsonUtil.GetSuccessForString("未找到数据");
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