
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DAL.Business;
using System.Data;
using MyOrm.Common;

namespace DAL
{
    public interface IGroupsService : IEntityService<Groups>, IEntityViewService<Groups>, IEntityService, IEntityViewService
    {
        DataTable GetAll();
        DataTable Query();
    }

    public class GroupsService : ServiceBase<Groups, Groups>, IGroupsService
    {

        public DataTable GetAll()
        {
            try
            {
                return ((GroupsViewDAO)ObjectViewDAO).GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Query()
        {
            try
            {
                return ((GroupsViewDAO)ObjectViewDAO).Query();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}