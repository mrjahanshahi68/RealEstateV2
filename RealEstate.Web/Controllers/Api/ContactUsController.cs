using RealEstate.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Common;
using System.Threading.Tasks;
using RealEstate.Web.Security.Filters;
using RealEstate.Web.Models.Common;
using static RealEstate.Common.AppConstants;
using QueryDesigner;
using System.Data.Entity;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Models;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
    public class ContactUsController : BaseApiController<ContactUs, ContactUsVM>
    {
		protected override IBusinessRule<ContactUs> CreateRule() => new ContactUsBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetContactUs()
		{
			try
			{
			
				var result =await BusinessRule.Queryable().OrderByDescending(e=>e.ID).FirstOrDefaultAsync();
				return Success(result);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion

		#region Validations

		protected override List<string> UpdateValidation(ContactUsVM viewmodel)
		{
			var errors = new List<string>();
			if (viewmodel.ID <= 0)
				errors.Add("کد شناسته نامعتبر است");
			var entity = BusinessRule.FindEntity(viewmodel.ID);
			if (entity == null) throw new Exception("رکورد مورد نظر یافت نشد");
			if (string.IsNullOrWhiteSpace(viewmodel.MobileNumber))
				errors.Add(string.Format(MessageTemplate.Required, " شماره موبایل "));

			if (string.IsNullOrWhiteSpace(viewmodel.PhoneNumber))
				errors.Add(string.Format(MessageTemplate.Required, " تلفن ثابت "));

			if (string.IsNullOrWhiteSpace(viewmodel.Address))
				errors.Add(string.Format(MessageTemplate.Required, " آدرس "));

			return errors;
		}
		#endregion
	}
}
