using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiMoMo
{
    public class StoresLib
    {
        public static DataTable Query()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CaiMoMoConnection"].ToString()))
            {
                conn.Open();
                string sql = "select * from dbo.SysGroupInfo ";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader sda = command.ExecuteReader();
                dt.Load(sda);
                conn.Close();
            }
            return dt;
        }


        public static DataTable Query(int groupId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CaiMoMoConnection"].ToString()))
            {
                conn.Open();
                string sql = "select * from dbo.SysStoreInfo where GroupId=" + groupId;
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader sda = command.ExecuteReader();
                dt.Load(sda);
                conn.Close();
            }
            return dt;
        }

        public static DataTable Get(int storeId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CaiMoMoConnection"].ToString()))
            {
                conn.Open();
                string sql = "select * from dbo.SysStoreInfo where UID=" + storeId;
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader sda = command.ExecuteReader();
                dt.Load(sda);
                conn.Close();
            }
            return dt;
        }
    }
}
