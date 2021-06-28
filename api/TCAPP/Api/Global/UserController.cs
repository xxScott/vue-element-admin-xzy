using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adapters;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Newtonsoft.Json.Linq;
using Utilities;

namespace HrApp.Api
{
    public class UserController : Controller
    {
        public JsonResult Query(string groupId, string storeId, string keyword, int? pageNumber, int? pageSize, string adapter)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IUsersService service = ServiceFactory.Factory.UsersService;
                ConditionSet condition = new ConditionSet();
                if (keyword != null && keyword != string.Empty)
                {
                    ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                    _condition.Add(new SimpleCondition("UserID", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("UserName", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Telephone", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Email", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                    condition.Add(_condition);
                }
                int totalCount = service.Count(condition);
                List<Users> users = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                json.Data = JsonUtil.GetSuccessForObject(users, totalCount);
                if (adapter != null && (Enum_Adapter)Convert.ToInt16(adapter) == Enum_Adapter.Combobox)
                {
                    JArray array = new ComboboxAdapter().GetUsers(groupId,storeId);
                    json.Data = JsonUtil.GetSuccessForObject(array);
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult QueryByGroup(string groupId, string storeId, string keyword, int? pageNumber, int? pageSize, string adapter)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IUsersService service = ServiceFactory.Factory.UsersService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("GroupID", groupId));
                condition.Add(new SimpleCondition("StoreID", -1));
                if (keyword != null && keyword != string.Empty)
                {
                    ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                    _condition.Add(new SimpleCondition("UserID", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("UserName", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Telephone", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Email", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                    condition.Add(_condition);
                }

                int totalCount = service.Count(condition);
                List<Users> users = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                json.Data = JsonUtil.GetSuccessForObject(users, totalCount);
                if (adapter != null && (Enum_Adapter)Convert.ToInt16(adapter) == Enum_Adapter.Combobox)
                {
                    JArray array = new ComboboxAdapter().GetUsers(groupId,storeId);
                    json.Data = JsonUtil.GetSuccessForObject(array);
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult QueryByStore(string groupId, string storeId, string keyword, int? pageNumber, int? pageSize, string adapter)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IUsersService service = ServiceFactory.Factory.UsersService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("StoreID", storeId));
                if (keyword != null && keyword != string.Empty)
                {
                    ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);
                    _condition.Add(new SimpleCondition("UserID", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("UserName", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Telephone", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Email", ConditionOperator.Like, "%" + keyword + "%"));
                    _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                    condition.Add(_condition);
                }

                int totalCount = service.Count(condition);
                List<Users> users = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
                json.Data = JsonUtil.GetSuccessForObject(users, totalCount);
                if (adapter != null && (Enum_Adapter)Convert.ToInt16(adapter) == Enum_Adapter.Combobox)
                {
                    JArray array = new ComboboxAdapter().GetUsers(groupId,storeId);
                    json.Data = JsonUtil.GetSuccessForObject(array);
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
                IUsersService service = ServiceFactory.Factory.UsersService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                Users user = service.SearchOne(condition);
                if (user.Password != null)
                    user.Password = Encrypt.Instance.DecryptString(user.Password);

                List<UserInRole> roles = ServiceFactory.Factory.UserInRoleService.Search(new SimpleCondition("UserID", id));

                var data = new { User = user, Roles = roles };

                json.Data = JsonUtil.GetSuccessForObject(data);
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
                json.Data = ServiceFactory.Factory.UsersService.Delete(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Users u, string roleCode)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IUsersService service = ServiceFactory.Factory.UsersService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", u.ID));
                //查询数据
                Users user = service.SearchOne(condition);
                if (user == null)
                {
                    Users _u = new Users();
                    _u.UserID = u.UserID;
                    _u.UserName = u.UserName;
                    _u.Password = Encrypt.Instance.EncryptString(u.Password);
                    _u.Telephone = u.Telephone;

                    _u.Email = u.Email;
                    _u.KeyNum = u.KeyNum;
                    _u.IsChangePwd = u.IsChangePwd;
                    _u.IsLocked = u.IsLocked;
                    _u.GroupID = u.GroupID;
                    _u.StoreID = u.StoreID;
                    _u.CreateTime = DateTime.Now;
                    _u.Creator = Utilities.Security.CurrentUser.UserName;
                    _u.CreateIP = Network.ClientIP;
                    _u.UpdateTime = DateTime.Now;
                    _u.Updater = Utilities.Security.CurrentUser.UserName;
                    _u.UpdateIP = Network.ClientIP;


                    service.Insert(_u, roleCode);
                    json.Data = JsonUtil.GetSuccessForString("新增已完成");
                }
                else
                {
                    user.UserID = u.UserID;
                    user.UserName = u.UserName;
                    user.Password = Encrypt.Instance.EncryptString(u.Password);
                    user.Telephone = u.Telephone;
                    user.Email = u.Email;
                    user.KeyNum = u.KeyNum;
                    user.IsChangePwd = u.IsChangePwd;
                    user.IsLocked = u.IsLocked;
                    user.UpdateTime = DateTime.Now;
                    user.Updater = Utilities.Security.CurrentUser.UserName;
                    user.UpdateIP = Network.ClientIP;
                    service.Update(user, roleCode);
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