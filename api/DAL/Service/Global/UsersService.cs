
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DAL.Business;
using MyOrm.Common;
using Newtonsoft.Json;
using log4net;

namespace DAL
{
    public interface IUsersService : IEntityService<Users>, IEntityViewService<Users>, IEntityService, IEntityViewService
    {
        int Login(string userId, string userPwd);

        int Login(string uId, string userId, string userPwd);


        [Transaction]
        bool Insert(Users user, string roleCode);

        [Transaction]
        bool Delete(int userId);

        [Transaction]
        bool Update(Users user, string roleCode);

        [Transaction]
        bool InsertCaiMoMo(Groups group, List<Stores> stores, Users user, string roleCode);
    }

    public class UsersService : ServiceBase<Users, Users>, IUsersService
    {
        public readonly static ILog logger = LogManager.GetLogger("ServiceInterceptor");

        public int Login(string uId, string userId, string userPwd)
        {
            logger.Info("Begin To Login,User Id:" + userId + ",User Pwd:" + userPwd);
            try
            {
                string message = string.Empty;
                ConditionSet condition = new ConditionSet();

                condition.Add(new SimpleCondition("UserID", ConditionOperator.Equals, userId));
                Groups group = ServiceFactory.Factory.GroupsService.SearchOne(new SimpleCondition("UID", uId));
                if (group != null)
                {
                    condition.Add(new SimpleCondition("GroupID", ConditionOperator.Equals, group.ID));
                }
                else
                {
                    Stores store = ServiceFactory.Factory.StoresService.SearchOne(new SimpleCondition("UID", uId));
                    if (store != null)
                    {
                        condition.Add(new SimpleCondition("StoreID", ConditionOperator.Equals, store.UID));
                    }
                    else
                    {
                        condition.Add(new SimpleCondition("StoreID", -1));
                        //condition.Add(new SimpleCondition("GroupID", -1));
                    }
                }

                List<Users> list = this.Search(condition);
                if (list.Count <= 0)
                {
                    message = "用户名\\密码错误";
                    logger.Error("User Id Error,User Id:" + userId + ",User Pwd:" + userPwd);
                    throw new CustomException(message);
                }
                Users u = list[0];
                if (u.Password != userPwd)
                {
                    message = "用户名\\密码错误";
                    logger.Error("User Pwd Error,User Id:" + userId + ",User Pwd:" + userPwd);
                    throw new CustomException(message);
                }
                logger.Info("Login Successfully,User Id:" + userId + ",User Pwd:" + userPwd);

                u.LastLogin = DateTime.Now;
                this.Update(u);
                return u.ID;
            }
            catch (Exception ex)
            {
                logger.Error("Login Exception,User Id:" + userId + ",User Pwd:" + userPwd, ex);
                throw ex;
            }
        }


        public int Login(string userId, string userPwd)
        {
            logger.Info("Begin To Login,User Id:" + userId + ",User Pwd:" + userPwd);
            try
            {
                string message = string.Empty;
                ConditionSet condition = new ConditionSet();

                condition.Add(new SimpleCondition("UserID", ConditionOperator.Equals, userId));

                condition.Add(new SimpleCondition("StoreID", -1));
                condition.Add(new SimpleCondition("GroupID", -1));
                List<Users> list = this.Search(condition);
                if (list.Count <= 0)
                {
                    message = "用户名\\密码错误";
                    logger.Error("User Id Error,User Id:" + userId + ",User Pwd:" + userPwd);
                    throw new CustomException(message);
                }
                Users u = list[0];
                if (u.Password != userPwd)
                {
                    message = "用户名\\密码错误";
                    logger.Error("User Pwd Error,User Id:" + userId + ",User Pwd:" + userPwd);
                    throw new CustomException(message);
                }
                logger.Info("Login Successfully,User Id:" + userId + ",User Pwd:" + userPwd);

                u.LastLogin = DateTime.Now;
                this.Update(u);
                return u.ID;
            }
            catch (Exception ex)
            {
                logger.Error("Login Exception,User Id:" + userId + ",User Pwd:" + userPwd, ex);
                throw ex;
            }
        }

        public bool Insert(Users user, string roleCode)
        {
            logger.Info("Begin To Insert Data,User:" + JsonConvert.SerializeObject(user));
            try
            {
                Factory.UsersService.Insert(user);

                if (roleCode != "")
                {
                    Roles role = ServiceFactory.Factory.RolesService.SearchOne(new SimpleCondition("RoleCode", roleCode));
                    if (role != null)
                    {
                        UserInRole roles = new UserInRole();
                        roles.RoleID = role.ID;
                        roles.UserID = user.ID;
                        Factory.UserInRoleService.Insert(roles);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Insert User Failed", ex);
                throw ex;
            }
        }

        public bool Delete(int userId)
        {
            logger.Info("Begin To Delete User,User Id:" + userId);
            try
            {
                Factory.UsersService.DeleteID(userId);
                List<UserInRole> roles = Factory.UserInRoleService.Search(new SimpleCondition("UserID", userId));
                Factory.UserInRoleService.BatchDelete(roles);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Delete User Failed", ex);
                throw ex;
            }
        }

        public bool Update(Users user, string roleCode)
        {
            try
            {
                logger.Info("Begin To Insert Data,User:" + JsonConvert.SerializeObject(user));

                Factory.UsersService.Update(user);
                List<UserInRole> old_roles = Factory.UserInRoleService.Search(new SimpleCondition("UserID", user.ID));
                Factory.UserInRoleService.BatchDelete(old_roles);

                if (roleCode != "")
                {
                    Roles role = ServiceFactory.Factory.RolesService.SearchOne(new SimpleCondition("RoleCode", roleCode));
                    if (role != null)
                    {
                        UserInRole roles = new UserInRole();
                        roles.RoleID = role.ID;
                        roles.UserID = user.ID;
                        Factory.UserInRoleService.Insert(roles);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Update User Failed", ex);
                throw ex;
            }
        }



        public bool InsertCaiMoMo(Groups group, List<Stores> stores, Users user, string roleCode)
        {
            logger.Info("Begin To Insert Data,User:" + JsonConvert.SerializeObject(user));
            try
            {

                //更新集团
                int groupId = -1, storeId = -1;
                //if (user != null && user.StoreID.HasValue && !user.StoreID.Value.ToString().EndsWith("00"))
                if (user != null)
                {
                    if (user.GroupID.HasValue)
                    {
                        groupId = user.GroupID.Value;
                    }
                    if (user.StoreID.HasValue && !user.StoreID.Value.ToString().EndsWith("00"))
                    {
                        storeId = user.StoreID.Value;
                    }
                }
                if (group != null)
                {
                    Groups old_group = Factory.GroupsService.SearchOne(new SimpleCondition("UID", group.UID));
                    if (old_group == null)
                    {
                        Factory.GroupsService.Insert(group);
                        groupId = group.ID;
                    }
                    else
                    {
                        old_group.UID = group.UID;
                        old_group.GroupName = group.GroupName;
                        old_group.Province = group.Province;
                        old_group.City = group.City;
                        old_group.Address = group.Address;
                        old_group.ContactName = group.ContactName;
                        old_group.Telephone = group.Telephone;
                        Factory.GroupsService.Update(old_group);
                        //groupId = old_group.ID;
                    }

                    user.GroupID = groupId;
                }

                //更新门店 //少B_Store表 hjt 2019-02-23 add 先注释
                for (int i = 0; i < stores.Count; i++)
                {
                    stores[i].GroupID = groupId;
                    Stores old_store = Factory.StoresService.SearchOne(new SimpleCondition("UID", stores[i].UID));
                    if (old_store == null)
                    {
                        Factory.StoresService.Insert(stores[i]);
                        if (user.StoreID == stores[i].UID)
                            storeId = stores[i].ID;
                    }
                    else
                    {

                        old_store.UID = stores[i].UID;
                        old_store.GroupID = groupId;
                        old_store.StoreName = stores[i].StoreName;
                        old_store.Province = stores[i].Province;
                        old_store.City = stores[i].City;
                        old_store.Address = stores[i].Address;
                        old_store.ContactName = stores[i].ContactName;
                        Factory.StoresService.Update(old_store);
                        //if (user.StoreID == stores[i].UID)
                        //    storeId = old_store.ID;
                    }
                }


                //更新用户
                Users old_user = Factory.UsersService.SearchOne(new SimpleCondition("UID", user.UID));
                if (old_user == null)
                {
                    user.StoreID = storeId;
                    Factory.UsersService.Insert(user, roleCode);
                }
                else
                {

                    old_user.UID = old_user.UID;
                    old_user.UserName = old_user.UserName;
                    old_user.UserID = old_user.UserID;
                    old_user.Password = old_user.Password;
                    old_user.QQ = old_user.QQ;
                    old_user.WXID = old_user.WXID;
                    old_user.ShouQuanCode = old_user.ShouQuanCode;
                    old_user.StoreID = storeId;
                    old_user.GroupID = groupId;

                    Factory.UsersService.Update(old_user, roleCode);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Insert User Failed", ex);
                throw ex;
            }
        }
    }
}