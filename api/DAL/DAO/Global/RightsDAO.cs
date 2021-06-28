using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class RightsDAO : CObjectDAO<Rights> { }

    public partial class RightsViewDAO : CObjectViewDAO<Rights>
    {
        public DataTable GetResult(int roleId)
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select ModuleID,HasAdd,HasUpdate,HasDelete,HasSubmit,HasQuery from GLO_Right where RoleID=@0", new object[] { roleId }))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetResult(int userId, int moduleId)
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select HasAdd,HasUpdate,HasDelete,HasSubmit,HasQuery from GLO_Right where RoleID in (select RoleID from GLO_Role_User where UserID=@0) and ModuleID=@1", new object[] { userId, moduleId }))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}