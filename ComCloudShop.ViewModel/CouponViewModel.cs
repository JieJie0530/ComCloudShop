using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class UserCouponViewModel
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public int Amount { get; set; }

        public string ConsumVal { get; set; }

        public string IsUseVal { get; set; }

        public string Original { get; set; }

        public string OrderId { get; set; }
        public string OpenId { get; set; }
    }

    public class CouponDetailViewModel
    {
        public int CouponId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CouponListViewModel
    {
        public int CouponId { get; set; }
        public bool IsCanUse { get; set; }
        public string S1 { get; set; }
        public string S2 { get; set; }
        public string S3 { get; set; }
    }
    public class UserCouponInfoViewModel
    {
        public string NickName { get; set; }
        public string OpenId { get; set; }
        public int Gender { get; set; }
        public int Amount { get; set; }
        public string ConsumVal { get; set; }
        public string CouponCode { get; set; }
        public string Original { get; set; }
        public int IsUse { get; set; }
        public int Consum { get; set; }
        public int ValidityDate { get; set; }
        public int PeriodDate { get; set; }

        public int CreateDate { get; set; }
    }


    public class AdminUserCouponListViewModel
    {
        public int CouponId { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public string Mobile { get; set; }
        public int Amount { get; set; }
        public int Consum { get; set; }
        public string Original { get; set; }
        public int IsUse { get; set; }
    }

    public class AdminUserCouponDetailViewModel
    {
        public int CouponId { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public string Mobile { get; set; }
        public int Amount { get; set; }
        public int Consum { get; set; }
        public string Original { get; set; }
        public int IsUse { get; set; }

        public string OrderNum { get; set; }
        public Nullable<int> CreateDate { get; set; }
    }

    public class AdminUserCouponAddViewModel
    {
        public string OpenId { get; set; }
        public int Amount { get; set; }
        public int Consum { get; set; }
        public string Original { get; set; }
    }

}
