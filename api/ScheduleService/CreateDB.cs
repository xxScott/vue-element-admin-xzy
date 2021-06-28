using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DAL;
using DAL.Business;
using MyOrm.Common;
namespace ScheduleService
{
    public class CreateDB
    {
        static bool Running { get; set; }
        Log log = new Log();

        public void Execute()
        {
            Running = true;
            while (Running)
            {
                try
                {
                    List<Groups> companys = ServiceFactory.Factory.GroupsService.Search(null);
                    IAttendParamService service = ServiceFactory.Factory.AttendParamService;
                    List<AttendParam> list = service.Search(null);
                    //  log.Info(DateTime.Now + "::::开始创建签到数据库或表！\n\t");
                    for (int i = 0; i < companys.Count; i++)
                    {
                        List<AttendParam> attendParams = list.Where(p => p.GroupID== companys[i].ID).ToList();
                        //获取设置参数
                        AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                        AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                        AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                        AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));
                        if (server != null && userId != null && userPwd != null && storage != null)
                        {
                            using (SqlConnection conn = new SqlConnection("Data Source=" + server.ParamValue + ";Initial Catalog=master;User ID=" + userId.ParamValue + ";Password=" + userPwd.ParamValue))
                            {
                                conn.Open();

                                string companyCode = companys[i].UID.ToString();
                                if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                                {
                                    //判断数据库是否存在
                                    SqlCommand comd = new SqlCommand("select count(*) from sys.databases where name='CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + "'", conn);
                                    int n = int.Parse(comd.ExecuteScalar().ToString());
                                    string sql = string.Empty;
                                    if (n <= 0)
                                    {
                                        //创建对应数据库
                                        sql = "CREATE DATABASE CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + " ON PRIMARY"
                                      + "(name=CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + ", filename = 'D:\\CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + ".mdf') log on"
                                      + "(name=CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + "_log, filename='D:\\CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + "_log.ldf')";
                                        comd = new SqlCommand(sql, conn);
                                        n = comd.ExecuteNonQuery();
                                        //log.Info(DateTime.Now + "::::创建库：CaiMoMo_HR_" + companys[i].GroupCode + "_" + DateTime.Now.Year + "成功！");
                                    }
                                    else
                                    {
                                        //log.Error(DateTime.Now + "::::已存在：CaiMoMo_HR_" + companys[i].GroupCode + "_" + DateTime.Now.Year + "，创建失败！");
                                    }
                                    //创建对应月份表
                                    for (int d = 1; d <= 12; d++)
                                    {
                                        //创建原始数据表
                                        sql = " use CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + " "
                                      + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + DateTime.Now.Year + d.ToString("00") + "'";
                                        comd = new SqlCommand(sql, conn);
                                        SqlDataReader sda = comd.ExecuteReader();
                                        if (sda.Read())
                                            n = Convert.ToInt32(sda["Count"].ToString());
                                        sda.Close();

                                        if (n <= 0)
                                        {
                                            sql = " use CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + " "
                                            + "CREATE TABLE Scan_" + DateTime.Now.Year + d.ToString("00")
                                            + "(ID int	NOT NULL	PRIMARY KEY identity(1,1),"
                                            + "CompanyID	int,"
                                            + "DeptID	int,"
                                            + "EmployeeID int,"
                                            + "ScanTime datetime,"
                                            + "ScanType int )";
                                            comd = new SqlCommand(sql, conn);
                                            n = comd.ExecuteNonQuery();
                                            log.Info(DateTime.Now + "::::创建原始数据表：Scan_" + DateTime.Now.Year + "_" + d.ToString("00") + "成功！\n\t");

                                        }

                                        //创建整理表
                                        sql = " use CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + " "
                            + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Month_" + DateTime.Now.Year + d.ToString("00") + "'";
                                        comd = new SqlCommand(sql, conn);
                                        sda = comd.ExecuteReader();
                                        if (sda.Read())
                                            n = Convert.ToInt32(sda["Count"].ToString());
                                        sda.Close();
                                        if (n <= 0)
                                        {
                                            sql = " use CaiMoMo_HR_" + companyCode + "_" + DateTime.Now.Year + " "
                                            + "CREATE TABLE Month_" + DateTime.Now.Year + d.ToString("00")
                                            + "(ID int	NOT NULL	PRIMARY KEY identity(1,1),"
                                            + "CompanyID	int,"
                                            + "DeptID	int,"
                                            + "EmployeeID int,"
                                            + "ClassID int,"
                                            + "StartTime datetime,"
                                            + "OverTime datetime,"
                                            + "FactStartTime datetime,"
                                            + "FactOverTime datetime,"
                                            + "BeLate int,"
                                            + "LeaveEarlier int,"
                                            + "StayAway bit )";
                                            comd = new SqlCommand(sql, conn);
                                            n = comd.ExecuteNonQuery();
                                            log.Info(DateTime.Now + "::::创建整理表：Month_" + DateTime.Now.Year + "_" + d.ToString("00") + "成功！\n\t");

                                        }
                                    }
                                    log.Info(DateTime.Now + "::::结束创建签到数据库或表！\n\t");
                                }

                                conn.Close();
                            }
                        }
                        else
                        {
                            log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + "参数未设置完全！\n\t");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(DateTime.Now + "::::出现错误：" + ex.Message + "！\n\t");
                    Thread.Sleep(1000 * 30);
                }

            }
        }



        public void CreateDMD()
        {
            List<Groups> groups = ServiceFactory.Factory.GroupsService.Search(null);
            List<Stores> stores = ServiceFactory.Factory.StoresService.Search(null);
            IAttendParamService service = ServiceFactory.Factory.AttendParamService;
            List<AttendParam> list = service.Search(null);
            List<Stores> dmdStores = stores.Where(s => s.GroupID == -1).ToList();

            for (int s = 0; s < dmdStores.Count; s++)
            {
                List<AttendParam> attendParams = list.Where(p => p.StoreID == dmdStores[s].ID).ToList();
                //获取设置参数
                AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));
                if (server != null && userId != null && userPwd != null && storage != null)
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=" + server.ParamValue + ";Initial Catalog=master;User ID=" + userId.ParamValue + ";Password=" + userPwd.ParamValue))
                    {
                        conn.Open();

                        string groupUID = "DMD";
                        if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                        {
                            //判断数据库是否存在
                            SqlCommand comd = new SqlCommand("select count(*) from sys.databases where name='CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + "'", conn);
                            int n = int.Parse(comd.ExecuteScalar().ToString());
                            string sql = string.Empty;
                            if (n <= 0)
                            {
                                //创建对应数据库
                                sql = "CREATE DATABASE CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " ON PRIMARY"
                              + "(name=CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + ", filename = 'D:\\CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + ".mdf') log on"
                              + "(name=CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + "_log, filename='D:\\CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + "_log.ldf')";
                                comd = new SqlCommand(sql, conn);
                                n = comd.ExecuteNonQuery();
                                //log.Info(DateTime.Now + "::::创建库：CaiMoMo_HR_" + companys[i].GroupCode + "_" + DateTime.Now.Year + "成功！");
                            }
                            else
                            {
                                //log.Error(DateTime.Now + "::::已存在：CaiMoMo_HR_" + companys[i].GroupCode + "_" + DateTime.Now.Year + "，创建失败！");
                            }



                            string storeUID = dmdStores[s].UID.ToString();
                            //创建对应月份表
                            for (int d = 1; d <= 12; d++)
                            {
                                //创建原始数据表
                                sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                              + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + storeUID + "_" + DateTime.Now.Year + d.ToString("00") + "'";
                                comd = new SqlCommand(sql, conn);
                                SqlDataReader sda = comd.ExecuteReader();
                                if (sda.Read())
                                    n = Convert.ToInt32(sda["Count"].ToString());
                                sda.Close();

                                if (n <= 0)
                                {
                                    sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                                    + "CREATE TABLE Scan_" + storeUID + "_" + DateTime.Now.Year + d.ToString("00")
                                    + "(ID int	NOT NULL	PRIMARY KEY identity(1,1),"
                                    + "StoreID	int,"
                                    + "DeptID	int,"
                                    + "EmployeeID int,"
                                    + "ScanTime datetime,"
                                    + "ScanType int )";
                                    comd = new SqlCommand(sql, conn);
                                    n = comd.ExecuteNonQuery();
                                    //  log.Info(DateTime.Now + "::::创建原始数据表：Scan_" + DateTime.Now.Year + "_" + d.ToString("00") + "成功！\n\t");

                                }

                                //创建整理表
                                sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                    + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Month_" + DateTime.Now.Year + d.ToString("00") + "'";
                                comd = new SqlCommand(sql, conn);
                                sda = comd.ExecuteReader();
                                if (sda.Read())
                                    n = Convert.ToInt32(sda["Count"].ToString());
                                sda.Close();
                                if (n <= 0)
                                {
                                    sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                                    + "CREATE TABLE Month_" + storeUID + "_" + DateTime.Now.Year + d.ToString("00")
                                    + "(ID int	NOT NULL	PRIMARY KEY identity(1,1),"
                                    + "StoreID	int,"
                                    + "DeptID	int,"
                                    + "EmployeeID int,"
                                    + "ClassID int,"
                                    + "StartTime datetime,"
                                    + "OverTime datetime,"
                                    + "FactStartTime datetime,"
                                    + "FactOverTime datetime,"
                                    + "BeLate int,"
                                    + "LeaveEarlier int,"
                                    + "StayAway bit )";
                                    comd = new SqlCommand(sql, conn);
                                    n = comd.ExecuteNonQuery();
                                    // log.Info(DateTime.Now + "::::创建整理表：Month_" + DateTime.Now.Year + "_" + d.ToString("00") + "成功！\n\t");

                                }
                            }
                        }
                    }
                }
            }

        }


        public void Create()
        {
            List<Groups> groups = ServiceFactory.Factory.GroupsService.Search(null);
            List<Stores> stores = ServiceFactory.Factory.StoresService.Search(null);
            IAttendParamService service = ServiceFactory.Factory.AttendParamService;
            List<AttendParam> list = service.Search(null);
            //  log.Info(DateTime.Now + "::::开始创建签到数据库或表！\n\t");


            for (int i = 0; i < groups.Count; i++)
            {
                List<AttendParam> attendParams = list.Where(p => p.GroupID == groups[i].ID).ToList();
                //获取设置参数
                AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));
                if (server != null && userId != null && userPwd != null && storage != null)
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=" + server.ParamValue + ";Initial Catalog=master;User ID=" + userId.ParamValue + ";Password=" + userPwd.ParamValue))
                    {
                        conn.Open();

                        string groupUID = groups[i].UID.ToString();
                        if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                        {
                            //判断数据库是否存在
                            SqlCommand comd = new SqlCommand("select count(*) from sys.databases where name='CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + "'", conn);
                            int n = int.Parse(comd.ExecuteScalar().ToString());
                            string sql = string.Empty;
                            if (n <= 0)
                            {
                                //创建对应数据库
                                sql = "CREATE DATABASE CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " ON PRIMARY"
                              + "(name=CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + ", filename = 'D:\\CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + ".mdf') log on"
                              + "(name=CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + "_log, filename='D:\\CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + "_log.ldf')";
                                comd = new SqlCommand(sql, conn);
                                n = comd.ExecuteNonQuery();
                                //log.Info(DateTime.Now + "::::创建库：CaiMoMo_HR_" + companys[i].GroupCode + "_" + DateTime.Now.Year + "成功！");
                            }
                            else
                            {
                                //log.Error(DateTime.Now + "::::已存在：CaiMoMo_HR_" + companys[i].GroupCode + "_" + DateTime.Now.Year + "，创建失败！");
                            }

                            List<Stores> groupStores = stores.Where(s => s.GroupID == groups[i].ID).ToList();
                            for (int s = 0; s < groupStores.Count; s++)
                            {
                                string storeUID = groupStores[s].UID.ToString();
                                //创建对应月份表
                                for (int d = 1; d <= 12; d++)
                                {
                                    //创建原始数据表
                                    sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                                  + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + storeUID + "_" + DateTime.Now.Year + d.ToString("00") + "'";
                                    comd = new SqlCommand(sql, conn);
                                    SqlDataReader sda = comd.ExecuteReader();
                                    if (sda.Read())
                                        n = Convert.ToInt32(sda["Count"].ToString());
                                    sda.Close();

                                    if (n <= 0)
                                    {
                                        sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                                        + "CREATE TABLE Scan_" + storeUID + "_" + DateTime.Now.Year + d.ToString("00")
                                        + "(ID int	NOT NULL	PRIMARY KEY identity(1,1),"
                                        + "StoreID	int,"
                                        + "DeptID	int,"
                                        + "EmployeeID int,"
                                        + "ScanTime datetime,"
                                        + "ScanType int )";
                                        comd = new SqlCommand(sql, conn);
                                        n = comd.ExecuteNonQuery();
                                        //  log.Info(DateTime.Now + "::::创建原始数据表：Scan_" + DateTime.Now.Year + "_" + d.ToString("00") + "成功！\n\t");

                                    }

                                    //创建整理表
                                    sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                        + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Month_" + DateTime.Now.Year + d.ToString("00") + "'";
                                    comd = new SqlCommand(sql, conn);
                                    sda = comd.ExecuteReader();
                                    if (sda.Read())
                                        n = Convert.ToInt32(sda["Count"].ToString());
                                    sda.Close();
                                    if (n <= 0)
                                    {
                                        sql = " use CaiMoMo_HR_" + groupUID + "_" + DateTime.Now.Year + " "
                                        + "CREATE TABLE Month_" + storeUID + "_" + DateTime.Now.Year + d.ToString("00")
                                        + "(ID int	NOT NULL	PRIMARY KEY identity(1,1),"
                                        + "StoreID	int,"
                                        + "DeptID	int,"
                                        + "EmployeeID int,"
                                        + "ClassID int,"
                                        + "StartTime datetime,"
                                        + "OverTime datetime,"
                                        + "FactStartTime datetime,"
                                        + "FactOverTime datetime,"
                                        + "BeLate int,"
                                        + "LeaveEarlier int,"
                                        + "StayAway bit )";
                                        comd = new SqlCommand(sql, conn);
                                        n = comd.ExecuteNonQuery();
                                        // log.Info(DateTime.Now + "::::创建整理表：Month_" + DateTime.Now.Year + "_" + d.ToString("00") + "成功！\n\t");

                                    }
                                }
                            }
                            // log.Info(DateTime.Now + "::::结束创建签到数据库或表！\n\t");
                        }
                        conn.Close();
                    }
                }
                else
                {
                    // log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + "参数未设置完全！\n\t");
                }
            }
        }


        /// <summary>
        /// 处理单门店基础打卡数据
        /// </summary>
        public void DataDMD()
        {

            //获取所有门店
            List<Stores> stores = ServiceFactory.Factory.StoresService.Search(new SimpleCondition("GroupID", -1));
            //获取获取所有期间
            List<Periods> periods = ServiceFactory.Factory.PeriodService.Search(null);
            //获取所有员工
            List<EmployeeBaseView> employees = ServiceFactory.Factory.EmployeeBaseService.Search(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));
            //获取所有设置
            List<AttendParam> list = ServiceFactory.Factory.AttendParamService.Search(null);

            //log.Info(DateTime.Now + "::::开始整理数据表基础数据！\n\t");
            try
            {
                for (int i = 0; i < stores.Count; i++)
                {
                    string companyCode = "DMD";
                    List<AttendParam> attendParams = list.Where(p => p.StoreID == stores[i].ID).ToList();

                    AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                    AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                    AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                    AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));
                    //获取公司对应期间
                    List<Periods> companyPeriods = periods.Where(p => p.StoreID == stores[i].ID).ToList();

                    string storeUID = stores[i].UID.ToString();
                    //获取公司对应正式员工
                    List<EmployeeBaseView> companyEmpes = employees.Where(e => e.StoreID == stores[i].ID).ToList();

                    if (server != null && userId != null && userPwd != null && storage != null)
                    {
                        using (SqlConnection conn = new SqlConnection("Data Source=" + server.ParamValue + ";Initial Catalog=master;User ID=" + userId.ParamValue + ";Password=" + userPwd.ParamValue))
                        {
                            conn.Open();

                            if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                            {
                                for (int p = 0; p < companyPeriods.Count; p++)
                                {
                                    #region 整理对应期间

                                    int m = Convert.ToDateTime(companyPeriods[p].StartDate.ToString()).Month;
                                    int y = Convert.ToDateTime(companyPeriods[p].StartDate.ToString()).Year;
                                    //判断当前期间年份数据库是否存在
                                    SqlCommand comd = new SqlCommand("select count(*) from sys.databases where name='CaiMoMo_HR_" + companyCode + "_" + y.ToString("0000") + "'", conn);
                                    int n = int.Parse(comd.ExecuteScalar().ToString());
                                    string sql = string.Empty;
                                    if (n > 0)
                                    {
                                        //判断当前期间数据基础表是否存在
                                        sql = " use CaiMoMo_HR_" + companyCode + "_" + y.ToString("0000") + " "
                                  + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + storeUID + "_" + y.ToString("0000") + m.ToString("00") + "'";
                                        comd = new SqlCommand(sql, conn);
                                        SqlDataReader sda = comd.ExecuteReader();
                                        if (sda.Read())
                                            n = Convert.ToInt32(sda["Count"].ToString());
                                        sda.Close();
                                        if (n > 0)
                                        {
                                            //删除原处理表数据
                                            sql = " use CaiMoMo_HR_" + companyCode + "_" + y.ToString("0000") + " "
                                  + " delete Month_" + storeUID + "_" + y.ToString("0000") + m.ToString("00");
                                            comd = new SqlCommand(sql, conn);
                                            if (comd.ExecuteNonQuery() > -1)
                                            {
                                                DataView dvData = new DataView();
                                                //取出当前期间所有签到数据
                                                sql = "select * from Scan_" + storeUID + "_" + DateTime.Now.Year + m.ToString("00");
                                                comd = new SqlCommand(sql, conn);

                                                DataSet ds = new DataSet();
                                                SqlDataAdapter adapter = new SqlDataAdapter(comd);
                                                adapter.Fill(ds);
                                                if (ds.Tables.Count > 0)
                                                {
                                                    dvData = ds.Tables[0].DefaultView;
                                                }

                                                //获取公司期间所排班次
                                                DataView dvClassData = ServiceFactory.Factory.MakeClassDataService.QueryData(stores[i].ID, companyPeriods[p].ID).DefaultView;
                                                for (int e = 0; e < companyEmpes.Count; e++)
                                                {

                                                    //构建母表数据
                                                    AttendPeriodData periodData = new AttendPeriodData();
                                                    DataTable dtPeriodData = new DataTable();
                                                    for (int d = 1; d <= 31; d++)
                                                    {
                                                        dtPeriodData.Columns.Add("Data" + d, typeof(Int16));
                                                    }
                                                    DataRow drPeriodData = dtPeriodData.NewRow();

                                                    periodData.EmployeeID = companyEmpes[e].ID;
                                                    //获取员工对应排班信息
                                                    dvClassData.RowFilter = string.Empty;
                                                    dvClassData.RowFilter = "EmployeeID=" + companyEmpes[e].ID;
                                                    if (dvClassData.Count == 1)
                                                    {
                                                        int day = (Convert.ToDateTime(companyPeriods[p].EndDate) - Convert.ToDateTime(companyPeriods[p].StartDate)).Days;
                                                        int peroidDay = Convert.ToDateTime(companyPeriods[p].StartDate).Day;
                                                        for (int d = 1; d <= day; d++)
                                                        {
                                                            //只处理当前小于当前时间的数据
                                                            DateTime diposeTime = Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00"));
                                                            if (DateTime.Compare(DateTime.Now, diposeTime) > 0)
                                                            {

                                                                int ClassID = Convert.ToInt32(dvClassData[0]["ClassID" + d].ToString());

                                                                Models.Data.AttendData attend = new Models.Data.AttendData();
                                                                attend.EmployeeID = Convert.ToInt32(companyEmpes[e].ID);
                                                                attend.ClassID = ClassID;
                                                                attend.StoreID = Convert.ToInt32(stores[i].ID);
                                                                attend.DeptID = Convert.ToInt32(companyEmpes[e].DeptID);
                                                                attend.StartTime = "NULL";
                                                                attend.OverTime = "NULL";
                                                                attend.FactOverTime = "NULL";
                                                                attend.FactStartTime = "NULL";
                                                                Classes c = ServiceFactory.Factory.ClassesService.SearchOne(new SimpleCondition("ID", attend.ClassID));
                                                                if (c != null)
                                                                {
                                                                    attend.StartTime = "'" + Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00") + "  " + c.StartHour + ":" + c.StartMinute).ToString() + "'";
                                                                    attend.OverTime = "'" + Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00") + "  " + c.OverHour + ":" + c.OverMinute).ToString() + "'";
                                                                }


                                                                //获取当天所有打卡数据
                                                                dvData.RowFilter = string.Empty;
                                                                dvData.RowFilter = "EmployeeID=" + attend.EmployeeID;
                                                                string startTime = string.Empty, overTime = string.Empty;
                                                                DataTable dtDays = ds.Tables[0].Clone();
                                                                for (int a = 0; a < dvData.Count; a++)
                                                                {

                                                                    DateTime scanDate = Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00"));

                                                                    DateTime scanDayStart = Convert.ToDateTime(scanDate.ToShortDateString());
                                                                    DateTime scanDayEnd = Convert.ToDateTime(scanDate.AddDays(1).ToShortDateString()).AddSeconds(-1);
                                                                    if (DateTime.Compare(Convert.ToDateTime(dvData[a]["ScanTime"]), Convert.ToDateTime(scanDayStart)) > 0 && DateTime.Compare(Convert.ToDateTime(dvData[a]["ScanTime"]), Convert.ToDateTime(scanDayEnd)) < 0)
                                                                    {
                                                                        DataRow dr = dvData[a].Row;
                                                                        dtDays.ImportRow(dr);
                                                                    }
                                                                }
                                                                //获取最早的上班签到和最早的下班签到
                                                                //获取当天所有打卡记录
                                                                DataView dvDates = dtDays.DefaultView;

                                                                //获取当天所有上班打卡记录
                                                                dvDates.RowFilter = "ScanType=" + Convert.ToInt16(Enum_StartOrOver.Start);
                                                                if (dvDates.Count > 0)
                                                                {
                                                                    for (int h = 0; h < dvDates.Count; h++)
                                                                    {
                                                                        startTime = Convert.ToDateTime(dvDates[0]["ScanTime"]).ToString();
                                                                        attend.FactStartTime = "'" + startTime + "'";
                                                                        if (DateTime.Compare(Convert.ToDateTime(dvDates[h]["ScanTime"]), Convert.ToDateTime(startTime)) < 0)
                                                                        {
                                                                            startTime = Convert.ToDateTime(dvDates[h]["ScanTime"]).ToString();
                                                                            attend.FactStartTime = "'" + dvDates[h]["ScanTime"].ToString() + "'";
                                                                        }
                                                                    }
                                                                }


                                                                //获取当天所有下班打卡记录
                                                                dvDates.RowFilter = string.Empty;
                                                                dvDates.RowFilter = "ScanType=" + Convert.ToInt16(Enum_StartOrOver.Over);

                                                                if (dvDates.Count > 0)
                                                                {
                                                                    for (int h = 0; h < dvDates.Count; h++)
                                                                    {
                                                                        overTime = Convert.ToDateTime(dvDates[0]["ScanTime"]).ToString();
                                                                        attend.FactOverTime = "'" + overTime + "'";
                                                                        if (DateTime.Compare(Convert.ToDateTime(dvDates[h]["ScanTime"]), Convert.ToDateTime(overTime)) < 0)
                                                                        {
                                                                            overTime = Convert.ToDateTime(dvDates[h]["ScanTime"]).ToString();
                                                                            attend.FactOverTime = "'" + dvDates[h]["ScanTime"].ToString() + "'";
                                                                        }
                                                                    }
                                                                }



                                                                if (attend.StartTime != "NULL" && attend.FactStartTime != "NULL")
                                                                {
                                                                    DateTime factStartTime = Convert.ToDateTime(attend.FactStartTime.Replace("'", ""));
                                                                    DateTime StartTime = Convert.ToDateTime(attend.StartTime.Replace("'", ""));
                                                                    if (DateTime.Compare(factStartTime, StartTime) > 0)
                                                                    {
                                                                        try
                                                                        {
                                                                            TimeSpan nowtimespan = new TimeSpan(factStartTime.Ticks);
                                                                            TimeSpan endtimespan = new TimeSpan(StartTime.Ticks);
                                                                            TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                                                                            attend.BeLate = Convert.ToInt32(timespan.TotalMinutes);
                                                                        }
                                                                        catch
                                                                        {
                                                                            attend.BeLate = 0;
                                                                        }
                                                                    }
                                                                }
                                                                if (attend.OverTime != "NULL" && attend.FactOverTime != "NULL")
                                                                {
                                                                    DateTime factOverTime = Convert.ToDateTime(attend.FactOverTime.Replace("'", ""));
                                                                    DateTime OverTime = Convert.ToDateTime(attend.OverTime.Replace("'", ""));
                                                                    if (DateTime.Compare(OverTime, factOverTime) > 0)
                                                                    {
                                                                        try
                                                                        {
                                                                            TimeSpan nowtimespan = new TimeSpan(factOverTime.Ticks);
                                                                            TimeSpan endtimespan = new TimeSpan(OverTime.Ticks);
                                                                            TimeSpan timespan = endtimespan.Subtract(nowtimespan).Duration();
                                                                            attend.LeaveEarlier = Convert.ToInt32(timespan.TotalMinutes);
                                                                        }
                                                                        catch
                                                                        {
                                                                            attend.LeaveEarlier = 0;
                                                                        }
                                                                    }
                                                                }
                                                                attend.StayAway = true;

                                                                string disposeSql = "insert Month_" + storeUID + "_" + y.ToString("0000") + m.ToString("00") + "(StoreID,DeptID,EmployeeID,ClassID,StartTime,OverTime,FactStartTime,FactOverTime,BeLate,LeaveEarlier,StayAway) values(" + attend.StoreID + "," + attend.DeptID + "," + attend.EmployeeID + "," + attend.ClassID + "," + attend.StartTime + "," + attend.OverTime + "," + attend.FactStartTime + "," + attend.FactOverTime + "," + attend.BeLate + "," + attend.LeaveEarlier + ",'" + attend.StayAway + "')";
                                                                comd = new SqlCommand(disposeSql, conn);
                                                                n = comd.ExecuteNonQuery();

                                                                if (n > 0)
                                                                {
                                                                    drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Normal);
                                                                    if (attend.ClassID == 0)
                                                                    {
                                                                        //未排班有签到信息 -异常
                                                                        if (attend.FactStartTime != "NULL" || attend.FactOverTime != "NULL")
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Exception);
                                                                        //未排班无签到信息 -休息
                                                                        else
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Rest);
                                                                    }
                                                                    else if (attend.ClassID != 0)
                                                                    {
                                                                        //有排班没有签到信息 -旷工
                                                                        if (attend.FactStartTime == "NULL" && attend.FactOverTime == "NULL")
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Absenteeism);
                                                                        //有排班没有完整上下班信息 -异常
                                                                        else if (attend.FactStartTime == "NULL" || attend.FactOverTime == "NULL")
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Exception);
                                                                        //迟到+早退
                                                                        else if (attend.BeLate != 0 && attend.LeaveEarlier != 0)
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.LateAndLeave);
                                                                        //迟到
                                                                        else if (attend.BeLate != 0)
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Late);
                                                                        //早退
                                                                        else if (attend.LeaveEarlier != 0)
                                                                            drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.LeaveEarly);
                                                                    }

                                                                    //log.Info(DateTime.Now + "::::插入整理数据！员工ID:" + attend.EmployeeID + "\n\t");

                                                                }
                                                                //期间开始日期类推
                                                                peroidDay = Convert.ToDateTime(companyPeriods[p].StartDate).AddDays(d).Day;
                                                            }
                                                        }
                                                        dtPeriodData.Rows.Add(drPeriodData);
                                                        ConditionSet condition = new ConditionSet();
                                                        condition.Add(new SimpleCondition("EmployeeID", companyEmpes[e].ID));
                                                        condition.Add(new SimpleCondition("StoreID", stores[i].ID));
                                                        condition.Add(new SimpleCondition("PeroidID", companyPeriods[p].ID));

                                                        periodData.EmployeeID = companyEmpes[e].ID;
                                                        periodData.StoreID = stores[i].ID;
                                                        periodData.DeptID = companyEmpes[e].DeptID;
                                                        periodData.EmployeeName = companyEmpes[e].EmployeeName;
                                                        periodData.PeroidID = companyPeriods[p].ID;
                                                        periodData.StartDate = companyPeriods[p].StartDate;
                                                        periodData.EndDate = companyPeriods[p].EndDate;

                                                        AttendPeriodData old_entity = ServiceFactory.Factory.AttendPeriodDataService.SearchOne(condition);

                                                        if (old_entity == null)
                                                        {
                                                            if (dtPeriodData.Rows.Count == 1)
                                                            {
                                                                for (int d = 1; d <= 31; d++)
                                                                {
                                                                    if (dtPeriodData.Rows[0]["Data" + d].ToString() == "")
                                                                        dtPeriodData.Rows[0]["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Wait);
                                                                }
                                                                periodData.Data1 = Convert.ToInt32(dtPeriodData.Rows[0]["Data1"]);
                                                                periodData.Data2 = Convert.ToInt32(dtPeriodData.Rows[0]["Data2"]);
                                                                periodData.Data3 = Convert.ToInt32(dtPeriodData.Rows[0]["Data3"]);
                                                                periodData.Data4 = Convert.ToInt32(dtPeriodData.Rows[0]["Data4"]);
                                                                periodData.Data5 = Convert.ToInt32(dtPeriodData.Rows[0]["Data5"]);
                                                                periodData.Data6 = Convert.ToInt32(dtPeriodData.Rows[0]["Data6"]);
                                                                periodData.Data7 = Convert.ToInt32(dtPeriodData.Rows[0]["Data7"]);
                                                                periodData.Data8 = Convert.ToInt32(dtPeriodData.Rows[0]["Data8"]);
                                                                periodData.Data9 = Convert.ToInt32(dtPeriodData.Rows[0]["Data9"]);
                                                                periodData.Data10 = Convert.ToInt32(dtPeriodData.Rows[0]["Data10"]);

                                                                periodData.Data11 = Convert.ToInt32(dtPeriodData.Rows[0]["Data11"]);
                                                                periodData.Data12 = Convert.ToInt32(dtPeriodData.Rows[0]["Data12"]);
                                                                periodData.Data13 = Convert.ToInt32(dtPeriodData.Rows[0]["Data13"]);
                                                                periodData.Data14 = Convert.ToInt32(dtPeriodData.Rows[0]["Data14"]);
                                                                periodData.Data15 = Convert.ToInt32(dtPeriodData.Rows[0]["Data15"]);
                                                                periodData.Data16 = Convert.ToInt32(dtPeriodData.Rows[0]["Data16"]);
                                                                periodData.Data17 = Convert.ToInt32(dtPeriodData.Rows[0]["Data17"]);
                                                                periodData.Data18 = Convert.ToInt32(dtPeriodData.Rows[0]["Data18"]);
                                                                periodData.Data19 = Convert.ToInt32(dtPeriodData.Rows[0]["Data19"]);
                                                                periodData.Data20 = Convert.ToInt32(dtPeriodData.Rows[0]["Data20"]);

                                                                periodData.Data21 = Convert.ToInt32(dtPeriodData.Rows[0]["Data21"]);
                                                                periodData.Data22 = Convert.ToInt32(dtPeriodData.Rows[0]["Data22"]);
                                                                periodData.Data23 = Convert.ToInt32(dtPeriodData.Rows[0]["Data23"]);
                                                                periodData.Data24 = Convert.ToInt32(dtPeriodData.Rows[0]["Data24"]);
                                                                periodData.Data25 = Convert.ToInt32(dtPeriodData.Rows[0]["Data25"]);
                                                                periodData.Data26 = Convert.ToInt32(dtPeriodData.Rows[0]["Data26"]);
                                                                periodData.Data27 = Convert.ToInt32(dtPeriodData.Rows[0]["Data27"]);
                                                                periodData.Data28 = Convert.ToInt32(dtPeriodData.Rows[0]["Data28"]);
                                                                periodData.Data29 = Convert.ToInt32(dtPeriodData.Rows[0]["Data29"]);
                                                                periodData.Data30 = Convert.ToInt32(dtPeriodData.Rows[0]["Data30"]);
                                                                periodData.Data31 = Convert.ToInt32(dtPeriodData.Rows[0]["Data31"]);
                                                            }
                                                            if (DateTime.Compare(Convert.ToDateTime(periodData.StartDate), DateTime.Now) < 0)
                                                            {
                                                                ServiceFactory.Factory.AttendPeriodDataService.Insert(periodData);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (dtPeriodData.Rows.Count == 1)
                                                            {
                                                                for (int d = 1; d <= 31; d++)
                                                                {
                                                                    if (dtPeriodData.Rows[0]["Data" + d].ToString() == "")
                                                                        dtPeriodData.Rows[0]["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Wait);
                                                                }
                                                                old_entity.Data1 = Convert.ToInt32(dtPeriodData.Rows[0]["Data1"]);
                                                                old_entity.Data2 = Convert.ToInt32(dtPeriodData.Rows[0]["Data2"]);
                                                                old_entity.Data3 = Convert.ToInt32(dtPeriodData.Rows[0]["Data3"]);
                                                                old_entity.Data4 = Convert.ToInt32(dtPeriodData.Rows[0]["Data4"]);
                                                                old_entity.Data5 = Convert.ToInt32(dtPeriodData.Rows[0]["Data5"]);
                                                                old_entity.Data6 = Convert.ToInt32(dtPeriodData.Rows[0]["Data6"]);
                                                                old_entity.Data7 = Convert.ToInt32(dtPeriodData.Rows[0]["Data7"]);
                                                                old_entity.Data8 = Convert.ToInt32(dtPeriodData.Rows[0]["Data8"]);
                                                                old_entity.Data9 = Convert.ToInt32(dtPeriodData.Rows[0]["Data9"]);
                                                                old_entity.Data10 = Convert.ToInt32(dtPeriodData.Rows[0]["Data10"]);

                                                                old_entity.Data11 = Convert.ToInt32(dtPeriodData.Rows[0]["Data11"]);
                                                                old_entity.Data12 = Convert.ToInt32(dtPeriodData.Rows[0]["Data12"]);
                                                                old_entity.Data13 = Convert.ToInt32(dtPeriodData.Rows[0]["Data13"]);
                                                                old_entity.Data14 = Convert.ToInt32(dtPeriodData.Rows[0]["Data14"]);
                                                                old_entity.Data15 = Convert.ToInt32(dtPeriodData.Rows[0]["Data15"]);
                                                                old_entity.Data16 = Convert.ToInt32(dtPeriodData.Rows[0]["Data16"]);
                                                                old_entity.Data17 = Convert.ToInt32(dtPeriodData.Rows[0]["Data17"]);
                                                                old_entity.Data18 = Convert.ToInt32(dtPeriodData.Rows[0]["Data18"]);
                                                                old_entity.Data19 = Convert.ToInt32(dtPeriodData.Rows[0]["Data19"]);
                                                                old_entity.Data20 = Convert.ToInt32(dtPeriodData.Rows[0]["Data20"]);

                                                                old_entity.Data21 = Convert.ToInt32(dtPeriodData.Rows[0]["Data21"]);
                                                                old_entity.Data22 = Convert.ToInt32(dtPeriodData.Rows[0]["Data22"]);
                                                                old_entity.Data23 = Convert.ToInt32(dtPeriodData.Rows[0]["Data23"]);
                                                                old_entity.Data24 = Convert.ToInt32(dtPeriodData.Rows[0]["Data24"]);
                                                                old_entity.Data25 = Convert.ToInt32(dtPeriodData.Rows[0]["Data25"]);
                                                                old_entity.Data26 = Convert.ToInt32(dtPeriodData.Rows[0]["Data26"]);
                                                                old_entity.Data27 = Convert.ToInt32(dtPeriodData.Rows[0]["Data27"]);
                                                                old_entity.Data28 = Convert.ToInt32(dtPeriodData.Rows[0]["Data28"]);
                                                                old_entity.Data29 = Convert.ToInt32(dtPeriodData.Rows[0]["Data29"]);
                                                                old_entity.Data30 = Convert.ToInt32(dtPeriodData.Rows[0]["Data30"]);
                                                                old_entity.Data31 = Convert.ToInt32(dtPeriodData.Rows[0]["Data31"]);
                                                            }
                                                            if (DateTime.Compare(Convert.ToDateTime(periodData.StartDate), DateTime.Now) < 0)
                                                            {
                                                                ServiceFactory.Factory.AttendPeriodDataService.Update(old_entity);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //  log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + ",员工：" + companyEmpes[e].EmployeeName + ",期间：" + companyPeriods[p].PeriodName + ",排班数据不存在！\n\t");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + ",期间：" + companyPeriods[p].PeriodName + ",月份基础数据表不存在！\n\t");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + ",期间：" + companyPeriods[p].PeriodName + ",年份基础数据库不存在！\n\t");
                                    }
                                    #endregion
                                }

                            }
                            conn.Close();
                        }
                    }
                }
                //Thread.Sleep(ConfigSetting.Interval);
            }
            catch (Exception ex)
            {
                //log.Error(DateTime.Now + "::::出现错误：" + ex.Message + "！\n\t");
                //Thread.Sleep(1000 * 30);
            }
        }



        /// <summary>
        /// 处理基础打卡数据
        /// </summary>
        public void Data()
        {
            //获取所有公司
            List<Groups> groups = ServiceFactory.Factory.GroupsService.Search(null);
            //获取所有门店
            List<Stores> storesList = ServiceFactory.Factory.StoresService.Search(null);
            //获取获取所有期间
            List<Periods> periods = ServiceFactory.Factory.PeriodService.Search(null);
            //获取所有员工
            List<EmployeeBaseView> employees = ServiceFactory.Factory.EmployeeBaseService.Search(new SimpleCondition("State", Convert.ToInt16(Enum_EmployeeState.Pass)));
            //获取所有设置
            List<AttendParam> list = ServiceFactory.Factory.AttendParamService.Search(null);

            //log.Info(DateTime.Now + "::::开始整理数据表基础数据！\n\t");
            try
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    string companyCode = groups[i].UID.ToString();
                    List<AttendParam> attendParams = list.Where(p => p.GroupID == groups[i].ID).ToList();

                    AttendParam server = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.Server));
                    AttendParam userId = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserId));
                    AttendParam userPwd = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.UserPwd));
                    AttendParam storage = attendParams.SingleOrDefault(p => p.ParamCode == Convert.ToInt32(Enum_ParamType.StorageMode));
                    //获取公司对应期间
                    List<Periods> companyPeriods = periods.Where(p => p.GroupID == groups[i].ID).ToList();

                    List<Stores> stores = storesList.Where(s => s.GroupID == groups[i].ID).ToList();
                    for (int s = 0; s < stores.Count; s++)
                    {
                        string storeUID = stores[s].UID.ToString();
                        //获取公司对应正式员工
                        List<EmployeeBaseView> companyEmpes = employees.Where(e => e.StoreID == stores[s].ID).ToList();

                        if (server != null && userId != null && userPwd != null && storage != null)
                        {
                            using (SqlConnection conn = new SqlConnection("Data Source=" + server.ParamValue + ";Initial Catalog=master;User ID=" + userId.ParamValue + ";Password=" + userPwd.ParamValue))
                            {
                                conn.Open();

                                if (Convert.ToInt32(storage.ParamValue) == Convert.ToInt32(Enum_StorageType.Year))
                                {

                                    for (int p = 0; p < companyPeriods.Count; p++)
                                    {

                                        #region 整理对应期间

                                        int m = Convert.ToDateTime(companyPeriods[p].StartDate.ToString()).Month;
                                        int y = Convert.ToDateTime(companyPeriods[p].StartDate.ToString()).Year;
                                        //判断当前期间年份数据库是否存在
                                        SqlCommand comd = new SqlCommand("select count(*) from sys.databases where name='CaiMoMo_HR_" + companyCode + "_" + y.ToString("0000") + "'", conn);
                                        int n = int.Parse(comd.ExecuteScalar().ToString());
                                        string sql = string.Empty;
                                        if (n > 0)
                                        {
                                            //判断当前期间数据基础表是否存在
                                            sql = " use CaiMoMo_HR_" + companyCode + "_" + y.ToString("0000") + " "
                                      + " select COUNT(*) as Count from sysobjects where type='U' and name= 'Scan_" + storeUID + "_" + y.ToString("0000") + m.ToString("00") + "'";
                                            comd = new SqlCommand(sql, conn);
                                            SqlDataReader sda = comd.ExecuteReader();
                                            if (sda.Read())
                                                n = Convert.ToInt32(sda["Count"].ToString());
                                            sda.Close();
                                            if (n > 0)
                                            {
                                                //删除原处理表数据
                                                sql = " use CaiMoMo_HR_" + companyCode + "_" + y.ToString("0000") + " "
                                      + " delete Month_" + storeUID + "_" + y.ToString("0000") + m.ToString("00");
                                                comd = new SqlCommand(sql, conn);
                                                if (comd.ExecuteNonQuery() > -1)
                                                {
                                                    DataView dvData = new DataView();
                                                    //取出当前期间所有签到数据
                                                    sql = "select * from Scan_" + storeUID + "_" + DateTime.Now.Year + m.ToString("00");
                                                    comd = new SqlCommand(sql, conn);

                                                    DataSet ds = new DataSet();
                                                    SqlDataAdapter adapter = new SqlDataAdapter(comd);
                                                    adapter.Fill(ds);
                                                    if (ds.Tables.Count > 0)
                                                    {
                                                        dvData = ds.Tables[0].DefaultView;
                                                    }

                                                    //获取公司期间所排班次
                                                    DataView dvClassData = ServiceFactory.Factory.MakeClassDataService.QueryData(stores[s].ID, companyPeriods[p].ID).DefaultView;
                                                    for (int e = 0; e < companyEmpes.Count; e++)
                                                    {

                                                        //构建母表数据
                                                        AttendPeriodData periodData = new AttendPeriodData();
                                                        DataTable dtPeriodData = new DataTable();
                                                        for (int d = 1; d <= 31; d++)
                                                        {
                                                            dtPeriodData.Columns.Add("Data" + d, typeof(Int16));
                                                        }
                                                        DataRow drPeriodData = dtPeriodData.NewRow();

                                                        periodData.EmployeeID = companyEmpes[e].ID;
                                                        //获取员工对应排班信息
                                                        dvClassData.RowFilter = string.Empty;
                                                        dvClassData.RowFilter = "EmployeeID=" + companyEmpes[e].ID;
                                                        if (dvClassData.Count == 1)
                                                        {
                                                            int day = (Convert.ToDateTime(companyPeriods[p].EndDate) - Convert.ToDateTime(companyPeriods[p].StartDate)).Days;
                                                            int peroidDay = Convert.ToDateTime(companyPeriods[p].StartDate).Day;
                                                            for (int d = 1; d <= day; d++)
                                                            {
                                                                //只处理当前小于当前时间的数据
                                                                DateTime diposeTime = Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00"));
                                                                if (DateTime.Compare(DateTime.Now, diposeTime) > 0)
                                                                {

                                                                    int ClassID = Convert.ToInt32(dvClassData[0]["ClassID" + d].ToString());

                                                                    Models.Data.AttendData attend = new Models.Data.AttendData();
                                                                    attend.EmployeeID = Convert.ToInt32(companyEmpes[e].ID);
                                                                    attend.ClassID = ClassID;
                                                                    attend.StoreID = Convert.ToInt32(stores[s].ID);
                                                                    attend.DeptID = Convert.ToInt32(companyEmpes[e].DeptID);
                                                                    attend.StartTime = "NULL";
                                                                    attend.OverTime = "NULL";
                                                                    attend.FactOverTime = "NULL";
                                                                    attend.FactStartTime = "NULL";
                                                                    Classes c = ServiceFactory.Factory.ClassesService.SearchOne(new SimpleCondition("ID", attend.ClassID));
                                                                    if (c != null)
                                                                    {
                                                                        attend.StartTime = "'" + Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00") + "  " + c.StartHour + ":" + c.StartMinute).ToString() + "'";
                                                                        attend.OverTime = "'" + Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00") + "  " + c.OverHour + ":" + c.OverMinute).ToString() + "'";
                                                                    }


                                                                    //获取当天所有打卡数据
                                                                    dvData.RowFilter = string.Empty;
                                                                    dvData.RowFilter = "EmployeeID=" + attend.EmployeeID;
                                                                    string startTime = string.Empty, overTime = string.Empty;
                                                                    DataTable dtDays = ds.Tables[0].Clone();
                                                                    for (int a = 0; a < dvData.Count; a++)
                                                                    {

                                                                        DateTime scanDate = Convert.ToDateTime(y.ToString("0000") + "/" + m.ToString("00") + "/" + peroidDay.ToString("00"));

                                                                        DateTime scanDayStart = Convert.ToDateTime(scanDate.ToShortDateString());
                                                                        DateTime scanDayEnd = Convert.ToDateTime(scanDate.AddDays(1).ToShortDateString()).AddSeconds(-1);
                                                                        if (DateTime.Compare(Convert.ToDateTime(dvData[a]["ScanTime"]), Convert.ToDateTime(scanDayStart)) > 0 && DateTime.Compare(Convert.ToDateTime(dvData[a]["ScanTime"]), Convert.ToDateTime(scanDayEnd)) < 0)
                                                                        {
                                                                            DataRow dr = dvData[a].Row;
                                                                            dtDays.ImportRow(dr);
                                                                        }
                                                                    }
                                                                    //获取最早的上班签到和最早的下班签到
                                                                    //获取当天所有打卡记录
                                                                    DataView dvDates = dtDays.DefaultView;

                                                                    //获取当天所有上班打卡记录
                                                                    dvDates.RowFilter = "ScanType=" + Convert.ToInt16(Enum_StartOrOver.Start);
                                                                    if (dvDates.Count > 0)
                                                                    {
                                                                        for (int h = 0; h < dvDates.Count; h++)
                                                                        {
                                                                            startTime = Convert.ToDateTime(dvDates[0]["ScanTime"]).ToString();
                                                                            attend.FactStartTime = "'" + startTime + "'";
                                                                            if (DateTime.Compare(Convert.ToDateTime(dvDates[h]["ScanTime"]), Convert.ToDateTime(startTime)) < 0)
                                                                            {
                                                                                startTime = Convert.ToDateTime(dvDates[h]["ScanTime"]).ToString();
                                                                                attend.FactStartTime = "'" + dvDates[h]["ScanTime"].ToString() + "'";
                                                                            }
                                                                        }
                                                                    }


                                                                    //获取当天所有下班打卡记录
                                                                    dvDates.RowFilter = string.Empty;
                                                                    dvDates.RowFilter = "ScanType=" + Convert.ToInt16(Enum_StartOrOver.Over);

                                                                    if (dvDates.Count > 0)
                                                                    {
                                                                        for (int h = 0; h < dvDates.Count; h++)
                                                                        {
                                                                            overTime = Convert.ToDateTime(dvDates[0]["ScanTime"]).ToString();
                                                                            attend.FactOverTime = "'" + overTime + "'";
                                                                            if (DateTime.Compare(Convert.ToDateTime(dvDates[h]["ScanTime"]), Convert.ToDateTime(overTime)) < 0)
                                                                            {
                                                                                overTime = Convert.ToDateTime(dvDates[h]["ScanTime"]).ToString();
                                                                                attend.FactOverTime = "'" + dvDates[h]["ScanTime"].ToString() + "'";
                                                                            }
                                                                        }
                                                                    }



                                                                    if (attend.StartTime != "NULL" && attend.FactStartTime != "NULL")
                                                                    {
                                                                        DateTime factStartTime = Convert.ToDateTime(attend.FactStartTime.Replace("'", ""));
                                                                        DateTime StartTime = Convert.ToDateTime(attend.StartTime.Replace("'", ""));
                                                                        if (DateTime.Compare(factStartTime, StartTime) > 0)
                                                                        {
                                                                            try
                                                                            {
                                                                                TimeSpan nowtimespan = new TimeSpan(factStartTime.Ticks);
                                                                                TimeSpan endtimespan = new TimeSpan(StartTime.Ticks);
                                                                                TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                                                                                attend.BeLate = Convert.ToInt32(timespan.TotalMinutes);
                                                                            }
                                                                            catch
                                                                            {
                                                                                attend.BeLate = 0;
                                                                            }
                                                                        }
                                                                    }
                                                                    if (attend.OverTime != "NULL" && attend.FactOverTime != "NULL")
                                                                    {
                                                                        DateTime factOverTime = Convert.ToDateTime(attend.FactOverTime.Replace("'", ""));
                                                                        DateTime OverTime = Convert.ToDateTime(attend.OverTime.Replace("'", ""));
                                                                        if (DateTime.Compare(OverTime, factOverTime) > 0)
                                                                        {
                                                                            try
                                                                            {
                                                                                TimeSpan nowtimespan = new TimeSpan(factOverTime.Ticks);
                                                                                TimeSpan endtimespan = new TimeSpan(OverTime.Ticks);
                                                                                TimeSpan timespan = endtimespan.Subtract(nowtimespan).Duration();
                                                                                attend.LeaveEarlier = Convert.ToInt32(timespan.TotalMinutes);
                                                                            }
                                                                            catch
                                                                            {
                                                                                attend.LeaveEarlier = 0;
                                                                            }
                                                                        }
                                                                    }
                                                                    attend.StayAway = true;

                                                                    string disposeSql = "insert Month_" + storeUID + "_" + y.ToString("0000") + m.ToString("00") + "(StoreID,DeptID,EmployeeID,ClassID,StartTime,OverTime,FactStartTime,FactOverTime,BeLate,LeaveEarlier,StayAway) values(" + attend.StoreID + "," + attend.DeptID + "," + attend.EmployeeID + "," + attend.ClassID + "," + attend.StartTime + "," + attend.OverTime + "," + attend.FactStartTime + "," + attend.FactOverTime + "," + attend.BeLate + "," + attend.LeaveEarlier + ",'" + attend.StayAway + "')";
                                                                    comd = new SqlCommand(disposeSql, conn);
                                                                    n = comd.ExecuteNonQuery();

                                                                    if (n > 0)
                                                                    {
                                                                        drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Normal);
                                                                        if (attend.ClassID == 0)
                                                                        {
                                                                            //未排班有签到信息 -异常
                                                                            if (attend.FactStartTime != "NULL" || attend.FactOverTime != "NULL")
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Exception);
                                                                            //未排班无签到信息 -休息
                                                                            else
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Rest);
                                                                        }
                                                                        else if (attend.ClassID != 0)
                                                                        {
                                                                            //有排班没有签到信息 -旷工
                                                                            if (attend.FactStartTime == "NULL" && attend.FactOverTime == "NULL")
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Absenteeism);
                                                                            //有排班没有完整上下班信息 -异常
                                                                            else if (attend.FactStartTime == "NULL" || attend.FactOverTime == "NULL")
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Exception);
                                                                            //迟到+早退
                                                                            else if (attend.BeLate != 0 && attend.LeaveEarlier != 0)
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.LateAndLeave);
                                                                            //迟到
                                                                            else if (attend.BeLate != 0)
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Late);
                                                                            //早退
                                                                            else if (attend.LeaveEarlier != 0)
                                                                                drPeriodData["Data" + d] = Convert.ToInt16(Enum_AttendStatus.LeaveEarly);
                                                                        }

                                                                        //log.Info(DateTime.Now + "::::插入整理数据！员工ID:" + attend.EmployeeID + "\n\t");

                                                                    }
                                                                    //期间开始日期类推
                                                                    peroidDay = Convert.ToDateTime(companyPeriods[p].StartDate).AddDays(d).Day;
                                                                }
                                                            }
                                                            dtPeriodData.Rows.Add(drPeriodData);
                                                            ConditionSet condition = new ConditionSet();
                                                            condition.Add(new SimpleCondition("EmployeeID", companyEmpes[e].ID));
                                                            condition.Add(new SimpleCondition("StoreID", stores[s].ID));
                                                            condition.Add(new SimpleCondition("PeroidID", companyPeriods[p].ID));

                                                            periodData.EmployeeID = companyEmpes[e].ID;
                                                            periodData.StoreID = stores[s].ID;
                                                            periodData.DeptID = companyEmpes[e].DeptID;
                                                            periodData.EmployeeName = companyEmpes[e].EmployeeName;
                                                            periodData.PeroidID = companyPeriods[p].ID;
                                                            periodData.StartDate = companyPeriods[p].StartDate;
                                                            periodData.EndDate = companyPeriods[p].EndDate;

                                                            AttendPeriodData old_entity = ServiceFactory.Factory.AttendPeriodDataService.SearchOne(condition);

                                                            if (old_entity == null)
                                                            {
                                                                if (dtPeriodData.Rows.Count == 1)
                                                                {
                                                                    for (int d = 1; d <= 31; d++)
                                                                    {
                                                                        if (dtPeriodData.Rows[0]["Data" + d].ToString() == "")
                                                                            dtPeriodData.Rows[0]["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Wait);
                                                                    }
                                                                    periodData.Data1 = Convert.ToInt32(dtPeriodData.Rows[0]["Data1"]);
                                                                    periodData.Data2 = Convert.ToInt32(dtPeriodData.Rows[0]["Data2"]);
                                                                    periodData.Data3 = Convert.ToInt32(dtPeriodData.Rows[0]["Data3"]);
                                                                    periodData.Data4 = Convert.ToInt32(dtPeriodData.Rows[0]["Data4"]);
                                                                    periodData.Data5 = Convert.ToInt32(dtPeriodData.Rows[0]["Data5"]);
                                                                    periodData.Data6 = Convert.ToInt32(dtPeriodData.Rows[0]["Data6"]);
                                                                    periodData.Data7 = Convert.ToInt32(dtPeriodData.Rows[0]["Data7"]);
                                                                    periodData.Data8 = Convert.ToInt32(dtPeriodData.Rows[0]["Data8"]);
                                                                    periodData.Data9 = Convert.ToInt32(dtPeriodData.Rows[0]["Data9"]);
                                                                    periodData.Data10 = Convert.ToInt32(dtPeriodData.Rows[0]["Data10"]);

                                                                    periodData.Data11 = Convert.ToInt32(dtPeriodData.Rows[0]["Data11"]);
                                                                    periodData.Data12 = Convert.ToInt32(dtPeriodData.Rows[0]["Data12"]);
                                                                    periodData.Data13 = Convert.ToInt32(dtPeriodData.Rows[0]["Data13"]);
                                                                    periodData.Data14 = Convert.ToInt32(dtPeriodData.Rows[0]["Data14"]);
                                                                    periodData.Data15 = Convert.ToInt32(dtPeriodData.Rows[0]["Data15"]);
                                                                    periodData.Data16 = Convert.ToInt32(dtPeriodData.Rows[0]["Data16"]);
                                                                    periodData.Data17 = Convert.ToInt32(dtPeriodData.Rows[0]["Data17"]);
                                                                    periodData.Data18 = Convert.ToInt32(dtPeriodData.Rows[0]["Data18"]);
                                                                    periodData.Data19 = Convert.ToInt32(dtPeriodData.Rows[0]["Data19"]);
                                                                    periodData.Data20 = Convert.ToInt32(dtPeriodData.Rows[0]["Data20"]);

                                                                    periodData.Data21 = Convert.ToInt32(dtPeriodData.Rows[0]["Data21"]);
                                                                    periodData.Data22 = Convert.ToInt32(dtPeriodData.Rows[0]["Data22"]);
                                                                    periodData.Data23 = Convert.ToInt32(dtPeriodData.Rows[0]["Data23"]);
                                                                    periodData.Data24 = Convert.ToInt32(dtPeriodData.Rows[0]["Data24"]);
                                                                    periodData.Data25 = Convert.ToInt32(dtPeriodData.Rows[0]["Data25"]);
                                                                    periodData.Data26 = Convert.ToInt32(dtPeriodData.Rows[0]["Data26"]);
                                                                    periodData.Data27 = Convert.ToInt32(dtPeriodData.Rows[0]["Data27"]);
                                                                    periodData.Data28 = Convert.ToInt32(dtPeriodData.Rows[0]["Data28"]);
                                                                    periodData.Data29 = Convert.ToInt32(dtPeriodData.Rows[0]["Data29"]);
                                                                    periodData.Data30 = Convert.ToInt32(dtPeriodData.Rows[0]["Data30"]);
                                                                    periodData.Data31 = Convert.ToInt32(dtPeriodData.Rows[0]["Data31"]);
                                                                }
                                                                if (DateTime.Compare(Convert.ToDateTime(periodData.StartDate), DateTime.Now) < 0)
                                                                {
                                                                    ServiceFactory.Factory.AttendPeriodDataService.Insert(periodData);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dtPeriodData.Rows.Count == 1)
                                                                {
                                                                    for (int d = 1; d <= 31; d++)
                                                                    {
                                                                        if (dtPeriodData.Rows[0]["Data" + d].ToString() == "")
                                                                            dtPeriodData.Rows[0]["Data" + d] = Convert.ToInt16(Enum_AttendStatus.Wait);
                                                                    }
                                                                    old_entity.Data1 = Convert.ToInt32(dtPeriodData.Rows[0]["Data1"]);
                                                                    old_entity.Data2 = Convert.ToInt32(dtPeriodData.Rows[0]["Data2"]);
                                                                    old_entity.Data3 = Convert.ToInt32(dtPeriodData.Rows[0]["Data3"]);
                                                                    old_entity.Data4 = Convert.ToInt32(dtPeriodData.Rows[0]["Data4"]);
                                                                    old_entity.Data5 = Convert.ToInt32(dtPeriodData.Rows[0]["Data5"]);
                                                                    old_entity.Data6 = Convert.ToInt32(dtPeriodData.Rows[0]["Data6"]);
                                                                    old_entity.Data7 = Convert.ToInt32(dtPeriodData.Rows[0]["Data7"]);
                                                                    old_entity.Data8 = Convert.ToInt32(dtPeriodData.Rows[0]["Data8"]);
                                                                    old_entity.Data9 = Convert.ToInt32(dtPeriodData.Rows[0]["Data9"]);
                                                                    old_entity.Data10 = Convert.ToInt32(dtPeriodData.Rows[0]["Data10"]);

                                                                    old_entity.Data11 = Convert.ToInt32(dtPeriodData.Rows[0]["Data11"]);
                                                                    old_entity.Data12 = Convert.ToInt32(dtPeriodData.Rows[0]["Data12"]);
                                                                    old_entity.Data13 = Convert.ToInt32(dtPeriodData.Rows[0]["Data13"]);
                                                                    old_entity.Data14 = Convert.ToInt32(dtPeriodData.Rows[0]["Data14"]);
                                                                    old_entity.Data15 = Convert.ToInt32(dtPeriodData.Rows[0]["Data15"]);
                                                                    old_entity.Data16 = Convert.ToInt32(dtPeriodData.Rows[0]["Data16"]);
                                                                    old_entity.Data17 = Convert.ToInt32(dtPeriodData.Rows[0]["Data17"]);
                                                                    old_entity.Data18 = Convert.ToInt32(dtPeriodData.Rows[0]["Data18"]);
                                                                    old_entity.Data19 = Convert.ToInt32(dtPeriodData.Rows[0]["Data19"]);
                                                                    old_entity.Data20 = Convert.ToInt32(dtPeriodData.Rows[0]["Data20"]);

                                                                    old_entity.Data21 = Convert.ToInt32(dtPeriodData.Rows[0]["Data21"]);
                                                                    old_entity.Data22 = Convert.ToInt32(dtPeriodData.Rows[0]["Data22"]);
                                                                    old_entity.Data23 = Convert.ToInt32(dtPeriodData.Rows[0]["Data23"]);
                                                                    old_entity.Data24 = Convert.ToInt32(dtPeriodData.Rows[0]["Data24"]);
                                                                    old_entity.Data25 = Convert.ToInt32(dtPeriodData.Rows[0]["Data25"]);
                                                                    old_entity.Data26 = Convert.ToInt32(dtPeriodData.Rows[0]["Data26"]);
                                                                    old_entity.Data27 = Convert.ToInt32(dtPeriodData.Rows[0]["Data27"]);
                                                                    old_entity.Data28 = Convert.ToInt32(dtPeriodData.Rows[0]["Data28"]);
                                                                    old_entity.Data29 = Convert.ToInt32(dtPeriodData.Rows[0]["Data29"]);
                                                                    old_entity.Data30 = Convert.ToInt32(dtPeriodData.Rows[0]["Data30"]);
                                                                    old_entity.Data31 = Convert.ToInt32(dtPeriodData.Rows[0]["Data31"]);
                                                                }
                                                                if (DateTime.Compare(Convert.ToDateTime(periodData.StartDate), DateTime.Now) < 0)
                                                                {
                                                                    ServiceFactory.Factory.AttendPeriodDataService.Update(old_entity);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //  log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + ",员工：" + companyEmpes[e].EmployeeName + ",期间：" + companyPeriods[p].PeriodName + ",排班数据不存在！\n\t");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + ",期间：" + companyPeriods[p].PeriodName + ",月份基础数据表不存在！\n\t");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //log.Error(DateTime.Now + "::::公司：" + companys[i].GroupName + ",期间：" + companyPeriods[p].PeriodName + ",年份基础数据库不存在！\n\t");
                                        }
                                        #endregion
                                    }

                                }
                                conn.Close();
                            }
                        }
                    }
                }
                //Thread.Sleep(ConfigSetting.Interval);
            }
            catch (Exception ex)
            {
                //log.Error(DateTime.Now + "::::出现错误：" + ex.Message + "！\n\t");
                //Thread.Sleep(1000 * 30);
            }
        }
    }
}
