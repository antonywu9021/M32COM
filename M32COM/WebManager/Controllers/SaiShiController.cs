using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel;

namespace WebManager.Controllers
{
    public class SaiShiController : CommonController
    {
        public ActionResult Index(int PageIndex = 1)
        {
            var Model = M<SaiShi>.Where("isdelete=0  order by id desc");
            Model.ForEach(pp => {
                pp.IsAblePinFen = M<User>.Where(" IsDelete=0 and  UserType=2  and SaiShiID=" + pp.ID).Count > 0 ? true : false; 
            });
            var temp = PageView<SaiShi>.Page(Model, PageIndex, DAConfig.PageSize, "/SaiShi/Index");
            if (!Request.IsAjaxRequest())
            {
                return View(temp);
            }
            else
            {
                return PartialView("Index_Partial", temp);
            }
        }
        public ActionResult EditSaiShi(int id)
        {
            var temp = M<SaiShi>.SingleWhere(" isdelete=0 and ID=" + id);
            return View(temp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditSaiShiForm(SaiShi model)
        {
            if (model.ID == 0)
            {
                if (M<SaiShi>.Where(" IsDelete=0 and  Name=@Name", model.Name.GetParameter("Name")).Count > 0)
                {
                    return Content("Match Is  Exist");
                }
                else if (model.BeginTime >= model.EndTime)
                {
                    return Content("BeginTime Biger EndTimer");
                }
                else if (string.IsNullOrEmpty(model.JianJie))
                {
                    return Content("Abstract is Empty");
                }
                else if (string.IsNullOrEmpty(model.PinFenGuiZe))
                {
                    return Content("Regular is Empty");
                }
                else
                {
                    model.IsDelete = 0;
                    model.CreateID = DAConfig.MyUser().ID;
                    model.Status = 0;
                    model.CreateTime = DateTime.Now;
                    return Content(model.InsertChanges() > 0 ? "true" : "false");
                }
            }
            else
            {
                if (M<SaiShi>.Where(" IsDelete=0 and  Name=@Name and ID!=" + model.ID, model.Name.GetParameter("Name")).Count > 0)
                {
                    return Content("Match Is  Exist");
                }
                else if (model.BeginTime >= model.EndTime)
                {
                    return Content("BeginTime Biger EndTimer");
                }
                else if (string.IsNullOrEmpty(model.JianJie))
                {
                    return Content("Abstract is Empty");
                }
                else if (string.IsNullOrEmpty(model.PinFenGuiZe))
                {
                    return Content("Regular is Empty");
                }
                else
                {
                    var temp = M<SaiShi>.SingleWhere("ID=" + model.ID);
                    temp.BeginTime = model.BeginTime;
                    temp.Name = model.Name;
                    temp.BeginTime = model.BeginTime;
                    temp.EndTime = model.EndTime;
                    temp.JianJie = model.JianJie;
                    temp.PinFenGuiZe = model.PinFenGuiZe;
                    temp.img1 = model.img1;
                    temp.img2=  model.img2;
                    temp.img3 = model.img3;
                    temp.img4 = model.img4;
                    temp.img5 = model.img5;
                    return Content(temp.UpdateChanges() ? "true" : "false");
                }
            }
        }
        public ActionResult DelSaiShi(int id)
        {
            var Temp = M<SaiShi>.SingleWhere("ID=" + id);
            Temp.IsDelete = 1;
            return Content(Temp.UpdateChanges() ? "true" : "Delete Error");
        }
        public ActionResult EndSaiShi(int id)
        {
            var Temp = M<SaiShi>.SingleWhere("ID=" + id);
            var Dep = M<Department>.Where(" IsDelete=0  and  SaiShiID=" + id);
            if (Dep.Count>0 && Dep.Select(pp => pp.Score).Count()>0)
            {
                Temp.Status = 2;
                return Content(Temp.UpdateChanges() ? "true" : "Error Operation!");
            }
            else
            {
                return Content("You Cant't End It!");
            }
        }
        public ActionResult EditPinFen(int id)
        {
            var Temp = M<SaiShi>.SingleWhere(" IsDelete=0  and  ID=" + id);
            var Departments = M<Department>.Where(" IsDelete=0  and  SaiShiID=" + id +"  order by score desc");
            Departments.ForEach(pp =>
            {
                var mm = M<User>.Where(" IsDelete=0 and  UserType=2  and SaiShiID=" + id + " and  DepartmentID=" + pp.ID);
                string users = "";
                foreach (var nn in mm)
                {
                    users += nn.TrueName + " ,";
                }
                pp.SaiShiName = Temp.Name;
                pp.IsAblePinFen = Temp.Status != 2 ? true : false;
                pp.Users = users.Trim(',');
            });
            return View(Departments);
        }
        public ActionResult PinFenForm(int id)
        {
            var temp = M<Department>.SingleWhere(" IsDelete=0  and  ID=" + id);
            return View(temp);
        }
        [HttpPost]
        public ActionResult SaveSaiShiForm(Department model)
        {
            var Departments = M<Department>.SingleWhere(" IsDelete=0  and  ID=" + model.ID);
            Departments.Score = model.Score;
            return Content(Departments.UpdateChanges() ? "true" : "Error Operation!");
        }
    }
}
