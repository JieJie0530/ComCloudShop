using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;

namespace ComCloudShop.Layer
{
    public class CartService : BaseService
    {
        public bool Add(CartViewModel instance)
        {
            if (instance.CartProducts.Count == 0)
            {
                return false;
            }
            foreach (var q in instance.CartProducts)
            {
                var o = db.Carts.FirstOrDefault(x => x.ProductId == q.ProductId && x.MemberId == instance.MemberId);
                if (o == null)
                {

                    db.Carts.Add(new Cart
                    {
                        MemberId = instance.MemberId,
                        ProductId = q.ProductId,
                        ProductNum = q.BuyNum,
                        IsDelete = false,
                        Selected = true,
                        CreateDate = DateTime.Now
                    });
                }
                else
                {
                    o.ProductNum += q.BuyNum;
                }
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

        public bool Update(int CartId, int BuyNum, bool IsSelected)
        {
            var m = db.Carts.FirstOrDefault(x => x.CartId == CartId);
            if (m == null)
            {
                return false;
            }
            m.ProductNum = BuyNum;
            m.Selected = IsSelected;
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

        /// <summary>
        /// 更新购物车信息
        /// </summary>
        /// <param name="CartId">购物车id</param>
        /// <param name="BuyNum">数量</param>
        /// <returns></returns>
        public bool Update(int CartId, int BuyNum)
        {
            var m = db.Carts.FirstOrDefault(x => x.CartId == CartId);
            if (m == null)
            {
                return false;
            }
            m.ProductNum = BuyNum;
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

        public bool Delete(int CartId)
        {
            var m = db.Carts.FirstOrDefault(x => x.CartId == CartId);
            if (m == null)
            {
                return false;
            }
            try
            {
                db.Carts.Remove(m);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除cart
        /// </summary>
        /// <param name="list">cid集合</param>
        /// <returns></returns>
        public bool Delete(int[] list)
        {
            try
            {
                using (db)
                {
                    foreach (var cid in list)
                    {
                        var m = db.Carts.FirstOrDefault(x => x.CartId == cid);
                        if (m == null)
                        {
                            return false;
                        }
                        db.Carts.Remove(m);
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

        public IEnumerable<CartDisplayModel> getPageList(int uid, int page = 1, int size = 20)
        {
            var strSql = new StringBuilder();
            var pageindex = size * (page - 1);
            strSql.AppendFormat(" select top {0} d.SubTitle,d.Describle, img.P1,img.P2,img.P3,  a.Selected, a.CartId,a.ProductId,a.ProductNum,a.MemberId,m.NickName as MemberName,d.SPMC,d.SPDM,d.SPGG,d.ProductGuid,d.Sale,d.Title,d.Discount*d.Sale*a.ProductNum as DiscountSale  from Cart as a  ", uid > 0 ? 100 : size);
            strSql.Append(" JOIN Member as m ON m.MemberId = a.MemberId ");
            strSql.Append(" JOIN Product as d ON d.ProductId = a.ProductId ");
            strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = a.ProductId");
            strSql.Append(" where 1 = 1 ");
            if (uid > 0)
            {
                strSql.AppendFormat(" and a.MemberId={0} ", uid);
            }
            else
            {
                strSql.AppendFormat(" AND a.CartId NOT IN (SELECT TOP {0} b.CartId FROM Cart as b ORDER BY b.CartId)  ORDER BY a.CartId; ", pageindex);
            }
            return db.Database.SqlQuery<CartDisplayModel>(strSql.ToString());

        }

        /// <summary>
        /// 获取提交订单的列表
        /// </summary>
        /// <param name="cid">cartId</param>
        /// <returns></returns>
        public IEnumerable<CartListForOrderViewModel> getPageList(int[] cid)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select img.P3 as Pic,  a.CartId,a.ProductId,a.ProductNum,d.SPMC,d.SPGG,d.Sale,d.Discount  from Cart as a  ");
            strSql.Append(" JOIN Product as d ON d.ProductId = a.ProductId ");
            strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = a.ProductId");
            strSql.AppendFormat(" where a.CartId in ({0})",string.Join(",", cid));
            return db.Database.SqlQuery<CartListForOrderViewModel>(strSql.ToString());
        }

        /// <summary>
        /// 获取购物车商品信息
        /// </summary>
        /// <param name="cid">购物车id列表</param>
        /// <returns></returns>
        public IEnumerable<CartForAddOrderModel> GetCartForAddOrder(int[] cid)
        {
            try
            {
                var list = new List<CartForAddOrderModel>();

                var strSql = new StringBuilder();
                strSql.Append(" select  a.CartId,a.ProductId,a.ProductNum,d.Sale,d.Discount  from Cart as a  ");
                strSql.Append(" JOIN Product as d ON d.ProductId = a.ProductId ");
                strSql.AppendFormat(" where a.CartId in ({0})", string.Join(",", cid));

                using (var db = new MircoShopEntities())
                {
                    list = db.Database.SqlQuery<CartForAddOrderModel>(strSql.ToString()).ToList();
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取购物车商品数量
        /// </summary>
        /// <param name="Menberid">客户id</param>
        /// <returns></returns>
        public int GetCartNunber(int Menberid)
        {
            try
            {
                using (db)
                {
                    var num = db.Carts.Where(x => x.MemberId == Menberid && x.IsDelete == false).Sum(x => x.ProductNum);
                    return (int)num;
                }
            }
            catch
            {
                return 0;
            }
        }


    }
}
