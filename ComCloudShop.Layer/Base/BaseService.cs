using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.Service;

namespace ComCloudShop.Layer
{
    public  abstract class BaseService
    {
        protected  MircoShopEntities db = new MircoShopEntities();
    }
}
