using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Caimomo.Common;
using Com.Caimomo.Entity;
using System.Collections.Specialized;

namespace Com.Caimomo.Handlers
{
    public class GetGroupInfo : IAjaxHandler
    {
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            //try
            //{
            //    int GroupID = int.Parse(queryString["GroupID"]);

            //    using (EasyDataContext context = DataContextFactory.GetDataContext<EasyDataContext>(DataContextFactory.CanYinCenter))
            //    {
            //            SysGroupInfo groupInfo = context.SysGroupInfo.NoLock().SingleOrDefault(g=>g.UID == GroupID);
            //            if (!object.Equals(groupInfo, null))
            //                return new HandleResult { ResultCode = 0, Message = "", Data = groupInfo };
            //            else
            //                return new HandleResult { ResultCode = 1, Message = "集团不存在", Data = null };

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog("获取集团出错:" + ex);
            //    return new HandleResult { ResultCode = 1, Message = "获取集团出错:" + ex, Data = null };
            //}
            return null;
        }
    }

}