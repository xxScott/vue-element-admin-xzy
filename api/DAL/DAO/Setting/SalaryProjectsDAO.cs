using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class SalaryProjectsDAO : CObjectDAO<SalaryProjects> { }

    public partial class SalaryProjectsViewDAO : CObjectViewDAO<SalaryProjects>
    {
        
    }
}