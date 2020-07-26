using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers.View
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View(DateTime.Now);
        }
    }
}