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
    
    public partial class GiftList
    {
        public int ID { get; set; }
        public string MemberID { get; set; }
        public string ProductID { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public Nullable<System.DateTime> LQTime { get; set; }
        public string OrderID { get; set; }
        public string ManagerID { get; set; }
        public Nullable<int> State { get; set; }
    }
}