using DotNet.Common;
using DotNet.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNet.MVC.Models
{
    public class UserStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var mysession = filterContext.HttpContext.Request.Cookies["mysessionId"];
            if (mysession!=null)
            {
                string mySessionId = mysession.Value;
                var userSession = CacheHelper.Get(mySessionId) as PermissUserLogin;
                if (userSession == null)
                {
                    filterContext.HttpContext.Response.Redirect("/AdminLogin/Index");
                }
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/AdminLogin/Index");
            }
        }
    }
}