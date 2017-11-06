using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;
using ComCloudShop.Utility;

namespace ComCloudShop.Layer
{
    public class RuleService
    {
        //public bool SaveOrUpdate(RuleViewModel model)
        //{
        //    var m = new SaleRule();
        //    if (model.Id > 0)
        //    {
        //        m = db.SaleRules.FirstOrDefault(x => x.RuleId == model.Id);
        //        if (m == null)
        //        {
        //            return false;
        //        }
        //    }
        //    m.Amount = model.Amount;
        //    m.Discount = model.Discount;
        //    m.Content = model.Rule;
        //    try
        //    {
        //        if (model.Id == 0)
        //        {
        //            m.CreateDate = DateTime.Now;
        //            db.SaleRules.Add(m);
        //        }
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public IEnumerable<RuleViewModel> GetRulesList(int page = 1, int size = 10)
        //{
        //    var query = (from q in db.SaleRules
        //                 orderby q.RuleId descending
        //                 select q).Skip((page - 1) * size).Take(size);
        //    var list = new List<RuleViewModel>();
        //    foreach (var q in query)
        //    {
        //        list.Add(new RuleViewModel
        //        {
        //            Amount = q.Amount,
        //            Discount = q.Discount,
        //            Id = q.RuleId,
        //            Rule = q.Content
        //        });
        //    }
        //    return list;
        //}

        //public bool Delete(int Id) 
        //{
        //    var m = db.SaleRules.FirstOrDefault(x => x.RuleId == Id);
        //    if (m == null) 
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        db.SaleRules.Remove(m);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 返回满减优惠价格
        /// </summary>
        /// <param name="price">订单总价</param>
        /// <returns></returns>
        public RuleViewModel ReductionPrice(decimal price)
        {
            var model = new RuleViewModel();
            try
            {
                using (var db =new MircoShopEntities())
                {
                    var data = db.SaleRules.OrderByDescending(x => x.Amount).FirstOrDefault(x => x.Amount <= price);
                    if (data != null)
                    {
                        model.Id = data.RuleId;
                        model.Rule = data.Content;
                        model.Amount = (decimal)data.Amount;
                        model.Discount = (decimal)data.Discount;
                        return model;
                    }
                }
            }
            catch
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RuleViewModel> GetRulesList()
        {
            using (var db = new MircoShopEntities())
            {
                var query = (from q in db.SaleRules
                             orderby q.Amount ascending
                             select q);
                var list = new List<RuleViewModel>();
                foreach (var q in query)
                {
                    list.Add(new RuleViewModel
                    {
                        Amount = q.Amount,
                        Discount = q.Discount,
                        Id = q.RuleId,
                        Rule = q.Content
                    });
                }
                return list;
            }
        }






        /// <summary>
        /// 获取满减规则
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminRuleViewModel>> GetRulesList(int page, int size)
        {
            var result = new ResultViewModel<IEnumerable<AdminRuleViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.SaleRules
                                     orderby a.Amount descending
                                     select new AdminRuleViewModel
                                     { 
                                        RuleId = a.RuleId,
                                        Amount = a.Amount,
                                        Discount = a.Discount,
                                        Content=a.Content
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.SaleRules
                                    orderby a.Amount descending
                                    select new AdminRuleViewModel
                                    {
                                        RuleId = a.RuleId,
                                        Amount = a.Amount,
                                        Discount = a.Discount,
                                        Content = a.Content
                                    }).Count();
                }

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
            }
            catch(Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 添加/修改 满减规则
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        public ResultViewModel<bool> AddOrUpdate(AdminRuleViewModel data)
        {

            var result = new ResultViewModel<bool>();
            try
            {
                var model = new SaleRule();
                using (var db = new MircoShopEntities())
                {
                    if (data.RuleId > 0)
                    {
                        model = db.SaleRules.FirstOrDefault(x => x.RuleId == data.RuleId);
                        if (model == null)
                        {
                            result.error = (int)ErrorEnum.Fail;
                            result.msg = "no data";

                            return result;
                        }
                        model.Amount = data.Amount;
                        model.Discount = data.Discount;
                        model.Content = data.Content;
                    }
                    else
                    {
                        model.Amount = data.Amount;
                        model.Discount = data.Discount;
                        model.Content = data.Content;
                        model.CreateDate = DateTime.Now;

                        db.SaleRules.Add(model);
                    }
                    db.SaveChanges();
                }

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
        /// 删除满减规则
        /// </summary>
        /// <param name="RuleId">主键RuleId</param>
        /// <returns></returns>
        public ResultViewModel<bool >Delete(int RuleId)
        {

            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.SaleRules.FirstOrDefault(x => x.RuleId == RuleId);

                    if (model == null)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "no data";

                        return result;
                    }

                    db.SaleRules.Remove(model);
                    db.SaveChanges();
                }

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











    }
}
