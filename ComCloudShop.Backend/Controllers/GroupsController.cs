using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;
using System.IO;

namespace ComCloudShop.Backend.Controllers
{
    public class GroupsController : BaseController
    {
        protected ProductService _service = new ProductService();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var query = _service.GetGroupList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int sysno)
        {
            var model = new ProductGroupViewModel();
            if (sysno > 0)
            {
                model = _service.GetProductGroup(sysno);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductGroupViewModel model, HttpPostedFileBase file)
        {

            if (string.IsNullOrEmpty(model.GroupName))
            {
                Response.Write("<script>alert('套餐名称不能为空');location.href=\"/groups/edit/?sysno=" + model.GroupId + "\";</script>");
            }

            var fileName = string.Empty;
            if (file != null)
            {
                fileName = Path.Combine(Request.MapPath("~/Upload/groups/"), Path.GetFileName(file.FileName));
            }
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    file.SaveAs(fileName);
                    model.PicUrl = "upload/groups/" + Path.GetFileName(file.FileName);
                }
                var b = false;
                if (model.GroupId == 0)
                {
                    b = _service.Add(model);
                }
                else
                {
                    b = _service.Update(model);
                }
                if (b)
                {
                    Response.Write("<script>alert('保存成功');location.href=\"/groups/index\";</script>");
                }
                else 
                {
                    Response.Write("<script>alert('保存错误');location.href=\"/groups/edit/?sysno=" + model.GroupId + "\";</script>");
                }
            }
            catch
            {
                Response.Write("<script>alert('出现异常错误');location.href=\"/groups/edit/?sysno=" + model.GroupId + "\";</script>");
            }
            return null;
        }

        public ActionResult Detail(int sysno,string dm,string mc) 
        {
            if (sysno == 0) 
            {
                return Content("参数错误");
            }
            var group = _service.GetProductGroup(sysno);
            if (group == null) 
            {
                return Content("参数错误");
            }
            ViewData["page"] = _service.GetProductPageCount(dm,mc, AppConstant.PageSize);
            ViewData["dm"] = dm;
            ViewData["mc"] = mc;
            return View(group);
        }

        public JsonResult ProductList(int page,string dm="",string mc="") 
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
        public JsonResult AddGroupRelation(int pid, int gid) 
        {
            var b = _service.AddProductGroupRelation(pid, gid);
            return Json(b, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Products(int sysno) 
        {
            if (sysno == 0)
            {
                return Content("参数错误");
            }
            var group = _service.GetProductGroup(sysno);
            if (group == null)
            {
                return Content("参数错误");
            }
            ViewData["id"] = sysno;
            return View();
        }

        public JsonResult RelationProduct(int sysno) 
        {
            var query = _service.GetRandomGroupList(sysno);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteGroupRelation(int pid, int gid)
        {
            var b = _service.DeleteProductGroupRelation(pid, gid);
            return Json(b, JsonRequestBehavior.AllowGet);
        }

    }
}
