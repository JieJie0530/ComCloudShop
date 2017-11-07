using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using ComCloudShop.WeixinOauth;
using ComCloudShop.Utility;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;

namespace ComCloudShop.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ProductService _service = new ProductService();


        public void UpdateJf(string jf)
        {
            string js = jf;
        }

        ComCloudShop.Layer.CateService bllcate = new CateService();
        MircoShopEntities db = new MircoShopEntities();
        ComCloudShop.Layer.ProductService bllpro = new ProductService();
        public ActionResult Index()
        {
            try
            {
                var data = _service.GetProductGroupSaleAll();
               
                
                BrandService bll = new BrandService();
                ViewBag.BrandList = bll.GetBrandList(1, 1, 10000000);
                CategoryService bllcate = new CategoryService();
                ViewBag.Cate = bllcate.GetCategoryList(1, 1, 10000000);

                ProductService bllps = new ProductService();
                ViewBag.Product = bllps.GetProductList(1, 10);

                //ViewBag.CateList = db.MangerCates.OrderByDescending(d => d.Sort).ToList();
                return View(data);
            }
            catch(Exception e)
            {
               return Error();
            }
        }
        public ActionResult ShopList()
        {
            string title = Request["title"];
            List<ComCloudShop.Service.Manger> list = new List<Manger>();
            using (var db = new MircoShopEntities())
            {
                list = db.Mangers.Where(d => d.Category.Contains(title) && d.State!=0).OrderBy(d => d.IsRecommend).ToList();
            }
            return View(list);
        }

        public ActionResult User() 
        {
            return View(UserInfo);
        }


        public ActionResult Map() {
            int id = Convert.ToInt32(Request["id"]);
            ComCloudShop.Service.Manger model = db.Mangers.Where(d => d.ID == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult Shop() {
            int id = Convert.ToInt32(Request["id"]);
            ComCloudShop.Service.Manger model=  db.Mangers.Where(d => d.ID == id).FirstOrDefault();
            return View(model);
        }



    }
}