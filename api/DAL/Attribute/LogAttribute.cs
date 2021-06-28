using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = true)]
    public class LogAttribute : Attribute
    {
        public LogAttribute() { Enabled = true; }

        public LogAttribute(bool enabled) { Enabled = enabled; }

        public bool Enabled { get; set; }
    }
}
