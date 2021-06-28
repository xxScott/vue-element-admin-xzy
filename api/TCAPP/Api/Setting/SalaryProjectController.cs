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
using System;
using System.Collections.Generic;
using System.Data;

namespace HrApp.Api.Setting
{
    public class SalaryProjectController : Controller
    {
        public JsonResult Query(string groupId,string storeId,string keyword, string adapter, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                if (adapter != null)
                {
                    Enum_Adapter data_adapter = Enum_Adapter.None;
                    if (adapter != null)
                        data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);

                    if (data_adapter == Enum_Adapter.None)
                    {
                        ISalaryProjectsService service = ServiceFactory.Factory.SalaryProjectsService;
                        ConditionSet condition = new ConditionSet();

                        //start /query /groupid&storeid /20190405
                        GroupIdAndStoreIdAdd.AddGroupIdAndStoreId(condition);
                        //end

                        //condition.Add(new SimpleCondition("GroupID", groupId));
                        //condition.Add(new SimpleCondition("StoreID", storeId));
                        if (keyword != null && keyword != string.Empty)
                        {
                            ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                            _condition.Add(new SimpleCondition("ProjectCode", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("ProjectName", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("ProjectProperty", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("ProjectSource", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("DesignFormulas", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("ProjectOrder", ConditionOperator.Like, "%" + keyword + "%"));
                            _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                            condition.Add(_condition);
                        }
                        //查询记录总数
                        int totalCount = service.Count(condition);
                        //查询分页数据
                        List<SalaryProjects> projects = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                        //数据转换，查询枚举描述
                        DataTable dat = new DataTable();
                        dat.Columns.Add("ID", typeof(Int16));
                        dat.Columns.Add("ProjectCode", typeof(string));
                        dat.Columns.Add("ProjectName", typeof(string));
                        dat.Columns.Add("ProjectProperty", typeof(Int16));
                        dat.Columns.Add("ProjectPropertyName", typeof(string));
                        dat.Columns.Add("ProjectSource", typeof(Int16));
                        dat.Columns.Add("ProjectSourceName", typeof(string));
                        dat.Columns.Add("Description", typeof(string));
                        for (int i = 0; i < projects.Count; i++)
                        {
                            DataRow row = dat.NewRow();
                            row["ID"] = projects[i].ID;
                            row["ProjectCode"] = projects[i].ProjectCode;
                            row["ProjectName"] = projects[i].ProjectName;
                            if (projects[i].ProjectProperty != null)
                            {
                                row["ProjectProperty"] = projects[i].ProjectProperty;
                                row["ProjectPropertyName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectProperty)projects[i].ProjectProperty);
                            }
                            if (projects[i].ProjectSource != null)
                            {
                                row["ProjectSource"] = projects[i].ProjectSource;
                                row["ProjectSourceName"] = DAL.Util.GetDisplayName((Enum_SalaryProjectSource)projects[i].ProjectSource);
                            }
                            row["Description"] = projects[i].Description;
                            dat.Rows.Add(row);
                        }
                        json.Data = JsonUtil.GetSuccessForObject(dat, totalCount);
                    }
                    else if (data_adapter == Enum_Adapter.Combobox)
                    {
                        JArray array = new ComboboxAdapter().GetSalaryProject(groupId,storeId);
                        json.Data = JsonUtil.GetSuccessForObject(array);
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
                ISalaryProjectsService service = ServiceFactory.Factory.SalaryProjectsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                SalaryProjects stations = service.SearchOne(condition);
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
                json.Data = ServiceFactory.Factory.SalaryProjectsService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(SalaryProjects entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalaryProjectsService service = ServiceFactory.Factory.SalaryProjectsService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", entity.ID));
                //查询数据
                SalaryProjects old_entity = service.SearchOne(condition);
                if (old_entity == null)
                {
                    SalaryProjects new_entity = new SalaryProjects();

                    //start /insert /groupid&storeid /20190405
                    Users currentUser = Utilities.Security.CurrentUser;
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;
                    new_entity.GroupID = groudid.HasValue? groudid.Value : -1;
                    new_entity.StoreID = storeid.HasValue ? storeid.Value : -1;
                    //end

                    new_entity.GroupID = entity.GroupID;
                    new_entity.StoreID = entity.StoreID;
                    new_entity.ProjectCode = entity.ProjectCode;
                    new_entity.ProjectName = entity.ProjectName;
                    new_entity.ProjectProperty = entity.ProjectProperty;
                    new_entity.ProjectSource = entity.ProjectSource;
                    new_entity.DesignFormulas = entity.DesignFormulas;
                    new_entity.ProjectOrder = entity.ProjectOrder;


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

                    old_entity.ProjectCode = entity.ProjectCode;
                    old_entity.ProjectName = entity.ProjectName;
                    old_entity.ProjectProperty = entity.ProjectProperty;
                    old_entity.ProjectSource = entity.ProjectSource;
                    old_entity.DesignFormulas = entity.DesignFormulas;
                    old_entity.ProjectOrder = entity.ProjectOrder;
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