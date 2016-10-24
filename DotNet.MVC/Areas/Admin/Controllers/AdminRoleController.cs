using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;
using DotNet.MVC.Models;

namespace DotNet.MVC.Areas.Admin.Controllers
{
    public class AdminRoleController : Controller
    {
        public IPermissRoleBll RoleBll { get; set; }
        public IPermissDepartmentBll DepartmentBll { get; set; }
        // GET: AdminRole
        public ActionResult Index()
        {
            object o = CacheHelper.Get("RoleList");
            if (o == null)
            {
                var roleList = RoleBll.ORMLoadEntities(r => true).ToList();
                List<RoleViewModel> roleViewModelList = new List<RoleViewModel>();
                foreach (var item in roleList)
                {
                    RoleViewModel model = new RoleViewModel();
                    model.Id = item.Id;
                    model.RoleName = item.RoleName;
                    model.Description = item.Description;
                    model.AddDate = item.AddDate;
                    model.ModifyDate = item.ModifyDate;
                    model.Department = DepartmentBll.ORMLoadEntities(d => d.Id == item.DepartmentId).FirstOrDefault().DepartmentName;
                    roleViewModelList.Add(model);
                }
                o = roleViewModelList;
                CacheHelper.Add("RoleList", o, DateTime.Now.AddMinutes(20));
            }
            object o2 = CacheHelper.Get("DepartmentList");
            if (o2 == null)
            {
                List<PermissDepartment> allDepartments = DepartmentBll.ORMLoadEntities(d => true).OrderBy(d => d.Sort).ToList();
                o2 = DepartmentBll.CreateDepartmentTree(allDepartments, null);
                CacheHelper.Add("DepartmentList", o2, DateTime.Now.AddMinutes(20));
            }
            ViewData["depTree"] = o2.ToString();
            return View(o as List<RoleViewModel>);
        }
        [HttpPost]
        public ContentResult Add(FormCollection collection)
        {
            try
            {
                PermissRole model = new PermissRole();
                string roleName = collection["RoleName"];
                model.RoleName = roleName;
                model.ModifyDate = DateTime.Now;
                model.Description = collection["RoleDesc"];
                string id = collection["idList"];
                if (id.Length == 36)//如果DepartmentId是Guid
                {
                    Guid depId = new Guid(id);
                    var depModel = DepartmentBll.ORMLoadEntities(d => d.Id == depId).FirstOrDefault();
                    if (depModel != null)//如果这个GUID存在
                    {
                        model.DepartmentId = depId;
                        var rolemodel = RoleBll.ORMLoadEntities(r => r.RoleName == roleName
                                                                    && r.DepartmentId == depId).FirstOrDefault();
                        //如果这个部门下没有这个角色
                        if (rolemodel != null)
                        {
                            if (RoleBll.ORMAdd(model) > 0)
                            {
                                CacheHelper.Remove("RoleList");
                                return Content("1");
                            }
                        }
                    }
                }
                return Content("0");
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(ex.ToString());
                return Content("0");
            }
        }
        [HttpPost]
        public ContentResult Edit(FormCollection collection)
        {
            string roleId = collection["RoleId"];
            if (!string.IsNullOrEmpty(roleId))
            {
                
                var roleModel = RoleBll.ORMLoadEntities(r => r.Id == new Guid(roleId)).FirstOrDefault();
                if (roleModel != null)//存在该实体
                {
                    roleModel.RoleName = collection["EditRoleName"];
                    roleModel.ModifyDate = DateTime.Now;
                    roleModel.Description = collection["EditRoleDesc"];
                    string depId = collection["DepId"];
                    if (depId == "-1")//没变
                    {
                        if (RoleBll.ORMUpdate(roleModel))
                        {
                            CacheHelper.Remove("RoleList");
                            return Content("1");
                        }
                    }
                    else if (depId.Length == 36)//一个新的GUID
                    {
                        //判断是否存在这个部门
                       var depModel= DepartmentBll.ORMLoadEntities(d => d.Id == new Guid(depId)).FirstOrDefault();
                        if (depModel!=null)
                        {
                            roleModel.DepartmentId=new Guid(depId);
                            if (RoleBll.ORMUpdate(roleModel))
                            {
                                CacheHelper.Remove("RoleList");
                                return Content("1");
                            }
                        }
                    }
                }
            }
            return Content("0");
        }

        public ContentResult Delete(string idList)
        {
            //删除Role UserRole RoleMenuButton
            bool result = false;
            string[] idArr = idList.Substring(0, idList.Length - 1).Split('|');
            if (idArr.Length == 1)
            {
                result = RoleBll.DeleteButonAtAnyWhere(idArr[0]);
            }
            else
            {
                result = RoleBll.DeleteButonAtAnyWhere(idArr);
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