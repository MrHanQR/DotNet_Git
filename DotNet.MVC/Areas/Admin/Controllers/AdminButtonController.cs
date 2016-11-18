using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;

namespace DotNet.MVC.Areas.Admin.Controllers
{
    public class AdminButtonController : Controller
    {
        public IPermissButtonBll ButtonBll { get; set; }
        //
        // GET: /AdminButton/
        [HttpGet]
        public ActionResult Index()
        {
            object o=CacheHelper.Get("ButtonList");
            if (o==null)
            {
                List<PermissButton> allButtons = ButtonBll.LoadEntities(u => true).OrderBy(u => u.Sort).ToList();
                o = allButtons;
                CacheHelper.Add("ButtonList",o,DateTime.Now.AddMinutes(20));
            }
            //因为使用假分页，取出全部数据放入缓存，提高性能，不再查询数据库
            //当修改数据时，移除缓存，重新查询
            ViewData["ButtonList"] = o as List<PermissButton>;
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            PermissButton model = new PermissButton();
            model.Id = Guid.NewGuid();
            model.Name = collection["ButtonName"];
            model.Icon = collection["ButtonIcon"];
            model.Sort = Convert.ToInt32(collection["ButtonSort"]);
            model.HttpMethod = collection["ButtonHttpMethod"];
            model.ActionNameCode = collection["ButtonActionName"];
            model.AddDate = DateTime.Now;
            model.Description = collection["ButtonDesc"];
            
            if (ButtonBll.Add(model) > 0)
            {
                CacheHelper.Remove("ButtonList");
                return RedirectToAction("Index");
            }
            else
            {
                return Content("<script >alert('添加失败！');</script >", "text/html"); 
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            PermissButton model = new PermissButton();
            model.Id = new Guid(collection["hideId"]);
            model.Name = collection["EditButtonName"];
            model.Icon = collection["EditButtonIcon"];
            model.Sort = Convert.ToInt32(collection["EditButtonSort"]);
            model.HttpMethod = collection["EditButtonHttpMethod"];
            model.ActionNameCode = collection["EditButtonActionName"];
            model.AddDate = DateTime.Now;
            model.Description = collection["EditButtonDesc"];
            if (ButtonBll.Update(model))
            {
                CacheHelper.Remove("ButtonList");
                return RedirectToAction("Index");
            }
            else
            {
                return Content("<script >alert('修改失败！');</script >", "text/html"); 
            }
        }
        [HttpPost]
        public ActionResult Delete(string idList)
        {
            //删除这个按钮
            //删除MenuButton
            //删除RoleMenuButton
            //删除UserMenuButton
            bool result=false;
            string[] idArr = idList.Substring(0,idList.Length-1).Split('|');
            if (idArr.Length==1)
            {
                result = ButtonBll.DeleteButonAtAnyWhere(idArr[0]);
            }
            else
            {
                result = ButtonBll.DeleteButonAtAnyWhere(idArr);
            }
            if (result)
            {
                CacheHelper.Remove("ButtonList");
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
	}
}
