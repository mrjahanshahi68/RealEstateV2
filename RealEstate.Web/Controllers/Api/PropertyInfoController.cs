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
using static RealEstate.Common.AppEnums;

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
					var states = await uow.Repository<State>().Queryable().ToListAsync();
					//var cities= await uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City).ToListAsync();
					//var regions= await uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region).ToListAsync();

					return Success(new 
					{
						PropertyTypes=propertyTypes,
						DocumentTypes=documentTypes,
						Welfares=welfares,
						States= states,
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
