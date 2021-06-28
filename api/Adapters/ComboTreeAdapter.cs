using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.Business;
using Newtonsoft.Json.Linq;
using DAL;
using MyOrm.Common;

namespace Adapters
{
    public class ComboTreeAdapter
    {
        /// <summary>
        /// 根据“模块”数据生成ComboTree格式的Json数据
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
                JArray _array = this.GetModule(ref _dv, _dv[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                {
                    obj["children"] = _array;
                }
                array.Add(obj);

                _dv.RowFilter = "UpperModuleID='" + _pid + "'";
                _dv.Sort = "Sequence";
            }
            return array;
        }

        /// <summary>
        /// 根据“栏目”数据生成ComboTree格式的Json数据
        /// </summary>
        /// <param name="companyId">分公司ID</param>
        /// <returns></returns>
        public JArray GetDept(int groupId, int storeId)
        {
            //if (groupId != -1)
            //{
            //    storeId = -1;
            //}

            DataView dv = ServiceFactory.Factory.DeptsService.GetAll(groupId, storeId).DefaultView;
            return GetDept(dv);
        }

        /// <summary>
        /// 根据“栏目”数据生成ComboTree格式的Json数据
        /// </summary>
        /// <param name="companyId">分公司ID</param>
        /// <returns></returns>
        public JArray GetStore(int groupId, int storeId)
        {
            //if (groupId != -1)
            //{
            //    storeId = -1;
            //}
            ConditionSet condition = new ConditionSet();
            List<Stores> lstStores = ServiceFactory.Factory.StoresService.Search(condition);
            return GetStore(lstStores);
        }

        /// <summary>
        /// 根据“栏目”数据生成ComboTree格式的Json数据
        /// </summary>
        /// <param name="companyId">分公司ID</param>
        /// <returns></returns>
        public JArray GetStoreByID(int groupId, int storeId)
        {
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("UID", storeId));
            List<Stores> lstStores = ServiceFactory.Factory.StoresService.Search(condition);
            return GetStore(lstStores);
        }

        /// <summary>
        /// 根据“栏目”数据生成ComboTree格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetStore(List<Stores> lstStores)
        {
            JArray array = new JArray();
            for (int i = 0; i < lstStores.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = lstStores[i].ID;
                obj["text"] = lstStores[i].StoreName;
                JArray _array = this.GetDept(lstStores[i].GroupID.Value, lstStores[i].UID.Value);
                if (_array.Count > 0)
                {
                    obj["children"] = _array;
                }
                array.Add(obj);

               
            }
            return array;
        }

        /// <summary>
        /// 根据“栏目”数据生成ComboTree格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetDept(List<Depts> list)
        {
            DataTable dat = new DataTable();
            dat.Columns.Add("ID");
            dat.Columns.Add("DeptCode");
            dat.Columns.Add("DeptName");
            dat.Columns.Add("UpperDeptID");
            for (int i = 0; i < list.Count; i++)
            {
                DataRow dr = dat.NewRow();
                dr["ID"] = list[i].ID;
                dr["DeptCode"] = list[i].DeptCode;
                dr["DeptName"] = list[i].DeptName;
                dr["UpperDeptID"] = list[i].UpperDeptID;
                dat.Rows.Add(dr);
            }
            return GetDept(dat.DefaultView);
        }

        /// <summary>
        /// 根据“栏目”数据生成ComboTree格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetDept(DataView dv)
        {
            JArray array = new JArray();
            dv.RowFilter = "UpperDeptID is null";
            dv.Sort = "DeptCode";
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = dv[i]["ID"].ToString().Trim();
                obj["text"] = dv[i]["DeptName"].ToString().Trim();
                JArray _array = this.GetDept(ref dv, dv[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                {
                    obj["children"] = _array;
                }
                array.Add(obj);

                dv.RowFilter = "UpperDeptID is null";
                dv.Sort = "DeptCode";
            }
            return array;
        }

        private JArray GetDept(ref DataView _dv, string _pid)
        {
            JArray array = new JArray();

            _dv.RowFilter = "UpperDeptID='" + _pid + "'";
            _dv.Sort = "DeptCode";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["text"] = _dv[i]["DeptName"].ToString().Trim();
                JArray _array = this.GetDept(ref _dv, _dv[i]["ID"].ToString().Trim());
                if (_array.Count > 0)
                {
                    obj["children"] = _array;
                }
                array.Add(obj);

                _dv.RowFilter = "UpperDeptID='" + _pid + "'";
                _dv.Sort = "DeptCode";
            }
            return array;
        }

    }
}
