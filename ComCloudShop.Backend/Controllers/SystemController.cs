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
    public class SystemController : BaseController
    {

        protected SystemService _service = new SystemService();

        public ActionResult Index()
        {
            if (Request["type"] != null) {
                ViewBag.Type = Request["type"].ToString();
            }
            return View();
        }

        /// <summary>
        /// 获取活动列表页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        public JsonResult List(int page = 1)
        {
            string type = Request.QueryString["type"];
            var query = _service.GetBatchList(page, AppConstant.PageSize, type);
            return Json(query, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(BatchListViewModel data)
        {
            return Json(_service.AddOrUpdate(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="PicId">PicId</param>
        /// <param name="Image">图片路径</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNew(int PicId, string Image)
        {
            var result = _service.DeleteNew(PicId);
            if (result.error == 100)
            {
                Image = Server.MapPath("~/" + Image);
                if (System.IO.File.Exists(Image))
                {
                    System.IO.File.Delete(Image);
                }
            }
            return Json(result);
        }


        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file)
        {
            var fileName = string.Empty;
            var model = new HomeImageViewModel();
            if (file != null)
            {
                fileName = Path.Combine(Request.MapPath("~/Upload/index/"), Path.GetFileName(file.FileName));
            }
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    file.SaveAs(fileName);
                    model.Image = "upload/index/" + Path.GetFileName(file.FileName);
                }
                model.Useful = "首页";
                var b = false;
                b = _service.SaveOrUpdate(model);
                if (b)
                {
                    Response.Write("<script>alert('保存成功');location.href=\"/system/index\";</script>");
                }
                else
                {
                    Response.Write("<script>alert('保存错误');location.reload();</script>");
                }
            }
            catch
            {
                Response.Write("<script>alert('出现异常错误');location.reload();</script>");
            }
            return null;
        }

        [HttpPost]
        public JsonResult Delete(int sysno, string path)
        {
            var b = _service.Delete(sysno);
            if (b)
            {
                path = Server.MapPath("~/" + path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return Json(b, JsonRequestBehavior.AllowGet);
        }
    }
}
