using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CaiMoMo
{
    public class SysGroupUser
    {
        public int? UID { get; set; }
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
