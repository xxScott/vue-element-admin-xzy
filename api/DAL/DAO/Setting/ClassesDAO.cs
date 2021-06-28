using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class ClassesDAO : CObjectDAO<Classes> { }

    public partial class ClassesViewDAO : CObjectViewDAO<ClassesView>
    {
       
    }
}