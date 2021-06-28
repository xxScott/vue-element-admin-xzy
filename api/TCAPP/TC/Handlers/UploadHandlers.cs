using Com.Caimomo.Common;
using HrApp.TC.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Utilities;
using System.IO;
using System.Web.Mvc;
using DAL;
using Models;
using System.Drawing;
using System.Threading;

namespace Com.Caimomo.Handlers
{
    public class CheckImgUploadMbHandlers : IAjaxHandler
    {
        public class Source
        {
            private string m_Iconbase64 = string.Empty;

            public string Iconbase64
            {
                get
                {
                    return m_Iconbase64;
                }

                set
                {
                    m_Iconbase64 = value;
                }
            }
        }
        /// <summary>
        /// 移动端--图片上传接口
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="queryString"></param>
        /// <param name="postContent"></param>
        /// <returns></returns>
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    Source source = Newtonsoft.Json.JsonConvert.DeserializeObject<Source>(postContent);
                    string[] img_array = source.Iconbase64.Split(',');
                    string iconbase64 = img_array[1];

                    if (!string.IsNullOrEmpty(iconbase64))
                    {
                        string save_url = "/Uploads/" + groudid + "/" + storeid + "/Inspection/" + DateTime.Now.ToString("yyyyMMdd");
                        //string save_path = string.Empty;
                  
                        string save_path = System.Web.HttpContext.Current.Server.MapPath(save_url);
                        if (!Directory.Exists(save_path)) Directory.CreateDirectory(save_path);

                        string file_name = DateTime.Now.ToString("yyyyMMddHHmmssms") + "_" + "checkimg.jpg";
                        string url = save_url + "/" + file_name;
                        string save_name = System.Web.HttpContext.Current.Server.MapPath(url);
                        if (System.IO.File.Exists(save_name)) System.IO.File.Delete(save_name);
                        Base64StringToImage(iconbase64, save_name);

                        

                        return new HandleResult { ResultCode = 0, Message = "", Data = url };
                    }
                    else
                    {
                        return new HandleResult { ResultCode = 1, Message = "", Data = null };
                    }

                }
            }
            catch (Exception ex)
            {
                Com.Caimomo.Common.Log.WriteLog("图片上传出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "图片上传出错:" + ex, Data = null };
            }
        }

        private void Base64StringToImage(string strbase64, string strsavepath)
        {
            Users currentUser = Utilities.Security.CurrentUser;

            byte[] arr = Convert.FromBase64String(strbase64);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);

            string imageWater = currentUser.UserName.ToString() + "_" + System.DateTime.Now.ToString(); //图片水印添加的文字
            HovercWarter.AddTextToImg(bmp,imageWater, @strsavepath);
            
            //bmp.Save(@strsavepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //bmp.Save(@"d:\"test.bmp", ImageFormat.Bmp);
            //bmp.Save(@"d:\"test.gif", ImageFormat.Gif);
            //bmp.Save(@"d:\"test.png", ImageFormat.Png);
            ms.Close();
        }

    }

    public class CheckImgDeteleMbHandlers : IAjaxHandler
    {
        public class Source
        {
            private string m_ID = string.Empty;
            /// <summary>
            /// 唯一ID
            /// </summary>
            public string ID
            {
                get { return m_ID; }
                set
                {
                    m_ID = value;
                }
            }
        }

        public class PicRecordResponse
        {
            private string m_UID = string.Empty;
            /// <summary>
            /// 主键
            /// </summary>
            public string UID
            {
                get
                {
                    return m_UID;
                }

                set
                {
                    m_UID = value;
                }
            }

            private string m_URL = string.Empty;
            /// <summary>
            /// URL
            /// </summary>
            public string URL
            {
                get
                {
                    return m_URL;
                }

                set
                {
                    m_URL = value;
                }
            }
        }

        /// <summary>
        /// 移动端--图片删除接口
        /// </summary>
        /// <param name="hContext"></param>
        /// <param name="queryString"></param>
        /// <param name="postContent"></param>
        /// <returns></returns>
        public HandleResult HandleRequest(HttpContext hContext, NameValueCollection queryString, string postContent)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";

            try
            {
                using (TCDataContextDataContext context = MyDataContextFactory.GetDataContext<TCDataContextDataContext>(MyDataContextFactory.CaiMoMo_TC))
                {
                    Users currentUser = Utilities.Security.CurrentUser;
                    if (currentUser == null) return new HandleResult { ResultCode = 1, Message = "没有登录", Data = null };
                    int? storeid = currentUser.StoreID;
                    int? groudid = currentUser.GroupID;

                    Source source = Newtonsoft.Json.JsonConvert.DeserializeObject<Source>(postContent);
                    string URL = string.Empty;

                    TC_Image tC_Image = context.TC_Image.SingleOrDefault(g => g.ID == source.ID);
                    if (tC_Image != null ) URL = tC_Image.Url;
                    if ( URL != string.Empty ) {
                        //删除表数据
                        context.TC_Image.DeleteOnSubmit(tC_Image);
                        context.SubmitChanges();
                        //删除文件
                        string save_name = System.Web.HttpContext.Current.Server.MapPath(URL);
                        if (System.IO.File.Exists(save_name)) {
                            System.IO.File.Delete(save_name);
                        }
                    }

                    return new HandleResult { ResultCode = 0, Message = "图片删除成功", Data = null };
                }
            }
            catch (Exception ex)
            {
                Com.Caimomo.Common.Log.WriteLog("图片删除出错:" + ex);
                return new HandleResult { ResultCode = 1, Message = "图片删除出错:" + ex, Data = null };
            }
        }
    }
}