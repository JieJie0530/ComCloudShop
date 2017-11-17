using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }
       
        public string OpenId { get; set; }
       
        public string NickName { get; set; }
       
        public byte Gender { get; set; }
       
        public string City { get; set; }
       
        public string Province { get; set; }
       
        public string Country { get; set; }
       
        public string HeadImgUrl { get; set; }
       
        public string OrignKey { get; set; }
       
        public string Mobile { get; set; }
       
        public string Email { get; set; }

        public string Sex { get; set; }

        public int LastLoginTime { get; set; }

        public string UserName { get; set; }

        public string Birth { get; set; }

        public string QQ { get; set; }

        public string follow { get; set; }
        public string balance { get; set; }
        public string Cashbalance { get; set; }

        public int fsate { get; set; }
        public int ISVip { get; set; }

        public decimal integral { get; set; }
        public decimal TotalIn { get; set; }

    }


    public class AdminReturnListViewModel
    {
        public string OrderNum { get; set; }
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string TrueName { get; set; }
        public string Reason { get; set; }
        public decimal Price { get; set; }
        public string AddTime { get; set; }
        public int State { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }

        public string Bank { get; set; }
        public string BankNumber { get; set; }
    }

    public class AdminWithdrawalsListViewModel {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string TrueName { get; set; }
        public string Bank { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }
        public string AddTime { get; set; }
        public string DZTime { get; set; }
        public int State { get; set; }
        public string Remack { get; set; }
        public string UserName { get; set; }
        public string Moblie { get; set; }
        public string Phone { get; set; }
    }

    public class AdminMemberListViewModel
    {
        public decimal TotalIn { get; set; }
        public int MemberId { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public byte Gender { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string HeadImgUrl { get; set; }
        public string Mobile { get; set; }

        public int IsVip { get; set; }
    }

    public class AdminMemberDetailViewModel
    {
        public int MemberId { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public byte Gender { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactAddr { get; set; }
        public string OrignKey { get; set; }
        public Nullable< int> LastLoginDate { get; set; }
        public string UserName { get; set; }
        public string QQ { get; set; }
        public string Birth { get; set; }
        public string District { get; set; }
    }


}
