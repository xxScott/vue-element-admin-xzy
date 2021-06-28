
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
    public interface IClassesService : IEntityService<Classes>, IEntityViewService<ClassesView>, IEntityService, IEntityViewService
    {

    }

    public class ClassesService : ServiceBase<Classes, ClassesView>, IClassesService
    {

    }
}