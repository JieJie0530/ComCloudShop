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
    
    public partial class OrderComment
    {
        public int ID { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string OrderNum { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Contents { get; set; }
        public string Pics { get; set; }
        public Nullable<int> ProductScore { get; set; }
        public Nullable<int> ProductPackaging { get; set; }
        public Nullable<int> DeliverySpeed { get; set; }
        public string Remack { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public string UserName { get; set; }
    }
}
