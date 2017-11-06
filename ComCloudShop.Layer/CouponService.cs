using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Layer
{
    public  class CouponService:BaseService
    {
        /// <summary>
        /// 获取可使用最优惠的卡券数据
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="price">总价</param>
        /// <returns></returns>
        public CouponDetailViewModel GetBestCoupon(string openid, decimal price)
        {
            var model = new CouponDetailViewModel();
            try
            {
                using (db)
                {
                    var data = db.UserCoupons.OrderByDescending(x=>x.Amount).FirstOrDefault(x => x.OpenId == openid && x.Consum <= price&&x.IsUse==0);

                    if (data != null)
                    {
                        model.CouponId = data.CouponId;
                        model.Amount = (decimal)data.Amount;
                    }
                    return model;
                }
            }
            catch
            {
                return null;
            }
        }

        public CouponDetailViewModel GetCouponById(int id)
        {
            var model = new CouponDetailViewModel();
            try
            {
                using (db)
                {
                    var data = db.UserCoupons.FirstOrDefault(x => x.CouponId == id && x.IsUse == 0);

                    if (data != null)
                    {
                        model.CouponId = data.CouponId;
                        model.Amount = (decimal)data.Amount;
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
        /// 获取卡券列表
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="price"></param>
        /// <param name="flag">true：全部可用卡券；2：满足订单的卡券</param>
        /// <returns></returns>
        public IEnumerable<CouponListViewModel> GetCouponList(string openid, decimal price,bool flag)
        { 
            var list = new List<CouponListViewModel>();
            try
            {
                using (db)
                {
                    //var data = new List<Service.UserCoupon>();
                    //if (flag)
                    //{
                    //    var data = db.UserCoupons.OrderByDescending(x => x.Amount).Where(x => x.OpenId == openid && x.IsUse == 0).ToList();
                    //}
                    //else
                    //{
                    //    data = db.UserCoupons.OrderByDescending(x => x.Amount).Where(x => x.OpenId == openid && x.Consum <= price && x.IsUse == 0).ToList();
                    //}

                    var data = db.UserCoupons.OrderByDescending(x => x.Amount).Where(x => x.OpenId == openid && x.IsUse == 0);

                    if (data.Count()>0)
                    {
                        CouponListViewModel model;
                        foreach (var d in data)
                        {
                            model = new CouponListViewModel();
                            model.CouponId = d.CouponId;
                            if (flag)
                            {
                                model.IsCanUse = true;
                            }
                            else
                            {
                                if (d.Consum <= price)
                                {
                                    model.IsCanUse = true;
                                }
                                else
                                {
                                    model.IsCanUse = false;
                                }
                            }
                            if (d.Consum > 0)
                            {
                                model.S1 = "满" + d.Consum + "减" + d.Amount + "元";
                            }
                            else
                            {
                                model.S1 =  d.Amount + "元";
                            }
                            model.S2 = "有效范围：全场使用";
                            model.S3 = "有效期：无限制，仅此一次";

                            list.Add(model);
                        }
                    }
                    return list;
                }
            }
            catch
            {
                return list;
            }
        }

    }
}
