using ComCloudShop.Service;
using ComCloudShop.Utility;
using ComCloudShop.ViewModel;
using ComCloudShop.WeixinOauth;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Web.Controllers
{
    public class AuthorizeMangeController : Controller
    {
        private WeixinOauthHelper helper = new WeixinOauthHelper(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);

        public ActionResult Index()
        {
            if (this.Session[AppConstant.weixinAdminuser] == null)
            {
                #region 本地测试

                //本地测试
                //var user = new ComCloudShop.Service.Manger();
                //user.ID = 1;
                //user.UserName = "admin";
                //user.Pwd = "1";
                //user.OpenID = "ooi4mw92zYdTtIU1FkcW4yS_82ww";
                //this.Session[AppConstant.weixinAdminuser] = user;
                #endregion

                //#region 上线部署

                //上线
                var data = WeiXinWebAuthorize();
                logger.Debug("data.error=" + data.error);
                if (data.error == 0)
                {
                    return RedirectToAction("Index", "Business");
                }
                else
                {
                    return RedirectToAction("Login", "Business");
                }

                //#endregion
            }
            return RedirectToAction("index", "Business");
            //Response.Redirect(helper.GetAuthorizeUri(Server.UrlEncode("http://www.cc-wang.cn/authorize/do/"), OauthScope.snsapi_userinfo));
        }

        MircoShopEntities db = new MircoShopEntities();
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                        if (this.Session[AppConstant.weixinAdminuser] == null)
                        {
                            //通过，用code换取access_token
                            var data = OAuthApi.GetAccessToken(appId, appSecret, code);
                            string token = AccessTokenContainer.TryGetAccessToken(WeiXinConst.AppId, WeiXinConst.AppSecret, false);//如果没
                           
                            var data_user = OAuthApi.GetUserInfo(data.access_token, data.openid);
                            logger.Debug("data_user=" + data_user.nickname);
                            this.Session["OAmodel"] = data_user;
                            var model = db.Mangers.Where(d => d.OpenID == data_user.openid).FirstOrDefault();
                            if (model == null) {
                                result.error =1;
                            }
                            else
                            {
                                this.Session[AppConstant.weixinAdminuser] = model;
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