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
using RealEstate.Web.Infrastrcuture;
using System.IO;
using System.Web;
using RealEstate.Common;
using QueryDesigner;
using RealEstate.Web.Models;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
	public class PropertyInfoController : BaseApiController<PropertyInfo,PropertyInfoVM>
    {
		private string PropertyFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory , AppConfigurations.PropertyImageFolder);
		public PropertyInfoController()
		{

		}
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
		[Authorize]
		public async Task<HttpResponseMessage> SubmitProperty()
		{
			DbContextTransaction transaction = null;
			try
			{
				var fileDataRequestParameters = HttpContext.Current.Request.GetDataFileRequestParameters<PropertyInfoVM, FileDataModel>();
				var parameters = fileDataRequestParameters.ViewModel;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var errors = new List<string>();
				if (string.IsNullOrWhiteSpace(parameters.PropertyCode))
					errors.Add(string.Format(MessageTemplate.Required, "کدملک"));
				if (string.IsNullOrWhiteSpace(parameters.Title))
					errors.Add(string.Format(MessageTemplate.Required, "عنوان"));
				if (!parameters.Type.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "نوع معامله"));
				if (!parameters.PropertyTypeId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "نوع ملک"));
				if (!parameters.DocumentTypeId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "نوع سند"));
				if(!parameters.StateId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "استان"));
				if (!parameters.CityId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "شهر"));
				if (!parameters.RegionId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "منطقه"));

				if (errors.Any())
					throw new ValidationModelException(errors);

				using(var dbContext=new AppDataContext())
				{

					transaction = dbContext.Database.BeginTransaction();
					using(var uow=new UnitOfWork<AppDataContext>(dbContext))
					{
						var propertyInfoRepo = uow.Repository<PropertyInfo>();
						var attachmentRepo = uow.Repository<Attachment>();
						var coverImageFile = fileDataRequestParameters.Files.Where(e => e.FileData.ExtraInfoString == "CoverImage").SingleOrDefault();
						var slideImageFile = fileDataRequestParameters.Files.Where(e => e.FileData.ExtraInfoString == "SlideImage").SingleOrDefault();
						var entity = Mapper.Map<PropertyInfo>(parameters);
						entity.Welfares = Newtonsoft.Json.JsonConvert.SerializeObject(parameters.Welfares);
						if (entity.ID > 0)
						{
							var dbEntity = propertyInfoRepo.Find(entity.ID);
							if (dbEntity == null)
								throw new ValidationModelException(MessageTemplate.RecordNotFound);
							entity.HashKey = dbEntity.HashKey;
							if (coverImageFile == null)
								entity.CoverImage = dbEntity.CoverImage;
							else
							{
								string coverImagePath = Path.Combine(PropertyFolder, entity.HashKey, $"CoverImage_{entity.CoverImage}");
								if (File.Exists(coverImagePath))
									File.Delete(coverImagePath);
							}
							if (slideImageFile == null)
								entity.SlideImage = dbEntity.SlideImage;
							else
							{
								string slideImagePath = Path.Combine(PropertyFolder, entity.HashKey, $"SlideImage_{entity.CoverImage}");
								if (File.Exists(slideImagePath))
									File.Delete(slideImagePath);
							}
							entity.InsertDateTime = dbEntity.InsertDateTime;
							entity.InsertUserId = dbEntity.InsertUserId;
							entity.UpdateDateTime = DateTime.Now;
							entity.UpdateUserId = CurrentUser.UserId;

							uow.Repository<PropertyInfo>().Update(entity);
						}
						else
						{
							string hashkey = string.Empty;
							while (true)
							{
								hashkey = Guid.NewGuid().ToString();
								if (uow.Repository<PropertyInfo>().Queryable().Where(e => e.HashKey == hashkey).Any()) continue;
								break;
							}
							entity.HashKey = hashkey;
							entity.UrlTitle = entity.Title.AsOptimizeUrl();
							if (coverImageFile != null)
								entity.CoverImage = coverImageFile.HttpPostedFile?.FileName;
							if (slideImageFile != null)
								entity.SlideImage = slideImageFile.HttpPostedFile?.FileName;
							entity.InsertDateTime = DateTime.Now;
							entity.InsertUserId = CurrentUser.UserId;

							uow.Repository<PropertyInfo>().Insert(entity);
						}
						
						await uow.SaveChangeAsync();

						var currentDirectory = Path.Combine(PropertyFolder, entity.HashKey);
						if (!Directory.Exists(currentDirectory))
							Directory.CreateDirectory(currentDirectory);

						var attachments = new List<Attachment>();
						foreach (var item in fileDataRequestParameters.Files)
						{
							var file = item.HttpPostedFile;
							
							
							if (item.FileData?.ExtraInfoString == "CoverImage")
							{
								file.SaveAs($"{currentDirectory}/CoverImage_{file.FileName}");
							}
							else if(item.FileData?.ExtraInfoString == "SlideImage")
							{
								file.SaveAs($"{currentDirectory}/SlideImage_{file.FileName}");
							}
							else if (item.FileData?.ExtraInfoString == "Attachment")
							{
								attachments.Add(new Attachment
								{
									HashKey = Guid.NewGuid().ToString(),
									ContentType = file.ContentType,
									FileName = file.FileName,
									ObjectId = entity.ID,
									ObjectType = ObjectType.Property,
									InsertDateTime = DateTime.Now,
									InsertUserId = CurrentUser.UserId,
								});

								file.SaveAs($"{currentDirectory}/Attachment_{file.FileName}");
							}
						}
						
						foreach (var item in attachments)
							attachmentRepo.Insert(item);
						await uow.SaveChangeAsync();

						transaction.Commit();
					}
				}

				
				return Success();
			}
			catch(Exception ex)
			{
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		public async Task<HttpResponseMessage> GetProperties(FilterContainer filter)
		{
			try
			{
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var propertyTypeQuery = BusinessRule.UnitOfWork.Repository<PropertyType>().Queryable();
				var documentTypeQuery = BusinessRule.UnitOfWork.Repository<DocumentType>().Queryable();
				var propertyQuery= BusinessRule.Queryable();
				var regionQuery = from state in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State)
								  join city in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City) on state.ID equals city.ParentId
								  join region in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region) on city.ID equals region.ParentId
								  select new { state, city, region };
				//var regions = regionQuery.ToList();

				var query = from property in propertyQuery
							join propType in propertyTypeQuery on property.PropertyTypeId equals propType.ID
							join docType in documentTypeQuery on property.DocumentTypeId equals docType.ID
							join region in regionQuery on property.RegionId equals region.region.ID
							select new PropertyInfoVM
							{
								ID=property.ID,
								PropertyTypeId =(int?)property.PropertyTypeId,
								PropertyTypeName= propType.Name,
								DocumentTypeId = property.DocumentTypeId,
								DocumentTypeName= docType.Name,
								HashKey = property.PropertyCode,
								PropertyCode = property.PropertyCode,
								Title = property.Title,
								UrlTitle = property.UrlTitle,
								Type = property.Type,
								BuildingArea =(float?) property.BuildingArea,
								LandingArea = (float?)property.LandingArea,
								BathRoom = property.BathRoom,
								BedRoom = property.BedRoom,
								Floor = property.Floor,
								FloorNumber = property.FloorNumber,
								Apartment = property.Apartment,
								Position = property.Position,
								PropertyView = property.PropertyView,
								ConstructorYear = property.ConstructorYear,
								StateId = region.state.ID,
								CityId = region.city.ID,
								RegionId = property.RegionId,
								RegionName=region.state.Name + DbFunctions.AsUnicode(" ") + region.city.Name + DbFunctions.AsUnicode(" ") + region.region.Name,
								Address = property.Address,
								Description = property.Description,
								SalePrice = property.SalePrice,
								MortgagePrice = property.MortgagePrice,
								RentalPrice = property.RentalPrice,
								FirstName = property.FirstName,
								LastName = property.LastName,
								Tel = property.Tel,
								FullName = property.FirstName + DbFunctions.AsUnicode(" ") + property.LastName,
								ReagentFirstName = property.ReagentFirstName,
								ReagentLastName = property.ReagentLastName,
								ReagentTel = property.ReagentTel,
								ReagentFullName = property.ReagentFirstName+ DbFunctions.AsUnicode(" ") + property.ReagentLastName,
								WelfaresString = property.Welfares,
								CoverImage=property.CoverImage,
								SlideImage=property.SlideImage,
								Status=property.Status,
								RegisterDate=property.InsertDateTime,
								IsDeleted=property.IsDeleted,
								
							};

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
				var totalCount = await query.CountAsync();
				query = query.Request(filter);
				var result = await query.ToListAsync();
				result.ForEach(prop => {
					prop.Welfares = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WelfareVM>>(prop.WelfaresString);
					if (!string.IsNullOrWhiteSpace(prop.CoverImage))
						prop.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/{prop.HashKey}/CoverImage_{prop.CoverImage}";
					if (!string.IsNullOrWhiteSpace(prop.SlideImage))
						prop.SlideImagePath = $"{AppConfigurations.PropertyImageFolder}/{prop.HashKey}/SlideImage_{prop.SlideImage}";
				});

				return Success(new FilterQueryRsponse {
					Records=result,
					TotalCount=totalCount,
				});
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		public async Task<HttpResponseMessage> GetProperty(PropertyInfoVM parameters)
		{
			try
			{
				PropertyInfoVM result = null;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				if (parameters.ID <= 0)
					throw new ValidationModelException(MessageTemplate.InvalidIdentity);

				var propertyTypeQuery = BusinessRule.UnitOfWork.Repository<PropertyType>().Queryable();
				var documentTypeQuery = BusinessRule.UnitOfWork.Repository<DocumentType>().Queryable();
				var propertyQuery = BusinessRule.Queryable();
				var regionQuery = from state in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State)
								  join city in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City) on state.ID equals city.ParentId
								  join region in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region) on city.ID equals region.ParentId
								  select new { state, city, region };
				
				//var regions = regionQuery.ToList();

				var query = from property in propertyQuery
							join propType in propertyTypeQuery on property.PropertyTypeId equals propType.ID
							join docType in documentTypeQuery on property.DocumentTypeId equals docType.ID
							join region in regionQuery on property.RegionId equals region.region.ID
							//join attach in attachmentQuery on property.ID equals attach.ObjectId into attachmentGroup
							where property.ID==parameters.ID
							select new PropertyInfoVM
							{
								ID = property.ID,
								PropertyTypeId = (int?)property.PropertyTypeId,
								PropertyTypeName = propType.Name,
								DocumentTypeId = property.DocumentTypeId,
								DocumentTypeName = docType.Name,
								HashKey = property.HashKey,
								PropertyCode = property.PropertyCode,
								Title = property.Title,
								UrlTitle = property.UrlTitle,
								Type = property.Type,
								BuildingArea = (float?)property.BuildingArea,
								LandingArea = (float?)property.LandingArea,
								BathRoom = property.BathRoom,
								BedRoom = property.BedRoom,
								Floor = property.Floor,
								FloorNumber = property.FloorNumber,
								Apartment = property.Apartment,
								Position = property.Position,
								PropertyView = property.PropertyView,
								ConstructorYear = property.ConstructorYear,
								StateId = region.state.ID,
								CityId = region.city.ID,
								RegionId = property.RegionId,
								Address = property.Address,
								Description = property.Description,
								SalePrice = property.SalePrice,
								MortgagePrice = property.MortgagePrice,
								RentalPrice = property.RentalPrice,
								FirstName = property.FirstName,
								LastName = property.LastName,
								Tel = property.Tel,
								ReagentFirstName = property.ReagentFirstName,
								ReagentLastName = property.ReagentLastName,
								ReagentTel = property.ReagentTel,
								WelfaresString = property.Welfares,
								CoverImage = property.CoverImage,
								SlideImage = property.SlideImage,
								Status = property.Status,
								RegisterDate = property.InsertDateTime,
								IsDeleted = property.IsDeleted,
								//Attachments= attachmentGroup.Any() ? attachmentGroup.ToList() : null,
							};
				result =await query.FirstOrDefaultAsync();
				List<Attachment> attachments = null;
				if (result != null)
				{
					result.Welfares = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WelfareVM>>(result.WelfaresString);

					attachments =await BusinessRule.UnitOfWork.Repository<Attachment>().Queryable().Where(e => e.ObjectType == ObjectType.Property && e.ObjectId== parameters.ID).ToListAsync();
					result.Attachments = attachments.Select(e => new AttachmentVM
					{
						FileName=e.FileName,
						HashKey=e.HashKey,
						ObjectId=e.ObjectId,
						FileSize=e.FileSize,
						ContentType=e.ContentType,
						Path=$"{AppConfigurations.PropertyImageFolder}/{result.HashKey}/Attachment_{e.FileName}",
					}).ToList();
					if (!string.IsNullOrWhiteSpace(result.CoverImage))
						result.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/{result.HashKey}/CoverImage_{result.CoverImage}";
					if (!string.IsNullOrWhiteSpace(result.SlideImage))
						result.SlideImagePath = $"{AppConfigurations.PropertyImageFolder}/{result.HashKey}/SlideImage_{result.SlideImage}";
					
					
				}
				
				return Success(result);
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		
	}
}
