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
using ComCloudShop.ViewModel;
using System.Web.Configuration;

using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP;
using ComCloudShop.Layer;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;

namespace ComCloudShop.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private WeixinOauthHelper helper = new WeixinOauthHelper(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);

        public ActionResult Index()
        {
            if (this.Session[AppConstant.weixinuser] == null)
            {
                #region 本地测试

                //本地测试
                var user = new WeixinOauthUserInfo();
                user.openid = "oVketxEVM7Rg0M5Zi05ppRFHNsHc";
                user.nickname = "杰杰";
                user.Id = 3533;
                this.Session[AppConstant.weixinuser] = user;
                #endregion

                //#region 上线部署

                //上线
                //var data = WeiXinWebAuthorize();
                //logger.Debug("data.error=" + data.error);
                //if (data.error == 0)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Error");
                //}

                //#endregion
            }
            return RedirectToAction("Index", "Home");
            //Response.Redirect(helper.GetAuthorizeUri(Server.UrlEncode("http://www.cc-wang.cn/authorize/do/"), OauthScope.snsapi_userinfo));
        }

        
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
                        if (this.Session[AppConstant.weixinuser] == null)
                        {
                            //通过，用code换取access_token
                            var data = OAuthApi.GetAccessToken(appId, appSecret, code);
                            string token = AccessTokenContainer.TryGetAccessToken(WeiXinConst.AppId, WeiXinConst.AppSecret, false);//如果没
                            logger.Debug("access_token=" + data.access_token);
                            logger.Debug("token1=" + token);
                            var data_user = OAuthApi.GetUserInfo(data.access_token, data.openid);
                            logger.Debug("data_user=" + data_user.openid+","+data_user.nickname);
                            var model = new WeixinOauthUserInfo();
                            model.openid = data.openid;
                            model.nickname = data_user.nickname;
                            model.headimgurl = data_user.headimgurl;
                            var _mservice = new MemberService();
                            var member = _mservice.GetWechatUser(data.openid);
                            if (member.MemberId == 0)
                            {
                                member = new MemberViewModel();
                                member.OpenId = data.openid;
                                member.NickName = data_user.nickname;
                                member.HeadImgUrl = data_user.headimgurl;
                                if (Request["MID"] != null)
                                {
                                    member.follow = Request["MID"].ToString();//上级
                                    member.fsate = 1;//关系待订单确认
                                }
                                else {
                                    member.follow = "";//上级
                                }
                                member.ISVip = 0;
                                member.Cashbalance = "0";
                                member.balance = "0";
                                member.integral = 0;
                                member.TotalIn = 0;
                                member.Email = "zj";
                                if (_mservice.Add(member))
                                {
                                    model.Id = _mservice.GetWechatUser(data.openid).MemberId;
                                }
                                else
                                {
                                    model.Id = 0;
                                }
                            }
                            else
                            {
                                model.Id = member.MemberId;
                                member.NickName = data_user.nickname;
                                member.HeadImgUrl = data_user.headimgurl;
                                if (member.fsate == 1) {
                                    if (Request["MID"] != null)
                                    {
                                        member.follow = Request["MID"].ToString();//上级
                                        
                                    }
                                }
                                _mservice.Update(member);
                            }
                            this.Session[AppConstant.weixinuser] = model;
                        }
                        result.error = 0;
                        result.msg = "获取授权成功！";
                    }
                }
            }
            return result;
        }


    }
}