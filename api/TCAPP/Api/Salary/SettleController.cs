using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Newtonsoft.Json.Linq;
using Utilities;
using Adapters;
using System.Data;

namespace HrApp.Api
{
    public class SettleController : Controller
    {
        public JsonResult Query(string storeId)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                JArray array = new TreeGridAdapter().GetSalaryPeroid(Convert.ToInt16(storeId), Convert.ToInt16(Enum_AssessState.Pass));
                json.Data = JsonUtil.GetSuccessForObject(array);


            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult Submit(SalarySettle entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                ISalarySettleService service = ServiceFactory.Factory.SalarySettleService;
                SalarySettle settle = new SalarySettle();
                settle.CompanyID = entity.CompanyID;
                settle.PeroidID = entity.PeroidID;
                settle.StartDate = entity.StartDate;
                settle.EndDate = entity.EndDate;
                settle.TotalAmout = entity.TotalAmout;
                settle.DeductAmount = entity.DeductAmount;
                settle.SettleAmount = entity.SettleAmount;
                settle.HasSettled = true;

                settle.CreateTime = DateTime.Now;
                settle.Creator = Utilities.Security.CurrentUser.UserName;
                settle.CreateIP = Network.ClientIP;
                settle.UpdateTime = DateTime.Now;
                settle.Updater = Utilities.Security.CurrentUser.UserName;
                settle.UpdateIP = Network.ClientIP;
                service.Insert(settle);

                json.Data = JsonUtil.GetSuccessForString("结算完成");


            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

    }
}