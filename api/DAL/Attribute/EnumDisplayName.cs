using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDisplayNameAttribute : Attribute
    {
        public EnumDisplayNameAttribute(string name) { DisplayName = name; }

        public virtual string DisplayName { get; private set; }
    }
}
