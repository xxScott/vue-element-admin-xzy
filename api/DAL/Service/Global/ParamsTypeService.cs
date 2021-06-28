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
    public interface IParamsTypeService : IEntityService<ParamsType>, IEntityViewService<ParamsType>, IEntityService, IEntityViewService
    {
    }

    public class ParamsTypeService : ServiceBase<ParamsType, ParamsType>, IParamsTypeService
    {
    }
}