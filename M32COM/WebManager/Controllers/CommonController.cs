using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebManager.Controllers
{
    public class CommonController : Controller
    {
        protected WebModel.User _m_User;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.HttpContext.Session["WebAdminUser"] != null)
            {
                string url = requestContext.HttpContext.Request.RawUrl.ToLower();
                url = url.IndexOf("?times") > 0 ? url.Substring(0, url.IndexOf("?times")) : url;
                url = url.IndexOf("&times") > 0 ? url.Substring(0, url.IndexOf("&times")) : url;
                _m_User = (WebModel.User)requestContext.HttpContext.Session["WebAdminUser"];
                //var temp = _m_User.Where(pp => url.Contains(pp.Url)).ToList();
                //if (_m_User.UserType !=1)
                //{
                //    _m_User.IsAddBtn = temp.Count > 0 ? temp[0].IsAddBtn : false;
                //    _m_User.IsEditBtn = temp.Count > 0 ? temp[0].IsEditBtn : false;
                //    _m_User.IsDelBtn = temp.Count > 0 ? temp[0].IsDelBtn : false;
                //}
                //else
                //{
                //    _m_User.IsAddBtn = true;
                //    _m_User.IsEditBtn = true;
                //    _m_User.IsDelBtn = true;
                //}
            }
            else if (requestContext.HttpContext.Request.IsAjaxRequest())
            {
                requestContext.HttpContext.Session["ReturnUrl"] = requestContext.HttpContext.Request.RawUrl;
                requestContext.HttpContext.Session["WebAdminUser"] = null;
                requestContext.HttpContext.Session["RoleMenu"]=null;
                requestContext.HttpContext.Response.Write("CS00|/login/index");
                requestContext.HttpContext.Response.End();
                return;
            }
            else
            {
                requestContext.HttpContext.Session["ReturnUrl"] = requestContext.HttpContext.Request.RawUrl;
                requestContext.HttpContext.Session["WebAdminUser"] = null;
                requestContext.HttpContext.Session["RoleMenu"] = null;
                requestContext.HttpContext.Response.Redirect("~/login/index");
                return;
            }
            base.Initialize(requestContext);
        }
    }
}
