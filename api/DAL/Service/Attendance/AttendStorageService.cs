using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{


    public interface IAttendStorageService : IEntityService<AttendStorage>, IEntityViewService<AttendStorage>, IEntityService, IEntityViewService
    {
    }

    public class AttendStorageService : ServiceBase<AttendStorage, AttendStorage>, IAttendStorageService
    {
    }
}
