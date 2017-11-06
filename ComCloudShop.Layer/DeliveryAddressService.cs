
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.ViewModel;
using ComCloudShop.Service;

namespace ComCloudShop.Layer
{
    public class DeliveryAddressService : BaseService
    {
        public DeliveryAddressViewModel GetDeliveryAddressById(int id)
        {
            var m = new DeliveryAddressViewModel();
            if (id < 1)
            {
                return m;
            }
            var e = (from d in db.DeliveryAddresses
                     join u in db.Members on d.MemberId equals u.MemberId
                     where d.AddressId == id
                     select new
                     {
                         Address = d,
                         User = u
                     }).FirstOrDefault();
            if (e != null)
            {
                m.Address = e.Address.Address;
                m.AddressId = e.Address.AddressId;
                m.City = e.Address.City;
                m.District = e.Address.District;
                m.IsMainAddr = e.Address.IsMainAddr.HasValue?e.Address.IsMainAddr.Value:false;
                m.MemberId = e.User.MemberId;
                m.MemberName = e.User.NickName;
                m.UserName = e.Address.UserName;
                m.Province = e.Address.Province;
                m.Mobile = e.Address.Mobile;
                m.Tags = e.Address.Tags;
            }
            return m;
        }
        //public bool SaveOrUpdate(DeliveryAddressViewModel instance)
        //{
        //    var m = db.DeliveryAddresses.FirstOrDefault(x => x.AddressId == instance.AddressId && x.IsDelete == false);
        //    if (m == null)
        //    {
        //        m = new DeliveryAddress();
        //    }
        //    m.Address = instance.Address;
        //    m.IsMainAddr = instance.IsMainAddr;
        //    m.Province = instance.Province;
        //    m.City = instance.City;
        //    m.District = instance.District;
        //    m.Mobile = instance.Mobile;
        //    m.MemberId = instance.MemberId;
        //    m.UserName = instance.UserName;
        //    m.Tags = instance.Tags;
        //    if (instance.AddressId == 0)
        //    {
        //        m.IsDelete = false;
        //        db.DeliveryAddresses.Add(m);
        //    }
        //    var q = db.DeliveryAddresses.FirstOrDefault(x => x.IsMainAddr == true && x.MemberId == instance.MemberId);
        //    if (m.IsMainAddr.HasValue && m.IsMainAddr.Value && q != null && q.IsMainAddr.HasValue && q.IsMainAddr.Value)
        //    {
        //        q.IsMainAddr = false;
        //    }
        //    try
        //    {
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 更新用户地址
        /// </summary>
        /// <param name="instance">参数</param>
        /// <returns></returns>
        public bool SaveOrUpdate(DeliveryAddressViewModel instance)
        {
            
            if (instance.AddressId == 0)
            {
                var m = new DeliveryAddress();

                m.Address = instance.Address;
                if (db.DeliveryAddresses.Where(x => x.MemberId == instance.MemberId && x.IsDelete == false && x.IsMainAddr == true).Count() > 0)
                {
                    m.IsMainAddr = false;
                }
                else
                {
                    m.IsMainAddr = true;
                }
                m.Province = instance.Province;
                m.City = instance.City;
                m.District = instance.District;
                m.Mobile = instance.Mobile;
                m.MemberId = instance.MemberId;
                m.UserName = instance.UserName;
                m.Tags = instance.Tags;
                m.IsDelete = false;

                db.DeliveryAddresses.Add(m);
            }
            else
            {
                var m = db.DeliveryAddresses.FirstOrDefault(x => x.AddressId == instance.AddressId && x.IsDelete == false);

                m.UserName = instance.UserName;
                m.Mobile = instance.Mobile;
                m.Province = instance.Province;
                m.City = instance.City;
                m.District = instance.District;
                m.Address = instance.Address;
            }
            //var q = db.DeliveryAddresses.FirstOrDefault(x => x.IsMainAddr == true && x.MemberId == instance.MemberId);
            //if (m.IsMainAddr.HasValue && m.IsMainAddr.Value && q != null && q.IsMainAddr.HasValue && q.IsMainAddr.Value)
            //{
            //    q.IsMainAddr = false;
            //}
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

        public bool Delete(int Id, int UserId)
        {
            var q = db.DeliveryAddresses.FirstOrDefault(x => x.AddressId == Id && x.MemberId == UserId);
            if (q != null)
            {
                try
                {
                    q.IsDelete = true;
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

        public int GetAddressCount(int uid)
        {
            return db.DeliveryAddresses.Where(x => (uid > 0 ? x.MemberId == uid : 1 == 1) && x.IsDelete == false).Count();
        }

        public double GetAddressPageCount(int uid, int size = 10)
        {
            var total = GetAddressCount(uid);
            return Math.Ceiling((double)total / size);
        }

        public IEnumerable<DeliveryAddressViewModel> GetAddresssList(int uid, int page = 1, int size = 10)
        {

            StringBuilder strSql = new StringBuilder();
            var pageindex = size * (page - 1);
            strSql.AppendFormat(" select top {0} a.Tags, a.Address,a.Province,a.City,a.District,(CASE WHEN a.IsMainAddr = 1 THEN '是'   ELSE '否' END) as MainAddr,a.AddressId,a.UserName,a.Mobile,a.IsMainAddr,m.NickName as MemberName,a.MemberId  from DeliveryAddress as a ", size);
            strSql.Append(" JOIN Member as m ON m.MemberId = a.MemberId ");
            strSql.Append(" where 1 = 1 And a.IsDelete=0 ");
            if (uid > 0)
            {
                strSql.AppendFormat(" and a.MemberId={0} ", uid);
                strSql.AppendFormat(" and a.AddressId NOT IN (SELECT TOP {0} b.AddressId FROM DeliveryAddress as b  WHERE b.MemberId={1} and b.IsDelete=0 ORDER BY b.AddressId)  ORDER BY a.AddressId ", pageindex, uid);
            }
            else
            {
                strSql.AppendFormat(" and a.AddressId NOT IN (SELECT TOP {0} b.AddressId FROM DeliveryAddress as b  WHERE  b.IsDelete=0 ORDER BY b.AddressId)  ORDER BY a.AddressId ", pageindex);
            }
            return db.Database.SqlQuery<DeliveryAddressViewModel>(strSql.ToString());

        }

        /// <summary>
        /// 获取用户默认地址
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        public AddressDetailViewModel GetDefaultAddress(int uid)
        {
            var model = new AddressDetailViewModel();
            try
            {
                using (db)
                {
                    var data = db.DeliveryAddresses.FirstOrDefault(x => x.IsMainAddr == true && x.MemberId == uid && x.IsDelete == false);
                    if (data != null)
                    {
                        model.AddressId = data.AddressId;
                        model.UserName = data.UserName;
                        model.Mobile = data.Mobile;
                        model.City = data.City;
                        model.Province = data.Province;
                        model.District = data.District;
                        model.Address = data.Address;
                    }
                    return model;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public AddressDetailViewModel GetAddressById(int id)
        {
            var model = new AddressDetailViewModel();
            try
            {
                using (db)
                {
                    var data = db.DeliveryAddresses.FirstOrDefault(x => x.AddressId==id && x.IsDelete == false);
                    if (data != null)
                    {
                        model.AddressId = data.AddressId;
                        model.UserName = data.UserName;
                        model.Mobile = data.Mobile;
                        model.City = data.City;
                        model.Province = data.Province;
                        model.District = data.District;
                        model.Address = data.Address;
                    }
                    return model;
                }
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// Admin 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName">会员昵称</param>
        /// <param name="userName">收件人</param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminAddressViewModel>> AdminGetAddressList(int page, int size, string nickName, string userName)
        {
            var result = new ResultViewModel<IEnumerable<AdminAddressViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.DeliveryAddresses
                                     join
                                         b in db.Members on a.MemberId equals b.MemberId
                                     where a.IsDelete == false && (string.IsNullOrEmpty(nickName) ? 1 == 1 : b.NickName.Contains(nickName)) && (string.IsNullOrEmpty(userName) ? 1 == 1 : a.UserName.Contains(userName))
                                     orderby a.AddressId descending
                                     select new AdminAddressViewModel
                                     {
                                         AddressId = a.AddressId,
                                         NickName = b.NickName,
                                         IsMainAddr = (bool)a.IsMainAddr,
                                         UserName = a.UserName,
                                         Mobile = a.Mobile,
                                         Province = a.Province,
                                         City = a.City,
                                         District = a.District,
                                         Address = a.Address
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.DeliveryAddresses
                                    join
                                        b in db.Members on a.MemberId equals b.MemberId
                                    where a.IsDelete == false &&  (string.IsNullOrEmpty(nickName) ? 1 == 1 : b.NickName.Contains(nickName)) && (string.IsNullOrEmpty(userName) ? 1 == 1 : a.UserName.Contains(userName))
                                    orderby a.AddressId descending
                                    select new AdminAddressViewModel
                                    {
                                        AddressId = a.AddressId
                                    }).Count();
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
