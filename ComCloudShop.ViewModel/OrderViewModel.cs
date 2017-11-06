using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.ViewModel
{
    public class OrderDetailDisplayModel
    {
        public int OrderId { get; set; }

        public string OrderNum { get; set; }

        public string ProductArrs { get; set; }

        public string BuyNums { get; set; }

        public int AddressId { get; set; }

        public string Payway { get; set; }

        public decimal Carriage { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal AcutalCarriage { get; set; }

        public decimal PayableAmount { get; set; }

        public string PayNum { get; set; }

        public DateTime Createdate { get; set; }

        public DateTime ModifyDate { get; set; }

        public DateTime PayDate { get; set; }

        public string LogisticsType { get; set; }

        public string ExpressNum { get; set; }

        public int MemberId { get; set; }

        public byte Stutas { get; set; }

        public string Status { get; set; }

        public DateTime CompleteDate { get; set; }

        public bool IsCancle { get; set; }

        public bool IsDelete { get; set; }

        public string MemberName { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string UserName { get; set; }

        public string ErpOrderId { get; set; }

        public string Message { get; set; }

        public string Receipt { get; set; }
        
        public string ReceiptType { get; set; }
        
        public string OpenId { get; set; }

        public string SaleArrs { get; set; }

        public string RealSaleArrs { get; set; }

        public string Original { get; set; }
    }

    public class OrderDetailViewModel
    {
        public string OrderNum { get; set; }

        public int AddressId { get; set; }

        public string Payway { get; set; }

        public decimal Carriage { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal AcutalCarriage { get; set; }

        public decimal PayableAmount { get; set; }

        public string LogisticsType { get; set; }

        public int MemberId { get; set; }

        public string Message { get; set; }

        public string Receipt { get; set; }

        public string ReceiptType { get; set; }

        public int CouponId { get; set; }


        public ICollection<CartProductViewModel> CartProducts { get; set; }
    }

    public class OrderDetailResultModel
    {
        public int OrderId { get; set; }
        public string OpenId { get; set; }
        public string OrderNum { get; set; }
        public string MemberName { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }
        public DateTime Createdate { get; set; }
        public decimal Carriage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AcutalCarriage { get; set; }
        public decimal PayableAmount { get; set; }
        public int ProductId { get; set; }
        public int BuyNum { get; set; }
        public string SPMC { get; set; }
        public decimal Sale { get; set; }
        public byte Stutas { get; set; }
        public string Mobile { get; set; }
        public string Title { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string UserName { get; set; }
        public string Receipt { get; set; }
        public string ReceiptType { get; set; }
        public string SPDM { get; set; }
        public string ProductGuid { get; set; }
        public string Message { get; set; }
        public decimal BuySale { get; set; }
        public decimal RealSale { get; set; }
        public string ErpOrderId { get; set; }
        public decimal Discount { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string Original { get; set; }
        public string ProductArrs { get; set; }
    }

    public class OrderResultModel
    {
        public int OrderId { get; set; }
        public string OrderNum { get; set; }
        public string MemberName { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }
        public DateTime Createdate { get; set; }
        public decimal Carriage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AcutalCarriage { get; set; }
        public decimal PayableAmount { get; set; }
        public int Stutas { get; set; }
        public string Mobile { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string UserName { get; set; }
        public string Receipt { get; set; }
        public string ReceiptType { get; set; }
        public string Message { get; set; }
        public string ProductGuid { get; set; }

        public string OpenId { get; set; }
        public List<ProductResultModel> Product { get; set; }
    }

    public class ProductResultModel
    {
        public int ProductId { get; set; }
        public int BuyNum { get; set; }
        public string SPMC { get; set; }
        public decimal Sale { get; set; }
        public decimal BuySale { get; set; }
        public decimal RealSale { get; set; }
        public string Title { get; set; }
        public string SPDM { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
    }

    /// <summary>
    /// order提交页面
    /// </summary>
    public class OrderConfigViewModel
    {
        public IEnumerable<CartListForOrderViewModel> list { get; set; }
        public CouponDetailViewModel coupon { get; set; }
        public AddressDetailViewModel address { get; set; }
        public RuleViewModel rule { get; set; }
    }

    /// <summary>
    /// orderSessionModel
    /// </summary>
    public class OrderParmModel
    {
        /// <summary>
        /// 地址id
        /// </summary>
        public int aid { get; set; }
        /// <summary>
        /// 卡券id
        /// </summary>
        public int cid { get; set; }
        /// <summary>
        /// 卡券list
        /// </summary>
        public int[] list { get; set; }

        /// <summary>
        /// 使用积分
        /// </summary>
        public string Jifen { get; set; }

        /// <summary>
        /// 积分使用比例
        /// </summary>
        public decimal BL { get; set; }
    }

    /// <summary>
    /// 订单列表
    /// </summary>
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public string OrderNum { get; set; }
        public int Status { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderProductViewModel> product { get; set; }
    }

    public class OrderDetailNewViewModel
    {
        public int OrderId { get; set; }
        public string OrderNum { get; set; }
        public int Status { get; set; }
        public string CreatTime { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PayPrice { get; set; }
        public decimal DisCount { get; set; }
        public decimal KDPay { get; set; }
        public string Address { get; set; }
        public string Addressee { get; set; }
        public string KD { get; set; }
        public int Number { get; set; }
        public UserIndexViewModel MemberModel { get; set; }
        public string Jifen { get; set; }
        
        public string opendid { get; set; }
        public IEnumerable<OrderProductViewModel> list { get; set; }
    }

    public class OrderProductViewModel
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 购买价格
        /// </summary>
        public decimal BuySale { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal RealSale { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string SPMC { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describle { get; set; }

        public int Status { get; set; }
    }

    /// <summary>
    /// 提交订单Model
    /// </summary>
    public class OrderAddParmModel
    {
        public string SPGG { get; set; }
        /// <summary>
        /// 勾选的购物车id列表
        /// </summary>
        public int[] CartList { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 地址id
        /// </summary>
        public int AddressId { get; set; }
        /// <summary>
        /// 卡券id
        /// </summary>
        public int CouponId { get; set; }
        /// <summary>
        /// 快递方式
        /// </summary>
        public string KuaiDi { get; set; }
        /// <summary>
        /// 快递费
        /// </summary>
        public decimal Carriage { get; set; }
        /// <summary>
        /// 购物备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 使用积分
        /// </summary>
        public string Jifen { get; set; }

        /// <summary>
        /// 使用比例
        /// </summary>
        public decimal BL { get; set; }
    }

    /// <summary>
    /// 提交订单返回Model
    /// </summary>
    public class OrderAddReturnModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal TotalPrice { get; set; }
    }


     
    public class ERPAddOrderPramModel
    {
        public string mail { get; set; }
        public string itemsns { get; set; }
        public string prices { get; set; }
        public string nums { get; set; }
        public string receiver_name { get; set; }

        public string receiver_address { get; set; }
        public string receiver_state { get; set; }
        public string receiver_city { get; set; }
        public string receiver_district { get; set; }
        public string logistics_type { get; set; }
        public string outer_tid { get; set; }
        /// <summary>
        /// 卖家账号
        /// </summary>
        public string outer_shop_code { get; set; }
        public string receiver_mobile { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string outer_ddly { get; set; }
        public string buyer_message { get; set; }
        public string logistics_fee { get; set; }
        public string pay_moneys { get; set; }

        public Nullable<DateTime> pay_datatimes { get; set; }
        /// <summary>
        /// 交易单号
        /// </summary>
        public string ticket_no { get; set; }
    }

    /// <summary>
    /// erpViewModel
    /// </summary>
    public class ErpOrderViewModel
    {
        public int id { get; set; }
        public string mail { get; set; }
        public string itemsns { get; set; }
        public string prices { get; set; }
        public string nums { get; set; }
        public string receiver_name { get; set; }

        public string receiver_address { get; set; }
        public string receiver_state { get; set; }
        public string receiver_city { get; set; }
        public string receiver_district { get; set; }
        public string logistics_type { get; set; }
        public string outer_tid { get; set; }
        /// <summary>
        /// 卖家账号
        /// </summary>
        public string outer_shop_code { get; set; }
        public string receiver_mobile { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string outer_ddly { get; set; }
        public string buyer_message { get; set; }
        public string logistics_fee { get; set; }
        public string pay_moneys { get; set; }

        public Nullable<DateTime> pay_datatimes { get; set; }
        /// <summary>
        /// 交易单号
        /// </summary>
        public string ticket_no { get; set; }
        public DateTime ctime { get; set;}
        public string  err_message{get;set;}
        public bool isresend { get;set;}
        }
    
    public class ListViewModel<T>
    {
        public int total { get; set; }
        public T list { get; set; }
        public int error { get; set; }
        public string msg { set; get; }
    }

    /// <summary>
    /// Admin 订单列表
    /// </summary>
    public class AdminOrderListViewModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 应支付金额
        /// </summary>
        public decimal PayableAmount { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public byte Stutas { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public Nullable<DateTime> PayDate { get; set; }
        /// <summary>
        /// 微信支付码
        /// </summary>
        public string PayNum { get; set; }
        /// <summary>
        /// ERP订单号
        /// </summary>
        public string ErpOrderId { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNum { get; set; }
    }

    /// <summary>
    /// Admin 订单查询
    /// </summary>
    public class AdminOrderSearchViewModel
    {
        public int page { get; set; }
        public int size { get; set; }
        public string orderNum { get; set; }
        public string erpOrderId { get; set; }
        public string nickName { get; set; }
        public string payNum { get; set; }
        public string expressNum { get; set; }
        public int Stutas { get; set; }
        public string SPGG { get; set; }
    }

    /// <summary>
    /// Admin 订单详情
    /// </summary>
    public class AdminOrderShowDetailViewModel
    {
        public AdminOrderDetailViewModel detail { get; set; }
        public IEnumerable<AdminOrderProductListViewModel> product { get; set; }
    }

    public class AdminOrderDetailViewModel
    {
        public string OrderNum { get; set; }
        public string Payway { get; set; }
        public decimal Carriage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AcutalCarriage { get; set; }
        public decimal PayableAmount { get; set; }
        public string PayNum { get; set; }
        public Nullable<DateTime> Createdate { get; set; }
        public Nullable<DateTime> PayDate { get; set; }
        public string ExpressNum { get; set; }
        public byte Stutas { get; set; }
        public string ErpOrderId { get; set; }
        public string Message { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public byte Gender { get; set; }
        public string MemberMobile { get; set; }
        public string HeadImgUrl { get; set; }

        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }

        public Nullable<int> Consum { get; set; }
        public Nullable<int> Amount { get; set; }
        public string Original { get; set; }
    }

    public class AdminOrderProductListViewModel
    {
        public string SPDM { get; set; }
        public string SPMC { get; set; }
        public decimal RealSale { get; set; }
        public decimal BuySale { get; set; }
        public int BuyNum { get; set; }
        public string P1 { get; set; }
    }

}

