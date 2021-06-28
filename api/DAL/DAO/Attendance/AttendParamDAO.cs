using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{


    public partial class AttendParamDAO : CObjectDAO<AttendParam> { }

    public partial class AttendParamViewDAO : CObjectViewDAO<AttendParam>
    {
        public DataTable GetAll(int groupId, int storeId)
        {
            try
            {
                using (IDbCommand command = MakeParamCommand("select * from dbo.Attend_Param  where GroupID=@0 and StoreID=@1", new object[] { groupId, storeId }))
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
