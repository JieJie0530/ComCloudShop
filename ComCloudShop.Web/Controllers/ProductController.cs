using ComCloudShop.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using ComCloudShop.ViewModel;
using ComCloudShop.WeixinOauth;
using ComCloudShop.Service;

namespace ComCloudShop.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ProductService _service = new ProductService();
     
        public ActionResult Index(string search="")
        {
            ViewBag.Search = search;
            ViewBag.type = 0;
            ViewBag.brand = 0;
            if (Request["type"] != null) {
                ViewBag.type = Request["type"].ToString();
                var _cservice = new CategoryService();
                int Types = Convert.ToInt32(Request["type"]);
                var data = _cservice.Get(Types);
                ViewBag.CateName = data.CategoryName;
            }
            if (Request["brand"] != null)
            {
                ViewBag.brand = Request["brand"].ToString();
            }
            return View();
        }
        MircoShopEntities db = new MircoShopEntities();
        public ActionResult Index1(string search = "")
        {
            ViewBag.Search = search;
            ViewBag.type = 0;
            ViewBag.brand = 0;
            if (Request["type"] != null)
            {
                ViewBag.type = Request["type"].ToString();
            }
            if (Request["brand"] != null)
            {
                ViewBag.brand = Request["brand"].ToString();
            }
            return View();
        }
        /// <summary>
        /// 商品详情页
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public ActionResult Detail(int pid)
        {

            Member m = db.Members.Where(d => d.MemberId == UserInfo.Id).FirstOrDefault();
            
            ViewBag.Rols = m.ISVip;
               
            var data = _service.GetDetailById(pid);
            OrderService _serviceorder = new OrderService();
            var CommentList = _serviceorder.GetCommentList(pid);
            ViewData["CommentList"] = CommentList;
            return View(data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="search"></param>
        /// <param name="type"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProductList1()
        {
            var result = new ResultViewModel<IEnumerable<ProductListViewModel>>();
            try
            {
                var data = _service.GetProductList();
                result.error = 0;
                result.msg = "success";
                result.result = new List<ProductListViewModel>();
                result.result = data;
            }
            catch (Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="search"></param>
        /// <param name="type"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProductList10(int type = 0)
        {
            var result = new ResultViewModel<IEnumerable<ProductListViewModel>>();
            try
            {
                var data = _service.GetProductList10(type);
                result.error = 0;
                result.msg = "success";
                result.result = new List<ProductListViewModel>();
                result.result = data;
            }
            catch (Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="search"></param>
        /// <param name="type"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProductList(string spgg,int page = 1, int size = 20, string search = "", int type = 0, int begin = 0, int end = 0,int minprice=0,int maxprice = 0)
        {
            var result = new ResultViewModel<IEnumerable<ProductListViewModel>>();
            try
            {
                var data = _service.GetProductList(spgg,page, size, search, type, begin, end, minprice, maxprice);
                result.error = 0;
                result.msg = "success";
                result.result = new List<ProductListViewModel>();
                result.result = data;
            }
            catch(Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetCategoryList(int type = 1)
        {
            var result = new ResultViewModel<IEnumerable<CategoryViewModel>>();
            try 
            { 
                var _cservice = new CategoryService();
                int Types = Convert.ToInt32(Request["types"]);
                var data = _cservice.GetCategoryList(type, Types);
                result.error = 0;
                result.msg = "success";
                result.result = new List<CategoryViewModel>();
                result.result = data;
            }
            catch(Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } 

	}
}