using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BLLFactory;
using Common;
using Model;

namespace WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //替换自带的依赖注入
            DependencyResolver.SetResolver(new NinjectDependencyResolver());

         
        }

        protected void Application_Error()
        {
            Exception e = HttpContext.Current.Server.GetLastError();
            LogHelper.Error("logerror","未处理异常",e);

        }
        protected void Application_End()
        {

        }

    }
}
