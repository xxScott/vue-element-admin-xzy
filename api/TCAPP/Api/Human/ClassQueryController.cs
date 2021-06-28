using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Utilities;

namespace HrApp.Api.Human
{
    public class ClassQueryController : Controller
    {
        public JsonResult Query(string keyword)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                IMakeClassDataService service = ServiceFactory.Factory.MakeClassDataService;
                DataTable dt = service.QueryAll(keyword);

                DataView data = dt.Copy().DefaultView;

                DataView dv = dt.DefaultView;
                dv.RowFilter = "UpperID is not null";



                DataTable dat = new DataTable();
                dat.Columns.Add("ID", typeof(Int32));
                dat.Columns.Add("UpperID", typeof(Int32));
                dat.Columns.Add("EmployeeName", typeof(string));
                dat.Columns.Add("EmployeeID", typeof(Int32));
                dat.Columns.Add("PeriodName", typeof(string));
                dat.Columns.Add("DeptName", typeof(string));
                dat.Columns.Add("State", typeof(Int32));
                for (int i = 1; i <= 31; i++)
                {
                    dat.Columns.Add("ClassID" + i, typeof(string));
                }

                data.RowFilter = "UpperID is Null ";

                for (int i = 0; i < data.Count; i++)
                {

                    DataRow row = dat.NewRow();
                    row["ID"] = data[i]["ID"];
                    row["UpperID"] = data[i]["UpperID"];
                    row["EmployeeName"] = data[i]["EmployeeName"];
                    row["EmployeeID"] = data[i]["EmployeeID"];
                    row["PeriodName"] = data[i]["PeriodName"];
                    row["DeptName"] = data[i]["DeptName"];
                 
                    row["State"] = data[i]["State"];
                    for (int j = 1; j <= 31; j++)
                    {
                        row["ClassID" + j] = data[i]["ClassID" + j];
                    }


                    if (data[i]["ID"].ToString() != "")
                    {
                        dv.RowFilter = "UpperID=" + data[i]["ID"];
                        string colum = "";
                        if (dv.Count == 1)
                        {
                            for (int j = 1; j <= 31; j++)
                            {
                                if (Convert.ToInt32(dv[0]["ClassID" + j]) != 0)
                                    colum += "ClassID" + j + "|";
                            }
                            if (colum != "") colum = colum.Substring(0, colum.Length - 1);
                            string[] colums = colum.Split('|');
                            for (int j = 0; j < colums.Length; j++)
                            {
                                row[colums[j].ToString()] += "|" + dv[0][colums[j].ToString()];
                            }
                        }
                    }

                    dat.Rows.Add(row);
                }

                json.Data = JsonUtil.GetSuccessForObject(dat);
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult QueryClass()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IClassesService service = ServiceFactory.Factory.ClassesService;
                List<ClassesView> classes = service.Search(null);
                json.Data = JsonUtil.GetSuccessForObject(classes);

            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

    }
}