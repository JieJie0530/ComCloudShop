using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using ComCloudShop.WeixinOauth;
using ComCloudShop.Utility;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;

namespace ComCloudShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _service = new ProductService();

        static String product = "Dysmsapi";//短信API产品名称
        static String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
        static String accessId = "LTAIVz1ITzYLoYTk";
        static String accessSecret = "LLLeYswcxoYwdMsVyU9Vcb2yG8L47I";
        static String regionIdForPop = "cn-hangzhou";

        public ActionResult SendMsm() {
            string Phone = Request["mobile"].ToString();
            Random rad = new Random();
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {

                int mobile_code = rad.Next(1000, 10000);
                Session["mobile_code"] = mobile_code.ToString();
                request.PhoneNumbers = Phone;
                request.SignName = "心智通";
                request.TemplateCode = "SMS_110840176";
                request.TemplateParam = "{\"code\":\"" + mobile_code + "\"}";
                request.OutId = "xxxxxxxx";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                System.Console.WriteLine(sendSmsResponse.Message);

            }
            catch (ServerException e)
            {
                return Content("err");
            }
            catch (ClientException e)
            {
                return Content("err");
            }
            return Content("ok");
        }

        public void UpdateJf(string jf)
        {
            string js = jf;
        }

        ComCloudShop.Layer.CateService bllcate = new CateService();
        MircoShopEntities db = new MircoShopEntities();
        ComCloudShop.Layer.ProductService bllpro = new ProductService();
        public ActionResult Index()
        {
            try
            {
                var data = _service.GetProductGroupSaleAll();

                if (this.Session[AppConstant.weixinuser] == null)
                {
                    ViewBag.Rols = "0";
                }
                else {
                    WeixinOauthUserInfo user = this.Session[AppConstant.weixinuser] as WeixinOauthUserInfo;
                    Member m = db.Members.Where(d => d.MemberId == user.Id).FirstOrDefault();
                    if (m.ISVip == 0)
                    {
                        ViewBag.Rols = "0";
                    }
                    else {
                        ViewBag.Rols = "1";
                    }
                }

                BrandService bll = new BrandService();
                ViewBag.BrandList = bll.GetBrandList(1, 1, 10000000);
                CategoryService bllcate = new CategoryService();
                ViewBag.Cate = bllcate.GetCategoryList(1, 1, 10000000);

                ProductService bllps = new ProductService();
                ViewBag.Product = bllps.GetProductList1(1, 10);

                //ViewBag.CateList = db.MangerCates.OrderByDescending(d => d.Sort).ToList();
                return View(data);
            }
            catch(Exception e)
            {
                
                return null;
            }
        }
        public ActionResult ShopList()
        {
            string title = Request["title"];
            List<ComCloudShop.Service.Manger> list = new List<Manger>();
            using (var db = new MircoShopEntities())
            {
                list = db.Mangers.Where(d => d.Category.Contains(title) && d.State!=0).OrderBy(d => d.IsRecommend).ToList();
            }
            return View(list);
        }




    }
}