
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
    public interface IAccessTokenService : IEntityService<AccessToken>, IEntityViewService<AccessToken>, IEntityService, IEntityViewService
    {
    }

    public class AccessTokenService : ServiceBase<AccessToken, AccessToken>, IAccessTokenService
    {
    }
}