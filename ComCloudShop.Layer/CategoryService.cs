using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;

namespace ComCloudShop.Layer
{
    public class CategoryService : BaseService
    {
        public bool Add(CategoryViewModel instance)
        {
            var m = new Category();
            m.CategoryName = instance.CategoryName;
            m.CategoryType = instance.CategoryType;
            m.CreateDate = DateTime.Now;
            m.ParentId = instance.ParentId;
            try
            {
                db.Categories.Add(m);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(int Id, string Name, int Type, int ParentId=0)
        {
            var m = db.Categories.FirstOrDefault(x => x.CategoryId == Id);
            if (m != null)
            {
                m.CategoryName = Name;
                m.CategoryType = Type;
                m.ParentId = ParentId;
                try
                {
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

        public bool AddCategoryRelation(CategoryRelationViewModel instance)
        {
            var model = new CategoryRelation();
            if (instance.CategoryType == 1)
            {
                var query2 = db.Categories.Where(x => x.CategoryType == 1);
                var query = from x in db.CategoryRelations.Where(x => x.ProductId == instance.ProductId)
                            join p in query2 on x.CategoryId equals p.CategoryId
                            select x;
                if (query.Count() > 0)
                {
                    model = query.FirstOrDefault();
                    model.CategoryId = instance.CategoryId;
                }
                else
                {
                    model.CreateDate = DateTime.Now;
                    model.CategoryId = instance.CategoryId;
                    model.ProductId = instance.ProductId;
                    db.CategoryRelations.Add(model);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(instance.CategoryArrs))
                {
                    var arr = instance.CategoryArrs.Split(',');
                    model = new CategoryRelation();
                    var query2 = db.Categories.Where(x => x.CategoryType == 2);
                    var query = from x in db.CategoryRelations.Where(x => x.ProductId == instance.ProductId)
                                join p in query2 on x.CategoryId equals p.CategoryId
                                select x;
                    foreach (var q in query)
                    {
                        db.CategoryRelations.Remove(q);
                    }
                    for (var i = 0; i < arr.Length; i++)
                    {
                        model.CreateDate = DateTime.Now;
                        model.CategoryId = int.Parse(arr[i]);
                        model.ProductId = instance.ProductId;
                        db.CategoryRelations.Add(model);
                    }
                }
            }

            try
            {
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CategoryViewModel Get(int CategoryId) 
        {
            var m = db.Categories.FirstOrDefault(x => x.CategoryId == CategoryId);
            var q = new CategoryViewModel();
            if (m != null) 
            {
                q.CategoryId = m.CategoryId;
                q.CategoryName = m.CategoryName;
                q.CategoryType = (int)m.CategoryType;
                q.ParentId = (int)m.ParentId;
            }
            return q;
        }

        public IEnumerable<CategoryViewModel> GetCategoryList(int Type,int types)
        {
            var query = db.Categories.Where(x => x.CategoryType == Type && x.ParentId== types);
            var list = new List<CategoryViewModel>();
            foreach (var q in query)
            {
                list.Add(new CategoryViewModel { CategoryId = q.CategoryId, CategoryName = q.CategoryName, CategoryType = (int)q.CategoryType, ParentId = (int)q.ParentId });
            }
            return list;
        }

        /// <summary>
        /// 删除品种
        /// </summary>
        /// <param name="CategoryID">品种ID</param>
        /// <returns></returns>
        public bool Delete(int CategoryID)
        {
            try
            {
                var model = db.Categories.FirstOrDefault(x => x.CategoryId == CategoryID);
                if (model == null)
                {
                    return false;
                }

                db.Categories.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        #region New

        /// <summary>
        /// 获取品种功能列表
        /// </summary>
        /// <param name="Type">类别</param>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<CategoryViewModel>> GetCategoryList(int Type, int page, int size)
        {
            var result = new ResultViewModel<IEnumerable<CategoryViewModel>>();
            try
            {
                result.result = (from a in db.Categories
                                 where a.CategoryType == Type
                                 orderby a.CreateDate descending
                                 select new CategoryViewModel
                                 {
                                     CategoryId = a.CategoryId,
                                     CategoryName = a.CategoryName,
                                     CategoryType = a.CategoryType,
                                     ParentId = (int)a.ParentId
                                 }).Skip((page - 1) * size).Take(size).ToList();

                result.total = (from a in db.Categories
                                where a.CategoryType == Type
                                orderby a.CreateDate descending
                                select new CategoryViewModel
                                {
                                    CategoryId = a.CategoryId
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
        public ResultViewModel<bool> AddOrUpdate(CategoryViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new Category();
                    if (data.CategoryId > 0)
                    {
                        model = db.Categories.FirstOrDefault(x => x.CategoryId == data.CategoryId);
                        model.CategoryName = data.CategoryName;
                    }
                    else
                    {
                        model.CategoryName = data.CategoryName;
                        model.CategoryType = data.CategoryType;
                        model.ParentId = data.ParentId;
                        model.CreateDate = DateTime.Now;

                        db.Categories.Add(model);
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



        /// <summary>
        /// 删除品种
        /// </summary>
        /// <param name="CategoryID">品种ID</param>
        /// <returns></returns>
        public ResultViewModel< bool> DeleteNew(int CategoryID)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                var model = db.Categories.FirstOrDefault(x => x.CategoryId == CategoryID);
                if (model == null)
                {
                    result.error = (int)ErrorEnum.Fail;
                    result.msg = "no data";
                    return result;
                }

                db.Categories.Remove(model);
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




        #endregion

    }
}
