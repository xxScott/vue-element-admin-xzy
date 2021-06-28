using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Com.Caimomo.Entity
{
    /// <summary>
    /// TC_AccountManagement 的摘要说明
    /// </summary>
    public partial class TC_AccountManagement
    {
        private List<TC_UserAuths> lstTC_UserAuths;

        public List<TC_UserAuths> LstTC_UserAuths
        {
            get
            {
                return lstTC_UserAuths;
            }

            set
            {
                lstTC_UserAuths = value;
            }
        }
    }
}