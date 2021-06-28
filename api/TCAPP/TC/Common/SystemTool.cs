using Com.Caimomo.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Com.Caimomo.Common
{
    public class SystemTool
    {
        public static List<Dictionary<string, object>> ToDictionaryList(DataTable dt)
        {
            var list = dt.Rows.Cast<DataRow>().Select(x =>
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    dict.Add(dc.ColumnName, x[dc.ColumnName]);
                }
                return dict;
            }).ToList();

            return list;
        }


        /// <summary>
        /// DataTable通过流导出Excel
        /// </summary>
        /// <param name="ds">数据源DataSet</param>
        /// <param name="columns">DataTable中列对应的列名(可以是中文),若为null则取DataTable中的字段名</param>
        /// <param name="fileName">保存文件名(例如：a.xls)</param>
        /// <returns></returns>
        public static bool StreamExport(DataTable dt, string[] columns, string fileName, HttpContext pages)
        {
            if (dt.Rows.Count > 65535) //总行数大于Excel的行数 
            {
                throw new Exception("预导出的数据总行数大于excel的行数");
            }
            if (string.IsNullOrEmpty(fileName)) return false;

            StringBuilder content = new StringBuilder();
            StringBuilder strtitle = new StringBuilder();
            content.Append("<html>");
            content.Append("<head><title></title>");
            //注意：[if gte mso 9]到[endif]之间的代码，用于显示Excel的网格线，若不想显示Excel的网格线，可以去掉此代码
            content.Append("<!--[if gte mso 9]>");
            content.Append("<xml>");
            content.Append(" <x:ExcelWorkbook>");
            content.Append("  <x:ExcelWorksheets>");
            content.Append("   <x:ExcelWorksheet>");
            content.Append("    <x:Name>Sheet1</x:Name>");
            content.Append("    <x:WorksheetOptions>");
            content.Append("      <x:Print>");
            content.Append("       <x:ValidPrinterInfo />");
            content.Append("      </x:Print>");
            content.Append("    </x:WorksheetOptions>");
            content.Append("   </x:ExcelWorksheet>");
            content.Append("  </x:ExcelWorksheets>");
            content.Append("</x:ExcelWorkbook>");
            content.Append("</xml>");
            content.Append("<![endif]-->");
            content.Append("</head><body><table style='border-collapse:collapse;table-layout:fixed;'><tr>");

            if (columns != null)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i] != null && columns[i] != "")
                    {
                        content.Append("<td><b>" + columns[i] + "</b></td>");
                    }
                    else
                    {
                        content.Append("<td><b>" + dt.Columns[i].ColumnName + "</b></td>");
                    }
                }
            }
            else
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    content.Append("<td><b>" + dt.Columns[j].ColumnName + "</b></td>");
                }
            }
            content.Append("</tr>\n");

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                content.Append("<tr>");
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    object obj = dt.Rows[j][k];
                    Type type = obj.GetType();
                    if (type.Name == "Int32" || type.Name == "Single" || type.Name == "Double" || type.Name == "Decimal")
                    {
                        double d = obj == DBNull.Value ? 0.0d : Convert.ToDouble(obj);
                        if (type.Name == "Int32" || (d - Math.Truncate(d) == 0))
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0'>{0}</td>", obj);
                        else
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0.00'>{0}</td>", obj);
                    }
                    else
                        content.AppendFormat("<td style='vnd.ms-excel.numberformat:@'>{0}</td>", obj);
                }
                content.Append("</tr>\n");
            }
            content.Append("</table></body></html>");


            content.Replace("&nbsp;", "");
            pages.Response.Clear();
            pages.Response.Buffer = true;
            pages.Response.ContentType = "application/ms-excel";  //"application/ms-excel";
            pages.Response.Charset = "UTF-8";
            pages.Response.ContentEncoding = System.Text.Encoding.UTF8;

            pages.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + "\"");
            pages.Response.Write(content.ToString());
            //pages.Response.End();  //注意，若使用此代码结束响应可能会出现“由于代码已经过优化或者本机框架位于调用堆栈之上,无法计算表达式的值。”的异常。
            HttpContext.Current.ApplicationInstance.CompleteRequest(); //用此行代码代替上一行代码，则不会出现上面所说的异常。
            return true;
        }

        public static string GetSystemConfigValue(int storeid, string configName)
        {
            return GetSystemConfigValue(storeid, Security.CurrentUser.GroupID, configName, "");
        }

        public static string GetSystemConfigValue(int storeid, string configName, string defaultValue)
        {
            return GetSystemConfigValue(storeid, Security.CurrentUser.GroupID, configName, defaultValue);
        }

        public static string GetSystemConfigValue(int storeid, int groupid, string configName, string defaultValue)
        {
            using (EasyDataContext context = DataContextFactory.GetDataContext<EasyDataContext>(groupid))
            {
                BaseSystemConfig baseConfig = context.BaseSystemConfig.SingleOrDefault(c => c.StoreID == storeid && c.SystemConfigName.Trim().Equals(configName));
                if (!object.Equals(baseConfig, null))
                    return baseConfig.SystemConfigValue;
                else
                {
                    using (EasyDataContext ccontext = DataContextFactory.GetDataContext<EasyDataContext>(DataContextFactory.CanYinCenter))
                    {
                        BaseSystemConfigDefault baseConfigDefault = ccontext.BaseSystemConfigDefault.SingleOrDefault(c => c.SystemConfigName.Trim().Equals(configName));
                        if (!object.Equals(baseConfigDefault, null))
                            return baseConfigDefault.SystemConfigValue;
                        else
                            return defaultValue;
                    }
                }
            }
        }

        public static void SaveSystemConfig(int storeid, SystemConfig config, string value, SysGroupUser user)
        {
            using (EasyDataContext context = DataContextFactory.GetDataContext<EasyDataContext>(Security.CurrentUser.GroupID))
            {
                BaseSystemConfig baseConfig = context.BaseSystemConfig.SingleOrDefault(c => c.StoreID == storeid && c.SystemConfigAlias == config.ToString());
                if (object.Equals(baseConfig, null))
                {
                    baseConfig = new BaseSystemConfig();
                    baseConfig.StoreID = storeid;
                    baseConfig.SystemConfigName = config.ToString();
                    baseConfig.SystemConfigAlias = SystemConfigTool.GetConfigAlias(config);
                    baseConfig.AddTime = DateTime.Now;
                    baseConfig.AddUser = user.UID;
                    baseConfig.SystemConfigTypeName = "";

                    context.BaseSystemConfig.InsertOnSubmit(baseConfig);
                }

                baseConfig.SystemConfigValue = value;
                baseConfig.UpdateTime = DateTime.Now;
                baseConfig.UpdateUser = user.UID;
                context.SubmitChanges();
            }
        }
    }
}