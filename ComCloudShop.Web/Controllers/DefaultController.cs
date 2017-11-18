using ComCloudShop.Layer;
using ComCloudShop.WeixinOauth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ComCloudShop.Web.Controllers
{
    public class DefaultController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Default
        public ActionResult Index()
        {

            try
            {
                Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
                byte[] requestByte = new byte[requestStream.Length];
                requestStream.Read(requestByte, 0, (int)requestStream.Length);
                string xmlString = Encoding.UTF8.GetString(requestByte);
                logger.Info("xmlString " + xmlString);
                var res = XDocument.Parse(xmlString);
                //通信成功
                if (res.Element("xml").Element("return_code").Value == "SUCCESS")
                {
                    if (res.Element("xml").Element("result_code").Value == "SUCCESS")
                    {

                        //string trade_no = res.Element("xml").Element("transaction_id").Value;
                        //string ordecode = res.Element("xml").Element("out_trade_no").Value;
                        //logger.Info("100  OrderNum " + trade_no);
                        //ComCloudShop.Layer.RechargeService bll = new RechargeService();
                        //ComCloudShop.Service.Recharge model = bll.GetRechargeByMemberOrderID(ordecode);
                        //if (model != null && model.State==0)
                        //{
                        //    bll.UpdateWithd(ordecode);
                        //    int memberID = Convert.ToInt32(model.MemberID);
                        //    var user = _user.GetMemberBID(memberID);
                        //    user.integral += 1000; //如果是会员卡。。增加1000元购物积分
                        //    if (user.fsate == 1)
                        //    {
                        //        user.fsate = 2;
                        //    }
                        //    user.Cashbalance = (Convert.ToDecimal(user.Cashbalance) + 3000).ToString();//增加可提现额度3000
                        //    _user.UpdateMember(user);
                        //    WeixinOauthHelper.TuiSong(user.OpenId, "恭喜您增加了1000的购物积分！");
                        //    //如果它有上级
                        //    if (user.follow != "")
                        //    {
                        //        j = 0;
                        //        AddCommission(Convert.ToInt32(user.follow));
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {

            }
           
            return Content("");
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
    }
}