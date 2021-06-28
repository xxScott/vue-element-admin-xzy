using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CaiMoMo
{
    public class SysGroupInfo
    {
        public int UID { get; set; }
        public string GroupName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string RegionID { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string ContactName { get; set; }
        public string Telephone { get; set; }
        public string BankType { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccount { get; set; }
        public string IsSingleStore { get; set; }
        public string AddTime { get; set; }
        public string AddUser { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateUser { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string MchId { get; set; }
        public string AboutUs { get; set; }
        public string WXId { get; set; }
        public string WelImageUrl { get; set; }
    }
}
