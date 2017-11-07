using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Utility;
using System.IO;
using ComCloudShop.WeixinOauth;
using ComCloudShop.Service;
using System.Text;
using System.Data.Entity;

namespace ComCloudShop.Web.Controllers
{
    public class UserController : BaseController
    {

        // GET: Business
        [HttpGet]
        public ActionResult Reg()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Regs()
        {
            return null;
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
            var list = db.Members.Where(d => d.Mobile == username && d.QQ == Pwd);
            if (list.Count() > 0)
            {
                ComCloudShop.Service.Member models = list.FirstOrDefault();
                //db.Entry<ComCloudShop.Service.Manger>(models).State = EntityState.Modified;
                //db.SaveChanges();
                WeixinOauthUserInfo modeluser = new WeixinOauthUserInfo();
                modeluser.Id = models.MemberId;
                modeluser.nickname = models.NickName;
                modeluser.headimgurl = models.HeadImgUrl;
                modeluser.openid = models.OpenId;
                modeluser.Phone = models.Mobile;
                modeluser.province = models.Province;
                modeluser.city = models.City;
                modeluser.country = models.Country;
                modeluser.sex = models.Gender;
                Session[AppConstant.weixinuser] = modeluser;

                result.result = "登录成功";
                result.error = 0;
                result.msg = "登录成功";
            }
            else
            {
                result.error = 1;
                result.msg = "用户名密码错误";
            }
            return Json(result);
        }
        UserService _service = new UserService();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult RenderImg() {
            UserService user = new UserService();
            Member m = user.GetMemberByOpenID(UserInfo.openid);
            if (Convert.ToInt32(m.Cashbalance) > 0)
            {
                if (string.IsNullOrEmpty(m.OrignKey))
                {
                    string url = WeixinOauthHelper.CreateQrCode(m);
                    m.OrignKey = url;
                    logger.Debug("erweima=" + m.OrignKey);
                    user.UpdateMember(m);
                }
            }
            return View(m);
        }

        ///// <summary>
        ///// 接收请求
        ///// </summary>
        //public ActionResult RendMsg() {
        //    if (Request.HttpMethod.ToUpper() == "GET")
        //    {
        //        string signature = Request.QueryString["signature"];
        //        string timestamp = Request.QueryString["timestamp"];
        //        string nonce = Request.QueryString["nonce"];
        //        string echostr = Request.QueryString["echostr"];
        //        string str = WeixinOauthHelper.Auth(signature, timestamp, nonce, echostr);
        //        return Content(str);
        //    }
        //    else {
        //        Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
        //        byte[] requestByte = new byte[requestStream.Length];
        //        requestStream.Read(requestByte, 0, (int)requestStream.Length);
        //        string requestStr = Encoding.UTF8.GetString(requestByte);
        //        Dictionary<string, string> dic = new Dictionary<string, string>();
        //        if (!string.IsNullOrEmpty(requestStr))
        //        {
        //            return Content(WeixinOauthHelper.MakeRequest(requestStr));
        //        }
        //    }
        //    return Content("");
        //}

        //public void UpdateJF() {
        //    string jf = Request.QueryString["dot"];
        //    var modejf = new MeIntegralViewModel();
        //    modejf.OpenId = UserInfo.openid;
        //    modejf.NickName = UserInfo.nickname;
        //    modejf.JiFen = jf;
        //    var jfservice = new MeIntegralService();
        //    jfservice.Update(modejf);
        //}


        //public ActionResult GetBL()
        //{
        //    SetSupService s = new SetSupService();
        //    SetSupViewModel model = s.Read1();
        //    if (model == null)
        //    {
        //        model = new SetSupViewModel();
        //        model.SetBL = "0";
        //    }
        //    return Content(model.SetBL);
        //}

        public void AddCommission1(int memberID)
        {
            List<Member> list = user.GetMemberFollow(memberID);
            if (list.Count>0)
            {
                list1 = list;
                foreach (Member item in list)
                {
                    AddCommission2(item.MemberId);
                }
            }
        }
        public void AddCommission2(int memberID)
        {
            List<Member> list = user.GetMemberFollow(memberID);
            if (list.Count > 0)
            {
                list2.AddRange(list);
                foreach (Member item in list)
                {
                    AddCommission3(item.MemberId);
                }
            }
        }
        public void AddCommission3(int memberID)
        {
            List<Member> list = user.GetMemberFollow(memberID);
            if (list.Count > 0)
            {
                list3.AddRange(list);
            }
        }
        int j = 0;
        Member qjuser = null;
        UserService user = new UserService();
        List<Member> list1 = new List<Member>();
        List<Member> list2 = new List<Member>();
        List<Member> list3 = new List<Member>();
        public ActionResult LineMember() {
            
            Member m = user.GetMemberByOpenID(UserInfo.openid);
            AddCommission1(Convert.ToInt32(m.MemberId));
            string type = Request["type"];
            if (type== "1") {
                ViewData["list"] = list1;
            }
            else if (type == "2")
            {
                ViewData["list"] = list2;
            }else if (type == "3")
            {
                ViewData["list"] = list3;
            }

            return View(m);
        }

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddWithd()
        {
            UserService user = new UserService();
            Member m = user.GetMemberByOpenID(UserInfo.openid);
            return View(m);
        }


        public ActionResult BuyCard() {
            UserService user = new UserService();
            Member m = user.GetMemberByOpenID(UserInfo.openid);
            return View(m);
        }


        
        [HttpPost]
        public ContentResult AddIntList()
        {
            try
            {
                UserService user = new UserService();
                IntList w = new IntList();
                w.MemberID = Request["MemberID"];
                w.ManagerID = Request["ManageID"];
                w.Price = Convert.ToDecimal(Request["Price"]);
                w.State = Convert.ToInt32(Request["State"]);
                w.AddTime = DateTime.Now;
                if (user.AddIntList(w))
                {
                    Member m = user.GetMemberBID(Convert.ToInt32(w.MemberID));
                    m.integral = Convert.ToDecimal(m.integral) - Convert.ToDecimal(w.Price);
                    user.UpdateMember(m);

                    using (var db = new MircoShopEntities())
                    {
                        int mid = Convert.ToInt32(w.ManagerID);
                        Manger manger = db.Mangers.Where(d => d.ID == mid).FirstOrDefault();
                        manger.balance += w.Price;
                        db.Entry<ComCloudShop.Service.Manger>(manger).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Content("ok");
                }
                else {
                    return Content("err");
                }
            }
            catch (Exception ex)
            {

                return Content("err" + ex.ToString());
            }
        }
        [HttpPost]
        public ContentResult AddWithdW()
        {

            try
            {
                UserService user = new UserService();
                Withdrawal w = new Withdrawal();
                w.MemberID = Convert.ToInt32(Request["MemberID"]);
                w.Type = Request["Type"].ToString();
                w.Number = Request["Number"].ToString();
                w.Price = Convert.ToDecimal(Request["Price"]);
                w.AddTime = DateTime.Now;
                w.State = 0;
                w.DZTime = DateTime.Now;
                w.Remack =Request["Remack"].ToString();
                w.TrueName = Request["TrueName"].ToString();
                w.Bank = Request["Bank"];
                w.Phone = Request["Phone"];
                w.Types = Convert.ToInt32(Request["Types"]);
                if (user.AddWithd(w))
                {
                    Member m = user.GetMemberBID(Convert.ToInt32(w.MemberID));
                    m.balance = (Convert.ToDecimal(m.balance) - Convert.ToDecimal(w.Price)).ToString();
                    m.Cashbalance = (Convert.ToDecimal(m.Cashbalance) - Convert.ToDecimal(w.Price)).ToString();
                    user.UpdateMember(m);
                    return Content("ok");
                }
                return Content("err");
            }
            catch (Exception ex)
            {

                return Content("err"+ex.ToString());
            }
        }

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Withd() {
            UserService user = new UserService();
            List<Withdrawal> listwi = user.GetUserWithdrawalList(UserInfo.Id);
            return View(listwi);
        }
        
        public JsonResult GetMemberInfo()
        {
            var model = new ResultViewModel<Member>();
            model.error = 1;
            try
            {
                UserService user = new UserService();
                Member memeber = user.GetMemberByOpenID(UserInfo.openid);
                model.error = 1;
                model.msg = "加载成功";
                model.result = memeber;
            }
            catch (Exception ex)
            {
                model.error = 2;
                model.msg = ex.Message;
            }
            return Json(model);
        }

        public ActionResult AddVip() {
            UserService user = new UserService();
            Member m = user.GetMemberByOpenID(UserInfo.openid);
            return View(m);
        }

        // GET: User
        public ActionResult Index()
        {
            try
            {
                var model = new UserIndexViewModel();
                UserService user = new UserService();
                Member m = user.GetMemberByOpenID(UserInfo.openid);
                
                model.username = UserInfo.nickname;
                model.url = UserInfo.headimgurl;
                model.openid = UserInfo.openid;
                model.phone = m.Mobile;
                model.balance = m.balance;
                model.follow = m.follow;
                model.Cashbalance = m.Cashbalance;
                model.TotalIn = Convert.ToDecimal(m.TotalIn);
                model.integral = Convert.ToDecimal(m.integral);
                var _oservice = new OrderService(); 
                

                model.number = _oservice.GetSaveMoney(UserInfo.Id);
                AddCommission1(Convert.ToInt32(m.MemberId));
                j = 0;
                ViewData["list1"] = list1;
                ViewData["list2"] = list2;
                ViewData["list3"] = list3;
                return View(model);
            }
            catch(Exception e)
            {
                WriteTokenToTxt(e.ToString());
                return Error();
            }
        }

        private void WriteTokenToTxt(string token)
        {
            try
            {
                FileStream fs = new FileStream(@"D://1.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write("");
                sw.Write(token + "\r\n");
                sw.Write(DateTime.Now.ToString());
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 编辑用户个人资料
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit()
        {
            try
            {
                var data = await _service.GetUserInfo(UserInfo.Id);
                return View(data);
            }
            catch
            {
                return Error();
            }
        }
        /// <summary>
        /// 更新VIP信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UpdateVip() {
            if (_service.UpdateVip(UserInfo.Id)) {
                return Content("ok");
            }
            return Content("err");
        }

        /// <summary>
        /// 提交个人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(UserEditInfoViewModel model)
        {
            try
            {
                if (_service.Edit(model))
                {
                    return RedirectToAction("Index", "User");
                }
                return Error();
            }
            catch
            {
                return Error();
            }
        }

        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public ActionResult Order(int c=0)
        {
            try
            {
                var _oservice = new OrderService();
                var list = _oservice.GetOrderPageWithUser(UserInfo.Id,c);
                return View(list);
            }
            catch
            {
                return Error();
            }
        }

        public ActionResult MyGifList() {
            Member m = user.GetMemberByOpenID(UserInfo.openid);
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
                                                where a.MemberID == m.MemberId.ToString()
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
                                                }).ToList();
                    return View(list);
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }


        public ActionResult GifQrcode() {
            ViewData["GifID"] = Request["GID"];
            return View();
        }

        /// <summary>
        /// 会员扫商家码 支付页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Qrcode()
        {
            UserService user = new UserService();
            Member m = user.GetMemberByOpenID(UserInfo.openid);
            
            ViewData["MemberID"] = m.MemberId;
            ViewData["ManageID"] = Request["MID"];
            return View(m);
        }

    }
}