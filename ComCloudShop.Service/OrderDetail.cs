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
    
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public string OrderNum { get; set; }
        public int AddressId { get; set; }
        public string Payway { get; set; }
        public decimal Carriage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AcutalCarriage { get; set; }
        public decimal PayableAmount { get; set; }
        public string PayNum { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
        public string LogisticsType { get; set; }
        public string ExpressNum { get; set; }
        public int MemberId { get; set; }
        public byte Stutas { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public bool IsCancle { get; set; }
        public bool IsDelete { get; set; }
        public string ProductArrs { get; set; }
        public string BuyNums { get; set; }
        public string ErpOrderId { get; set; }
        public string Message { get; set; }
        public int CouponId { get; set; }
        public string Receipt { get; set; }
        public string ReceiptType { get; set; }
        public string Original { get; set; }
        public string Jifen { get; set; }
        public string SPGG { get; set; }
    }
}