using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Utility;

namespace ComCloudShop.Backend.Controllers
{
    public class BaseController : Controller
    {
        //private readonly string admin = "AdminUser";

        protected void AddUserAuth(string username) 
        {
            this.Session[AppConstant.admin] = username;
        }

        protected void ClearUserAuth() 
        {
            this.Session[AppConstant.admin] = null;
            if (this.Session[AppConstant.admin] == null) 
            {
                Response.Redirect("~/account/login");
            }
        }

        protected string AdminUser
        {
            get 
            {
                if (this.Session[AppConstant.admin] == null)
                {
                    return "未知";
                }
                return this.Session[AppConstant.admin].ToString();
            } 
        }

        protected bool IsLogin
        {
            get
            {
                if (this.Session[AppConstant.admin] == null)
                {
                    return false;
                }
                return true;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsLogin && !(filterContext.ActionDescriptor.ActionName == "login"))
            {
                Response.Redirect("~/account/login");
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
