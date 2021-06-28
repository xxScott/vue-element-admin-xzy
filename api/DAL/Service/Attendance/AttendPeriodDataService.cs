using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{



    public interface IAttendPeriodDataService : IEntityService<AttendPeriodData>, IEntityViewService<AttendPeriodData>, IEntityService, IEntityViewService
    {
     
    }

    public class AttendPeriodDataService : ServiceBase<AttendPeriodData, AttendPeriodData>, IAttendPeriodDataService
    {
    }
}
