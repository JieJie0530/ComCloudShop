using ComCloudShop.Service;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using ComCloudShop.WeixinOauth;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Web.Controllers
{
    public class BusinessController : BaseMangeController
    {
        // GET: Business
        public ActionResult Index()
        {
            ComCloudShop.Service.Manger model = null;
            if (Session[AppConstant.weixinAdminuser] != null)
            {
                model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;

                model = db.Mangers.Where(d => d.ID == model.ID).FirstOrDefault();

                ViewData["SumPrice"] = db.IntLists.Where(d => d.ManagerID == model.ID.ToString()).Sum(d => d.Price);
                ViewData["GifCount"] = db.GiftLists.Where(d => d.ManagerID == model.ID.ToString() && d.State==1).Count();

            }
            else {
                return RedirectToAction("Login", "Business");
            }
            return View(model);
        }

        // GET: Business
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        MircoShopEntities db = new MircoShopEntities();
        [HttpPost]
        public JsonResult Logins()
        {
            string username = Request["UserName"];
            string Pwd = Request["Pwd"];
            var result = new ResultViewModel<string>();
            var list = db.Mangers.Where(d => d.UserName == username && d.Pwd == Pwd);
            if (list.Count() > 0)
            {

                var model = this.Session["OAmodel"] as OAuthUserInfo;
                ComCloudShop.Service.Manger models = list.FirstOrDefault();
                models.OpenID = model.openid;
                db.Entry<ComCloudShop.Service.Manger>(models).State = EntityState.Modified;
                db.SaveChanges();
                
                Session[AppConstant.weixinAdminuser] = models;

                result.result = "登录成功";
                result.error = 0;
                result.msg = "登录成功";
            }
            else {
                result.error = 1;
                result.msg = "用户名密码错误";
            }
            return Json(result);
        }

        [HttpGet]
        public ActionResult IntegralIndex() {
            ComCloudShop.Service.Manger model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;
            //List<ComCloudShop.Service.GiftList> list = db.GiftLists.Where(d => d.ManagerID == model.ID.ToString()).ToList();

            try
            {
                using (var db = new MircoShopEntities())
                {
                    List<IntViewModel> list = (from a in db.IntLists
                                                join
                                                    e in db.Members on a.MemberID equals e.MemberId.ToString()
                                                where a.ManagerID == model.ID.ToString()
                                                select new IntViewModel
                                                {
                                                    ID = a.ID,
                                                    MemberID = a.MemberID,
                                                    ManagerID = a.ManagerID,
                                                    State = (int)a.State,
                                                    NickName = e.NickName,
                                                    HeadImgUrl=e.HeadImgUrl,
                                                    Price= (decimal)a.Price,
                                                    AddTime=(DateTime)a.AddTime
                                                }).ToList();
                    return View(list);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 赠品领取记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GitIndex()
        {
            ComCloudShop.Service.Manger model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;
            //List<ComCloudShop.Service.GiftList> list = db.GiftLists.Where(d => d.ManagerID == model.ID.ToString()).ToList();

            try
            {
                using (var db = new MircoShopEntities())
                {
                    List<GiftViewModel> list = (from a in db.GiftLists
                                join
                                    b in db.ProductImgs on a.ProductID equals b.ProductId.ToString()
                                join
                                    e in db.Members on a.MemberID equals e.MemberId.ToString()
                                join
                                    f in db.Products on a.ProductID equals f.ProductId.ToString()
                                where a.ManagerID == model.ID.ToString()
                                select new GiftViewModel
                                {
                                    ID = a.ID,
                                    SPMC = f.SPMC,
                                    P1 = b.P1,
                                    MemberID = a.MemberID,
                                    ProductID = a.ProductID,
                                    AddTime = (DateTime)a.AddTime,
                                    LQTime = (DateTime)a.LQTime,
                                    OrderID = a.OrderID,
                                    ManagerID = a.ManagerID,
                                    State = (int)a.State,
                                    NickName=e.NickName
                                }).ToList();
                    return View(list);
                }
            }
            catch
            {
                return null;
            }

            
        }

        /// <summary>
        /// 商家让会员扫码支付积分的二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Qrcode() {
            ComCloudShop.Service.Manger model = null;
            if (Session[AppConstant.weixinAdminuser] != null)
            {
                model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;

            }
            else
            {
                return RedirectToAction("Login", "Business");
            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PushGit() {
            ComCloudShop.Service.Manger modelS = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;
            int ID = Convert.ToInt32(Request["ID"]);
            int MangerID = modelS.ID;
            ViewData["MangerID"] = MangerID;
            try
            {
                using (var db = new MircoShopEntities())
                {
                    GiftViewModel model = (from a in db.GiftLists
                                                join
                                                    b in db.ProductImgs on a.ProductID equals b.ProductId.ToString()
                                                join
                                                    e in db.Members on a.MemberID equals e.MemberId.ToString()
                                                join
                                                    f in db.Products on a.ProductID equals f.ProductId.ToString()
                                                where a.ID == ID
                                                select new GiftViewModel
                                                {
                                                    ID = a.ID,
                                                    SPMC = f.SPMC,
                                                    P1 = b.P1,
                                                    MemberID = a.MemberID,
                                                    ProductID = a.ProductID,
                                                    AddTime = (DateTime)a.AddTime,
                                                    OrderID = a.OrderID,
                                                    ManagerID = a.ManagerID,
                                                    State = (int)a.State,
                                                    NickName = e.NickName
                                                }).FirstOrDefault();
                    return View(model);
                }
            }
            catch
            {
                return View();
            }
        }


        public string DuiHuan() {
            int ID = Convert.ToInt32(Request["ID"]);
            string MangerID = Request["MangerID"];
            ComCloudShop.Service.GiftList model = db.GiftLists.Where(d => d.ID == ID).FirstOrDefault();
            model.ManagerID = MangerID;
            model.State = 1;
            model.LQTime = DateTime.Now;
            db.Entry<ComCloudShop.Service.GiftList>(model).State = EntityState.Modified;
            if (db.SaveChanges() > 0) {
                return "Ok";
            }
            return "err";

        }

        /// <summary>
        /// 商家提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Withd() {
            ComCloudShop.Service.Manger model = null;
            if (Session[AppConstant.weixinAdminuser] != null)
            {
                model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;
                List<ComCloudShop.Service.Withdrawal> list= db.Withdrawals.Where(x => x.MemberID == model.ID).OrderByDescending(x => x.AddTime).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Business");
            }
        }

        /// <summary>
        /// 添加提现
        /// </summary>
        /// <returns></returns>
        public ActionResult AddWithd() {
            ComCloudShop.Service.Manger model = null;
            if (Session[AppConstant.weixinAdminuser] != null)
            {
                model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;
                model = db.Mangers.Where(d => d.ID == model.ID).FirstOrDefault();
                 return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Business");
            }
        }


        public ActionResult Exit() {
            Session[AppConstant.weixinAdminuser] = null;
            return Content("ok");
        }

        [HttpPost]
        public ContentResult AddWithdW()
        {

            try
            {
                ComCloudShop.Service.Manger model = Session[AppConstant.weixinAdminuser] as ComCloudShop.Service.Manger;
                ComCloudShop.Layer.UserService user = new Layer.UserService();
                Withdrawal w = new Withdrawal();
                decimal p = Convert.ToDecimal(Request["Price"]);
                w.MemberID = Convert.ToInt32(Request["MemberID"]);
                w.Type = Request["Type"].ToString();
                w.Number = Request["Number"].ToString();
                w.Price = p * model.Proportion;
                w.AddTime = DateTime.Now;
                w.State = 0;
                w.DZTime = DateTime.Now;
                w.Remack = Request["Remack"].ToString();
                w.TrueName = Request["TrueName"].ToString();
                w.Bank = Request["Bank"];
                w.Phone = Request["Phone"];
                w.Types = Convert.ToInt32(Request["Types"]);
                if (user.AddWithd(w))
                {
                    if (Session[AppConstant.weixinAdminuser] != null)
                    {

                        model = db.Mangers.Where(d => d.ID == model.ID).FirstOrDefault();
                        model.balance = Convert.ToDecimal(model.balance) - Convert.ToDecimal(w.Price);
                        db.Entry<ComCloudShop.Service.Manger>(model).State = EntityState.Modified;
                        db.SaveChanges();
                        return Content("ok");
                    }
                  
                  
                   
                }
                return Content("err");
            }
            catch (Exception ex)
            {

                return Content("err" + ex.ToString());
            }
        }

        public ActionResult Reg() {

            return View();
        }

        public ActionResult Regs() {
            try
            {
                ComCloudShop.Service.Manger model = new Manger();
                string license = Request["license"];
                string Storefront = Request["Storefront"];

                WeixinOauth.WeixinOauthHelper w = new WeixinOauth.WeixinOauthHelper(WeiXinConst.AppId, WeiXinConst.AppSecret);


                string ShopName = Request["ShopName"];
                string Business = Request["Business"];
                string Introduce = Request["Introduce"];
                string ShopAddress = Request["ShopAddress"];
                string Contacts = Request["Contacts"];
                string Phone = Request["Phone"];
                string UserName = Request["UserName"];
                string Pwd = Request["Pwd"];
                string lng = Request["lng"];
                string lat = Request["lat"];
                var list = db.Mangers.Where(d => d.UserName == UserName);
                if (list.Count() > 0)
                {
                    return Content("该登录名已存在!");
                }
                else
                {
                    model.UserName = UserName;
                    model.Pwd = Pwd;
                    model.Category = "";
                    model.ShopName = ShopName;
                    model.ShopAddress = ShopAddress;
                    model.Lng = lng;
                    model.Lat = lat;
                    model.Proportion = 1;
                    model.IsRecommend = 0;
                    model.balance = 0;
                    model.Contacts = Contacts;
                    model.license = w.GetImgs(license, false);
                    model.Storefront = w.GetImgs(Storefront, false);
                    model.Phone = Phone;
                    model.Business = Business;
                    model.Introduce = Introduce;
                    model.State = 0;
                    db.Mangers.Add(model);
                    if (db.SaveChanges() > 0)
                    {
                        return Content("ok");
                    }
                    return Content("申请报错");
                }
            }
            catch (Exception ex)
            {

                return Content(ex.ToString());
            }

        }
    }
}