using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{

    #region Add 2017

    /// <summary>
    /// 产品列表ViewModel
    /// </summary>
    public class AdminProductListViewModel
    {
        public int Weight { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 商品代码
        /// </summary>
        public string SPDM { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string SPMC { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Sale { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
    }


    /// <summary>
    /// 产品列表ViewModel
    /// </summary>
    public class AdminProductDetailViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// 商品代码
        /// </summary>
        public string SPDM { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string SPMC { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Sale { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describle { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 月龄 开始
        /// </summary>
        public int BeginUseAge { get; set; }
        /// <summary>
        /// 月龄 结束
        /// </summary>
        public int EndUseAge { get; set; }
        /// <summary>
        /// 品种ID
        /// </summary>
        public Nullable<int> CategoryId { get; set; }
        /// <summary>
        /// 正面图
        /// </summary>
        public string P1 { get; set; }
        /// <summary>
        /// 侧面
        /// </summary>
        public string P2 { get; set; }
        /// <summary>
        /// 背面
        /// </summary>
        public string P3 { get; set; }
        /// <summary>
        /// 产品详细描述
        /// </summary>
        public string Contents { get; set; }

        public string SPGG { get; set; }

    }

    /// <summary>
    /// 产品详情 ViewModel
    /// </summary>
    public class AdminProductItemDetailViewModel
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
    }

    public class SelectViewModel
    {
        public int dm { get; set; }
        public string mc { get; set; }
    }

    #endregion













    public class ProductViewModel
    {
        public int ProductId { get; set; }
        
        public string ProductGuid { get; set; }
        
        public string Title { get; set; }
        
        public string SPDM { get; set; }
        
        public string SPMC { get; set; }
        
        public string FKCCK { get; set; }
        
        public decimal BZSJ { get; set; }

        public decimal Sale { get; set; }

        public decimal Discount { get; set; }

        public int Status { get; set; }

        public int BeginUseAge { get; set; }

        public int EndUseAge { get; set; }

        public string SubTitle { get; set; }

        public string Describle { get; set; }

        public decimal  Weight { get; set; }


        public string Contents { get; set; }

        public string P1 { get; set; }
        
        public string P2 { get; set; }
        
        public string P3 { get; set; }
    }

    public class ProductDetailViewModel
    {
        public int DetailId { get; set; }

        public int ProductId { get; set; }

        public string A1 { get; set; }

        public string A2 { get; set; }

        public string A3 { get; set; }

        public byte DetailType { get; set; }
    }

    public class ProductImgViewModel
    {
        public int ImgId { get; set; }
        
        public int ProductId { get; set; }

        public string P1 { get; set; }

        public string P2 { get; set; }

        public string P3 { get; set; }
    }

    public class ProductGroupViewModel
    {
        public int GroupId { get; set; }
        
        public string GroupName { get; set; }

        public string Content { get; set; }

        public string Suggest { get; set; }

        public string PicUrl { get; set; }
    }

    public class ProductUpModel 
    {
        public int ProductId { get; set; }

        public string SPMC { get; set; }

        public decimal Sale { get; set; }

        public decimal Discount { get; set; }

        public int BeginUseAge { get; set; }

        public int EndUseAge { get; set; }

        public string SubTitle { get; set; }

        public string Describle { get; set; }

        public decimal Weight { get; set; }

        public string ProductCategory { get; set; }

        public string ProductFunction { get; set; }

        public string Contents { get; set; }
    }

    public class ProductGroupRelationViewModel
    {
        public int GroupId { get; set; }
        public int RelationId { get; set; }
        public int ProductId { get; set; }
        public string GroupName { get; set; }
        public string Content { get; set; }
        public string Suggest { get; set; }
        public string PicUrl { get; set; }
        public string SPDM { get; set; }
        public string SPMC { get; set; }
    }

    public class DetailViewModel
    {
        public int DetailId { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
    }

    public class ProductDetailResultModel
    {
        public int ProductId { get; set; }
        public string ProductGuid { get; set; }
        public string Title { get; set; }
        public string SPDM { get; set; }
        public string SPMC { get; set; }
        public Nullable<decimal> Sale { get; set; }
        public decimal Discount { get; set; }
        public Nullable<int> BeginUseAge { get; set; }
        public Nullable<int> EndUseAge { get; set; }
        public string SubTitle { get; set; }
        public string Describle { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public int ImgId { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }

        public int? DetailId { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryType { get; set; }

        public string Contents { get; set; }

    }

    public class ProductExtendResultModel
    {
        public int ProductId { get; set; }
        public string ProductGuid { get; set; }
        public string Title { get; set; }
        public string SPDM { get; set; }
        public string SPMC { get; set; }
        public Nullable<decimal> Sale { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<int> BeginUseAge { get; set; }
        public Nullable<int> EndUseAge { get; set; }
        public string SubTitle { get; set; }
        public string Describle { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public int ImgId { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }

        public string Contents { get; set; }
        public List<DetailViewModel> Detail { get; set; }
        public List<CategoryResultModel> Category { get; set; }
    }

    public class CategoryResultModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }

    }

    public class ProductGroupResultModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Content { get; set; }
        public string Suggest { get; set; }
        public string PicUrl { get; set; }
        public int ProductId { get; set; }
        public string SPMC { get; set; }
        public Nullable<decimal> Sale { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string SubTitle { get; set; }
        public string Describle { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }

        public Guid random { get; set; }

    }

    public class ProductSaleGroupResultModel
    {
        public int RelationId { get; set; }
        public int SaleId { get; set; }
        public string SaleName { get; set; }
        public int SaleOrder { get; set; }
        public string PicSmallUrl { get; set; }
        public string PicBigUrl { get; set; }
        public int ProductId { get; set; }
        public string SPMC { get; set; }
        public Nullable<decimal> Sale { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string SubTitle { get; set; }
        public string Describle { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }

        public Guid random { get; set; }

    }

    public class SaleDetailViewModel
    {
        public int SaleId { get; set; }
        public string PicBigUrl { get; set; }
    }

    public class SaleProductListModel
    {
        public int ProductId { get; set; }
        public string SPMC { get; set; }
        public decimal Sale { get; set; }
        public decimal Discount { get; set; }
        public string Describle { get; set; }
        public string Pic { get; set; }
    }


    public class ProductGroupSaleViewModel
    {
        public int saleId { get; set; }
        public string saleName { get; set; }
        public string picSmallUrl { get; set; }
        public string picBigUrl { get; set; }
        public int saleOrder { get; set; }
    }

    public class ProductSaleGroupNewViewModel
    {
        public int SaleId { get; set; }
        public string SaleName { get; set; }
        public string PicSmallUrl { get; set; }
        public string PicBigUrl { get; set; }
        public int SaleOrder { get; set; }
    }

    /// <summary>
    /// 活动首页
    /// </summary>
    public class HomeViewModel
    {
        public IEnumerable<BatchViewModel> batch { get; set; }
        public IEnumerable<SaleViewModel> sale { get;set; }
    }

    public class SaleViewModel
    {
        public int id { get; set; }
        public string url { get; set; }
    }

    public class BatchViewModel
    {
        public string url { get; set; }
        public string pic { get; set; }
        public string Useful { get; set; }

        public List<ComCloudShop.ViewModel.ProductListViewModel> list { get; set; }

    }


    public class GroupSaleRelationViewModel
    {
        public int relationId { get; set; }
        public int productId { get; set; }
        public int saleId { get; set; }
        public int relationOrder { get; set; }
    }

    /// <summary>
    /// 产品列表页Model
    /// </summary>
    public class ProductListViewModel
    {
        public int ProductId { get; set; }
        public string SPMC { get; set; }
        public string Pic { get; set; }
        public string Describle { get; set; }
        public int BeginUseAge { get; set; }
        public int EndUseAge { get; set; }
        public decimal Sale { get; set; }
        public decimal Discount { get; set; }

        public int Weight { get; set; }

        public decimal Weigths { get; set; }

        public Nullable<int> SaleNum { get; set; }
    }

    /// <summary>
    /// 最新推荐产品列表
    /// </summary>
    public class SaleProductNewViewModel
    {
        public int RelationId { get; set; }
        public int ProductId { get; set; }
        public string SPDM { get; set; }
        public string SPMC { get; set; }
    }

}
