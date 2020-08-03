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
    public class BlogController : BaseApiController<Blog, BlogVM>
    {
		protected override IBusinessRule<Blog> CreateRule() => new BlogBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetBlogs(FilterContainer filter)
		{
			try
			{
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var query = BusinessRule.Queryable();
				if (filter.OrderBy == null)
				{
					filter.OrderBy = new List<OrderFilter>
					{
						new OrderFilter
						{
							Field="InsertDateTime",
						    Order = OrderFilterType.Desc
						}
					};
				}
				query = query.Request(filter);
				var totalCount = await query.CountAsync();
				var result =await query.ToListAsync();
				return Success(new FilterQueryRsponse
				{
					TotalCount = totalCount,
					Records = result,
				});
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion

		#region Validations
		protected override List<string> InsertValidation(BlogVM viewmodel)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(viewmodel.Title))
				errors.Add(string.Format(MessageTemplate.Required, " عنوان "));
			if (string.IsNullOrWhiteSpace(viewmodel.Body))
				errors.Add(string.Format(MessageTemplate.Required, " متن "));
			if (string.IsNullOrWhiteSpace(viewmodel.Summery))
				errors.Add(string.Format(MessageTemplate.Required, " خلاصه  "));
			if (!viewmodel.CategoryId.HasValue)
				errors.Add(string.Format(MessageTemplate.Required, " دسته بندی "));

			var entityByName = BusinessRule.Queryable()
				.SingleOrDefault(c => c.Title == viewmodel.Title);
			if (entityByName != null)
				errors.Add(string.Format(MessageTemplate.Repetitious));

			viewmodel.PublishDateTime = DateTime.Now;

			return errors;

		}

		protected override List<string> UpdateValidation(BlogVM viewmodel)
		{
			var errors = new List<string>();
			if (viewmodel.ID <= 0)
				errors.Add("کد شناسته نامعتبر است");
			var entity = BusinessRule.FindEntity(viewmodel.ID);
			if (entity == null) throw new Exception("رکورد مورد نظر یافت نشد");
			if (string.IsNullOrWhiteSpace(viewmodel.Title))
				errors.Add(string.Format(MessageTemplate.Required, " عنوان "));
			if (string.IsNullOrWhiteSpace(viewmodel.Body))
				errors.Add(string.Format(MessageTemplate.Required, " متن "));
			if (string.IsNullOrWhiteSpace(viewmodel.Summery))
				errors.Add(string.Format(MessageTemplate.Required, " خلاصه  "));
			if (viewmodel.CategoryId == 0)
				errors.Add(string.Format(MessageTemplate.Required, " دسته بندی "));

			var entityByName = BusinessRule.Queryable()
				.SingleOrDefault(c => c.ID != viewmodel.ID && c.Title == viewmodel.Title);
			if (entityByName != null)
				errors.Add(string.Format(MessageTemplate.Repetitious));

			return errors;
		}
		#endregion
	}
}
