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
    public interface IParamsService : IEntityService<Params>, IEntityViewService<ParamsView>, IEntityService, IEntityViewService
    {
        List<ParamsView> GetParam(string paramTypeCode);
    }

    public class ParamsService : ServiceBase<Params, ParamsView>, IParamsService
    {
        public List<ParamsView> GetParam(string paramTypeCode)
        {
            try
            {
                Sorting s = new Sorting();
                s.PropertyName = "ParamValue";
                s.Direction = System.ComponentModel.ListSortDirection.Ascending;
                List<ParamsView> list = Factory.ParamsService.SearchWithOrder(new SimpleCondition("ParamTypeCode", paramTypeCode), new Sorting[] { s });
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}