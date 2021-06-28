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
    public class ContractController : Controller
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
                        IEmployeeContractService service = ServiceFactory.Factory.EmployeeContractService;
                        ConditionSet condition = new ConditionSet();

                        //start /query /groupid&storeid /20190405
                        GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                        //end

                        condition.Add(new SimpleCondition("EmployeeID", employeeId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                            _condition.Add(new SimpleCondition("ContractNo", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("SignDate", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("ExpiredDate", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<EmployeeContractView> empContract = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                        //数据转换，查询枚举描述
                        DataTable dat = new DataTable();
                        dat.Columns.Add("ID", typeof(Int16));
                        dat.Columns.Add("ContractNo", typeof(string));
                        dat.Columns.Add("ContractType", typeof(string));
                        dat.Columns.Add("SignDate", typeof(DateTime));
                        dat.Columns.Add("ExpiredDate", typeof(DateTime));
                        dat.Columns.Add("UpdateTime", typeof(string));
                        dat.Columns.Add("Description", typeof(string));
                        dat.Columns.Add("TypeName", typeof(string));
                        dat.Columns.Add("EmployeeCode", typeof(string));
                        dat.Columns.Add("EmployeeName", typeof(string));
                        for (int i = 0; i < empContract.Count; i++)
                        {
                            DataRow row = dat.NewRow();
                            row["ID"] = empContract[i].ID;
                            row["ContractNo"] = empContract[i].ContractNo;
                            row["ContractType"] = empContract[i].ContractType;
                            if (empContract[i].ContractType != null)
                            {
                                row["ContractType"] = empContract[i].ContractType;
                                row["TypeName"] = DAL.Util.GetDisplayName((Enum_ContractType)empContract[i].ContractType);
                            }

                            row["SignDate"] = empContract[i].SignDate;
                            row["ExpiredDate"] = empContract[i].ExpiredDate;
                            row["EmployeeCode"] = empContract[i].EmployeeCode;
                            row["EmployeeName"] = empContract[i].EmployeeName;
                            row["UpdateTime"] = empContract[i].UpdateTime;

                            row["Description"] = empContract[i].Description;
                            dat.Rows.Add(row);
                        }
                        json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
                    }
                    if (data_adapter == Enum_Adapter.TreeGrid)
                    {
                        int totalCount = 0;
                        //JArray array = new TreeGridAdapter().GetEmpContract(Convert.ToInt16(groupId), Convert.ToInt16(storeId), keyword, pageNumber, pageSize, out totalCount);
                        //json.Data = JsonUtil.GetSuccessForObject(array, totalCount);
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
                IEmployeeContractService service = ServiceFactory.Factory.EmployeeContractService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                EmployeeContract empContract = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(empContract);
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
                json.Data = ServiceFactory.Factory.EmployeeContractService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Save(EmployeeContract entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IEmployeeContractService service = ServiceFactory.Factory.EmployeeContractService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                EmployeeContract old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    EmployeeContract new_entity = new EmployeeContract();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.EmployeeID = entity.EmployeeID;
                    new_entity.ContractNo = entity.ContractNo;
                    new_entity.ContractType = entity.ContractType;
                    new_entity.SignDate = entity.SignDate;
                    new_entity.ExpiredDate = entity.ExpiredDate;



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

                    old_entity.ContractNo = entity.ContractNo;
                    old_entity.ContractType = entity.ContractType;
                    old_entity.SignDate = entity.SignDate;
                    old_entity.ExpiredDate = entity.ExpiredDate;

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