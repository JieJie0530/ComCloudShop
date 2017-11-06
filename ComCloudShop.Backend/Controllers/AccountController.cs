using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Utility;
using System.Text;

namespace ComCloudShop.Backend.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            //Func<IEnumerable<IComparable>, IEnumerable<IComparable>> quick_sort = null;
            //quick_sort = new Func<IEnumerable<IComparable>, IEnumerable<IComparable>>(l => l.Count() <= 1 ? l : quick_sort(l.Skip(1).Where(t => t.CompareTo(l.ElementAt(0)) < 0)).Concat(new IComparable[] { l.ElementAt(0) }).Concat(quick_sort(l.Skip(1).Where(t => t.CompareTo(l.ElementAt(0)) >= 0))));

            //List<int> t2 = new List<int>();
            //t2.Add(1);
            //t2.Add(2);
            //t2.Add(5);
            //t2.Add(4);
            //t2.Add(7);
            //IEnumerable<IComparable> test3 = t2.Cast<IComparable>();
            //var list = quick_sort(test3);
            //var s = string.Empty;
            //list.ToList().ForEach(x => s += x + ",");
            //return Content(s);
            ////var h = string.Empty;
            ////var m = EncryptHelper.Encrypt3DESToHexString("http://localhost:10787/account/login");
            ////h += "加密：" + m;
            ////h += "解密：" + EncryptHelper.Decrypt3DESFromHexString(m);
            ////var b = EncryptHelper.EncodeUrlSafeBase64(m);
            ////h += " Base64加密：" + b;
            ////h += " Base64解密：" + EncryptHelper.DecodeUrlSafeBase64(b);
            ////h += " SHA1加密：" + EncryptHelper.SHA1("https://www.baidu.com/s?ie=utf-8&f=3&rsv_bp=1&rsv_idx=1&tn=baidu&wd=ExpressionVisitor&oq=TortoiseGit&rsv_pq=9580ff980001b32a&rsv_t=3ed8uqWP5oQGoYzCDgiRk%2FPDv4awqEZb7uUWpPiNMNHMqy6fxxSgYulEXcA&rsv_enter=0&inputT=815&rsv_n=2&rsv_sug3=14&prefixsug=%3ExpressionVisitor&rsp=0&rsv_sug4=815");
            ////return Content(h);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string pwd)
        {
            if (username == "liaojinhui" && EncryptHelper.MD5(pwd, 32) == "26821956bc917fbce9474ae173acaaa8")
            {
                AddUserAuth(username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Response.Write("<script>alert('账号或者密码错误，请重新输入');</script>");
            }
            return View();
        }

        public void Logout()
        {
            ClearUserAuth();
        }

        public ActionResult Test()
        {
            Func<IEnumerable<IComparable>, IEnumerable<IComparable>> quick_sort = null;
            quick_sort = new Func<IEnumerable<IComparable>, IEnumerable<IComparable>>(l => l.Count() <= 1 ? l : quick_sort(l.Skip(1).Where(t => t.CompareTo(l.ElementAt(0)) < 0)).Concat(new IComparable[] { l.ElementAt(0) }).Concat(quick_sort(l.Skip(1).Where(t => t.CompareTo(l.ElementAt(0)) >= 0))));

            List<int> t2 = new List<int>();
            t2.Add(1);
            t2.Add(2);
            t2.Add(5);
            t2.Add(4);
            t2.Add(7);
            IEnumerable<IComparable> test3 = t2.Cast<IComparable>();
            var list = quick_sort(test3);
            var s=string.Empty;
            list.ToList().ForEach(x => s+=x+",");
            return Content(s);
        }

       
    }
}
