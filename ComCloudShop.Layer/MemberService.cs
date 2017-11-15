using ComCloudShop.Service;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComCloudShop.Utility;
namespace ComCloudShop.Layer
{
    public class MemberService
    {
        public Member GetMemberByMemberID(int memberID)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.MemberId == memberID);
                return m;
            }
            return null;

        }
        public Member GetMemberByOpenID(string openid)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.OpenId == openid);
                return m;
            }
            return null;
            
        }

        public bool Add(MemberViewModel model)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.OpenId == model.OpenId);
                if (m == null)
                {
                    m = new Member();
                    m.OpenId = model.OpenId;
                    m.NickName = model.NickName;
                    m.HeadImgUrl = model.HeadImgUrl;
                    m.Gender = model.Gender;
                    m.OrignKey = model.OrignKey;
                    m.City = model.City;
                    m.Country = model.Country;
                    m.CreateDate = DateTime.Now;
                    m.Province = model.Province;
                    m.balance = model.balance;
                    m.Cashbalance = model.Cashbalance;
                    m.follow = model.follow;
                    m.fsate = model.fsate;
                    m.ISVip = model.ISVip;
                    m.TotalIn = model.TotalIn;
                    m.integral = model.integral;
                    m.Email = model.Email;
                    m.QQ = model.QQ;
                    m.Mobile = model.Mobile;
                    try
                    {
                        db.Members.Add(m);
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
        

        public bool UpdateWithd(int ID)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Withdrawals.FirstOrDefault(x => x.ID == ID);
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

        public bool UpdateVips(int MemberId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.MemberId == MemberId);
                if (m != null)
                {
                    try
                    {
                        m.ISVip = 2;
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
        public bool Update(MemberViewModel instance)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.MemberId == instance.MemberId);
                if (m != null)
                {
                    TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    m.LastLoginDate = m.LoginDate;
                    m.Mobile = instance.Mobile;
                    m.UserName = instance.UserName;
                    m.Birth = instance.Birth;
                    m.Email = instance.Email;
                    m.QQ = instance.QQ;
                    m.NickName = instance.NickName;
                    m.HeadImgUrl = instance.HeadImgUrl;
                    m.Gender = instance.Gender;
                    m.City = instance.City;
                    m.Country = instance.Country;
                    m.Province = instance.Province;

                    m.LoginDate = Convert.ToInt32(ts.TotalSeconds);

                    m.follow = instance.follow;
                    m.fsate = instance.fsate;
                    m.ISVip = instance.ISVip;
                    m.balance = instance.balance;
                    m.Cashbalance = instance.Cashbalance;
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

        public int GetUserCount()
        {
            using (var db = new MircoShopEntities())
            {
                return db.Members.Count();
            }
        }

        public double GetUserPageCount(int size=10) 
        {
            var total = GetUserCount();
            return Math.Ceiling((double)total /size);
        }

        public MemberViewModel GetWechatUser(string OpenId)
        {
            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.OpenId == OpenId);
                var model = new MemberViewModel();
                if (m != null)
                {
                    model.MemberId = m.MemberId;
                    model.OpenId = m.OpenId;
                    model.NickName = m.NickName;
                    model.Country = m.Country;
                    model.UserName = m.UserName;
                    model.Mobile = m.Mobile;
                    model.Email = m.Email;
                    model.QQ = m.QQ;
                    model.Birth = m.Birth;
                    model.City = m.City;
                    model.Province = m.Province;
                    model.HeadImgUrl = m.HeadImgUrl;
                    model.follow = m.follow;
                    model.balance = m.balance;
                    model.Cashbalance = m.Cashbalance;
                    model.fsate = Convert.ToInt32(m.fsate);
                    model.ISVip = Convert.ToInt32(m.ISVip);
                    model.TotalIn =  Convert.ToDecimal(m.TotalIn);
                    model.integral = Convert.ToDecimal(m.integral);
                }
                return model;
            }
        }

        public IEnumerable<MemberViewModel> GetMemberList(out int total,int page = 1, int size = 10)
        {
            StringBuilder strSql = new StringBuilder();
            var pageindex = size * (page - 1);
            strSql.AppendFormat(" select top {0}  a.MemberId,a.OpenId,a.NickName,a.Gender,a.City,a.Province,a.Country,a.HeadImgUrl,a.Mobile,a.Email,(CASE WHEN a.Gender = 1 THEN '男'  WHEN a.Gender = 2 THEN '女'      ELSE '未知' END) as Sex from Member as a where a.MemberId NOT IN (SELECT TOP {1} b.MemberId FROM Member as b ORDER BY b.MemberId)  ORDER BY a.MemberId ", size, pageindex);
            using (var db = new MircoShopEntities())
            {
                total = db.Members.Count();
                return db.Database.SqlQuery<MemberViewModel>(strSql.ToString());
            }
        }
        
        public ResultViewModel<IEnumerable<ComCloudShop.Service.Leave>> GetLeave(int page, int size)
        {
            var result = new ResultViewModel<IEnumerable<ComCloudShop.Service.Leave>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Leaves
                                     orderby a.AddTime select a
                                     ).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Leaves
                                    orderby a.AddTime
                                    select a
                                    ).Count();

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
        /// 获取用户申请列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminWithdrawalsListViewModel>> GetMemberWithdrawalsListNew(int page, int size, string nickName,int State)
        {
            var result = new ResultViewModel<IEnumerable<AdminWithdrawalsListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Withdrawals
                                     join p in db.Members on a.MemberID equals p.MemberId
                                     where (State==3?1==1:a.State == State)
                                     orderby a.ID descending
                                     select new AdminWithdrawalsListViewModel
                                     {
                                         ID = a.ID,
                                         MemberID = (int)a.MemberID,
                                         Type = a.Type,
                                         Number = a.Number,
                                         Price = (decimal)a.Price,
                                         AddTime = a.AddTime.ToString(),
                                         DZTime = a.DZTime.ToString(),
                                         State= (int)a.State,
                                         Remack=a.Remack,
                                         UserName=p.NickName,
                                         Moblie=p.Mobile,
                                         TrueName=a.TrueName,
                                         Bank=a.Bank,
                                         Phone=a.Phone
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Withdrawals
                                    join p in db.Members on a.MemberID equals p.MemberId
                                    where (a.State == State)
                                    orderby a.ID descending
                                    select new AdminWithdrawalsListViewModel
                                    {
                                        ID = a.ID,
                                        MemberID = (int)a.MemberID,
                                        Type = a.Type,
                                        Number = a.Number,
                                        Price = (decimal)a.Price,
                                        AddTime = a.AddTime.ToString(),
                                        DZTime = a.DZTime.ToString(),
                                        State = (int)a.State,
                                        Remack = a.Remack,
                                        UserName = p.UserName
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
        /// 获取用户申请列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminWithdrawalsListViewModel>> GetMemberWithdrawalsListNewByManger(int page, int size, string nickName, int State)
        {
            var result = new ResultViewModel<IEnumerable<AdminWithdrawalsListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Withdrawals
                                     join p in db.Mangers on a.MemberID equals p.ID
                                     where (State == 3 ? 1 == 1 : a.State == State)
                                     orderby a.ID descending
                                     select new AdminWithdrawalsListViewModel
                                     {
                                         ID = a.ID,
                                         MemberID = (int)a.MemberID,
                                         Type = a.Type,
                                         Number = a.Number,
                                         Price = (decimal)a.Price,
                                         AddTime = a.AddTime.ToString(),
                                         DZTime = a.DZTime.ToString(),
                                         State = (int)a.State,
                                         Remack = a.Remack,
                                         UserName = p.UserName,
                                         Moblie = "",
                                         TrueName = a.TrueName,
                                         Bank = a.Bank,
                                         Phone = a.Phone
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Withdrawals
                                    join p in db.Mangers on a.MemberID equals p.ID
                                    where (a.State == State)
                                    orderby a.ID descending
                                    select new AdminWithdrawalsListViewModel
                                    {
                                        ID = a.ID,
                                        MemberID = (int)a.MemberID,
                                        Type = a.Type,
                                        Number = a.Number,
                                        Price = (decimal)a.Price,
                                        AddTime = a.AddTime.ToString(),
                                        DZTime = a.DZTime.ToString(),
                                        State = (int)a.State,
                                        Remack = a.Remack,
                                        UserName = p.UserName
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
        /// 获取用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminMemberListViewModel>> GetMemberListNew1(int page, int size, string nickName, string mobile, string openid, int isvip,string follow)
        {
            var result = new ResultViewModel<IEnumerable<AdminMemberListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Members
                                     where (string.IsNullOrEmpty(nickName) ? 1 == 1 : a.NickName.Contains(nickName)) &&
                                     (string.IsNullOrEmpty(follow) ? 1 == 1 : a.follow== follow) &&
                                               (string.IsNullOrEmpty(mobile) ? 1 == 1 : a.Mobile.Contains(mobile)) &&
                                               (string.IsNullOrEmpty(openid) ? 1 == 1 : a.OpenId.Contains(openid)) &&
                                               (isvip == 0 ? 1 == 1 : a.ISVip == isvip)
                                     orderby a.MemberId descending
                                     select new AdminMemberListViewModel
                                     {
                                         MemberId = a.MemberId,
                                         OpenId = a.OpenId,
                                         NickName = a.NickName,
                                         HeadImgUrl = a.HeadImgUrl,
                                         Gender = a.Gender,
                                         Province = a.Province,
                                         City = a.City,
                                         Mobile = a.Mobile,
                                         IsVip = (int)a.ISVip
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Members
                                    where (string.IsNullOrEmpty(nickName) ? 1 == 1 : a.NickName.Contains(nickName)) &&
                                      (string.IsNullOrEmpty(follow) ? 1 == 1 : a.follow == follow) &&
                                              (string.IsNullOrEmpty(mobile) ? 1 == 1 : a.Mobile.Contains(mobile))
                                    orderby a.MemberId descending
                                    select new AdminMemberListViewModel
                                    {
                                        MemberId = a.MemberId
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
        /// 获取用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ResultViewModel<IEnumerable<AdminMemberListViewModel>> GetMemberListNew(int page, int size, string nickName, string mobile,string openid,int isvip)
        {
            var result = new ResultViewModel<IEnumerable<AdminMemberListViewModel>>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Members
                                     where (string.IsNullOrEmpty(nickName) ? 1 == 1 : a.NickName.Contains(nickName)) &&
                                               (string.IsNullOrEmpty(mobile) ? 1 == 1 : a.Mobile.Contains(mobile)) &&
                                               (string.IsNullOrEmpty(openid) ? 1 == 1 : a.OpenId.Contains(openid)) &&
                                               (isvip == 0 ? 1 == 1 : a.ISVip == isvip )
                                     orderby a.MemberId descending
                                     select new AdminMemberListViewModel
                                     {
                                         MemberId = a.MemberId,
                                         OpenId = a.OpenId,
                                         NickName = a.NickName,
                                         HeadImgUrl = a.HeadImgUrl,
                                         Gender = a.Gender,
                                         Province = a.Province,
                                         City = a.City,
                                         Mobile = a.Mobile,
                                         IsVip=(int)a.ISVip
                                     }).Skip((page - 1) * size).Take(size).ToList();

                    result.total = (from a in db.Members
                                    where (string.IsNullOrEmpty(nickName) ? 1 == 1 : a.NickName.Contains(nickName)) &&
                                              (string.IsNullOrEmpty(mobile) ? 1 == 1 : a.Mobile.Contains(mobile))
                                    orderby a.MemberId descending
                                    select new AdminMemberListViewModel
                                    {
                                        MemberId = a.MemberId
                                    }).Count();

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

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public ResultViewModel<AdminMemberDetailViewModel> GetMemberDetailNew(int memberID)
        {
            var result = new ResultViewModel<AdminMemberDetailViewModel>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    result.result = (from a in db.Members
                                     where a.MemberId == memberID
                                     select new AdminMemberDetailViewModel
                                     {
                                         MemberId = a.MemberId,
                                         OpenId = a.OpenId,
                                         Birth = a.Birth,
                                         City = a.City,
                                         ContactAddr = a.ContactAddr,
                                         Country = a.Country,
                                         District = a.District,
                                         Email = a.Email,
                                         Gender = a.Gender,
                                         HeadImgUrl = a.HeadImgUrl,
                                         LastLoginDate = (int)a.LastLoginDate,
                                         Mobile = a.Mobile,
                                         NickName = a.NickName,
                                         OrignKey = a.OrignKey,
                                         Province = a.Province,
                                         QQ = a.QQ,
                                         UserName = a.UserName
                                     }).FirstOrDefault();
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
        

    }
}
