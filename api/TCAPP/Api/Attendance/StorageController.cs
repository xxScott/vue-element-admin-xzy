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
    public class StorageController : Controller
    {
        public JsonResult Query(string companyId, string keyword, string adapter, int? pageNumber, int? pageSize)
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
                        IAttendStorageService service = ServiceFactory.Factory.AttendStorageService;
                        ConditionSet condition = new ConditionSet();
                        condition.Add(new SimpleCondition("CompanyID", companyId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                            _condition.Add(new SimpleCondition("Src", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("DataSource", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("CatalogName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("UserID", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Password", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));

                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<AttendStorage> storage = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                        //数据转换，查询枚举描述
                        DataTable dat = new DataTable();
                        dat.Columns.Add("ID", typeof(Int16));
                        dat.Columns.Add("StorageMode", typeof(Int16));
                        dat.Columns.Add("ModeName", typeof(string));
                        dat.Columns.Add("StorageType", typeof(Int16));
                        dat.Columns.Add("TypeName", typeof(string));
                        dat.Columns.Add("Src", typeof(string));
                        dat.Columns.Add("DataSource", typeof(string));
                        dat.Columns.Add("CatalogName", typeof(string));
                        dat.Columns.Add("UserID", typeof(string));
                        dat.Columns.Add("Password", typeof(string));
                        dat.Columns.Add("Description", typeof(string));


                        for (int i = 0; i < storage.Count; i++)
                        {
                            DataRow row = dat.NewRow();
                            row["ID"] = storage[i].ID;
                            row["StorageMode"] = storage[i].StorageMode;
                            row["StorageMode"] = storage[i].StorageMode;
                            if (storage[i].StorageType != null)
                            {
                                row["StorageType"] = storage[i].StorageType;
                                row["TypeName"] = DAL.Util.GetDisplayName((Enum_Sex)storage[i].StorageType);
                            }
                            row["Src"] = storage[i].Src;
                            row["DataSource"] = storage[i].DataSource;
                            row["CatalogName"] = storage[i].CatalogName;
                            row["UserID"] = storage[i].UserID;
                            row["Password"] = storage[i].Password;

                            row["Description"] = storage[i].Description;
                            dat.Rows.Add(row);
                        }
                        json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
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