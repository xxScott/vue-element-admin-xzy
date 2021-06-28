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
    public class RalationController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string employeeId, string keyword, string adapter, int? pageNumber, int? pageSize)
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
                        IEmployeeRalationService service = ServiceFactory.Factory.EmployeeRalationService;
                        ConditionSet condition = new ConditionSet();

                        //start /query /groupid&storeid /20190405
                        GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                        //end

                        condition.Add(new SimpleCondition("EmployeeID", employeeId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                            _condition.Add(new SimpleCondition("RelationType", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("RelationName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("RelationPhone", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("RelationWorkCompany", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("RelationAddress", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));

                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<EmployeeRalationView> empRalationes = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                        //数据转换，查询枚举描述
                        DataTable dat = new DataTable();
                        dat.Columns.Add("ID", typeof(Int16));
                        dat.Columns.Add("RelationName", typeof(string));
                        dat.Columns.Add("RelationType", typeof(string));
                        dat.Columns.Add("EmployeeCode", typeof(string));
                        dat.Columns.Add("EmployeeName", typeof(string));
                        dat.Columns.Add("RelationSex", typeof(string));
                        dat.Columns.Add("RelationPhone", typeof(string));
                        dat.Columns.Add("RelationWorkCompany", typeof(string));
                        dat.Columns.Add("UpdateTime", typeof(string));
                        dat.Columns.Add("Description", typeof(string));
                        dat.Columns.Add("SexName", typeof(string));

                        for (int i = 0; i < empRalationes.Count; i++)
                        {
                            DataRow row = dat.NewRow();
                            row["ID"] = empRalationes[i].ID;
                            row["RelationName"] = empRalationes[i].RelationName;
                            row["RelationType"] = empRalationes[i].RelationType;
                            if (empRalationes[i].RelationSex != null)
                            {
                                row["RelationSex"] = empRalationes[i].RelationSex;
                                row["SexName"] = DAL.Util.GetDisplayName((Enum_Sex)empRalationes[i].RelationSex);
                            }
                            row["EmployeeCode"] = empRalationes[i].EmployeeCode;
                            row["EmployeeName"] = empRalationes[i].EmployeeName;
                            row["RelationPhone"] = empRalationes[i].RelationPhone;
                            row["RelationWorkCompany"] = empRalationes[i].RelationWorkCompany;
                            row["UpdateTime"] = empRalationes[i].UpdateTime;

                            row["Description"] = empRalationes[i].Description;
                            dat.Rows.Add(row);
                        }
                        json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
                    }
                    if (data_adapter == Enum_Adapter.TreeGrid)
                    {
                        int totalCount = 0;
                        JArray array = new TreeGridAdapter().GetEmpRalation(Convert.ToInt16(groupId), Convert.ToInt16(storeId), keyword, pageNumber, pageSize, out totalCount);
                        json.Data = JsonUtil.GetSuccessForObject(array, totalCount);
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
                IEmployeeRalationService service = ServiceFactory.Factory.EmployeeRalationService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeRalation empRalation = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(empRalation);
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
                json.Data = ServiceFactory.Factory.EmployeeRalationService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Save(EmployeeRalation entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IEmployeeRalationService service = ServiceFactory.Factory.EmployeeRalationService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                EmployeeRalationView old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    EmployeeRalationView new_entity = new EmployeeRalationView();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.RelationType = entity.RelationType;
                    new_entity.RelationName = entity.RelationName;
                    new_entity.RelationSex = entity.RelationSex;
                    new_entity.RelationPhone = entity.RelationPhone;
                    new_entity.RelationWorkCompany = entity.RelationWorkCompany;
                    new_entity.RelationAddress = entity.RelationAddress;



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

                    old_entity.RelationType = entity.RelationType;
                    old_entity.RelationName = entity.RelationName;
                    old_entity.RelationSex = entity.RelationSex;
                    old_entity.RelationPhone = entity.RelationPhone;
                    old_entity.RelationWorkCompany = entity.RelationWorkCompany;
                    old_entity.RelationAddress = entity.RelationAddress;

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