using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel;
namespace WebManager.Controllers
{
    public class UserController : CommonController
    {
        //
        // GET: /User/

        public ActionResult Index(int PageIndex=1)
        {
            var temp = PageView<User>.Page(M<User>.WhereSql("select a.*,b.Name DepartmentName,c.Name as SaiShiName,(case when usertype=1 then 'ScoreUser' else '参赛User' end) as UserTypeName from " + DAConfig.GetQz() + "User a left join  " + DAConfig.GetQz() + "Department b on a.DepartmentID=b.ID and b.IsDelete=0 left join  " + DAConfig.GetQz() + "SaiShi c on c.ID=a.SaiShiID  where  a.UserType>0  and  a.IsDelete=0  "), PageIndex, DAConfig.PageSize, "/user/index");
            if (!Request.IsAjaxRequest())
            {
                return View(temp);
            }
            else
            {
                return PartialView("Index_Partial",temp);
            }
        }
        public ActionResult EditUser(int id)
        {
            var temp = M<User>.SingleSql("select a.*,b.Name DepartmentName from " + DAConfig.GetQz() + "User a left join  " + DAConfig.GetQz() + "Department b on a.DepartmentID=b.ID where  a.UserType>1  and  a.IsDelete=0  and   a.ID=" + id);
            var department = M<Department>.Where("IsDelete=0");
            var saishi = M<SaiShi>.Where("IsDelete=0");
            List<SelectListItem> select = new List<SelectListItem>();
            List<SelectListItem> select2 = new List<SelectListItem>();
            select.Add(new SelectListItem()
            {
                Value = "0",
                Text = "----Default----",
                Selected = false
            });
            select2.Add(new SelectListItem()
            {
                Value = "0",
                Text = "----Default----",
                Selected = false
            });
            foreach (var nn in saishi)
            {
                select2.Add(new SelectListItem()
                {
                    Value = nn.ID.ToString(),
                    Text = nn.Name,
                    Selected = temp.SaiShiID == nn.ID ? true : false
                });
            }
            foreach (var mm in department)
            {
                select.Add(new SelectListItem()
                {
                    Value = mm.ID.ToString(),
                    Text = mm.Name,
                    Selected = temp.DepartmentID == mm.ID ? true : false
                });
            }
            ViewData["saishiselect"] = select2;
            ViewData["menuselect"] = select;
            List<SelectListItem> roleselect = new List<SelectListItem>();
            roleselect.Add(new SelectListItem()
            {
                Value = "2",
                Text = "----Participants----",
                Selected = temp.UserType ==2 ? true : false
            });
            roleselect.Add(new SelectListItem()
            {
                Value = "1",
                Text = "----Scoring Officer----",
                Selected = temp.UserType == 1 ? true : false
            });
            ViewData["roleelect"] = roleselect;
            return View(temp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditUserForm(User model)
        {
            if (model.ID == 0)
            {
                if (M<User>.Where(" IsDelete=0 and  username=@name", model.UserName.GetParameter("name")).Count > 0)
                {
                    return Content("UserName is Exist");
                }
                else if (model.UserType == 2 && model.SaiShiID == 0)
                {
                    return Content("Please Choice Your Match");
                }
                else if (model.UserType==2 && model.DepartmentID == 0)
                {
                    return Content("Please Choice Group");
                }
                else
                {
                    model.IsDelete = 0;
                    model.CreateID = DAConfig.MyUser().ID;
                    model.CreateTime = DateTime.Now;
                    model.UserPassword = Md5.JiaMi(model.UserPassword);
                    return Content(model.InsertChanges()>0? "true":"false");
                }
            }
            else
            {
                if (M<User>.Where(" IsDelete=0 and  username=@name and ID!=" + model.ID, model.UserName.GetParameter("name")).Count > 0)
                {
                    return Content("UserName is Exist");
                }
                else if (model.UserType == 2 && model.SaiShiID == 0)
                {
                    return Content("Please Choice Your Match");
                }
                else if (model.UserType == 2 && model.DepartmentID == 0)
                {
                    return Content("Please Choice Group");
                }
                else
                {
                    var temp=M<User>.SingleWhere("ID=" + model.ID);
                    temp.UpdateID = DAConfig.MyUser().ID;
                    temp.UpdateTime = DateTime.Now;
                    temp.TrueName = model.TrueName;
                    temp.Remark = model.Remark;
                    temp.DepartmentID = model.DepartmentID;
                    temp.Remark = HttpUtility.UrlDecode(model.Remark);
                    temp.Phone = model.Phone;
                    temp.Age = model.Age;
                    temp.Email = model.Email;
                    temp.UserType = model.UserType;
                    return Content(temp.UpdateChanges() ? "true" : "false");
                }
            }
        }
        public ActionResult DelUser(int id)
        {
            var Temp = M<User>.SingleWhere(" IsDelete=0 and  ID=" + id);
            Temp.IsDelete =1;
            return Content(Temp.UpdateChanges()? "true":"Delete Error");
        }
    }
}
