using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class GiftViewModel
    {
        public int ID { get; set; }
        public string MemberID { get; set; }
        public string ProductID { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime LQTime { get; set; }
        public string OrderID { get; set; }
        public string ManagerID { get; set; }
        public int State { get; set; }
        public string NickName { get; set; }
        public string SPMC { get; set; }
        public string P1 { get; set; }
    }
}
