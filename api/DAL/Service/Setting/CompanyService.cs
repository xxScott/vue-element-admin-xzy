
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
    public interface ICompanysService : IEntityService<Companys>, IEntityViewService<Companys>, IEntityService, IEntityViewService
    {
        DataTable GetAll(object groupId, object storeId);

        DataTable Query(object groupId, object storeId, string keyword);
    }

    public class CompanyService : ServiceBase<Companys, Companys>, ICompanysService
    {
        public DataTable GetAll(object groupId, object storeId)
        {
            try
            {
                return ((CompanysViewDAO)ObjectViewDAO).GetAll(groupId, storeId);
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
                return ((CompanysViewDAO)ObjectViewDAO).Query(groupId, storeId, keyword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}