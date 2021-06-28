
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
    public interface IFormationsService : IEntityService<Formations>, IEntityViewService<Formations>, IEntityService, IEntityViewService
    {

    }

    public class FormationsService : ServiceBase<Formations, Formations>, IFormationsService
    {

    }
}