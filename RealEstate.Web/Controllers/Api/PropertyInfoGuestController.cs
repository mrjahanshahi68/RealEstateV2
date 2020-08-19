using RealEstate.Common;
using RealEstate.Common.Entities.Common;
using RealEstate.Common.Entities.Property;
using RealEstate.Common.Exceptions;
using RealEstate.DataAccess;
using RealEstate.Domain;
using RealEstate.Domain.Property;
using RealEstate.Web.Log;
using RealEstate.Web.Models.Common;
using RealEstate.Web.Models.Property;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static RealEstate.Common.AppConstants;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Controllers.Api
{
    public class PropertyInfoGuestController :  BaseApiController<PropertyInfo, PropertyInfoVM>
	{
		private string PropertyFolder => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigurations.AttachmentFolder, "Properties");
		public PropertyInfoGuestController()
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
				using (var uow = new AppUnitOfWork())
				{
					var propertyTypes = await uow.Repository<PropertyType>().Queryable().Select(e => new KeyValueVM { Key = e.ID, Value = e.Name }).ToListAsync();
					var documentTypes = await uow.Repository<DocumentType>().Queryable().Select(e => new KeyValueVM { Key = e.ID, Value = e.Name }).ToListAsync();
					var welfares = await uow.Repository<Welfare>().Queryable().Select(e => new KeyValueVM { Key = e.ID, Value = e.Name }).ToListAsync();
					var states = await uow.Repository<State>().Queryable().ToListAsync();
					
					return Success(new
					{
						PropertyTypes = propertyTypes,
						DocumentTypes = documentTypes,
						Welfares = welfares,
						States = states,
					});
				}
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<HttpResponseMessage> SubmitProperty()
		{
			DbContextTransaction transaction = null;
			try
			{
				//LogManager.WriteLog<FileLogger>(Newtonsoft.Json.JsonConvert.SerializeObject(HttpContext.Current.Request.Headers), "request.txt");
				var fileDataRequestParameters = HttpContext.Current.Request.GetDataFileRequestParameters<PropertyInfoVM, FileDataModel>();
				var coverImageFile = fileDataRequestParameters.Files.Where(e => e.FileData.ExtraInfoString == "CoverImage").FirstOrDefault();
				//LogManager.WriteLog<FileLogger>(Newtonsoft.Json.JsonConvert.SerializeObject(fileDataRequestParameters),"parameters.txt");
				var parameters = fileDataRequestParameters.ViewModel;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var errors = new List<string>();
				if (!parameters.Type.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "نوع معامله"));
				if (!parameters.PropertyTypeId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "نوع ملک"));
				if (!parameters.DocumentTypeId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "نوع سند"));
				if (!parameters.StateId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "استان"));
				if (!parameters.CityId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "شهر"));
				if (!parameters.RegionId.HasValue)
					errors.Add(string.Format(MessageTemplate.Required, "منطقه"));

				if (errors.Any())
					throw new ValidationModelException(errors);

				using (var dbContext = new AppDataContext())
				{

					transaction = dbContext.Database.BeginTransaction();
					using (var uow = new UnitOfWork<AppDataContext>(dbContext))
					{
						var propertyInfoRepo = uow.Repository<PropertyInfo>();
						var attachmentRepo = uow.Repository<Attachment>();
						var entity = Mapper.Map<PropertyInfo>(parameters);
						entity.Status = AppEnums.PropertyStatus.Submitted;
						entity.Title = "ثبت شده توسط کاربر مهمان";
						entity.UrlTitle = entity.Title.AsOptimizeUrl();
						entity.Welfares = Newtonsoft.Json.JsonConvert.SerializeObject(parameters.Welfares);
						string hashkey = string.Empty;
						while (true)
						{
							hashkey = Guid.NewGuid().ToString();
							if (uow.Repository<PropertyInfo>().Queryable().Where(e => e.HashKey == hashkey).Any()) continue;
							break;
						}
						entity.HashKey = hashkey;
						if (coverImageFile != null)
							entity.CoverImage = coverImageFile.HttpPostedFile?.FileName;
						entity.SlideImage = null;
						entity.InsertDateTime = DateTime.Now;
						entity.InsertUserId = AppConstants.GuestUserId;

						uow.Repository<PropertyInfo>().Insert(entity);

						await uow.SaveChangeAsync();

						var currentDirectory = Path.Combine(PropertyFolder,entity.HashKey);
						//LogManager.WriteLog<FileLogger>(currentDirectory, "beforepath.txt");
						if (!Directory.Exists(currentDirectory))
							Directory.CreateDirectory(currentDirectory);
						//LogManager.WriteLog<FileLogger>(currentDirectory, "path.txt");
						var attachments = new List<Attachment>();
						foreach (var item in fileDataRequestParameters.Files)
						{
							var file = item.HttpPostedFile;
							if (item.FileData?.ExtraInfoString == "CoverImage")
							{
								file.SaveAs($"{currentDirectory}/CoverImage_{file.FileName}");
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
									InsertUserId = AppConstants.GuestUserId,
								});

								file.SaveAs($"{currentDirectory}/Attachment_{file.FileName}");
							}
						}

						foreach (var item in attachments)
							attachmentRepo.Insert(item);
						await uow.SaveChangeAsync();
						//LogManager.WriteLog<FileLogger>(Newtonsoft.Json.JsonConvert.SerializeObject(transaction), "beforecommit.txt");
						transaction.Commit();

					}
				}


				return Success();
			}
			catch (Exception ex)
			{
				//LogManager.WriteLog<FileLogger>(Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message), "exception.txt");
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}
		}

	}
}
