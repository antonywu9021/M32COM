using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using Microsoft.International.Converters.PinYinConverter;
using System.Web.Routing;
namespace WebManager
{
    public class DAConfig
    {
        
        public static int PageSize
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]); }
        }
        
        public static string ErrorMsg
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ErrorMsg"]; }
        }
       
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            if (length > 0 && strText.Length > length)
            {
                return strText.Substring(0, length)+"..";
            }
            else
            {
                return strText;
            }
        }
        
        public static string ImageUrl
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ImageUrl"]; }
        }
        public static string GetFileRoot
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["FilePath"]; }
        }
        public static string GetFilePath
        {
            get { return HttpContext.Current.Server.MapPath("")+System.Configuration.ConfigurationManager.AppSettings["FilePath"]; }
        }
        
        public static string GetPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }
        
        public static string GetFirstPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }
        
        private static string RndNum(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(',');
            string VNum = "";  
            int temp = -1;  
           
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(VcArray.Length);
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }
        
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result) || !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                return "127.0.0.1";
            return result;
        }
       
        public static string GetQz()
        {
            return DbHelperSQL._QianZhui;
        }
        public string StripHT(string strHtml)  
        {
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = regex.Replace(strHtml, ""); 
            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("&nbsp;", "");
            return strOutput;
        }
        public static WebModel.User MyUser()
        {
            return (WebModel.User)HttpContext.Current.Session["WebAdminUser"];
        }
        
        public static string HttpGet(string Url)
        {
            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;// (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string retString = stream.ReadToEnd();
            // string retString = myStreamReader.ReadToEnd();
            //myStreamReader.Close();
            // myResponseStream.Close();
            return retString;

           

        }
        public static string HttpPost(string Url, string postDataStr)
        {
            string ret = string.Empty;
            Encoding code = Encoding.UTF8;
            byte[] byteArray = code.GetBytes(postDataStr);  
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";

            webReq.ContentLength = byteArray.Length;
            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length); 
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            ret = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
            return ret;
        }
    }
}