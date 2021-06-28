
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DAL.Business;
using System.Data;
using MyOrm.Common;

namespace DAL
{
    public interface IWorkStationsService : IEntityService<WorkStations>, IEntityViewService<WorkStations>, IEntityService, IEntityViewService
    {

    }

    public class WorkStationsService : ServiceBase<WorkStations, WorkStations>, IWorkStationsService
    {

    }
}