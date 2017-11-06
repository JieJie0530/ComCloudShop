using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;
using ComCloudShop.Service;

namespace ComCloudShop.Backend.Controllers
{
    public class UserController : BaseController
    {
        protected MemberService _service = new MemberService();

        public ActionResult AddJi()
        {
            int MemberId = Convert.ToInt32(Request["MemberId"].ToString());
            int Type = Convert.ToInt32(Request["Type"]);
            int jifen = Convert.ToInt32(Request["jifen"]);

            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.MemberId == MemberId);
                if (Type == 1)
                {
                    m.integral-= jifen;
                }
                else {
                    m.integral+= jifen;
                }
                db.SaveChanges();
            }
            return Content("ok");
        }
        public ActionResult Withd() {
            return View();
        }

        public ActionResult Index()
        {
            ViewData["page"] = _service.GetUserPageCount(AppConstant.PageSize);
            return View();
        }


        
        /// <summary>
        /// 获取用户提现列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult listWithd(string nickName, int State, int page = 1)
        {

            var list = _service.GetMemberWithdrawalsListNew(page, AppConstant.PageSize, nickName, State);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult List( string nickName,string mobile,string openid, int isvip,int page = 1) 
        {
            var list = _service.GetMemberListNew(page, AppConstant.PageSize, nickName, mobile, openid, isvip);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ContentResult WithdOk(int id) {
            if (_service.UpdateWithd(id))
            {
                return Content("ok");
            }
            return Content("err");
        }

        public ContentResult RenZhen(int id) {

            if (_service.UpdateVips(id)) {
                return Content("ok");
            }
            return Content("err");
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Detail(int id)
        {
            var result = _service.GetMemberDetailNew(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
