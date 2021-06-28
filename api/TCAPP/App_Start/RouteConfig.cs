using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HrApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //   "HelloWorldApi",
            //   "api/{controller}/{action}/{id}",
            //   new { controller = "HelloWorld", id = UrlParameter.Optional });

            //routes.MapRoute(
            //   "AccountApi",
            //   "api/{controller}/{action}/{certificateNo}",
            //   new { controller = "Account", certificateNo = UrlParameter.Optional });

            //routes.MapRoute(
            //   "OrderApi",
            //   "api/{controller}/{action}/{certificateNo}",
            //   new { controller = "Order", certificateNo = UrlParameter.Optional });

            //routes.MapRoute(
            //   "BalanceApi",
            //   "api/{controller}/{action}/{certificateNo}",
            //   new { controller = "Balance", certificateNo = UrlParameter.Optional });

            //routes.MapRoute(
            //   "SubjectApi",
            //   "api/{controller}/{action}/{platformCode}",
            //   new { controller = "Subject", platformCode = UrlParameter.Optional });

            routes.MapRoute(
               "DefaultWebApi",
               "api/{controller}/{action}/{id}",
               new { id = UrlParameter.Optional });

            /*********************************************************************************************************/

            routes.MapRoute(
                name: "Default",
                url: "{action}",
                defaults: new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            );
        }
    }
}