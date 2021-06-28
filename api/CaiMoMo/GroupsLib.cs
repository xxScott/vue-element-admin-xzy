using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Business;

namespace CaiMoMo
{
    public class GroupsLib
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


        public static DataTable Query(int uid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CaiMoMoConnection"].ToString()))
            {
                conn.Open();
                string sql = "select * from dbo.SysGroupInfo where UID=" + uid;
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader sda = command.ExecuteReader();
                dt.Load(sda);
                conn.Close();
            }
            return dt;
        }

      
       
    }
}
