using ComCloudShop.Service;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.IO;

namespace ComCloudShop.Layer
{
    public class OrderService 
    {
        public bool UpdateWithd(int ID)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderReturns.FirstOrDefault(x => x.ID == ID);
                if (m != null)
                {
                    try
                    {
                        m.Statu = 1;
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 获取用户申请列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminReturnListViewModel>> GetMemberReturnListNew(int page, int size, string nickName, int State)
        {
            var result = new ResultViewModel<IEnumerable<AdminReturnListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.OrderReturns
                                     join p in db.Members on a.MemberID equals p.MemberId
                                     where (State == 3 ? 1 == 1 : a.Statu == State)
                                     orderby a.ID descending
                                     select new AdminReturnListViewModel
                                     {
                                         ID = a.ID,
                                         MemberID = (int)a.MemberID,
                                         Price = (decimal)a.Price,
                                         AddTime = a.AddTime.ToString(),
                                         State = (int)a.Statu,
                                         Reason = a.Reason,
                                         UserName = a.UserName,
                                         Bank=a.Bank,
                                         BankNumber=a.BankNumber,
                                         Phone = a.Phone,
                                         OrderNum=a.OrderNum
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.OrderReturns
                                    join p in db.Members on a.MemberID equals p.MemberId
                                    where (a.Statu == State)
                                    orderby a.ID descending
                                    select new AdminReturnListViewModel
                                    {
                                        ID = a.ID,
                                        MemberID = (int)a.MemberID,
                                        Price = (decimal)a.Price,
                                        AddTime = a.AddTime.ToString(),
                                        State = (int)a.Statu,
                                        Reason = a.Reason,
                                        UserName = p.NickName,
                                        Phone = p.Mobile,
                                        OrderNum = a.OrderNum
                                    }).Count();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "success";
                }
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }

        public List<OrderCommentViewModel> GetCommentList(int pid) {
            using (var db = new MircoShopEntities())
            {
                List<OrderCommentViewModel> list = new List<OrderCommentViewModel>();
                list = db.OrderComments.Where(x => x.ProductID == (int)pid).OrderByDescending(x=>x.AddTime).Select( x => new OrderCommentViewModel()
                {
                    ID = x.ID,
                    MemberID = (int)x.MemberID,
                    UserName = x.UserName,
                    OrderNum = x.OrderNum,
                    ProductID = (int)x.ProductID,
                    Contents = x.Contents,
                    Pics = x.Pics,
                    ProductScore = (int)x.ProductScore,
                    //ProductPackaging = (int)x.ProductPackaging,
                    //DeliverySpeed = (int)x.DeliverySpeed,
                    AddTime = (DateTime)x.AddTime,
                    Remack = x.Remack
                }).ToList();
                //list = db.OrderComments.Where(x => x.ProductID == (int)pid).OrderByDescending(x => x.AddTime)
                //    .Select(x => new OrderCommentViewModel()
                //    {
                //        ID = x.ID,
                //        MemberID = (int)x.MemberID,
                //        UserName = "",
                //        OrderNum = x.OrderNum,
                //        ProductID = (int)x.ProductID,
                //        Contents = x.Contents,
                //        Pics = x.Pics,
                //        ProductScore = (int)x.ProductScore,
                //        //ProductPackaging = (int)x.ProductPackaging,
                //        //DeliverySpeed = (int)x.DeliverySpeed,
                //        AddTime = (DateTime)x.AddTime,
                //        Remack = x.Remack
                //    }).ToList();
                try
                {


                    return list;  
                }
                catch
                {
                    return null;
                }
            }
        }
        
            public bool AddReturn(OrderReturn instance)
        {
            using (var db = new MircoShopEntities())
            {
                int productID = Convert.ToInt32(instance.ProductID);
                List<OrderProductDetail> list = db.OrderProductDetails.Where(d => d.ProductId == productID && d.OrderNum == instance.OrderNum).ToList();
                if (list.Count > 0)
                {
                    instance.Price = list[0].BuySale * list[0].BuyNum;
                    db.OrderReturns.Add(instance);
                    try
                    {
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public bool Add(OrderComment instance)
        {
            using (var db = new MircoShopEntities())
            {
                instance.AddTime = DateTime.Now;
                db.OrderComments.Add(instance);
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool Adds(List<OrderComment> instance)
        {
            using (var db = new MircoShopEntities())
            {
                db.OrderComments.AddRange(instance);
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Add(OrderDetailViewModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var d = new OrderDetail();
                d.OrderNum = SetOrderNo();
                d.Createdate = DateTime.Now;
                d.MemberId = instance.MemberId;
                d.BuyNums = string.Join(",", instance.CartProducts.Select(x => x.BuyNum));
                d.ProductArrs = string.Join(",", instance.CartProducts.Select(x => x.ProductId));
                d.AcutalCarriage = instance.AcutalCarriage;
                d.AddressId = instance.AddressId;
                d.Carriage = instance.Carriage;
                d.DiscountAmount = instance.DiscountAmount;
                d.LogisticsType = string.IsNullOrEmpty(instance.LogisticsType) ? "YUNDA" : instance.LogisticsType;
                d.PayableAmount = instance.PayableAmount;
                d.Original = "shop";
                d.Message = instance.Message;
                d.Receipt = instance.Receipt;
                d.ReceiptType = instance.ReceiptType;
                d.Stutas = 0;
                db.OrderDetails.Add(d);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }

                foreach (var q in instance.CartProducts)
                {
                    var o = new ProductOrder();
                    o.BuyNum = q.BuyNum;
                    o.ProductId = q.ProductId;
                    var product = db.Products.FirstOrDefault(x => x.ProductId == q.ProductId);
                    if (product != null && o.BuyNum > 0)
                    {
                        o.BuySale = product.Sale * product.Discount;
                        o.RealSale = product.Sale;
                    }
                    o.OrderId = d.OrderId;
                    db.ProductOrders.Add(o);
                }
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="instance">参数</param>
        /// <returns></returns>
        public string AddOrder(OrderDetailViewModel instance)
        {
            try
            {
                var orderNum = SetOrderNo();
                using (var db = new MircoShopEntities())
                {
                    var d = new OrderDetail();
                    d.OrderNum = orderNum;
                    d.Createdate = DateTime.Now;
                    d.MemberId = instance.MemberId;
                    //d.BuyNums = string.Join(",", instance.CartProducts.Select(x => x.BuyNum));
                    //d.ProductArrs = string.Join(",", instance.CartProducts.Select(x => x.ProductId));
                    d.AcutalCarriage = instance.AcutalCarriage;
                    d.AddressId = instance.AddressId;
                    d.Carriage = instance.Carriage;
                    d.DiscountAmount = instance.DiscountAmount;
                    d.LogisticsType = string.IsNullOrEmpty(instance.LogisticsType) ? "YUNDA" : instance.LogisticsType;
                    d.PayableAmount = instance.PayableAmount;
                    d.Original = "shop";
                    d.Message = instance.Message;
                    d.Receipt = instance.Receipt;
                    d.ReceiptType = instance.ReceiptType;
                    d.Stutas = 0;
                    d.CouponId = instance.CouponId;
                    db.OrderDetails.Add(d);

                    //更新优惠券使用情况
                    if (instance.CouponId > 0)
                    {
                        var coupon = db.UserCoupons.FirstOrDefault(x => x.CouponId == instance.CouponId);
                        coupon.IsUse = 1;
                        coupon.OrderNum = orderNum;
                    }

                    //添加订单商品信息
                    foreach (var q in instance.CartProducts)
                    {
                        var o = new OrderProductDetail();
                        o.BuyNum = q.BuyNum;
                        o.ProductId = q.ProductId;
                        var product = db.Products.FirstOrDefault(x => x.ProductId == q.ProductId);
                        if (product != null && o.BuyNum > 0)
                        {
                            o.BuySale = product.Sale * product.Discount;
                            o.RealSale = product.Sale;
                        }
                        o.OrderNum = d.OrderNum;
                        db.OrderProductDetails.Add(o);
                    }



                    db.SaveChanges();
                    return d.OrderNum;
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 添加订单事件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public OrderAddReturnModel AddOrder(OrderAddParmModel data)
        {
            var model = new OrderAddReturnModel();
            try
            {
                var orderNum =  SetOrderNo();

                using (var db = new MircoShopEntities())
                {
                    var d = new OrderDetail();
                    d.OrderNum = orderNum;
                    d.MemberId = data.MemberId;
                    d.Createdate = DateTime.Now;
                    d.AddressId = data.AddressId;
                    d.CouponId = data.CouponId;
                    d.LogisticsType = data.KuaiDi;
                    d.Original = "shop";
                    d.Stutas = 0;
                    d.Message = data.Remark;
                    d.Carriage = data.Carriage;
                    d.AcutalCarriage = data.Carriage;
                    d.Jifen = data.Jifen;
                    d.SPGG = data.SPGG;
                    var couponPrice = 0;

                    //更新优惠券使用情况
                    if (data.CouponId > 0)
                    {
                        var coupon = db.UserCoupons.FirstOrDefault(x => x.CouponId == data.CouponId);
                        coupon.IsUse = 1;
                        coupon.OrderNum = d.OrderNum;

                        //卡券减免金额
                        couponPrice = coupon.Amount;
                    }

                    var _cservice = new CartService();
                    var CartProduct = _cservice.GetCartForAddOrder(data.CartList);

                    if (CartProduct == null || CartProduct.Count() == 0)
                    {
                        model.OrderNum = "";
                        return model;
                    }

                    //添加订单商品信息
                    foreach (var q in CartProduct)
                    {
                        var o = new OrderProductDetail();
                        o.BuyNum = q.ProductNum;
                        o.ProductId = q.ProductId;
                        o.BuySale = q.Sale * q.Discount;
                        o.RealSale = q.Sale;
                        o.OrderNum = d.OrderNum;

                        //添加到订单商品详情
                        db.OrderProductDetails.Add(o);

                        //移除购物车
                        var m = db.Carts.FirstOrDefault(x => x.CartId == q.CartId);
                        db.Carts.Remove(m);
                    }

                    var allprice = CartProduct.Sum(x => x.ProductNum * x.Sale * x.Discount);

                    var rule = db.SaleRules.OrderByDescending(x => x.Amount).FirstOrDefault(x => x.Amount <= allprice);

                    //满减金额
                    d.DiscountAmount = rule == null ? 0 : rule.Discount;
                    //实付金额
                    d.PayableAmount = allprice - d.DiscountAmount - (Convert.ToDecimal(d.Jifen) * 1) - couponPrice + d.Carriage;

                    //添加订单
                    db.OrderDetails.Add(d);
                    //提交至数据库
                    db.SaveChanges();

                    model.OrderNum = orderNum;
                    model.TotalPrice = d.PayableAmount;
                }
            }
            catch
            {
                model.OrderNum = "";
            }
            return model;
        }


        public List<OrderProductDetail> GetOrderProductDetailList(string OrderNum)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderProductDetails.Where(x => x.OrderNum == OrderNum).ToList();
                return m;
            }
            return null;
        }

        public OrderDetail GetOrderDetail(string OrderNum)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderDetails.FirstOrDefault(x => x.OrderNum == OrderNum);
                return m;
            }
            return null;
        }

        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="OrderNum"></param>
        /// <param name="PayNum"></param>
        /// <returns></returns>
        public bool UpdateOrderReturnOk(string OrderNum)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderDetails.FirstOrDefault(x => x.OrderNum == OrderNum);
                if (m == null)
                {
                    return false;
                }
                m.Stutas = 4;
                try
                {
                    var member = db.Members.Where(d => d.MemberId == m.MemberId).FirstOrDefault();
                    if (Convert.ToDecimal(m.Jifen) > 0)
                    {
                        member.integral = member.integral + Convert.ToDecimal(m.Jifen);

                    }
                    if (db.SaveChanges() > 0)
                    {
                        
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="OrderNum"></param>
        /// <param name="PayNum"></param>
        /// <returns></returns>
        public bool UpdateOrderReturn(string OrderNum)
        {
            using (var db = new MircoShopEntities())
            {

                var list= db.OrderReturns.Where(d => d.OrderNum == OrderNum).ToList();
                if (list.Count <= 0)
                {
                    var m = db.OrderDetails.FirstOrDefault(x => x.OrderNum == OrderNum);
                    if (m == null)
                    {
                        return false;
                    }
                    m.Stutas = 3;
                    try
                    {
                        if (db.SaveChanges() > 0)
                        {

                        }
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
        }



        public bool AddGif(ComCloudShop.Service.GiftList model)
        {
            using (var db = new MircoShopEntities())
            {
                db.GiftLists.Add(model);
                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 支付完成更新订单信息
        /// </summary>
        /// <param name="OrderNum"></param>
        /// <param name="PayNum"></param>
        /// <returns></returns>
        public bool UpdateOrderOrPayOK(string OrderNum, string PayNum)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderDetails.FirstOrDefault(x => x.OrderNum == OrderNum);
                if (m == null)
                {
                    return false;
                }
                m.PayNum = PayNum;
                m.Payway = "微信支付";
                m.PayDate = DateTime.Now;
                m.Stutas = 1;
                try
                {
                    if (db.SaveChanges() > 0) {
                       
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更新本地ERP订单号
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="ErpOrderId">ERP订单号</param>
        /// <returns></returns>
        public bool UpdateOrderSetERP(string OrderNum, string ErpOrderId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderDetails.FirstOrDefault(x => x.OrderNum == OrderNum);
                if (m == null)
                {
                    return false;
                }
                m.ErpOrderId = ErpOrderId;
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取Erp所需参数
        /// </summary>
        /// <param name="OrderNum"></param>
        /// <param name="PayNum"></param>
        /// <returns></returns>
        public ERPAddOrderPramModel GetERPOrderData(string OrderNum, string PayNum)
        { 
            var model = new ERPAddOrderPramModel();

            try
            {
                using (var db = new MircoShopEntities())
                {
                    model = (from a in db.OrderDetails
                             join
                                 b in db.Members on a.MemberId equals b.MemberId
                             join
                                 c in db.DeliveryAddresses on a.AddressId equals c.AddressId
                             where a.OrderNum == OrderNum
                             select new ERPAddOrderPramModel
                             {
                                 mail = b.OpenId,
                                 receiver_name = c.UserName,
                                 receiver_address = c.Address,
                                 receiver_state = c.Province,
                                 receiver_city = c.City,
                                 receiver_district = c.District,
                                 logistics_type = a.LogisticsType,
                                 outer_tid = a.OrderNum,
                                 receiver_mobile = c.Mobile,
                                 outer_ddly = a.Original,
                                 buyer_message = a.Message,
                                 logistics_fee = a.AcutalCarriage.ToString(),
                                 pay_moneys = a.PayableAmount.ToString(),
                                 pay_datatimes = a.PayDate,
                                 ticket_no = PayNum
                             }).FirstOrDefault();

                    //var list = db.OrderProductDetails.Where(x => x.OrderNum == OrderNum);
                    var list = from a in db.OrderProductDetails
                               join
                                   b in db.Products on a.ProductId equals b.ProductId
                               where a.OrderNum == OrderNum
                               select new { a.ProductId, a.BuyNum, a.BuySale, b.SPDM };
                    model.itemsns = string.Join(",", list.Select(x => x.SPDM));
                    model.prices = string.Join(",", list.Select(x => x.BuySale));
                    model.nums = string.Join(",", list.Select(x => x.BuyNum));
                    model.outer_shop_code = "方广官方微商城";
                    model.outer_ddly = "2";
                    model.logistics_type = "yunda";
                    model.pay_datatimes = DateTime.Now;
                }
            }
            catch
            {

            }

            return model;
        }

        public bool Update(int Id, string ErpOrderId, string PayNum)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderDetails.FirstOrDefault(x => x.OrderId == Id);
                if (m == null)
                {
                    return false;
                }
                m.ErpOrderId = ErpOrderId;
                m.PayNum = PayNum;
                m.Payway = "微信支付";
                m.PayDate = DateTime.Now;
                m.Stutas = 1;
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Update(int Id, string ExpressNum)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.OrderDetails.FirstOrDefault(x => x.OrderId == Id);
                if (m == null)
                {
                    return false;
                }
                m.ExpressNum = ExpressNum;
                m.ModifyDate = DateTime.Now;
                m.Stutas = 2;
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Delete(int Id)
        {
            using (var db = new MircoShopEntities())
            {
                var d = db.OrderDetails.FirstOrDefault(x => x.OrderId == Id);
                if (d == null)
                {
                    return false;
                }
                d.IsDelete = true;
                d.IsCancle = true;
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Delete(string No)
        {
            using (var db = new MircoShopEntities())
            {
                var d = db.OrderDetails.FirstOrDefault(x => x.OrderNum == No);
                if (d == null)
                {
                    return false;
                }
                d.IsDelete = true;
                d.IsCancle = true;


                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 用户取消订单
        /// </summary>
        /// <param name="no">订单号</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public bool Delete(string no, int uid)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var m = db.OrderDetails.FirstOrDefault(x => x.OrderNum == no && x.MemberId == uid);
                    if (m == null)
                    {
                        return false;
                    }
                    m.IsDelete = true;
                    m.IsCancle = true;

                    if (m.CouponId > 0)
                    {
                        var coupon = db.UserCoupons.FirstOrDefault(x => x.CouponId == m.CouponId);
                        coupon.IsUse = 0;
                    }

                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public int GetOrderCount()
        {
            using (var db = new MircoShopEntities())
            {
                return db.OrderDetails.Where(x => x.IsCancle == false && x.IsDelete == false).Count();
            }
        }

        public int GetOrderCount(int uid, int status)
        {
            using (var db = new MircoShopEntities())
            {
                return db.OrderDetails.Where(x => x.MemberId == uid && x.Stutas == status).Count();
            }
        }

        public double GetOrderPageCount(int size = 10)
        {
            var total = GetOrderCount();
            return Math.Ceiling((double)total / size);
        }

        public IEnumerable<OrderDetailDisplayModel> GetOrderDetailList(int uid, int page = 1, int size = 20)
        {
            using (var db = new MircoShopEntities())
            {
                var strSql = new StringBuilder();
                var pageindex = size * (page - 1);
                strSql.AppendFormat(" select top {0} case when a.Stutas=1 THEN '待支付' WHEN a.Stutas = 2 THEN '配送中' WHEN a.Stutas=3 THEN '已完成' ELSE '' END AS Status, a.Createdate, a.PayNum,a.Message,a.ErpOrderId,a.PayableAmount,a.Stutas, m.OpenId,  a.OrderId,a.OrderNum,a.ProductArrs,a.BuyNums,m.NickName as MemberName,d.Address,d.Mobile,d.UserName from OrderDetail as a  ", size);
                strSql.Append(" JOIN Member as m ON a.MemberId = m.MemberId ");
                strSql.Append(" Left JOIN DeliveryAddress as d ON a.AddressId = d.AddressId ");
                strSql.Append(" where 1 = 1 And a.IsDelete=0 And a.IsCancle =0 ");
                if (uid > 0)
                {

                    strSql.AppendFormat(" and a.MemberId={0} ", uid);
                    strSql.AppendFormat("  AND a.OrderId NOT IN (SELECT TOP {0} b.OrderId FROM OrderDetail as b WHERE b.MemberId={1} And b.IsDelete=0 And b.IsCancle =0  ORDER BY b.OrderId)  ORDER BY a.OrderId; ", pageindex, uid);
                }
                else
                {
                    strSql.AppendFormat(" AND a.OrderId NOT IN (SELECT TOP {0} b.OrderId FROM OrderDetail as b WHERE b.IsDelete=0 And b.IsCancle =0  ORDER BY b.OrderId)  ORDER BY a.OrderId; ", pageindex);
                }
                return db.Database.SqlQuery<OrderDetailDisplayModel>(strSql.ToString()).ToList();
            }
        }


        public IEnumerable<OrderDetailResultModel> GetOrderDetailByNo(string OrderNo)
        {
            using (var db = new MircoShopEntities())
            {
                var strSql = new StringBuilder();
                strSql.Append(" SELECT  o.ProductArrs, o.Original,m.Discount,img.P1,img.P2,img.P3, p.RealSale, p.BuySale, u.OpenId,o.ErpOrderId, o.Message,o.Receipt,o.ReceiptType, o.Stutas,o.OrderId,o.OrderNum,o.AddressId,a.Address,a.Province,a.City,a.UserName,a.District ,o.Createdate,o.Carriage,o.AcutalCarriage,o.DiscountAmount,o.PayableAmount,p.ProductId,p.BuyNum,m.SPDM,m.Sale,m.SPMC,m.ProductGuid,u.NickName as MemberName,a.Mobile,m.Title FROM OrderDetail as o ");
                strSql.Append(" JOIN OrderProductDetail as p ON o.OrderNum = p.OrderNum ");
                strSql.Append(" Left JOIN DeliveryAddress as a ON a.AddressId = o.AddressId ");
                strSql.Append(" JOIN Product as m ON m.ProductId = p.ProductId ");
                strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId =m.ProductId ");
                strSql.Append(" JOIN Member as u ON u.MemberId = o.MemberId ");
                strSql.Append(" WHERE o.OrderNum=@no ");
                return db.Database.SqlQuery<OrderDetailResultModel>(strSql.ToString(), new SqlParameter("@no", OrderNo)).ToList();
            }
        }

        public IEnumerable<OrderResultModel> GetOrderPageWithUser(int uid, int stutas, int page = 1, int size = 20)
        {
            using (var db = new MircoShopEntities())
            {
                var query = GetOrderWithUser(uid, stutas, page, size);
                var list = query.Select(x => x.OrderId).Distinct();
                var result = new List<OrderResultModel>();
                if (query.Count() > 0)
                {
                    foreach (var l in list)
                    {
                        var list2 = query.Where(x => x.OrderId == l);
                        var s = new OrderDetailResultModel();
                        var r = new OrderResultModel();
                        s = list2.FirstOrDefault();
                        r.OrderId = s.OrderId;
                        r.OrderNum = s.OrderNum;
                        r.Stutas = s.Stutas;
                        r.Address = s.Address;
                        r.AddressId = s.AddressId;
                        r.Carriage = s.Carriage;
                        r.Createdate = s.Createdate;
                        r.AcutalCarriage = s.AcutalCarriage;
                        r.DiscountAmount = s.DiscountAmount;
                        r.PayableAmount = s.PayableAmount;
                        r.MemberName = s.MemberName;
                        r.Mobile = s.Mobile;
                        r.UserName = s.UserName;
                        r.Receipt = s.Receipt;
                        r.ReceiptType = s.ReceiptType;
                        r.Province = s.Province;
                        r.City = s.City;
                        r.District = s.District;

                        var pList = new List<ProductResultModel>();
                        foreach (var q in list2)
                        {
                            var p = new ProductResultModel();
                            p.BuyNum = q.BuyNum;
                            p.ProductId = q.ProductId;
                            p.Sale = q.Sale;
                            p.SPMC = q.SPMC;
                            p.Title = q.Title;
                            p.SPDM = q.SPDM;
                            p.BuySale = q.BuySale;
                            p.P1 = q.P1;
                            p.P2 = q.P2;
                            p.P3 = q.P3;
                            pList.Add(p);
                        }
                        r.Product = pList;
                        result.Add(r);
                    }
                }
                return result;
            }
        }

        private IEnumerable<OrderDetailResultModel> GetOrderWithUser(int uid, int stutas, int page, int size)
        {
            using (var db = new MircoShopEntities())
            {
                var strSql = new StringBuilder();
                var pageindex = size * (page - 1);
                strSql.Append(" SELECT x.*,img.P1,img.P2,img.P3,p.ProductId,p.BuyNum, p.BuySale,p.RealSale,m.Sale,m.SPMC,m.Title,m.SPDM,m.ProductGuid  from ( ");
                strSql.AppendFormat(" select top {0} m.OpenId, a.Message,a.Receipt,a.ReceiptType, a.OrderId,a.OrderNum,a.Stutas,a.AddressId,a.Createdate,a.Carriage,a.AcutalCarriage,a.DiscountAmount,a.PayableAmount, m.NickName as MemberName,d.Address,d.Mobile,d.Province,d.City,d.UserName,d.District  from OrderDetail as a  ", size);
                strSql.Append(" JOIN Member as m ON a.MemberId = m.MemberId ");
                strSql.Append(" Left JOIN DeliveryAddress as d ON a.AddressId = d.AddressId ");
                strSql.Append(" where 1 = 1 And a.IsDelete=0 And a.IsCancle =0 ");
                strSql.AppendFormat(" and a.MemberId={0} ", uid);
                if (stutas > 0 && stutas < 10)
                {
                    strSql.AppendFormat(" and a.Stutas={0} ", stutas);
                    strSql.AppendFormat("  AND a.OrderId NOT IN (SELECT TOP {0} b.OrderId FROM OrderDetail as b WHERE b.MemberId={1} AND a.IsCancle = 0 AND a.IsDelete = 0 and a.Stutas={2}  ORDER BY a.Stutas, b.OrderId DESC)   ORDER BY a.Stutas, a.OrderId DESC ", pageindex, uid, stutas);
                }
                else
                {
                    strSql.AppendFormat("  and a.Stutas>0 AND a.OrderId NOT IN (SELECT TOP {0} b.OrderId FROM OrderDetail as b WHERE b.MemberId={1} AND a.IsCancle = 0 AND a.IsDelete = 0 and a.Stutas>0 ORDER BY a.Stutas, b.OrderId DESC)   ORDER BY a.Stutas, a.OrderId DESC ", pageindex, uid);
                }
                strSql.Append(" ) as x ");
                strSql.Append(" JOIN OrderProductDetails as p ON x.OrderNum = p.OrderNum ");
                strSql.Append(" JOIN Product as m ON m.ProductId = p.ProductId ");
                strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = p.ProductId");
                strSql.Append(" ORDER BY x.Stutas,x.OrderId DESC; ");

                return db.Database.SqlQuery<OrderDetailResultModel>(strSql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="status">订单状态</param>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <returns></returns>
        public IEnumerable<OrderListViewModel> GetOrderListPage(int uid, int status, int page = 1, int size = 20)
        {
            var list = new List<OrderListViewModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    
                    var  data = new List<OrderDetail>();
                    if (status >= 0)
                    {
                        data = db.OrderDetails.Where(x => x.Stutas == status&&x.MemberId==uid&& x.IsCancle==false&&x.IsDelete==false).OrderByDescending(x=>x.OrderId).Skip((page - 1) * size).Take(size).ToList();
                    }
                    else
                    {
                        data = db.OrderDetails.Where(x => x.MemberId == uid && x.IsCancle == false && x.IsDelete == false).OrderByDescending(x => x.OrderId).Skip((page - 1) * size).Take(size).ToList();
                    }

                    OrderListViewModel model;
                    foreach (var d in data)
                    {
                        model = new OrderListViewModel();
                        model.OrderId = d.OrderId;
                        model.OrderNum = d.OrderNum;
                        model.Status = d.Stutas;
                        model.TotalPrice = d.PayableAmount;

                        model.product = new List<OrderProductViewModel>();
                        model.product = GetOrderProductDetail(d.OrderNum);

                        list.Add(model);
                    }

                }
            }
            catch
            {
                
            }
            return list;
        }

        /// <summary>
        /// 获取订单详情数据
        /// </summary>
        /// <param name="no">订单号</param>
        /// <returns></returns>
        public OrderDetailNewViewModel GetOrderDetailByNO(string no)
        {
            var model = new OrderDetailNewViewModel();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = (from a in db.OrderDetails
                                join
                                   c in db.UserCoupons on a.CouponId equals c.CouponId into temp
                                from tt in temp.DefaultIfEmpty()
                                join
                                    d in db.DeliveryAddresses on a.AddressId equals d.AddressId
                                where a.OrderNum == no
                                select new
                                {
                                    a.OrderId,
                                    a.OrderNum,
                                    a.Stutas,
                                    CreatTime = a.Createdate,
                                    TotalPrice = a.PayableAmount,
                                    a.Carriage,
                                    a.DiscountAmount,
                                    Amount = tt.Amount == null ? 0 : tt.Amount,
                                    Addressee = d.UserName,
                                    KD = a.LogisticsType,
                                    Jifen = a.Jifen,
                                    MemberID = a.MemberId,
                                   Address = d.Province + d.City + d.District + d.Address
                               }).ToList().FirstOrDefault();

                    if (data != null)
                    {
                        model.Address = data.Address;
                        model.Addressee = data.Addressee;
                        model.CreatTime = ((DateTime)data.CreatTime).ToString("yyyy-MM-dd HH:mm:ss");
                        model.KD = data.KD;
                        model.KDPay = data.Carriage;
                        model.OrderId = data.OrderId;
                        model.OrderNum = data.OrderNum;
                        model.Status = data.Stutas;
                        model.Jifen = data.Jifen;
                        model.list = new List<OrderProductViewModel>();
                        model.list = GetOrderProductDetail(no);
                        UserService user = new UserService();
                        
                        model.MemberModel = user.GetUserIndexInfo(data.MemberID);
                        WriteTokenToTxt(model.MemberModel.openid);
                        model.opendid = model.MemberModel.openid;
                        model.TotalPrice = model.list.Sum(x => x.RealSale * x.Number);
                        model.DisCount = model.TotalPrice + data.Carriage - data.TotalPrice;
                        model.PayPrice = data.TotalPrice;
                        model.Number = model.list.Sum(x => x.Number);
                    }
                }
            }
            catch
            {

            }
            return model;
        }
        private void WriteTokenToTxt(string token)
        {
            try
            {
                FileStream fs = new FileStream(@"D://1.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write("");
                sw.Write(token + "\r\n");
                sw.Write(DateTime.Now.ToString());
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }
        /// <summary>
        /// 获取订单产品详情
        /// </summary>
        /// <param name="orderNum">订单号码</param>
        /// <returns></returns>
        public IEnumerable<OrderProductViewModel> GetOrderProductDetail(string orderNum)
        {
            var list = new List<OrderProductViewModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    //var data = db.OrderProductDetails.Where(x => x.OrderNum == orderNum).ToList();
                    var data = from x in db.OrderProductDetails join
                                                                y in db.Products on x.ProductId equals y.ProductId join
                                                                z in db.ProductImgs on y.ProductId equals z.ProductId
                                                                where x.OrderNum ==orderNum
                                                                select new {x.ProductId,x.OrderNum,x.BuyNum,x.BuySale,x.RealSale,y.SPMC,y.Describle,z.P1};

                    OrderProductViewModel model; 
                    foreach (var d in data)
                    {
                        model = new OrderProductViewModel();
                        model.ProductId = d.ProductId;
                        model.SPMC = d.SPMC;
                        model.Number = d.BuyNum;
                        model.BuySale = d.BuySale;
                        model.RealSale = d.RealSale;
                        model.Describle = d.Describle;
                        model.Pic = d.P1;

                        var list1= db.OrderReturns.Where(x => x.OrderNum == orderNum && x.ProductID == model.ProductId).ToList();
                        if (list1.Count > 0)
                        {
                            model.Status = 1;
                        }
                        else {
                            model.Status = 0;
                        }
                        list.Add(model);
                    }

                }
            }
            catch
            {

            }
            return list;
        }

        /// <summary>
        /// 获取订单支付金额
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public decimal GetOrderPayPrice(string no)
        {
            try
            {
                using(var db = new MircoShopEntities())
                {
                    var price = db.OrderDetails.FirstOrDefault(x => x.OrderNum == no).PayableAmount;
                    return price;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public string GetOrderPrepayid(string no)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var price = db.OrderDetails.FirstOrDefault(x => x.OrderNum == no).PayNum;
                    return price;
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 更新预微信支付单号
        /// </summary>
        /// <param name="no">订单号</param>
        /// <param name="prepayid">微信支付单号</param>
        /// <returns></returns>
        public bool UpdateOrderPrepayid(string no,string prepayid )
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.OrderDetails.FirstOrDefault(x => x.OrderNum == no);
                    model.PayNum = prepayid;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取提交页数据
        /// </summary>
        /// <param name="list">cartid集合</param>
        /// <param name="userid">用户id</param>
        /// <param name="openid">openid</param>
        /// <param name="aid">地址id</param>
        /// <param name="cid">优惠券id</param>
        /// <returns></returns>
        public OrderConfigViewModel GetOrderConfig(int[] list, int userid, string openid, int aid, int cid)
        {
            var model = new OrderConfigViewModel();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    //购物车集合
                    model.list = new List<CartListForOrderViewModel>();
                    var _cartService = new CartService();
                    model.list =_cartService.getPageList(list);

                    var allprice = model.list.Sum(x => x.Sale * x.Discount * x.ProductNum);

                    //满减
                    model.rule = new RuleViewModel();
                    var _ruleService = new RuleService();
                    model.rule = _ruleService.ReductionPrice(allprice);

                    //优惠券
                    model.coupon = new CouponDetailViewModel();
                    var _couponservice = new CouponService();
                    if (cid > 0)
                    {
                        model.coupon = _couponservice.GetCouponById(cid);
                    }
                    else
                    {
                        model.coupon = _couponservice.GetBestCoupon(openid, allprice);
                    }

                    //地址
                    model.address = new AddressDetailViewModel();
                    var _addressservice = new DeliveryAddressService();
                    if (aid > 0)
                    {
                        model.address = _addressservice.GetAddressById(aid);
                    }
                    else
                    {
                        model.address = _addressservice.GetDefaultAddress(userid);
                    }

                    return model;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 返回订单商品总价
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public decimal GetOrderTotalPrice(int[] list)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    //购物车集合
                    var _cartService = new CartService();
                    var data = _cartService.getPageList(list);
                    var allprice = data.Sum(x => x.Sale * x.Discount * x.ProductNum);
                    return allprice;
                }
            }
            catch
            {
                return 0M;
            }
        }

        /// <summary>
        /// 节省的金额
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public decimal GetSaveMoney(int memberId)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = (from a in db.OrderDetails
                               join
                                   b in
                                   (from x in db.OrderProductDetails
                                    group x by x.OrderNum into y
                                    select new
                                    {
                                        OrderNum = y.Key,
                                        price = y.Sum(x => (x.RealSale - x.BuySale) * x.BuyNum)
                                    }) on a.OrderNum equals b.OrderNum
                               join
                                   c in db.Members on a.MemberId equals c.MemberId
                               join
                                   d in db.UserCoupons on a.CouponId equals d.CouponId  into temp
                               from e in temp.DefaultIfEmpty()
                               where a.MemberId == memberId && a.IsCancle==false && a.IsDelete==false
                               select new
                               {
                                   pSave = b.price,
                                   cSave = e.Amount==null?0:e.Amount
                               }).ToList();
                    if (data.Count() > 0)
                    {
                       return data.Sum(x => x.pSave) + data.Sum(x => x.cSave);
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取用户订单数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="status">状态：0：未支付；1：配送中；2：完成；</param>
        /// <returns></returns>
        public int GetOrderToPayNumber(int uid, int status)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = db.OrderDetails.Where(x => x.MemberId == uid && x.Stutas == status&& x.IsCancle==false&&x.IsDelete==false).Count();
                    return data;
                }
            }
            catch
            {
                return 0;
            }
        }

        #region function
        private string SetOrderNo()
        {
            lock (this)
            {
                using (var db = new MircoShopEntities())
                {
                    TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    int key = 10;
                    var l = Convert.ToInt64(ts.TotalSeconds).ToString() + GenForInt(1, 999, ref key);
                    l += "00";
                    var query = db.OrderDetails.SingleOrDefault(x => x.OrderNum == l);
                    if (query != null)
                    {
                        var i = int.Parse(query.OrderNum) + 1;
                        return i.ToString();
                    }
                    return l;
                }
            }
        }

        private string GenForInt(int minvalue, int maxvalue, ref int myran)
        {
            Random ran = new Random();
            int RandKey;
            do
            {
                RandKey = ran.Next(minvalue, maxvalue);
            } while (RandKey == myran);
            myran = RandKey;
            return RandKey.ToString();
        }
        #endregion
        #region Erp订单
        /// <summary>
        /// erp订单列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ListViewModel<IEnumerable<ErpOrderViewModel>> GetErpOrderList(int page, int pageSize,string sqlSearch)
        {
            var list = new ListViewModel<IEnumerable<ErpOrderViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = (from a in db.ErpOrders
                                 where 1 == 1 && (a.receiver_name.Contains(sqlSearch) || a.receiver_mobile.Contains(sqlSearch) || a.outer_tid.Contains(sqlSearch))
                                 orderby a.ctime descending
                                 select new ErpOrderViewModel
                                 {
                                     id = a.id,
                                     mail = a.mail,
                                     itemsns = a.itemsns,
                                     prices = a.prices,
                                     nums = a.nums,
                                     receiver_mobile = a.receiver_mobile,
                                     receiver_address = a.receiver_address,
                                     receiver_city = a.receiver_city,
                                     receiver_district = a.receiver_district,
                                     receiver_name = a.receiver_name,
                                     receiver_state = a.receiver_state,
                                     logistics_type = a.logistics_type,
                                     logistics_fee = a.logistics_fee,
                                     outer_tid = a.outer_tid,
                                     outer_shop_code = a.outer_shop_code,
                                     outer_ddly = a.outer_ddly,
                                     buyer_message = a.buyer_message,
                                     pay_datatimes = a.pay_datatimes,
                                     pay_moneys = a.pay_moneys,
                                     ctime = a.ctime,
                                     err_message = a.err_message,
                                     ticket_no = a.ticket_no,
                                     isresend=a.isresend,
                                 }).ToList();
                    list.list = model.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                    list.total = 0;
                    if(model.Count>0)
                    list.total = model.Count;
                    list.error = 0;
                    list.msg = "success";
                }
            }
            catch(Exception ex)
            {
                list.error = 1;
                list.msg = ex.Message;
            }
            return list;
        }

        /// <summary>
        /// 获取erp订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ListViewModel<ErpOrderViewModel> GetErpOrder(int id)
        {
            var result = new ListViewModel<ErpOrderViewModel>();
            using (var db = new MircoShopEntities()) {
                var model = (from a in db.ErpOrders
                             where 1 == 1 && a.id==id
                             orderby a.ctime descending
                             select new ErpOrderViewModel
                             {
                                 id = a.id,
                                 mail = a.mail,
                                 itemsns = a.itemsns,
                                 prices = a.prices,
                                 nums = a.nums,
                                 receiver_mobile = a.receiver_mobile,
                                 receiver_address = a.receiver_address,
                                 receiver_city = a.receiver_city,
                                 receiver_district = a.receiver_district,
                                 receiver_name = a.receiver_name,
                                 receiver_state = a.receiver_state,
                                 logistics_type = a.logistics_type,
                                 logistics_fee = a.logistics_fee,
                                 outer_tid = a.outer_tid,
                                 outer_shop_code = a.outer_shop_code,
                                 outer_ddly = a.outer_ddly,
                                 buyer_message = a.buyer_message,
                                 pay_datatimes = a.pay_datatimes,
                                 pay_moneys = a.pay_moneys,
                                 ctime = a.ctime,
                                 err_message = a.err_message,
                                 ticket_no = a.ticket_no,
                                 isresend=a.isresend,
                             }).FirstOrDefault();
                if (model != null)
                {
                    result.error = 0;
                    result.list = model;
                }
                else
                    result.error = 1;
                return result;
            }
        }

        /// <summary>
        /// 发送成功，更新erp表字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateErpOrder(ErpOrderViewModel model) 
        {
            //var result = false;
            try 
            {
                using (var db = new MircoShopEntities())
                {
                    var order =db.ErpOrders.FirstOrDefault(x=>x.id==model.id);
                    order.itemsns = model.itemsns;
                    order.buyer_message = model.buyer_message;
                    order.ctime = model.ctime;
                    order.err_message = model.err_message;
                    order.logistics_fee = model.logistics_fee;
                    order.logistics_type = model.logistics_type;
                    order.mail = model.mail;
                    order.nums = model.nums;
                    order.outer_ddly = model.outer_ddly;
                    order.outer_shop_code = model.outer_shop_code;
                    order.outer_tid = model.outer_tid;
                    order.pay_datatimes = model.pay_datatimes;
                    order.pay_moneys = model.pay_moneys;
                    order.prices = model.prices;
                    order.receiver_address = model.receiver_address;
                    order.receiver_city = model.receiver_city;
                    order.receiver_district = model.receiver_district;
                    order.receiver_mobile = model.receiver_mobile;
                    order.receiver_name = model.receiver_name;
                    order.receiver_state = model.receiver_state;
                    order.ticket_no = model.ticket_no;
                    order.isresend = true;
                    db.SaveChanges();
                    //result = true;
                }
            }
            catch { }
            //return result;
        }
       
        #endregion


        #region AdminOrder

        /// <summary>
        /// Admin 获取订单列表
        /// </summary>
        /// <param name="data">查询参数</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminOrderListViewModel>> AdminGetOrderList(AdminOrderSearchViewModel data)
        {
            var result = new ResultViewModel<IEnumerable<AdminOrderListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.OrderDetails
                                     join
                                         b in db.Members on a.MemberId equals b.MemberId
                                     where (string.IsNullOrEmpty(data.orderNum) ? 1 == 1 : a.OrderNum == data.orderNum) &&
                                               (string.IsNullOrEmpty(data.nickName) ? 1 == 1 : b.NickName.Contains(data.nickName)) && (string.IsNullOrEmpty(data.erpOrderId) ? 1 == 1 : a.ErpOrderId == data.erpOrderId) && (string.IsNullOrEmpty(data.payNum) ? 1 == 1 : a.PayNum == data.payNum) && (string.IsNullOrEmpty(data.expressNum) ? 1 == 1 : a.ExpressNum == data.expressNum) && (data.Stutas==10 ? 1 == 1 : a.Stutas == data.Stutas) && a.SPGG==data.SPGG
                                     orderby a.PayDate descending, a.Createdate descending
                                     select new AdminOrderListViewModel
                                     {
                                         OrderId = a.OrderId,
                                         OrderNum = a.OrderNum,
                                         MemberId = a.MemberId,
                                         NickName = b.NickName,
                                         ErpOrderId = a.ErpOrderId,
                                         Stutas = a.Stutas,
                                         PayDate = a.PayDate,
                                         PayNum = a.PayNum,
                                         PayableAmount = a.PayableAmount,
                                         ExpressNum =a.ExpressNum
                                     }).Skip((data.page - 1) * data.size).Take(data.size).ToList();

                    result.total = (from a in db.OrderDetails
                                    join
                                        b in db.Members on a.MemberId equals b.MemberId
                                    where (string.IsNullOrEmpty(data.orderNum) ? 1 == 1 : a.OrderNum == data.orderNum) &&
                                              (string.IsNullOrEmpty(data.nickName) ? 1 == 1 : b.NickName.Contains(data.nickName)) && (string.IsNullOrEmpty(data.erpOrderId) ? 1 == 1 : a.ErpOrderId == data.erpOrderId) && (string.IsNullOrEmpty(data.payNum) ? 1 == 1 : a.PayNum == data.payNum) && (string.IsNullOrEmpty(data.expressNum) ? 1 == 1 : a.ExpressNum == data.expressNum) && (data.Stutas == 10 ? 1 == 1 : a.Stutas == data.Stutas) && a.SPGG == data.SPGG
                                    orderby a.PayDate descending, a.Createdate descending
                                    select new AdminOrderListViewModel
                                    {
                                        OrderId = a.OrderId
                                    }).Count();
                }

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Admin 获取订单详情
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <returns></returns>
        public ResultViewModel<AdminOrderShowDetailViewModel> AdminGetOrderDetail(string OrderNum)
        {
            var result = new ResultViewModel<AdminOrderShowDetailViewModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var sqlOrder = string.Format(
                                            @"SELECT a.OrderNum,a.Payway,a.Carriage,a.DiscountAmount,
                                                            a.AcutalCarriage,a.PayableAmount,a.PayNum,a.Createdate,
                                                            a.PayDate,a.ExpressNum,a.Stutas,a.ErpOrderId,a.Message,
                                                            b.OpenId,b.NickName,b.Gender,b.Mobile AS MemberMobile,b.HeadImgUrl,
                                                            c.UserName,c.Mobile,c.Province,c.City,c.District,c.Address,
			                                                d.Consum,d.Amount,d.Original 
                                                 FROM	dbo.OrderDetail AS a LEFT JOIN 
                                                            dbo.Member  AS b ON a.MemberId = b.MemberId LEFT JOIN
			                                                dbo.DeliveryAddress AS c ON a.AddressId = c.AddressId LEFT JOIN
			                                                dbo.UserCoupon AS d ON a.CouponId =d.CouponId          
	                                            WHERE	a.OrderNum = '{0}'", OrderNum);

                    var sqlOrderDetail = string.Format(
                                                    @"SELECT	    b.SPDM,b.SPMC,a.RealSale,a.BuySale,a.BuyNum,'http://admin.hofaf.com/'+p.P1 as P1
	                                                      FROM    dbo.OrderProductDetail AS a LEFT JOIN
			                                                            dbo.Product AS b ON a.ProductId = b.ProductId LEFT JOIN 
 ProductImg as p on p.ProductId=b.ProductId WHERE	a.OrderNum = '{0}'", OrderNum);
                    result.result = new AdminOrderShowDetailViewModel();
                    //result.result.detail = new AdminOrderDetailViewModel();
                    result.result.detail = db.Database.SqlQuery<AdminOrderDetailViewModel>(sqlOrder.ToString()).FirstOrDefault();
                    //result.result.product = new List< AdminOrderProductListViewModel>();
                    result.result.product = db.Database.SqlQuery<AdminOrderProductListViewModel>(sqlOrderDetail.ToString()).ToList();
                }

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 设置订单快递信息
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="ExpressNum">快递单号</param>
        /// <param name="IsReceive">0：配送中；1：已签收；</param>
        /// <returns></returns>
        public ResultViewModel<bool> AdminSetOrderExpressInfo(string OrderNum, string ExpressNum, int IsReceive)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.OrderDetails.FirstOrDefault(x => x.OrderNum == OrderNum);
                    if (model == null)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "no data";

                        return result;
                    }
                    //该订单未支付
                    if (model.Stutas == 0)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "该订单未支付，不能设置快递信息";

                        return result;
                    }
                    model.ExpressNum = ExpressNum;
                    if (IsReceive == 1)
                    {

                        

                        //设置为已完成
                        model.Stutas = 2;
                    }

                    db.SaveChanges();
                }

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }

            return result;
        }
        #endregion
    }
}
