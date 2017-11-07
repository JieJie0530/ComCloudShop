using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Web.Controllers
{
    public class SaleController : BaseController
    {
        private readonly ProductService _service = new ProductService();

        // GET: Sale
        public ActionResult Index(int pid)
        {
            try
            {
                var data = _service.GetSaleDetail(pid);
                return View(data);
            }
            catch
            {
                return Error();
            }
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public JsonResult GetPageList(int pid,int page = 1, int size=10)
        {
            var result = new ResultViewModel<IEnumerable<SaleProductListModel>>();
            try
            {
                result.result = new List<SaleProductListModel>();
                result.result = _service.GetSaleProductPageList(page, size, pid);
                result.error = 0;
                result.msg = "success";
            }
            catch(Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

    }
}