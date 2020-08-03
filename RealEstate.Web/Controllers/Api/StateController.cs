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
using static RealEstate.Common.AppEnums;
using System.Data.Entity;
using QueryDesigner;
using static RealEstate.Common.AppConstants;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Models;
using RealEstate.Web.Security.Filters;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
	public class StateController : BaseApiController<State,State>
    {
		protected override IBusinessRule<State> CreateRule()
		{
			return new StateBusinessRule();
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<HttpResponseMessage> GetStatesAndCities()
		{
			try
			{
				var results=await BusinessRule.Queryable().Where(e => e.Level != (int)Level.Region).ToListAsync();
				return Success(results);
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<HttpResponseMessage> GetRegions(FilterContainer filter)
		{
			try
			{
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var query = BusinessRule.Queryable().Where(e => e.Level == (int)Level.Region);
				if (filter.OrderBy == null)
				{
					filter.OrderBy = new List<OrderFilter>
					{
						new OrderFilter
						{
							Field="ID",
						}
					};
				}
				query = query.Request(filter);
				var totalCount = await query.CountAsync();
				var result = await query.ToListAsync();
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
	}
}
