using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using DAL.Business;

namespace Adapters
{
    public class MenuAdapter
    {
        /// <summary>
        /// 获取所有“模块”数据生成Menu格式的Json数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JArray GetAllModule(int userId)
        {
            DataView dv = ServiceFactory.Factory.ModulesService.GetParentByUser(userId).DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();

                obj["path"] = dv[i]["Path"].ToString().Trim();
                obj["component"] = dv[i]["Component"].ToString().Trim();
                obj["name"] = dv[i]["Name"].ToString().Trim();
                obj["redirect"] = dv[i]["Redirect"].ToString().Trim();
                obj["alwaysShow"] = dv[i]["AlwaysShow"].ToString().Trim();
                obj["hidden"] = dv[i]["Hidden"].ToString().Trim();

                JObject objMeta = new JObject();
                objMeta["title"] = dv[i]["Title"].ToString().Trim();
                objMeta["icon"] = dv[i]["Icon"].ToString().Trim();
                objMeta["nocache"] = dv[i]["NoCache"].ToString().Trim();
                objMeta["breadcrumb"] = dv[i]["BreadCrumb"].ToString().Trim();
                objMeta["affix"] = dv[i]["Affix"].ToString().Trim();
                obj["meta"] = objMeta;

                obj["children"] = GetChildModule(int.Parse(dv[i]["ID"].ToString()), userId);

                obj["menuid"] = dv[i]["ID"].ToString().Trim();
                obj["menuname"] = dv[i]["DisplayName"].ToString().Trim();
                //obj["icon"] = dv[i]["ImageUrl"].ToString().Trim();
                //DataView childs = ServiceFactory.Factory.ModulesService.GetChildByUser(Convert.ToInt32(dv[i]["ID"].ToString()), userId).DefaultView;
                //obj["childCount"] = childs.Count;
                //obj["menus"] = GetChildModule(int.Parse(dv[i]["ID"].ToString()), userId);

                array.Add(obj);
            }
            return array;
        }

        /// <summary>
        /// 根据顶层“模块”数据生成Menu格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetParentModule(int userId)
        {
            DataView dv = ServiceFactory.Factory.ModulesService.GetParentByUser(userId).DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["menuid"] = dv[i]["ID"].ToString().Trim();
                obj["menuname"] = dv[i]["DisplayName"].ToString().Trim();
                obj["icon"] = dv[i]["ImageUrl"].ToString().Trim();
                DataView childs = ServiceFactory.Factory.ModulesService.GetChildByUser(Convert.ToInt32(dv[i]["ID"].ToString()), userId).DefaultView;
                obj["childCount"] = childs.Count;
                obj["menus"] = new JArray();
                array.Add(obj);
            }
            return array;
        }

        /// <summary>
        /// 根据顶层“模块”ID查询子模块数据生成Menu格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetChildModule(int moduleId, int userId)
        {
            DataView dv = ServiceFactory.Factory.ModulesService.GetChildByUser(moduleId, userId).DefaultView;

            JArray array = new JArray();
            dv.RowFilter = "UpperModuleID is not null";
            dv.Sort = "Sequence";
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();

                obj["path"] = dv[i]["Path"].ToString().Trim();
                obj["component"] = dv[i]["Component"].ToString().Trim();
                obj["name"] = dv[i]["Name"].ToString().Trim();

                JObject objMeta = new JObject();
                objMeta["title"] = dv[i]["Title"].ToString().Trim();
                objMeta["icon"] = dv[i]["Icon"].ToString().Trim();
                objMeta["nocache"] = dv[i]["NoCache"].ToString().Trim();
                objMeta["breadcrumb"] = dv[i]["BreadCrumb"].ToString().Trim();
                objMeta["affix"] = dv[i]["Affix"].ToString().Trim();
                obj["meta"] = objMeta;

                obj["redirect"] = dv[i]["Redirect"].ToString().Trim();
                obj["alwaysShow"] = dv[i]["AlwaysShow"].ToString().Trim();
                obj["hidden"] = dv[i]["Hidden"].ToString().Trim();

                obj["menuid"] = dv[i]["ID"].ToString().Trim();
                obj["menuname"] = dv[i]["DisplayName"].ToString().Trim();
                //obj["icon"] = dv[i]["ImageUrl"].ToString().Trim();
                //obj["menus"] = this.GetModule(ref dv, dv[i]["ID"].ToString().Trim());
                array.Add(obj);

                dv.RowFilter = "UpperModuleID is not null";
                dv.Sort = "Sequence";
            }
            return array;
        }


        /// <summary>
        /// 根据“模块”数据生成Menu格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetModule(int userId)
        {
            DataView dv = ServiceFactory.Factory.ModulesService.GetByUser(userId).DefaultView;

            JArray array = new JArray();
            dv.RowFilter = "UpperModuleID is null";
            dv.Sort = "Sequence";
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["menuid"] = dv[i]["ID"].ToString().Trim();
                obj["menuname"] = dv[i]["DisplayName"].ToString().Trim();
                obj["icon"] = dv[i]["ImageUrl"].ToString().Trim();
                obj["menus"] = this.GetModule(ref dv, dv[i]["ID"].ToString().Trim());
                array.Add(obj);

                dv.RowFilter = "UpperModuleID is null";
                dv.Sort = "Sequence";
            }
            return array;
        }

        /// <summary>
        /// 根据“模块”数据生成Menu格式的Json数据
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
                obj["menuid"] = dv[i]["ID"].ToString().Trim();
                obj["menuname"] = dv[i]["DisplayName"].ToString().Trim();
                obj["icon"] = dv[i]["ImageUrl"].ToString().Trim();
                obj["menus"] = this.GetModule(ref dv, dv[i]["ID"].ToString().Trim());
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
                obj["menuid"] = _dv[i]["ID"].ToString().Trim();
                obj["menuname"] = _dv[i]["DisplayName"].ToString().Trim();
                obj["icon"] = _dv[i]["ImageUrl"].ToString().Trim();
                if (_dv[i]["Src"].ToString().Trim() != string.Empty)
                    obj["url"] = _dv[i]["Src"].ToString().Trim() + "?ModuleID=" + _dv[i]["ID"].ToString().Trim();
                JArray _array = this.GetModule(ref _dv, _dv[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                    obj["child"] = _array;
                array.Add(obj);

                _dv.RowFilter = "UpperModuleID='" + _pid + "'";
                _dv.Sort = "Sequence";
            }
            return array;
        }

    }
}
