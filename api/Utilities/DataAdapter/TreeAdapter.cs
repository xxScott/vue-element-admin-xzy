using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using DAL.Business;

namespace Utilities.DataAdapter
{
    public class TreeAdapter
    {
        public static string module_func_char = "#";
        //public static string module_func_visible = "visible";
        public static string module_func_add = "add";
        public static string module_func_update = "update";
        public static string module_func_delete = "delete";
        public static string module_func_submit = "submit";
        public static string module_func_query = "query";

        #region   GetModule

        /// <summary>
        /// 根据“模块”数据生成Tree格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetModule()
        {
            DataView dv = ServiceFactory.Factory.ModulesService.GetAll().DefaultView;

            JArray array = new JArray();
            dv.RowFilter = "UpperModuleID is null";
            dv.Sort = "Sequence";
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = dv[i]["ID"].ToString().Trim();
                obj["text"] = dv[i]["DisplayName"].ToString().Trim();
                JArray _array = this.GetModule(ref dv, dv[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                {
                    obj["children"] = _array;
                }
                array.Add(obj);

                dv.RowFilter = "UpperModuleID is null";
                dv.Sort = "Sequence";
            }
            return array;
        }

        private JArray GetModule(ref DataView _dv, string _pid)
        {
            JArray array = new JArray();
            _dv.RowFilter = "UpperModuleID='" + _pid + "'";
            _dv.Sort = "Sequence";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["text"] = _dv[i]["DisplayName"].ToString().Trim();

                //if (_dv[i]["Src"].ToString().Trim() != string.Empty)
                //{
                //    JArray _array_func = this.GetModuleFunc(_dv[i]["ID"].ToString().Trim(), Convert.ToBoolean(_dv[i]["QueryFlag"]), Convert.ToBoolean(_dv[i]["AddFlag"]), Convert.ToBoolean(_dv[i]["UpdateFlag"]), Convert.ToBoolean(_dv[i]["DeleteFlag"]), Convert.ToBoolean(_dv[i]["SubmitFlag"]));
                //    if (_array_func.Count > 0)
                //        obj["children"] = _array_func;
                //}

                JArray _array = this.GetModule(ref _dv, _dv[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                    obj["children"] = _array;
                array.Add(obj);

                _dv.RowFilter = "UpperModuleID='" + _pid + "'";
                _dv.Sort = "Sequence";
            }
            return array;
        }

        //private JArray GetModuleFunc(string _id, bool visible_func, bool add_func, bool update_func, bool delete_func, bool submit_func)
        //{
        //    JArray array = new JArray();

        //    if (visible_func)
        //    {
        //        JObject obj_visible = new JObject();
        //        obj_visible["id"] = _id + module_func_char + module_func_visible;
        //        obj_visible["text"] = "可见";
        //        array.Add(obj_visible);
        //    }

        //    if (add_func)
        //    {
        //        JObject obj_add = new JObject();
        //        obj_add["id"] = _id + module_func_char + module_func_add;
        //        obj_add["text"] = "添加";
        //        array.Add(obj_add);
        //    }

        //    if (update_func)
        //    {
        //        JObject obj_update = new JObject();
        //        obj_update["id"] = _id + module_func_char + module_func_update;
        //        obj_update["text"] = "修改";
        //        array.Add(obj_update);
        //    }

        //    if (delete_func)
        //    {
        //        JObject obj_delete = new JObject();
        //        obj_delete["id"] = _id + module_func_char + module_func_delete;
        //        obj_delete["text"] = "删除";
        //        array.Add(obj_delete);
        //    }

        //    if (submit_func)
        //    {
        //        JObject obj_submit = new JObject();
        //        obj_submit["id"] = _id + module_func_char + module_func_submit;
        //        obj_submit["text"] = "提交";
        //        array.Add(obj_submit);
        //    }

        //    return array;
        //}

        #endregion

        #region   GetRight

        /// <summary>
        /// 根据“权限”数据生成Tree格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetRight(int roleId)
        {
            DataView modules = ServiceFactory.Factory.ModulesService.GetAll().DefaultView;
            DataView rights = ServiceFactory.Factory.RightsService.Search(roleId).DefaultView;

            JArray array = new JArray();
            modules.RowFilter = "UpperModuleID is null";
            modules.Sort = "Sequence";
            for (int i = 0; i < modules.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = modules[i]["ID"].ToString().Trim();
                obj["text"] = modules[i]["DisplayName"].ToString().Trim();
                JArray _array = this.GetRight(ref modules, ref rights, modules[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                {
                    obj["children"] = _array;
                }
                array.Add(obj);

                modules.RowFilter = "UpperModuleID is null";
                modules.Sort = "Sequence";
            }
            return array;
        }

        private JArray GetRight(ref DataView _modules, ref DataView _rights, string _pid)
        {
            JArray array = new JArray();
            _modules.RowFilter = "UpperModuleID='" + _pid + "'";
            _modules.Sort = "Sequence";
            for (int i = 0; i < _modules.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _modules[i]["ID"].ToString().Trim();
                obj["text"] = _modules[i]["DisplayName"].ToString().Trim();
                if (_modules[i]["Src"].ToString().Trim() != string.Empty)
                {
                    JArray _array_func = this.GetRightFunc(ref _modules, ref _rights, _modules[i]["ID"].ToString().Trim());
                    if (_array_func.Count > 0)
                    {
                        obj["children"] = _array_func;
                    }
                    else
                    {
                        obj["checked"] = GetChecked(ref _rights, _modules[i]["ID"].ToString().Trim());
                    }
                    array.Add(obj);
                }
                else
                {
                    JArray _array = this.GetRight(ref _modules, ref _rights, _modules[i]["ID"].ToString().Trim());
                    if (_array.Count > 0)
                    {
                        obj["children"] = _array;
                    }
                    array.Add(obj);
                }
                _modules.RowFilter = "UpperModuleID='" + _pid + "'";
                _modules.Sort = "Sequence";
            }
            return array;
        }

        private bool GetChecked(ref DataView _rights, string _id)
        {
            bool visible_func_checked = false;

            DataRow[] drc=_rights.Table.Select("ModuleID='" + _id + "'");
            if (drc.Length > 0) visible_func_checked = Convert.ToBoolean(drc[0]["Visible"]);

            return visible_func_checked;
        }

        //private JArray GetRightFunc(ref DataView _dv, ref DataView _dat, string _id)
        //{
        //    JArray array = new JArray();
        //    bool visible_func_checked = false;

        //    _dat.RowFilter = "ModuleID='" + _id + "'";
        //    if (_dat.Count > 0)
        //    {
        //        visible_func_checked = Convert.ToBoolean(_dat[0]["Visible"]);
        //    }

        //    JObject obj_visible = new JObject();
        //    obj_visible["id"] = _id + module_func_char + module_func_visible;
        //    obj_visible["text"] = "可见";
        //    obj_visible["checked"] = visible_func_checked;
        //    obj_visible["children"] = GetSubRightFunc(ref _dv, ref _dat, _id);
        //    array.Add(obj_visible);
        //    return array;
        //}

        private JArray GetRightFunc(ref DataView _modules, ref DataView _rights, string _id)
        {
            JArray array = new JArray();
            bool add_flag = false, update_flag = false, delete_flag = false, submit_flag = false, query_flag = false ;
            bool add_func_checked = false, update_func_checked = false, delete_func_checked = false, submit_func_checked = false, query_func_checked = false;
            string add_caption = "添加", delete_caption = "删除", update_caption = "修改", submit_caption = "提交", query_caption = "查询";

            DataRow[] drc_right = _rights.Table.Select("ModuleID='" + _id + "'");
            if (drc_right.Length > 0)
            {
                add_func_checked = Convert.ToBoolean(drc_right[0]["HasAdd"]);
                update_func_checked = Convert.ToBoolean(drc_right[0]["HasUpdate"]);
                delete_func_checked = Convert.ToBoolean(drc_right[0]["HasDelete"]);
                submit_func_checked = Convert.ToBoolean(drc_right[0]["HasSubmit"]);
                query_func_checked = Convert.ToBoolean(drc_right[0]["HasQuery"]);
            }

            DataRow[] drc_module = _modules.Table.Select("ID='" + _id + "'");
            if (drc_module.Length > 0)
            {
                
                add_flag = Convert.ToBoolean(drc_module[0]["AddFlag"]);
                update_flag = Convert.ToBoolean(drc_module[0]["UpdateFlag"]);
                delete_flag = Convert.ToBoolean(drc_module[0]["DeleteFlag"]);
                submit_flag = Convert.ToBoolean(drc_module[0]["SubmitFlag"]);
                query_flag = Convert.ToBoolean(drc_module[0]["QueryFlag"]);

                add_caption = drc_module[0]["AddCaption"].ToString().Trim();
                delete_caption = drc_module[0]["DeleteCaption"].ToString().Trim();
                update_caption = drc_module[0]["UpdateCaption"].ToString().Trim();
                submit_caption = drc_module[0]["SubmitCaption"].ToString().Trim();
                query_caption = drc_module[0]["QueryCaption"].ToString().Trim();
            }
            
            if (add_flag)
            {
                JObject obj_add = new JObject();
                obj_add["id"] = _id + module_func_char + module_func_add;
                obj_add["text"] = add_caption;
                obj_add["checked"] = add_func_checked;
                array.Add(obj_add);
            }
            if (update_flag)
            {
                JObject obj_update = new JObject();
                obj_update["id"] = _id + module_func_char + module_func_update;
                obj_update["text"] = update_caption;
                obj_update["checked"] = update_func_checked;
                array.Add(obj_update);
            }
            if (delete_flag)
            {
                JObject obj_delete = new JObject();
                obj_delete["id"] = _id + module_func_char + module_func_delete;
                obj_delete["text"] = delete_caption;
                obj_delete["checked"] = delete_func_checked;
                array.Add(obj_delete);
            }
            if (submit_flag)
            {
                JObject obj_submit = new JObject();
                obj_submit["id"] = _id + module_func_char + module_func_submit;
                obj_submit["text"] = submit_caption;
                obj_submit["checked"] = submit_func_checked;
                array.Add(obj_submit);
            }
            if (query_flag)
            {
                JObject obj_add = new JObject();
                obj_add["id"] = _id + module_func_char + module_func_query;
                obj_add["text"] = query_caption;
                obj_add["checked"] = query_func_checked;
                array.Add(obj_add);
            }
            return array;
        }

        #endregion

        #region   GetDept

        /// <summary>
        /// 根据“部门”数据生成Tree格式的Json数据
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetDept(string keyword)
        {
            DataView dv = ServiceFactory.Factory.DeptsService.Query(keyword).DefaultView;

            JArray array = new JArray();
            if (keyword != null && keyword != string.Empty)
            {
                dv.Sort = "DeptCode";
                for (int i = 0; i < dv.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = dv[i]["DeptID"].ToString().Trim();
                    obj["text"] = dv[i]["DeptName"].ToString().Trim();
                    obj["state"] = "opened";
                    obj["children"] = null;
                    array.Add(obj);
                }
            }
            else
            {
                dv.RowFilter = "ParentID='0'";
                dv.Sort = "DeptCode";
                for (int i = 0; i < dv.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = dv[i]["DeptID"].ToString().Trim();
                    obj["text"] = dv[i]["DeptName"].ToString().Trim();
                    JArray _array = this.GetDept(ref dv, dv[i]["DeptID"].ToString().Trim());
                    if (_array.Count > 0)
                    {
                        obj["state"] = "opened";
                        obj["children"] = _array;
                    }
                    array.Add(obj);

                    dv.RowFilter = "ParentID='0'";
                    dv.Sort = "DeptCode";
                }
            }
            return array;
        }

        private JArray GetDept(ref DataView _dv, string _pid)
        {
            JArray array = new JArray();
            _dv.RowFilter = "ParentID='" + _pid + "'";
            _dv.Sort = "DeptCode";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["DeptID"].ToString().Trim();
                obj["text"] = _dv[i]["DeptName"].ToString().Trim();
                JArray _array = this.GetDept(ref _dv, _dv[i]["DeptID"].ToString().Trim());
                if (_array.Count > 0)
                {
                    obj["state"] = "closed";
                    obj["children"] = _array;
                }
                array.Add(obj);

                _dv.RowFilter = "ParentID='" + _pid + "'";
                _dv.Sort = "DeptCode";
            }
            return array;
        }

        #endregion
    }
}

