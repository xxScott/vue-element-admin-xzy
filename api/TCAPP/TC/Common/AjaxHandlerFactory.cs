using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Caimomo.Common
{
    public class AjaxHandlerFactory
    {
        public static IAjaxHandler GetHandler(string methodName)
        {
            string typeName = "HrApp.TC.Handlers." + methodName;
            return Activator.CreateInstance(Type.GetType(typeName)) as IAjaxHandler;
        }
    }
}