using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{


    public interface IAttendParamService : IEntityService<AttendParam>, IEntityViewService<AttendParam>, IEntityService, IEntityViewService
    {
        DataTable GetAll(int groupId, int storeId);
    }

    public class AttendParamService : ServiceBase<AttendParam, AttendParam>, IAttendParamService
    {

        public DataTable GetAll(int groupId, int storeId)
        {
            try
            {
                return ((AttendParamViewDAO)ObjectViewDAO).GetAll(groupId, storeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
