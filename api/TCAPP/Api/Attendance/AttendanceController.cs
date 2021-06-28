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
    public class AttendanceController : Controller
    {
        public JsonResult GetColumns(int id)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                Periods period = ServiceFactory.Factory.PeriodService.SearchOne(new SimpleCondition("ID", id));
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
                        row["field"] = "Data" + (i + 1);
                        row["title"] = dtStart.AddDays(i).ToString("MM-dd");
                        row["width"] = 0;
                        row["formatter"] = "";
                        dat.Rows.Add(row);
                    }
                    json.Data = JsonUtil.GetSuccessForObject(dat);
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult Get(int storeId, int periodId, string keyword)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IAttendPeriodDataService service = ServiceFactory.Factory.AttendPeriodDataService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("StoreID", storeId));
                condition.Add(new SimpleCondition("PeroidID", periodId));
                if (keyword != null && keyword != string.Empty)
                {
                    ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                    _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                    condition.Add(_condition);
                }
                List<AttendPeriodData> list = service.Search(condition);
                json.Data = JsonUtil.GetSuccessForObject(list);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult Save(AttendPeriodData entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IAttendPeriodDataService service = ServiceFactory.Factory.AttendPeriodDataService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                AttendPeriodData old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    AttendPeriodData new_entity = new AttendPeriodData();
                    new_entity.StoreID = entity.StoreID;
                    new_entity.PeroidID = entity.PeroidID;
                    new_entity.DeptID = entity.DeptID;
                    new_entity.StartDate = entity.StartDate;
                    new_entity.EndDate = entity.EndDate;
                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.EmployeeName = entity.EmployeeName;
                    new_entity.State = 0;



                    new_entity.Data1 = entity.Data1;
                    new_entity.Data2 = entity.Data2;
                    new_entity.Data3 = entity.Data3;
                    new_entity.Data4 = entity.Data4;
                    new_entity.Data5 = entity.Data5;
                    new_entity.Data6 = entity.Data6;
                    new_entity.Data7 = entity.Data7;
                    new_entity.Data8 = entity.Data8;
                    new_entity.Data9 = entity.Data9;
                    new_entity.Data10 = entity.Data10;

                    new_entity.Data11 = entity.Data11;
                    new_entity.Data12 = entity.Data12;
                    new_entity.Data13 = entity.Data13;
                    new_entity.Data14 = entity.Data14;
                    new_entity.Data15 = entity.Data15;
                    new_entity.Data16 = entity.Data16;
                    new_entity.Data17 = entity.Data17;
                    new_entity.Data18 = entity.Data18;
                    new_entity.Data19 = entity.Data19;
                    new_entity.Data20 = entity.Data20;

                    new_entity.Data21 = entity.Data21;
                    new_entity.Data22 = entity.Data22;
                    new_entity.Data23 = entity.Data23;
                    new_entity.Data24 = entity.Data24;
                    new_entity.Data25 = entity.Data25;
                    new_entity.Data26 = entity.Data26;
                    new_entity.Data27 = entity.Data27;
                    new_entity.Data28 = entity.Data28;
                    new_entity.Data29 = entity.Data29;
                    new_entity.Data30 = entity.Data30;
                    new_entity.Data31 = entity.Data31;
                    service.Insert(new_entity);
                    json.Data = JsonUtil.GetSuccessForString("新增已完成");
                }
                else
                {

                    old_entity.Data1 = entity.Data1;
                    old_entity.Data2 = entity.Data2;
                    old_entity.Data3 = entity.Data3;
                    old_entity.Data4 = entity.Data4;
                    old_entity.Data5 = entity.Data5;
                    old_entity.Data6 = entity.Data6;
                    old_entity.Data7 = entity.Data7;
                    old_entity.Data8 = entity.Data8;
                    old_entity.Data9 = entity.Data9;
                    old_entity.Data10 = entity.Data10;

                    old_entity.Data11 = entity.Data11;
                    old_entity.Data12 = entity.Data12;
                    old_entity.Data13 = entity.Data13;
                    old_entity.Data14 = entity.Data14;
                    old_entity.Data15 = entity.Data15;
                    old_entity.Data16 = entity.Data16;
                    old_entity.Data17 = entity.Data17;
                    old_entity.Data18 = entity.Data18;
                    old_entity.Data19 = entity.Data19;
                    old_entity.Data20 = entity.Data20;

                    old_entity.Data21 = entity.Data21;
                    old_entity.Data22 = entity.Data22;
                    old_entity.Data23 = entity.Data23;
                    old_entity.Data24 = entity.Data24;
                    old_entity.Data25 = entity.Data25;
                    old_entity.Data26 = entity.Data26;
                    old_entity.Data27 = entity.Data27;
                    old_entity.Data28 = entity.Data28;
                    old_entity.Data29 = entity.Data29;
                    old_entity.Data30 = entity.Data30;
                    old_entity.Data31 = entity.Data31;
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