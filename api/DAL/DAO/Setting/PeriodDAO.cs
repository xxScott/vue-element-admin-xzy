using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public partial class PeriodDAO : CObjectDAO<Periods> { }

    public partial class PeriodViewDAO : CObjectViewDAO<Periods>
    {
        public DataTable Query(int companyId)
        {
            try
            {
                string sql = "select B_Period.*, Salary_Settle.State,Salary_Settle.HasSettled from dbo.B_Period  left outer join dbo.Salary_Settle on ";
                sql += "  B_Period.ID=Salary_Settle.PeroidID where CompanyID=@0 ";
                using (IDbCommand command = MakeParamCommand(sql, new object[] { companyId }))
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
