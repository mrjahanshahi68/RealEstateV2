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
    public class MessageController : BaseApiController<Message, MessageVM>
    {
		protected override IBusinessRule<Message> CreateRule() => new MessageBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetMessages(FilterContainer filter)
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
	}
}
