using ComCloudShop.WeiXinPay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.WeiXinPay
{
    public class WeiXinPayHelper
    {
        private string appId;
        private string appSercet;

        public WeiXinPayBackModel GetWeiXinPayData(string appid, string appsercet,string openid)
        {
            this.appId = appid;
            this.appSercet = appsercet;

            var model = new WeiXinPayBackModel();
            try
            {

                return model;
            }
            catch
            {
                return null;
            }
        }

        public 

    }
}
