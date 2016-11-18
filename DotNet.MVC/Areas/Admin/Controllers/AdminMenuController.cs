using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;

namespace DotNet.MVC.Areas.Admin.Controllers
{
    public class AdminMenuController : Controller
    {
        public IPermissMenuBll MenuBll { get; set; }
        public IPermissButtonBll ButtonBll { get; set; }
        public IPermissRefMenuButtonBll MenuButtonBll { get; set; }
        [HttpGet]
        public ActionResult Index()
        {
            object o1 = CacheHelper.Get("MenuList");
            object o2 = CacheHelper.Get("MenuDpdList");
            if (o1 == null || o2 == null)//如果没有这俩的缓存
            {
                //构建tbMenu集合和下拉列表集合
                List<PermissMenu> allMenus = MenuBll.LoadEntities(u => true).OrderBy(u => u.MenuLevel).ThenBy(u => u.Sort).ToList();
                var fatherMenuList = (from r in allMenus
                                      where r.HaveChild == true
                                      select r);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("无", "-1");
                foreach (PermissMenu item in fatherMenuList)
                {
                    dic.Add(item.Name, item.Id.ToString());
                }
                //给o1 o2赋值
                o1 = allMenus;
                o2 = dic;
                //放入缓存
                CacheHelper.Add("MenuList", allMenus);
                CacheHelper.Add("MenuDpdList", dic);
            }
            ViewData["DpdList"] = o2 as Dictionary<string, string>;
            return View(o1 as List<PermissMenu>);
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                string parentId = collection["MenuParent"];
                PermissMenu model = new PermissMenu();
                //model.Id = Guid.NewGuid();
                model.Name = collection["MenuName"];
                model.Icon = collection["MenuIcon"];
                model.Sort = Convert.ToInt32(collection["MenuSort"]);
                if (parentId == "-1")
                {
                    model.ParentId = null;
                    model.MenuLevel = 1;
                }
                else
                {
                    model.ParentId = new Guid(parentId);
                    model.MenuLevel = MenuBll.GetModel(parentId).MenuLevel + 1;
                }
                model.HaveChild = collection["MenuHaveChild"] == "1";
                model.ControllerNameCode = collection["MenuControllerName"];
                model.ActionNameCode = collection["MenuActionName"];
                //model.AddDate = DateTime.Now;
                if (MenuBll.Add(model) > 0)
                {
                    CacheHelper.Remove("MenuList");
                    CacheHelper.Remove("MenuDpdList");
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("<script >alert('添加失败！');location.reload(true);</script >", "text/html");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                return Content("<script >alert('服务器遇到错误！');location.reload(true);</script >", "text/html");
            }
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Icon,Sort,ControllerNameCode,ActionNameCode")] PermissMenu model)
        {
            try
            {
                string parentId = Request.Form["ParentId"];
                //model.AddDate = DateTime.Now;
                model.HaveChild = Request.Form["HaveChild"] == "1";
                if (parentId == "-1")
                {
                    model.ParentId = null;
                    model.MenuLevel = 1;
                }
                else
                {
                    model.ParentId = new Guid(parentId);
                    model.MenuLevel = MenuBll.GetModel(parentId).MenuLevel + 1;
                }
                if (MenuBll.Update(model))
                {
                    CacheHelper.Remove("MenuList");
                    CacheHelper.Remove("MenuDpdList");
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("<script >alert('修改失败！');location.reload(true);</script >", "text/html");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                return Content("<script >alert('服务器遇到错误！');location.reload(true);</script >", "text/html");
            }
        }
        [HttpPost]
        public ActionResult Delete(string idList)
        {
            bool result = false;
            string[] list = idList.Substring(0, idList.Length - 1).Split('|');
            if (list.Length == 1)
            {
                result = MenuBll.DeleteMenuAtAnyWhere(list[0]);
            }
            else
            {
                result = MenuBll.DeleteMenuAtAnyWhere(list);
            }
            if (result)
            {
                CacheHelper.Remove("MenuList");
                CacheHelper.Remove("MenuDpdList");
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        [HttpPost]
        public ActionResult SetButton()
        {
            //拿到要设置的菜单的Id
            string menuId = Request.Form["hideMenuId"];
            //拿到用户提交的ButtonId字符串数组
            var requestString = Request.Form["checkButton"];
            //如果接收的数据不正确，结束。
            if (!string.IsNullOrEmpty(requestString) && !string.IsNullOrEmpty(menuId))
            {
                //拿到用户提交的Button的Id
                string[] idList = requestString.Split(',');
                //删除该菜单以前的按钮，把这次提交的添加进去
                if (MenuButtonBll.ORMChangeButton(idList, menuId))
                {
                    return Content("1");
                }
                else
                {
                    return Content("0");
                }
            }
            else
            {
                return Content("0");
            }
        }

        [HttpPost]
        public ContentResult GetButton()
        {
            //拿到菜单Id
            string menuId = Request.Form["menuId"];
            //查询出该菜单已有的Button
            var menuButtonList = (from c in MenuButtonBll.LoadEntities(u => u.MenuId == new Guid(menuId))
                                  select c.ButtonId).ToList();
            var buttonList = ButtonBll.LoadEntities(u => true).OrderBy(u => u.Sort).ToList();
            StringBuilder sb = new StringBuilder();
            int cellCount = 0;
            sb.Append("<table><tr>");
            foreach (PermissButton buttonItem in buttonList)
            {

                sb.Append("<td style='width:150px'>");
                //如果已有该Button，置于选中状态。
                if (menuButtonList.Contains(buttonItem.Id))
                {
                    sb.AppendFormat("<input type='checkbox' name='checkButton' checked='checked' value='{0}'/>", buttonItem.Id);
                }
                else
                {
                    sb.AppendFormat("<input type='checkbox' name='checkButton' value='{0}'/>", buttonItem.Id);
                }
                sb.Append("<a href='javascript:void(0)' class='btn btn-app'>");
                sb.AppendFormat("<i class='fa {0}'></i> {1}", buttonItem.Icon, buttonItem.Name);
                sb.Append("</a></td>");
                cellCount++;
                if (cellCount == 5)
                {
                    sb.Append("</tr></tr>");
                    cellCount = 0;
                }
            }
            sb.Append("</tr></table>");
            return Content(sb.ToString());
        }
    }
}
