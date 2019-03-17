using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager
{
    public class PagingInfo
    {
       
        public int total { get; set; }
        
        public int PageSize { get; set; }
      
        public int PageIndex { get; set; }
        public string ReturnUrl{ get; set; }
        
        public int pages
        {
            get { return (int)Math.Ceiling((decimal)total / (PageSize == 0 ? 1 : PageSize)); }
        }
    }
}