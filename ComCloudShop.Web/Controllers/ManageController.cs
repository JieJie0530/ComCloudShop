using ComCloudShop.WeixinOauth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Web.Controllers
{
    public class ManageController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 接收请求
        /// </summary>
        public ActionResult RendMsg()
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                string signature = Request.QueryString["signature"];
                string timestamp = Request.QueryString["timestamp"];
                string nonce = Request.QueryString["nonce"];
                string echostr = Request.QueryString["echostr"];
                string str = WeixinOauthHelper.Auth(signature, timestamp, nonce, echostr);
                return Content(str);
            }
            else
            {
                Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
                byte[] requestByte = new byte[requestStream.Length];
                requestStream.Read(requestByte, 0, (int)requestStream.Length);
                string requestStr = Encoding.UTF8.GetString(requestByte);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(requestStr))
                {
                    logger.Debug("requestStr=" + requestStr);
                    string respent = WeixinOauthHelper.MakeRequest(requestStr);
                    logger.Debug("respent=" + respent);
                    return Content(respent);
                }
            }
            return Content("");
        }
    }
}