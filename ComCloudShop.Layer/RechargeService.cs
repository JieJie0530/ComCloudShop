using ComCloudShop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Layer
{
    public class RechargeService
    {
        public Recharge GetRechargeByMemberOrderID(string OrderID)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Recharges.FirstOrDefault(x => x.OrderID == OrderID);
                return m;
            }
        }

        public bool Add(ComCloudShop.Service.Recharge model)
        {
            using (var db = new MircoShopEntities())
            {
                try
                {
                    db.Recharges.Add(model);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool UpdateWithd(string OrderID)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Recharges.FirstOrDefault(x => x.OrderID == OrderID);
                if (m != null)
                {
                    try
                    {
                        m.State = 1;
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
        }
    }
}
