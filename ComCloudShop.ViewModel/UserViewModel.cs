using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class UserIndexViewModel
    {
        public string username { get; set; }
        public string url { get; set; }
        public decimal number { get; set; }
        public string phone { get; set; }
        public string openid { get; set; }
        public string balance { get; set; }
        public string follow { get; set; }
        public string Cashbalance { get; set; }
        public decimal integral { get; set; }
        public decimal TotalIn { get; set; }
    }

    /// <summary>
    /// 用户个人信息Model
    /// </summary>
    public class UserEditInfoViewModel
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string Birthday { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }




}
