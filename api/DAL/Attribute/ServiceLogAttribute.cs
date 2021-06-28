using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class ServiceLogAttribute : Attribute
    {
        public ServiceLogAttribute() { LogLevel = LogLevel.Info; LogFormat = LogFormat.Full; }

        public LogLevel LogLevel { get; set; }
        public LogFormat LogFormat { get; set; }
    }

    public enum LogLevel
    {
        Off,
        Debug,
        Info
    }

    [Flags]
    public enum LogFormat
    {
        None = 0,
        Args = 1,
        ReturnValue = 2,
        Full = Args | ReturnValue
    }
}
