using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{

    public interface ISalarySettleService : IEntityService<SalarySettle>, IEntityViewService<SalarySettle>, IEntityService, IEntityViewService
    {
        DataTable Query(int companyId, int state);
    }

    public class SalarySettleService : ServiceBase<SalarySettle, SalarySettle>, ISalarySettleService
    {
        public DataTable Query(int companyId, int state)
        {
            try
            {
                return ((SalarySettleViewDAO)ObjectViewDAO).Query(companyId, state);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
