using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Web.Configuration;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;


namespace ComCloudShop.Web.Third
{
    public class ThirdPartyPackage
    {
        private readonly static string erp_url = WebConfigurationManager.AppSettings["FGERPDeployUrl"];

        public static ERPAddOrderResultModel AddOrderToERP(ERPAddOrderPramModel data)
        {
            try
            {
                var model = new ERPAddOrderResultModel();

                //测试
                data.receiver_name = "test";

                var url = erp_url + "?";
                url += "method=ecerp.trade.add_order_new&appkey=02A983BDED92448897666B0AC4D471BD";
                url += "&itemsns=" + data.itemsns + "&prices=" + data.prices;
                url += "&nums=" + data.nums + "&mail=" + data.mail;
                url += "&receiver_name=" + data.receiver_name + "&receiver_address=" + data.receiver_address;
                url += "&receiver_state=" + data.receiver_state + "&receiver_city=" + data.receiver_city + "&outer_shop_code=" + data.outer_shop_code;
                url += "&receiver_district=" + data.receiver_district + "&logistics_type=" + data.logistics_type + "&outer_tid=" + data.outer_tid;
                url += "&receiver_mobile=" + data.receiver_mobile + "&outer_ddly=" + data.outer_ddly + "&buyer_message=" + data.buyer_message;
                url += "&pay_moneys=" + data.pay_moneys + "&pay_datatimes=" + data.pay_datatimes + "&logistics_fee="; //+data.logistics_fee;

                var xdoc = XDocument.Load(url);

                var result = (from e in xdoc.Element("TradeOrdersNew").Element("trade_orders_response").Elements("trade")
                              select e).FirstOrDefault();

                if (result != null)
                {
                    model.created = result.Element("created") == null ? "" : result.Element("created").Value;
                    model.tid = result.Element("tid") == null ? "" : result.Element("tid").Value;
                }

                return model;
            }
            catch (Exception ex)
            {
                using (var db = new MircoShopEntities())
                {
                    var err_order = new ErpOrder();
                    err_order.mail = data.mail;
                    err_order.itemsns = data.itemsns;
                    err_order.prices = data.prices;
                    err_order.nums = data.nums;
                    err_order.receiver_name = data.receiver_name;
                    err_order.receiver_address = data.receiver_address;
                    err_order.receiver_state = data.receiver_state;
                    err_order.receiver_city = data.receiver_city;
                    err_order.receiver_district = data.receiver_district;
                    err_order.logistics_type = data.logistics_type;
                    err_order.outer_tid = data.outer_tid;
                    err_order.outer_shop_code = data.outer_shop_code;
                    err_order.receiver_mobile = data.receiver_mobile;
                    err_order.outer_ddly = data.outer_ddly;
                    err_order.buyer_message = data.buyer_message;
                    err_order.logistics_fee = data.logistics_fee;
                    err_order.pay_moneys = data.pay_moneys;
                    err_order.ticket_no = data.ticket_no;
                    err_order.pay_datatimes = data.pay_datatimes;
                    err_order.err_message = ex.Message;
                    err_order.ctime = DateTime.Now;
                    err_order.isresend = false;
                    db.ErpOrders.Add(err_order);
                    db.SaveChanges();
                }
                return null;
            }
        }





        public static string GetOrderToERP()
        {

            var url = erp_url + "?";
            url += "method=ecerp.trade.get&appkey=02A983BDED92448897666B0AC4D471BD";
            url += "&shopcode=方广官方微商城";

            var xdoc = XDocument.Load(url);

            return "";
        }

        //public static int GetErpProductsCount(string dm = "")
        //{
        //    //var url = "http://localhost:10787/third/shanpin.xml";
        //    var url = "http://121.199.170.109:30017/fgspcgerp/data.dpk?method=ecerp.shangpin.get&appkey=02A983BDED92448897666B0AC4D471BD";
        //    if (!string.IsNullOrEmpty(dm))
        //    {
        //        url += "&condition=SPDM='" + dm + "'";
        //    }
        //    var xdoc = XDocument.Load(url);
        //    var sum = xdoc.Element("shangpin_get_response").Element("total_results").Value;
        //    var total = 0;
        //    int.TryParse(sum, out total);
        //    return total;
        //}

    }
}