using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{

    public interface IStoresService : IEntityService<Stores>, IEntityViewService<Stores>, IEntityService, IEntityViewService
    {
        DataTable Query();
    }

    public class StoresService : ServiceBase<Stores, Stores>, IStoresService
    {
        public DataTable Query()
        {
            try
            {
                return ((StoresViewDAO)ObjectViewDAO).Query();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
