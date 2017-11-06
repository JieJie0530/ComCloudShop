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
    public class RuleController : BaseController
    {
        protected RuleService _service = new RuleService();

        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult List(int page=1,int size=10) 
        //{
        //    var query = _service.GetRulesList(page, size);
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Delete(int sysno) 
        //{
        //    var b = _service.Delete(sysno);
        //    return Json(b, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Edit(int id, decimal amount, decimal discount) 
        //{
        //    var r = new RuleViewModel();
        //    r.Id = id;
        //    r.Amount = amount;
        //    r.Discount = discount;
        //    if (r.Discount > r.Amount) 
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //    r.Rule = "满" + r.Amount + "减" + discount;
        //    var b = _service.SaveOrUpdate(r);
        //    return Json(b, JsonRequestBehavior.AllowGet);
        //}



        /// <summary>
        /// 获取满减规则列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        public JsonResult List(int page = 1)
        {
            var query = _service.GetRulesList(page,AppConstant.PageSize);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/修改 满减规则
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(AdminRuleViewModel data)
        {
            return Json(_service.AddOrUpdate(data));
        }

        /// <summary>
        /// 删除 满减规则
        /// </summary>
        /// <param name="RuleId">主键RuleId</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int RuleId)
        {
            return Json(_service.Delete(RuleId));
        }

    }
}
