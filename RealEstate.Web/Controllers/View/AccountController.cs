﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Controllers.View
{
    public class AccountController : Controller
    {
		// GET: Account
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ChangePassWord()
		{
			return View();
		}
	}
}