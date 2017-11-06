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
    public class AddressController : BaseController
    {
        protected DeliveryAddressService _service = new DeliveryAddressService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取 地址 List
        /// </summary>
        /// <param name="nickName">会员昵称</param>
        /// <param name="userName">收件人</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult List(string nickName, string userName, int page = 1)
        {
            var list = _service.AdminGetAddressList(page, AppConstant.PageSize, nickName, userName);
            return Json(list, JsonRequestBehavior.AllowGet);
        }




    }
}
