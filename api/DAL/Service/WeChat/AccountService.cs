
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
    public interface IAccountService : IEntityService<Account>, IEntityViewService<Account>, IEntityService, IEntityViewService
    {
    }

    public class AccountService : ServiceBase<Account, Account>, IAccountService
    {
    }
}