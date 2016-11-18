using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.Entity.Enum;
using DotNet.IBLL;
using DotNet.MVC.Models;

namespace DotNet.MVC.Areas.Admin.Controllers
{
    public class AdminUserController : Controller
    {
        public IPermissUserLoginBll UserLoginBll { get; set; }
        public IPermissUserDetailsBll UserDetailsBll { get; set; }
        public IPermissDepartmentBll DepartmentBll { get; set; }
        public IPermissRefUserDepartmentBll UserDepartmentBll { get; set; }
        // GET: AdminUser
        [HttpGet]
        public ActionResult Index()
        {
            object o = CacheHelper.Get("UserList");
            if (o == null)
            {
                o = UserLoginBll.LoadEntities(u => u.DeleteFlag == DelFlagEnum.Normal).ToList();
                CacheHelper.Add("UserList", o);
            }
            object o2 = CacheHelper.Get("DepartmentList");
            if (o2 == null)
            {
                List<PermissDepartment> allDepartments = DepartmentBll.LoadEntities(d => true).OrderBy(d => d.Sort).ToList();
                o2 = DepartmentBll.CreateDepartmentTree(allDepartments, null);
                CacheHelper.Add("DepartmentList", o2, DateTime.Now.AddMinutes(20));
            }
            ViewData["depTree"] = o2.ToString();
            ViewData["UserList"] = o as List<PermissUserLogin>;
            return View();
        }
        [HttpGet]
        public ActionResult OldUserIndex()
        {
            object o = CacheHelper.Get("OldUserList");
            if (o == null)
            {
                o = UserLoginBll.LoadEntities(u => u.DeleteFlag == DelFlagEnum.Deleted).ToList();
                CacheHelper.Add("OldUserList", o);
            }
            return View(o as List<PermissUserLogin>);
        }
        [HttpGet]
        [ActionName("AuditUser")]
        public ActionResult WaitForCheckUser()
        {
            object o = CacheHelper.Get("AuditUserList");
            if (o == null)
            {
                List<PermissUserLogin> userList =
               UserLoginBll.LoadEntities(u => u.IsAble == false
                                              && u.AddDate == null).ToList();
                List<AuditUserViewModel> auditUserList = new List<AuditUserViewModel>();
                foreach (PermissUserLogin user in userList)
                {
                    AuditUserViewModel model = new AuditUserViewModel();
                    model.Id = user.Id;
                    model.LoginId = user.LoginId;
                    model.LoginPwd = user.LoginPwd;
                    model.UserEmail = user.UserEmail;
                    model.UserName = user.UserName;
                    model.ApplyDate = user.ApplyDate;
                    auditUserList.Add(model);
                }
                o = auditUserList;
            }
            return View(o as List<AuditUserViewModel>);
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                string errorMsg = string.Empty;
                string loginId = collection["AddLoginId"];
                string loginPwd = collection["AddloginPwd"];
                string userName = collection["AddUserName"];
                string userEmail = collection["AddUserEmail"];
                HttpPostedFileBase file = Request.Files["AddPhotoPath"];
                if (string.IsNullOrEmpty(loginId)
                    || string.IsNullOrEmpty(loginPwd)
                    || string.IsNullOrEmpty(userName)
                    || string.IsNullOrEmpty(userEmail))//没填全，从页面填事，不检测遇到攻击可能异常。
                {
                    errorMsg = "请填写必要信息";
                }
                else//正常填写
                {
                    PermissUserLogin user = UserLoginBll.LoadEntities(u => u.LoginId == loginId || u.UserEmail == userEmail).FirstOrDefault();
                    if (user != null)//用户名 邮箱 占用
                    {
                        errorMsg = "用户名或邮箱已被注册";
                    }
                    else
                    {
                        PermissUserLogin model = new PermissUserLogin();
                        model.LoginId = loginId;
                        model.LoginPwd = DESEncryptHelper.GetStringMD5(loginPwd);
                        model.UserName = userName;
                        model.UserEmail = userEmail;
                        model.IsAble = true;
                        model.ShortDescription = string.Empty;
                        DateTime dt = DateTime.Now;
                        model.AddDate = dt;
                        model.ApplyDate = dt;
                        model.DeleteFlag = DelFlagEnum.Normal;
                        model.PhotoPath = FileOperatorHelper.SaveImages(file, out errorMsg, 140, 140);//"165C9DCC948C38B305578011F06852FC"
                        //如果file文件有问题，out返回ErrMsg，否则为String.Empty
                        //返回md5FileName;
                        if (UserLoginBll.Add(model) > 0)
                        {
                            CacheHelper.Remove("UserList");
                            return RedirectToAction("Index", "AdminUser");
                        }
                        else
                        {
                            errorMsg = "添加失败";
                        }
                    }
                }
                return Content("<script >alert('" + errorMsg + "！');</script >", "text/html");
            }
            catch (Exception ex)
            {
                return Content("<script >alert(' 操作异常 ！');</script >", "text/html");
            }

        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                string errorMsg = string.Empty;
                string id = collection["Id"];
                string loginPwd = collection["LoginPwd"];
                string userName = collection["UserName"];
                string userEmail = collection["UserEmail"];
                HttpPostedFileBase file = Request.Files["PhotoPath"];
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(userEmail))
                {
                    errorMsg = "请填写必要信息";
                }
                else
                {
                    //查到该用户
                    PermissUserLogin user = UserLoginBll.LoadEntities(u => u.Id == new Guid(id)).FirstOrDefault();
                    //验证修改后的邮箱会不会重复
                    PermissUserLogin validUser = UserLoginBll.LoadEntities(u => u.UserEmail == userEmail).FirstOrDefault();
                    if (validUser != null && validUser.Id != new Guid(id))//存在一条记录是这个邮箱，且不是修改的这条
                    {
                        errorMsg = "邮箱已被占用";
                    }
                    else
                    {
                        user.UserName = userName;
                        user.UserEmail = userEmail;
                        if (!string.IsNullOrEmpty(loginPwd))//密码改了
                        {
                            user.LoginPwd = DESEncryptHelper.GetStringMD5(loginPwd);
                        }
                        if (file.ContentLength > 0)//改头像了
                        {
                            user.PhotoPath = FileOperatorHelper.SaveImages(file, out errorMsg, 140, 140);
                        }
                        if (UserLoginBll.Update(user))
                        {
                            CacheHelper.Remove("UserList");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            errorMsg = "修改失败";
                        }
                    }
                }
                return Content("<script >alert('" + errorMsg + "！');</script >", "text/html");
            }
            catch (Exception ex)
            {
                return Content("<script >alert(' 操作异常 ！');</script >", "text/html");
            }
        }
        [HttpPost]
        public ContentResult Delete(string idList)
        {
            if (idList.Length > 0)
            {
                bool result = false;
                string[] list = idList.Substring(0, idList.Length - 1).Split('|');
                //不是真删除！是逻辑删除
                if (list.Length == 1)
                {
                    result = UserLoginBll.ORMLogicDelete(list[0]);
                }
                else
                {
                    result = UserLoginBll.ORMLogicDelete(list);
                }
                if (result)
                {
                    CacheHelper.Remove("UserList");
                    CacheHelper.Remove("OldUserList");
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
        public ContentResult DisEnable(string id, string disable)
        {
            //b7a96cb0-3086-43d3-8131-12e33313a389
            if (id.Length != 36)
            {
                return Content("0");
            }
            else
            {
                var model = UserLoginBll.LoadEntities(u => u.Id == new Guid(id)).FirstOrDefault();
                if (model != null)
                {
                    if (disable == "1")
                    {
                        model.IsAble = false;
                    }
                    else if (disable == "0")
                    {
                        model.IsAble = true;
                    }
                    if (UserLoginBll.Update(model))
                    {
                        CacheHelper.Remove("UserList");
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
        }

        // 以往用户-恢复
        [HttpPost]
        public ContentResult Restore(string id)
        {
            if (id.Length != 36)
            {
                return Content("0");
            }
            else
            {
                var model = UserLoginBll.LoadEntities(u => u.Id == new Guid(id)).FirstOrDefault();
                if (model != null)
                {
                    model.AddDate = DateTime.Now;
                    model.DeleteFlag = DelFlagEnum.Normal;
                    if (UserLoginBll.Update(model))
                    {
                        CacheHelper.Remove("OldUserList");
                        CacheHelper.Remove("UserList");
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
        }

        // 彻底删除，不保留用户数据
        [HttpPost]
        public ContentResult PhysicallyDelete(string id)
        {
            if (id.Length != 36)
            {
                return Content("0");
            }
            else
            {
                var model = UserLoginBll.LoadEntities(u => u.Id == new Guid(id)).FirstOrDefault();
                if (model != null)
                {
                    if (UserLoginBll.ORMPhysicallyDelete(id))
                    {
                        CacheHelper.Remove("OldUserList");
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
        }
        //审核用户
        public ContentResult PassCheck(string idList)
        {
            if (idList.Length > 0)
            {
                bool result = false;
                string[] list = idList.Substring(0, idList.Length - 1).Split('|');
                if (list.Length == 1)//只通过一个
                {
                    result = UserLoginBll.ORMPassCheck(list[0]);
                }
                else//批量审核通过
                {
                    result = UserLoginBll.ORMPassCheck(list);
                }
                if (result)
                {
                    CacheHelper.Remove("UserList");
                    CacheHelper.Remove("AuditUserList");
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

        [HttpGet]
        public ActionResult Details(string id)
        {
            if (id.Length != 36)//传过来的根本不是guid
            {
                return Content("0");
            }
            else
            {
                Guid guidId = new Guid(id);
                var loginModel = UserLoginBll.LoadEntities(u => u.Id == guidId).FirstOrDefault();
                if (loginModel == null)//Login表里根本没这条记录
                {
                    return Content("0");
                }
                else
                {
                    UserDetailsViewModel model = new UserDetailsViewModel();
                    model.Id = loginModel.Id;
                    model.LoginId = loginModel.LoginId;
                    model.LoginPwd = loginModel.LoginPwd;
                    model.UserName = loginModel.UserName;
                    model.UserEmail = loginModel.UserEmail;
                    model.IsAble = loginModel.IsAble == true ? "是" : "否";
                    model.DeleteFlag = loginModel.DeleteFlag == DelFlagEnum.Normal ? "正常" : "不可用";
                    model.AddDate = (DateTime)loginModel.AddDate;
                    model.ApplyDate = loginModel.ApplyDate;
                    model.PhotoPath = loginModel.PhotoPath;
                    model.ShortDescription = string.IsNullOrEmpty(loginModel.ShortDescription)
                        ? "这家伙很懒，什么都没有说"
                        : loginModel.ShortDescription;
                    var detailModel = UserDetailsBll.LoadEntities(u => u.UserId == guidId).FirstOrDefault();
                    if (detailModel == null)
                    {
                        model.RealName = "未知";
                        model.Gender = "未知";
                        model.Birth = "未知";
                        model.IdentityCardNumber = "未知";
                        model.Address = "未知";
                        model.PhoneNumber = "未知";
                        model.Description = "可惜他什么都没说";
                    }
                    else
                    {
                        model.RealName = string.IsNullOrEmpty(detailModel.RealName) ? "未知" : detailModel.RealName;
                        model.Gender = detailModel.Gender == null ? "未知"
                                      : detailModel.Gender == true ? "男" : "女";
                        model.Birth = detailModel.Birth == null ? "未知" : detailModel.Birth.ToString();
                        model.IdentityCardNumber = string.IsNullOrEmpty(detailModel.IdentityCardNumber)
                            ? "未知"
                            : detailModel.IdentityCardNumber;
                        model.Address = string.IsNullOrEmpty(detailModel.Address) ? "未知" : detailModel.Address;
                        model.PhoneNumber = string.IsNullOrEmpty(detailModel.PhoneNumber)
                            ? "未知"
                            : detailModel.PhoneNumber;
                        model.Description = string.IsNullOrEmpty(detailModel.Description)
                            ? "可惜他什么都没说"
                            : detailModel.Description;
                    }
                    return View(model);
                }
            }
        }

        [HttpPost]
        public ContentResult SetDepartment(string hideUserList, string hideDepId)
        {
            //拿到GUID
            Guid depId;
            if (Guid.TryParse(hideDepId, out depId))
            {
                //是否存在这个部门
                var depModel=DepartmentBll.LoadEntities(d => d.Id == depId).FirstOrDefault();
                if (depModel!=null)
                {
                    //拿到用户Id
                    string[] idArr = hideUserList.Substring(0, hideUserList.Length - 1).Split('|');
                    if (UserDepartmentBll.ORMSetUserDep(depId,idArr))
                    {
                        return Content("1");
                    }
                }
            }
            return Content("0");
        }
    }
}