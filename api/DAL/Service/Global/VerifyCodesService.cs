
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
    public interface IVerifyCodesService : IEntityService<VerifyCodes>, IEntityViewService<VerifyCodes>, IEntityService, IEntityViewService
    {

    }

    public class VerifyCodesService : ServiceBase<VerifyCodes, VerifyCodes>, IVerifyCodesService
    {
    }
}