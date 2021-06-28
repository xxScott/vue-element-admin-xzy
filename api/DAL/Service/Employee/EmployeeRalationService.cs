using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{
    public interface IEmployeeRalationService : IEntityService<EmployeeRalation>, IEntityViewService<EmployeeRalationView>, IEntityService, IEntityViewService
    {
        DataTable GetAll();
    }

    public class EmployeeRalationService : ServiceBase<EmployeeRalation, EmployeeRalationView>, IEmployeeRalationService
    {
        public DataTable GetAll()
        {
            try
            {
                return ((EmployeeRalationViewDAO)ObjectViewDAO).GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
