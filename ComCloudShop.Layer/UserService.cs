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
    public class UserService:BaseService
    {
        
 public bool AddIntList(IntList w)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    db.IntLists.Add(w);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddWithd(Withdrawal w) {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    db.Withdrawals.Add(w);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Withdrawal> GetUserWithdrawalList(int memberID) {
            using (db)
            {
                var data = db.Withdrawals.Where(x => x.MemberID == memberID).OrderByDescending(x => x.AddTime).ToList();
                
                return data;
            }
            return null;
        }

        /// <summary>
        /// 获取用户首页数据
        /// </summary>
        /// <param name="memberId">用户id</param>
        /// <returns></returns>
        public UserIndexViewModel GetUserIndexInfo(int memberId)
        {
            var model = new UserIndexViewModel();
            try
            {
                using (db)
                {
                    var data = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                    if (data != null)
                    {
                        model.username = data.NickName;
                        model.url = data.HeadImgUrl;
                        model.openid = data.OpenId;
                        model.phone = data.Mobile;
                        model.number = 0;
                        model.follow = data.follow;
                        model.balance = data.balance;
                        model.Cashbalance = data.Cashbalance;
                    }
                    return model;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateMember(Member model) {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var m = db.Members.FirstOrDefault(x => x.MemberId == model.MemberId);
                    m.TotalIn = model.TotalIn;
                    m.fsate = model.fsate;
                    m.follow = model.follow;
                    m.integral = model.integral;
                    m.Cashbalance = model.Cashbalance;
                    m.balance = model.balance;
                    m.OrignKey = model.OrignKey;
                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public List<Member> GetMemberFollow(int memberID)
        {
            List<Member> list = db.Members.Where(x => x.follow == memberID.ToString() && x.fsate == 2).ToList();
            return list;
        }
        public Member GetMemberBID(int memberID)
        {
            Member data = db.Members.FirstOrDefault(x => x.MemberId == memberID);
            return data;
        }

        public Member GetMemberByOpenID(string openid) {
            Member data = db.Members.FirstOrDefault(x => x.OpenId == openid);
            return data;
        }

        /// <summary>
        /// 获取用户个人资料信息
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <returns></returns>
        public UserEditInfoViewModel GetUserInfoByID(int id)
        {
            var model = new UserEditInfoViewModel();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data = db.Members.FirstOrDefault(x => x.MemberId == id);
                    model.Address = data.ContactAddr;
                    model.Birthday = data.Birth;
                    model.City = data.City;
                    model.Province = data.Province;
                    model.QQ = data.QQ;
                    model.Name = data.UserName;
                    model.Mobile = data.Mobile;
                    model.MemberId = data.MemberId;
                    model.Email = data.Email;
                    model.District = data.District;
                }
            }
            catch
            {

            }
            return model;
        }

        /// <summary>
        /// 获取用户个人资料信息
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <returns></returns>
        public async Task<UserEditInfoViewModel> GetUserInfo(int id)
        {
            var model = new UserEditInfoViewModel();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var data =await Task.Run(() => db.Members.FirstOrDefault(x => x.MemberId == id));
                    model.Address = data.ContactAddr;
                    model.Birthday = data.Birth;
                    model.City = data.City;
                    model.Province = data.Province;
                    model.QQ = data.QQ;
                    model.Name = data.UserName;
                    model.Mobile = data.Mobile;
                    model.MemberId = data.MemberId;
                    model.Email = data.Email;
                    model.District = data.District;
                }
            }
            catch
            {

            }
            return model;
        }


        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateVip(int id)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.Members.FirstOrDefault(x => x.MemberId == id);
                    model.ISVip = 2;
                    model.integral += 100000; //增加1000购物积分
                    model.Cashbalance = (Convert.ToDecimal(model.Cashbalance)+ 1000000).ToString();//增加3000体现额度
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
        /// 更新个人信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Edit(UserEditInfoViewModel data)
        {
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = db.Members.FirstOrDefault(x => x.MemberId == data.MemberId);

                    model.MemberId = data.MemberId;
                    model.UserName = data.Name;
                    model.Mobile = data.Mobile;
                    model.Email = data.Email;
                    model.QQ = data.QQ;
                    model.Birth = (DateTime.Parse(data.Birthday)).ToString("yyyy-MM-dd");
                    model.Province = data.Province;
                    model.City = data.City;
                    model.District = data.District;
                    model.ContactAddr = data.Address;

                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


    }
}
