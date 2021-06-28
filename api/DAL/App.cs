using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace DAL
{
    public static class App
    {
        public readonly static WindsorContainer Container = new WindsorContainer(new XmlInterpreter("Component.xml"));
    }
}
