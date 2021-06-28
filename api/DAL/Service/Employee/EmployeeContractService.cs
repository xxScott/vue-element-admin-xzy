using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{
    public interface IEmployeeContractService : IEntityService<EmployeeContract>, IEntityViewService<EmployeeContractView>, IEntityService, IEntityViewService
    {
        DataTable GetAll();
    }

    public class EmployeeContractService : ServiceBase<EmployeeContract, EmployeeContractView>, IEmployeeContractService
    {
        public DataTable GetAll()
        {
            try
            {
                return ((EmployeeContractViewDAO)ObjectViewDAO).GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
