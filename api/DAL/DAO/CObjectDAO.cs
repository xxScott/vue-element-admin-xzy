using DAL.Business;
using MyOrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MyOrm.Common;

namespace DAL
{

    public interface ICObjectDAO<T> : IObjectDAO<T>
    {
        int GetMaxID();
    }

    public interface ICObjectViewDAO<T> : IObjectViewDAO<T> { }

    public class CObjectDAO<T> : ObjectDAO<T>, ICObjectDAO<T> where T : new()
    {
        public CObjectDAO() { SessionManager = ServiceFactory.SessionManager; }

        public int GetMaxID()
        {
            using (IDbCommand command = MakeParamCommand("select max(ID) as MaxID from " + TableName))
            {
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["MaxID"] == DBNull.Value || reader["MaxID"] == null ? 0 : Convert.ToInt32(reader["MaxID"]);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }

    public class CObjectViewDAO<T> : ObjectViewDAO<T>, ICObjectViewDAO<T> where T : new()
    {
        public CObjectViewDAO() { SessionManager = ServiceFactory.SessionManager; }
    }
}