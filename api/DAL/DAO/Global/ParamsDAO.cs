using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class ParamsDAO : CObjectDAO<Params> { }

    public partial class ParamsViewDAO : CObjectViewDAO<ParamsView>
    {
    }
}