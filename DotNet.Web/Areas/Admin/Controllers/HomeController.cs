﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;

namespace DotNet.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPermissMenuBll _menuBll;
        private readonly IPermissUserLoginBll _userLoginBll;

        public HomeController(IPermissMenuBll menuBll,IPermissUserLoginBll userLoginBll)
        {
            _menuBll = menuBll;
            _userLoginBll = userLoginBll;
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            List<PermissMenu> allMenuList = _menuBll.ORMLoadEntities(u => true).ToList();
            //拿到一级菜单
            var firstLeveMenus = (from m in allMenuList
                                  where m.MenuLevel == 1
                                  orderby m.Sort
                                  select m).ToList();
            //拿到二、三级菜单
            var secondLeveMenus = (from m in allMenuList
                                   where m.MenuLevel != 1
                                   orderby m.Sort
                                   select m).ToList();
            ViewData["firstLeveMenus"] = firstLeveMenus;
            ViewData["secondLeveMenus"] = secondLeveMenus;
            var mySession = ControllerContext.HttpContext.Request.Cookies["mysessionId"];
            if (mySession == null)
            {
                return RedirectToAction("Index", "Login", new { Area = "Admin" });
            }
            else
            {
                string mySessionId = mySession.Value;
                var userSession = CacheHelper.Get(mySessionId) as PermissUserLogin;
                if (userSession == null)//执行记住我之后，直接登陆到HomeIndex，Session中却没有User实体
                {
                    //检查是不是已经“记住我”
                    string loginName = Request.Cookies["N"] == null ? string.Empty : Request.Cookies["N"].Value;
                    string loginPwd = Request.Cookies["W"] == null ? string.Empty : Request.Cookies["W"].Value;
                    if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))//cookie里没有
                    {
                        return RedirectToAction("Index","Login",new {Area="Admin"});
                    }
                    //已经记住我 则自己登录
                    userSession = _userLoginBll.ORMLoadEntities(u => u.LoginId == loginName && u.LoginPwd == loginPwd).FirstOrDefault();
                    CacheHelper.Add("mysessionId", userSession);
                }
                ViewData["userSession"] = userSession;
                return View();
            }

        }
        [HttpPost]
        public ContentResult SignOut()
        {
            string sessionId = Request.Cookies["mysessionId"].Value;
            if (!string.IsNullOrEmpty(sessionId) && sessionId.Length == 36)
            {
                CacheHelper.Remove(sessionId);
                Response.Cookies["N"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["W"].Expires = DateTime.Now.AddDays(-1);
            }
            RedirectToAction("Index", "Login");
            return Content("1");
        }
    }
}