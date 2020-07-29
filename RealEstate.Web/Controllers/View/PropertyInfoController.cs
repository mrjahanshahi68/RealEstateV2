using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers.View
{
    public class PropertyInfoController : Controller
    {
        // GET: PropertyInfo
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult RegisterProperty()
		{
			return View();
		}
    }
}