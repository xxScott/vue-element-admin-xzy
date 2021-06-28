using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Business;
using DAL;
using Newtonsoft.Json.Linq;
using MyOrm.Common;

namespace Utilities.DataAdapter
{
    public class DataListAdapter
    {
        /// <summary>
        /// 根据“角色”数据生成DataList格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetRole()
        {
            List<Roles> list= ServiceFactory.Factory.RolesService.Search(null);

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

        /// <summary>
        /// 根据“应用”数据生成DataList格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetApp()
        {
            List<Apps> list = ServiceFactory.Factory.AppsService.Search(null);
            JArray array = new JArray();

            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i]["ID"].ToString().Trim();
                obj["text"] = list[i]["AppName"].ToString().Trim();
                array.Add(obj);
            }
            return array;
        }
    }
}
