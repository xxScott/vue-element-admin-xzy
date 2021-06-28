using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;


namespace DAL
{
    public interface IBarCodeService : IEntityService<BarCode>, IEntityViewService<BarCode>, IEntityService, IEntityViewService
    {
    }

    public class BarCodeService : ServiceBase<BarCode, BarCode>, IBarCodeService
    {
    }
}
