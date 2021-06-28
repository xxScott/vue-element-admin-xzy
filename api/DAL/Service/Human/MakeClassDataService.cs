using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;
using MyOrm.Common;

namespace DAL
{


    public interface IMakeClassDataService : IEntityService<MakeClassData>, IEntityViewService<MakeClassData>, IEntityService, IEntityViewService
    {
        DataTable Query(int storeId, int periodId);

        DataTable QueryAll(string keyword);
        DataTable QueryData(int companyId, int periodId, int deptId);
        DataTable QueryData(int storeId, int periodId);
        [Transaction]
        bool SaveMakeClass(MakeClassPlan makePlan, MakeClassData makeData);

        [Transaction]
        bool SaveMakeClassData(List<MakeClassData> list);
        [Transaction]
        bool SaveMakeClassData(List<MakeClassData> list, MakeClassPlan plan);
    }





    public class MakeClassDataService : ServiceBase<MakeClassData, MakeClassData>, IMakeClassDataService
    {
        public DataTable Query(int storeId, int periodId)
        {
            try
            {
                return ((MakeClassDataViewDAO)ObjectViewDAO).Query(storeId, periodId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable QueryAll(string keyword)
        {
            try
            {
                return ((MakeClassDataViewDAO)ObjectViewDAO).QueryAll(keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable QueryData(int companyId, int periodId, int deptId)
        {
            try
            {
                return ((MakeClassDataViewDAO)ObjectViewDAO).QueryData(companyId, periodId, deptId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable QueryData(int storeId, int periodId)
        {
            try
            {
                return ((MakeClassDataViewDAO)ObjectViewDAO).QueryData(storeId, periodId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveMakeClass(MakeClassPlan makePlan, MakeClassData makeData)
        {
            try
            {
                Factory.MakeClassPlanService.Insert(makePlan);

                makeData.MakeClassID = makePlan.ID;
                Factory.MakeClassDataService.Insert(makeData);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveMakeClassData(List<MakeClassData> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    ConditionSet condition = new ConditionSet();
                    condition.Add(new SimpleCondition("MakeClassID", list[0].MakeClassID));
                    condition.Add(new SimpleCondition("EmployeeID", list[0].EmployeeID));
                    //查询数据
                    List<MakeClassData> old_list = Factory.MakeClassDataService.Search(condition);
                    Factory.MakeClassDataService.BatchDelete(old_list);



                    Factory.MakeClassDataService.Insert(list[0]);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (i != 0)
                    {
                        list[i].UpperID = list[0].ID;
                        Factory.MakeClassDataService.Insert(list[i]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SaveMakeClassData(List<MakeClassData> list, MakeClassPlan plan)
        {
            try
            {

                Factory.MakeClassPlanService.Insert(plan);

                if (list.Count > 0)
                {
                    ConditionSet condition = new ConditionSet();
                    condition.Add(new SimpleCondition("MakeClassID", list[0].MakeClassID));
                    condition.Add(new SimpleCondition("EmployeeID", list[0].EmployeeID));
                    //查询数据
                    List<MakeClassData> old_list = Factory.MakeClassDataService.Search(condition);
                    Factory.MakeClassDataService.BatchDelete(old_list);

                    list[0].MakeClassID = plan.ID;
                    Factory.MakeClassDataService.Insert(list[0]);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (i != 0)
                    {
                        list[i].MakeClassID = plan.ID;
                        list[i].UpperID = list[0].ID;
                        Factory.MakeClassDataService.Insert(list[i]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
