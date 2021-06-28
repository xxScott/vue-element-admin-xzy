using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Utilities;
using DAL;
using System.Drawing;

namespace HrApp.Api
{
    public class FileController:Controller
    {
        public JsonResult Upload()
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                
                HttpContext.Response.ContentType = "text/html";
                System.Web.HttpFileCollectionBase files = HttpContext.Request.Files;


                Users currentUser = Utilities.Security.CurrentUser;
                int? storeid = currentUser.StoreID;
                int? groudid = currentUser.GroupID;


                if (files.Count >0)
                {
                    string save_url = "/Uploads/" + groudid + "/" + storeid + "/" + DateTime.Now.ToString("yyyyMMdd");
                    string save_path = HttpContext.Server.MapPath(save_url);
                    if (!Directory.Exists(save_path)) Directory.CreateDirectory(save_path);

                    //string file_name = Path.GetFileName(files[0].FileName);
                    string file_name = DateTime.Now.ToString("yyyyMMddHHmmssms") + "_" + Path.GetFileName(files[0].FileName);
                    string url = save_url + "/" + file_name;
                    string save_name = HttpContext.Server.MapPath(url);
                    if (System.IO.File.Exists(save_name)) System.IO.File.Delete(save_name);
                    files[0].SaveAs(save_name);

                    json.Data = JsonUtil.GetSuccessForString(url);
                }
                else
                {
                    json.Data = JsonUtil.GetFail(DAL.Enum_StatusCode.Error_FileUploadFailed);
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message+ex.StackTrace);

            }
            return json;
        }
        
    }

    public class FileCardController : Controller
    {
        public JsonResult Upload(Models.Data.EntryDataView entity)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {

                HttpContext.Response.ContentType = "text/html";
                string iconbase64 = entity.EmployeePic;

                Users currentUser = Utilities.Security.CurrentUser;
                int? storeid = currentUser.StoreID;
                int? groudid = currentUser.GroupID;


                if ( !string.IsNullOrEmpty(iconbase64) )
                {
                    string save_url = "/Uploads/" + groudid + "/" + storeid + "/" + DateTime.Now.ToString("yyyyMMdd");
                    string save_path = HttpContext.Server.MapPath(save_url);
                    if (!Directory.Exists(save_path)) Directory.CreateDirectory(save_path);

                    string file_name = DateTime.Now.ToString("HHmmssms") + "_" + "zp.bmp";
                    string url = save_url + "/" + file_name;
                    string save_name = HttpContext.Server.MapPath(url);
                    if (System.IO.File.Exists(save_name)) System.IO.File.Delete(save_name);
                        Base64StringToImage(iconbase64, save_name);
                    
                    json.Data = JsonUtil.GetSuccessForString(url);
                }
                else
                {
                    json.Data = JsonUtil.GetFail(DAL.Enum_StatusCode.Error_FileUploadFailed);
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message + ex.StackTrace);

            }
            return json;
        }

        private void Base64StringToImage(string strbase64,string strsavepath)
        {
            byte[] arr = Convert.FromBase64String(strbase64);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);

            bmp.Save(@strsavepath, System.Drawing.Imaging.ImageFormat.Bmp);
            //bmp.Save(@"d:\"test.bmp", ImageFormat.Bmp);
            //bmp.Save(@"d:\"test.gif", ImageFormat.Gif);
            //bmp.Save(@"d:\"test.png", ImageFormat.Png);
            ms.Close();
        }

    }
}