using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MyOrm.Common;
using System.ComponentModel;
using System.Collections;
using System.Security.Cryptography;
using DAL.Data;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;
namespace DAL
{
    public static class Util
    {
        private static Dictionary<Enum, string> enumNames = new Dictionary<Enum, string>();
        private static Dictionary<Type, bool> enumInitlized = new Dictionary<Type, bool>();

        public static string GetDisplayName(Enum value)
        {
            if (value == null) return null;
            if (!enumNames.ContainsKey(value))
            {
                Type enumType = value.GetType();
                if (!enumInitlized.ContainsKey(enumType))
                {
                    foreach (FieldInfo field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        object[] atts = field.GetCustomAttributes(typeof(EnumDisplayNameAttribute), true);
                        if (atts.Length > 0)
                        {
                            EnumDisplayNameAttribute att = (EnumDisplayNameAttribute)atts[0];
                            SetDisplayName((Enum)field.GetValue(null), att.DisplayName ?? field.Name);
                        }
                        else
                            SetDisplayName((Enum)field.GetValue(null), field.Name);
                    }
                    enumInitlized[enumType] = true;
                }
                if (!enumNames.ContainsKey(value)) SetDisplayName(value, value.ToString());
            }
            return enumNames[value];
        }

        public static void SetDisplayName(Enum value, string name)
        {
            enumNames[value] = name;
        }

        public static DataTable GetAll(Type enumType)
        {
            try
            {
                //获取枚举的所有值
                Array array = System.Enum.GetValues(enumType);
                DataTable dt = new DataTable();
                dt.Columns.Add("String", Type.GetType("System.String"));
                dt.Columns.Add("Value", typeof(int));
                foreach (var obj in array)
                {
                    DataRow row = dt.NewRow();
                    row[0] = GetDisplayName((Enum)Enum.Parse(enumType, obj.ToString()));
                    row[1] = (int)obj;
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        ///  根据条件生成用于显示的文本
        /// </summary>
        /// <param name="op">条件类型</param>
        /// <param name="opposite">是否为非</param>
        /// <param name="value">用于比较的值</param>
        /// <returns></returns>
        public static string ToDisplayText(ConditionOperator op, bool opposite, object value)
        {
            StringBuilder sb = new StringBuilder();
            if (opposite) sb.AppendFormat("不");
            switch (op)
            {
                case ConditionOperator.In:
                    List<string> values = new List<string>();
                    foreach (object o in value as IEnumerable)
                    {
                        values.Add(ToDisplayText(o));
                    }
                    sb.AppendFormat("在{0}中", String.Join(",", values.ToArray()));
                    break;
                case ConditionOperator.LargerThan:
                    sb.AppendFormat("大于{0}", ToDisplayText(value));
                    break;
                case ConditionOperator.SmallerThan:
                    sb.AppendFormat("小于{0}", ToDisplayText(value));
                    break;
                case ConditionOperator.Contains:
                    sb.AppendFormat("包含{0}", ToDisplayText(value));
                    break;
                case ConditionOperator.Like:
                    sb.AppendFormat("匹配{0}", ToDisplayText(value));
                    break;
                case ConditionOperator.Equals:
                    sb.AppendFormat("等于{0}", ToDisplayText(value));
                    break;
                case ConditionOperator.EndsWith:
                    sb.AppendFormat("以{0}结尾", ToDisplayText(value));
                    break;
                case ConditionOperator.StartsWith:
                    sb.AppendFormat("以{0}开头", ToDisplayText(value));
                    break;
                default:
                    sb.Append(ToDisplayText(value)); break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据值生成显示的文本
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string ToDisplayText(object value)
        {
            if (value == null) return "空";
            return ToDisplayString(value);
        }

        public static string ToDisplayString(object value)
        {
            if (value is Enum) return GetDisplayName((Enum)value);
            else if (value is bool) return (bool)value ? "是" : "否";
            return Convert.ToString(value);
        }

        public static string GetLogString(object o, int expandDepth)
        {
            if (o is byte[])
            {
                byte[] bytes = (byte[])o;
                if (bytes.Length > 1024)
                    return "[bytes:" + bytes.Length + "]";
                else
                    return Convert.ToBase64String(bytes);
            }
            else if (o is ILogable) return "{" + ((ILogable)o).ToLog() + "}";
            else if (o is Array || o is ICollection)
            {
                if (expandDepth > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (object value in (IEnumerable)o)
                    {
                        if (sb.Length > 0) sb.Append(",");
                        sb.Append(GetLogString(value, expandDepth - 1));
                    }
                    return "{" + sb.ToString() + "}";
                }
                else
                {
                    int count = o is Array ? ((Array)o).Length : ((ICollection)o).Count;
                    return Convert.ToString(o).TrimEnd('[', ']') + "[" + count + "]";
                }
            }
            else return Convert.ToString(o);
        }

        public static List<SimpleCondition> ParseQueryCondition(IEnumerable<KeyValuePair<string, string>> queryString, Type type)
        {
            List<SimpleCondition> conditions = new List<SimpleCondition>();
            foreach (KeyValuePair<string, string> param in queryString)
            {
                PropertyDescriptor property = GetFilterProperties(type).Find(param.Key, true);
                if (property != null)

                    conditions.Add(ConditionConvert.ParseCondition(property, param.Value));
            }
            return conditions;
        }

        private static Dictionary<Type, PropertyDescriptorCollection> typeProperties = new Dictionary<Type, PropertyDescriptorCollection>();

        private static PropertyDescriptorCollection GetFilterProperties(Type type)
        {
            if (!typeProperties.ContainsKey(type))
            {
                List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(type))
                {
                    if (property.Attributes[typeof(ColumnAttribute)] == null)
                        properties.Add(property);
                    else
                    {
                        ColumnAttribute att = (ColumnAttribute)property.Attributes[typeof(ColumnAttribute)];
                        if (att.IsColumn && (att.ColumnMode & ColumnMode.Read) == ColumnMode.Read)
                        {
                            properties.Add(property);
                        }
                    }
                }
                properties.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, true));
                typeProperties[type] = new PropertyDescriptorCollection(properties.ToArray(), true);
            }
            return typeProperties[type];
        }

        /// <summary>
        /// 构建分页SQL
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="source">数据源</param>
        /// <param name="fields">输出字段</param>
        /// <param name="sorts">排序字段</param>
        /// <returns>分页的SQL查询语句</returns>
        public static string GetSQL(int? pageIndex, int? pageSize, string source, string fields, string sorts)
        {
            return "select _t_data.* from (SELECT ROW_NUMBER() OVER(ORDER BY " + sorts + ") AS RowNum," + fields + " FROM " + (source.IndexOf("from") != -1 ? "(" + source + ")" : source) + " as _t_source)_t_data where _t_data.RowNum>=" + ((pageIndex - 1) * pageSize + 1) + " and _t_data.RowNum<=" + pageIndex * pageSize + " ORDER BY _t_data.RowNum";
        }

    }







    public enum ExamAvailableEnum
    {
        Available,
        NotExist,
        UnStarted,
        ClosedOrCanceled
    }
}
