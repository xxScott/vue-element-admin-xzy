using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{

    public interface IMakeClassPlanService : IEntityService<MakeClassPlan>, IEntityViewService<MakeClassPlanView>, IEntityService, IEntityViewService
    {
        DataTable Query(int makeId);
    }

    public class MakeClassPlanService : ServiceBase<MakeClassPlan, MakeClassPlanView>, IMakeClassPlanService
    {
        public DataTable Query(int makeId)
        {
            try
            {
                return ((MakeClassPlanViewDAO)ObjectViewDAO).Query(makeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
