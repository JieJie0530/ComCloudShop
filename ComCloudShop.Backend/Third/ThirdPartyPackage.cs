using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using ComCloudShop.ViewModel;

using System.Web.Configuration;
namespace ComCloudShop.Backend
{
    public static class ThirdPartyPackage
    {
        private readonly static string erp_url = WebConfigurationManager.AppSettings["FGERPDeployUrl"];
        public static IEnumerable<ThirdProductModel> GetErpProducts(int page = 1,string dm="")
        {
            //var url = "http://localhost:10787/third/shanpin.xml";
            var url = "http://121.199.170.109:30017/fgspcgerp/data.dpk?method=ecerp.shangpin.get&appkey=02A983BDED92448897666B0AC4D471BD&page_no=" + page;
            if (!string.IsNullOrEmpty(dm)) 
            {
                url += "&condition=SPDM='" + dm + "'";
            }
            var xdoc = XDocument.Load(url);
            var list = new List<ThirdProductModel>();
            var sum = xdoc.Element("shangpin_get_response").Element("total_results").Value;
            if (sum == "0") 
            {
                return list;
            }
            var product = from e in xdoc.Element("shangpin_get_response").Element("shangpins").Elements("shangpin")
                          select e;
            foreach (var q in product)
            {
                list.Add(new ThirdProductModel
                {
                    id = q.Element("id") == null ? 0 : int.Parse(q.Element("id").Value),
                    guid = q.Element("guid") == null ? string.Empty : q.Element("guid").Value,
                    spdm = q.Element("spdm") == null ? string.Empty : q.Element("spdm").Value,
                    spmc = q.Element("spmc") == null ? string.Empty : q.Element("spmc").Value,
                    fkcck = q.Element("fkcck") == null ? string.Empty : q.Element("fkcck").Value,
                    zl = q.Element("zl") == null ? string.Empty : q.Element("zl").Value,
                    bzsj = q.Element("bzsj") == null ? string.Empty : q.Element("bzsj").Value,
                });
            }
            return list;
        }

        public static int GetErpProductsCount(string dm="")
        {
            //var url = "http://localhost:10787/third/shanpin.xml";
            var url = "http://121.199.170.109:30017/fgspcgerp/data.dpk?method=ecerp.shangpin.get&appkey=02A983BDED92448897666B0AC4D471BD";
            if (!string.IsNullOrEmpty(dm))
            {
                url += "&condition=SPDM='" + dm + "'";
            }
            var xdoc = XDocument.Load(url);
            var sum = xdoc.Element("shangpin_get_response").Element("total_results").Value;
            var total = 0;
            int.TryParse(sum, out total);
            return total;
        }
        /// <summary>
        /// erp重发
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ReSentErpOrder(ErpOrderViewModel data)
        {
            var result = false;
            try
            {
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

                var model = (from e in xdoc.Element("TradeOrdersNew").Element("trade_orders_response").Elements("trade")
                              select e).FirstOrDefault();

                if (model != null)
                {
                    result= true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
            
        }


        /// <summary>
        /// 获取ERP列表
        /// </summary>
        /// <param name="page">分页数</param>
        /// <param name="spdm">商品代码</param>
        /// <param name="spmc">商品名称</param>
        /// <returns></returns>
        public static ResultViewModel<IEnumerable<AdminGYERPViewModel>> GetERPProductNew(int page, int size, string spdm, string spmc)
        {
            var result = new ResultViewModel<IEnumerable<AdminGYERPViewModel>>();
            try
            {
                //var url = "http://localhost:10787/third/shanpin.xml";
                var url = "http://121.199.170.109:30017/fgspcgerp/data.dpk?method=ecerp.shangpin.get&appkey=02A983BDED92448897666B0AC4D471BD";
                url += "&fields=ID,GUID,SPDM,SPMC,TY,SJ,ZL,BZSJ";
                var where = "";
                if (!string.IsNullOrEmpty(spdm))
                {
                    where += "SPDM='" + spdm + "'";
                }
                if (!string.IsNullOrEmpty(spmc))
                {
                    if (where != "")
                    {
                        where += " and ";
                    }
                    where += "spmc='" + spmc + "'";
                }
                url += "&condition="+where;

                url += "&page_no=" + page + "&page_size=" + size;

                //获取商品总数
                var xdoc = XDocument.Load(url);
                result.total = int.Parse(xdoc.Element("shangpin_get_response").Element("total_results").Value);
                if (result.total == 0)
                {
                    result.error = (int)ErrorEnum.OK;
                    result.msg = "no data";
                    return result;
                }

                //获取数据
                var list = new List<AdminGYERPViewModel>();
                var data = from e in xdoc.Element("shangpin_get_response").Element("shangpins").Elements("shangpin")
                           select e;
                foreach (var d in data)
                {
                    list.Add(new AdminGYERPViewModel
                    {
                        ID = d.Element("id") == null ? 0 : int.Parse(d.Element("id").Value),
                        GUID = d.Element("guid") == null ? string.Empty : d.Element("guid").Value,
                        SPDM = d.Element("spdm") == null ? string.Empty : d.Element("spdm").Value,
                        SPMC = d.Element("spmc") == null ? string.Empty : d.Element("spmc").Value,
                        TY = d.Element("ty") == null ? string.Empty : d.Element("ty").Value,
                        SJ = d.Element("sj") == null ? string.Empty : d.Element("sj").Value,
                        ZL = d.Element("zl") == null ? string.Empty : d.Element("zl").Value,
                        BZSJ = d.Element("bzsj") == null ? string.Empty : d.Element("bzsj").Value,
                    });
                }

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
                result.result = list;
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }


    }
}