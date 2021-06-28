
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
    public interface ISalaryProjectsService : IEntityService<SalaryProjects>, IEntityViewService<SalaryProjects>, IEntityService, IEntityViewService
    {

    }

    public class SalaryProjectsService : ServiceBase<SalaryProjects, SalaryProjects>, ISalaryProjectsService
    {

    }
}