using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class DeliveryAddressViewModel
    {
        public int AddressId { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string UserName { get; set; }

        public string Mobile { get; set; }
       
        public int MemberId { get; set; }
       
        public string MemberName { get; set; }
       
        public bool IsMainAddr { get; set; }
       
        public string MainAddr { get; set; }

        public string Tags { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressDetailViewModel
    {
        public int AddressId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
    }


    public class AddressEditViewModel
    {
        public int addressID { get; set; }
        public string username { get; set; }
        public string mobile { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public string source { get; set; }
    }

    /// <summary>
    /// Admin 列表/详情
    /// </summary>
    public class AdminAddressViewModel
    {
        public int AddressId { get; set; }
        public string NickName { get; set; }
        public bool IsMainAddr { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }

}
