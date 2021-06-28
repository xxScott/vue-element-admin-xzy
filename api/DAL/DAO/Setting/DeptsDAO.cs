using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class DeptsDAO : CObjectDAO<Depts> { }

    public partial class DeptsViewDAO : CObjectViewDAO<Depts>
    {
        public DataTable GetAll(object groupId, object storeId)
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from B_Dept where GroupID=@0 and StoreID=@1", new object[] { groupId, storeId }))
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

        public DataTable Query(object groupId, object storeId, string keyword)
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from B_Dept where GroupID=@0 and StoreID=@1 and (DeptCode like @2 or DeptName like @2 or Description like @2)", new object[] { groupId, storeId, "%" + keyword + "%" }))
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