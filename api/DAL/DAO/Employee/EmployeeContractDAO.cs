using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public partial class EmployeeContractDAO : CObjectDAO<EmployeeContract> { }

    public partial class EmployeeContractViewDAO : CObjectViewDAO<EmployeeContractView>
    {
        public DataTable GetAll()
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from dbo.Emp_Contract ", null))
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
