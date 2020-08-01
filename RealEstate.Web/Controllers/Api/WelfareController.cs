using RealEstate.Common.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Property;
using System.Threading.Tasks;
using RealEstate.Web.Security.Filters;
using RealEstate.Web.Models.Property;
using static RealEstate.Common.AppConstants;
using QueryDesigner;
using System.Data.Entity;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Models;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
    public class WelfareController : BaseApiController<Welfare, WelfareVM>
    {
		protected override IBusinessRule<Welfare> CreateRule() => new WelfareBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetWelfares(FilterContainer filter)
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
							Field="ID",
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
		protected override List<string> InsertValidation(WelfareVM viewmodel)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(viewmodel.Name))
				errors.Add(string.Format(MessageTemplate.Required, " عنوان "));
			var entityByName = BusinessRule.Queryable()
				.SingleOrDefault(c => c.Name == viewmodel.Name);

			if (entityByName != null)
				errors.Add(string.Format(MessageTemplate.Repetitious));

			return errors;

		}

		protected override List<string> UpdateValidation(WelfareVM viewmodel)
		{
			var errors = new List<string>();
			if (viewmodel.ID <= 0)
				errors.Add("کد شناسته نامعتبر است");
			var entity = BusinessRule.FindEntity(viewmodel.ID);
			if (entity == null) throw new Exception("رکورد مورد نظر یافت نشد");
			if (string.IsNullOrWhiteSpace(viewmodel.Name))
				errors.Add(string.Format(MessageTemplate.Required, " عنوان "));

			var entityByName = BusinessRule.Queryable()
				.SingleOrDefault(c=> c.ID != viewmodel.ID && c.Name == viewmodel.Name);

			if (entityByName != null)
				errors.Add(string.Format(MessageTemplate.Repetitious));

			return errors;
		}
		#endregion
	}
}
