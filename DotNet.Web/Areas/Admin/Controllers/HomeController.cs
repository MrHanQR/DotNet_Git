using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;
using DotNet.Web.Areas.Admin.Attributes;

namespace DotNet.Web.Areas.Admin.Controllers
{
    [CheckLogin]
    public class HomeController : Controller
    {
        private readonly IPermissMenuBll _menuBll;

        public HomeController(IPermissMenuBll menuBll)
        {
            _menuBll = menuBll;
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            List<PermissMenu> allMenuList = _menuBll.LoadEntities(u => true).ToList();
            //拿到一级菜单
            var firstLevelMenus = (from m in allMenuList
                                  where m.MenuLevel == 1
                                  orderby m.Sort
                                  select m).ToList();
            //拿到二、三级菜单
            var secondLevelMenus = (from m in allMenuList
                                   where m.MenuLevel != 1
                                   orderby m.Sort
                                   select m).ToList();
            ViewBag.FirstLevelMenus = firstLevelMenus;
            ViewBag.SecondLevelMenus = secondLevelMenus;
            var mySession = ControllerContext.HttpContext.Request.Cookies["mysessionId"];
            if (mySession != null)
            {
                string mySessionId = mySession.Value;
                var userSession = CacheHelper.Get(mySessionId) as PermissUserLogin;
                ViewBag.UserSession = userSession;
            }
            return View();
        }
        [HttpPost]
        public ContentResult SignOut()
        {
            var httpCookie = Request.Cookies["mysessionId"];
            if (httpCookie != null)
            {
                string sessionId = httpCookie.Value;
                if (!string.IsNullOrEmpty(sessionId) && sessionId.Length == 36)
                {
                    CacheHelper.Remove(sessionId);
                    Response.Cookies["N"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["W"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            RedirectToAction("Index", "Login");
            return Content("1");
        }
    }
}