using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComCloudShop.Backend.Controllers
{
    public class SetSupController : BaseController
    {
        //
        // GET: /SetSup/
        public ActionResult Index()
        {
            SetSupService s = new SetSupService();
            SetSupViewModel model = s.Read();
            if (model == null) {
                model = new SetSupViewModel();
                model.SetBL = "0";
            }
            return View(model);
        }

        public ActionResult GetBL() {
            SetSupService s = new SetSupService();
            SetSupViewModel model = s.Read();
            if (model == null)
            {
                model = new SetSupViewModel();
                model.SetBL = "0";
            }
            return Content(model.SetBL);
        }

        public ActionResult Edit() {
            
            SetSupService s = new SetSupService();
            string id = Request["id"];
            if (s.Write(id))
            {
                return Content("ok");
            }
            else {
                return Content("err");
            }
        }

        //
        // GET: /SetSup/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SetSup/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SetSup/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        

        //
        // GET: /SetSup/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SetSup/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
