﻿using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class RolesDAO : CObjectDAO<Roles> { }

    public partial class RolesViewDAO : CObjectViewDAO<Roles> {
        
    }
}