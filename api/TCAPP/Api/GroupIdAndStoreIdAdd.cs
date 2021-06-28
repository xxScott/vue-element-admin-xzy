using DAL;
using MyOrm.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HrApp.Api
{
    public static class GroupIdAndStoreIdAdd
    {
        public static void AddGroupIdAndStoreId(ConditionSet condition)
        {
            Users currentUser = Utilities.Security.CurrentUser;
            int? storeid = currentUser.StoreID;
            int? groudid = currentUser.GroupID;

            if (groudid.HasValue)
            {
                condition.Add(new SimpleCondition("GroupID", groudid.Value));
            }
            if (storeid.HasValue)
            {
                condition.Add(new SimpleCondition("StoreID", storeid.Value));
            }
        }
    }
}