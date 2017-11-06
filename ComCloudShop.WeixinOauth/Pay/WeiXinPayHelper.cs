using ComCloudShop.WeixinOauth.Pay.Helper;
using ComCloudShop.WeixinOauth.Pay.Models.UnifiedMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.WeixinOauth
{
    public  class WeiXinPayHelper
    {
        private string appId;
        private string appSercet;

        public WeiXinPayResultModel GetWeiXinPayData(string appid, string appsercet,string openid)
        {
            this.appId = appid;
            this.appSercet = appsercet;

            var model = new WeiXinPayResultModel();
            try
            {

                return model;
            }
            catch
            {
                return null;
            }
        }

        #region V3 统一支付接口

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static UnifiedPrePayMessage UnifiedPrePay(string postData)
        {
            string url = WeiXinConst.WeiXin_Pay_UnifiedPrePayUrl;
            var message = HttpClientHelper.PostXmlResponse<UnifiedPrePayMessage>(url, postData);
            return message;
        }

        #endregion

        //public static string CreatePrePayPackage(string description, string tradeNo, string totalFee, string createIp, string notifyUrl, string openid)
        //{
        //    Dictionary<string, string> nativeObj = new Dictionary<string, string>();

        //    nativeObj.Add("appid", AppId);
        //    nativeObj.Add("mch_id", PartnerId);
        //    nativeObj.Add("nonce_str", CommonUtil.CreateNoncestr());
        //    nativeObj.Add("body", description);
        //    nativeObj.Add("out_trade_no", tradeNo);
        //    nativeObj.Add("total_fee", totalFee); //todo:写死为1
        //    nativeObj.Add("spbill_create_ip", createIp);
        //    nativeObj.Add("notify_url", notifyUrl);
        //    nativeObj.Add("trade_type", "JSAPI");
        //    nativeObj.Add("openid", openid);
        //    nativeObj.Add("sign", GetCftPackage(nativeObj));

        //    return DictionaryToXmlString(nativeObj);
        //} 

    }
}
