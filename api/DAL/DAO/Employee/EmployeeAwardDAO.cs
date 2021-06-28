using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class EmployeeAwardDAO : CObjectDAO<EmployeeAward> { }

    public partial class EmployeeAwardViewDAO : CObjectViewDAO<EmployeeAwardView>
    {
        public DataTable GetAll()
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from dbo.Emp_Award ", null))
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
