using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using Senparc.Weixin.MP.Containers;
using System.IO;
using Senparc.Weixin.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections;
using System.Xml;
using ComCloudShop.ViewModel;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using System.Web;
using System.Drawing;
using ComCloudShop.Service;
using System.Drawing.Imaging;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Newtonsoft.Json;
using ComCloudShop.Layer;

namespace ComCloudShop.WeixinOauth
{
    public class WeixinOauthHelper
    {

        public static string AddHead(string url, Member m) {
            WebRequest wreq = WebRequest.Create(url);
            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
            Stream s = wresp.GetResponseStream();
            System.Drawing.Image img;
            img = System.Drawing.Image.FromStream(s);
            string fileHead = "head_" + m.MemberId + ".jpg";//二维码图片路径  
            string AddressHead = HttpContext.Current.Server.MapPath("~/ewmPic/" + fileHead);
            img.Save(AddressHead, ImageFormat.Gif);   //保存
            return AddressHead;
        }

        public static string CreateQrCode(Member m) {

            try
            {
                string token = AccessTokenContainer.TryGetAccessToken(WeiXinConst.AppId, WeiXinConst.AppSecret, false);//如果没有注册则进行注册
                CreateQrCodeResult qrCodeResult = Create(token, 0, m.MemberId);// 永久二维码调用此方法
                MemoryStream memStream = new MemoryStream();
                QrCodeApi.ShowQrCode(qrCodeResult.ticket, memStream);
                long leng = memStream.Length;
                System.Drawing.Image img = System.Drawing.Bitmap.FromStream(memStream);
                string newfilename = "ewm_" + m.MemberId + ".jpg";//二维码图片路径  
                string address = HttpContext.Current.Server.MapPath("~/ewmPic/" + newfilename);
                img.Save(address);

                string AddressHead = AddHead(m.HeadImgUrl, m);
                //保存头像
                //MemoryStream memStreamHead = new MemoryStream();
                //QrCodeApi.ShowQrCode(m.HeadImgUrl, memStreamHead);
                //long lengHead = memStreamHead.Length;
                //System.Drawing.Image imgHead = System.Drawing.Bitmap.FromStream(memStreamHead);
                //string fileHead = "head_" + m.MemberId + ".jpg";//二维码图片路径  
                //string AddressHead = HttpContext.Current.Server.MapPath("~/ewmPic/" + newfilename);
                //imgHead.Save(AddressHead);

                return AddShuiYing(m, address, AddressHead);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        ComCloudShop.Layer.MemberService _user = new Layer.MemberService();
        static string AddShuiYing(Member m, string WaterImg, string head) {
            string ImagePath = HttpContext.Current.Server.MapPath("~/ewmPic/moban.jpg");
            try
            {
                Image image = Image.FromFile(ImagePath);
                Image image1 = Image.FromFile(WaterImg);
                Image imghead = Image.FromFile(head);
                Graphics g = Graphics.FromImage(image);
                //水印内容
                string waterText = m.NickName;
                Font font = new Font("宋体", 35);
                //用于确定水印的大小
                SizeF zisizeF = new SizeF();
                zisizeF = g.MeasureString(waterText, font);
                //亮度，红色，绿色，蓝色
                SolidBrush solidBrush = new SolidBrush(Color.Red);
                //水印
                g.DrawString(waterText, font, solidBrush, 248, 340);
                g.DrawImage(image1, 225, 780, 190, 190);
                g.DrawImage(imghead, 43, 330, 98, 98);
                string newfilename = "Hewm_" + m.MemberId + ".jpg";//二维码图片路径
                string address = HttpContext.Current.Server.MapPath("~/ewmPic/" + newfilename);
                image.Save(address);
                return "/ewmPic/" + newfilename;
                //图片另存
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过1800。0时为永久二维码</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时最大值为1000</param>
        /// <returns></returns> 
        public static CreateQrCodeResult Create(string accessToken, int expireSeconds, int sceneId)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            object data = null;
            if (expireSeconds > 0)
            {
                data = new
                {
                    expire_seconds = expireSeconds,
                    action_name = "QR_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
            }
            else
            {
                data = new
                {
                    action_name = "QR_LIMIT_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
            }
            return CommonJsonSend.Send<CreateQrCodeResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce, string token)
        {
            string[] arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            string arrString = string.Join("", arr);
            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }

        /// <summary>
        /// 检查签名是否正确
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce, string token)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }
        /// <summary>
        /// 处理微信服务器验证消息
        /// </summary>
        public static string Auth(string signature, string timestamp, string nonce, string echostr)
        {
            string token = "andszq";
            //get method - 仅在微信后台填写URL验证时触发
            if (CheckSignature(signature, timestamp, nonce, token))
            {
                return echostr; //返回随机字符串则表示验证通过
            }
            else
            {
                return "failed:" + signature + "," + GetSignature(timestamp, nonce, token) + "。" + "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。";
            }
            return "";
        }


        public static string MakeRequest(string requestStr) {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                //封装请求类
                XmlDocument requestDocXml = new XmlDocument();
                requestDocXml.LoadXml(requestStr);
                XmlElement rootElement = requestDocXml.DocumentElement;
                WxXmlModel WxXmlModel = new WxXmlModel();
                WxXmlModel.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                WxXmlModel.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                WxXmlModel.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                WxXmlModel.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
                switch (WxXmlModel.MsgType)
                {
                    case "text"://文本
                        WxXmlModel.Content = rootElement.SelectSingleNode("Content").InnerText;
                        dic.Add("ToUserName", WxXmlModel.FromUserName);
                        dic.Add("FromUserName", WxXmlModel.ToUserName);
                        dic.Add("CreateTime", GenerateTimeStamp());
                        dic.Add("MsgType", "text");
                        dic.Add("Content", "您好！您有什么需要和任何疑问欢迎致联系客服。");
                        break;
                    case "image"://图片
                        WxXmlModel.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        break;
                    case "event"://事件
                        WxXmlModel.Event = rootElement.SelectSingleNode("Event").InnerText;
                        if (WxXmlModel.Event == "subscribe")//关注类型(用户未关注)
                        {
                            string token = AccessTokenContainer.TryGetAccessToken(WeiXinConst.AppId, WeiXinConst.AppSecret,false);//如果没有注册则进行注册

                            //根据appId判断获取    
                            //if (!AccessTokenContainer.CheckRegistered(WeiXinConst.AppId))    //检查是否已经注册    
                            //{
                            //    AccessTokenContainer.Register(WeiXinConst.AppId, WeiXinConst.AppSecret);    //如果没有注册则进行注册    
                            //}

                            // string access_token = AccessTokenContainer.GetAccessTokenResult(WeiXinConst.AppId).access_token; //AccessToke
                            WxModel model= GetModel(token, WxXmlModel.FromUserName);

                            try
                            {
                                WxXmlModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                                string Ticket = rootElement.SelectSingleNode("Ticket").InnerText;
                                string[] envenkeylist = WxXmlModel.EventKey.Split('_');
                                string follw = "";
                                if (envenkeylist.Length > 1)
                                {
                                    follw = envenkeylist[1];
                                }
                                ComCloudShop.Layer.MemberService _user = new Layer.MemberService();
                                var member = _user.GetWechatUser(WxXmlModel.FromUserName);
                                if (member.MemberId == 0)
                                {

                                    member = new MemberViewModel();
                                    member.OpenId = WxXmlModel.FromUserName;
                                    member.NickName = model.nickname;
                                    member.HeadImgUrl = model.headimgurl;
                                    member.follow = follw;
                                    member.ISVip = 0;
                                    member.fsate = 1;
                                    member.Cashbalance = "0";
                                    member.balance = "0";
                                    member.integral = 0;
                                    member.TotalIn = 0;
                                    member.Email = "ewm";
                                    if (_user.Add(member))
                                    {
                                        Member m = _user.GetMemberByMemberID(Convert.ToInt32(member.follow));
                                        if (m!=null)
                                        {
                                            TuiSong(m.OpenId, "您的下级会员" + member.NickName + "通过您的二维码关注了,快点通知他成为分享一员吧，尽快成为您的伙伴！");
                                        }
                                        Member ms = _user.GetMemberByOpenID(member.OpenId);
                                        TuiSong(member.OpenId, "恭喜您成为本平台第" + ms.MemberId + "位会员！");
                                    }
                                }
                                else {
                                    if (member.fsate != 2) {
                                        member.follow = follw;
                                        _user.Update(member);
                                        Member m = _user.GetMemberByMemberID(Convert.ToInt32(member.follow));
                                        if (m!=null)
                                        {
                                            TuiSong(m.OpenId, "您的下级会员" + member.NickName + "通过您的二维码关注了,快点通知他成为分享一员吧，尽快成为您的伙伴！");
                                        }
                                        Member ms = _user.GetMemberByOpenID(member.OpenId);
                                        TuiSong(member.OpenId, "恭喜您成为本平台第" + ms.MemberId + "位会员！");
                                    }
                                }
                               
                            }
                            catch (Exception ex)
                            {
                                return "";
                            }

                            dic.Add("ToUserName", WxXmlModel.FromUserName);
                            dic.Add("FromUserName", WxXmlModel.ToUserName);
                            dic.Add("CreateTime", GenerateTimeStamp());
                            dic.Add("MsgType", "text");
                            dic.Add("Content", getMainMenu());
                        }
                        else if (WxXmlModel.Event == "SCAN")//关注类型(用户已经关注)
                        {
                            dic.Add("ToUserName", WxXmlModel.FromUserName);
                            dic.Add("FromUserName", WxXmlModel.ToUserName);
                            dic.Add("CreateTime", GenerateTimeStamp());
                            dic.Add("MsgType", "text");
                            dic.Add("Content", getMainMenu());
                        }
                        else if (WxXmlModel.Event == "unsubscribe")//取消关注
                        {

                        }
                        else if (WxXmlModel.Event == "CLICK")
                        {   //点击非跳转的菜单
                            //Model.WxUser wx1 = bll.GetModel(WxXmlModel.FromUserName);
                            //dic.Add("ToUserName", WxXmlModel.FromUserName);
                            //dic.Add("FromUserName", WxXmlModel.ToUserName);
                            //dic.Add("CreateTime", GenerateTimeStamp());
                            //dic.Add("MsgType", "text");
                            //if (wx1.IsGz == 0)
                            //{ //如果已经领取了

                            //    dic.Add("Content", "您已经领导的礼品！");
                            //}
                            //else if (wx1.IsGz == 1)
                            //{ //如果没有领取
                            //    wx1.IsGz = 0;
                            //    bll.Update(wx1);
                            //    dic.Add("Content", "领取成功,感谢关注本平台！");
                            //}
                        }
                        break;
                    default:
                        break;
                }
                string xmlstr = DictionaryToXmlString(dic);
                return xmlstr;
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

        }

        private static WxModel GetModel(string token, string openid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token + "&openid=" + openid + "&lang=zh_CN";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";

            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
            string data = sr.ReadToEnd();

            WxModel oauth = new WxModel();

            oauth = (WxModel)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(WxModel));
            return oauth;
        }

        /// <summary>
        /// dictionary转为xml 字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static string DictionaryToXmlString(Dictionary<string, string> dic)
        {
            StringBuilder xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            foreach (string key in dic.Keys)
            {
                xmlString.Append(string.Format("<{0}>{1}</{0}>", key, dic[key]));
            }
            xmlString.Append("</xml>");
            return xmlString.ToString();
        }

        /// <summary>
        /// xml字符串 转换为  dictionary
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XmlToDictionary(string xmlString)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(xmlString);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            var nodes = document.FirstChild.ChildNodes;

            foreach (System.Xml.XmlNode item in nodes)
            {
                dic.Add(item.Name, item.InnerText);
            }
            return dic;
        }
        public static string getMainMenu()
        {

            System.Text.StringBuilder buffer = new StringBuilder();

            buffer.Append(" 金辉物联欢迎您!").Append("\n");
            buffer.Append("花1000送2000赚3000!");
            //buffer.Append("客服电话:4000-710-107");
            return buffer.ToString();

        }

        public static string TuiSong(string openid, string content) {
            //根据appId判断获取    
            if (!AccessTokenContainer.CheckRegistered(WeiXinConst.AppId))    //检查是否已经注册    
            {
                AccessTokenContainer.Register(WeiXinConst.AppId, WeiXinConst.AppSecret);    //如果没有注册则进行注册    
            }

            string access_token = AccessTokenContainer.GetAccessTokenResult(WeiXinConst.AppId).access_token; //AccessToken  
            string posturl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;
            string postData = "{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
            return GetPage(posturl, postData);
        }

        public string GetImgs(string media, bool isY) //获取图片
        {
            if (!AccessTokenContainer.CheckRegistered(WeiXinConst.AppId))    //检查是否已经注册    
            {
                AccessTokenContainer.Register(WeiXinConst.AppId, WeiXinConst.AppSecret);    //如果没有注册则进行注册    
            }

            string access_token = AccessTokenContainer.GetAccessTokenResult(WeiXinConst.AppId).access_token; //AccessToken  
            string url_Menu_Get = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + access_token + "&media_id=" + media;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url_Menu_Get);
            req.Method = "GET";
            string file = string.Empty;

            using (WebResponse wr = req.GetResponse())
            {

                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                string strpath = myResponse.ResponseUri.ToString();
                //WriteTokenToTxt1("D://Test/path", strpath);
                WebClient mywebclient = new WebClient();
                string ran = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4);
                string savepath = "upload\\sale\\" + ran + ".jpg";

                try
                {
                    mywebclient.DownloadFile(strpath, HttpContext.Current.Server.MapPath("/admin/") + savepath);
                    if (isY)
                    {
                        string newpath = "\\Upload\\" + ran + "thu.jpg";
                    }
                }
                catch (Exception ex)
                {
                    savepath = ex.ToString();
                }
                file = savepath;

            }
            return file;
        }

        public static string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);

                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return err;
            }
        }
        private string _appid;
        private string _sercet;
        
        public WeixinOauthHelper(string appId, string sercet)
        {
            this._appid = appId;
            this._sercet = sercet;
        }

        public string GetAuthorizeUri(string url, OauthScope scope)
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state=7#wechat_redirect", this._appid, url, scope.ToString());
        }

        //public Tuple<WeixinTokenResult, string> GetOauth2AccessToken(string code)
        //{
        //    var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={1}&secret={2}&code={0}&grant_type=authorization_code", code, this._appid, this._sercet);
        //    var client = new HttpClient();
        //    try
        //    {
        //        var response = client.GetStringAsync(url);
        //        var resJson = JsonConvert.DeserializeObject<WeixinTokenResult>(response.Result);
        //        return new Tuple<WeixinTokenResult, string>(resJson, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Tuple<WeixinTokenResult, string>(null, ex.Message);
        //    }
        //    finally
        //    {
        //        client.Dispose();
        //    }
        //}


        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetJsapiTicket() //获取js通行证
        {

            
          

            if (System.Web.HttpContext.Current.Application["JsapiTicket"] != null)
            {
                string jt = System.Web.HttpContext.Current.Application["JsapiTicket"].ToString();
                if ((DateTime.Now - Convert.ToDateTime(jt.Split(',')[1])).Seconds < 72)
                {
                    return jt.Split(',')[0];
                }
            }
            string token = AccessTokenContainer.TryGetAccessToken(WeiXinConst.AppId, WeiXinConst.AppSecret, false);//如果没有注册则进行注册
            string url_token = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url_token);
            req.Method = "GET";
            string result = "";
            using (WebResponse wr = req.GetResponse())
            {
                StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.Default);
                string value = reader.ReadToEnd();

                result = value;
            }
            try
            {

                result = result.Replace("\"", "").Split(new string[] { "ticket:" }, StringSplitOptions.RemoveEmptyEntries)[1];
                result = result.Substring(0, result.IndexOf(","));
            }
            catch
            {

                result = "";
            }

            System.Web.HttpContext.Current.Application["JsapiTicket"] = result + "," + DateTime.Now.ToString();
            return result;

        }

        public Tuple<WeixinOauthUserInfo, string> GetOauth2Userinfo(string token, string openid)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", token, openid);
            var client = new HttpClient();
            try
            {
                var response = client.GetStringAsync(url);
                var resJson = JsonConvert.DeserializeObject<WeixinOauthUserInfo>(response.Result);
                return new Tuple<WeixinOauthUserInfo, string>(resJson, null);
            }
            catch (Exception ex)
            {
                return new Tuple<WeixinOauthUserInfo, string>(null, ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }
       
        public string Refund(ComCloudShop.Service.OrderDetail modelo)
        {
            try
            {

                if (modelo.PayableAmount > 0 && Convert.ToDecimal(modelo.PayableAmount)>Convert.ToDecimal(modelo.Jifen))
                {

                    string nonceStr = Senparc.Weixin.MP.TenPayLibV3.TenPayV3Util.GetNoncestr();

                    Senparc.Weixin.MP.TenPayLibV3.RequestHandler packageReqHandler = new Senparc.Weixin.MP.TenPayLibV3.RequestHandler(null);

                    decimal price = Convert.ToDecimal(modelo.PayableAmount) - Convert.ToDecimal(modelo.Jifen);

                    //设置package订单参数
                    packageReqHandler.SetParameter("appid", _appid);          //公众账号ID
                    packageReqHandler.SetParameter("mch_id", WeiXinConst.mch_id);          //商户号
                    packageReqHandler.SetParameter("out_trade_no", modelo.OrderNum);                 //填入商家订单号
                    packageReqHandler.SetParameter("out_refund_no", DateTime.Now.ToString("yyyyMMddhhmmssfff"));                //填入退款订单号
                    packageReqHandler.SetParameter("total_fee", Convert.ToInt32(Convert.ToDecimal(price) * 100).ToString("0.##"));               //填入总金额
                    packageReqHandler.SetParameter("refund_fee", Convert.ToInt32(Convert.ToDecimal(price) * 100).ToString("0.##"));               //填入退款金额
                    packageReqHandler.SetParameter("op_user_id", WeiXinConst.mch_id);   //操作员Id，默认就是商户号
                    packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串

                    string sign = packageReqHandler.CreateMd5Sign("key", WeiXinConst.PartnerKey);

                    packageReqHandler.SetParameter("sign", sign); //签名

                    //退款需要post的数据
                    string data = packageReqHandler.ParseXML();

                    //退款接口地址
                    string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";

                    //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
                    string cert = @"D:\ComCloudShop\cert\apiclient_cert.p12";

                    //私钥（在安装证书时设置）
                    string password = WeiXinConst.mch_id;

                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    //调用证书
                    X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

                    #region 发起post请求
                    HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    webrequest.ClientCertificates.Add(cer);
                    webrequest.Method = "post";

                    byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
                    webrequest.ContentLength = postdatabyte.Length;
                    Stream stream;
                    stream = webrequest.GetRequestStream();
                    stream.Write(postdatabyte, 0, postdatabyte.Length);
                    stream.Close();

                    HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    string responseContent = streamReader.ReadToEnd();
                    #endregion

                    var res = System.Xml.Linq.XDocument.Parse(responseContent);
                    string return_code = res.Element("xml").Element("return_code").Value;

                    Hashtable hashtable = new Hashtable();
                    return return_code;
                }
                else {
                    return "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                
                return "err";
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

    }
}
