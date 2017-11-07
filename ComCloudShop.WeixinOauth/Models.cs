using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.WeixinOauth
{

    public class WeiXinConst
    {
        //方广
        public const string AppId = "wx70b0579cfed26caf";
        public const string AppSecret = "30fbd19e58a8dbba87dc610a628f2604";
        public const string PartnerId = "1465897002";//商户id
        public const string PartnerKey = "02A983BDED92448897666B0AC4D471BD";

        public const string mch_id = "1465897002";
        public const string mch_key = "1465897002";
        public const string OneED = "3000";//买一张会员卡提高3000的提现额度


        ////方格蓝
        //public const string AppId = "wx8a15d7d674739db5";
        //public const string AppSecret = "e0c8d8d45b1ad1338ee84c7880865a43";
        //public const string PartnerId = "1307230401";//商户id
        //public const string PartnerKey = "abCdefgHigkLmnOpqRst0987654321FK";

        public const string WeiXin_Pay_UnifiedPrePayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        public const string Customer = "金辉物联";
        public const string HostIP = "116.255.166.239";
    }

    /// <summary>
    /// 微信支付接口
    /// </summary>
    public sealed class WeiXinPayResultModel
    {
        public string AppId { get; set; }
        public string Package { get; set; }
        public string Timestamp { get; set; }
        public string Noncestr { get; set; }
        public string PaySign { get; set; }
        public string SignType { get { return "MD5"; } }
    }

    public sealed class WeiXin
    {

    }

    public sealed class WeixinTokenResult
    {
        public string access_token { get; set; }

        public string expires_in { get; set; }

        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }

        public string unionid { get; set; }
    }

    public sealed class WeixinOauthUserInfo
    {
        public string openid { get; set; }

        public string nickname { get; set; }

        public int sex { get; set; }

        public string province { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string headimgurl { get; set; }

        public string[] privilege { get; set; }

        public string unionid { get; set; }

        public int Id { get; set; }

    }

}
