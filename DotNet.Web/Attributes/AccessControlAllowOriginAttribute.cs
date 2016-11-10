using System;
using System.Web;
using System.Web.Mvc;

namespace DotNet.Web.Attributes
{
    public class AccessControlAllowOriginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var urlRefer = HttpContext.Current.Request.UrlReferrer;
            //if (urlRefer != null)
            //{
            //    string host = urlRefer.ToString().ToLower();
            //    if (host.IndexOf("localhost") >=0 )
            //    {
            //        HttpContext.Current.Response.Headers.Set("Access-Control-Allow-Origin", "http://" + urlRefer.Host);
            //    }
            //}
            //HttpContext.Current.Response.Headers.Set("Access-Control-Allow-Origin", "http://localhost:17878" );
            base.OnActionExecuting(filterContext);
        }
    }
}