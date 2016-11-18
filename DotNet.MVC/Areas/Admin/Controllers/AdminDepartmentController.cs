using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;

namespace DotNet.MVC.Areas.Admin.Controllers
{
    public class AdminDepartmentController : Controller
    {
        public IPermissDepartmentBll DepartmentBll { get; set; }
        [HttpGet]
        public ActionResult Index()
        {
            //拿到缓存中的部门树和父节点列表
            object o = CacheHelper.Get("DepartmentList");
            object fatherNodeDic = CacheHelper.Get("DepatrmentFatherNodeDic");
            if (o == null || fatherNodeDic == null)
            {
                //拿到所有的Department
                List<PermissDepartment> allDepartments = DepartmentBll.LoadEntities(d => true).OrderBy(d => d.Sort).ToList();
                var fatherNodeList = (from d in allDepartments
                                      where d.HaveChild
                                      select d).ToList();
                //父节点列表加入缓存
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("根目录", "-1");
                foreach (var item in fatherNodeList)
                {
                    dic.Add(item.DepartmentName, item.Id.ToString());
                }
                string strTreeHtml = DepartmentBll.CreateDepartmentTree(allDepartments);

                fatherNodeDic = dic;
                o = strTreeHtml;
                //部门树加入缓存
                CacheHelper.Add("DepatrmentFatherNodeDic", fatherNodeDic, DateTime.Now.AddMinutes(20));
                CacheHelper.Add("DepartmentList", o, DateTime.Now.AddMinutes(20));
            }
            //赋值给ViewData,当修改数据时，移除缓存，重新查询
            ViewData["treeHtml"] = o.ToString();
            ViewData["DpdList"] = fatherNodeDic as Dictionary<string, string>;
            return View();
        }
        [HttpPost]
        public ContentResult Add(FormCollection collection)
        {
            try
            {
                int sort;
                string parentId = string.Empty;
                PermissDepartment model = new PermissDepartment();
                //传入部门名
                model.DepartmentName = collection["DepName"].ToString();
                //传入部门父节点Id
                parentId = collection["DepParent"].ToString();
                if (parentId == "-1")
                {
                    model.ParentId = null;
                }
                else if (parentId.Length != 36)//不是Guid
                {
                    return Content("0");
                }
                else
                {
                    model.ParentId = new Guid(parentId);
                }
                //传入部门顺序
                if (!int.TryParse(collection["DepSort"], out sort))
                {
                    sort = 1;
                }
                model.Sort = sort;
                model.HaveChild = collection["DepHaveChild"].ToString() == "1";
                if (DepartmentBll.Add(model) > 0)
                {
                    CacheHelper.Remove("DepatrmentFatherNodeDic");
                    CacheHelper.Remove("DepartmentList");
                    return Content("1");
                }
                else
                {
                    return Content("0");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                return Content("0");
            }
        }
        [HttpPost]
        public ContentResult Edit([Bind(Include = "DepartmentName,Sort")]PermissDepartment model)
        {
            try
            {
                model.Id = new Guid(Request.Form["Id"].ToString());
                model.HaveChild = Request.Form["HaveChild"].ToString() == "1" ? true : false;
                 
                if (Request.Form["ParentId"] == "-1")
                {
                    model.ParentId = null;
                }
                else if (Request.Form["ParentId"].ToString().Length != 36)
                {
                    return Content("0");
                }
                else
                {
                    //如果父节点是它的后代节点？错误
                    List<PermissDepartment> allDepartments = DepartmentBll.LoadEntities(d => true).ToList();
                    var fatherNode =
                        DepartmentBll.LoadEntities(u => u.Id == new Guid(Request.Form["Id"].ToString())).FirstOrDefault();
                    List<PermissDepartment> ProgenList = new List<PermissDepartment>();
                    DepartmentBll.GetProgenyDepartmentList(allDepartments, fatherNode, ProgenList);
                    var item = from d in ProgenList
                        where d.Id == new Guid(Request.Form["ParentId"])
                        select d;
                    if (item.Count()>0)//找到有一项子节点的Id是这次设置的父节点
                    {
                        return Content("0");
                    }
                    model.ParentId = new Guid(Request.Form["ParentId"]);
                   
                }
                if (DepartmentBll.Update(model))
                {
                    CacheHelper.Remove("DepatrmentFatherNodeDic");
                    CacheHelper.Remove("DepartmentList");
                    return Content("1");
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(ex.ToString());
                return Content("0");
            }
        }
        [HttpPost]
        public ContentResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)||id.Length!=36)//不是Guid
            {
                return Content("0");
            }
            else
            {
                //Department UserDepartment  Role UserRole
            }
            return null;
        }
        
    }
}