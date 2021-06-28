using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
namespace ScheduleService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        Log log = new Log();
        protected override void OnStart(string[] args)
        {
            DAL.Business.ServiceFactory.RegisterServiceFactory();

            List<Thread> list = new List<Thread>();

            list.Add(new Thread(new CreateDB().Create));
            list.Add(new Thread(new CreateDB().CreateDMD));
            list.Add(new Thread(new CreateDB().Data));
            list.Add(new Thread(new CreateDB().DataDMD));

            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i].Start();
                    Thread.Sleep(ConfigSetting.Interval);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
