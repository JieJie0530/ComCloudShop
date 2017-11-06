using ComCloudShop.Layer;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using ComCloudShop.Utility.Helper;

namespace ComCloudShop.Backend.Controllers
{
    public class SaleController : BaseController
    {
        protected ProductService _service = new ProductService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取推荐活动列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult List(int page = 1)
        {
            var query = _service.GetGroupSaleList(page, AppConstant.PageSize);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/修改最新推荐
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(ProductSaleGroupNewViewModel data)
        {
            return Json(_service.AddOrUpdateProductSaleGroup(data));
        }

        /// <summary>
        /// 上移下移
        /// </summary>
        /// <param name="SaleId">主键SaleId</param>
        /// <param name="flag">1：上移；2：下移</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Move(int SaleId,int flag)
        {
            return Json(_service.MoveProductSaleGroup(SaleId,flag));
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNew(ProductSaleGroupNewViewModel data)
        {
            var result = _service.DeleteProductSaleGroupNew(data.SaleId);
            if (result.error == 100)
            {
                data.PicBigUrl = Server.MapPath("~/" + data.PicBigUrl);
                if (System.IO.File.Exists(data.PicBigUrl))
                {
                    System.IO.File.Delete(data.PicBigUrl);
                }

                data.PicSmallUrl = Server.MapPath("~/" + data.PicSmallUrl);
                if (System.IO.File.Exists(data.PicSmallUrl))
                {
                    System.IO.File.Delete(data.PicSmallUrl);
                }
            }
            return Json(result);
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
            if ((fileName.LastIndexOf('.') > -1 && (filetype == ".JPG" || filetype == ".PNG" || filetype == ".JPEG")))
            {
                try
                {
                    string filePhysicalPath = Server.MapPath("~/upload/sale/" + name + filetype);
                    upImg.SaveAs(filePhysicalPath);
                    pic = "upload/sale/" + name + filetype;
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
        /// 添加产品到推荐列表
        /// </summary>
        /// <param name="SaleId">最新推荐主键</param>
        /// <param name="ProductId">产品主键</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddSaleRelation(int SaleId, int ProductId)
        {
            return Json(_service.AddSaleRelation(SaleId, ProductId));
        }

        /// <summary>
        /// 获取最新推荐的商品列表
        /// </summary>
        /// <param name="saleId">最新推荐主键</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        public JsonResult GetRelationList(int saleId,int page)
        {
            return Json(_service.GetSaleRelationList(page, AppConstant.PageSize, saleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取 未推荐的商品列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <param name="saleId">最新推荐主键</param>
        /// <param name="spdm">商品代码</param>
        /// <param name="spmc">商品名称</param>
        /// <returns></returns>
        public JsonResult GetProductList(int page, int size,int saleId,string spdm,string spmc)
        {
            return Json(_service.GetSaleRelationProductList(page, size, saleId, spdm, spmc), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除 最新推荐产品关系表
        /// </summary>
        /// <param name="RelationId">最新推荐产品关系表主键</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteSaleRelation(int RelationId)
        {
            return Json(_service.DeleteSaleRelation(RelationId));
        }








        public ActionResult Edit(int sysno)
        {
            var model = new ProductGroupSaleViewModel();
            if (sysno > 0)
            {
                model = _service.GetProductSaleGroup(sysno);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductGroupSaleViewModel model, HttpPostedFileBase filesmall, HttpPostedFileBase filebig)
        {
            if (string.IsNullOrEmpty(model.saleName))
            {
                Response.Write("<script>alert('活动名称不能为空');location.href=\"/sale/edit/?sysno=" + model.saleId + "\";</script>");
            }

            try
            {
                if (filesmall != null)
                {
                    var fileName = model.saleName + "_small" + Path.GetExtension(filesmall.FileName);
                    filesmall.SaveAs(Path.Combine(Request.MapPath("~/Upload/sale/"), fileName));
                    model.picSmallUrl = "upload/sale/" + fileName;
                }
                if (filebig != null)
                {
                    var fileName = model.saleName + "_big" + Path.GetExtension(filebig.FileName);
                    filebig.SaveAs(Path.Combine(Request.MapPath("~/Upload/sale/"), fileName));
                    model.picBigUrl = "upload/sale/" + fileName;
                }

                var b = false;
                if (model.saleId == 0)
                {
                    b = _service.Add(model);
                }
                else
                {
                    b = _service.Update(model);
                }
                if (b)
                {
                    Response.Write("<script>alert('保存成功');location.href=\"/sale/index\";</script>");
                }
                else
                {
                    Response.Write("<script>alert('保存错误');location.href=\"/sale/edit/?sysno=" + model.saleId + "\";</script>");
                }
            }
            catch
            {
                Response.Write("<script>alert('出现异常错误');location.href=\"/sale/edit/?sysno=" + model.saleId + "\";</script>");
            }
            return null;
        }

        [HttpPost]
        public JsonResult Delete(int sysno)
        {
            var b = _service.DeleteProductSaleGroup(sysno);
            return Json(b);
        }

        public ActionResult Detail(int sysno, string dm, string mc)
        {
            if (sysno == 0)
            {
                return Content("参数错误");
            }
            var sale = _service.GetProductSaleGroup(sysno);
            if (sale == null)
            {
                return Content("参数错误");
            }
            ViewData["page"] = _service.GetProductPageCount(dm, mc, AppConstant.PageSize);
            ViewData["dm"] = dm;
            ViewData["mc"] = mc;
            return View(sale);
        }

        public JsonResult ProductList(int page, string dm = "", string mc = "")
        {
            if (string.IsNullOrEmpty(dm) && string.IsNullOrEmpty(mc))
            {
                var query = _service.GetProductList(page, AppConstant.PageSize);
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            var list = _service.GetProductSearchList(dm, mc, page, AppConstant.PageSize);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddGroupRelation(GroupSaleRelationViewModel model)
        {
            var b = _service.Add(model);
            return Json(b, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Products(int sysno)
        {
            if (sysno == 0)
            {
                return Content("参数错误");
            }
            var group = _service.GetProductSaleGroup(sysno);
            if (group == null)
            {
                return Content("参数错误");
            }
            ViewData["id"] = sysno;
            ViewData["name"] = group.saleName;
            return View();
        }

        public JsonResult RelationProduct(int sysno)
        {
            var query = _service.GetRandomGroupSaleList(sysno);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteGroupRelation(int pid)
        {
            var b = _service.DeleteProductGroupSaleRelation(pid);
            return Json(b, JsonRequestBehavior.AllowGet);
        }

    }
}
