using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{

    public partial class EmployeeBaseDAO : CObjectDAO<EmployeeBase> { }

    public partial class EmployeeBaseViewDAO : CObjectViewDAO<EmployeeBaseView>
    {
    }
}
