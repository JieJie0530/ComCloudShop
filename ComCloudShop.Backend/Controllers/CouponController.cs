using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;

namespace ComCloudShop.Backend.Controllers
{
    public class CouponController : BaseController
    {
        protected UserCouponService _service = new UserCouponService();

        public ActionResult Index()
        {
            ViewData["page"] = _service.GetUserCouponListPageCount(AppConstant.PageSize);
            return View();
        }

        public JsonResult List(int page = 1, int size = 10) 
        {
            var query = _service.GetUserCouponList(page, AppConstant.PageSize);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取列表页数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public JsonResult GetUserCouponListPage(string search)
        {
            var result = _service.GetUserCouponListPage(search);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 优惠券列表数据
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="openid"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult ListNew(string nickName, string openid, string mobile, int page = 1)
        {
            var result = _service.GetUserCouponListNew(page, AppConstant.PageSize, nickName, openid,mobile);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取卡券详情
        /// </summary>
        /// <param name="id">CouponID</param>
        /// <returns></returns>
        public JsonResult Detail(int id)
        {
            var result = _service.AdminDetail(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public JsonResult GetUserInfo(string openid)
        {
            var model = _service.GetUserInfo(openid);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddUserCoupon(UserCouponInfoViewModel model)
        {
            var result = _service.AddCoupon(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult Add(AdminUserCouponAddViewModel data)
        {
            return Json(_service.Add(data));
        }
    }
}
