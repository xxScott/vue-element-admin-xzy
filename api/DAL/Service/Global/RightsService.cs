
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
    public interface IRightsService : IEntityService<Rights>, IEntityViewService<Rights>, IEntityService, IEntityViewService
    {
        DataTable Search(int userId, int moduleId);

        DataTable Search(int roleId);

        [Transaction]
        bool Update(List<Rights> list, string roleId);
    }

    public class RightsService : ServiceBase<Rights, Rights>, IRightsService
    {
        public DataTable Search(int userId, int moduleId)
        {
            try
            {
                return ((RightsViewDAO)ObjectViewDAO).GetResult(userId, moduleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int roleId)
        {
            try
            {
                return ((RightsViewDAO)ObjectViewDAO).GetResult(roleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<Rights> list, string roleId)
        {
            try
            {
                List<Rights> _list = Search(new SimpleCondition("RoleID", roleId));
                Factory.RightsService.BatchDelete(_list);
                Factory.RightsService.BatchInsert(list);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}