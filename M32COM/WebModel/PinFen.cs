using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class PinFen
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int SaiShiID { get; set; }
        public int DepartmentID { get; set; }
        public decimal Score { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateID { get; set; }
        public int Status { get; set; }
    }
}
