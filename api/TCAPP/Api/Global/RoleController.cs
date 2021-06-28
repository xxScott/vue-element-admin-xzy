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

namespace HrApp.Api
{
    public class RoleController : Controller
    {
        public JsonResult Query(string keyword,string adapter,int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                Enum_Adapter data_adapter = Enum_Adapter.None;
                if (adapter != null)
                    data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);

                if (data_adapter == Enum_Adapter.None)
                {
                    IRolesService service = ServiceFactory.Factory.RolesService;
                    ConditionSet condition = new ConditionSet();
                    if (keyword != null && keyword != string.Empty)
                    {
                        ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                        _condition.Add(new SimpleCondition("RoleCode", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("RoleName", ConditionOperator.Like, "%" + keyword + "%"));
                        _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                        condition.Add(_condition);
                    }
                    //查询记录总数
                    int totalCount = service.Count(condition);
                    //查询分页数据
                    List<Roles> roles = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

                    json.Data=JsonUtil.GetSuccessForObject(roles, totalCount);
                }
                else if (data_adapter == Enum_Adapter.DataList)
                {
                    JArray array = new DataListAdapter().GetRole();
                    json.Data=JsonUtil.GetSuccessForObject(array);
                }
                else
                {
                    json.Data=JsonUtil.GetFailForString("数据加载失败");
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
                IRolesService service = ServiceFactory.Factory.RolesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                //查询数据
                Roles role = service.SearchOne(condition);
                json.Data=JsonUtil.GetSuccessForObject(role);
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
                json.Data = ServiceFactory.Factory.RolesService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Roles r)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IRolesService service = ServiceFactory.Factory.RolesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", r.ID));
                //查询数据
                Roles role = service.SearchOne(condition);
                if (role == null)
                {
                    Roles _r = new Roles();
                    _r.RoleCode = r.RoleCode;
                    _r.RoleName = r.RoleName;
                    _r.Description = r.Description;
                    _r.CreateTime = DateTime.Now;
                    _r.Creator = Utilities.Security.CurrentUser.UserName;
                    _r.CreateIP = Network.ClientIP;
                    _r.UpdateTime = DateTime.Now;
                    _r.Updater = Utilities.Security.CurrentUser.UserName;
                    _r.UpdateIP = Network.ClientIP;
                    service.Insert(_r);
                    json.Data=JsonUtil.GetSuccessForString("新增已完成");
                }
                else
                {
                    role.RoleCode = r.RoleCode;
                    role.RoleName = r.RoleName;
                    role.Description = r.Description;
                    role.UpdateTime = DateTime.Now;
                    role.Updater = Utilities.Security.CurrentUser.UserName;
                    role.UpdateIP = Network.ClientIP;
                    service.Update(role);
                    json.Data=JsonUtil.GetSuccessForString("修改已完成");
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