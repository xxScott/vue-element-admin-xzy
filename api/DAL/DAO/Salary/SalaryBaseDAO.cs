using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public partial class SalaryBaseDAO : CObjectDAO<SalaryBase> { }

    public partial class SalaryBaseViewDAO : CObjectViewDAO<SalaryBase>
    {
        public DataTable Query(int empId)
        {
            try
            {
                string sql = "select B_SalaryProject.*,tab.ID as salaryId,tab.SalaryAmount,tab.Description as salaryDescription  from dbo.B_SalaryProject left outer ";
                sql += "join (select * from Salary_Base where EmployeeID=@0) tab on B_SalaryProject.ID =tab.SalaryProjectID";

                using (IDbCommand command = MakeParamCommand(sql, new object[] { empId }))
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
