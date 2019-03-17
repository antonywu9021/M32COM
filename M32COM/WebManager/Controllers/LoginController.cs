using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using WebModel;
namespace WebManager.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginForm(WebModel.User model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.UserPassword))
            {
                return Content("UserName Or Password  Can't Empty");
            }
            else
            {
                var loginUser = M<User>.SingleWhere(" IsDelete=0 and  username=@code", new SqlParameter[] { new SqlParameter("code", System.Data.SqlDbType.VarChar) { Value = model.UserName } });
                if (loginUser.ID==0)
                {
                    return Content("UserName Or Password is Error");
                }
                else if (Md5.JiaMi(model.UserPassword) != loginUser.UserPassword)
                {
                    return Content("UserName Or Password is Error");
                }
                else
                {
                    loginUser.UserPassword = "";
                    Session["WebAdminUser"] = loginUser;
                    return Content("ok|/admin/index");
                }
            }
        }
        public ActionResult UpPwd()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult UpPwdForm(WebModel.User model)
        {
            var temp = Session["WebAdminUser"] as WebModel.User;
            var tmpmpdel = M<User>.SingleWhere(" IsDelete=0 and  id=" + temp.ID);
            if (tmpmpdel.UserPassword != Md5.JiaMi(model.OldUserPassword))
            {
                return Content("Oldpassword is error");
            }
            else
            {
                tmpmpdel.UserPassword = Md5.JiaMi(model.UserPassword);
                return Content(tmpmpdel.UpdateChanges() ? "true" : "false");
            }
        }
        public ActionResult LoginOut()
        {
            Session["RoleMenu"] = null;
            Session["WebAdminUser"] = null;
            return Redirect("/login/index");
        }
        public ActionResult Register()
        {
            return View("~/Views/Admin/User Registration.cshtml");
        }
        public ActionResult RegisterForm(WebModel.User model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.UserPassword))
            {
                return Content("UserName Or Password  Can't Empty");
            }
            else
            {
                var loginUser = M<User>.SingleWhere(" IsDelete=0  and  username=@code", new SqlParameter[] { new SqlParameter("code", System.Data.SqlDbType.VarChar) { Value = model.UserName } });
                if (loginUser.ID != 0)
                {
                    return Content("You Has Register");
                }
                else
                {
                    
                        model.IsDelete=0;
                        model.UserPassword=Md5.JiaMi(model.UserPassword);
                        model.UserType=2; 
                        model.DepartmentID=2;
                        model.CreateTime=DateTime.Now;
                        model.Status=0;
                        Session["WebAdminUser"] = model;
                        return Content(model.InsertChanges()>0? "true":"false");
                }
            }
        }
    }
}
