using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{
    public interface IMakeClassLogService : IEntityService<MakeClassLog>, IEntityViewService<MakeClassLog>, IEntityService, IEntityViewService
    {
    }

    public class MakeClassLogService : ServiceBase<MakeClassLog, MakeClassLog>, IMakeClassLogService
    {
    }
}
