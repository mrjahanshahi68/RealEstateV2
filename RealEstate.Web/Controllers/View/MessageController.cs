using RealEstate.Common.Entities.Common;
using RealEstate.DataAccess;
using RealEstate.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using static RealEstate.Common.AppConstants;

namespace RealEstate.Web.Controllers.View
{
    public class MessageController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult ContactUs()
        {
            var model = new MessageVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult SendMessage(MessageVM model)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(model.FullName))
                errors.Add(string.Format(MessageTemplate.Required, " نام و نام خانوادگی "));
            if (string.IsNullOrWhiteSpace(model.Email))
                errors.Add(string.Format(MessageTemplate.Required, " پست الکترونیک "));
            if (string.IsNullOrWhiteSpace(model.Text))
                errors.Add(string.Format(MessageTemplate.Required, " توضیحات  "));

            var message = new Message
            {
                Email = model.Email,
                Mobile = model.Mobile,
                FullName = model.FullName,
                Text = model.Text,
                IsDeleted =false,
                InsertDateTime = DateTime.Now
                
            };

            if (errors != null && errors.Any())
            {
                return Json(new { success = false,errors = errors });
            }
            else
            {
                using (var uow = new AppUnitOfWork())
                {
                    var query = uow.Repository<Message>();
                    query.Insert(message);

                    uow.SaveChange();
                }

                return Json(new { success = true });
            }
        }
    }
}