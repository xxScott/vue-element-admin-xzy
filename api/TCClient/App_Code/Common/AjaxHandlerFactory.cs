using System;

namespace Com.Caimomo.Common
{
    public class AjaxHandlerFactory
    {
        public static IAjaxHandler GetHandler(string methodName)
        {
            string typeName = "HrApp.TCClient.Handlers." + methodName;
            return Activator.CreateInstance(Type.GetType(typeName)) as IAjaxHandler;
        }
    }
}