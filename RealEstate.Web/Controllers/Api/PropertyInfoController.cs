using RealEstate.Common.Entities.Property;
using RealEstate.Web.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Property;
using System.Threading.Tasks;
using RealEstate.DataAccess;
using RealEstate.Web.Models.Common;
using System.Data.Entity;
using RealEstate.Common.Entities.Common;

namespace RealEstate.Web.Controllers.Api
{
    public class PropertyInfoController : BaseApiController<PropertyInfo,PropertyInfoVM>
    {
		protected override IBusinessRule<PropertyInfo> CreateRule()
		{
			return new PropertyInfoBusinessRule();
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<HttpResponseMessage> InitializeParameters()
		{
			try
			{
				using(var uow=new AppUnitOfWork())
				{
					var propertyTypes = await uow.Repository<PropertyType>().Queryable().Select(e => new KeyValueVM{Key = e.ID,Value = e.Name}).ToListAsync();
					var documentTypes = await uow.Repository<DocumentType>().Queryable().Select(e => new KeyValueVM { Key = e.ID, Value = e.Name }).ToListAsync();
					var welfares = await uow.Repository<Welfare>().Queryable().Select(e => new KeyValueVM { Key = e.ID, Value = e.Name }).ToListAsync();
					var stateQuery = uow.Repository<State>().Queryable();
					return Success(new 
					{
						PropertyTypes=propertyTypes,
						DocumentTypes=documentTypes,
						Welfares=welfares,
					});
				}
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
	}
}
