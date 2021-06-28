using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{


    public interface ISalaryChangeService : IEntityService<SalaryChange>, IEntityViewService<SalaryChange>, IEntityService, IEntityViewService
    {
        DataTable QueryIncrease(int empId, int property);

        DataTable QueryChange(int empId);

        DataTable QueryEvaluation(int empId, int property, int periodId);

        DataTable Query(int groupId, int storeId);
    }

    public class SalaryChangeService : ServiceBase<SalaryChange, SalaryChange>, ISalaryChangeService
    {

        public DataTable QueryIncrease(int empId, int property)
        {
            try
            {
                return ((SalaryChangeViewDAO)ObjectViewDAO).QueryIncrease(empId, property);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable QueryChange(int empId)
        {
            try
            {
                return ((SalaryChangeViewDAO)ObjectViewDAO).QueryChange(empId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable QueryEvaluation(int empId, int property, int periodId)
        {
            try
            {
                return ((SalaryChangeViewDAO)ObjectViewDAO).QueryEvaluation(empId, property, periodId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Query(int groupId, int storeId)
        {
            try
            {
                return ((SalaryChangeViewDAO)ObjectViewDAO).Query(groupId, storeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
