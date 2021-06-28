using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using DAL.Business;
using DAL;
using MyOrm.Common;

namespace Utilities.DataAdapter
{
    public class TreeGridAdapter
    {
        /// <summary>
        /// 根据“模块”数据生成TreeGrid格式的Json数据
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
                obj["name"] = dv[i]["DisplayName"].ToString().Trim();
                obj["src"] = dv[i]["Src"].ToString().Trim();
                obj["sequence"] = dv[i]["Sequence"].ToString().Trim();
                obj["visible"] = string.Empty;
                obj["add"] = string.Empty;
                obj["update"] = string.Empty;
                obj["delete"] = string.Empty;
                obj["submit"] = string.Empty;
                obj["updatetime"] = Convert.ToDateTime(dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["iconCls"] = dv[i]["ImageUrl"].ToString().Trim();
                array.Add(obj);

                this.GetModule(ref array, ref dv, dv[i]["ID"].ToString().Trim());

                dv.RowFilter = "UpperModuleID is null";
                dv.Sort = "Sequence";
            }
            return array;
        }

        private void GetModule(ref JArray _json_array, ref DataView _dv, string _pid)
        {
            _dv.RowFilter = "UpperModuleID='" + _pid + "'";
            _dv.Sort = "Sequence";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["name"] = _dv[i]["DisplayName"].ToString().Trim();
                obj["src"] = _dv[i]["Src"].ToString().Trim();
                obj["sequence"] = _dv[i]["Sequence"].ToString().Trim();
                obj["visible"] = Convert.ToBoolean(_dv[i]["QueryFlag"]) ? "1" : "0";
                obj["add"] = Convert.ToBoolean(_dv[i]["AddFlag"]) ? "1" : "0";
                obj["update"] = Convert.ToBoolean(_dv[i]["UpdateFlag"]) ? "1" : "0";
                obj["delete"] = Convert.ToBoolean(_dv[i]["DeleteFlag"]) ? "1" : "0";
                obj["submit"] = Convert.ToBoolean(_dv[i]["SubmitFlag"]) ? "1" : "0";
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["iconCls"] = _dv[i]["ImageUrl"].ToString().Trim();
                obj["_parentId"] = _pid;
                _json_array.Add(obj);

                this.GetModule(ref _json_array, ref _dv, _dv[i]["ID"].ToString().Trim());

                _dv.RowFilter = "UpperModuleID='" + _pid + "'";
                _dv.Sort = "Sequence";
            }
        }

        /// <summary>
        /// 根据“部门”数据生成TreeGrid格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetDept()
        {
            DataView dv = ServiceFactory.Factory.DeptsService.GetAll().DefaultView;

            JArray array = new JArray();
            dv.RowFilter = "ParentID='0'";
            dv.Sort = "DeptCode";
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = dv[i]["ID"].ToString().Trim();
                obj["deptid"] = dv[i]["DeptID"].ToString().Trim();
                obj["deptcode"] = dv[i]["DeptCode"].ToString().Trim();
                obj["deptname"] = dv[i]["DeptName"].ToString().Trim();
                obj["enabled"] = dv[i]["Enabled"].ToString().Trim();
                array.Add(obj);

                this.GetDept(ref array, ref dv, dv[i]["DeptID"].ToString().Trim());

                dv.RowFilter = "ParentID='0'";
                dv.Sort = "DeptCode";
            }
            return array;
        }

        private void GetDept(ref JArray _json_array, ref DataView _dv, string _pid)
        {
            _dv.RowFilter = "ParentID='" + _pid + "'";
            _dv.Sort = "DeptCode";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["deptid"] = _dv[i]["DeptID"].ToString().Trim();
                obj["deptcode"] = _dv[i]["DeptCode"].ToString().Trim();
                obj["deptname"] = _dv[i]["DeptName"].ToString().Trim();
                obj["enabled"] = _dv[i]["Enabled"].ToString().Trim();
                obj["_parentId"] = _pid;
                _json_array.Add(obj);

                this.GetDept(ref _json_array, ref _dv, _dv[i]["DeptID"].ToString().Trim());

                _dv.RowFilter = "ParentID='" + _pid + "'";
                _dv.Sort = "DeptCode";
            }
        }

        /// <summary>
        /// 根据“栏目”数据生成TreeGrid格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetClass(string keyword)
        {
            DataView dv = ServiceFactory.Factory.ClassesService.Query(keyword).DefaultView;

            JArray array = new JArray();
            if (keyword != null && keyword != string.Empty)
            {
                dv.Sort = "ClassCode";
                for (int i = 0; i < dv.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = dv[i]["ID"].ToString().Trim();
                    obj["classcode"] = dv[i]["ClassCode"].ToString().Trim();
                    obj["classname"] = dv[i]["ClassName"].ToString().Trim();
                    obj["description"] = dv[i]["Description"].ToString().Trim();
                    obj["enabled"] = dv[i]["Enabled"].ToString().Trim();
                    obj["updatetime"] = Convert.ToDateTime(dv[i]["UpdateTime"]);
                    array.Add(obj);
                }
            }
            else
            {
                dv.RowFilter = "UpperClassID is null";
                dv.Sort = "ClassCode";
                for (int i = 0; i < dv.Count; i++)
                {
                    JObject obj = new JObject();
                    obj["id"] = dv[i]["ID"].ToString().Trim();
                    obj["classcode"] = dv[i]["ClassCode"].ToString().Trim();
                    obj["classname"] = dv[i]["ClassName"].ToString().Trim();
                    obj["description"] = dv[i]["Description"].ToString().Trim();
                    obj["enabled"] = dv[i]["Enabled"].ToString().Trim();
                    obj["updatetime"] = Convert.ToDateTime(dv[i]["UpdateTime"]);
                    array.Add(obj);

                    this.GetClass(ref array, ref dv, dv[i]["ID"].ToString().Trim());

                    dv.RowFilter = "UpperClassID is null";
                    dv.Sort = "ClassCode";
                }
            }
            return array;
        }

        private void GetClass(ref JArray _json_array, ref DataView _dv, string _pid)
        {
            _dv.RowFilter = "UpperClassID='" + _pid + "'";
            _dv.Sort = "ClassCode";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["classcode"] = _dv[i]["ClassCode"].ToString().Trim();
                obj["classname"] = _dv[i]["ClassName"].ToString().Trim();
                obj["description"] = _dv[i]["Description"].ToString().Trim();
                obj["enabled"] = _dv[i]["Enabled"].ToString().Trim();
                obj["_parentId"] = _pid;
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]);
                _json_array.Add(obj);

                this.GetClass(ref _json_array, ref _dv, _dv[i]["ID"].ToString().Trim());

                _dv.RowFilter = "UpperClassID='" + _pid + "'";
                _dv.Sort = "ClassCode";
            }
        }


        /// <summary>
        /// 根据“账户”数据生成TreeGrid格式的Json数据
        /// </summary>
        /// <returns></returns>
        public JArray GetAccount(List<Employees> employees, DataView accounts)
        {
            Apps app = ServiceFactory.Factory.AppsService.SearchOne(new SimpleCondition("ID", 0));

            JArray array = new JArray();
            for (int i = 0; i < employees.Count; i++)
            {
                accounts.RowFilter = "EmployeeCode='" + employees[i].EmployeeCode + "' and AppID=0";
                if (accounts.Count > 0)
                {
                    for (int n = 0; n < accounts.Count; n++)
                    {
                        JObject obj = new JObject();
                        obj["ID"] = accounts[n]["ID"].ToString();
                        obj["EmployeeID"] = employees[i].EmployeeCode;
                        obj["EmployeeCode"] = employees[i].EmployeeCode;
                        obj["ChineseName"] = employees[i].ChineseName;
                        obj["AppID"] = Convert.ToInt16(accounts[n]["ID"]);
                        obj["AppName"] = accounts[n]["AppName"].ToString();
                        obj["LoginCode"] = accounts[n]["LoginCode"].ToString().Trim();
                        obj["KeyNum"] = accounts[n]["KeyNum"].ToString().Trim();
                        obj["Description"] = accounts[n]["Description"].ToString().Trim();
                        obj["Enabled"] = Convert.ToBoolean(accounts[n]["Enabled"]);
                        obj["UpdateTime"] = Convert.ToDateTime(accounts[n]["UpdateTime"]);
                        array.Add(obj);

                        GetAccount(ref array, ref accounts, employees[i].EmployeeCode, accounts[n]["ID"].ToString());
                    }
                }
                else
                {
                    JObject obj = new JObject();
                    obj["ID"] = Guid.NewGuid().ToString();
                    obj["EmployeeID"] = employees[i].EmployeeCode;
                    obj["EmployeeCode"] = employees[i].EmployeeCode;
                    obj["ChineseName"] = employees[i].ChineseName;
                    obj["AppID"] = app.ID;
                    obj["AppName"] =  app.AppName;
                    array.Add(obj);
                }
            }
            return array;
        }

        private void GetAccount(ref JArray _json_array, ref DataView _accounts, string _employeeCode, string _pId)
        {
            _accounts.RowFilter = "EmployeeCode='" + _employeeCode + "' and AppID>0";
            _accounts.Sort = "ID";
            for (int i = 0; i < _accounts.Count; i++)
            {
                JObject obj = new JObject();
                obj["ID"] = _accounts[i]["ID"].ToString();
                obj["EmployeeID"] = _accounts[i]["EmployeeCode"].ToString();
                obj["EmployeeCode"] = string.Empty;//_accounts[i]["EmployeeCode"].ToString();
                obj["ChineseName"] = string.Empty;//_accounts[i]["ChineseName"].ToString();
                obj["AppID"] = Convert.ToInt16(_accounts[i]["AppId"]);
                obj["AppName"] = _accounts[i]["AppName"].ToString();
                obj["LoginCode"] = _accounts[i]["LoginCode"].ToString().Trim();
                obj["KeyNum"] = _accounts[i]["KeyNum"].ToString().Trim();
                obj["Description"] = _accounts[i]["Description"].ToString().Trim();
                obj["UpdateTime"] = Convert.ToDateTime(_accounts[i]["UpdateTime"]);
                obj["Enabled"] = Convert.ToBoolean(_accounts[i]["Enabled"]);
                obj["_parentId"] = _pId;
                _json_array.Add(obj);
            }
        }
    }
}
