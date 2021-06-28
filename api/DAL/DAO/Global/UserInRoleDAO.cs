using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL
{
    public partial class UserInRoleDAO : CObjectDAO<UserInRole> { }

    public partial class UserInRoleViewDAO : CObjectViewDAO<UserInRole> { }
}