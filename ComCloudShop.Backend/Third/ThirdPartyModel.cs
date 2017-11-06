using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComCloudShop.Backend
{
    public class ThirdProductModel
    {
        public int id { get; set; }
        public string guid { get; set; }
        public string spdm { get; set; }
        public string spmc { get; set; }
        public string fkcck { get; set; }
        public string zl { get; set; }
        public string bzsj { get; set; }
    }

    /// <summary>
    /// 管易ERPViewModel
    /// </summary>
    public class AdminGYERPViewModel
    {
        /// <summary>
        /// 商品ID编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 商品GUID编码
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 商品代码
        /// </summary>
        public string SPDM { get; set; }
        /// <summary>
        /// 商品简称
        /// </summary>
        public string SPJC { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string SPMC { get; set; }
        /// <summary>
        /// 是否停用 ( 0-否,1-是 )
        /// </summary>
        public string TY { get; set; }
        /// <summary>
        /// 是否上架。( 0-否,1-是 )
        /// </summary>
        public string SJ { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public string ZL { get; set; }
        /// <summary>
        /// 标准售价
        /// </summary>
        public string BZSJ { get; set; }
    }




}