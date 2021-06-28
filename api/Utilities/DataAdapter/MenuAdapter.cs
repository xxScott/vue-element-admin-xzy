using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using DAL.Business;

namespace Utilities.DataAdapter
{
    public class MenuAdapter
    {
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
                if (_dv[i]["Src"].ToString().Trim()!=string.Empty)
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
