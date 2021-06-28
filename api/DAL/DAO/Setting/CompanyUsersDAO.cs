using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class CompanyUsersDAO : CObjectDAO<CompanyUsers> { }

    public partial class CompanyUsersViewDAO : CObjectViewDAO<CompanyUsers>
    {
        
    }
}