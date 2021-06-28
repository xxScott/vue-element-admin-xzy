using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{
    public interface IEmployeeBaseService : IEntityService<EmployeeBase>, IEntityViewService<EmployeeBaseView>, IEntityService, IEntityViewService
    {
        [Transaction]
        bool SaveEmpBase(EmployeeBase emp, List<SalaryBase> salarys);
        [Transaction]
        bool UpdateEmpBase(EmployeeBase emp, List<SalaryBase> salarys);
    }

    public class EmployeeBaseService : ServiceBase<EmployeeBase, EmployeeBaseView>, IEmployeeBaseService
    {
        public bool SaveEmpBase(EmployeeBase emp, List<SalaryBase> salarys)
        {
            try
            {
                Factory.EmployeeBaseService.Insert(emp);

                for (int i = 0; i < salarys.Count; i++)
                {
                    salarys[i].EmployeeID = emp.ID;
                }
                Factory.SalaryBaseService.BatchInsert(salarys);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateEmpBase(EmployeeBase emp, List<SalaryBase> salarys)
        {
            try
            {
                Factory.EmployeeBaseService.Update(emp);


                Factory.SalaryBaseService.BatchUpdateOrInsert(salarys);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
