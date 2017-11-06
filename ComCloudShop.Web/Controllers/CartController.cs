using ComCloudShop.Layer;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using ComCloudShop.WeixinOauth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartService _service = new CartService();

        public ActionResult Index()
        {
            var list = _service.getPageList(UserInfo.Id, 1, 20);

            //foreach (ComCloudShop.ViewModel.CartDisplayModel item in list)
            //{
            //    if (item.Sale <= 0)
            //    {
            //        int cid = item.CartId;
            //        _service.Delete(cid);

            //    }
            //}
            //list = _service.getPageList(UserInfo.Id, 1, 20);
            return View(list);
        }



        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <param name="number">商品数量</param>
        /// <param name="flag">1：加入购物车；2：购买；</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(int pid, int number,int flag)
        {
            var result = new ResultViewModel<int>();
            try
            {
                var user = this.Session[AppConstant.weixinuser] as WeixinOauthUserInfo;

                var data = new CartViewModel();
                var list =new List<CartProductViewModel>();

                data.MemberId = user.Id;

                var model = new CartProductViewModel();
                model.ProductId = pid;
                model.BuyNum = number;
                list.Add(model);
                data.CartProducts = list;

                if (_service.Add(data))
                {
                    result.error = 0;
                    result.msg = "success";
                    result.total = 1;
                    if (flag == 1)
                    {
                        result.result = number;
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
        /// 获取购物车商品数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCartNumber()
        {
            try
            {
                var num = _service.GetCartNunber(UserInfo.Id);
                return Json(num, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 更新购物车数量变化
        /// </summary>
        /// <param name="cid">购物车id</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(int cid, int number) 
        {
            var result = new ResultViewModel<int>();
            try
            {
                if (_service.Update(cid, number))
                {
                    result.error = 0;
                    result.msg = "success";
                    result.total = 1;
                    result.result = number;
                }
                else
                {
                    result.error = 1;
                    result.msg = "faild";
                    result.total = 1;
                    result.result = number;
                }
            }
            catch(Exception ex)
            {
                result.error = 2;
                result.msg = ex.Message;
            }
            return Json(result);
        }


        [HttpPost]
        public JsonResult Delete(int[] cid)
        {
            var result = new ResultViewModel<int>();
            try
            {
                if (_service.Delete(cid))
                {
                    result.error = 0;
                    result.msg = "success";
                    result.total = 1;
                }
                else
                {
                    result.error = 1;
                    result.msg = "faild";
                    result.total = 1;
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
        /// 设置orderSession
        /// </summary>
        /// <param name="cid">orde列表</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Submit(int[] cid)
        {
            try
            {
                var session = new OrderParmModel();
                List<ComCloudShop.ViewModel.CartDisplayModel> list = _service.getPageList(UserInfo.Id, 1, 20).ToList();

                int[] cids =new int[list.Count];
                for (int i = 0; i < cids.Length; i++)
                {
                    cids[i] = list[i].CartId;
                }


                session.list = cids;
                Session[AppConstant.orderparm] = session;
                return Json(0);
            }
            catch (Exception ex)
            {
                return Json(2);
            }
        }

        /// <summary>
        /// 获取促销列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRuleList() 
        {
            var result = new ResultViewModel<IEnumerable<RuleViewModel>>();
            try
            {
                var _rservice = new RuleService();
                var data = _rservice.GetRulesList();
                result.result = new List<RuleViewModel>();
                result.result = data;
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

	}
}