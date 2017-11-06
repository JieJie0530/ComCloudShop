﻿using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using ComCloudShop.WeixinOauth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ComCloudShop.Web.Controllers
{
    public class BaseMangeController : Controller
    {
        protected void AddUserAuth(WeixinOauthUserInfo user)
        {
            this.Session[AppConstant.weixinAdminuser] = user;
        }

        protected WeixinOauthUserInfo UserInfo
        {
            get
            {
                if (this.Session[AppConstant.weixinAdminuser] == null)
                {
                    return null;
                }
                return this.Session[AppConstant.weixinAdminuser] as WeixinOauthUserInfo;
            }
        }

        protected bool IsLogin
        {
            get
            {
                if (this.Session[AppConstant.weixinAdminuser] == null)
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

            string query = Request.Url.Query.ToString();
            if (!IsLogin)
            {
                if (filterContext.ActionDescriptor.ActionName != "Login" && filterContext.ActionDescriptor.ActionName != "Reg" && filterContext.ActionDescriptor.ActionName != "Regs")
                {
                    Response.Redirect(Url.Content("~/AuthorizeMange/" + query));
                }
                //RedirectToAction("Index", "authorize");
            }
            ShareViewModel model = new ShareViewModel();
            model.timestamp = WeixinOauthHelper.GenerateTimeStamp();
            model.nonceStr = WeixinOauthHelper.GenerateNonceStr();

            string url = "http://" + Request.Url.Host + Request.Url.LocalPath + query.Replace("#", "");//不能要

            string str = "jsapi_ticket=" + WeixinOauthHelper.GetJsapiTicket() + "&noncestr=" + model.nonceStr + "&timestamp=" + model.timestamp + "&url=" + url;
            model.signature = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            model.Url = url;
            ViewData["ShareViewModel"] = model;

            base.OnActionExecuting(filterContext);
        }
    }
}