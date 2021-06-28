
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
    public interface IRolesService : IEntityService<Roles>, IEntityViewService<Roles>, IEntityService, IEntityViewService
    {
        [Transaction]
        bool DeleteAll(object id);
    }

    public class RolesService : ServiceBase<Roles, Roles>, IRolesService
    {
        public bool DeleteAll(object id)
        {
            try
            {
                Factory.RolesService.DeleteID(id);
                List<UserInRole> users = Factory.UserInRoleService.Search(new SimpleCondition("RoleID", id));
                Factory.UserInRoleService.BatchDelete(users);

                List<Rights> rights = Factory.RightsService.Search(new SimpleCondition("RoleID", id));
                Factory.RightsService.BatchDelete(users);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}