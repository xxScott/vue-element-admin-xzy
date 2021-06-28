using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Models;
using DAL;
using System.Data;

namespace Utilities
{
    public class JsonUtil
    {
        public static string SerializeObject(BaseDataView bdv)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(bdv, Newtonsoft.Json.Formatting.Indented, timeFormat);
        }
        //public static string SerializeObject(BaseDataView bdv, string dateFormat)
        //{
        //    IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
        //    timeFormat.DateTimeFormat = dateFormat;
        //    return JsonConvert.SerializeObject(bdv, Newtonsoft.Json.Formatting.Indented, timeFormat);
        //}
        /// <summary>
        /// 返回指定结果的Json字符串
        /// </summary>
        /// <param name="statusCode">指定结果</param>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetFail(Enum_StatusCode statusCode)
        {
            BaseDataView value = new BaseDataView() { code = (int)statusCode, message = Util.GetDisplayName(statusCode), data = null };
            return value;
        }

        /// <summary>
        /// 返回指定数据的Json字符串
        /// </summary>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetSuccess()
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = null };
            return value;
        }

        /// <summary>
        /// 返回执行失败的Json字符串，支持用户指定返回信息
        /// </summary>
        /// <param name="ex">系统异常</param>
        /// <returns>执行失败的Json字符串</returns>
        public static BaseDataView GetFailForException(Exception ex)
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Fail, message = ex.Message, data = null };
            return value;
        }

        /// <summary>
        /// 返回执行失败的Json字符串，支持用户指定返回信息
        /// </summary>
        /// <param name="message">用户指定返回信息</param>
        /// <returns>执行失败的Json字符串</returns>
        public static BaseDataView GetFailForString(string message)
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Fail, message = message, data = null };
            return value;
        }

        /// <summary>
        /// 返回指定数据的Json字符串
        /// </summary>
        /// <param name="data">指定数据</param>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetSuccessForDataTable(DataTable data)
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = data };
            return value;
        }

        /// <summary>
        /// 返回指定字符串的Json字符串
        /// </summary>
        /// <param name="data">指定字符串</param>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetSuccessForString(string data)
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = data };
            return value;
        }

        /// <summary>
        /// 返回执行成功的Json字符串
        /// </summary>
        /// <param name="data">需要返回的数据</param>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetSuccessForObject(object data)
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = data };
            return value;
        }

        /// <summary>
        /// 返回执行成功的Json字符串
        /// </summary>
        /// <param name="data">需要返回的数据</param>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetSuccessForObject(object data,string groupId,string storeId)
        {
            BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = data, groupid = groupId,storeid=storeId };
            return value;
        }

        ///// <summary>
        ///// 返回执行成功的Json字符串
        ///// </summary>
        ///// <param name="data">需要返回的数据</param>
        ///// <returns>Json字符串</returns>
        //public static BaseDataView GetSuccessForObject(Periods data)
        //{
        //    BaseDataView value = new BaseDataView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = data };
        //    return value;
        //}
        /// <summary>
        /// 返回执行成功的Json字符串
        /// </summary>
        /// <param name="data">需要返回的数据</param>
        /// <param name="total_count">记录总数</param>
        /// <returns>Json字符串</returns>
        public static BaseDataView GetSuccessForObject(object data, int total_count)
        {
            BaseCountView value = new BaseCountView() { code = (int)Enum_StatusCode.Success, message = Util.GetDisplayName(Enum_StatusCode.Success), data = data, total_count = total_count };
            return value;
        }
    }
}
