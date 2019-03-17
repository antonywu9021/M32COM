using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel;
namespace WebManager.Controllers
{
    public class AdminController : CommonController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult top()
        {
           
            return View();
        }
        public ActionResult left()
        {
            return View();
        }
        public ActionResult right()
        {
             
            return View();//Tuple.Create(Temp1,Temp2,Temp3));
        }
        public ActionResult icofont()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

    }
}
