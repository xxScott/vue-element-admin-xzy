using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using DAL;
using DAL.Business;
using MyOrm.Common;
using System.Data;
using Utilities;
namespace Adapters
{
    public class ComboboxAdapter
    {
        public JArray GetEnum(Type enumType)
        {
            DataView dv = DAL.Util.GetAll(enumType).DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = dv[i]["Value"].ToString().Trim();
                obj["text"] = dv[i]["String"].ToString().Trim();
                array.Add(obj);
            }
            return array;
        }


        public JArray GetWorkStation(string groupId, string storeId)
        {
            if (groupId != "-1")
            {
                storeId = "-1";
            }
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            List<WorkStations> list = ServiceFactory.Factory.WorkStationsService.Search(condition);

            JArray array = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i].ID.ToString().Trim();
                obj["text"] = list[i].StationName.ToString().Trim();
                array.Add(obj);
            }
            return array;
        }

        public JArray GetPeriod(string groupId, string storeId)
        {
            if (groupId != "-1")
            {
                storeId = "-1";
            }
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            List<Periods> list = ServiceFactory.Factory.PeriodService.Search(condition);

            JArray array = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i].ID.ToString().Trim();
                obj["text"] = list[i].PeriodName.ToString().Trim();
                array.Add(obj);
            }
            return array;
        }
        public JArray GetSalaryProject(string groupId, string storeId)
        {
            if (groupId != "-1")
            {
                storeId = "-1";
            }

            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            List<SalaryProjects> list = ServiceFactory.Factory.SalaryProjectsService.Search(condition);

            JArray array = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i].ID.ToString().Trim();
                obj["text"] = list[i].ProjectName.ToString().Trim();
                array.Add(obj);
            }
            return array;
        }

        public JArray GetUsers(string groupId, string storeId)
        {

            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            List<Users> list = ServiceFactory.Factory.UsersService.Search(condition);

            JArray array = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i].ID.ToString().Trim();
                obj["text"] = list[i].UserName.ToString().Trim();
                array.Add(obj);
            }
            return array;
        }
        public JArray GetClass(string groupId, string storeId)
        {
            if (groupId != "-1")
            {
                storeId = "-1";
            }
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            List<ClassesView> list = ServiceFactory.Factory.ClassesService.Search(condition);

            JArray array = new JArray();
            for (int i = 0; i < list.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = list[i].ID.ToString().Trim();
                obj["text"] = list[i].ClassName.ToString().Trim();
                array.Add(obj);
            }
            return array;
        }
    }
}
