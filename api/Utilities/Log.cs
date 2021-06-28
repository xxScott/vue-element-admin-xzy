using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Utilities
{
    public class Log
    {
        public readonly ILog logger =null;

        public Log()
        {
            logger = LogManager.GetLogger("ServiceInterceptor"); 
        }

        public Log(string name)
        {
            logger = LogManager.GetLogger(name);
        }
         

        public void Info(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " " + message);
            logger.Info(message);
        }

        public void Debug(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")+" "+message);
            logger.Debug(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " " + message);
            logger.Debug(message);
        }

        public void Error(Exception ex)
        {
            StackTrace ss = new StackTrace(true);
            Type t = ss.GetFrame(1).GetMethod().DeclaringType;
            int lineNo = ss.GetFrame(1).GetFileLineNumber();
            String sName = ss.GetFrame(1).GetMethod().Name;
            string message = DateTime.Now.ToString() + "typeName:" + t.FullName + " lineNo:" + lineNo + " sName: " + sName + "\r\n";
            message += "异常信息：(" + ex.Message + ")\r\n";
            message += "异常方法：(" + ex.TargetSite + ")\r\n";
            message += "异常来源：(" + ex.Source + ")\r\n";
            message += "异常处理：\r\n" + ex.StackTrace + "\r\n";
            message += "异常实例：\r\n" + ex.InnerException + "\r\n";
            message += "//**********************************************************************************************************************" + "\r\n";

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + " " + message);
            logger.Error(message);
        }
    }
}
