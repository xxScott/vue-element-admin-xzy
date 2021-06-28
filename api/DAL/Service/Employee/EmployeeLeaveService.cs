using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{

    public interface IEmployeeLeaveService : IEntityService<EmployeeLeave>, IEntityViewService<EmployeeLeaveView>, IEntityService, IEntityViewService
    {
        DataTable GetAll();
    }

    public class EmployeeLeaveService : ServiceBase<EmployeeLeave, EmployeeLeaveView>, IEmployeeLeaveService
    {
        public DataTable GetAll()
        {
            try
            {
                return ((EmployeeLeaveViewDAO)ObjectViewDAO).GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
