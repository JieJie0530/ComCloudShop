using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class CommissionViewModel
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public string OrderID { get; set; }
        public decimal Price { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }
    }

    public class CommissionViewModelDetia
    {
        public int ID { get; set; }
        public string Phone { get; set; }

        public string NikeName { get; set; }

        public string OrderID { get; set; }
        public decimal Price { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }
    }
}
