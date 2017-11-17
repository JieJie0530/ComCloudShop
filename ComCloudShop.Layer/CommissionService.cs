using ComCloudShop.Service;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Layer
{
    public class CommissionService: BaseService
    {


        /// <summary>
        /// 获取品种功能列表
        /// </summary>
        /// <param name="Type">类别</param>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<CommissionViewModelDetia>> GetCommissionsList(string Phone, int page, int size)
        {
            var result = new ResultViewModel<IEnumerable<CommissionViewModelDetia>>();
            try
            {
                result.result = (from a in db.Commissions
                                 orderby a.AddTime where a.Phone==Phone
                                 select new CommissionViewModelDetia
                                 {
                                     ID = a.ID,
                                     OrderID = a.OrderID,
                                     Remark = a.Remark,
                                     AddTime = (DateTime)a.AddTime,
                                     Phone=a.Phone,
                                     Price=(decimal)a.Price
                                 }).Skip((page - 1) * size).Take(size).ToList();

                result.total = (from a in db.Commissions
                                orderby a.AddTime
                                where a.Phone == Phone
                                select new CommissionViewModelDetia
                                {
                                    ID = a.ID
                                }).Count();

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 添加/修改 推荐活动
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        public ResultViewModel<bool> AddOrUpdate(string Phone,string OrderID, string Remark,decimal Price)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new Commission();
                        model.Phone = Phone;
                        model.OrderID = OrderID;
                        model.AddTime = DateTime.Now;
                        model.Remark = Remark;
                    model.Price = Price;
                        db.Commissions.Add(model);
                    db.SaveChanges();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "success";
                }
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
