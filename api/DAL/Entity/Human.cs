using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using MyOrm.Common;

namespace DAL
{
    [Serializable]
    [Table("Hum_MakeClass")]
    [Description("排班计划表")]
    public class MakeClassPlan : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        [ForeignType(typeof(Periods))]
        public int? PeriodID { get; set; }
        [ForeignType(typeof(Depts))]
        public int? DeptID { get; set; }
        [ForeignType(typeof(Users))]
        public int? UserID { get; set; }
        public string Description { get; set; }
        public int? State { get; set; }
    }

    [Serializable]
    [Description("排班计划视图")]
    public class MakeClassPlanView : MakeClassPlan
    {
        [ForeignColumn(typeof(Periods))]
        public string PeriodName { get; set; }
        [ForeignColumn(typeof(Depts))]
        public string DeptName { get; set; }
        [ForeignColumn(typeof(Users))]
        public string UserName { get; set; }
    }


    [Serializable]
    [Table("Hum_MakeClass_Data")]
    [Description("排班计划表")]
    public class MakeClassData : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public int? MakeClassID { get; set; }
        public int? EmployeeID { get; set; }
        public int? UpperID { get; set; }
        public int ClassID1 { get; set; }
        public int ClassID2 { get; set; }
        public int ClassID3 { get; set; }
        public int ClassID4 { get; set; }
        public int ClassID5 { get; set; }
        public int ClassID6 { get; set; }
        public int ClassID7 { get; set; }
        public int ClassID8 { get; set; }
        public int ClassID9 { get; set; }
        public int ClassID10 { get; set; }

        public int ClassID11 { get; set; }
        public int ClassID12 { get; set; }
        public int ClassID13 { get; set; }
        public int ClassID14 { get; set; }
        public int ClassID15 { get; set; }
        public int ClassID16 { get; set; }
        public int ClassID17 { get; set; }
        public int ClassID18 { get; set; }
        public int ClassID19 { get; set; }
        public int ClassID20 { get; set; }

        public int ClassID21 { get; set; }
        public int ClassID22 { get; set; }
        public int ClassID23 { get; set; }
        public int ClassID24 { get; set; }
        public int ClassID25 { get; set; }
        public int ClassID26 { get; set; }
        public int ClassID27 { get; set; }
        public int ClassID28 { get; set; }
        public int ClassID29 { get; set; }
        public int ClassID30 { get; set; }
        public int ClassID31 { get; set; }

    }



    [Serializable]
    [Table("Hum_MakeClass_Log")]
    [Description("排班计划表")]
    public class MakeClassLog : BusinessEntity
    {
        public int? MakeClassID { get; set; }
        public DateTime? OccurTime { get; set; }
        public int? SourceState { get; set; }
        public int? TargetState { get; set; }
        public int? UserID { get; set; }

    }
}
