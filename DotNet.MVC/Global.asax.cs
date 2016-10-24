using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DotNet.MVC
{
    public class MvcApplication :Spring.Web.Mvc.SpringMvcApplication// System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //让log4net起作用
            //log4net.Config.XmlConfigurator.Configure();
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(Server.MapPath("~") + @"/Config/Log4NetConfig.xml"));
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            //记录日志
            Exception ex = Server.GetLastError();

            string errorMsg = ex.ToString();

            //日志可能写到多个地方去。那么可能产生  变化点。
            Common.LogHelper.WriteLog(errorMsg);

            //转到错误页或者跳转。
            //Response.Redirect("/Error.html");
        }
    }
}
