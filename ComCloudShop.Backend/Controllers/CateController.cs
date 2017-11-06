using ComCloudShop.Layer;
using ComCloudShop.Utility;
using ComCloudShop.Utility.Helper;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Backend.Controllers
{
    public class CateController : Controller
    {
        //
        // GET: /Cate/

        public ActionResult Index()
        {
            return View();
        }

        protected CateService _service = new CateService();
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="status">1：品种；2：功能；</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>ge
        public JsonResult List(int status, int page = 1)
        {
            var query = _service.GetBrandList(status, page, AppConstant.PageSize);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/修改 品种/功能
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(CateViewModel data)
        {
            return Json(_service.AddOrUpdate(data));
        }

        /// <summary>
        /// 删除 品种/功能
        /// </summary>
        /// <param name="CategoryId">主键CategoryId</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNew(int ID)
        {
            return Json(_service.DeleteNew(ID));
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

    }
}
