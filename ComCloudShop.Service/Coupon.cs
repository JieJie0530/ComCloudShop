//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComCloudShop.Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coupon
    {
        public int CouponId { get; set; }
        public string CouponNum { get; set; }
        public string CouponCode { get; set; }
        public Nullable<decimal> PayValue { get; set; }
        public string CouponType { get; set; }
        public Nullable<decimal> ConsumAmount { get; set; }
        public Nullable<System.DateTime> ValidityDate { get; set; }
        public Nullable<System.DateTime> PeriodDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}