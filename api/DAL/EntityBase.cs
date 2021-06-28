using System;
using System.Collections.Generic;
using System.Text;
using MyOrm.Common;
using System.ComponentModel;
using System.Reflection;

namespace DAL.Data
{
    [Serializable]
    public abstract class EntityBase : ICopyable, ICloneable, ILogable
    {
        [Browsable(false)]
        [DisplayName("序号")]
        [Column(IsPrimaryKey = true, IsIdentity = false)]
        public int ID { get; set; }

        public virtual void CopyFrom(object target)
        {
            if (!(target is EntityBase)) return;
            Type type = this.GetType();
            if (type.IsSubclassOf(target.GetType())) type = target.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                property.SetValue(this, property.GetValue(target, null), null);
            }
        }

        [Column(false)]
        public virtual object this[string propertyName]
        {
            get
            {
                PropertyInfo property = this.GetType().GetProperty(propertyName);
                if (propertyName == null) throw new ArgumentOutOfRangeException(propertyName);
                return property.GetValue(this, null);
            }
            set
            {
                PropertyInfo property = this.GetType().GetProperty(propertyName);
                if (propertyName == null) throw new ArgumentOutOfRangeException(propertyName);
                property.SetValue(this, value, null);
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        private static Dictionary<Type, string[]> toLogProperties = new Dictionary<Type, string[]>();

        protected virtual string[] ToLogProperties()
        {
            if (!toLogProperties.ContainsKey(this.GetType()))
            {
                List<string> logProperties = new List<string>();
                foreach (PropertyInfo property in this.GetType().GetProperties())
                {
                    if (property.GetIndexParameters().Length == 0)
                    {
                        LogAttribute att = Utility.GetAttribute<LogAttribute>(property);
                        if (att == null || att.Enabled == true) { logProperties.Add(property.Name); }
                    }
                }
                toLogProperties[this.GetType()] = logProperties.ToArray();
            }
            return toLogProperties[this.GetType()];
        }

        public virtual string ToLog(object target)
        {
            string[] properties = ToLogProperties();
            StringBuilder sb = new StringBuilder();
            if (properties != null)
            {
                if (target != null && target.GetType() == this.GetType())
                    foreach (string propertyName in properties)
                    {
                        object o = this[propertyName];
                        if (!Equals(o, ((EntityBase)target)[propertyName]))
                        {
                            if (sb.Length > 0) sb.Append(",");
                            sb.AppendFormat("{0}:{1}", propertyName, o);
                        }
                    }
                else
                    foreach (string propertyName in properties)
                    {
                        string strProperty = Convert.ToString(this[propertyName]);
                        if (!String.IsNullOrEmpty(strProperty))
                        {
                            if (sb.Length > 0) sb.Append(",");
                            sb.AppendFormat("{0}:{1}", propertyName, strProperty);
                        }
                    }
            }
            return sb.ToString();
        }

        public virtual string ToLog()
        {
            return ToLog(null);
        }
    }

    [Serializable]
    [Table]
    public abstract partial class BusinessEntity : EntityBase
    {
        [Column(ColumnMode = ColumnMode.Final)]
        [DisplayName("创建时间")]
        [Log(false)]
        public DateTime? CreateTime { get; set; }

        [Browsable(false)]
        [DisplayName("创建人员")]
        [Log(false)]
        public string Creator { get; set; }

        [Browsable(false)]
        [DisplayName("创建IP")]
        [Log(false)]
        public string CreateIP { get; set; }

        [Browsable(false)]
        [DisplayName("最后更新时间")]
        [Log(false)]
        public DateTime? UpdateTime { get; set; }

        [Browsable(false)]
        [DisplayName("更新人员")]
        [Log(false)]
        public string Updater { get; set; }

        [Browsable(false)]
        [DisplayName("更新IP")]
        [Log(false)]
        public string UpdateIP { get; set; }
    }

    public interface ICopyable
    {
        void CopyFrom(object target);
    }

    public interface ILogable
    {
        string ToLog();
    }
}
