using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class BaseDataView
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 集团ID
        /// </summary>
        public string groupid { get; set; }

        /// <summary>
        /// 门店ID
        /// </summary>
        public string storeid { get; set; }
    }
}
