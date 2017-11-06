using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class IntViewModel
    {
        public int ID { get; set; }
        public string MemberID { get; set; }
        public string ManagerID{ get; set; }
        public decimal Price { get; set; }
        public int State { get; set; }
        public DateTime AddTime { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
    }
}
