using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;
using ComCloudShop.Utility;


namespace ComCloudShop.Layer
{
    public class UserCouponService
    {
        public IEnumerable<UserCouponViewModel> GetUserCouponList(int page = 1, int size = 10)
        {
            using (var db = new MircoShopEntities())
            {
                var query = (from q in db.UserCoupons
                             join u in db.Members on q.OpenId equals u.OpenId into temp
                             from tt in temp.DefaultIfEmpty()
                             orderby q.CouponId ascending
                             select new
                             {
                                 UserCoupon = q,
                                 NickName = tt == null ? q.OpenId : tt.NickName
                             }).Skip((page - 1) * size).Take(size);

                var list = new List<UserCouponViewModel>();

                foreach (var q in query)
                {
                    list.Add(new UserCouponViewModel
                    {
                        Id = q.UserCoupon.CouponId,
                        Amount = q.UserCoupon.Amount,
                        NickName = q.NickName,
                        ConsumVal = q.UserCoupon.Consum == 0 ? "不限" : q.UserCoupon.Consum.ToString(),
                        IsUseVal = q.UserCoupon.IsUse == 1 ? "已用" : "未用",
                        Original = q.UserCoupon.Original,
                        OrderId = q.UserCoupon.OrderNum
                    });
                }
                return list;
            }
        }

        public double GetUserCouponListPageCount(int size =10)
        {
            using (var db = new MircoShopEntities())
            {
                var i = (double)db.UserCoupons.Count() / size;
                return Math.Ceiling(i);
            }
        }
       
        /// <summary>
        /// 获取优惠券列表数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName"></param>
        /// <param name="openid"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminUserCouponListViewModel>> GetUserCouponListNew(int page, int size, string nickName, string openid, string mobile)
        {
            var result = new ResultViewModel<IEnumerable<AdminUserCouponListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.UserCoupons
                                     join b in db.Members on a.OpenId equals b.OpenId into temp
                                     from c in temp.DefaultIfEmpty()
                                     where (string.IsNullOrEmpty(nickName) ? 1 == 1 : c.NickName.Contains(nickName)) &&
                                                (string.IsNullOrEmpty(openid) ? 1 == 1 : a.OpenId.Contains(openid)) &&
                                                (string.IsNullOrEmpty(mobile) ? 1 == 1 : c.Mobile.Contains(mobile))
                                     orderby a.CouponId descending
                                     select new AdminUserCouponListViewModel
                                     {
                                         CouponId = a.CouponId,
                                         OpenId = a.OpenId,
                                         NickName = c.NickName,
                                         HeadImgUrl = c.HeadImgUrl,
                                         Mobile = c.Mobile,
                                         Amount = a.Amount,
                                         Consum = a.Consum,
                                         Original = a.Original,
                                         IsUse = a.IsUse
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.UserCoupons
                                    join b in db.Members on a.OpenId equals b.OpenId into temp
                                    from c in temp.DefaultIfEmpty()
                                    where (string.IsNullOrEmpty(nickName) ? 1 == 1 : c.NickName.Contains(nickName)) &&
                                               (string.IsNullOrEmpty(openid) ? 1 == 1 : a.OpenId.Contains(openid)) &&
                                               (string.IsNullOrEmpty(mobile) ? 1 == 1 : c.Mobile.Contains(mobile))
                                    orderby a.CouponId descending
                                    select new AdminUserCouponListViewModel
                                    {
                                        CouponId = a.CouponId
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

        /// <summary>
        /// 获取卡券详情
        /// </summary>
        /// <param name="id">CouponId</param>
        /// <returns></returns>
        public ResultViewModel<AdminUserCouponDetailViewModel> AdminDetail(int id)
        {
            var result = new ResultViewModel<AdminUserCouponDetailViewModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.UserCoupons
                                     join b in db.Members on a.OpenId equals b.OpenId into temp
                                     from c in temp.DefaultIfEmpty()
                                     where a.CouponId ==id
                                     select new AdminUserCouponDetailViewModel
                                     {
                                         CouponId = a.CouponId,
                                         OpenId = a.OpenId,
                                         NickName = c.NickName,
                                         HeadImgUrl = c.HeadImgUrl,
                                         Mobile = c.Mobile,
                                         Amount = a.Amount,
                                         Consum = a.Consum,
                                         Original = a.Original,
                                         IsUse = a.IsUse,
                                         OrderNum = a.OrderNum,
                                         CreateDate =a.CreateDate
                                     }).FirstOrDefault();

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


        /// <summary>
        /// 获取数据页数
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public double GetUserCouponListPage(string search,int size = 10)
        {
            using (var db = new MircoShopEntities())
            {
                var i = (double)(from q in db.UserCoupons
                                 join u in db.Members on q.OpenId equals u.OpenId into temp
                                 from tt in temp.DefaultIfEmpty()
                                 where search == null || search.Trim() == "" ? 1 == 1 : tt.OpenId.Contains(search) || tt.NickName.Contains(search)
                                 orderby q.CouponId ascending
                                 select new
                                 {
                                     UserCoupon = q,
                                     NickName = tt == null ? q.OpenId : tt.NickName
                                 }).Count() / size;
                return Math.Ceiling(i);
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public UserCouponInfoViewModel GetUserInfo(string openid)
        {
            if (openid != null && openid.Trim() != "")
            {
                using (var db = new MircoShopEntities())
                {
                    var model = (from a in db.Members
                                 where a.OpenId == openid
                                 select new UserCouponInfoViewModel
                                 {
                                     NickName = a.NickName,
                                     OpenId = a.OpenId,
                                     Gender = a.Gender
                                 }).FirstOrDefault();
                    return model;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddCoupon(UserCouponInfoViewModel model)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var user=db.Members.FirstOrDefault(x=>x.OpenId==model.OpenId);
                    if (user == null)
                    {
                        return 2;
                    }
                    var Coupon = new UserCoupon();
                    Coupon.OpenId = model.OpenId;
                    Coupon.Amount = model.Amount;
                    Coupon.Consum = model.Consum;
                    Coupon.CouponCode = model.CouponCode;
                    Coupon.ValidityDate = model.ValidityDate;
                    Coupon.PeriodDate = model.PeriodDate;
                    Coupon.CreateDate = (Nullable<int>)Convert.ToInt32(ComCloudShop.Utility.Helper.TimestampUtil.GenerateTimeStamp(DateTime.Now));
                    Coupon.IsUse = model.IsUse;
                    Coupon.Original = model.Original;
                    db.UserCoupons.Add(Coupon);
                    if (db.SaveChanges() > 0)
                        return 0;
                    else
                        return 1;
                }
            }
            catch 
            {
                return 1;
            }
        }


        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultViewModel<bool> Add(AdminUserCouponAddViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new UserCoupon();
                    model.OpenId = data.OpenId;
                    model.Amount = data.Amount;
                    model.Consum = data.Consum;
                    model.CreateDate = (int)ComCloudShop.Utility.Helper.TimestampUtil.GenerateTimeStamp(DateTime.Now);
                    model.Original = data.Original;
                    model.PeriodDate = 0;
                    model.ValidityDate = 0;
                    db.UserCoupons.Add(model);
                    if (db.SaveChanges() > 0)
                    {
                        result.error = (int)ErrorEnum.OK;
                        result.msg = "success";
                    }
                    else
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg ="0条数据受影响";
                    }
                }
            }
            catch(Exception ex)
            {
                result.error = (int)ErrorEnum.Fail;
                result.msg = ex.Message;
            }
            return result;
        }

    }
}
