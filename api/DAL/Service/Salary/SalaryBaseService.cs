using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{
    public interface ISalaryBaseService : IEntityService<SalaryBase>, IEntityViewService<SalaryBase>, IEntityService, IEntityViewService
    {
        DataTable QueryEmpSalary(int empId);
    }

    public class SalaryBaseService : ServiceBase<SalaryBase, SalaryBase>, ISalaryBaseService
    {
        public DataTable QueryEmpSalary(int empId)
        {
            try
            {
                return ((SalaryBaseViewDAO)ObjectViewDAO).Query(empId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
