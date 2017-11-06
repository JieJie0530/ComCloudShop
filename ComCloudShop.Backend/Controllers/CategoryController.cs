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
    public class CategoryController : BaseController
    {
        protected CategoryService _service = new CategoryService();

        public ActionResult Index(int status = 1)
        {
            //if (status == 0) 
            //{
            //    return Content("参数错误");
            //}
            //ViewData["name"] = status == 1 ? "品种管理" : "功能管理";
            //ViewData["btnName"] = status == 2 ? "品种管理" : "功能管理";
            //ViewData["status"] = status;
            return View();
        }

        /// <summary>
        /// 获取品种列表
        /// </summary>
        /// <param name="status">1：品种；2：功能；</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>ge
        public JsonResult List(int status, int page = 1)
        {
            var query = _service.GetCategoryList(status, page, AppConstant.PageSize);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/修改 品种/功能
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(CategoryViewModel data)
        {
            return Json(_service.AddOrUpdate(data));
        }

        /// <summary>
        /// 删除 品种/功能
        /// </summary>
        /// <param name="CategoryId">主键CategoryId</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNew(int CategoryId)
        {
            return Json(_service.DeleteNew(CategoryId));
        }


        public ActionResult Edit(int status, int cid = 0) 
        {
            if (status == 0)
            {
                return Content("参数错误");
            }
            ViewData["name"] = status == 1 ? "品种" : "功能";
            ViewData["status"] = status;
            CategoryViewModel m;
            if (cid == 0) 
            {
                m = new CategoryViewModel();
            }
            m = _service.Get(cid);
            if (m == null) 
            {
                m = new CategoryViewModel();
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model) 
        {
            var b = false;
            if (model.CategoryId == 0)
            {
                b = _service.Add(model);
            }
            else 
            {
                b = _service.Update(model.CategoryId, model.CategoryName, model.CategoryType);
            }
            if (b)
            {
                Response.Redirect("/category/index/?status=" + model.CategoryType);
            }
            else 
            {
                Response.Write("<script>alert('保存错误');location.href=\"/category/edit/?status=" + model.CategoryType + "&cid="+model.CategoryId+"\";</script>");
            }
            return null;
        }

        /// <summary>
        /// 删除品种
        /// </summary>
        /// <param name="sysno">品种ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int sysno)
        {
            var b = _service.Delete(sysno);
            return Json(b);
        }

    }
}
