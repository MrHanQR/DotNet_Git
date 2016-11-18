using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Autofac;
using DotNet.BLL;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;

namespace DotNet.Web.Areas.Admin.Attributes
{
    public class CheckLoginAttribute : AuthorizeAttribute//ActionFilterAttribute
    {
        private readonly IPermissUserLoginBll _userLoginBll;

        public CheckLoginAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<PermissUserLoginBll>().As<IPermissUserLoginBll>().InstancePerRequest();
            IContainer container = builder.Build();
            _userLoginBll = container.Resolve<PermissUserLoginBll>();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var mySession = httpContext.Request.Cookies["mysessionId"];
            if (mySession == null)
            {
                return false;
            }
            else
            {
                string mySessionId = mySession.Value;
                var userSession = CacheHelper.Get(mySessionId) as PermissUserLogin;
                if (userSession == null)//执行记住我之后，直接登陆到HomeIndex，Session中却没有User实体
                {
                    //检查是不是已经“记住我”
                    string loginName = httpContext.Request.Cookies["N"] == null ? string.Empty : httpContext.Request.Cookies["N"].Value;
                    string loginPwd = httpContext.Request.Cookies["W"] == null ? string.Empty : httpContext.Request.Cookies["W"].Value;
                    if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))//cookie里没有
                    {
                        return false;
                    }
                    //已经记住我 则自己登录
                    userSession = _userLoginBll.LoadEntities(u => u.LoginId == loginName && u.LoginPwd == loginPwd).FirstOrDefault();
                    CacheHelper.Add("mysessionId", userSession);
                }
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/admin/login/index");
            }
        }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var context = filterContext.HttpContext;
        //    var mySession =context.Request.Cookies["mysessionId"];
        //    if (mySession == null)
        //    {
        //        filterContext.Result = new RedirectResult("/admin/login/index");
        //    }
        //    else
        //    {
        //        string mySessionId = mySession.Value;
        //        var userSession = CacheHelper.Get(mySessionId) as PermissUserLogin;
        //        if (userSession == null)//执行记住我之后，直接登陆到HomeIndex，Session中却没有User实体
        //        {
        //            //检查是不是已经“记住我”
        //            string loginName =context. Request.Cookies["N"] == null ? string.Empty :context. Request.Cookies["N"].Value;
        //            string loginPwd = context.Request.Cookies["W"] == null ? string.Empty :context. Request.Cookies["W"].Value;
        //            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))//cookie里没有
        //            {
        //                filterContext.Result = new RedirectResult("/admin/login/index");
        //            }
        //            //已经记住我 则自己登录
        //            userSession = _userLoginBll.ORMLoadEntities(u => u.LoginId == loginName && u.LoginPwd == loginPwd).FirstOrDefault();
        //            CacheHelper.Add("mysessionId", userSession);
        //        }
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
    }
}