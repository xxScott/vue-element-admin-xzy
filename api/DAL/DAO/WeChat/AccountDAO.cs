using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class AccountDAO : CObjectDAO<Account> { }

    public partial class AccountViewDAO : CObjectViewDAO<Account>
    {
        
    }
}