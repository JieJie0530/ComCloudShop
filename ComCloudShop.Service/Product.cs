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
    
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductGuid { get; set; }
        public string Title { get; set; }
        public string SPDM { get; set; }
        public string SPMC { get; set; }
        public byte FKCCK { get; set; }
        public Nullable<decimal> BZSJ { get; set; }
        public byte Statuts { get; set; }
        public decimal Sale { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public decimal Discount { get; set; }
        public int BeginUseAge { get; set; }
        public int EndUseAge { get; set; }
        public string SubTitle { get; set; }
        public string Describle { get; set; }
        public decimal Weight { get; set; }
        public string Contents { get; set; }
        public bool IsShow { get; set; }
        public string SPGG { get; set; }
    }
}