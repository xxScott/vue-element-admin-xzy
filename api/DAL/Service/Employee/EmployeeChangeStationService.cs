using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{

    public interface IEmployeeChangeStationService : IEntityService<EmployeeChangeStation>, IEntityViewService<EmployeeChangeStationView>, IEntityService, IEntityViewService
    {
        DataTable Query(int state, string groupId, string storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount);
    }

    public class EmployeeChangeStationService : ServiceBase<EmployeeChangeStation, EmployeeChangeStationView>, IEmployeeChangeStationService
    {
        public DataTable Query(int state, string groupId, string storeId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            try
            {
                return ((EmployeeChangeStationViewDAO)ObjectViewDAO).Query(state, groupId,storeId, keyword, pageNumber, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
