using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DAL;
using DAL.Business;
using MyOrm.Common;
using Utilities;
namespace CaiMoMo
{
    public class UsersLib
    {
        public static bool Login(int uid, string userId, string pwd, DAL.Groups outGroup, ref string message)
        {
            message = string.Empty;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CaiMoMoConnection"].ToString()))
            {

                conn.Open();
                string sql = "select * from dbo.SysGroupUser where UserID='" + userId + "' and StoreID='" + uid.ToString() + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader sda = command.ExecuteReader();

                if (sda.Read())
                {
                    if (sda["PasswordBak"].ToString() == pwd.ToString())
                    {

                        message = "登录成功";

                        //更新集团数据
                        DataView dv = GroupsLib.Query(Convert.ToInt32(sda["GroupID"])).DefaultView;
                        DAL.Groups group = new DAL.Groups();
                        
                        if (dv.Count == 1)
                        {
                            group.UID = Convert.ToInt32(dv[0]["UID"]);
                            group.GroupName = dv[0]["GroupName"].ToString();
                            group.Province = dv[0]["Province"].ToString();
                            group.City = dv[0]["City"].ToString();
                            group.Address = dv[0]["Address"].ToString();
                            group.ContactName = dv[0]["ContactName"].ToString();
                            group.Telephone = dv[0]["Telephone"].ToString();
                        }

                        //更新门店数据
                        DataView dvStores = new DataView();
                        List<Stores> stores = new List<Stores>();

                        if (sda["StoreID"].ToString().Substring(sda["StoreID"].ToString().Length - 2, 2) == "01")
                        {
                            dvStores = StoresLib.Query(Convert.ToInt32(sda["GroupID"])).DefaultView;
                        }
                        else
                        {
                            dvStores = StoresLib.Get(Convert.ToInt32(sda["StoreID"])).DefaultView;

                        }
                        for (int i = 0; i < dvStores.Count; i++)
                        {
                            DAL.Stores store = new DAL.Stores();
                            store.UID = Convert.ToInt32(dvStores[i]["UID"]);
                            store.GroupID = Convert.ToInt32(sda["GroupID"]);
                            store.StoreName = dvStores[i]["StoreName"].ToString();
                            store.Province = dvStores[i]["Province"].ToString();
                            store.City = dvStores[i]["City"].ToString();
                            store.Address = dvStores[i]["Address"].ToString();
                            store.ContactName = dvStores[i]["ContactName"].ToString();
                            stores.Add(store);
                        }

                        //更新用户数据
                        DAL.Users user = new Users();

                        user.UID = sda["UID"].ToString();
                        user.UserName = sda["TrueName"].ToString();
                        user.UserID = sda["UserID"].ToString();
                        user.Password = Encrypt.Instance.EncryptString(sda["PasswordBak"].ToString());
                        user.QQ = sda["QQ"].ToString();
                        user.WXID = sda["WXID"].ToString();
                        user.ShouQuanCode = sda["ShouQuanCode"].ToString();


                        if (sda["GroupID"].ToString() != "" && sda["StoreID"].ToString() != "")
                        {
                            user.StoreID = Convert.ToInt32(sda["StoreID"]);
                            user.GroupID = Convert.ToInt32(sda["GroupID"]);


                            //yangyc 需求于20190426，团餐后台只有一个默认角色，即系统管理员
                            ServiceFactory.Factory.UsersService.InsertCaiMoMo(null, stores, user, Utilities.Strings.RoleCode_Admin);
                            //if (sda["StoreID"].ToString().Substring(sda["StoreID"].ToString().Length - 2, 2) == "01")
                            //{
                            //    //user.GroupID = -1;//hjt 门店用户也要GroupID
                            //    ServiceFactory.Factory.UsersService.InsertCaiMoMo(null, stores, user, Utilities.Strings.RoleCode_OnlyStore);

                            //}
                            //else if (sda["StoreID"].ToString().Substring(sda["StoreID"].ToString().Length - 2, 2) == "00")
                            //{
                            //    ServiceFactory.Factory.UsersService.InsertCaiMoMo(group, stores, user, Utilities.Strings.RoleCode_Group);
                            //}
                            //else
                            //{
                            //    ServiceFactory.Factory.UsersService.InsertCaiMoMo(group, stores, user, Utilities.Strings.RoleCode_Store);
                            //}

                            outGroup.UID = group.UID;
                        }

                    }
                    else
                    {
                        message = "密码错误";
                        return false;
                    }
                }
                else
                {
                    message = "登录用户不存在";
                    return false;
                }
                conn.Close();
            }
            return true;
        }
    }
}
