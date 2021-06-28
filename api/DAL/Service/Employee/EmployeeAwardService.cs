using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;


namespace DAL
{

    public interface IEmployeeAwardService : IEntityService<EmployeeAward>, IEntityViewService<EmployeeAwardView>, IEntityService, IEntityViewService
    {
        DataTable GetAll();
    }

    public class EmployeeAwardService : ServiceBase<EmployeeAward, EmployeeAwardView>, IEmployeeAwardService
    {
        public DataTable GetAll()
        {
            try
            {
                return ((EmployeeAwardViewDAO)ObjectViewDAO).GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
