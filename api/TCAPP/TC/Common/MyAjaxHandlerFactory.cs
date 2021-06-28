using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Caimomo.Common
{
    public class MyAjaxHandlerFactory
    {
        public static IAjaxHandler GetMyHandler(string methodName)
        {
            string typeName = "Com.Caimomo.Handlers." + methodName;
            return Activator.CreateInstance(Type.GetType(typeName)) as IAjaxHandler;
        }
    }
}