using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL
{
    public partial class UsersDAO : CObjectDAO<Users> { }

    public partial class UsersViewDAO : CObjectViewDAO<Users> { }
}