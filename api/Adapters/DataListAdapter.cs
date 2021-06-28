using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Business;
using DAL;
using Newtonsoft.Json.Linq;
using MyOrm.Common;
using Utilities;
namespace Adapters
{
    public class DataListAdapter
    {
        /// <summary>
        /// 根据“角色”数据生成DataList格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetRole()
        {
            List<Roles> list = ServiceFactory.Factory.RolesService.Search(null);

            JArray array = new JArray();

            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i]["ID"].ToString().Trim();
                obj["text"] = list[i]["RoleName"].ToString().Trim();
                array.Add(obj);
            }
            return array;
        }
        public JArray GetGroupAdmin()
        {
            List<Groups> list = new List<Groups>();
            JArray array = new JArray();
            //系统管理员
            if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_Admin)
            {
                list = ServiceFactory.Factory.GroupsService.Search(null);
                for (int i = 0; i < list.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = list[i]["ID"].ToString().Trim();
                    obj["text"] = list[i]["GroupName"].ToString().Trim();
                    array.Add(obj);

                }

            }
            //集团管理员
            else if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_Group)
            {
                list = ServiceFactory.Factory.GroupsService.Search(new SimpleCondition("ID", Security.CurrentUser.GroupID));
                for (int i = 0; i < list.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = list[i]["ID"].ToString().Trim();
                    obj["text"] = list[i]["GroupName"].ToString().Trim();
                    array.Add(obj);
                }
            }//单门店管理员
            else if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_OnlyStore)
            {
                Stores store = ServiceFactory.Factory.StoresService.SearchOne(new SimpleCondition("ID", Security.CurrentUser.StoreID));
                JObject obj = new JObject();
                obj["id"] = store.ID.ToString().Trim();
                obj["text"] = store.StoreName.ToString().Trim() + "-(单独门店)";
                array.Add(obj);
            }

            return array;

        }
        /// <summary>
        /// 根据“公司”数据生成DataList格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetGroup()
        {
            List<Groups> list = new List<Groups>();
            JArray array = new JArray();
            //系统管理员
            if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_Admin)
            {
                list = ServiceFactory.Factory.GroupsService.Search(null);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ID == 0)
                    {
                        List<DAL.Stores> stores = ServiceFactory.Factory.StoresService.Search(new SimpleCondition("GroupID", -1));
                        for (int j = 0; j < stores.Count; j++)
                        {
                            JObject obj = new JObject();
                            obj["id"] = stores[j]["ID"].ToString().Trim();
                            obj["text"] = stores[j]["StoreName"].ToString().Trim() + "-(单独门店)";
                            array.Add(obj);
                        }
                    }
                    else
                    {
                        JObject obj = new JObject();
                        obj["id"] = list[i]["ID"].ToString().Trim();
                        obj["text"] = list[i]["GroupName"].ToString().Trim();
                        array.Add(obj);
                    }
                }

            }
            //集团管理员
            else if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_Group)
            {
                list = ServiceFactory.Factory.GroupsService.Search(new SimpleCondition("ID", Security.CurrentUser.GroupID));
                for (int i = 0; i < list.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = list[i]["ID"].ToString().Trim();
                    obj["text"] = list[i]["GroupName"].ToString().Trim();
                    array.Add(obj);
                }
            }//单门店管理员
            else if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_OnlyStore)
            {
                Stores store = ServiceFactory.Factory.StoresService.SearchOne(new SimpleCondition("ID", Security.CurrentUser.StoreID));
                JObject obj = new JObject();
                obj["id"] = store.ID.ToString().Trim();
                obj["text"] = store.StoreName.ToString().Trim() + "-(单独门店)";
                array.Add(obj);
            }
            else if (Security.CurrentUserRole.RoleCode == Utilities.Strings.RoleCode_Store)
            {
                list = ServiceFactory.Factory.GroupsService.Search(new SimpleCondition("ID", Security.CurrentUser.GroupID));
                for (int i = 0; i < list.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = list[i]["ID"].ToString().Trim();
                    obj["text"] = list[i]["GroupName"].ToString().Trim();
                    array.Add(obj);
                }
            }

            return array;

        }

    }
}
