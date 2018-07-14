using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class MBController : Controller
    {
        // GET: MB
        public ActionResult Index()
        {
            ViewData.Model = "Hello ViewData.Model";
            return View();
        }

        public ActionResult ViewDataDemo()
        {
            ViewData["Demo"] = "Hello ViewData[Demo]";

            return View();
        }

        public ActionResult ViewBagDemo()
        {
            ViewBag.Demo = "Hello ViewBag.Demo";

            return View();
        }

        public ActionResult TempDataSave()
        {
            TempData["Demo"] = "Hello ViewData[Demo]";

            return RedirectToAction("TempDataDemo");
        }

        public ActionResult TempDataDemo()
        {

            return View();
        }
    }
}