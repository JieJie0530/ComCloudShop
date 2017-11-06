using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;
using ComCloudShop.WeixinOauth;
using System.IO;
using System.Data;

namespace ComCloudShop.Backend.Controllers
{
    public class OrderController : BaseController
    {
        public ContentResult WithdOk(int id)
        {
            if (_service.UpdateWithd(id))
            {
                return Content("ok");
            }
            return Content("err");
        }
        /// <summary>
        /// 获取用户退货列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult listR(string nickName, int State, int page = 1)
        {

            var list = _service.GetMemberReturnListNew(page, AppConstant.PageSize, nickName, State);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RList() {

            return View();
        }
        //listr
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View = "~/Views/Shared/Error.cshtml")]
        public ActionResult Browse(HttpPostedFileBase file)
        {
            string pid = Request["ProductId"];
            if (".xlsx" == Path.GetExtension(file.FileName) || ".xls" == Path.GetExtension(file.FileName))
            {
               
            }
            else {
                throw new ArgumentException("当前文件格式不正确,请确保正确的Excel文件格式!");
            }

            var severPath = this.Server.MapPath("/files/"); //获取当前虚拟文件路径

            var savePath = Path.Combine(severPath, file.FileName); //拼接保存文件路径
            List<ComCloudShop.Service.OrderComment> list = new List<ComCloudShop.Service.OrderComment>();
            try
            {
                file.SaveAs(savePath);
                DataTable dt = ComCloudShop.Utility.Helper.ExcelHelper.ReadExcelToEntityList<ComCloudShop.Service.OrderComment>(savePath, pid);
                if (dt == null) return null;


                Random r = new Random();
                //遍历DataTable中所有的数据行 
                foreach (DataRow dr in dt.Rows)
                {

                    ComCloudShop.Service.OrderComment model = new ComCloudShop.Service.OrderComment();
                    model.AddTime = DateTime.Now.AddMonths(r.Next(-10, -1)).AddHours(r.Next(1, 12)).AddMinutes(r.Next(1, 60)).AddSeconds(r.Next(1, 60));
                    model.Pics = "";
                    model.ProductID = Convert.ToInt32(pid);
                    model.Contents = dr[1].ToString();
                    model.UserName = dr[0].ToString();
                    model.ProductScore = Convert.ToInt32(dr[2].ToString());
                    model.ProductPackaging = 0;
                    model.DeliverySpeed = 0;
                    model.Remack = "";
                    model.OrderNum = "";
                    model.MemberID = 0;
                    //对象添加到泛型集合中 
                    list.Add(model);
                }

            }
            finally
            {
                System.IO.File.Delete(savePath);//每次上传完毕删除文件
            }
            
            if (_service.Adds(list))
            {
                return Content("ok");
            }
            return Content("err");
        }

        protected OrderService _service = new OrderService();

        public ActionResult Index()
        {
            //ViewData["page"] = _service.GetOrderPageCount(AppConstant.PageSize);
            return View();
        }


        public ActionResult Index1()
        {
            //ViewData["page"] = _service.GetOrderPageCount(AppConstant.PageSize);
            return View();
        }


        public ActionResult OkTuiKuan() {
            string OrderNum = Request["OrderNum"].ToString();

            ComCloudShop.Service.OrderDetail modelo = _service.GetOrderDetail(OrderNum);
             
            ComCloudShop.WeixinOauth.WeixinOauthHelper w = new WeixinOauth.WeixinOauthHelper(WeiXinConst.AppId, WeiXinConst.AppSecret);

            string s= w.Refund(modelo);
            if (s == "SUCCESS") {
                _service.UpdateOrderReturnOk(modelo.OrderNum);
            }
            return Content(s);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="data">查询参数</param>
        /// <returns></returns>
        public JsonResult List(AdminOrderSearchViewModel data) 
        {
            data.size = AppConstant.PageSize;
            var list = _service.AdminGetOrderList(data);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <returns></returns>
        public JsonResult Detail(string OrderNum)
        {
            return Json(_service.AdminGetOrderDetail(OrderNum), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置订单快递信息
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="ExpressNum">快递单号</param>
        /// <param name="IsReceive">0：配送中；1：已签收；</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetOrderExpressInfo(string OrderNum, string ExpressNum, int IsReceive)
        {

            ComCloudShop.Service.OrderDetail o= _service.GetOrderDetail(OrderNum);
            ComCloudShop.Layer.MemberService _user = new MemberService();
            ComCloudShop.Service.Member member= _user.GetMemberByMemberID(o.MemberId);
             WeixinOauthHelper.TuiSong(member.OpenId, "您的订单号"+OrderNum+"已发货，快递单号"+ExpressNum+"请注意查收！");
            return Json(_service.AdminSetOrderExpressInfo(OrderNum, ExpressNum, IsReceive), JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Detail(string n) 
        //{
        //    var query = _service.GetOrderDetailByNo(n);
        //    return View(query);
        //}

        //[HttpPost]
        //public JsonResult Delete(string n) 
        //{
        //    var b = _service.Delete(n);
        //    return Json(b, JsonRequestBehavior.AllowGet);
        //}






        /// <summary>
        /// erp订单
        /// </summary>
        /// <returns></returns>
        public ActionResult ErpOrder()
        {
            return View();
        }
        /// <summary>
        /// erp订单列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sqlSearch"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetErpOrderList(int pageSize, int page = 1, string sqlSearch = "")
        {
            var result = _service.GetErpOrderList(page, pageSize, sqlSearch);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// erp订单详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetErpOrder(int id=1)
        {
            var model = _service.GetErpOrder(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 重新发送erp订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="receiver_state"></param>
        /// <param name="receiver_city"></param>
        /// <param name="receiver_district"></param>
        /// <param name="receiver_address"></param>
        /// <returns></returns>
        public JsonResult ReSendErpOrder(int id,string receiver_state, string receiver_city, string receiver_district, string receiver_address)
        {
            var model = _service.GetErpOrder(id).list; ;
            model.receiver_state = receiver_state;
            model.receiver_city = receiver_city;
            model.receiver_district = receiver_district;
            model.receiver_address = receiver_address;
            var result =ThirdPartyPackage.ReSentErpOrder(model);
            if (result)
            {
                _service.UpdateErpOrder(model);
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}
