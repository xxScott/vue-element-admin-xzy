using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class WorkStationsDAO : CObjectDAO<WorkStations> { }

    public partial class WorkStationsViewDAO : CObjectViewDAO<WorkStations>
    {
        
    }
}