using RealEstate.Common.Entities.Customer;
using RealEstate.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.Domain;
using RealEstate.Domain.Customer;
using System.Threading.Tasks;
using QueryDesigner;
using System.Net.Http;
using RealEstate.Common.Exceptions;
using static RealEstate.Common.AppConstants;
using System.Data.Entity;

namespace RealEstate.Web.Controllers.Api
{
	public class CustomerInfoController : BaseApiController<CustomerInfo,CustomerInfoVM>
	{
		protected override IBusinessRule<CustomerInfo> CreateRule()
		{
			return new CustomerInfoBusinessRule();
		}
		public async Task<HttpResponseMessage> GetCsutomersName(FilterContainer filter)
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
						}
					};
				}
				query = query.Request(filter);
				var result = await query.ToListAsync();
				return Success();
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
	}
}