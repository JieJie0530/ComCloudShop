using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;

namespace ComCloudShop.Web.Controllers
{
    public class AddressController : BaseController
    {
        private readonly DeliveryAddressService _service = new DeliveryAddressService();

        public ActionResult List(string c="")
        {
            var list = _service.GetAddresssList(UserInfo.Id, 1, 100);
            if (!string.IsNullOrEmpty( c ))
            {
                Session["address_source"] = c;
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult Edit(int? id) 
        {
            var m = _service.GetDeliveryAddressById(id.HasValue ? id.Value : 0);
            return View(m);
        }

        //[HttpPost]
        //public ActionResult Edit(DeliveryAddressViewModel model)
        //{
        //    var b = _service.SaveOrUpdate(model);
        //    return RedirectToAction("index?c=order", "address");
        //}

        [HttpPost]
        public ActionResult Edit(AddressEditViewModel model)
        {
            var data = new DeliveryAddressViewModel();

            data.AddressId = model.addressID;
            data.UserName = model.username;
            data.Mobile = model.mobile;
            data.Province = model.province;
            data.City = model.city;
            data.District = model.district;
            data.Address = model.address;
            data.MemberId = UserInfo.Id;

            var b = _service.SaveOrUpdate(data);
            return RedirectToAction("list", "address");
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="addressid"></param>
        /// <returns></returns>
        public JsonResult SetAddress(int addressid)
        {
            try
            {
                var data = Session[AppConstant.orderparm] as OrderParmModel;
                data.aid = addressid;
                Session[AppConstant.orderparm] = data;
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(1,JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除个人地址
        /// </summary>
        /// <param name="addressid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelAddress(int addressid)
        {
            try
            {
                if (_service.Delete(addressid,UserInfo.Id))
                {
                    return Json(0);
                }
                else
                {
                    return Json(1);
                }
            }
            catch
            {
                return Json(1);
            }
        }

	}
}