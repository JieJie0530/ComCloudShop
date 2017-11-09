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
    public class ProductService
    {
        #region Product
        public ProductUpModel Get(int Id)
        {
            using (var db = new MircoShopEntities())
            {
                var model = new ProductUpModel();
                if (Id == 0)
                {
                    return model;
                }
                var entity = db.Products.FirstOrDefault(x => x.ProductId == Id);
                if (entity != null)
                {
                    model.ProductId = entity.ProductId;
                    model.SPMC = entity.SPMC;
                    model.Sale = entity.Sale;
                    model.Discount = entity.Discount;
                    model.BeginUseAge = entity.BeginUseAge;
                    model.EndUseAge = entity.EndUseAge;
                    model.SubTitle = entity.SubTitle;
                    model.Describle = entity.Describle;
                    model.Weight = entity.Weight;
                    model.Contents = entity.Contents;
                    var query = (from q in db.CategoryRelations
                                 join c in db.Categories on q.CategoryId equals c.CategoryId
                                 where q.ProductId == Id
                                 select new
                                 {
                                     q.CategoryId,
                                     c.CategoryType
                                 });

                    model.ProductCategory = string.Join(",", query.Where(x => x.CategoryType == 1).Select(x => x.CategoryId));
                    model.ProductFunction = string.Join(",", query.Where(x => x.CategoryType == 2).Select(x => x.CategoryId));
                }
                return model;
            }
        }

        public bool Add(ProductViewModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.Products.FirstOrDefault(x => x.SPDM == instance.SPDM);
                if (query == null)
                {
                    byte x = 1;
                    byte y = 0;
                    query = new Product();
                    query.ProductGuid = instance.ProductGuid;
                    query.FKCCK = instance.FKCCK == "是" ? x : y;
                    query.BZSJ = instance.BZSJ;
                    query.SPDM = instance.SPDM;
                    query.SPMC = instance.SPMC;
                    query.Statuts = (byte)AppProductStatus.Enable;
                    query.Discount = 1;
                    query.BeginUseAge = instance.BeginUseAge;
                    query.EndUseAge = instance.EndUseAge;
                    query.SubTitle = instance.SubTitle;
                    query.Describle = instance.Describle;
                    query.Weight = instance.Weight;
                    query.CreateDate = DateTime.Now;
                    try
                    {
                        db.Products.Add(query);
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

        public bool AddErp(string ProductGuid, string SPDM, string SPMC, string Weight, string BZSJ)
        {
            using (var db = new MircoShopEntities())
            {
                var model = db.Products.FirstOrDefault(x => x.SPDM == SPDM);
                var w = 0M;
                var s = 0M;
                decimal.TryParse(Weight, out w);
                decimal.TryParse(BZSJ, out s);
                if (model == null)
                {
                    model = new Product();
                    model.Statuts = 1;
                }
                model.SPDM = SPDM;
                model.SPMC = SPMC;
                model.Weight = w;
                model.BZSJ = s;
                if (model.ProductId == 0)
                {
                    db.Products.Add(model);
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
        }
        public bool Update1(ProductUpModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.Products.FirstOrDefault(x => x.ProductId == instance.ProductId);
                if (query != null)
                {
                    query.Contents = instance.Contents;
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
        }
        public bool Update(ProductUpModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.Products.FirstOrDefault(x => x.ProductId == instance.ProductId);
                if (query != null)
                {
                    query.Discount = instance.Discount;
                    query.SPMC = instance.SPMC;
                    query.Sale = instance.Sale;
                    query.BeginUseAge = instance.BeginUseAge;
                    query.EndUseAge = instance.EndUseAge;
                    query.SubTitle = instance.SubTitle;
                    query.Describle = instance.Describle;
                    query.Weight = instance.Weight;

                    var list = (from q in db.CategoryRelations
                                join c in db.Categories on q.CategoryId equals c.CategoryId
                                where q.ProductId == instance.ProductId
                                select new
                                {
                                    q,
                                    c.CategoryType
                                });
                    if (!string.IsNullOrEmpty(instance.ProductCategory))
                    {
                        var i = int.Parse(instance.ProductCategory.TrimEnd(','));
                        foreach (var c in list.Where(x => x.CategoryType == 1).Select(x => x.q))
                        {
                            db.CategoryRelations.Remove(c);
                        }
                        db.CategoryRelations.Add(new CategoryRelation { CategoryId = i, ProductId = instance.ProductId, CreateDate = DateTime.Now });
                    }

                    if (!string.IsNullOrEmpty(instance.ProductFunction))
                    {
                        var arr = instance.ProductFunction.TrimEnd(',').Split(',');
                        foreach (var c in list.Where(x => x.CategoryType == 2).Select(x => x.q))
                        {
                            db.CategoryRelations.Remove(c);
                        }
                        foreach (var s in arr)
                        {
                            var i = int.Parse(s);
                            db.CategoryRelations.Add(new CategoryRelation { CategoryId = i, ProductId = instance.ProductId, CreateDate = DateTime.Now });
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
                return false;
            }
        }

        public bool Delete(int Id)
        {
            using (var db = new MircoShopEntities())
            {
                var entity = db.Products.FirstOrDefault(x => x.ProductId == Id);
                entity.Statuts = (byte)AppProductStatus.Delete;//删除
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
        }

        /// <summary>
        /// 获取产品图片
        /// </summary>
        /// <param name="id">ProductID</param>
        /// <returns></returns>
        public ProductImgViewModel GetProductImg(int id)
        {
            var model = new ProductImgViewModel();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    model =( from a in db.ProductImgs
                                where a.ProductId == id
                                select new ProductImgViewModel
                                {
                                    ImgId = a.ImgId,
                                    P1 = a.P1,
                                    P2 = a.P2,
                                    P3 = a.P3,
                                    ProductId = (int)a.ProductId
                                }).FirstOrDefault();
                    return model;
                }
            }
            catch
            {
                return model;
            }
        }

        /// <summary>
        /// 发布/下架
        /// </summary>
        /// <param name="id">ProductID</param>
        /// <param name="show">IsShow</param>
        /// <returns></returns>
        public bool SetIsShowProduct(int id, bool show)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.Products.FirstOrDefault(x => x.ProductId == id);
                    if (show != model.IsShow)
                    {
                        return false;
                    }
                    model.IsShow = !model.IsShow;
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
        /// ERP 上架
        /// </summary>
        /// <param name="ProductGuid">ERP商品Guid</param>
        /// <param name="SPDM">商品代码</param>
        /// <param name="SPMC">商品名称</param>
        /// <param name="Weight">重量</param>
        /// <param name="BZSJ">标准售价</param>
        /// <returns></returns>
        public ResultViewModel<bool> ErpToSale(string ProductGuid, string SPDM, string SPMC, decimal Weight, decimal BZSJ)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = db.Products.FirstOrDefault(x => x.SPDM == SPDM);

                    if (data != null)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "该商品已上架";
                        return result;
                    }

                    var model = new Product();
                    model.ProductGuid = ProductGuid;
                    model.SPDM = SPDM;
                    model.SPMC = SPMC;
                    model.Weight = Weight;
                    model.BZSJ = BZSJ;
                    model.Sale = BZSJ;
                    model.Discount = 1;
                    model.Statuts = 1;

                    db.Products.Add(model);
                    db.SaveChanges();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "success";
                }
            }
            catch(Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }

        public IEnumerable<ProductViewModel> GetProductSearchList(string spdm, string spmc, int page = 1, int size = 10)
        {
            if (string.IsNullOrEmpty(spdm) && string.IsNullOrEmpty(spmc))
            {
                return GetProductList(page, size);
            }
            StringBuilder strSql = new StringBuilder();
            var pageindex = size * (page - 1);
            strSql.AppendFormat(" select top {0} img.P1,img.P2,img.P3, a.Weight, a.BeginUseAge,a.EndUseAge,a.SubTitle,a.Describle,  a.ProductId,a.ProductGuid,a.Title,a.SPDM,a.SPMC,a.BZSJ,a.Sale,a.Discount from Product as a  ", size);
            strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = a.ProductId ");
            if (!string.IsNullOrEmpty(spdm))
            {
                strSql.AppendFormat(" where a.Statuts=1 and  a.spdm like '%{1}%' And a.ProductId NOT IN (SELECT TOP {0} b.ProductId FROM Product as b where b.spdm like '%{1}%' and  a.Statuts=1 ORDER BY b.ProductId)  ORDER BY a.ProductId ", pageindex, FilterHelper.FilterSpecial(spdm));
            }
            else
            {
                if (!string.IsNullOrEmpty(spmc))
                {
                    strSql.AppendFormat(" where   a.Statuts=1 and  a.spmc like '%{1}%' And a.ProductId NOT IN (SELECT TOP {0} b.ProductId FROM Product as b where b.spmc like '%{1}%' and  a.Statuts=1 ORDER BY b.ProductId)  ORDER BY a.ProductId ", pageindex, FilterHelper.FilterSpecial(spmc));
                }
            } 
            using (var db = new MircoShopEntities())
            {
                return db.Database.SqlQuery<ProductViewModel>(strSql.ToString()).ToList();
            }
        }


        /// <summary>
        /// 后台获取商品列表数据
        /// </summary>
        /// <param name="spdm"></param>
        /// <param name="spmc"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminProductListViewModel>> GetProductSearchListNew(string spgg,string spdm, string spmc, int page = 1, int size = 10)
        {
            var result = new ResultViewModel<IEnumerable<AdminProductListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Products
                                where (string.IsNullOrEmpty(spdm) ? 1 == 1 : a.SPDM.Contains(spdm)) &&
                                (string.IsNullOrEmpty(spmc) ? 1 == 1 : a.SPMC.Contains(spmc)) 
                                orderby a.ProductId descending
                                select new AdminProductListViewModel
                                {
                                    ProductID = a.ProductId,
                                    SPDM = a.SPDM,
                                    SPMC = a.SPMC,
                                    Sale = a.Sale,
                                    Discount = a.Discount,
                                    IsShow = a.IsShow
                                }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Products
                                    where ( string.IsNullOrEmpty(spdm) ? 1 == 1 : a.SPDM.Contains(spdm)) &&
                                    (string.IsNullOrEmpty(spmc) ? 1 == 1 : a.SPMC.Contains(spmc))
                                    orderby a.ProductId descending
                                    select new AdminProductListViewModel
                                    {
                                        ProductID = a.ProductId,
                                        SPDM = a.SPDM,
                                        SPMC = a.SPMC,
                                        Sale = a.Sale,
                                        Discount = a.Discount,
                                        IsShow = a.IsShow
                                    }).Count();
                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 获取品种下拉数据
        /// </summary>
        /// <param name="CategoryType"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<SelectViewModel>> GetBrandSelect(int CategoryType)
        {
            var result = new ResultViewModel<IEnumerable<SelectViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.ProductBrands
                                     orderby a.AddTime descending
                                     select new SelectViewModel
                                     {
                                         dm = a.ID,
                                         mc = a.Brand
                                     }).ToList();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 获取品种下拉数据
        /// </summary>
        /// <param name="CategoryType"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<SelectViewModel>> GetCategorySelect(int CategoryType)
        {
            var result = new ResultViewModel<IEnumerable<SelectViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Categories
                                     where a.CategoryType == CategoryType
                                     orderby a.CategoryId descending
                                     select new SelectViewModel
                                     {
                                         dm=a.CategoryId,
                                         mc =a.CategoryName
                                     }).ToList();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 获取产品详情
        /// </summary>
        /// <param name="ProductId">主键Id</param>
        /// <returns></returns>
        public ResultViewModel<AdminProductDetailViewModel> AdminGetProducDetail(int ProductId)
        {
            var result = new ResultViewModel<AdminProductDetailViewModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Products join
                                                   bb in db.ProductImgs on a.ProductId equals bb.ProductId into temp
                                                   from b in temp.DefaultIfEmpty() join
                                                   cc in db.CategoryRelations on a.ProductId equals cc.ProductId into temp2
                                                   from c in temp2.DefaultIfEmpty()
                                     where a.ProductId ==ProductId
                                     select new AdminProductDetailViewModel
                                     {
                                         ProductID = a.ProductId,
                                         SPDM = a.SPDM,
                                         SPMC = a.SPMC,
                                         Sale = a.Sale,
                                         Discount = a.Discount,
                                         Describle =a.Describle,
                                         Weight = a.Weight,
                                         BeginUseAge = a.BeginUseAge,
                                         EndUseAge = a.EndUseAge,
                                         CategoryId = c.CategoryId,
                                         P1 = b.P1,
                                         P2 = b.P2,
                                         P3 = b.P3,
                                         Contents = a.Contents,
                                         SPGG=a.SPGG
                                     }).FirstOrDefault();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 更新产品
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultViewModel<bool> UpdateProduct(AdminProductDetailViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    //更新产品
                    var modelProduct = db.Products.FirstOrDefault(x => x.ProductId == data.ProductID);
                    if (modelProduct == null) {
                        modelProduct = new Product();
                        modelProduct.ProductGuid = Guid.NewGuid().ToString();
                        int spdm = 3000;
                        if (db.Products.ToList().Count() > 0) {
                            string spdms = db.Products.OrderByDescending(d => d.SPDM).First().SPDM;
                            spdm = Convert.ToInt32(spdms) + 1;
                        }
                        modelProduct.BZSJ = 0;
                        modelProduct.SPDM = spdm.ToString();
                        modelProduct.SPMC = data.SPMC;
                        modelProduct.Sale = data.Sale;
                        modelProduct.Discount = data.Discount;
                        modelProduct.Describle = data.Describle;
                        modelProduct.Weight = data.Weight;
                        modelProduct.BeginUseAge = data.BeginUseAge;
                        modelProduct.EndUseAge = data.EndUseAge;
                        modelProduct.SPGG = data.SPGG;
                        modelProduct.SubTitle = data.P1;//视频
                        modelProduct.Title = data.P3;//图片
                        db.Products.Add(modelProduct);
                        db.SaveChanges();
                        data.ProductID = modelProduct.ProductId;
                    }
                    modelProduct.BZSJ = 0;
                    modelProduct.SPMC = data.SPMC;
                    modelProduct.Sale = data.Sale;
                    modelProduct.Discount = data.Discount;
                    modelProduct.Describle = data.Describle;
                    modelProduct.Weight = data.Weight;
                    modelProduct.BeginUseAge = data.BeginUseAge;
                    modelProduct.EndUseAge = data.EndUseAge;
                    modelProduct.SPGG = data.SPGG;
                    modelProduct.SubTitle = data.P1;//视频
                    modelProduct.Title = data.P3;//图片
                    //modelProduct.Contents = data.Contents;
                    //更新产品品种
                    if (data.CategoryId > 0)
                    {
                        var modelCategory = db.CategoryRelations.FirstOrDefault(x=>x.ProductId ==data.ProductID);
                        if (modelCategory == null)
                        {
                            modelCategory = new CategoryRelation();
                            modelCategory.CategoryId = (int)data.CategoryId;
                            modelCategory.ProductId = data.ProductID;
                            modelCategory.CreateDate = DateTime.Now;
                            db.CategoryRelations.Add(modelCategory);
                        }
                        modelCategory.CategoryId = (int)data.CategoryId;
                    }
                    //更新产品配图
                    var modelPorductImg = db.ProductImgs.FirstOrDefault(x => x.ProductId == data.ProductID);

                    if (modelPorductImg == null)
                    {
                        modelPorductImg = new ProductImg();
                        modelPorductImg.ProductId = data.ProductID;
                        modelPorductImg.P1 = data.P1;
                        modelPorductImg.P2 ="";
                        modelPorductImg.P3 = data.P3;

                        db.ProductImgs.Add(modelPorductImg);
                    }
                    modelPorductImg.P1 = data.P1;
                    modelPorductImg.P2 = "";
                    modelPorductImg.P3 = data.P3;

                    db.SaveChanges();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 获取产品详情列表
        /// </summary>
        /// <param name="ProductId">主键Id</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable< AdminProductItemDetailViewModel>> AdminGetProducItemDetail(int ProductId)
        {
            var result = new ResultViewModel<IEnumerable<AdminProductItemDetailViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.ProductDetails
                                     where a.ProductId == ProductId
                                     select new AdminProductItemDetailViewModel
                                     {
                                         DetailId = a.DetailId,
                                         ProductId = (int)a.ProductId,
                                         A1 = a.A1,
                                         A2 = a.A2,
                                         A3 = a.A3
                                     }).ToList();

                    result.total = result.result.Count();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 添加/更新 产品详情
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultViewModel<bool> AddOrUpdateProductDetail(AdminProductItemDetailViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new ProductDetail();
                    if (data.DetailId > 0)
                    {
                        model = db.ProductDetails.FirstOrDefault(x => x.DetailId == data.DetailId);
                        
                        model.A1 = data.A1;
                        model.A2 = data.A2;
                        model.A3 = data.A3;
                    }
                    else
                    {
                        model.ProductId = data.ProductId;
                        model.A1 = data.A1;
                        model.A2 = data.A2;
                        model.A3 = data.A3;
                        model.CreateDate = DateTime.Now;

                        db.ProductDetails.Add(model);
                    }

                    db.SaveChanges();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
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
        /// 删除 产品详情
        /// </summary>
        /// <param name="DetailId">产品详情主键</param>
        /// <returns></returns>
        public ResultViewModel<bool> DeleteProductDetailNew(int DetailId)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.ProductDetails.FirstOrDefault(x => x.DetailId == DetailId);
                    if (model ==null)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "no data";

                        return result;
                    }

                    db.ProductDetails.Remove(model);
                    db.SaveChanges();

                    result.error = (int)ErrorEnum.OK;
                    result.msg = "Success";
                }

            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return result;
        }






        public int GetProductCount(string dm, string mc)
        {
            using (var db = new MircoShopEntities())
            {
                var query = (from q in db.Products
                             where (string.IsNullOrEmpty(dm) ? 1 == 1 : q.SPDM.Contains(dm)) && (string.IsNullOrEmpty(mc) ? 1 == 1 : q.SPMC.Contains(mc)) && q.Statuts == 1
                             select q).Count();
                return query;
            }
        }

        public double GetProductPageCount(string dm, string mc, int size = 10)
        {
            var total = GetProductCount(dm, mc);
            return Math.Ceiling((double)total / size);
        }

        public IEnumerable<ProductViewModel> GetProductList1(int page = 1, int size = 10)
        {
            using (var db = new MircoShopEntities())
            {
                StringBuilder strSql = new StringBuilder();
                var pageindex = size * (page - 1);
                strSql.AppendFormat(" select top {0} img.P1,img.P2,img.P3, a.Weight, a.BeginUseAge,a.EndUseAge,a.SubTitle,a.Describle,  a.ProductId,a.ProductGuid,a.Title,a.SPDM,a.SPMC,a.BZSJ,a.Sale,a.Discount,a.SPGG from Product as a  LEFT JOIN ProductImg as img on img.ProductId = a.ProductId where a.SPGG=1 and a.ProductId NOT IN (SELECT TOP {1} b.ProductId FROM Product as b where  b.Statuts=1  ORDER BY b.ProductId)  ORDER BY a.ProductId ", size, pageindex);
                return db.Database.SqlQuery<ProductViewModel>(strSql.ToString()).ToList();
            }
        }

        public IEnumerable<ProductViewModel> GetProductList(int page = 1, int size = 10)
        {
            using (var db = new MircoShopEntities())
            {
                StringBuilder strSql = new StringBuilder();
                var pageindex = size * (page - 1);
                strSql.AppendFormat(" select top {0} img.P1,img.P2,img.P3, a.Weight, a.BeginUseAge,a.EndUseAge,a.SubTitle,a.Describle,  a.ProductId,a.ProductGuid,a.Title,a.SPDM,a.SPMC,a.BZSJ,a.Sale,a.Discount from Product as a  LEFT JOIN ProductImg as img on img.ProductId = a.ProductId where a.ProductId NOT IN (SELECT TOP {1} b.ProductId FROM Product as b where  b.Statuts=1  ORDER BY b.ProductId)  ORDER BY a.ProductId ", size, pageindex);
                return db.Database.SqlQuery<ProductViewModel>(strSql.ToString()).ToList();
            }
        }

        public IEnumerable<ProductViewModel> GetProductSearchLisByCategory(int cid, int start, int end, int page = 1, int size = 10)
        {
            using (var db = new MircoShopEntities())
            {
                StringBuilder strSql = new StringBuilder();
                var pageindex = size * (page - 1);
                strSql.AppendFormat(" select top {0} img.P1,img.P2,img.P3, a.Weight, a.BeginUseAge,a.EndUseAge,a.SubTitle,a.Describle,  a.ProductId,a.ProductGuid,a.Title,a.SPDM,a.SPMC,a.BZSJ,a.Sale,a.Discount  FROM Product as a    ", size);
                if (cid > 0)
                {
                    strSql.Append(" JOIN CategoryRelation as cr ON cr.ProductId = a.ProductId  ");
                    strSql.Append(" JOIN Category as c ON cr.CategoryId = c.CategoryId  AND c.CategoryType=1 ");
                }
                strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = a.ProductId ");
                strSql.Append(" Where 1=1  and a.Statuts=1 ");
                if (cid > 0)
                {
                    strSql.AppendFormat(" And c.CategoryId = {0} ", cid);
                }
                if (start > 0 && end > 0)
                {
                    strSql.AppendFormat(" And a.BeginUseAge >= {0} And a.EndUseAge <= {0} And a.EndUseAge >= BeginUseAge  ", start, end);
                }
                else
                {
                    if (start > 0)
                    {
                        strSql.AppendFormat(" And a.BeginUseAge >= {0} ", end);
                    }
                    if (end > 0)
                    {
                        strSql.AppendFormat(" And a.EndUseAge >0  And a.EndUseAge <= {0}  And a.EndUseAge >= BeginUseAge ", end);
                    }
                }
                if (cid > 0)
                {
                    strSql.AppendFormat("  AND  a.ProductId not IN (SELECT top {0} p.ProductId FROM Product as p JOIN CategoryRelation as x ON x.ProductId=p.ProductId WHERE x.CategoryId={1} and p.Statuts=1 ORDER BY p.ProductId) ", pageindex, cid);
                }
                else
                {
                    strSql.AppendFormat("  AND  a.ProductId not IN (SELECT top {0} p.ProductId FROM Product as p where  p.Statuts=1  ORDER BY p.ProductId) ", pageindex);
                }
                return db.Database.SqlQuery<ProductViewModel>(strSql.ToString()).ToList();
            }
        }

        public ProductExtendResultModel GetDetailById(int productId)
        {
            using (var db = new MircoShopEntities())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" SELECT a.ProductId,a.ProductGuid,a.Title,a.SPDM,a.SPMC,a.Sale,a.Discount,a.BeginUseAge,a.EndUseAge,a.SubTitle,a.Describle,a.Weight,a.Contents, ");
                strSql.Append(" d.A1,d.A2,d.A3,d.DetailId,g.P1,g.P2,g.P3,g.ImgId,c.CategoryId,CategoryName,CategoryType ");
                strSql.Append(" FROM Product AS a ");
                strSql.Append(" LEFT JOIN ProductDetail as d ON d.ProductId=a.ProductId ");
                strSql.Append(" LEFT JOIN ProductImg as g ON g.ProductId = a.ProductId ");
                strSql.Append(" LEFT JOIN CategoryRelation as cr ON cr.ProductId = a.ProductId ");
                strSql.Append(" LEFT JOIN Category as c ON c.CategoryId = cr.CategoryId ");
                strSql.AppendFormat(" WHERE a.ProductId ={0} ", productId);
                var query = db.Database.SqlQuery<ProductDetailResultModel>(strSql.ToString()).ToList();
                var result = new ProductExtendResultModel();
                if (query.Count() > 0)
                {
                    result.ProductId = query.First().ProductId;
                    result.ProductGuid = query.First().ProductGuid;
                    result.Title = query.First().Title;
                    result.SPDM = query.First().SPDM;
                    result.SPMC = query.First().SPMC;
                    result.Sale = query.First().Sale;
                    result.Discount = query.First().Discount;
                    result.BeginUseAge = query.First().BeginUseAge;
                    result.EndUseAge = query.First().EndUseAge;
                    result.SubTitle = query.First().SubTitle;
                    result.Describle = query.First().Describle;
                    result.ImgId = query.First().ImgId;
                    result.P1 = query.First().P1;
                    result.P2 = query.First().P2;
                    result.P3 = query.First().P3;
                    result.Weight = query.First().Weight;
                    result.Contents = query.First().Contents;
                    var list = new List<DetailViewModel>();
                    var list2 = new List<CategoryResultModel>();

                    try
                    {
                        foreach (var p in query)
                        {
                            if (p.DetailId != null)
                            {


                                var d = new DetailViewModel();
                                d.DetailId = (int)p.DetailId;
                                d.A1 = string.IsNullOrEmpty(p.A1) ? string.Empty : p.A1;
                                d.A2 = string.IsNullOrEmpty(p.A2) ? string.Empty : p.A2;
                                d.A3 = string.IsNullOrEmpty(p.A3) ? string.Empty : p.A3;
                                if (d.DetailId > 0 && list.Where(x => x.DetailId == p.DetailId).Count() == 0)
                                {
                                    list.Add(d);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    result.Detail = list.Count > 0 ? list : null;
                    foreach (var p in query)
                    {
                        var c = new CategoryResultModel();
                        c.CategoryId = p.CategoryId;
                        c.CategoryName = string.IsNullOrEmpty(p.CategoryName) ? string.Empty : p.CategoryName;
                        c.CategoryType = p.CategoryType == 0 ? string.Empty : p.CategoryType == 1 ? "种类" : "功能";
                        if (c.CategoryId > 0 && list2.Where(x => x.CategoryId == p.CategoryId).Count() == 0)
                        {
                            list2.Add(c);
                        }

                    }
                    result.Category = list2.Count > 0 ? list2 : null;

                }
                return result;
            }
        }


        /// <summary>
        /// 获取产品页列表，晒选，排序
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <param name="search">查询</param>
        /// <param name="type">类别</param>
        /// <param name="begin">使用月龄开始时间</param>
        /// <param name="end">使用月龄截止时间</param>
        /// <returns></returns>
        public IEnumerable<ProductListViewModel> GetProductList()
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = (from a in db.Products
                                join
                                    b in db.ProductImgs on a.ProductId equals b.ProductId
                                join
                                    c in
                                    (from x in db.OrderProductDetails
                                     group x by x.ProductId into y
                                     select new
                                     {
                                         ProductId = y.Key,
                                         total = y.Sum(z => z.BuyNum)
                                     }) on a.ProductId equals c.ProductId into temp
                                from d in temp.DefaultIfEmpty()
                                join
                                    e in db.CategoryRelations on a.ProductId equals e.ProductId
                                join
                                    f in db.Categories on e.CategoryId equals f.CategoryId
                                where a.Sale==0 && a.IsShow == true
                                select new ProductListViewModel
                                {
                                    ProductId = a.ProductId,
                                    SPMC = a.SPMC,
                                    Pic = b.P1,
                                    Describle = a.Describle,
                                    BeginUseAge = a.BeginUseAge,
                                    Sale = a.Sale,
                                    Discount = a.Discount,
                                    SaleNum = d.total
                                }).ToList();
                    return data;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取产品页列表，晒选，排序
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">条数</param>
        /// <param name="search">查询</param>
        /// <param name="type">类别</param>
        /// <param name="begin">使用月龄开始时间</param>
        /// <param name="end">使用月龄截止时间</param>
        /// <returns></returns>
        public IEnumerable<ProductListViewModel> GetProductList(string spgg,int page, int size, string search, int type, int begin, int end, int minprice, int maxprice)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = (from a in db.Products
                               join
                                   b in db.ProductImgs on a.ProductId equals b.ProductId
                               join
                                   c in
                                   (from x in db.OrderProductDetails
                                    group x by x.ProductId into y
                                    select new
                                    {
                                        ProductId = y.Key,
                                        total = y.Sum(z => z.BuyNum)
                                    }) on a.ProductId equals c.ProductId into temp
                               from d in temp.DefaultIfEmpty()
                               join
                                   e in db.CategoryRelations on a.ProductId equals e.ProductId
                               join
                                   f in db.Categories on e.CategoryId equals f.CategoryId
                                where a.IsShow && a.SPGG==spgg && f.CategoryType == 1 && (minprice>=0?a.Sale>minprice: a.Sale>0) && (maxprice > 0 ? a.Sale < maxprice : a.Sale > 0) && (a.SPMC.Contains(search) || a.SPDM.Contains(search)) && (type > 0 ? e.CategoryId == type : 1 == 1) && (begin==0?1==1:a.BeginUseAge == begin) && a.IsShow == true
                               select new ProductListViewModel
                               {
                                   ProductId = a.ProductId,
                                   SPMC = a.SPMC,
                                   Pic = b.P1,
                                   Describle = a.Describle,
                                   BeginUseAge = a.BeginUseAge,
                                   Sale = a.Sale,
                                   Discount = a.Discount,
                                   SaleNum = d.total
                               }).OrderBy(x => x.Sale).Skip((page - 1) * size).Take(size).ToList();
                    return data;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        //public bool DeleteProduct(int productID)
        //{
        //    try
        //    {
                
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}



        #endregion

        #region ProductDetail
        public bool Add(ProductDetailViewModel instance)
        {
            var m = new ProductDetail();
            m.DetailType = instance.DetailType;
            m.A1 = instance.A1;
            m.A2 = instance.A2;
            m.A3 = instance.A3;
            m.ProductId = instance.ProductId;
            m.CreateDate = DateTime.Now;
            try
            {
                using (var db = new MircoShopEntities())
                {
                    db.ProductDetails.Add(m);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<ProductDetailViewModel> GetProductDetailList(int ProductId)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductDetails.Where(x => x.ProductId == ProductId);
                var list = new List<ProductDetailViewModel>();
                foreach (var q in query)
                {
                    list.Add(new ProductDetailViewModel
                    {
                        ProductId = (int)q.ProductId,
                        A1 = q.A1,
                        DetailId = q.DetailId,
                        A2 = q.A2,
                        A3 = q.A3,
                        DetailType = (byte)q.DetailType
                    });
                }
                return list;
            }
        }

        public bool Update(int DetailId, string A1, string A2, string A3)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductDetails.FirstOrDefault(x => x.DetailId == DetailId);
                if (query != null)
                {
                    query.A1 = A1;
                    query.A2 = A2;
                    query.A3 = A3;
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
        }

        public bool DeleteProductDetail(int DetailId)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductDetails.FirstOrDefault(x => x.DetailId == DetailId);
                if (query != null)
                {
                    try
                    {
                        db.ProductDetails.Remove(query);
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

        public ProductDetailViewModel GetProductDetailById(int DetailId)
        {
            using (var db = new MircoShopEntities())
            {
                var model = db.ProductDetails.FirstOrDefault(x => x.DetailId == DetailId);
                var view = new ProductDetailViewModel();
                if (model != null)
                {
                    view.A1 = model.A1;
                    view.A2 = model.A2;
                    view.A3 = model.A3;
                    view.ProductId = (int)model.ProductId;
                }
                return view;
            }
        }
        #endregion

        #region ProductImg
        public bool SaveOrUpdate(ProductImgViewModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductImgs.FirstOrDefault(x => x.ProductId == instance.ProductId);
                if (query == null)
                {
                    query = new ProductImg();
                    query.ProductId = instance.ProductId;
                }
                query.P1 = instance.P1;
                query.P2 = instance.P2;
                query.P3 = instance.P3;
                try
                {
                    if (query.ImgId == 0)
                    {
                        db.ProductImgs.Add(query);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteProductImg(int ImgId)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductImgs.FirstOrDefault(x => x.ImgId == ImgId);
                if (query != null)
                {
                    try
                    {
                        db.ProductImgs.Remove(query);
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
        public IEnumerable<ProductImg> GetProductImgList(int ProductId)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductImgs.Where(x => x.ProductId == ProductId);
                return query.ToList();
            }
        }

        #endregion

        #region ProductGroup
        public bool Add(ProductGroupViewModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var q = new ProductGroup();
                q.GroupName = instance.GroupName;
                q.Content = instance.Content;
                q.Suggest = instance.Suggest;
                q.PicUrl = instance.PicUrl;
                q.CreateDate = DateTime.Now;
                q.IsShow = 1;
                try
                {
                    db.ProductGroups.Add(q);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Update(ProductGroupViewModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var q = db.ProductGroups.FirstOrDefault(x => x.GroupId == instance.GroupId);
                if (q != null)
                {
                    q.GroupName = instance.GroupName;
                    q.Content = instance.Content;
                    q.Suggest = instance.Suggest;
                    q.PicUrl = instance.PicUrl;
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
        }

        public bool DetaleProductGroupRelation(int GroupId, int ProductId)
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.GroupRelations.FirstOrDefault(x => x.GroupId == GroupId && x.ProductId == ProductId);
                if (query != null)
                {
                    try
                    {
                        db.GroupRelations.Remove(query);
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

        public ProductGroupViewModel GetProductGroup(int GroupId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.ProductGroups.FirstOrDefault(x => x.GroupId == GroupId);
                var q = new ProductGroupViewModel();
                if (m != null)
                {
                    q.GroupName = m.GroupName;
                    q.GroupId = m.GroupId;
                    q.PicUrl = m.PicUrl;
                    q.Content = m.Content;
                    q.Suggest = m.Suggest;
                }
                return q;
            }
        }

        public IEnumerable<ProductGroupViewModel> GetGroupList()
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductGroups.OrderByDescending(x => x.GroupId);
                var list = new List<ProductGroupViewModel>();
                foreach (var q in query)
                {
                    list.Add(new ProductGroupViewModel { GroupId = q.GroupId, GroupName = q.GroupName, Content = q.Content, PicUrl = q.PicUrl, Suggest = q.Suggest });
                }
                return list;
            }
        }

        public IEnumerable<ProductGroupRelationViewModel> GetProductGroupRelation(int page, int size)
        {
            using (var db = new MircoShopEntities())
            {
                var query = (from g in db.ProductGroups
                             join r in db.GroupRelations on g.GroupId equals r.GroupId
                             join p in db.Products on r.ProductId equals p.ProductId
                             orderby g.GroupId ascending
                             select new
                             {
                                 g.GroupId,
                                 g.GroupName,
                                 g.Content,
                                 g.Suggest,
                                 g.PicUrl,
                                 r.RelationId,
                                 r.ProductId,
                                 p.SPDM,
                                 p.SPMC
                             });
                query = query.Skip((page - 1) * size).Take(size);
                var list = new List<ProductGroupRelationViewModel>();
                foreach (var q in list)
                {
                    list.Add(new ProductGroupRelationViewModel
                    {
                        GroupId = q.GroupId,
                        ProductId = q.ProductId,
                        RelationId = q.RelationId,
                        GroupName = q.GroupName,
                        Content = q.Content,
                        Suggest = q.Suggest,
                        PicUrl = q.PicUrl,
                        SPDM = q.SPDM,
                        SPMC = q.SPMC
                    });
                }
                return list;
            }
        }

        public IEnumerable<ProductGroupResultModel> GetRandomGroupList(int groupid)
        {
            using (var db = new MircoShopEntities())
            {
                var limit = groupid < 9 ? 3 : 4;
                StringBuilder strSql = new StringBuilder();
                if (groupid < 9)
                {
                    strSql.AppendFormat(" SELECT TOP {0} NEWID() as random,  img.P1,img.P2,img.P3, p.ProductId,p.SPMC,p.Sale,p.Discount,p.SubTitle ,p.Describle ,p.Weight,g.GroupId,g.GroupName,g.Content,g.Suggest,g.PicUrl ", limit);
                }
                else
                {
                    strSql.AppendFormat(" SELECT TOP {0}   img.P1,img.P2,img.P3, p.ProductId,p.SPMC,p.Sale,p.Discount,p.SubTitle ,p.Describle ,p.Weight,g.GroupId,g.GroupName,g.Content,g.Suggest,g.PicUrl ", limit);
                }
                strSql.Append(" FROM GroupRelation as r ");
                strSql.Append(" JOIN ProductGroup as g ON g.GroupId = r.GroupId  ");
                strSql.Append(" JOIN Product as p ON p.ProductId = r.ProductId  ");
                strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = p.ProductId ");
                strSql.AppendFormat(" WHERE r.GroupId = {0} ", groupid);
                if (groupid < 9)
                {
                    strSql.Append("ORDER by random ");
                }
                return db.Database.SqlQuery<ProductGroupResultModel>(strSql.ToString());
            }
        }

        #endregion

        #region ProductGroupRelation
        public bool AddProductGroupRelation(int ProductId, int GroupId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.GroupRelations.FirstOrDefault(x => x.ProductId == ProductId && x.GroupId == GroupId);
                if (m == null)
                {
                    m = new GroupRelation();
                    m.GroupId = GroupId;
                    m.ProductId = ProductId;
                    m.CreateDate = DateTime.Now;
                    try
                    {
                        db.GroupRelations.Add(m);
                        db.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteProductGroupRelation(int ProductId, int GroupId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.GroupRelations.FirstOrDefault(x => x.ProductId == ProductId && x.GroupId == GroupId);
                if (m == null)
                {
                    return false;
                }
                try
                {
                    db.GroupRelations.Remove(m);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion


        #region ProductSaleGroup

        public bool Add(ProductGroupSaleViewModel data)
        {
            using (var db = new MircoShopEntities())
            {
                try
                {
                    var model = new ProductSaleGroup();
                    model.SaleName = data.saleName;
                    model.SaleOrder = data.saleOrder;
                    model.PicSmallUrl = data.picSmallUrl;
                    model.PicBigUrl = data.picBigUrl;
                    model.Ctime = DateTime.Now;

                    db.ProductSaleGroups.Add(model);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Update(ProductGroupSaleViewModel data)
        {
            using (var db = new MircoShopEntities())
            {
                try
                {
                    var model = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == data.saleId);
                    if (model != null)
                    {
                        model.SaleName = data.saleName;
                        model.SaleOrder = data.saleOrder;
                        if (!string.IsNullOrEmpty(data.picSmallUrl))
                        {
                            model.PicSmallUrl = data.picSmallUrl;
                        }
                        if (!string.IsNullOrEmpty(data.picBigUrl))
                        {
                            model.PicBigUrl = data.picBigUrl;
                        }
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DetaleProductGroupSale(int saleId)
        {
            using (var db = new MircoShopEntities())
            {
                try
                {
                    var model = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == saleId);
                    if (model != null)
                    {
                        db.ProductSaleGroups.Remove(model);
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }



        public IEnumerable<ProductGroupSaleViewModel> GetGroupSaleList()
        {
            using (var db = new MircoShopEntities())
            {
                var query = db.ProductSaleGroups.OrderByDescending(x => x.SaleOrder);
                var list = new List<ProductGroupSaleViewModel>();
                foreach (var q in query)
                {
                    list.Add(new ProductGroupSaleViewModel { saleId = q.SaleId, saleName = q.SaleName, saleOrder = q.SaleOrder });
                }
                return list;
            }
        }

        /// <summary>
        /// 获取推荐活动列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<ProductSaleGroupNewViewModel>> GetGroupSaleList(int page, int size)
        {
            var result = new ResultViewModel<IEnumerable<ProductSaleGroupNewViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.ProductSaleGroups
                                     orderby a.SaleOrder descending
                                     select new ProductSaleGroupNewViewModel
                                     {
                                         SaleId = a.SaleId,
                                         SaleName = a.SaleName,
                                         PicBigUrl = a.PicBigUrl,
                                         PicSmallUrl = a.PicSmallUrl,
                                         SaleOrder = a.SaleOrder
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.ProductSaleGroups
                                    orderby a.SaleOrder descending
                                    select new ProductSaleGroupNewViewModel
                                    {
                                        SaleId = a.SaleId
                                    }).Count();
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
        /// 添加/修改推荐活动
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        public ResultViewModel<bool> AddOrUpdateProductSaleGroup(ProductSaleGroupNewViewModel data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new ProductSaleGroup();
                    if (data.SaleId > 0)
                    {
                        model = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == data.SaleId);
                        model.SaleName = data.SaleName;
                        model.PicSmallUrl = data.PicSmallUrl;
                        model.PicBigUrl = data.PicBigUrl;
                    }
                    else
                    {
                        model.SaleName = data.SaleName;
                        model.PicBigUrl = data.PicBigUrl;
                        model.PicSmallUrl = data.PicSmallUrl;
                        var sale = db.ProductSaleGroups.OrderByDescending(x => x.SaleOrder).FirstOrDefault();
                        if (sale == null)
                        {
                            model.SaleOrder = 1;
                        }
                        else
                        {
                            model.SaleOrder = sale.SaleOrder + 1;
                        }

                        model.Ctime = DateTime.Now;

                        db.ProductSaleGroups.Add(model);
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
        /// 删除推荐活动
        /// </summary>
        /// <param name="SaleId">主键：SaleId</param>
        /// <returns></returns>
        public ResultViewModel<bool> DeleteProductSaleGroupNew(int SaleId)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == SaleId);
                    if (model == null)
                    {
                        result.error = (int)ErrorEnum.Fail;
                        result.msg = "no data";
                    }
                    else
                    {
                        db.ProductSaleGroups.Remove(model);
                        db.SaveChanges();

                        result.error = (int)ErrorEnum.OK;
                        result.msg = "success";
                    }
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
        /// 上移下移，排序
        /// </summary>
        /// <param name="SaleId"></param>
        /// <param name="flag">1：上移；2：下移</param>
        /// <returns></returns>
        public ResultViewModel<bool> MoveProductSaleGroup(int SaleId, int flag)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    if (flag == 1)//上移
                    {
                        var oldo = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == SaleId);
                        var newo = db.ProductSaleGroups.OrderByDescending(x => x.SaleOrder).FirstOrDefault(x => x.SaleOrder > oldo.SaleOrder);
                        if (newo == null)
                        {
                            result.error = (int)ErrorEnum.Fail;
                            result.msg = "到顶啦！";
                            return result;
                        }
                        var move= oldo.SaleOrder;
                        oldo.SaleOrder = newo.SaleOrder;
                        newo.SaleOrder = move;
                    }
                    else//下移
                    {
                        var oldo = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == SaleId);
                        var newo = db.ProductSaleGroups.OrderBy(x => x.SaleOrder).FirstOrDefault(x => x.SaleOrder < oldo.SaleOrder);
                        if (newo == null)
                        {
                            result.error = (int)ErrorEnum.Fail;
                            result.msg = "到底啦！";
                            return result;
                        }
                        var move = oldo.SaleOrder;
                        oldo.SaleOrder = newo.SaleOrder;
                        newo.SaleOrder = move;
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
        /// 添加产品到推荐列表
        /// </summary>
        /// <param name="SaleId">最新推荐主键</param>
        /// <param name="ProductId">产品主键</param>
        /// <returns></returns>
        public ResultViewModel<bool> AddSaleRelation(int SaleId, int ProductId)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using(var db =new MircoShopEntities())
                {
                     var model = db.GroupSaleRelations.FirstOrDefault(x => x.ProductId == ProductId && x.SaleId == SaleId);
                     if (model == null)
                     {
                         model = new GroupSaleRelation();
                         var sale = db.GroupSaleRelations.OrderByDescending(x => x.RelationOrder).FirstOrDefault(x => x.SaleId == SaleId);
                         if (sale == null)
                         {
                             model.RelationOrder = 1;
                         }
                         else
                         {
                             model.RelationOrder = sale.RelationOrder + 1;
                         }

                         model.SaleId = SaleId;
                         model.ProductId = ProductId;
                         model.CreateDate = DateTime.Now;

                         db.GroupSaleRelations.Add(model);
                         db.SaveChanges();

                         result.error = (int)ErrorEnum.OK;
                         result.msg = "success";
                     }
                     else
                     {
                         result.error = (int)ErrorEnum.Fail;
                         result.msg = "该商品已经存在推荐列表中";
                     }
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
        /// 删除 产品推荐列表
        /// </summary>
        /// <param name="RelationId">关系主键</param>
        /// <returns></returns>
        public ResultViewModel<bool> DeleteSaleRelation(int RelationId)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using(var db =new MircoShopEntities())
                {
                     var model = db.GroupSaleRelations.FirstOrDefault(x => x.RelationId ==RelationId);
                     if (model == null)
                     {
                         result.error = (int)ErrorEnum.Fail;
                         result.msg = "no data";

                         return result;
                     }

                     db.GroupSaleRelations.Remove(model);
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
        /// 获取最新推荐的商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<SaleProductNewViewModel>> GetSaleRelationList(int page, int size, int saleId)
        {
            var result = new ResultViewModel<IEnumerable<SaleProductNewViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.GroupSaleRelations
                                     join
                                         b in db.Products on a.ProductId equals b.ProductId
                                     where a.SaleId == saleId && b.IsShow == true
                                     orderby a.RelationOrder descending
                                     select new SaleProductNewViewModel
                                     {
                                         RelationId = a.RelationId,
                                         SPDM = b.SPDM,
                                         SPMC = b.SPMC
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.GroupSaleRelations
                                    join
                                        b in db.Products on a.ProductId equals b.ProductId
                                    where a.SaleId == saleId && b.IsShow == true
                                    orderby a.RelationOrder descending
                                    select new SaleProductNewViewModel
                                    {
                                        ProductId = b.ProductId
                                    }).Count();

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
        /// 获取 未推荐的商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="saleId"></param>
        /// <param name="spdm"></param>
        /// <param name="spmc"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<SaleProductNewViewModel>> GetSaleRelationProductList(int page, int size, int saleId, string spdm, string spmc)
        {

            var result = new ResultViewModel<IEnumerable<SaleProductNewViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Products
                                           where a.IsShow == true && 
                                                     !(from b in db.GroupSaleRelations
                                                                    where b.SaleId ==saleId
                                                       select b.ProductId).Contains(a.ProductId) &&
                                                    (string.IsNullOrEmpty(spdm) ? 1 == 1 : a.SPDM == spdm) &&
                                                    (string.IsNullOrEmpty(spmc) ? 1 == 1 : a.SPMC.Contains(spmc))
                                     orderby a.ProductId descending
                                     select new SaleProductNewViewModel
                                     {
                                         ProductId = a.ProductId,
                                         SPDM = a.SPDM,
                                         SPMC = a.SPMC
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Products
                                    where a.IsShow == true &&
                                              !(from b in db.GroupSaleRelations
                                                where b.SaleId == saleId
                                                select b.ProductId).Contains(a.ProductId) &&
                                                    (string.IsNullOrEmpty(spdm) ? 1 == 1 : a.SPDM == spdm) &&
                                                    (string.IsNullOrEmpty(spmc) ? 1 == 1 : a.SPMC.Contains(spmc))
                                    orderby a.ProductId descending
                                    select new SaleProductNewViewModel
                                    {
                                        ProductId = a.ProductId
                                    }).Count();

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


        public ProductGroupSaleViewModel GetProductSaleGroup(int saleId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == saleId);
                var q = new ProductGroupSaleViewModel();
                if (m != null)
                {
                    q.saleId = m.SaleId;
                    q.saleName = m.SaleName;
                    q.picSmallUrl = m.PicSmallUrl;
                    q.picBigUrl = m.PicBigUrl;
                    q.saleOrder = m.SaleOrder;
                }
                return q;
            }
        }

        public IEnumerable<ProductSaleGroupResultModel> GetRandomGroupSaleList(int saleid)
        {
            //var limit = saleid < 9 ? 3 : 4;
            //StringBuilder strSql = new StringBuilder();
            //if (saleid < 9)
            //{
            //    strSql.AppendFormat(" SELECT TOP {0} NEWID() as random,  img.P1,img.P2,img.P3, p.ProductId,p.SPMC,p.Sale,p.Discount,p.SubTitle ,p.Describle ,p.Weight,g.SaleId,g.SaleName,g.SaleOrder,g.PicSmallUrl,g.PicBigUrl ,r.RelationId ", limit);
            //}
            //else
            //{
            //    strSql.AppendFormat(" SELECT TOP {0}   img.P1,img.P2,img.P3, p.ProductId,p.SPMC,p.Sale,p.Discount,p.SubTitle ,p.Describle ,p.Weight,g.SaleId,g.SaleName,g.SaleOrder,g.PicSmallUrl,g.PicBigUrl ,r.RelationId", limit);
            //}
            using (var db = new MircoShopEntities())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(" SELECT  img.P1,img.P2,img.P3, p.ProductId,p.SPMC,p.Sale,p.Discount,p.SubTitle ,p.Describle ,p.Weight,g.SaleId,g.SaleName,g.SaleOrder,g.PicSmallUrl,g.PicBigUrl ,r.RelationId");
                strSql.Append(" FROM GroupSaleRelation as r ");
                strSql.Append(" JOIN ProductSaleGroup as g ON g.SaleId = r.SaleId  ");
                strSql.Append(" JOIN Product as p ON p.ProductId = r.ProductId  ");
                strSql.Append(" LEFT JOIN ProductImg as img on img.ProductId = p.ProductId ");
                strSql.AppendFormat(" WHERE r.SaleId = {0} ", saleid);
                //if (saleid < 9)
                //{
                //    strSql.Append("ORDER by random ");
                //}
                return db.Database.SqlQuery<ProductSaleGroupResultModel>(strSql.ToString());
            }
        }

        /// <summary>
        /// 根据促销类别获取商品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="saleid">促销列表</param>
        /// <returns></returns>
        public IEnumerable<SaleProductListModel>  GetSaleProductPageList(int page,int size,int saleid)
        {
            var list = new List<SaleProductListModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {

                    list = (from a in db.GroupSaleRelations
                                join
                                    b in db.Products on a.ProductId equals b.ProductId
                                join
                                    c in db.ProductImgs on a.ProductId equals c.ProductId into temp
                                from d in temp.DefaultIfEmpty()
                                orderby b.ProductId ascending
                                where a.SaleId == saleid && b.IsShow ==true
                                select new SaleProductListModel
                                {
                                    Pic = d.P1,
                                    ProductId = a.ProductId,
                                    SPMC = b.SPMC,
                                    Sale = b.Sale,
                                    Discount = b.Discount,
                                    Describle = b.Describle
                                }).Skip((page - 1) * size).Take(size).ToList();
                }
            }
            catch
            {

            }
            return list;
        }

        /// <summary>
        /// 获取活动大图信息
        /// </summary>
        /// <param name="saleid"></param>
        /// <returns></returns>
        public SaleDetailViewModel GetSaleDetail(int saleid)
        {
            var model = new SaleDetailViewModel();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    model.PicBigUrl = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == saleid).PicBigUrl;
                    model.SaleId = saleid;
                }
            }
            catch
            {
                return null;
            }
            return model;
        }


        /// <summary>
        /// 获取最新推荐产品分类列表
        /// </summary>
        /// <returns></returns>
        public HomeViewModel GetProductGroupSaleAll()
        {
            var model = new HomeViewModel();
            try
            {
                using (var db=new MircoShopEntities())
                {
                    model.batch = (from a in db.HomeImages
                                   orderby a.Sort
                                   select new BatchViewModel
                                   {
                                       pic = a.Image,
                                       url = a.Link,
                                       Useful=a.Useful
                                   }).ToList();
                    model.sale = (from a in db.ProductSaleGroups
                                  orderby a.SaleOrder descending
                                  select new SaleViewModel
                                  {
                                      id = a.SaleId,
                                      url = a.PicSmallUrl
                                  }).ToList();
                }
            }
            catch
            {
                model = new HomeViewModel();
            }

            return model;
        }

        #endregion


        #region GroupSaleRelation 活动管理

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="SaleID">活动id</param>
        /// <returns></returns>
        public bool DeleteProductSaleGroup(int SaleID)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.ProductSaleGroups.FirstOrDefault(x => x.SaleId == SaleID);
                    if (model == null)
                    {
                        return false;
                    }

                    var list = db.GroupSaleRelations.Where(x => x.SaleId == SaleID);
                    foreach (var l in list)
                    {
                        db.GroupSaleRelations.Remove(l);
                    }

                    db.ProductSaleGroups.Remove(model);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Add(GroupSaleRelationViewModel data)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.GroupSaleRelations.FirstOrDefault(x => x.ProductId == data.productId && x.SaleId == data.saleId);
                    if (model == null)
                    {
                        int order;
                        var sale = db.GroupSaleRelations.OrderByDescending(x=>x.RelationOrder).FirstOrDefault(x => x.SaleId == data.saleId);
                        if (sale==null)
                        {
                            order = 1;
                        }
                        else
                        {
                            order = sale.RelationOrder + 1;
                        }


                        model = new GroupSaleRelation();
                        model.SaleId = data.saleId;
                        model.ProductId = data.productId;
                        model.RelationOrder = order;
                        model.CreateDate = DateTime.Now;

                        db.GroupSaleRelations.Add(model);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProductGroupSaleRelation(int relationId)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.GroupSaleRelations.FirstOrDefault(x => x.RelationId == relationId);
                    if (model == null)
                    {
                        return false;
                    }

                    db.GroupSaleRelations.Remove(model);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
