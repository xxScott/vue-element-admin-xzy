using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DAL
{
    public partial class GroupsDAO : CObjectDAO<Groups> { }

    public partial class GroupsViewDAO : CObjectViewDAO<Groups>
    {
        public DataTable GetAll()
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from dbo.B_Group"))
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
        public DataTable Query()
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from B_Dept left outer join dbo.B_Company on B_Dept.CompanyID=B_Company.ID"))
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