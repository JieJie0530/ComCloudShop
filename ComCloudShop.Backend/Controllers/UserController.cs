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
        //新增或更新会员
        public ActionResult Add()
        {
            using (var db = new MircoShopEntities())
            {
                string MemberID = Request["MemberID"];
                string NickName = Request["NickName"];
                string Mobile = Request["Mobile"];
                string flollw = Request["Follow"];
                Member m = new Member();
                if (MemberID != "")
                {
                    int MemberIDint = Convert.ToInt32(MemberID);
                    m = db.Members.Where(d => d.MemberId == MemberIDint).FirstOrDefault();
                    m.NickName = NickName;
                    m.Mobile = Mobile;
                    m.follow = flollw;
                    m.ContactAddr = "";
                    m.UserName = "";
                    m.OrignKey = "";
                }
                else
                {
                    m.NickName = NickName;
                    m.Mobile = Mobile;
                    m.follow = flollw;
                    m.OpenId = Guid.NewGuid().ToString();
                    m.fsate = 0;
                    m.ISVip = 0;
                    m.integral = 0;
                    m.TotalIn = 0;
                    m.balance = "0";
                    m.Cashbalance = "0";
                    m.HeadImgUrl = "";
                    m.CreateDate = DateTime.Now;
                    m.LastLoginDate = 0;
                    m.Gender = 0;
                    m.ContactAddr = "";
                    m.UserName = "";
                    m.OrignKey = "";
                    db.Members.Add(m);
                }

                db.SaveChanges();
            }
            return Content("ok");

        }
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
        public ActionResult Index1()
        {
            
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
