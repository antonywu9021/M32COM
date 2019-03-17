using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
namespace System.Web.Helpers
{
    public  static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                     WebManager.PagingInfo pagingInfo,
                      Func<string,string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            if (pagingInfo.pages > 1)
            {
                pagingInfo.ReturnUrl = pagingInfo.ReturnUrl.IndexOf("?")>0? pagingInfo.ReturnUrl + "&PageIndex=": pagingInfo.ReturnUrl + "?PageIndex=";
                result.Append("<div class=\"pagin\"><div class=\"message\">Sum<i class=\"blue\">" + pagingInfo.total + "</i>Records，Current&nbsp;<i class=\"blue\">" + pagingInfo.PageIndex + "&nbsp;</i>Psge</div>");
                //PrePage
                result.Append("<ul class=\"paginList\">");
                if (pagingInfo.PageIndex == 1)
                {
                    result.Append("<li class=\"paginItem\" disabled=\"true\">");
                    result.Append("<a style=\"color: #797474;\" href=\"javascript:void(0)\" aria-label=\"Previous\" title=\"HomePage\"><span aria-hidden=\"true\">&laquo;</span></a>");
                    result.Append("</li>");
                    result.Append("<li class=\"paginItem\" disabled=\"true\">");
                    result.Append("<a style=\"color: #797474;\" href=\"javascript:void(0)\" aria-label=\"Previous\" title=\"PrePage\"><span aria-hidden=\"true\">‹</span></a>");
                    result.Append("</li>");
                }
                else
                {
                    result.Append("<li class=\"paginItem\">");
                    result.Append("<a href=\"javascript:void(0)\"  aria-label=\"Previous\" title=\"HomePage\" onclick=\"" + pageUrl(pagingInfo.ReturnUrl).Replace("PageIndex=", "PageIndex=" +1) + "\"><span aria-hidden=\"true\">&laquo;</span></a>");
                    result.Append("</li>");
                    result.Append("<li class=\"paginItem\">");
                    result.Append("<a href=\"javascript:void(0)\"  aria-label=\"Previous\"  title=\"PrePage\" onclick=\"" + pageUrl(pagingInfo.ReturnUrl).Replace("PageIndex=", "PageIndex=" + (pagingInfo.PageIndex - 1)) + "\"><span aria-hidden=\"true\">‹</span></a>");
                    result.Append("</li>");
                }
                for (int i = 1; i <= pagingInfo.pages; i++)
                {
                    //内容页  总的要存在9页

                    if (pagingInfo.pages - pagingInfo.PageIndex > 8)
                    {
                        if (i == pagingInfo.pages - 2)
                        {
                            result.Append("<li class=\"paginItem more\">");
                            result.Append("<a href=\"javascript:void(0)\">…</a>");
                            result.Append("</li>");
                            continue;
                        }
                        if (i >= pagingInfo.PageIndex && (i - pagingInfo.PageIndex < 8 || pagingInfo.pages - 2 < i))
                        {
                            if (i == pagingInfo.PageIndex)
                            {
                                result.Append("<li class=\"paginItem current\">");
                                result.Append("<a style=\"color: #797474;\" href=\"javascript:void(0)\"");
                            }
                            else
                            {
                                result.Append(" <li  class=\"paginItem\">");
                                result.Append("<a href=\"javascript:void(0)\" onclick=\"" + pageUrl(pagingInfo.ReturnUrl).Replace("PageIndex=", "PageIndex="+ i) + "\"");
                            }
                            result.Append(">" + i + "</a>");
                            result.Append("</li>");
                        }
                    }
                    else
                    {
                        if (pagingInfo.pages - i < 9)
                        {
                            if (i == pagingInfo.PageIndex)
                            {
                                result.Append(" <li class=\"paginItem current\">");
                                result.Append("<a style=\"color: #797474;\" href=\"javascript:void(0)\"");
                            }
                            else
                            {
                                result.Append("<li class=\"paginItem\">");
                                result.Append("<a href=\"javascript:void(0)\" onclick=\"" + pageUrl(pagingInfo.ReturnUrl).Replace("PageIndex=", "PageIndex="+i) + "\"");
                            }
                            result.Append(">" + i + "</a>");
                            result.Append("</li>");
                        }
                    }
                }
                //NextPage
                if (pagingInfo.PageIndex == pagingInfo.pages)
                {
                    result.Append("<li  class=\"paginItem\">");
                    result.Append(" <a href=\"javascript:void(0)\" style=\"color: #797474;\" aria-label=\"Next\" title=\"NextPage\"><span aria-hidden=\"true\">›</span></a>");
                    result.Append(" <a href=\"javascript:void(0)\" style=\"color: #797474;\" aria-label=\"Next\" title=\"LastPage\"><span aria-hidden=\"true\">&raquo;</span></a>");
                    result.Append("</li>");
                }
                else
                {
                    result.Append("<li  class=\"paginItem\">");
                    result.Append("<a href=\"javascript:void(0)\"  aria-label=\"Next\" title=\"NextPage\" onclick=\"" + pageUrl(pagingInfo.ReturnUrl).Replace("PageIndex=", "PageIndex=" + (pagingInfo.PageIndex + 1)) + "\">›</span></a>");
                    result.Append("</li>");
                    result.Append("<li  class=\"paginItem\">");
                    result.Append("<a href=\"javascript:void(0)\"  aria-label=\"Next\" title=\"LastPage\" onclick=\"" + pageUrl(pagingInfo.ReturnUrl).Replace("PageIndex=", "PageIndex=" +pagingInfo.pages) + "\">&raquo;</span></a>");
                    result.Append("</li>");
                }
                result.Append("</ul>");
                result.Append("</div>");
            }else
            {
                result.Append("");
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}