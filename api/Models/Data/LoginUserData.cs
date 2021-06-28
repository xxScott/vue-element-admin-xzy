using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class LoginUserData
    {
        public int? UID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string LoginCode { get; set; }
    }
}
