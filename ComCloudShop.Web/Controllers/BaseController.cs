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
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using ComCloudShop.Service;

namespace ComCloudShop.Web.Controllers
{
    public class BaseController : Controller, System.Web.SessionState.IRequiresSessionState
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
                    //WeixinOauthUserInfo modeluser = new WeixinOauthUserInfo();
                    //modeluser.Id = 3681;
                    //modeluser.nickname = "胡锐杰";
                    //modeluser.headimgurl = "http://wx.qlogo.cn/mmopen/vi_32/DYAIOgq83erG7kFdtvhcXVWMFPCn6uC7PC4SK4caWIytThNEbtCKicwnb6B9OPtTJ170bYNpfYDVqakRFMeSPQw/0";
                    //modeluser.openid = "o5S56sw6JkhUtbJCiSYO_I7316Fs";
                    //modeluser.Phone = "18307285886";
                    //modeluser.province = "";
                    //modeluser.city = "";
                    //modeluser.country = "";
                    //modeluser.sex = 0;
                    //Session[AppConstant.weixinuser] = modeluser;
                    //return true;
                }
                return true;
            }
        }

        protected WeixinOauthUserInfo Oauth
        {
            get
            {
                if (this.Session[AppConstant.Oauth] != null)
                {
                   return HttpContext.Session[AppConstant.Oauth] as WeixinOauthUserInfo;
                }
                return null;
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
            string str = "false";
            if (Oauth == null) { str = "true"; }


            logger.Debug("Request.Url=" + Request.Url + "str=" + str);
            if (!IsLogin)
            {
                if (filterContext.ActionDescriptor.ActionName == "Login" || filterContext.ActionDescriptor.ActionName == "Reg" || filterContext.ActionDescriptor.ActionName == "Regs" || filterContext.ActionDescriptor.ActionName == "Logins" || filterContext.ActionDescriptor.ActionName == "Finds" || filterContext.ActionDescriptor.ActionName == "Find")
                {
                    if (filterContext.ActionDescriptor.ActionName!= "Regs" && filterContext.ActionDescriptor.ActionName!= "Logins" && filterContext.ActionDescriptor.ActionName != "Finds") {
                        var data = WeiXinWebAuthorize();
                    }
                    
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    var data = WeiXinWebAuthorize();
                    if (data.error == 0) {
                        base.OnActionExecuting(filterContext);
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Redirect("/User/Login/");
                    }
                }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }

        }

        MircoShopEntities db = new MircoShopEntities();
        /// <summary>
        /// 获取web授权数据
        /// </summary>
        /// <returns></returns>
        public ResultViewModel<OAuthAccessTokenResult> WeiXinWebAuthorize()
        {
            var result = new ResultViewModel<OAuthAccessTokenResult>();
           
                var appId = WeiXinConst.AppId;
                var appSecret = WeiXinConst.AppSecret;
                string code = Request["code"];
                string state = Request["state"];
                if (string.IsNullOrEmpty(state))
                {
                    //string url = "http://m.fingerlink.cn/fg/authorize/";
                    string url = Request.Url.ToString();
                    logger.Info("url " + url);
                    string UrlBase = OAuthApi.GetAuthorizeUrl(appId, url, "kevin", OAuthScope.snsapi_userinfo);
                    Response.Redirect(UrlBase);
                }
                else
                {
                    if (string.IsNullOrEmpty(code))
                    {
                        result.error = 1;
                        result.msg = "您拒绝了授权！";
                    }
                    else
                    {
                        if (state != "kevin")
                        {
                            //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                            //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                            result.error = 2;
                            result.msg = "验证失败！请从正规途径进入！";
                        }
                        else
                        {
                            if (this.Session[AppConstant.weixinuser] == null)
                            {
                                //通过，用code换取access_token
                                var data = OAuthApi.GetAccessToken(appId, appSecret, code);
                                string token = AccessTokenContainer.TryGetAccessToken(WeiXinConst.AppId, WeiXinConst.AppSecret, false);//如果没
                                //logger.Debug("access_token=" + data.access_token);
                               // logger.Debug("token1=" + token);
                                var data_user = OAuthApi.GetUserInfo(data.access_token, data.openid);
                                logger.Debug("data_user=" + data_user.openid + "," + data_user.nickname);
                                var model = new WeixinOauthUserInfo();
                                model.openid = data.openid;
                                model.nickname = data_user.nickname;
                                model.headimgurl = data_user.headimgurl;

                                var models = db.Members.Where(d => d.OpenId == data_user.openid).FirstOrDefault();
                                if (models == null)
                                {
                                    Session[AppConstant.Oauth] = model;
                                    result.error = 1;
                                }
                                else
                                {

                                    models.NickName = data_user.nickname;
                                    models.HeadImgUrl = data_user.headimgurl;
                                    db.SaveChanges();

                                    WeixinOauthUserInfo modeluser = new WeixinOauthUserInfo();
                                    modeluser.Id = models.MemberId;
                                    modeluser.nickname = data_user.nickname;
                                    modeluser.headimgurl = data_user.headimgurl;
                                    modeluser.openid = models.OpenId;
                                    modeluser.Phone = models.Mobile;
                                    modeluser.province = models.Province;
                                    modeluser.city = models.City;
                                    modeluser.country = models.Country;
                                    modeluser.sex = models.Gender;
                                    Session[AppConstant.weixinuser] = modeluser;



                                    result.error = 0;
                                    result.msg = "获取授权成功！";
                                }
                            }
                        }
                }
            }
            return result;
        }
    }
}