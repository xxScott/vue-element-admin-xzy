
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DAL.Business;
using MyOrm.Common;

namespace DAL
{
    public interface IUserInRoleService : IEntityService<UserInRole>, IEntityViewService<UserInRole>, IEntityService, IEntityViewService
    {
    }

    public class UserInRoleService : ServiceBase<UserInRole, UserInRole>, IUserInRoleService
    {
    }
}