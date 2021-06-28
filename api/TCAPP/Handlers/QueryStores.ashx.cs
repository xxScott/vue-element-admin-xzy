using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utilities;

namespace HrApp.Handlers
{
    /// <summary>
    /// QueryStores 的摘要说明
    /// </summary>
    public class QueryStores : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JObject data = new JObject();

            try
            {
                JArray array = new JArray();

                context.Response.ContentType = "text/plain";

                IGroupsService service = ServiceFactory.Factory.GroupsService;
                List<Groups> list = service.Search(null);
                for (int i = 0; i < list.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = list[i].ID;
                    obj["name"] = list[i].GroupName;
                    JArray _array = new JArray();
                    List<Stores> stores = ServiceFactory.Factory.StoresService.Search(new SimpleCondition("GroupID", list[i].ID == 0 ? -1 : list[i].ID));
                    for (int j = 0; j < stores.Count; j++)
                    {
                        JObject _obj = new JObject();
                        _obj["id"] = stores[j].UID;
                        _obj["name"] = stores[j].StoreName;
                        _array.Add(_obj);
                    }
                    obj["children"] = _array;

                    array.Add(obj);
                }
                data["result"] = "true";
                data["code"] = (int)Enum_StatusCode.Success;
                data["desc"] = Util.GetDisplayName(Enum_StatusCode.Success);
                data["data"] = array;
            }
            catch (Exception ex)
            {
                data["result"] = "false";
                data["code"] = (int)Enum_StatusCode.Fail;
                data["desc"] = ex.Message;
                data["data"] = null;
            }

            context.Response.Write(data);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}