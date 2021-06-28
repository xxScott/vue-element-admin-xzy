using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleService
{
    public class ConfigSetting
    {
        public static int Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"].ToString().Trim());
        public static int ErrorInterval = Convert.ToInt32(ConfigurationManager.AppSettings["ErrorInterval"].ToString().Trim());


    }
}
