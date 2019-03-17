using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebModel; 
namespace WebManager.Controllers
{
    public class UpLoadController : CommonController
    {
            public ActionResult UpLoadFile(string id = "", string fujianname="", string name = "", string type = "", string lastModifiedDate = "", int size = 0, HttpPostedFileBase file = null)
            {
                String savePath = "/UpLoad/";
                
                Hashtable extTable = new Hashtable();
                extTable.Add("image", "jpg,jpeg,png,bmp");
                //extTable.Add("flash", "swf,flv");
                //extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
                
                 
                int maxSize = 100000000;

                //var imgFile = HttpContext.Request.Files["imgFile"];
                if (file == null)
                {
                    return Json(new
                    {
                        jsonrpc = "2.0",
                        id = id,
                        filePath = "Please Choice Your Image" 
                    });
                }

                if (file.InputStream == null || file.InputStream.Length > maxSize)
                {
                    return Json(new
                    {
                        jsonrpc = "2.0",
                        id = id,
                        filePath = "100M" 
                    });
                }
                String ymd = DateTime.Now.ToString("yyyy_MM_dd", DateTimeFormatInfo.InvariantInfo);
                string dirName = ymd;
                String fileName = file.FileName;
                String fileExt = Path.GetExtension(fileName).ToLower();
                String newFileName = new Random().Next(10000, 90000) + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_fff", DateTimeFormatInfo.InvariantInfo) + fileExt;
                string[] array = {"jpg", "jpeg", "png", "bmp"};
                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(array, fileExt.Substring(1).ToLower()) == -1)
                {
                    return Json(new
                    {
                        jsonrpc = "2.0",
                        id = id,
                        filePath = "Must be image"
                    });
                }
                string types=fileExt.Substring(1).ToLower();
                string resulturl = "";
                resulturl =   "/images/" + types + ".jpg";
                String dirPath = HttpContext.Server.MapPath(savePath);
               
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                dirPath += fujianname == "" ? ymd + "\\" : fujianname + "\\";
                if (!Directory.Exists(dirPath)) 
                {
                    Directory.CreateDirectory(dirPath);
                }
                string fileUrl = savePath + (fujianname == "" ? ymd + "/" : fujianname + "/") + newFileName;
                file.SaveAs(System.IO.Path.Combine(dirPath, newFileName));  //gif,jpg,jpeg,png,bmp
                if (Array.IndexOf(array, fileExt.Substring(1).ToLower()) >= 0 && (types == "jpeg" || types == "jpg" || types == "gif" || types == "png" || types == "bmp"))
                {
                    resulturl = fileUrl;
                }
                return Json(new
                {
                    jsonrpc = "2.0",
                    id = id,
                    filevalue = fileUrl,
                    filename=fileName,
                    filePath = resulturl 
                });
            }
            public ActionResult DelImg(string filename,string filepathname)
            {
                string[] array = filepathname.Split('/');
                string localPath = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, @"UpLoad\" + array[2] + @"\" + array[3]);
                if (System.IO.File.Exists(localPath))
                {
                    System.IO.File.Delete(localPath);
                    //ZxwModel.User temp = DBQuery<ZxwModel.User>.Single("Id=" + _m_User.Id);
                    //temp.ImgUrl = "";
                    //temp.UpdateChanges();
                }
                if (!string.IsNullOrEmpty(filename))
                {
                    var temp = M<SaiShi>.SingleWhere("" + filename + " = '" + filepathname + "'");
                    if (temp.ID != 0)
                    {
                        switch (filename)
                        {
                            case "img1": temp.img1 = ""; break;
                            case "img2": temp.img2 = ""; break;
                            case "img3": temp.img3 = ""; break;
                            case "img4": temp.img4 = ""; break;
                            case "img5": temp.img5 = ""; break;
                        }
                        temp.UpdateChanges();
                    }
                }
                return Json(new
                {
                    result = "true" 
                });
            }
     }
}
