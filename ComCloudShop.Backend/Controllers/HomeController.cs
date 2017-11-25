using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Backend.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        static String product = "Dysmsapi";//短信API产品名称
        static String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
        static String accessId = "LTAIVz1ITzYLoYTk";
        static String accessSecret = "LLLeYswcxoYwdMsVyU9Vcb2yG8L47I";
        static String regionIdForPop = "cn-hangzhou";

        public ActionResult SendMsm()
        {
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
                return Content("err" + e.ToString());
            }
            catch (ClientException e)
            {
                return Content("err" + e.ToString());
            }
            return Content("ok");
        }
    }
}
