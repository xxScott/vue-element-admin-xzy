
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
    public interface ICompanyUsersService : IEntityService<CompanyUsers>, IEntityViewService<CompanyUsers>, IEntityService, IEntityViewService
    {

    }

    public class CompanyUsersService : ServiceBase<CompanyUsers, CompanyUsers>, ICompanyUsersService
    {

    }
}