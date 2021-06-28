using Com.Caimomo.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;

public class MyDataContextFactory
{
    public static System.Collections.Generic.Dictionary<int, string> ConnectionStrings = new System.Collections.Generic.Dictionary<int, string>();

    public static System.Collections.Generic.Dictionary<int, string> ConnectionStringsForRead = new System.Collections.Generic.Dictionary<int, string>();

    public static bool isReadWriteSeparate = true;

    public static readonly int CanYinCenter = -1;

    public static readonly int Supplier = -2;

    public static readonly int PayCenter = -3;

    public static readonly int TempDB = -4;

    public static readonly int WaiMai = -5;

    public static readonly int History = -6;

    public static readonly int meituan = -7;

    public static readonly int MessageCenter = -8;

    public static readonly int wechat = -9;

    public static readonly int jifenCenter = -10;

    public static readonly int Log = -11;

    public static readonly int CloudPrint = -12;

    public static readonly int OperationLog = -13;

    public static readonly int CaiMoMo_HR = -14;

    public static readonly int CaiMoMo_TC = -15;

    public static T GetDataContext<T>(int groupId) where T : DataContext
    {
        T result;
        if (groupId == DataContextFactory.CanYinCenter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CanYinCenterConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.PayCenter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PayCenterConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.History)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HistoryConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.meituan)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["meituanConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.jifenCenter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["JiFenCenterConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == MyDataContextFactory.CaiMoMo_HR)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CaiMoMo_HRConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == MyDataContextFactory.CaiMoMo_TC)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CaiMoMo_TCConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (DataContextFactory.ConnectionStrings.ContainsKey(groupId))
        {
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    DataContextFactory.ConnectionStrings[groupId]
            }) as T);
        }
        else
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CanYinCenterConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "SELECT UID,ServerAddress,DBName,UserID,Password FROM SysGroupInfoExtra WHERE UID=" + groupId.ToString();
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    string @string = sqlDataReader.GetString(1);
                    string string2 = sqlDataReader.GetString(2);
                    string string3 = sqlDataReader.GetString(3);
                    string string4 = sqlDataReader.GetString(4);
                    string text = string.Concat(new string[]
                    {
                            "Data Source=",
                            @string,
                            ";Initial Catalog=",
                            string2,
                            ";User ID=",
                            string3,
                            ";Password=",
                            string4,
                            ";"
                    });
                    DataContextFactory.ConnectionStrings[groupId] = text;
                    result = (System.Activator.CreateInstance(typeof(T), new object[]
                    {
                            text
                    }) as T);
                }
                else
                {
                    result = default(T);
                }
            }
        }
        return result;
    }

    public static T GetDataContext<T>(int groupId, bool isForRead) where T : DataContext
    {
        T result;
        if (!DataContextFactory.isReadWriteSeparate || !isForRead)
        {
            result = DataContextFactory.GetDataContext<T>(groupId);
        }
        else if (groupId == DataContextFactory.CanYinCenter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CanYinCenterConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.PayCenter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PayCenterConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.History)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HistoryConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.meituan)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["meituanConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (groupId == DataContextFactory.jifenCenter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["JiFenCenterConnectionString"].ConnectionString;
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    connectionString
            }) as T);
        }
        else if (DataContextFactory.ConnectionStringsForRead.ContainsKey(groupId))
        {
            result = (System.Activator.CreateInstance(typeof(T), new object[]
            {
                    DataContextFactory.ConnectionStringsForRead[groupId]
            }) as T);
        }
        else
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CanYinCenterConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "SELECT UID,ServerAddressForRead,DBName,UserID,Password FROM SysGroupInfoExtra WHERE UID=" + groupId.ToString();
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    string @string = sqlDataReader.GetString(1);
                    string string2 = sqlDataReader.GetString(2);
                    string string3 = sqlDataReader.GetString(3);
                    string string4 = sqlDataReader.GetString(4);
                    string text = string.Concat(new string[]
                    {
                            "Data Source=",
                            @string,
                            ";Initial Catalog=",
                            string2,
                            ";User ID=",
                            string3,
                            ";Password=",
                            string4,
                            ";"
                    });
                    DataContextFactory.ConnectionStringsForRead[groupId] = text;
                    result = (System.Activator.CreateInstance(typeof(T), new object[]
                    {
                            text
                    }) as T);
                }
                else
                {
                    result = default(T);
                }
            }
        }
        return result;
    }
}
