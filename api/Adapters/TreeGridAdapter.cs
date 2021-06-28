using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using DAL.Business;
using DAL;
using MyOrm.Common;

namespace Adapters
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
                obj["refresh"] = string.Empty;
                obj["visible"] = string.Empty;
                obj["add"] = string.Empty;
                obj["update"] = string.Empty;
                obj["delete"] = string.Empty;
                obj["submit"] = string.Empty;
                obj["query"] = string.Empty;
                obj["keyword"] = string.Empty;
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
                obj["refresh"] = Convert.ToBoolean(_dv[i]["RefreshFlag"]) ? "1" : "0";
                obj["add"] = Convert.ToBoolean(_dv[i]["AddFlag"]) ? "1" : "0";
                obj["update"] = Convert.ToBoolean(_dv[i]["UpdateFlag"]) ? "1" : "0";
                obj["delete"] = Convert.ToBoolean(_dv[i]["DeleteFlag"]) ? "1" : "0";
                obj["submit"] = Convert.ToBoolean(_dv[i]["SubmitFlag"]) ? "1" : "0";
                obj["query"] = Convert.ToBoolean(_dv[i]["QueryFlag"]) ? "1" : "0";
                obj["keyword"] = Convert.ToBoolean(_dv[i]["KeywordFlag"]) ? "1" : "0";
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["iconCls"] = _dv[i]["ImageUrl"].ToString().Trim();
                obj["_parentId"] = _pid;
                _json_array.Add(obj);

                this.GetModule(ref _json_array, ref _dv, _dv[i]["ID"].ToString().Trim());

                _dv.RowFilter = "UpperModuleID='" + _pid + "'";
                _dv.Sort = "Sequence";
            }
        }

        ///// <summary>
        ///// 根据“公司”数据生成TreeGrid格式的Json数据
        ///// </summary>
        ///// <param name="companyId">公司ID</param>
        ///// <param name="keyword">关键字</param>
        ///// <returns></returns>
        //public JArray GetCompany(int groupId, int storeId, string keyword)
        //{
        //    DataView dv = ServiceFactory.Factory.CompanysService.Query(groupId, storeId, keyword).DefaultView;

        //    JArray array = new JArray();
        //    dv.RowFilter = "UpperCompanyID is null";
        //    dv.Sort = "CompanyCode";
        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        JObject obj = new JObject();
        //        obj["id"] = dv[i]["ID"].ToString().Trim();
        //        obj["companycode"] = dv[i]["CompanyCode"].ToString().Trim();
        //        obj["companyname"] = dv[i]["CompanyName"].ToString().Trim();
        //        obj["description"] = dv[i]["Description"].ToString().Trim();
        //        obj["updatetime"] = Convert.ToDateTime(dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
        //        array.Add(obj);

        //        this.GetDept(ref array, ref dv, dv[i]["ID"].ToString().Trim());

        //        dv.RowFilter = "UpperCompanyID is null";
        //        dv.Sort = "CompanyCode";
        //    }
        //    return array;
        //}

        /// <summary>
        /// 根据“部门”数据生成TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetDept(int groupId, int storeId, string keyword)
        {
            DataView dv = ServiceFactory.Factory.DeptsService.Query(groupId, storeId, keyword).DefaultView;

            JArray array = new JArray();
            dv.RowFilter = "UpperDeptID is null";
            dv.Sort = "DeptCode";
            for (int i = 0; i < dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = dv[i]["ID"].ToString().Trim();
                obj["deptcode"] = dv[i]["DeptCode"].ToString().Trim();
                obj["deptname"] = dv[i]["DeptName"].ToString().Trim();
                obj["description"] = dv[i]["Description"].ToString().Trim();
                obj["updatetime"] = Convert.ToDateTime(dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                array.Add(obj);

                this.GetDept(ref array, ref dv, dv[i]["ID"].ToString().Trim());

                dv.RowFilter = "UpperDeptID is null";
                dv.Sort = "DeptCode";
            }
            return array;
        }


        private void GetDept(ref JArray _json_array, ref DataView _dv, string _pid)
        {
            _dv.RowFilter = "UpperDeptID=" + _pid;
            _dv.Sort = "DeptCode";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["deptcode"] = _dv[i]["DeptCode"].ToString().Trim();
                obj["deptname"] = _dv[i]["DeptName"].ToString().Trim();
                obj["description"] = _dv[i]["Description"].ToString().Trim();
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["_parentId"] = _pid;
                _json_array.Add(obj);

                this.GetDept(ref _json_array, ref _dv, _dv[i]["ID"].ToString().Trim());

                _dv.RowFilter = "UpperDeptID=" + _pid;
                _dv.Sort = "DeptCode";
            }
        }


        /// <summary>
        /// 根据“公司”数据获取“员工”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetSalaryPeroid(int companyId, int state)
        {
            ISalarySettleService service = ServiceFactory.Factory.SalarySettleService;

            DataView dv = service.Query(companyId, state).DefaultView;
            string[] PID = new string[dv.Count];
            JArray array = new JArray();

            for (int i = 0; i < dv.Count; i++)
            {

                if (Array.IndexOf<string>(PID, dv[i]["PID"].ToString().Trim()) == -1)
                {
                    dv.RowFilter = "PID=" + dv[i]["PID"].ToString().Trim();
                    double total = 0, deduct = 0, real = 0;
                    for (int j = 0; j < dv.Count; j++)
                    {
                        total += Convert.ToDouble(dv[j]["TotalAmount"].ToString());
                        deduct += Convert.ToDouble(dv[j]["DeductAmount"].ToString());
                        real += Convert.ToDouble(dv[j]["RealAmount"].ToString());
                    }
                    dv.RowFilter = string.Empty;
                    JObject obj = new JObject();
                    obj["id"] = dv[i]["PID"].ToString().Trim();
                    obj["name"] = dv[i]["PeriodName"].ToString().Trim();
                    obj["TotalAmount"] = total;
                    obj["DeductAmount"] = deduct;
                    obj["RealAmount"] = real;
                    obj["StartDate"] = dv[i]["StartDate"].ToString().Trim();
                    obj["EndDate"] = dv[i]["EndDate"].ToString().Trim();
                    if (dv[i]["HasSettled"].ToString() != "")
                    {
                        obj["HasSettled"] = Convert.ToBoolean(dv[i]["HasSettled"].ToString());
                        obj["StateName"] = Convert.ToBoolean(dv[i]["HasSettled"].ToString()) ? "已结算" : "未结算";
                    }
                    else
                    {
                        obj["HasSettled"] = false;
                        obj["StateName"] = "未结算";
                    }
                    array.Add(obj);
                    PID[i] = dv[i]["PID"].ToString().Trim();
                    this.GetSalaryAssess(ref array, ref dv, dv[i]["PID"].ToString().Trim());
                }

            }
            return array;
        }
        /// <summary>
        /// 根据“员工”数据获取“关系人”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        private void GetSalaryAssess(ref JArray _json_array, ref DataView _dv, string pid)
        {
            _dv.RowFilter = "PID=" + pid;
            _dv.Sort = "UpdateTime";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["name"] = _dv[i]["EmployeeName"].ToString().Trim();
                obj["TotalAmount"] = _dv[i]["TotalAmount"].ToString().Trim();
                obj["DeductAmount"] = _dv[i]["DeductAmount"].ToString().Trim();
                obj["RealAmount"] = _dv[i]["RealAmount"].ToString().Trim();
                obj["DeptName"] = _dv[i]["DeptName"].ToString().Trim();
                obj["StationName"] = _dv[i]["StationName"].ToString().Trim();
                obj["UpdateTime"] = _dv[i]["UpdateTime"].ToString().Trim();
                obj["_parentId"] = pid;
                _json_array.Add(obj);
            }
            _dv.RowFilter = string.Empty;
            _dv.Sort = "UpdateTime";
        }



        /// <summary>
        /// 根据“公司”数据获取“员工”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetEmpLeaveCheck(int storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            IEmployeeLeaveService service = ServiceFactory.Factory.EmployeeLeaveService;
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("StoreID", storeId));
            condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_LevelState.Wait)));

            if (keyword != null && keyword != string.Empty)
            {
                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                _condition.Add(new SimpleCondition("EmployeeCode", ConditionOperator.Like, "%" + keyword + "%"));
                _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                _condition.Add(new SimpleCondition("RequestDate", ConditionOperator.Like, "%" + keyword + "%"));
                _condition.Add(new SimpleCondition("LeaveDate", ConditionOperator.Like, "%" + keyword + "%"));
                _condition.Add(new SimpleCondition("LevelReason", ConditionOperator.Like, "%" + keyword + "%"));
                _condition.Add(new SimpleCondition("Description", ConditionOperator.Like, "%" + keyword + "%"));
                condition.Add(_condition);
            }
            //查询记录总数
            totalCount = service.Count(condition);
            //查询分页数据
            List<EmployeeLeaveView> empLeave = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);
            List<EmployeeLeaveView> emp = new List<EmployeeLeaveView>();
            for (int i = 0; i < empLeave.Count; i++)
            {
                if (!emp.Exists(e => e.EmployeeID == empLeave[i].EmployeeID))
                {
                    emp.Add(empLeave[i]);
                }
            }

            JArray array = new JArray();
            for (int i = 0; i < emp.Count; i++)
            {

                JObject obj = new JObject();
                obj["id"] = emp[i].EmployeeID.ToString().Trim();
                obj["name"] = emp[i].EmployeeName.ToString().Trim();
                array.Add(obj);

                this.GetLeaveCheck(ref array, ref empLeave, emp[i].EmployeeID.ToString().Trim());
            }
            return array;
        }
        /// <summary>
        /// 根据“员工”数据获取“关系人”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        private void GetLeaveCheck(ref JArray _json_array, ref List<EmployeeLeaveView> _list, string _empId)
        {

            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].EmployeeID == Convert.ToInt32(_empId))
                {
                    JObject obj = new JObject();
                    obj["id"] = _list[i].ID.ToString().Trim();
                    obj["name"] = "";
                    obj["requestdate"] = _list[i].RequestDate.ToString().Trim();
                    if (_list[i].LeaveDate != null)
                    {
                        obj["leavedate"] = _list[i].LeaveDate.ToString();
                    }
                    obj["levelstatus"] = DAL.Util.GetDisplayName((Enum_LeavelStatus)_list[i].LevelStatus); ;
                    if (_list[i].LevelReason != null)
                    {
                        obj["levelreason"] = _list[i].LevelReason.ToString().Trim();
                    }
                    obj["statename"] = DAL.Util.GetDisplayName((Enum_LevelState)_list[i].State);
                    if (_list[i].Description != null)
                    {
                        obj["description"] = _list[i].Description.ToString().Trim();
                    }
                    obj["updatetime"] = Convert.ToDateTime(_list[i].UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                    obj["_parentId"] = _empId;
                    _json_array.Add(obj);
                }
            }
        }
        /// <summary>
        /// 根据“公司”数据获取“员工”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetEmpLeave(int storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("StoreID", storeId));
            condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));
            if (keyword != null && keyword != string.Empty)
            {
                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                condition.Add(_condition);
            }
            //查询记录总数
            totalCount = service.Count(condition);
            //查询分页数据
            List<EmployeeBaseView> empBasees = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

            DataView awardes = ServiceFactory.Factory.EmployeeLeaveService.GetAll().DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < empBasees.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = empBasees[i].ID.ToString().Trim();
                obj["name"] = empBasees[i].EmployeeName.ToString().Trim();
                array.Add(obj);

                this.GetLeave(ref array, ref awardes, empBasees[i].ID.ToString().Trim());
            }
            return array;
        }
        /// <summary>
        /// 根据“员工”数据获取“关系人”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        private void GetLeave(ref JArray _json_array, ref DataView _dv, string _empId)
        {
            _dv.RowFilter = "EmployeeID=" + _empId;
            _dv.Sort = "CreateTime";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["name"] = "";
                obj["requestdate"] = _dv[i]["RequestDate"].ToString().Trim();
                obj["leavedate"] = _dv[i]["LeaveDate"].ToString().Trim();
                obj["levelstatus"] = DAL.Util.GetDisplayName((Enum_LeavelStatus)_dv[i]["LevelStatus"]); ;
                obj["levelreason"] = _dv[i]["LevelReason"].ToString().Trim();
                obj["submitstate"] = Convert.ToInt16(_dv[i]["State"]) == Convert.ToInt16(Enum_LevelState.Ready) ? true : false;
                obj["statename"] = DAL.Util.GetDisplayName((Enum_LevelState)_dv[i]["State"]);
                obj["description"] = _dv[i]["Description"].ToString().Trim();
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["_parentId"] = _empId;
                _json_array.Add(obj);
            }
            _dv.RowFilter = string.Empty;
            _dv.Sort = "CreateTime";
        }

        /// <summary>
        /// 根据“公司”数据获取“员工”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetEmpAward(int storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("storeId", storeId));
            condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));
            if (keyword != null && keyword != string.Empty)
            {
                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                condition.Add(_condition);
            }
            //查询记录总数
            totalCount = service.Count(condition);
            //查询分页数据
            List<EmployeeBaseView> empBasees = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

            DataView awardes = ServiceFactory.Factory.EmployeeAwardService.GetAll().DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < empBasees.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = empBasees[i].ID.ToString().Trim();
                obj["name"] = empBasees[i].EmployeeName.ToString().Trim();
                array.Add(obj);

                this.GetAward(ref array, ref awardes, empBasees[i].ID.ToString().Trim());
            }
            return array;
        }
        /// <summary>
        /// 根据“员工”数据获取“奖惩信息”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        private void GetAward(ref JArray _json_array, ref DataView _dv, string _empId)
        {
            _dv.RowFilter = "EmployeeID=" + _empId;
            _dv.Sort = "CreateTime";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["name"] = "";
                obj["awarddate"] = _dv[i]["AwardDate"].ToString().Trim();
                obj["awardtype"] = DAL.Util.GetDisplayName((Enum_AwardType)_dv[i]["AwardType"]);
                obj["awardamount"] = _dv[i]["AwardAmount"].ToString().Trim();
                obj["reason"] = _dv[i]["Reason"].ToString().Trim();
                obj["decision"] = _dv[i]["Decision"].ToString().Trim();
                obj["description"] = _dv[i]["Description"].ToString().Trim();
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["_parentId"] = _empId;
                _json_array.Add(obj);
            }
            _dv.RowFilter = string.Empty;
            _dv.Sort = "CreateTime";
        }

        /// <summary>
        /// 根据“公司”数据获取“员工”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetEmpRalation(int groupId, int storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));
            if (keyword != null && keyword != string.Empty)
            {
                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                condition.Add(_condition);
            }
            //查询记录总数
            totalCount = service.Count(condition);
            //查询分页数据
            List<EmployeeBaseView> empBasees = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

            DataView ralationes = ServiceFactory.Factory.EmployeeRalationService.GetAll().DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < empBasees.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = empBasees[i].ID.ToString().Trim();
                obj["name"] = empBasees[i].EmployeeName.ToString().Trim();
                array.Add(obj);

                this.GetRalation(ref array, ref ralationes, empBasees[i].ID.ToString().Trim());
            }
            return array;
        }
        /// <summary>
        /// 根据“员工”数据获取“关系人”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        private void GetRalation(ref JArray _json_array, ref DataView _dv, string _empId)
        {
            _dv.RowFilter = "EmployeeID=" + _empId;
            _dv.Sort = "CreateTime";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["name"] = _dv[i]["RelationName"].ToString().Trim();
                obj["relationtype"] = _dv[i]["RelationType"].ToString().Trim();
                obj["relationsex"] = DAL.Util.GetDisplayName((Enum_Sex)_dv[i]["RelationSex"]);
                obj["relationphone"] = _dv[i]["RelationPhone"].ToString().Trim();
                obj["relationworkcompany"] = _dv[i]["RelationWorkCompany"].ToString().Trim();
                obj["description"] = _dv[i]["Description"].ToString().Trim();
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["_parentId"] = _empId;
                _json_array.Add(obj);
            }
            _dv.RowFilter = string.Empty;
            _dv.Sort = "CreateTime";
        }




        /// <summary>
        /// 根据“公司”数据获取“员工”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public JArray GetEmpContract(int groupId, int storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            IEmployeeBaseService service = ServiceFactory.Factory.EmployeeBaseService;
            ConditionSet condition = new ConditionSet();
            condition.Add(new SimpleCondition("GroupID", groupId));
            condition.Add(new SimpleCondition("StoreID", storeId));
            condition.Add(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));
            if (keyword != null && keyword != string.Empty)
            {
                ConditionSet _condition = new ConditionSet(ConditionJoinType.Or);

                _condition.Add(new SimpleCondition("EmployeeName", ConditionOperator.Like, "%" + keyword + "%"));
                condition.Add(_condition);
            }
            //查询记录总数
            totalCount = service.Count(condition);
            //查询分页数据
            List<EmployeeBaseView> empBasees = service.SearchSection(condition, ((int)pageNumber - 1) * (int)pageSize, (int)pageSize, "ID", System.ComponentModel.ListSortDirection.Descending);

            DataView contracts = ServiceFactory.Factory.EmployeeContractService.GetAll().DefaultView;

            JArray array = new JArray();
            for (int i = 0; i < empBasees.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = empBasees[i].ID.ToString().Trim();
                obj["name"] = empBasees[i].EmployeeName.ToString().Trim();
                array.Add(obj);

                this.GetContract(ref array, ref contracts, empBasees[i].ID.ToString().Trim());
            }
            return array;
        }


        /// <summary>
        /// 根据“员工”数据获取“关系人”TreeGrid格式的Json数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        private void GetContract(ref JArray _json_array, ref DataView _dv, string _empId)
        {
            _dv.RowFilter = "EmployeeID=" + _empId;
            _dv.Sort = "CreateTime";
            for (int i = 0; i < _dv.Count; i++)
            {
                JObject obj = new JObject();
                obj["id"] = _dv[i]["ID"].ToString().Trim();
                obj["name"] = "";
                obj["contractno"] = _dv[i]["ContractNo"].ToString().Trim();
                obj["contracttype"] = DAL.Util.GetDisplayName((Enum_ContractType)_dv[i]["ContractType"]);
                obj["signdate"] = Convert.ToDateTime(_dv[i]["SignDate"]).ToString("yyyy-MM-dd");
                obj["expireddate"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd");
                obj["description"] = _dv[i]["Description"].ToString().Trim();
                obj["updatetime"] = Convert.ToDateTime(_dv[i]["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj["_parentId"] = _empId;
                _json_array.Add(obj);
            }
            _dv.RowFilter = string.Empty;
            _dv.Sort = "CreateTime";
        }

    }
}
