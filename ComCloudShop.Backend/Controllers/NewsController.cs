using ComCloudShop.Layer;
using ComCloudShop.Utility;
using ComCloudShop.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Backend.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/

        public ActionResult Index()
        {
            return View();
        }
         protected NewsService _service = new NewsService();


        //public ActionResult Add() {
        //    ComCloudShop.Service.News model = new Service.News();
        //    return View(model);
        //}

        ///// <summary>
        ///// 获取活动列表页
        ///// </summary>
        ///// <param name="page">当前页</param>
        ///// <returns></returns>
        //public JsonResult List(int page = 1)
        //{
        //    var query = _service.GetBatchList(page, AppConstant.PageSize);
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}


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
                    string filePhysicalPath = Server.MapPath("~/Upload/index/" + name + filetype);
                    upImg.SaveAs(filePhysicalPath);
                    pic = "Upload/index/" + name + filetype;
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

    }
}
