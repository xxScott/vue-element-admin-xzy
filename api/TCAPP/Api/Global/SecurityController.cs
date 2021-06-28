using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Business;
using Models;
using MyOrm.Common;
using Utilities;
using Com.Caimomo.Entity;

namespace HrApp.Api
{
    public class SecurityController : Controller
    {

        public JsonResult LoginAdmin(Models.Data.LoginUserData u)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                string message = string.Empty;
                if (!Security.Login(u.UserID, Utilities.Encrypt.Instance.EncryptString(u.Password), ref message))
                    json.Data = JsonUtil.GetFailForString(message);
                else
                    json.Data = JsonUtil.GetSuccess();
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult LoginCaiMoMo(Models.Data.LoginUserData u)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                string message = string.Empty;
                DAL.Groups outGroup = new Groups();
                if (!CaiMoMo.UsersLib.Login(Convert.ToInt32(u.UID), u.UserID, u.Password, outGroup, ref message))
                    json.Data = JsonUtil.GetFailForString(message);
                else
                {
                    int groupId=0;
                    if (outGroup != null && outGroup.UID.HasValue)
                    {
                        groupId = outGroup.UID.Value;
                    }
                    using (EasyDataContext context = MyDataContextFactory.GetDataContext<EasyDataContext>(MyDataContextFactory.CanYinCenter))
                    {
                        var query = from su in context.SysGroupUser
                                    where su.IsEnable == true && su.GroupID== groupId
                                    select new
                                    {
                                        StoreID = su.StoreID
                                    };

                        //是否集团账号
                        Utilities.Security.HaveGroupAccount = false;
                        foreach (var item in query)
                        {
                            if (item.StoreID.ToString().EndsWith("00"))
                            {
                                Utilities.Security.HaveGroupAccount = true;
                                break;
                            }
                        }
                    }

                    Login(u);
                    json.Data = JsonUtil.GetSuccess();
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }


        public JsonResult Login(Models.Data.LoginUserData u)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                string message = string.Empty;
                if (!Security.Login(u.UserID, Utilities.Encrypt.Instance.EncryptString(u.Password), u.UID.ToString(), ref message))
                    json.Data = JsonUtil.GetFailForString(message);
                else
                    json.Data = JsonUtil.GetSuccess();
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }



        public JsonResult ChangePwd(Users u)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                IUsersService service = ServiceFactory.Factory.UsersService;
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("ID", u.ID));
                //查询数据
                Users user = service.SearchOne(condition);
                if (user != null)
                {
                    user.Password = Encrypt.Instance.EncryptString(u.Password);
                    service.Update(user);
                    json.Data = JsonUtil.GetSuccessForString("密码修改已完成");
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }

        public JsonResult Logout()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                Utilities.Security.Logout();
                json.Data = JsonUtil.GetSuccess();
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);

            }
            return json;
        }
    }
}