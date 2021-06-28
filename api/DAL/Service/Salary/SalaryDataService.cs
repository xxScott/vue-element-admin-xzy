using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{


    public interface ISalaryDataService : IEntityService<SalaryData>, IEntityViewService<SalaryData>, IEntityService, IEntityViewService
    {
    }

    public class SalaryDataService : ServiceBase<SalaryData, SalaryData>, ISalaryDataService
    {
    }
}
