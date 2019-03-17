using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel;
using System.Data.SqlClient;
namespace WebManager.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var temp = M<SaiShi>.Where("IsDelete=0");
            string str = "[";
            foreach(var mm in temp)
            {
                string url = "/Home/XuanChuanYe?saishiid=" + mm.ID;
                str += "{";
                str += "  \"title\": \"" + mm.Name + "\",";
                str += " \"start\": new Date(" + mm.BeginTime.Year + "," + (mm.BeginTime.Month - 1) + "," + mm.BeginTime.Day + "),";
                str += " \"end\": new Date(" + mm.EndTime.Year + "," + (mm.EndTime.Month - 1) + "," + mm.EndTime.Day + "),";
                str += "  \"url\": \""+ url + "\"},";
            }
            ViewBag.SaiShi = str.Trim(',')+"]";
            return View();
        }
        public ActionResult XuanChuanYe(int saishiid=0)
        {
            var temp = M<SaiShi>.SingleWhere("IsDelete=0 and ID="+ saishiid);
            return View(temp);
        }
    }
}
