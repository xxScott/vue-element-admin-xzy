using System;
using System.Collections.Generic;
using System.Data;
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
    public class RightController : Controller
    {
        public JsonResult Query(int roleId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                //List<Modules> modules = ServiceFactory.Factory.ModulesService.Search(null);
                JArray array = new TreeAdapter().GetRight(roleId);
                json.Data = JsonUtil.GetSuccessForObject(array);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(string roleId,List<Models.Data.RightDataView> rights)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                DataTable data = new DataTable();
                data.Columns.Add("RoleID");
                data.Columns.Add("ModuleID");
                data.Columns.Add("HasAdd");
                data.Columns.Add("HasUpdate");
                data.Columns.Add("HasDelete");
                data.Columns.Add("HasSubmit");
                data.Columns.Add("HasQuery");

                for (int i = 0; i < rights.Count; i++)
                {
                    if (rights[i].ModuleID.ToString().IndexOf(TreeAdapter.module_func_char) == -1) continue;

                    string moduleId = rights[i].ModuleID.ToString().Substring(0, rights[i].ModuleID.ToString().IndexOf(TreeAdapter.module_func_char));
                    string moduleFunc = rights[i].ModuleID.ToString().Substring(rights[i].ModuleID.ToString().IndexOf(TreeAdapter.module_func_char) + TreeAdapter.module_func_char.Length, rights[i].ModuleID.ToString().Length - rights[i].ModuleID.ToString().IndexOf(TreeAdapter.module_func_char) - TreeAdapter.module_func_char.Length);

                    DataRow[] drc = data.Select("ModuleID='" + moduleId + "'");
                    if (drc.Length <= 0)
                    {
                        DataRow dr = data.NewRow();
                        dr["RoleID"] = rights[i].RoleID;
                        dr["ModuleID"] = moduleId;
                        dr["HasAdd"] = moduleFunc.ToLower() == TreeAdapter.module_func_add ? 1 : 0;
                        dr["HasUpdate"] = moduleFunc.ToLower() == TreeAdapter.module_func_update ? 1 : 0;
                        dr["HasDelete"] = moduleFunc.ToLower() == TreeAdapter.module_func_delete ? 1 : 0;
                        dr["HasSubmit"] = moduleFunc.ToLower() == TreeAdapter.module_func_submit ? 1 : 0;
                        dr["HasQuery"] = moduleFunc.ToLower() == TreeAdapter.module_func_query ? 1 : 0;
                        data.Rows.Add(dr);
                    }
                    else
                    {
                        for (int n = 0; n < data.DefaultView.Count; n++)
                        {
                            if (data.DefaultView[n]["ModuleID"].ToString().Trim() == moduleId)
                            {
                                if (data.DefaultView[n]["HasQuery"].ToString().Trim() == "0")
                                    data.DefaultView[n]["HasQuery"] = moduleFunc.ToLower() == TreeAdapter.module_func_query ? 1 : 0;
                                if (data.DefaultView[n]["HasAdd"].ToString().Trim() == "0")
                                    data.DefaultView[n]["HasAdd"] = moduleFunc.ToLower() == TreeAdapter.module_func_add ? 1 : 0;
                                if (data.DefaultView[n]["HasUpdate"].ToString().Trim() == "0")
                                    data.DefaultView[n]["HasUpdate"] = moduleFunc.ToLower() == TreeAdapter.module_func_update ? 1 : 0;
                                if (data.DefaultView[n]["HasDelete"].ToString().Trim() == "0")
                                    data.DefaultView[n]["HasDelete"] = moduleFunc.ToLower() == TreeAdapter.module_func_delete ? 1 : 0;
                                if (data.DefaultView[n]["HasSubmit"].ToString().Trim() == "0")
                                    data.DefaultView[n]["HasSubmit"] = moduleFunc.ToLower() == TreeAdapter.module_func_submit ? 1 : 0;
                            }
                        }
                    }
                }

                List<Rights> list = new List<Rights>();
                for (int i = 0; i < data.DefaultView.Count; i++)
                {
                    Rights r = new Rights();
                    r.RoleID = Convert.ToInt16(data.DefaultView[i]["RoleID"]);
                    r.ModuleID = Convert.ToInt16(data.DefaultView[i]["ModuleID"]);
                    r.HasAdd = data.DefaultView[i]["HasAdd"].ToString() == "1" ? true : false;
                    r.HasUpdate = data.DefaultView[i]["HasUpdate"].ToString() == "1" ? true : false;
                    r.HasDelete = data.DefaultView[i]["HasDelete"].ToString() == "1" ? true : false;
                    r.HasSubmit = data.DefaultView[i]["HasSubmit"].ToString() == "1" ? true : false;
                    r.HasQuery = data.DefaultView[i]["HasQuery"].ToString() == "1" ? true : false;
                    r.CreateTime = DateTime.Now;
                    r.Creator = Utilities.Security.CurrentUser.UserName;
                    r.CreateIP = Network.ClientIP;
                    r.UpdateTime = DateTime.Now;
                    r.Updater = Utilities.Security.CurrentUser.UserName;
                    r.UpdateIP = Network.ClientIP;
                    list.Add(r);
                }

                if (ServiceFactory.Factory.RightsService.Update(list, roleId))
                {
                    json.Data = JsonUtil.GetSuccessForString("设置已完成");
                }
                else
                {
                    json.Data = JsonUtil.GetSuccessForString("设置失败");
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