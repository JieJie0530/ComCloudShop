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
    
    public partial class Cart
    {
        public int CartId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> MemberId { get; set; }
        public Nullable<int> ProductNum { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> Selected { get; set; }
    }
}
