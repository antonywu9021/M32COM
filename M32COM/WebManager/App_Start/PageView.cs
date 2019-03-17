using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebManager
{
    public class PageView<T> where T:new()
    {
        public static MyPageInfo<T> Page(List<T> list,int PageIndex=1,int PageSize=10,string Url="")
        {
            MyPageInfo<T> Pages = new MyPageInfo<T>();
            Pages.List = list.Skip(PageSize*(PageIndex-1)).Take(PageSize).ToList<T>();
            Pages.Page = new PagingInfo()
            {
                PageIndex=PageIndex,
                PageSize=PageSize,
                total=list.Count,
                ReturnUrl=Url
            };
            return Pages;
        }
    }
    public class  MyPageInfo<T> where T:new(){
        public List<T> List=new List<T>();
        public PagingInfo Page=new PagingInfo();
    }
}