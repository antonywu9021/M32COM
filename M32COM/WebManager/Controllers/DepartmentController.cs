using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel;
namespace WebManager.Controllers
{
    public class DepartmentController : CommonController
    {
        //
        // GET: /Department/

        public ActionResult Index(int PageIndex = 1)
        {
            var temp = PageView<Department>.Page(M<Department>.Where("IsDelete=0"), PageIndex, DAConfig.PageSize, "/department/index");
            if (!Request.IsAjaxRequest())
            {
                return View(temp);
            }
            else
            {
                return PartialView("Index_Partial", temp);
            }
        }
        public ActionResult EditDepartment(int id)
        {
            var temp = M<Department>.SingleWhere("ID=" + id);
            var saishi = M<SaiShi>.Where("IsDelete=0");
            List<SelectListItem> select2 = new List<SelectListItem>();
            foreach (var nn in saishi)
            {
                select2.Add(new SelectListItem()
                {
                    Value = nn.ID.ToString(),
                    Text = nn.Name,
                    Selected = temp.SaiShiID == nn.ID ? true : false
                });
            }
            ViewData["saishiselect"] = select2;
            return View(temp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDepartmentForm(Department model)
        {
            if (model.ID == 0)
            {
                if (model.SaiShiID == 0)
                {
                    return Content("Please Choice Your Match");
                }
                else if (M<Department>.Where(" IsDelete=0  and  SaiShiID=" + model.SaiShiID +"  and  name =@name  ", model.Name.GetParameter("name")).Count > 0)
                {
                    return Content("Group Is  Exist");
                }
                else
                {
                    model.IsDelete = 0;
                    model.CreateID = DAConfig.MyUser().ID;
                    model.CreateTime = DateTime.Now;
                    return Content(model.InsertChanges() > 0 ? "true" : "false");
                }
            }
            else
            {
                if (model.SaiShiID == 0)
                {
                    return Content("Please Choice Your Match");
                }
                else if (M<Department>.Where(" IsDelete=0  and  SaiShiID=" + model.SaiShiID + "   and  name=@name   and ID!=" + model.ID, model.Name.GetParameter("name")).Count > 0)
                {
                    return Content("Group Is  Exist");
                }
                else
                {
                    var temp = M<Department>.SingleWhere("ID=" + model.ID);
                    temp.Name =model.Name;
                    temp.SaiShiID = model.SaiShiID;
                    return Content(temp.UpdateChanges() ? "true" : "false");
                }
            }
        }
        public ActionResult DelDepartment(int id)
        {
            var Temp = M<Department>.SingleWhere("ID=" + id);
            Temp.IsDelete = 0;
            return Content(Temp.UpdateChanges() ? "true" : "Delete Error");
        }
    }
}
