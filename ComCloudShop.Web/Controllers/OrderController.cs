using ComCloudShop.Layer;
using ComCloudShop.Service;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using ComCloudShop.Web.Third;
using ComCloudShop.WeixinOauth;
using ComCloudShop.WeixinOauth.Pay.Helper;
using ComCloudShop.WeixinOauth.Pay.Models;
using ComCloudShop.WeixinOauth.Pay.Models.UnifiedMessage;
using ComCloudShop.WeixinOauth.Pay.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;

namespace ComCloudShop.Web.Controllers
{
    public class OrderController : BaseController
    {
        OrderService _service = new OrderService();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 订单列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            try
            {
                //var data = _service.GetOrderDetailList(UserInfo.Id);
                return View();
            }
            catch
            {
                return Error();
            }
        }

        /// <summary>
        /// 订单详情页
        /// </summary>
        /// <param name="ordernum">订单号</param>
        /// <returns></returns>
        public ActionResult Detail(string ordernum)
        {
            try
            {
                var data = _service.GetOrderDetailByNO(ordernum);
                return View(data);
            }
            catch
            {
                return Error();
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="no">订单号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OrderCancel(string no)
        {
            try
            {
                
                if (_service.Delete(no, UserInfo.Id))
                    return Json(0);
                return Json(1);
            }
            catch
            {
                return Json(1);
            }
        }

        /// <summary>
        /// 订单详情页
        /// </summary>
        /// <param name="ordernum">订单号</param>
        /// <returns></returns>
        public ActionResult DetailPay(string ordernum)
        {
            try
            {
                var data = _service.GetOrderDetailByNO(ordernum);
                return View(data);
            }
            catch
            {
                return Error();
            }
        }

        [HttpPost]
        public ActionResult AddComment(OrderComment model) {

            ComCloudShop.Layer.UserService _user = new UserService();
            Member m = _user.GetMemberBID(Convert.ToInt32(model.MemberID));
            model.UserName = m.UserName;
            if (_service.Add(model)) {
                return Content("ok");
            }
            return Content("err");
        }

        public ActionResult AddReturn1()
        {
          
            string OrderNum = Request["OrderNum"].ToString();

            if (_service.UpdateOrderReturn(OrderNum))
            {
                return Content("ok");
            }
            else {
                return Content("err");
            } 
          
        }

        public ActionResult AddReturn() {
            OrderReturn o = new OrderReturn();
            o.MemberID = Convert.ToInt32(Request["MemberID"]);
            o.OrderNum = Request["OrderNum"].ToString();
            o.AddTime = DateTime.Now;
            o.Reason = Request["Reason"].ToString();
            o.ProductID = Convert.ToInt32(Request["ProductID"]);
            o.Phone = Request["Phone"].ToString();
            o.Bank = Request["Bank"].ToString();
            o.BankNumber = Request["Number"].ToString();
            o.UserName = Request["TrueName"].ToString();
            o.Statu = 0;
            if (_service.AddReturn(o))
            {
                //_service.UpdateOrderReturn(o.OrderNum);
                return Content("ok");
            }
            return Content("err");
        }

        public ActionResult ReturnOrder()
        {

            try
            {
                string OrderNum = Request["OrderNum"].ToString();
                var data = _service.GetOrderDetailByNO(OrderNum);
                ViewData["MemberID"] = UserInfo.Id;
                ViewData["ProductID"] = Request["ProductID"].ToString();
                return View(data);
            }
            catch
            {
                return Error();
            }
        }

        /// <summary>
        /// 订单页面
        /// </summary>
        /// <param name="aid">地址id</param>
        /// <param name="cid">优惠券id</param>
        /// <returns></returns>
        public ActionResult Confim() 
        {
            try
            {
                var data = Session[AppConstant.orderparm] as OrderParmModel;
                if (data.list.Length > 0)
                {
                    var model = _service.GetOrderConfig(data.list, UserInfo.Id, UserInfo.openid, data.aid, data.cid);
                    if (data.cid != model.coupon.CouponId )
                    {
                        data.cid = model.coupon.CouponId;
                    }
                    if(data.aid != model.address.AddressId)
                    {
                        data.aid = model.address.AddressId;
                    }

                    Session[AppConstant.orderparm] = data;
                    return View(model);
                }
                else
                {
                    return Error();
                }
            }
            catch
            {
                return Error();
            }

            //try
            //{
            //    var list = Session[AppConstant.cartlist] as int[];
            //    if (list.Length > 0)
            //    {

            //        var user = this.Session[AppConstant.weixinuser] as WeixinOauthUserInfo;
            //        var data = _service.GetOrderConfig(list, user.Id, user.openid, (int)(aid.HasValue ? aid : 0), (int)(cid.HasValue ? cid : 0));

            //        return View(data);
            //    }
            //    else
            //    {
            //        return Error();
            //    }
            //}
            //catch
            //{
            //    return Error();
            //}
        }

        /// <summary>
        /// 提交订单事件
        /// </summary>
        /// <param name="aid">地址id</param>
        /// <param name="cid">卡券id</param>
        /// <param name="url">返回地址</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(string remark)
        {
            var model = new ResultViewModel<WeiXinPayResultModel>();
            model.error = 1;
            try
            {
                var parm = Session[AppConstant.orderparm] as OrderParmModel;
                if (parm.list.Length > 0)
                {
                    decimal sfprice = Convert.ToDecimal(Request["sfprice"]);

                    var data = new OrderAddParmModel();
                    data.CartList = parm.list;
                    data.AddressId = parm.aid;
                    data.CouponId = parm.cid;
                    data.MemberId = UserInfo.Id;
                    data.Remark = remark;
                    data.KuaiDi = WebConfigurationManager.AppSettings["KUAIDI"];
                    data.Carriage = decimal.Parse(Request["Carriage"]);
                    data.SPGG= Request["spgg"].ToString();
                    if (Request["Jifen"] == "")
                    {
                        data.Jifen = "0";
                    }
                    else {
                        data.Jifen = Request["Jifen"].ToString();
                    }
                    var order = _service.AddOrder(data);
                    if (order != null && !string.IsNullOrEmpty(order.OrderNum))
                    {

                        if (sfprice > 0)
                        {
                            var hostip = HttpContext.Request.UserHostAddress;
                            var location = Request.Url.ToString();
                            var pay = Pay(order.OrderNum, ((int)(order.TotalPrice * 100)).ToString(), location, "", hostip);
                            //var pay = Pay(order.OrderNum, ((int)(1)).ToString(), location, "", hostip);
                            if (pay != null)
                            {
                                model = pay;
                            }
                        }
                        else {
                            string[] session = { order.OrderNum, "" };
                            Session[AppConstant.payorder] = session;
                            model.error = 0;
                            model.msg = "success";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                model.error=2;
                model.msg=ex.Message;
            }
            return Json(model);
        }

        public ActionResult OrderComment() {
            int pid = Convert.ToInt32(Request["ProductID"]);
            ProductService _service = new ProductService();
            var data = _service.GetDetailById(pid);
            ViewData["MemberID"] = UserInfo.Id;
            ViewData["OrderNum"] = Request["OrderNum"].ToString();
            return View(data);
        }


        

        /// <summary>
        /// 获取微信pay_jsapi
        /// </summary>
        /// <param name="order_no">订单id</param>
        /// <param name="total_fee">总额</param>
        /// <param name="return_url">返回地址</param>
        /// <param name="prepayid"></param>
        /// <param name="hostip">终端ip</param>
        /// <returns></returns>
        public ResultViewModel< WeiXinPayResultModel> Pay(string order_no, string total_fee, string return_url, string prepayid,string hostip)
        {
            var outR= new ResultViewModel<WeiXinPayResultModel>();
            try
            {
                logger.Info("OrderNum " + order_no);

                //创建Model
                UnifiedWxPayModel data = UnifiedWxPayModel.CreateUnifiedModel(WeiXinConst.AppId, WeiXinConst.PartnerId, WeiXinConst.PartnerKey);

                if (string.IsNullOrEmpty(prepayid))
                {
                    //预支付
                    UnifiedPrePayMessage result = WeiXinPayHelper.UnifiedPrePay(data.CreatePrePayPackage(WeiXinConst.Customer, order_no, total_fee, hostip, return_url, UserInfo.openid));

                    if (result == null || !result.ReturnSuccess || !result.ResultSuccess || string.IsNullOrEmpty(result.Prepay_Id))
                    {
                        throw new Exception(result.Return_Msg);
                    }

                    //预支付订单
                    prepayid = result.Prepay_Id;

                    _service.UpdateOrderPrepayid(order_no, prepayid);
                }

                //JSAPI 支付参数的Model
                var model = new WeiXinPayResultModel() 
                {
                    AppId = data.AppId,
                    Package = string.Format("prepay_id={0}", prepayid),
                    Timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(),
                    Noncestr = CommonUtil.CreateNoncestr(),
                };

                Dictionary<string, string> nativeObj = new Dictionary<string, string>();
                nativeObj.Add("appId", model.AppId);
                nativeObj.Add("package", model.Package);
                nativeObj.Add("timeStamp", model.Timestamp);
                nativeObj.Add("nonceStr", model.Noncestr);
                nativeObj.Add("signType", model.SignType);
                model.PaySign = data.GetCftPackage(nativeObj); //生成JSAPI 支付签名

                outR.error = 0;
                outR.msg = "success";
                outR.result = new WeiXinPayResultModel();
                outR.result = model;

                string[] session = { order_no, prepayid };
                Session[AppConstant.payorder] = session;

            }
            catch(Exception ex)
            {
                outR.error = 2;
                outR.msg = ex.Message;
            }
            return outR;
        }


        /// <summary>
        /// 支付购物卡
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PayBuyCard()
        {
            var result = new ResultViewModel<WeiXinPayResultModel>();
            try
            {

                var hostip = HttpContext.Request.UserHostAddress;
                var location = HttpContext.Request.Url.ToString();
                var ordernum = DateTime.Now.ToString("yyyyMMddhhmmssfff");//订单ID

                int MemberID = Convert.ToInt32(Request["MemberID"]);
                ComCloudShop.Layer.RechargeService bll = new RechargeService();
                Service.Recharge model = new Recharge();
                model.MemberID = MemberID;
                model.OrderID = ordernum;
                model.AddTime = DateTime.Now;
                model.State = 0;
                bll.Add(model);

                //100000
                var pay = Pay(ordernum, "100000", "http://mm.hofaf.com/Default/Index", "", hostip);
                //var pay = Pay(ordernum, "1", "http://mm.hofaf.com/Default/Index", "", hostip);
                if (pay != null)
                {
                    result = pay;
                }
                else
                {
                    result.error = 1;
                    result.msg = "faild";
                }
            }
            catch (Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 支付VIP费用
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PayVip() {
            var result = new ResultViewModel<WeiXinPayResultModel>();
            try
            {
                var hostip = HttpContext.Request.UserHostAddress;
                var location = HttpContext.Request.Url.ToString();
                var ordernum = DateTime.Now.ToString("yyyyMMddhhmmssfff");//订单ID
                //10000000
                var pay = Pay(ordernum, "10000000", location, "", hostip);
                    if (pay != null)
                    {
                        result = pay;
                    }
                    else
                    {
                        result.error = 1;
                        result.msg = "faild";
                    }
            }
            catch (Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 未支付订单点击支付事件
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OrderPay(string no)
        {
            var result = new ResultViewModel<WeiXinPayResultModel>();
            try
            {

                var hostip = HttpContext.Request.UserHostAddress;
                var location = HttpContext.Request.Url.ToString();
                var ordernum = no;
                var prepayid = _service.GetOrderPrepayid(no);

                if (!string.IsNullOrEmpty(prepayid))
                {
                    var pay = Pay(ordernum,"", location, prepayid, hostip);
                    if (pay != null)
                    {
                        result = pay;
                    }
                    else
                    {
                        result.error = 1;
                        result.msg = "faild";
                    }
                }
                else
                {
                    result.error = 1;
                    result.msg = "faild";
                }
            }
            catch(Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 添加支付结果
        /// </summary>
        /// <param name="msg">支付结果</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveBrandWCPayRequest(string msg)
        {
            try
            {
                var pay = Session[AppConstant.payorder] as string[];

                logger.Info(pay[0] +" " + msg);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException.Message);
            }
            return Json("");
        }
        
        
        UserService _user = new UserService();
        OrderProductViewModel _opv = new OrderProductViewModel();


       

        /// <summary>
        /// 支付完成
        /// </summary>
        /// <returns></returns>
        public JsonResult PayOK()
        {
            try
            {
                var pay = Session[AppConstant.payorder] as string[];

                if (_service.UpdateOrderOrPayOK(pay[0], pay[1]))
                {
                   
                    var orderdetail = _service.GetOrderDetail(pay[0]);
                    if (orderdetail != null) {
                        if (orderdetail.Stutas == 1) {//如果已经支付
                            var user = _user.GetMemberBID(orderdetail.MemberId);
                            WeixinOauthHelper.TuiSong(user.OpenId, "您的订单号" + pay[0] + "已付款，感谢您对我们的支持！");
                            if (orderdetail.Jifen != null) {
                                if (orderdetail.Jifen != "") {
                                    if ( Convert.ToDecimal(orderdetail.Jifen)>0)
                                    {
                                        user.integral -= Convert.ToDecimal(orderdetail.Jifen);
                                        _user.UpdateMember(user);
                                    }
                                }
                            }

                            IEnumerable<OrderProductViewModel> list = _service.GetOrderProductDetail(pay[0]).ToList();
                            foreach (OrderProductViewModel item in list)
                            {
                                if (item.BuySale <= 0) {
                                    ComCloudShop.Service.GiftList g = new GiftList();
                                    g.MemberID = orderdetail.MemberId.ToString();
                                    g.ProductID = item.ProductId.ToString();
                                    g.AddTime = DateTime.Now;
                                    g.LQTime = null;
                                    g.OrderID = orderdetail.OrderNum;
                                    g.ManagerID = "";
                                    g.State = 0;
                                    _service.AddGif(g);
                                }
                            }
                             
                            //if (user.follow != "" && user.fsate == 1) {
                            //    user.fsate = 2;
                                
                            //    _user.UpdateMember(user); //确定上下级关系 

                            //}
                            
                        }
                    }


                    return Json(0, JsonRequestBehavior.AllowGet);

                    //添加到ERP
                    //var data = _service.GetERPOrderData(pay[0], pay[1]);
                    //var model = ThirdPartyPackage.AddOrderToERP(data);
                    //if (model == null)
                    //{
                    //    return Json(4);
                    //}
                    //if (string.IsNullOrEmpty(model.tid))
                    //{
                    //    return Json(3);
                    //}
                    //else
                    //{
                    //    //更新ERP订单号到本地数据库
                    //    _service.UpdateOrderSetERP(pay[0], model.tid);
                    //    return Json(0);
                    //}
                    return Json(1);
                }
                else
                {
                    return Json(1);
                }
            }
            catch
            {
                return Json(2);
            }
        }

        /// <summary>
        /// 获取用户未支付订单数
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrderToPay()
        {
            var result = new ResultViewModel<int>();
            try
            {
                var num = _service.GetOrderToPayNumber(UserInfo.Id, 0);
                result.result = num;
                result.error = 0;
                result.msg = "success";
            }
            catch(Exception ex)
            {
                 result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <param name="status">状态：0：全部；1：待支付；2：配送中；3：已完成</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPageList(int status, int page = 1, int size = 20)
        {
            try
            {
                var data = _service.GetOrderListPage(UserInfo.Id, status-1, page, size);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

	}
}