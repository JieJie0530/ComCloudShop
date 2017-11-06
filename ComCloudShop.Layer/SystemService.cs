using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;
using ComCloudShop.Utility;

namespace ComCloudShop.Layer
{
    public class SystemService
    {
        public bool SaveOrUpdate(HomeImageViewModel model)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var m = new HomeImage();
                    if (model.Id > 0)
                    {
                        m = db.HomeImages.FirstOrDefault(x => x.PicId == model.Id);
                        if (m == null)
                        {
                            return false;
                        }
                    }
                    m.Image = model.Image;
                    m.Link = model.Link;
                    m.Useful = model.Useful;
                    if (model.Id == 0)
                    {
                        m.CreateDate = DateTime.Now;
                        db.HomeImages.Add(m);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<HomeImageViewModel> GetHomeImageList(int page, int size)
        {
            using (var db = new MircoShopEntities())
            {
                var query = (from q in db.HomeImages
                             orderby q.PicId descending
                             select q).Skip((page - 1) * size).Take(size);
                var list = new List<HomeImageViewModel>();
                foreach (var q in query)
                {
                    list.Add(new HomeImageViewModel
                    {
                        Image = q.Image,
                        Id = q.PicId,
                        Link = q.Link,
                        Useful = q.Useful
                    });
                }
                return list;
            }
        }

        public ResultViewModel<bool> DeleteNew(int PicId)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var m = db.HomeImages.FirstOrDefault(x => x.PicId == PicId);
                    if (m == null)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "no data";
                    }
                    else
                    {
                        db.HomeImages.Remove(m);
                        db.SaveChanges();

                        result.error = (int)ErrorEnum.OK;
                        result.msg = "success";
                    }
                }
            }
            catch(Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }

        public bool Delete(int Id)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var m = db.HomeImages.FirstOrDefault(x => x.PicId == Id);
                    if (m == null)
                    {
                        return false;
                    }
                    db.HomeImages.Remove(m);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<BatchListViewModel>> GetBatchList(int page, int size,string type)
        {

            var result = new ResultViewModel<IEnumerable<BatchListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.HomeImages
                                 orderby a.Sort
                                 select new BatchListViewModel
                                 {
                                     PicId = a.PicId,
                                     Image = a.Image,
                                     Useful = a.Useful,
                                     Link = a.Link,
                                     CreateDate = (DateTime)a.CreateDate,
                                     Sort=(int)a.Sort
                                 }).Where(x=>x.Useful==type).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.HomeImages
                                    orderby a.PicId descending
                                    select new BatchListViewModel
                                    {
                                        PicId = a.PicId,
                                        Useful = a.Useful
                                    }
                                 ).Where(x => x.Useful == type).Count();
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

        /// <summary>
        /// 添加或修改活动
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        public ResultViewModel<bool> AddOrUpdate(BatchListViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new HomeImage();
                    if (data.PicId > 0)
                    {
                        model = db.HomeImages.FirstOrDefault(x => x.PicId == data.PicId);
                        model.Image = data.Image;
                        model.Link = data.Link;
                        model.Useful = data.Useful;
                        model.Sort = data.Sort;
                        model.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        model.Image = data.Image;
                        model.Link = data.Link;
                        model.Useful = data.Useful;
                        model.Sort = data.Sort;
                        model.CreateDate = DateTime.Now;

                        db.HomeImages.Add(model);
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

    }
}
