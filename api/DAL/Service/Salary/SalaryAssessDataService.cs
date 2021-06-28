using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{


    public interface ISalaryAssessDataService : IEntityService<SalaryAssessData>, IEntityViewService<SalaryAssessData>, IEntityService, IEntityViewService
    {
    }

    public class SalaryAssessDataService : ServiceBase<SalaryAssessData, SalaryAssessData>, ISalaryAssessDataService
    {
    }
}
