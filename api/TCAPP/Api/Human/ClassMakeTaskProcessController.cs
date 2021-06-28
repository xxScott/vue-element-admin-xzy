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
    public class ClassMakeTaskProcessController : Controller
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

                //condition.Add(new SimpleCondition("GroupID", groupId));
                //condition.Add(new SimpleCondition("StoreID", storeId));
                condition.Add(new SimpleCondition("UserID", Security.CurrentUser.ID));
                condition.Add(new SimpleCondition("DeptID", ConditionOperator.Equals, 0, true));
                //condition.Add(new SimpleCondition("UserID", Security.CurrentUser.ID));
                //  condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_MakeClassState.Plan)));
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
                dat.Columns.Add("SubmitState", typeof(bool));
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
                        row["SubmitState"] = makeClassPlan[i].State == Convert.ToInt16(Enum_MakeClassState.MakeOver);
                        row["EditState"] = makeClassPlan[i].State == Convert.ToInt16(Enum_MakeClassState.MakeOver) || makeClassPlan[i].State == Convert.ToInt16(Enum_MakeClassState.Plan);
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
        public JsonResult GetColumns(int id)
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
                if (makeClass != null)
                {
                    Periods period = ServiceFactory.Factory.PeriodService.SearchOne(new SimpleCondition("ID", makeClass.PeriodID));
                    if (period != null)
                    {
                        DateTime dtStart = DateTime.Parse(period.StartDate.ToString());
                        DateTime dtEnd = DateTime.Parse(period.EndDate.ToString());
                        //int days = DateTime.DaysInMonth(dtStart.Year, dtStart.Month);
                        TimeSpan d3 = dtEnd.Subtract(dtStart);


                        DataTable dat = new DataTable();
                        dat.Columns.Add("text", typeof(string));
                        dat.Columns.Add("field", typeof(string));
                        dat.Columns.Add("title", typeof(string));
                        dat.Columns.Add("width", typeof(int));
                        dat.Columns.Add("formatter", typeof(string));


                        for (int i = 0; i < d3.Days; i++)
                        {
                            DataRow row = dat.NewRow();
                            row["text"] = "ClassName" + (i + 1);
                            row["field"] = "ClassID" + (i + 1);
                            row["title"] = dtStart.AddDays(i).ToString("MM-dd");
                            row["width"] = 0;
                            row["formatter"] = "";
                            dat.Rows.Add(row);
                        }
                        json.Data = JsonUtil.GetSuccessForObject(dat);
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
                IMakeClassPlanService service = ServiceFactory.Factory.MakeClassPlanService;
                DataTable dt = service.Query(id);
                DataView data = dt.Copy().DefaultView;

                DataView dv = dt.DefaultView;
                dv.RowFilter = "UpperID is not null";


                DataTable dat = new DataTable();
                dat.Columns.Add("dataId", typeof(Int32));
                dat.Columns.Add("UpperID", typeof(Int32));
                dat.Columns.Add("EmployeeName", typeof(string));
                dat.Columns.Add("EmpID", typeof(Int32));
                dat.Columns.Add("GroupID", typeof(Int32));
                dat.Columns.Add("StoreID", typeof(Int32));
                dat.Columns.Add("PeriodID", typeof(Int32));
                dat.Columns.Add("DeptID", typeof(Int32));
                dat.Columns.Add("UserID", typeof(Int32));
                dat.Columns.Add("State", typeof(Int32));
                for (int i = 1; i <= 31; i++)
                {
                    dat.Columns.Add("ClassID" + i, typeof(string));
                }

                data.RowFilter = "UpperID is Null ";

                for (int i = 0; i < data.Count; i++)
                {

                    DataRow row = dat.NewRow();
                    row["dataId"] = data[i]["dataId"];
                    row["UpperID"] = data[i]["UpperID"];
                    row["EmployeeName"] = data[i]["EmployeeName"];
                    row["EmpID"] = data[i]["EmpID"];
                    row["GroupID"] = data[i]["GroupID"];
                    row["StoreID"] = data[i]["StoreID"];
                    row["PeriodID"] = data[i]["PeriodID"];
                    row["DeptID"] = data[i]["DeptID"];
                    row["UserID"] = data[i]["UserID"];
                    row["State"] = data[i]["State"];
                    for (int j = 1; j <= 31; j++)
                    {
                        row["ClassID" + j] = data[i]["ClassID" + j];
                    }


                    if (data[i]["dataId"].ToString() != "")
                    {
                        dv.RowFilter = "UpperID=" + data[i]["dataId"];
                        string colum = "";
                        if (dv.Count == 1)
                        {
                            for (int j = 1; j <= 31; j++)
                            {
                                if (Convert.ToInt32(dv[0]["ClassID" + j]) != 0)
                                    colum += "ClassID" + j + "|";
                            }
                            if (colum != "") colum = colum.Substring(0, colum.Length - 1);
                            string[] colums = colum.Split('|');
                            for (int j = 0; j < colums.Length; j++)
                            {
                                row[colums[j].ToString()] += "|" + dv[0][colums[j].ToString()];
                            }
                        }
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

        public JsonResult Submit(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                MakeClassPlan plan = ServiceFactory.Factory.MakeClassPlanService.SearchOne(new SimpleCondition("ID", id));
                if (plan != null)
                {
                    if (plan.State == Convert.ToInt16(Enum_MakeClassState.MakeOver))
                    {
                        plan.State = Convert.ToInt16(Enum_MakeClassState.Wait);
                        plan.UpdateTime = DateTime.Now;
                        plan.Updater = Utilities.Security.CurrentUser.UserName;
                        plan.UpdateIP = Network.ClientIP;
                        ServiceFactory.Factory.MakeClassPlanService.Update(plan);

                        //记录日志
                        MakeClassLog log = new MakeClassLog();
                        log.OccurTime = DateTime.Now;
                        log.MakeClassID = plan.ID;
                        log.SourceState = Convert.ToInt16(Enum_MakeClassState.MakeOver);
                        log.TargetState = Convert.ToInt16(Enum_MakeClassState.Wait);
                        log.UserID = Security.CurrentUser.ID;
                        ServiceFactory.Factory.MakeClassLogService.Insert(log);

                        json.Data = JsonUtil.GetSuccessForString("提交审核成功");
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


        public bool AdjItem(Models.Data.MakeClassData entity)
        {
            if (entity.ClassID1 != null && entity.ClassID1.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID2 != null && entity.ClassID2.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID3 != null && entity.ClassID3.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID4 != null && entity.ClassID4.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID5 != null && entity.ClassID5.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID6 != null && entity.ClassID6.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID7 != null && entity.ClassID7.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID8 != null && entity.ClassID8.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID9 != null && entity.ClassID9.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID10 != null && entity.ClassID10.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID11 != null && entity.ClassID11.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID12 != null && entity.ClassID12.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID13 != null && entity.ClassID13.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID14 != null && entity.ClassID14.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID15 != null && entity.ClassID15.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID16 != null && entity.ClassID16.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID17 != null && entity.ClassID17.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID18 != null && entity.ClassID18.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID19 != null && entity.ClassID19.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID20 != null && entity.ClassID20.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID21 != null && entity.ClassID21.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID22 != null && entity.ClassID22.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID23 != null && entity.ClassID23.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID24 != null && entity.ClassID24.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID25 != null && entity.ClassID25.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID26 != null && entity.ClassID26.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID27 != null && entity.ClassID27.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID28 != null && entity.ClassID28.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID29 != null && entity.ClassID29.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID30 != null && entity.ClassID30.IndexOf("|") > -1)
                return true;
            else if (entity.ClassID31 != null && entity.ClassID31.IndexOf("|") > -1)
                return true;
            else
                return false;
        }


        public int AdjItemCount(Models.Data.MakeClassData entity)
        {
            int max = 0;
            if (entity.ClassID1.IndexOf("|") > -1)
            {
                string classId = entity.ClassID1;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }

            else if (entity.ClassID2.IndexOf("|") > -1)
            {
                string classId = entity.ClassID2;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID3.IndexOf("|") > -1)
            {
                string classId = entity.ClassID3;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID4.IndexOf("|") > -1)
            {
                string classId = entity.ClassID5;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID5.IndexOf("|") > -1)
            {
                string classId = entity.ClassID6;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID6.IndexOf("|") > -1)
            {
                string classId = entity.ClassID7;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID7.IndexOf("|") > -1)
            {
                string classId = entity.ClassID8;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID8.IndexOf("|") > -1)
            {
                string classId = entity.ClassID9;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID9.IndexOf("|") > -1)
            {
                string classId = entity.ClassID10;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID10.IndexOf("|") > -1)
            {
                string classId = entity.ClassID11;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID11.IndexOf("|") > -1)
            {
                string classId = entity.ClassID1;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID12.IndexOf("|") > -1)
            {
                string classId = entity.ClassID12;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID13.IndexOf("|") > -1)
            {
                string classId = entity.ClassID13;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID14.IndexOf("|") > -1)
            {
                string classId = entity.ClassID14;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID15.IndexOf("|") > -1)
            {
                string classId = entity.ClassID15;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID16.IndexOf("|") > -1)
            {
                string classId = entity.ClassID16;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID17.IndexOf("|") > -1)
            {
                string classId = entity.ClassID17;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID18.IndexOf("|") > -1)
            {
                string classId = entity.ClassID18;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID19.IndexOf("|") > -1)
            {
                string classId = entity.ClassID19;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID20.IndexOf("|") > -1)
            {
                string classId = entity.ClassID20;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID21.IndexOf("|") > -1)
            {
                string classId = entity.ClassID21;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID22.IndexOf("|") > -1)
            {
                string classId = entity.ClassID22;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID23.IndexOf("|") > -1)
            {
                string classId = entity.ClassID23;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID24.IndexOf("|") > -1)
            {
                string classId = entity.ClassID24;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID25.IndexOf("|") > -1)
            {
                string classId = entity.ClassID25;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID26.IndexOf("|") > -1)
            {
                string classId = entity.ClassID26;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID27.IndexOf("|") > -1)
            {
                string classId = entity.ClassID27;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID28.IndexOf("|") > -1)
            {
                string classId = entity.ClassID28;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID29.IndexOf("|") > -1)
            {
                string classId = entity.ClassID29;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID30.IndexOf("|") > -1)
            {
                string classId = entity.ClassID30;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            else if (entity.ClassID31.IndexOf("|") > -1)
            {
                string classId = entity.ClassID31;
                string str = classId.Replace("|", "");
                int count = classId.Length - str.Length;
                max = count > max ? count : max;
            }
            return max;
        }


        public JsonResult Save(Models.Data.MakeClassData entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IMakeClassDataService service = ServiceFactory.Factory.MakeClassDataService;

                if (!AdjItem(entity))
                {
                    MakeClassData new_entity = new MakeClassData();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.MakeClassID = entity.MakeClassID;
                    new_entity.EmployeeID = entity.EmployeeID;

                    new_entity.ClassID1 = Convert.ToInt32(entity.ClassID1);
                    new_entity.ClassID2 = Convert.ToInt32(entity.ClassID2);
                    new_entity.ClassID3 = Convert.ToInt32(entity.ClassID3);
                    new_entity.ClassID4 = Convert.ToInt32(entity.ClassID4);
                    new_entity.ClassID5 = Convert.ToInt32(entity.ClassID5);
                    new_entity.ClassID6 = Convert.ToInt32(entity.ClassID6);
                    new_entity.ClassID7 = Convert.ToInt32(entity.ClassID7);
                    new_entity.ClassID8 = Convert.ToInt32(entity.ClassID8);
                    new_entity.ClassID9 = Convert.ToInt32(entity.ClassID9);
                    new_entity.ClassID10 = Convert.ToInt32(entity.ClassID10);

                    new_entity.ClassID11 = Convert.ToInt32(entity.ClassID11);
                    new_entity.ClassID12 = Convert.ToInt32(entity.ClassID12);
                    new_entity.ClassID13 = Convert.ToInt32(entity.ClassID13);
                    new_entity.ClassID14 = Convert.ToInt32(entity.ClassID14);
                    new_entity.ClassID15 = Convert.ToInt32(entity.ClassID15);
                    new_entity.ClassID16 = Convert.ToInt32(entity.ClassID16);
                    new_entity.ClassID17 = Convert.ToInt32(entity.ClassID17);
                    new_entity.ClassID18 = Convert.ToInt32(entity.ClassID18);
                    new_entity.ClassID19 = Convert.ToInt32(entity.ClassID19);
                    new_entity.ClassID20 = Convert.ToInt32(entity.ClassID20);

                    new_entity.ClassID21 = Convert.ToInt32(entity.ClassID21);
                    new_entity.ClassID22 = Convert.ToInt32(entity.ClassID22);
                    new_entity.ClassID23 = Convert.ToInt32(entity.ClassID23);
                    new_entity.ClassID24 = Convert.ToInt32(entity.ClassID24);
                    new_entity.ClassID25 = Convert.ToInt32(entity.ClassID25);
                    new_entity.ClassID26 = Convert.ToInt32(entity.ClassID26);
                    new_entity.ClassID27 = Convert.ToInt32(entity.ClassID27);
                    new_entity.ClassID28 = Convert.ToInt32(entity.ClassID28);
                    new_entity.ClassID29 = Convert.ToInt32(entity.ClassID29);
                    new_entity.ClassID30 = Convert.ToInt32(entity.ClassID30);
                    new_entity.ClassID31 = Convert.ToInt32(entity.ClassID31);


                    new_entity.CreateTime = DateTime.Now;
                    new_entity.Creator = Utilities.Security.CurrentUser.UserName;
                    new_entity.CreateIP = Network.ClientIP;
                    new_entity.UpdateTime = DateTime.Now;
                    new_entity.Updater = Utilities.Security.CurrentUser.UserName;
                    new_entity.UpdateIP = Network.ClientIP;

                    List<MakeClassData> list = new List<MakeClassData>();
                    list.Add(new_entity);
                    service.SaveMakeClassData(list);
                    MakeClassPlan plan = ServiceFactory.Factory.MakeClassPlanService.SearchOne(new SimpleCondition("ID", entity.MakeClassID));
                    if (plan != null)
                    {
                        plan.State = Convert.ToInt16(Enum_MakeClassState.MakeOver);
                        plan.UpdateTime = DateTime.Now;
                        plan.Updater = Utilities.Security.CurrentUser.UserName;
                        plan.UpdateIP = Network.ClientIP;
                        ServiceFactory.Factory.MakeClassPlanService.Update(plan);

                        //记录日志
                        MakeClassLog log = new MakeClassLog();
                        log.OccurTime = DateTime.Now;
                        log.MakeClassID = plan.ID;
                        log.SourceState = Convert.ToInt16(Enum_MakeClassState.Plan);
                        log.TargetState = Convert.ToInt16(Enum_MakeClassState.MakeOver);
                        log.UserID = Security.CurrentUser.ID;
                        ServiceFactory.Factory.MakeClassLogService.Insert(log);
                    }
                    json.Data = JsonUtil.GetSuccessForString("保存已完成");
                }
                else
                {
                    int max = AdjItemCount(entity) + 1;
                    List<MakeClassData> list = new List<MakeClassData>();
                    for (int i = 0; i < max; i++)
                    {
                        MakeClassData _entity = new MakeClassData();

                        //start /insert /groupid&storeid /20190405
                        Users currentUser = Utilities.Security.CurrentUser;
                        int? storeid = currentUser.StoreID;
                        int? groudid = currentUser.GroupID;
                        _entity.GroupID = groudid.HasValue? groudid.Value : -1;
                        _entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                        //end

                        _entity.MakeClassID = entity.MakeClassID;
                        _entity.EmployeeID = entity.EmployeeID;

                        if (entity.ClassID1 != null)
                        {
                            string[] classId1 = entity.ClassID1.Split('|');
                            if (classId1.Length > i)
                            {
                                _entity.ClassID1 = int.Parse(classId1[i]);
                            }
                        }
                        if (entity.ClassID2 != null)
                        {
                            string[] classId2 = entity.ClassID2.Split('|');
                            if (classId2.Length > i)
                            {
                                _entity.ClassID2 = int.Parse(classId2[i]);
                            }
                        }
                        if (entity.ClassID3 != null)
                        {
                            string[] classId3 = entity.ClassID3.Split('|');
                            if (classId3.Length > i)
                            {
                                _entity.ClassID3 = int.Parse(classId3[i]);
                            }
                        }
                        if (entity.ClassID4 != null)
                        {
                            string[] classId4 = entity.ClassID4.Split('|');
                            if (classId4.Length > i)
                            {
                                _entity.ClassID4 = int.Parse(classId4[i]);
                            }
                        }
                        if (entity.ClassID5 != null)
                        {
                            string[] classId5 = entity.ClassID5.Split('|');
                            if (classId5.Length > i)
                            {
                                _entity.ClassID5 = int.Parse(classId5[i]);
                            }
                        }
                        if (entity.ClassID6 != null)
                        {
                            string[] classId6 = entity.ClassID6.Split('|');
                            if (classId6.Length > i)
                            {
                                _entity.ClassID6 = int.Parse(classId6[i]);
                            }
                        }
                        if (entity.ClassID7 != null)
                        {
                            string[] classId7 = entity.ClassID7.Split('|');
                            if (classId7.Length > i)
                            {
                                _entity.ClassID7 = int.Parse(classId7[i]);
                            }
                        }
                        if (entity.ClassID8 != null)
                        {
                            string[] classId8 = entity.ClassID8.Split('|');
                            if (classId8.Length > i)
                            {
                                _entity.ClassID8 = int.Parse(classId8[i]);
                            }
                        }
                        if (entity.ClassID9 != null)
                        {
                            string[] classId9 = entity.ClassID9.Split('|');
                            if (classId9.Length > i)
                            {
                                _entity.ClassID9 = int.Parse(classId9[i]);
                            }
                        }
                        if (entity.ClassID10 != null)
                        {
                            string[] classId10 = entity.ClassID10.Split('|');
                            if (classId10.Length > i)
                            {
                                _entity.ClassID10 = int.Parse(classId10[i]);
                            }
                        }
                        if (entity.ClassID11 != null)
                        {
                            string[] classId11 = entity.ClassID11.Split('|');
                            if (classId11.Length > i)
                            {
                                _entity.ClassID11 = int.Parse(classId11[i]);
                            }
                        }
                        if (entity.ClassID12 != null)
                        {
                            string[] classId12 = entity.ClassID12.Split('|');
                            if (classId12.Length > i)
                            {
                                _entity.ClassID12 = int.Parse(classId12[i]);
                            }
                        }
                        if (entity.ClassID13 != null)
                        {
                            string[] classId13 = entity.ClassID13.Split('|');
                            if (classId13.Length > i)
                            {
                                _entity.ClassID13 = int.Parse(classId13[i]);
                            }
                        }
                        if (entity.ClassID14 != null)
                        {
                            string[] classId14 = entity.ClassID14.Split('|');
                            if (classId14.Length > i)
                            {
                                _entity.ClassID14 = int.Parse(classId14[i]);
                            }
                        }
                        if (entity.ClassID15 != null)
                        {
                            string[] classId15 = entity.ClassID15.Split('|');
                            if (classId15.Length > i)
                            {
                                _entity.ClassID15 = int.Parse(classId15[i]);
                            }
                        }
                        if (entity.ClassID16 != null)
                        {
                            string[] classId16 = entity.ClassID16.Split('|');
                            if (classId16.Length > i)
                            {
                                _entity.ClassID16 = int.Parse(classId16[i]);
                            }
                        }
                        if (entity.ClassID17 != null)
                        {
                            string[] classId17 = entity.ClassID17.Split('|');
                            if (classId17.Length > i)
                            {
                                _entity.ClassID17 = int.Parse(classId17[i]);
                            }
                        }
                        if (entity.ClassID18 != null)
                        {
                            string[] classId18 = entity.ClassID18.Split('|');
                            if (classId18.Length > i)
                            {
                                _entity.ClassID18 = int.Parse(classId18[i]);
                            }
                        }
                        if (entity.ClassID19 != null)
                        {
                            string[] classId19 = entity.ClassID19.Split('|');
                            if (classId19.Length > i)
                            {
                                _entity.ClassID19 = int.Parse(classId19[i]);
                            }
                        }
                        if (entity.ClassID20 != null)
                        {
                            string[] classId20 = entity.ClassID20.Split('|');
                            if (classId20.Length > i)
                            {
                                _entity.ClassID20 = int.Parse(classId20[i]);
                            }
                        }
                        if (entity.ClassID21 != null)
                        {
                            string[] classId21 = entity.ClassID21.Split('|');
                            if (classId21.Length > i)
                            {
                                _entity.ClassID21 = int.Parse(classId21[i]);
                            }
                        }
                        if (entity.ClassID22 != null)
                        {
                            string[] classId22 = entity.ClassID22.Split('|');
                            if (classId22.Length > i)
                            {
                                _entity.ClassID22 = int.Parse(classId22[i]);
                            }
                        }
                        if (entity.ClassID23 != null)
                        {
                            string[] classId23 = entity.ClassID23.Split('|');
                            if (classId23.Length > i)
                            {
                                _entity.ClassID23 = int.Parse(classId23[i]);
                            }
                        }
                        if (entity.ClassID24 != null)
                        {
                            string[] classId24 = entity.ClassID24.Split('|');
                            if (classId24.Length > i)
                            {
                                _entity.ClassID24 = int.Parse(classId24[i]);
                            }
                        }
                        if (entity.ClassID25 != null)
                        {
                            string[] classId25 = entity.ClassID25.Split('|');
                            if (classId25.Length > i)
                            {
                                _entity.ClassID25 = int.Parse(classId25[i]);
                            }
                        }
                        if (entity.ClassID26 != null)
                        {
                            string[] classId26 = entity.ClassID26.Split('|');
                            if (classId26.Length > i)
                            {
                                _entity.ClassID26 = int.Parse(classId26[i]);
                            }
                        }
                        if (entity.ClassID27 != null)
                        {
                            string[] classId27 = entity.ClassID27.Split('|');
                            if (classId27.Length > i)
                            {
                                _entity.ClassID27 = int.Parse(classId27[i]);
                            }
                        }
                        if (entity.ClassID28 != null)
                        {
                            string[] classId28 = entity.ClassID28.Split('|');
                            if (classId28.Length > i)
                            {
                                _entity.ClassID28 = int.Parse(classId28[i]);
                            }
                        }
                        if (entity.ClassID29 != null)
                        {
                            string[] classId29 = entity.ClassID29.Split('|');
                            if (classId29.Length > i)
                            {
                                _entity.ClassID29 = int.Parse(classId29[i]);
                            }
                        }
                        if (entity.ClassID30 != null)
                        {
                            string[] classId30 = entity.ClassID30.Split('|');
                            if (classId30.Length > i)
                            {
                                _entity.ClassID30 = int.Parse(classId30[i]);
                            }
                        }
                        if (entity.ClassID31 != null)
                        {
                            string[] classId31 = entity.ClassID31.Split('|');
                            if (classId31.Length > i)
                            {
                                _entity.ClassID31 = int.Parse(classId31[i]);
                            }
                        }
                        list.Add(_entity);
                    }
                    service.SaveMakeClassData(list);
                    MakeClassPlan plan = ServiceFactory.Factory.MakeClassPlanService.SearchOne(new SimpleCondition("ID", entity.MakeClassID));
                    if (plan != null)
                    {
                        plan.State = Convert.ToInt16(Enum_MakeClassState.MakeOver);
                        plan.UpdateTime = DateTime.Now;
                        plan.Updater = Utilities.Security.CurrentUser.UserName;
                        plan.UpdateIP = Network.ClientIP;
                        ServiceFactory.Factory.MakeClassPlanService.Update(plan);

                        //记录日志
                        MakeClassLog log = new MakeClassLog();
                        log.OccurTime = DateTime.Now;
                        log.MakeClassID = plan.ID;
                        log.SourceState = Convert.ToInt16(Enum_MakeClassState.Plan);
                        log.TargetState = Convert.ToInt16(Enum_MakeClassState.MakeOver);
                        log.UserID = Security.CurrentUser.ID;
                        ServiceFactory.Factory.MakeClassLogService.Insert(log);
                    }
                    json.Data = JsonUtil.GetSuccessForString("保存已完成");
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