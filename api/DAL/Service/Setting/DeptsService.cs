
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
    public interface IDeptsService : IEntityService<Depts>, IEntityViewService<Depts>, IEntityService, IEntityViewService
    {
        DataTable GetAll(object groupId, object storeId);

        DataTable Query(object groupId, object storeId, string keyword);
    }

    public class DeptsService : ServiceBase<Depts, Depts>, IDeptsService
    {
        public DataTable GetAll(object groupId, object storeId)
        {
            try
            {
                return ((DeptsViewDAO)ObjectViewDAO).GetAll(groupId, storeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Query(object groupId, object storeId, string keyword)
        {
            try
            {
                return ((DeptsViewDAO)ObjectViewDAO).Query(groupId, storeId, keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}