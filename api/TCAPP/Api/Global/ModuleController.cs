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
using Adapters;

namespace HrApp.Api
{
    public class ModuleController : Controller
    {
        /// <summary>
        /// 门店巡检UI升级，统一改为左侧菜单,获取所有菜单明细
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryALLMenu()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                JArray array = new MenuAdapter().GetAllModule(Utilities.Security.CurrentUser.ID);

                //如果有集团账户，并且登陆用户不是集团账户，则过滤部分功能
                if (Utilities.Security.HaveGroupAccount && !Utilities.Security.IsGroupAccount)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        JArray childArray = array[i]["menus"] as JArray;
                        if (childArray != null)
                        {
                            for (int j = 0; j < childArray.Count; j++)
                            {
                               JArray lst = childArray[0]["menus"] as JArray;
                                for (int x = 0; x < lst.Count; x++)
                                {
                                    if (lst[x]["url"].ToString().ToLower().Contains("CompanyForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("StoreForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("DeptForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("WorkStationForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("FormationForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("SalaryProjectForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("ClassForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("PeriodForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("ParamForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("StorageForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("DepartmentManageForm.html".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("FlowForm.html".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("UserForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("RightForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("RightForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("ParamForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                    else if (lst[x]["url"].ToString().ToLower().Contains("ModuleForm.aspx".ToLower()))
                                    {
                                        lst.RemoveAt(x);
                                        x--;
                                    }
                                }
                               
                                //childArray[0] = lst;
                            }
                        }
                        if (array[i]["menuname"].ToString() == "系统管理")
                        {
                            array.RemoveAt(i);
                            i--;
                        }
                        else if (array[i]["menuname"].ToString() == "基础设置")
                        {
                            array.RemoveAt(i);
                            i--;
                        }
                    }
                }

                Users currentUser = Utilities.Security.CurrentUser;
                int? storeid = currentUser.StoreID;
                int? groudid = currentUser.GroupID;
                
                json.Data = JsonUtil.GetSuccessForObject(array,
                    groudid.HasValue ? groudid.Value.ToString() : string.Empty,
                    storeid.HasValue ? storeid.Value.ToString() : string.Empty);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }

           
            return json;
        }

        public JsonResult QueryParentMenu()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                JArray array = new MenuAdapter().GetParentModule(Utilities.Security.CurrentUser.ID);
                //JArray array = new Utilities.DataAdapter.MenuAdapter().GetModule();
                json.Data = JsonUtil.GetSuccessForObject(array);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult QueryChildMenu(int moduleId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                JArray array = new MenuAdapter().GetChildModule(moduleId, Utilities.Security.CurrentUser.ID);
                //JArray array = new Utilities.DataAdapter.MenuAdapter().GetModule();
                //如果有集团账户，并且登陆用户不是集团账户，则过滤部分功能
                if (Utilities.Security.HaveGroupAccount && !Utilities.Security.IsGroupAccount)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        if (array[i]["menuname"].ToString() == "系统管理")
                        {
                            array.RemoveAt(i);
                            i--;
                        }
                    }
                }
                json.Data = JsonUtil.GetSuccessForObject(array);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult QueryMenu()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                JArray array = new MenuAdapter().GetModule(Utilities.Security.CurrentUser.ID);
                //JArray array = new Utilities.DataAdapter.MenuAdapter().GetModule();
                json.Data = JsonUtil.GetSuccessForObject(array);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Query(string adapter, int? pageNumber, int? pageSize)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                if (adapter.ToString().IndexOf("|") == -1)
                {
                    Enum_Adapter data_adapter = Enum_Adapter.None;
                    if (adapter != null)
                        data_adapter = (Enum_Adapter)Convert.ToInt16(adapter);
                    List<Modules> modules = ServiceFactory.Factory.ModulesService.Search(null);

                    if (data_adapter == Enum_Adapter.None)
                    {
                        json.Data = JsonUtil.GetSuccessForObject(modules);
                    }
                    else if (data_adapter == Enum_Adapter.TreeGrid)
                    {
                        JArray array = new TreeGridAdapter().GetModule();
                        json.Data = JsonUtil.GetSuccessForObject(array, array.Count);
                    }
                    else if (data_adapter == Enum_Adapter.ComboTree)
                    {
                        JArray array = new ComboTreeAdapter().GetModule();
                        json.Data = JsonUtil.GetSuccessForObject(array);
                    }
                    else if (data_adapter == Enum_Adapter.Tree)
                    {
                        JArray array = new TreeAdapter().GetModule();
                        json.Data = JsonUtil.GetSuccessForObject(array);
                    }
                    else
                    {
                        json.Data = JsonUtil.GetFailForString("数据加载失败");
                    }
                }
                else
                {
                    JObject data = new JObject();
                    string[] req_data_array = adapter.ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < req_data_array.Length; i++)
                    {
                        Enum_Adapter data_adapter = Enum_Adapter.None;
                        if (adapter != null)
                            data_adapter = (Enum_Adapter)Convert.ToInt16(req_data_array[i]);
                        List<Modules> modules = ServiceFactory.Factory.ModulesService.Search(null);

                        if (data_adapter == Enum_Adapter.None)
                        {
                            data["ModuleData" + (int)data_adapter] = JArray.Parse(JsonConvert.SerializeObject(modules));
                        }
                        else if (data_adapter == Enum_Adapter.TreeGrid)
                        {
                            JArray array = new TreeGridAdapter().GetModule();
                            data["ModuleData" + (int)data_adapter] = array;
                        }
                        else if (data_adapter == Enum_Adapter.ComboTree)
                        {
                            JArray array = new ComboTreeAdapter().GetModule();
                            data["ModuleData" + (int)data_adapter] = array;
                        }
                        else if (data_adapter == Enum_Adapter.Tree)
                        {
                            JArray array = new TreeAdapter().GetModule();
                            data["ModuleData" + (int)data_adapter] = array;
                        }
                    }
                    json.Data = JsonUtil.GetSuccessForObject(data);
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
                IModulesService service = ServiceFactory.Factory.ModulesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", id));
                //查询数据
                Modules module = service.SearchOne(condition);
                json.Data = JsonUtil.GetSuccessForObject(module);
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
                json.Data = ServiceFactory.Factory.ModulesService.DeleteID(id) ? JsonUtil.GetSuccessForString("删除已成功") : JsonUtil.GetFailForString("删除失败");
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Save(Modules m)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IModulesService service = ServiceFactory.Factory.ModulesService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", m.ID));
                //查询数据
                Modules module = service.SearchOne(condition);
                if (module == null)
                {
                    Modules _m = new Modules();
                    _m.DisplayName = m.DisplayName;
                    _m.UpperModuleID = m.UpperModuleID;
                    _m.Src = m.Src;
                    _m.ImageUrl = m.ImageUrl;
                    _m.Sequence = m.Sequence;
                    _m.Description = m.Description;
                    _m.Notes = m.Notes;
                    if (m.Src != null && m.Src != string.Empty)
                    {
                        _m.Hide = !m.Hide;
                        _m.RefreshFlag = m.RefreshFlag;
                        _m.AddFlag = m.AddFlag;
                        _m.UpdateFlag = m.UpdateFlag;
                        _m.DeleteFlag = m.DeleteFlag;
                        _m.SubmitFlag = m.SubmitFlag;
                        _m.QueryFlag = m.QueryFlag;
                        _m.KeywordFlag = m.KeywordFlag;
                        if ((bool)_m.RefreshFlag)
                            _m.RefreshFunc = m.RefreshFunc;
                        if ((bool)_m.AddFlag)
                            _m.AddFunc = m.AddFunc;
                        if ((bool)_m.UpdateFlag)
                            _m.UpdateFunc = m.UpdateFunc;
                        if ((bool)_m.DeleteFlag)
                            _m.DeleteFunc = m.DeleteFunc;
                        if ((bool)_m.SubmitFlag)
                            _m.SubmitFunc = m.SubmitFunc;
                        if ((bool)_m.QueryFlag)
                            _m.QueryFunc = m.QueryFunc;
                        if ((bool)_m.RefreshFlag)
                            _m.RefreshIcon = m.RefreshIcon;
                        if ((bool)_m.AddFlag)
                            _m.AddIcon = m.AddIcon;
                        if ((bool)_m.UpdateFlag)
                            _m.UpdateIcon = m.UpdateIcon;
                        if ((bool)_m.DeleteFlag)
                            _m.DeleteIcon = m.DeleteIcon;
                        if ((bool)_m.SubmitFlag)
                            _m.SubmitIcon = m.SubmitIcon;
                        if ((bool)_m.QueryFlag)
                            _m.QueryIcon = m.QueryIcon;
                        if ((bool)_m.RefreshFlag)
                            _m.RefreshCaption = m.RefreshCaption;
                        if ((bool)_m.AddFlag)
                            _m.AddCaption = m.AddCaption;
                        if ((bool)_m.UpdateFlag)
                            _m.UpdateCaption = m.UpdateCaption;
                        if ((bool)_m.DeleteFlag)
                            _m.DeleteCaption = m.DeleteCaption;
                        if ((bool)_m.SubmitFlag)
                            _m.SubmitCaption = m.SubmitCaption;
                        if ((bool)_m.QueryFlag)
                            _m.QueryCaption = m.QueryCaption;
                    }
                    else
                    {
                        _m.Hide = false;
                        _m.RefreshFlag = false;
                        _m.AddFlag = false;
                        _m.UpdateFlag = false;
                        _m.DeleteFlag = false;
                        _m.SubmitFlag = false;
                        _m.QueryFlag = false;
                        _m.KeywordFlag = false;
                        _m.RefreshFunc = null;
                        _m.AddFunc = null;
                        _m.UpdateFunc = null;
                        _m.DeleteFunc = null;
                        _m.SubmitFunc = null;
                        _m.QueryFunc = null;
                        _m.RefreshIcon = null;
                        _m.AddIcon = null;
                        _m.UpdateIcon = null;
                        _m.DeleteIcon = null;
                        _m.SubmitIcon = null;
                        _m.QueryIcon = null;
                        _m.RefreshCaption = null;
                        _m.AddCaption = null;
                        _m.UpdateCaption = null;
                        _m.DeleteCaption = null;
                        _m.SubmitCaption = null;
                        _m.QueryCaption = null;
                    }
                    _m.CreateTime = DateTime.Now;
                    _m.Creator = Utilities.Security.CurrentUser.UserName;
                    _m.CreateIP = Network.ClientIP;
                    _m.UpdateTime = DateTime.Now;
                    _m.Updater = Utilities.Security.CurrentUser.UserName;
                    _m.UpdateIP = Network.ClientIP;
                    service.Insert(_m);
                    json.Data = JsonUtil.GetSuccessForString("新增已完成");
                }
                else
                {
                    module.DisplayName = m.DisplayName;
                    module.UpperModuleID = m.UpperModuleID;
                    module.Src = m.Src;
                    module.ImageUrl = m.ImageUrl;
                    module.Sequence = m.Sequence;
                    module.Description = m.Description;
                    module.Notes = m.Notes;
                    if (m.Src != null && m.Src != string.Empty)
                    {
                        module.Hide = !m.Hide;
                        module.RefreshFlag = m.RefreshFlag;
                        module.AddFlag = m.AddFlag;
                        module.UpdateFlag = m.UpdateFlag;
                        module.DeleteFlag = m.DeleteFlag;
                        module.SubmitFlag = m.SubmitFlag;
                        module.QueryFlag = m.QueryFlag;
                        module.KeywordFlag = m.KeywordFlag;
                        if ((bool)module.RefreshFlag)
                            module.RefreshFunc = m.RefreshFunc;
                        else
                            module.RefreshFunc = null;
                        if ((bool)module.AddFlag)
                            module.AddFunc = m.AddFunc;
                        else
                            module.AddFunc = null;
                        if ((bool)module.UpdateFlag)
                            module.UpdateFunc = m.UpdateFunc;
                        else
                            module.UpdateFunc = null;
                        if ((bool)module.DeleteFlag)
                            module.DeleteFunc = m.DeleteFunc;
                        else
                            module.DeleteFunc = null;
                        if ((bool)module.SubmitFlag)
                            module.SubmitFunc = m.SubmitFunc;
                        else
                            module.SubmitFunc = null;
                        if ((bool)module.QueryFlag)
                            module.QueryFunc = m.QueryFunc;
                        else
                            module.QueryFunc = null;
                        if ((bool)module.RefreshFlag)
                            module.RefreshIcon = m.RefreshIcon;
                        else
                            module.RefreshIcon = null;
                        if ((bool)module.AddFlag)
                            module.AddIcon = m.AddIcon;
                        else
                            module.AddIcon = null;
                        if ((bool)module.UpdateFlag)
                            module.UpdateIcon = m.UpdateIcon;
                        else
                            module.UpdateIcon = null;
                        if ((bool)module.DeleteFlag)
                            module.DeleteIcon = m.DeleteIcon;
                        else
                            module.DeleteIcon = null;
                        if ((bool)module.SubmitFlag)
                            module.SubmitIcon = m.SubmitIcon;
                        else
                            module.SubmitIcon = null;
                        if ((bool)module.QueryFlag)
                            module.QueryIcon = m.QueryIcon;
                        else
                            module.QueryIcon = null;
                        if ((bool)module.RefreshFlag)
                            module.RefreshCaption = m.RefreshCaption;
                        else
                            module.RefreshCaption = null;
                        if ((bool)module.AddFlag)
                            module.AddCaption = m.AddCaption;
                        else
                            module.AddCaption = null;
                        if ((bool)module.UpdateFlag)
                            module.UpdateCaption = m.UpdateCaption;
                        else
                            module.UpdateCaption = null;
                        if ((bool)module.DeleteFlag)
                            module.DeleteCaption = m.DeleteCaption;
                        else
                            module.DeleteCaption = null;
                        if ((bool)module.SubmitFlag)
                            module.SubmitCaption = m.SubmitCaption;
                        else
                            module.SubmitCaption = null;
                        if ((bool)module.QueryFlag)
                            module.QueryCaption = m.QueryCaption;
                        else
                            module.QueryCaption = null;
                    }
                    else
                    {
                        module.Hide = false;
                        module.RefreshFlag = false;
                        module.AddFlag = false;
                        module.UpdateFlag = false;
                        module.DeleteFlag = false;
                        module.SubmitFlag = false;
                        module.QueryFlag = false;
                        module.KeywordFlag = false;
                        module.RefreshFunc = null;
                        module.AddFunc = null;
                        module.UpdateFunc = null;
                        module.DeleteFunc = null;
                        module.SubmitFunc = null;
                        module.QueryFunc = null;
                        module.RefreshIcon = null;
                        module.AddIcon = null;
                        module.UpdateIcon = null;
                        module.DeleteIcon = null;
                        module.SubmitIcon = null;
                        module.QueryIcon = null;
                        module.RefreshCaption = null;
                        module.AddCaption = null;
                        module.UpdateCaption = null;
                        module.DeleteCaption = null;
                        module.SubmitCaption = null;
                        module.QueryCaption = null;
                    }
                    module.UpdateTime = DateTime.Now;
                    module.Updater = Utilities.Security.CurrentUser.UserName;
                    module.UpdateIP = Network.ClientIP;
                    service.Update(module);
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