using Com.Caimomo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Caimomo.Common
{
    public class MyHandleResult: HandleResult
    {
        private int m_total_count = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int total_count { get => m_total_count; set => m_total_count = value; }
    }
}