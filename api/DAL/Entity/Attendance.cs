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
    [Table("Attend_BarCode")]
    [Description("签到随机码")]
    public class BarCode : EntityBase
    {
        public int? StoreID { get; set; }
        public int? DeptID { get; set; }
        public int? StartOrOver { get; set; }
        public string RandomCode { get; set; }
        public string BarCodeData { get; set; }
    }

    [Serializable]
    [Table("Attend_Param")]
    [Description("签到参数表")]
    public class AttendParam : BusinessEntity
    {
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public int? ParamCode { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public bool Enabled { get; set; }
    }


    [Serializable]
    [Table("Attend_Storage")]
    [Description("签到数据设置表")]
    public class AttendStorage : BusinessEntity
    {
        public int? CompanyID { get; set; }
        public int? StorageMode { get; set; }
        public int? StorageType { get; set; }
        public string Src { get; set; }
        public string DataSource { get; set; }
        public string CatalogName { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
    }



    [Serializable]
    [Table("Attend_Period_Data")]
    [Description("期间签到数据表")]
    public class AttendPeriodData : EntityBase
    {
        public int? PeroidID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EmployeeID { get; set; }
        public int? StoreID { get; set; }
        public int? DeptID { get; set; }
        public int State { get; set; }
        public string EmployeeName { get; set; }

        public int Data1 { get; set; }
        public int Data2 { get; set; }
        public int Data3 { get; set; }
        public int Data4 { get; set; }
        public int Data5 { get; set; }

        public int Data6 { get; set; }
        public int Data7 { get; set; }
        public int Data8 { get; set; }
        public int Data9 { get; set; }
        public int Data10 { get; set; }

        public int Data11 { get; set; }
        public int Data12 { get; set; }
        public int Data13 { get; set; }
        public int Data14 { get; set; }
        public int Data15 { get; set; }

        public int Data16 { get; set; }
        public int Data17 { get; set; }
        public int Data18 { get; set; }
        public int Data19 { get; set; }
        public int Data20 { get; set; }

        public int Data21 { get; set; }
        public int Data22 { get; set; }
        public int Data23 { get; set; }
        public int Data24 { get; set; }
        public int Data25 { get; set; }

        public int Data26 { get; set; }
        public int Data27 { get; set; }
        public int Data28 { get; set; }
        public int Data29 { get; set; }
        public int Data30 { get; set; }
        public int Data31 { get; set; }


    }
}
