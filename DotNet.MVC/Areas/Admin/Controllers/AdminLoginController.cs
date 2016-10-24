using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Common;
using DotNet.Entity;
using DotNet.IBLL;

namespace DotNet.MVC.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        public IPermissUserLoginBll UserLoginBll { get; set; }
        // GET: AdminLogin
        public ActionResult Index()
        {
            //检查是不是已经“记住我”
            string loginName = Request.Cookies["N"] == null ? string.Empty : Request.Cookies["N"].Value;
            string loginPwd = Request.Cookies["W"] == null ? string.Empty : Request.Cookies["W"].Value;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))//cookie里没有
            {
                return View();
            }
            else//cookie里有
            {
                var user = UserLoginBll.ORMLoadEntities(u => u.LoginId == loginName && u.LoginPwd == loginPwd).FirstOrDefault();
                if (user == null)//密码已经更改，无法登陆
                {
                    return View();
                }
                else//成功登陆
                {
                    //用户memcached模拟Session
                    Guid guid = Guid.NewGuid();
                    //以guid为key，以登录用户为value放到mm里面去。
                    Common.CacheHelper.Add(guid.ToString(), user, DateTime.Now.AddMinutes(20));
                    //把guid写到cookie里面去
                    Response.Cookies["mysessionId"].Value = guid.ToString();
                    return RedirectToAction("Index", "AdminHome");
                }
            }
        }
        [HttpPost]
        public ActionResult CheckLogin(string loginId, string loginPwd, string vCode, string loginCity, string loginkeeping)
        {
            //第一步：校验验证码
            //从session中拿到咱们的验证码
            string strCode = Session["LoginCode"] == null ? string.Empty : Session["LoginCode"].ToString();
            Session["LoginCode"] = null;//验证码的Session只能用一次。

            if (string.IsNullOrEmpty(vCode))
            {
                return Content("请输入验证码！");
            }
            else if (strCode.ToLower() != vCode.ToLower())
            {
                Session["LoginCode"] = null;
                return Content("验证码验证失败");
            }
            else//验证码正确
            {
                //第二步：校验用户名密码
                //        密码要加密
                string md5Pwd = DESEncryptHelper.GetStringMD5(loginPwd);
                var user = UserLoginBll.ORMLoadEntities(u => (u.LoginId == loginId || u.UserEmail == loginId)
                                                       && u.LoginPwd == md5Pwd
                                                       && u.IsAble == true).FirstOrDefault();
                if (user == null)
                {
                    return Content("用户名密码错误！");
                }
                else
                {
                    //第三步：保存用户登录状态到Session，跳转到Home/Index主页面
                    //用户memcached模拟Session
                    Guid guid = Guid.NewGuid();
                    //以guid为key，以登录用户为value放到mm里面去。
                    Common.CacheHelper.Add(guid.ToString(), user, DateTime.Now.AddMinutes(20));
                    //把guid写到cookie里面去
                    Response.Cookies["mysessionId"].Value = guid.ToString();

                    //第四步，记住我功能。
                    if (!string.IsNullOrEmpty(loginkeeping))
                    {
                        HttpCookie loginIdCookie = new HttpCookie("N", loginId);
                        HttpCookie loginpwdCookie = new HttpCookie("W", md5Pwd);
                        loginIdCookie.Expires = DateTime.Now.AddDays(7);
                        loginpwdCookie.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(loginIdCookie);//不用明显意义的词汇
                        Response.Cookies.Add(loginpwdCookie);//不用明显意义的词汇
                    }
                    return Content("ok");
                }

            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="usernamesignup"></param>
        /// <param name="useridsignup"></param>
        /// <param name="emailsignup"></param>
        /// <param name="passwordsignup"></param>
        /// <returns></returns>
        public ActionResult Registe(string usernamesignup, string useridsignup, string emailsignup, string passwordsignup)
        {
            var user =
                UserLoginBll.ORMLoadEntities(u => (u.LoginId == useridsignup || u.UserEmail == emailsignup)).FirstOrDefault();
            if (user != null)//已有
            {
                return Content("对不起，账号 或 Email已被占用");
            }
            else
            {
                PermissUserLogin modelUser = new PermissUserLogin();
                modelUser.LoginId = useridsignup;
                modelUser.UserName = usernamesignup;
                modelUser.LoginPwd = DESEncryptHelper.GetStringMD5(passwordsignup);
                modelUser.UserEmail = emailsignup;
                modelUser.IsAble = false;
                modelUser.AddDate = null;
                modelUser.PhotoPath = "default.png";
                int r = UserLoginBll.ORMAdd(modelUser);
                if (r > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("注册失败，请与管理员联系");
                }
            }

        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowVCode()
        {
            Common.ValidateCodeHelper codeHelper = new ValidateCodeHelper();
            int vCodeLength = int.Parse(ConfigurationManager.AppSettings["VCodeLength"]);
            string strCode = codeHelper.CreateValidateCode(vCodeLength);
            
            Session["LoginCode"] = strCode;

            byte[] data = codeHelper.CreateValidateGraphic2(strCode);
            return File(data, "image/jpeg");
        }
    }
}