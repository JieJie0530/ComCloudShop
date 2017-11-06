using ComCloudShop.Layer;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Web.Controllers
{
    public class CouponController : BaseController
    {
        CouponService _service = new CouponService();

        /// <summary>
        /// 卡券页面
        /// </summary>
        /// <param name="c">来源</param>
        /// <param name="price">总额</param>
        /// <returns></returns>
        public ActionResult Index(string c="")
        {
            try
            {
                var flag = true;
                var price = 0M;
                if (c == "order")
                {
                    flag = false;
                    var parm = Session[AppConstant.orderparm] as OrderParmModel;
                    var _oservice = new OrderService();
                    price = _oservice.GetOrderTotalPrice(parm.list);
                    if (price == 0M)
                    {
                        return Error();
                    }
                }
                var data = _service.GetCouponList(UserInfo.openid, price, flag);
                Session["coupon_source"] = c;

                return View(data);
            }
            catch
            {
                return Error();
            }
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="couponid"></param>
        /// <returns></returns>
        public JsonResult SetCoupon(int couponid)
        {
            try
            {
                var data = Session[AppConstant.orderparm] as OrderParmModel;
                data.cid = couponid;
                Session[AppConstant.orderparm] = data;
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

	}
}