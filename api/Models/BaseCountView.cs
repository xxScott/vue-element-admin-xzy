using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class BaseCountView:BaseDataView
    {
        /// <summary>
        /// 记录总数
        /// </summary>
        public int total_count { get; set; }
    }
}
