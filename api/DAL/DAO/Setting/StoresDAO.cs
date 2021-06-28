using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public partial class StoresDAO : CObjectDAO<Stores> { }

    public partial class StoresViewDAO : CObjectViewDAO<Stores>
    {
        public DataTable Query()
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from B_Store left outer join dbo.B_Group on B_Store.GroupID=B_Group.ID"))
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
