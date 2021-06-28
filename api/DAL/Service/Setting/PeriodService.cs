using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{



    public interface IPeriodService : IEntityService<Periods>, IEntityViewService<Periods>, IEntityService, IEntityViewService
    {
        DataTable Query(int companyId);
    }

    public class PeriodService : ServiceBase<Periods, Periods>, IPeriodService
    {
        public DataTable Query(int companyId)
        {
            try
            {
                return ((PeriodViewDAO)ObjectViewDAO).Query(companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
