using ComCloudShop.Service;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Layer
{
    public class CateService : BaseService
    {

        /// <summary>
        /// 删除品种
        /// </summary>
        /// <param name="CategoryID">品种ID</param>
        /// <returns></returns>
        public ResultViewModel<bool> DeleteNew(int ID)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                var model = db.MangerCates.FirstOrDefault(x => x.ID == ID);
                if (model == null)
                {
                    result.error = (int)ErrorEnum.Fail;
                    result.msg = "no data";
                    return result;
                }

                db.MangerCates.Remove(model);
                db.SaveChanges();

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
        #region New

        /// <summary>
        /// 获取品种功能列表
        /// </summary>
        /// <param name="Type">类别</param>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<CateViewModel>> GetBrandList(int Type, int page, int size)
        {
            var result = new ResultViewModel<IEnumerable<CateViewModel>>();
            try
            {
                result.result = (from a in db.MangerCates
                                 orderby a.Sort
                                 select new CateViewModel
                                 {
                                     ID = a.ID,
                                     Brand = a.Brand,
                                     BrandPic = a.BrandPic,
                                     AddTime = (DateTime)a.AddTime,
                                     Sort = (int)a.Sort
                                 }).Skip((page - 1) * size).Take(size).ToList();

                result.total = (from a in db.MangerCates
                                orderby a.Sort
                                select new CateViewModel
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
        public ResultViewModel<bool> AddOrUpdate(CateViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new MangerCate();
                    if (data.ID > 0)
                    {
                        model = db.MangerCates.FirstOrDefault(x => x.ID == data.ID);
                        model.BrandPic = data.BrandPic;
                        model.Brand = data.Brand;
                        model.Sort = data.Sort;
                    }
                    else
                    {
                        model.Brand = data.Brand;
                        model.BrandPic = data.BrandPic;
                        model.AddTime = DateTime.Now;
                        model.Sort = data.Sort;
                        db.MangerCates.Add(model);
                    }
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



        #endregion
    }
}
