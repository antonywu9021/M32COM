using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
   public class SaiShi
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string JianJie { get; set; }
        public string PinFenGuiZe { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsDelete { get; set; }  
        public int CreateID { get; set; }
        public int Status { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string img4 { get; set; }
        public string img5 { get; set; }
        public string Remark { get; set; }
        public bool IsAblePinFen { get; set; }
    }
}
