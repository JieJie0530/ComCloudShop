using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;
using System.IO;
using ComCloudShop.Utility.Helper;

namespace ComCloudShop.Backend.Controllers
{
    public class ProductController : BaseController
    {
        protected ProductService _service = new ProductService();

        public ActionResult Index()
        {
            //ViewData["page"] = _service.GetProductPageCount(dm, mc, AppConstant.PageSize);
            //ViewData["spdm"] = dm;
            //ViewData["spmc"] = mc;
            int page = 1;
            if (Request["Page"] != null) {
                page = Convert.ToInt32(Request["Page"]);
            }
            
            ViewBag.Page = page;
            return View();
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="dm">商品代码</param>
        /// <param name="mc">商品名称</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        public JsonResult List(string dm, string mc, int page = 1)
        {
            string spgg = Request["spgg"].ToString();
            var query = _service.GetProductSearchListNew(spgg, dm, mc, page, AppConstant.PageSize);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <returns></returns>
        public JsonResult GetDetail(int id)
        {
            var query = _service.AdminGetProducDetail(id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取品种查询列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCategorySelect()
        {
            return Json(_service.GetCategorySelect(1), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取品牌查询列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBrandSelect()
        {
            return Json(_service.GetBrandSelect(1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Erp()
        {
            //var total = (double)ThirdPartyPackage.GetErpProductsCount();
            //ViewData["page"] = Math.Ceiling(total / AppConstant.PageSize) - 1;
            //ViewData["spdm"] = dm;
            return View();
        }

        /// <summary>
        /// 获取ERP 列表数据
        /// </summary>
        /// <param name="spdm">商品代码</param>
        /// <param name="spmc">商品名称</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        public JsonResult GetERPList(string spdm, string spmc, int page)
        {
            //获取管易ERP数据
            var result = ThirdPartyPackage.GetERPProductNew(page, AppConstant.PageSize, spdm, spmc);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="data">商品参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateProduct(AdminProductDetailViewModel data)
        {
            return Json(_service.UpdateProduct(data));
        }


        /// <summary>
        /// 获取产品详情列表
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public JsonResult GetProductItem(int ProductId)
        {
            return Json(_service.AdminGetProducItemDetail(ProductId),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/修改 产品详情
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdateProductDetail(AdminProductItemDetailViewModel data)
        {
            return Json(_service.AddOrUpdateProductDetail(data));
        }

        /// <summary>
        /// 删除 产品详情
        /// </summary>
        /// <param name="DetailId">产品详情主键</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteProductDetail(int DetailId)
        {
            return Json(_service.DeleteProductDetailNew(DetailId));
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImg(FormCollection collection)//HttpPostedFileBase upImg
        {
            //Request.Files["goods_image"];
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase upImg = files["upImg"];
            string fileName = System.IO.Path.GetFileName(upImg.FileName);
            long name = TimestampUtil.GenerateTimeStamp(DateTime.Now);
            string pic = "", error = "";
            //判断文件格式
            string filetype = fileName.Substring(fileName.LastIndexOf('.')).ToUpper();
            if ((fileName.LastIndexOf('.') > -1 && (filetype == ".MP4")))
            {
                try
                {
                    string filePhysicalPath = Server.MapPath("~/public/product/" + name + filetype);
                    upImg.SaveAs(filePhysicalPath);
                    pic = "public/product/" + name + filetype;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            else
            {
                error = "上传的文件格式不符合要求";
            }
            return Json(new
            {
                pic = pic,
                error = error
            });
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImg1(FormCollection collection)//HttpPostedFileBase upImg
        {
            //Request.Files["goods_image"];
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase upImg = files["upImg"];
            string fileName = System.IO.Path.GetFileName(upImg.FileName);
            long name = TimestampUtil.GenerateTimeStamp(DateTime.Now);
            string pic = "", error = "";
            //判断文件格式
            string filetype = fileName.Substring(fileName.LastIndexOf('.')).ToUpper();
            if ((fileName.LastIndexOf('.') > -1 && (filetype == ".JPG" || filetype == ".PNG" || filetype == ".JPEG")))
            {
                try
                {
                    string filePhysicalPath = Server.MapPath("~/public/product/" + name + filetype);
                    upImg.SaveAs(filePhysicalPath);
                    pic = "public/product/" + name + filetype;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            else
            {
                error = "上传的文件格式不符合要求";
            }
            return Json(new
            {
                pic = pic,
                error = error
            });
        }




        public JsonResult ErpList(string dm = "", int page = 1)
        {
            var query = ThirdPartyPackage.GetErpProducts(page, dm);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int sysno = 0)
        {
            var model = _service.Get(sysno);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductUpModel model)
        {
            var b = _service.Update(model);
            if (b)
            {
                Response.Redirect("/product/index");
            }
            else
            {
                Response.Write("<script>alert('保存错误');location.href=\"/product/edit/?status=" + model.ProductId + "\";</script>");
            }
            return null;
        }


        public ActionResult Detail(int sysno)
        {
            ViewData["Id"] = sysno;
            if (sysno == 0)
            {
                return Content("参数错误");
            }
            var model = _service.Get(sysno);
            if (model == null)
            {
                return Content("参数错误");
            }
            ViewData["Name"] = model.SPMC;
            return View(new ProductDetailViewModel());
        }

        public JsonResult DetailList(int sysno)
        {
            var query = _service.GetProductDetailList(sysno);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DetailUpdate(ProductDetailViewModel model, HttpPostedFileBase file)
        {
            var m = new ProductDetailViewModel();
            if (model.DetailId > 0)
            {
                m = _service.GetProductDetailById(model.DetailId);
                if (m == null)
                {
                    return Content("参数错误");
                }
            }
            var fileName = string.Empty;
            if (file != null)
            {
                fileName = Path.Combine(Request.MapPath("~/Upload/detail/"), Path.GetFileName(file.FileName));
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                file.SaveAs(fileName);
                model.A3 = Path.GetFileName(file.FileName);
            }
            var b = false;
            if (model.DetailId > 0)
            {
                b = _service.Update(model.DetailId, model.A1, model.A2, model.A3);
            }
            else
            {
                b = _service.Add(model);
            }
            if (b)
            {
                Response.Write("<script>alert('保存成功');location.href=\"/product/detail/?sysno=" + model.ProductId + "\";</script>");
            }
            else
            {
                Response.Write("<script>alert('保存错误');location.href=\"/product/detail/?sysno=" + model.ProductId + "\";</script>");
            }
            return null;

        }

        [HttpPost]
        public JsonResult DetailDelete(int sysno, string path)
        {
            if (sysno > 0)
            {
                var b = _service.DeleteProductDetail(sysno);
                if (b)
                {
                    path = Server.MapPath("~/Upload/detail/" + path);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                return Json(b, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 发布/下架
        /// </summary>
        /// <param name="sysno">ProductID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetIsShow(int sysno,bool show)
        {
            var b = _service.SetIsShowProduct(sysno,show);
            return Json(b);
        }

        /// <summary>
        /// 获取产品图片
        /// </summary>
        /// <param name="id">ProductID</param>
        /// <returns></returns>
        public JsonResult GetProductImg(int id)
        {
            return Json(_service.GetProductImg(id),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Erp 上架
        /// </summary>
        /// <param name="ProductGuid">ERP 商品Guid</param>
        /// <param name="SPDM">商品代码</param>
        /// <param name="SPMC">商品名称</param>
        /// <param name="Weight">重量</param>
        /// <param name="BZSJ">标准售价</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ErpToSale(string ProductGuid, string SPDM, string SPMC, decimal Weight, decimal BZSJ)
        {
            return Json(_service.ErpToSale(ProductGuid, SPDM, SPMC, Weight, BZSJ));
        }


        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, int ProductId)
        {
            if (ProductId < 1) {
                return Content("参数错误");
            }
            var model = new ProductImgViewModel();
            model.ProductId = ProductId;
            var i = 0;
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var path = Path.Combine(Request.MapPath("~/Upload/public/product/"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    if (i == 0)
                    {
                        model.P1 = "public/product/" + Path.GetFileName(file.FileName);
                    }
                    else if (i == 1)
                    {
                        model.P2 = "public/product/" + Path.GetFileName(file.FileName);
                    }
                    else if (i == 2)
                    {
                        model.P3 = "public/product/" + Path.GetFileName(file.FileName);
                    }
                }
                i++;
            }
            var b = _service.SaveOrUpdate(model);
            if (b)
            {
                Response.Write("<script>alert('保存成功');location.href=\"/product/index/\";</script>");
            }
            else
            {
                Response.Write("<script>alert('保存错误');location.href=\"/product/index/\";</script>");
            }
            return null;
        }
        [HttpPost]
        public JsonResult ErpSale(string ProductGuid, string SPDM, string SPMC, string Weight, string BZSJ)
        {
            var b = _service.AddErp(ProductGuid, SPDM, SPMC, Weight, BZSJ);
            return Json(b, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditShowPic(string Contents,string Proid)
        {
            
            ProductUpModel model = _service.Get(Convert.ToInt32(Proid));
            model.Contents = Contents;
            _service.Update1(model);
          
            return  Content("ok");
        }

        [HttpGet]
        public ActionResult EditShowPic(string sysno)
        {
            ProductUpModel model = _service.Get(Convert.ToInt32(sysno));
            int page = Convert.ToInt32(Request["Page"]);
            ViewBag.Page = page;
            return View(model);
        }

    }
}
