using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.WeixinOauth;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using System.Web.Security;

namespace ComCloudShop.Web.Controllers
{
    public class BaseController : Controller
    {
        protected void AddUserAuth(WeixinOauthUserInfo user)
        {
            this.Session[AppConstant.weixinuser] = user;
        }

        protected WeixinOauthUserInfo UserInfo
        {
            get
            {
                if (this.Session[AppConstant.weixinuser] == null)
                {
                    return null;
                }
                return this.Session[AppConstant.weixinuser] as WeixinOauthUserInfo;
            }
        }

        protected bool IsLogin
        {
            get
            {
                if (this.Session[AppConstant.weixinuser] == null)
                {
                    return false;
                }
                return true;
            }
        }

        protected ActionResult Error()
        {
            return RedirectToAction("Index", "Error");
        }
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string query = Request.Url.Query.ToString();
            if (!IsLogin)
            {
                if (filterContext.ActionDescriptor.ActionName == "Login" || filterContext.ActionDescriptor.ActionName == "Reg" || filterContext.ActionDescriptor.ActionName == "Regs" || filterContext.ActionDescriptor.ActionName == "Logins")
                {
                  
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/Authorize/Index/"+query+"");
                }

            }
          
          
          
            //ShareViewModel model = new ShareViewModel();
            //model.timestamp = WeixinOauthHelper.GenerateTimeStamp();
            //model.nonceStr = WeixinOauthHelper.GenerateNonceStr();
            //string url = "http://" + Request.Url.Host + Request.Url.LocalPath + query.Replace("#", "");//不能要
            //string str = "jsapi_ticket=" + WeixinOauthHelper.GetJsapiTicket() + "&noncestr=" + model.nonceStr + "&timestamp=" + model.timestamp + "&url=" + url;
            //model.signature = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            //model.Url = url;
            //model.MemberID = UserInfo.Id.ToString();
            //ViewData["ShareViewModel"] = model;

            
        }
    }
}