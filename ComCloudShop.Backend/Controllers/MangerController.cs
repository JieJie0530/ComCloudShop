using ComCloudShop.Layer;
using ComCloudShop.Service;
using ComCloudShop.Utility;
using ComCloudShop.Utility.Helper;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Backend.Controllers
{
    public class MangerController : BaseController
    {
        //
        // GET: /Manger/

        public ActionResult Index()
        {
            return View();
        }
        MircoShopEntities db = new MircoShopEntities();
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="status">1：品种；2：功能；</param>
        /// <param name="page">当前页</param>
        /// <returns></returns>ge
        [HttpGet]
        public JsonResult List(int status, int page = 1)
        {
            var result = new ResultViewModel<List<ComCloudShop.Service.Manger>>();
            try
            {
                result.result = db.Mangers.OrderBy(d=>d.ID).Skip((page - 1) * 10).Take(10).ToList();

                result.total = db.Mangers.Count();

                result.error = (int)ErrorEnum.OK;
                result.msg = "success";
            }
            catch (Exception ex)
            {
                result.error = (int)ErrorEnum.Error;
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OkTuiKuan() {
            int id = Convert.ToInt32(Request["ID"]);
            ComCloudShop.Service.Manger m= db.Mangers.Where(d => d.ID == id).FirstOrDefault();
            m.State = 1;
            db.SaveChanges();
            return Content("ok");
        }

        /// <summary>
        /// 添加/修改 品种/功能
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(ComCloudShop.Service.Manger data)
        {
            var result = new ResultViewModel<bool>();
            try
            {
                using (var db = new MircoShopEntities())
                {
                    var model = new ComCloudShop.Service.Manger();
                    if (data.ID > 0)
                    {
                        model = db.Mangers.FirstOrDefault(x => x.ID == data.ID);
                        model.UserName = data.UserName;
                        model.Pwd = data.Pwd;
                        model.ShopAddress = data.ShopAddress;
                        model.ShopName = data.ShopName;
                        model.IsRecommend = data.IsRecommend;
                        model.Category = data.Category;
                        model.OpenID = data.OpenID;
                        model.Proportion = data.Proportion;

                        model.Contacts = data.Contacts;
                        model.Phone = data.Phone;
                        model.Business = data.Business;
                        model.Introduce = data.Introduce;
                        model.license = data.license;
                        model.Storefront = data.Storefront;
                        model.Lat = data.Lat;
                        model.Lng = data.Lng;
                    }
                    else
                    {
                        model.UserName = data.UserName;
                        model.Pwd = data.Pwd;
                        model.ShopAddress = data.ShopAddress;
                        model.ShopName = data.ShopName;
                        model.IsRecommend = data.IsRecommend;
                        model.Category = data.Category;
                        model.OpenID ="";
                        model.balance = 0;
                        model.Proportion = 0;
                        model.Contacts = data.Contacts;
                        model.Phone = data.Phone;
                        model.Business = data.Business;
                        model.Introduce = data.Introduce;
                        model.license = data.license;
                        model.Storefront = data.Storefront;
                        model.Lat = data.Lat;
                        model.Lng = data.Lng;
                        db.Mangers.Add(model);
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Del()
        {
            int id = Convert.ToInt32(Request["ID"]);
            using (var db = new MircoShopEntities())
            {
                Manger m = db.Mangers.Where(d => d.ID == id).FirstOrDefault();
                db.Mangers.Remove(m);
                
                string ids = id.ToString();
                List<IntList> list = db.IntLists.Where(d => d.ManagerID == ids).ToList();
                db.IntLists.RemoveRange(list);
                db.SaveChanges();
                return Content("ok");
            }
            return Content("err");
        }

        public ActionResult Withd()
        {
            return View();
        }
        protected MemberService _service = new MemberService();
        public ActionResult listWithd(string nickName, int State, int page = 1)
        {
            var list = _service.GetMemberWithdrawalsListNewByManger(page, AppConstant.PageSize, nickName, State);
            return Json(list, JsonRequestBehavior.AllowGet); 
        }


        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImg(FormCollection collection)//HttpPostedFileBase upImg
        {
            //Request.Files["goods_image"];
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase upImg = files["upImg"];
            string fileName = System.IO.Path.GetFileName(upImg.FileName);
            long name = TimestampUtil.GenerateTimeStamp(DateTime.Now);
            string pic = "", error = "";
            //判断文件格式
            string filetype = fileName.Substring(fileName.LastIndexOf('.')).ToUpper();
            if ((fileName.LastIndexOf('.') > -1 && (filetype == ".JPG" || filetype == ".PNG" || filetype == ".JPEG")))
            {
                try
                {
                    string filePhysicalPath = Server.MapPath("~/upload/sale/" + name + filetype);
                    upImg.SaveAs(filePhysicalPath);
                    pic = "upload/sale/" + name + filetype;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            else
            {
                error = "上传的文件格式不符合要求";
            }
            return Json(new
            {
                pic = pic,
                error = error
            });
        }
    }
}
