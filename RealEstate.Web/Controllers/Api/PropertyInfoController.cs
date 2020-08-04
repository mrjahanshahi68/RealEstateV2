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
using static RealEstate.Common.AppConstants;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Security.Filters;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
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

		[HttpPost]
		public async Task<HttpResponseMessage> SubmitProperty(PropertyInfoVM parameters)
		{
			try
			{
				var request =await Request.Content.ReadAsMultipartAsync();
				//var xxxx = request.Contents[0];
				foreach (var content in request.Contents)
				{
					var dataAsArray =await content.ReadAsByteArrayAsync();
					var key = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(content.Headers.ContentDisposition.Name);
					if (key == "SlideImage")
					{
						var contentType = content.Headers.ContentType;
						var fileName = Newtonsoft.Json.JsonConvert.DeserializeObject(content.Headers.ContentDisposition.FileName);
						if (contentType.MediaType == "image/jpeg")
						{
							System.IO.File.WriteAllBytes(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Common.AppConfigurations.PropertyImageFolder, fileName + ".jpg"), dataAsArray);
						}
					}

					
					
				}
				//using(var ms=new System.IO.MemoryStream(arr))
				//{
				//	var image = System.Drawing.Image.FromStream(ms);
				//	var contentType = xxxx.Headers.ContentType;
				//	if(contentType.MediaType== "image/jpeg")
				//	{
				//		var z = Newtonsoft.Json.JsonConvert.DeserializeObject(xxxx.Headers.ContentDisposition.Name);
				//		if (z.ToString() == "SlideImage")
				//		{
				//			var fileName = Newtonsoft.Json.JsonConvert.DeserializeObject(xxxx.Headers.ContentDisposition.FileName);
				//			System.IO.File.WriteAllBytes(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Common.AppConfigurations.PropertyImageFolder, fileName + ".jpg"), arr);
				//		}
						
				//	}
				//}
				var request1 = await Request.Content.ReadAsStringAsync();
				var request2 = await Request.Content.ReadAsFormDataAsync();
				var request3 = await Request.Content.ReadAsFormDataAsync();
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);
				return Success();
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

	}
}
