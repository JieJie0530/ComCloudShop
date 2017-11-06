using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.Service;
using ComCloudShop.ViewModel;

namespace ComCloudShop.Layer
{
    public class NewsService
    {

        ///// <summary>
        ///// 获取活动列表
        ///// </summary>
        ///// <param name="page">当前页</param>
        ///// <param name="size">条数</param>
        ///// <returns></returns>
        //public ResultViewModel<IEnumerable<News>> GetBatchList(int page, int size)
        //{

        //    var result = new ResultViewModel<IEnumerable<News>>();
        //    try
        //    {
        //        using (var db = new MircoShopEntities())
        //        {
        //            result.result = db.News.Where(x => x.ISDel == 0).OrderByDescending(x=>x.AddTime).Skip((page - 1) * size).Take(size).ToList();

        //            result.total = db.News.Where(x => x.ISDel == 0).OrderByDescending(x => x.AddTime).Count();
        //            result.error = (int)ErrorEnum.OK;
        //            result.msg = "success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.error = (int)ErrorEnum.Error;
        //        result.msg = ex.Message;
        //    }
        //    return result;
        //}

    }
}
