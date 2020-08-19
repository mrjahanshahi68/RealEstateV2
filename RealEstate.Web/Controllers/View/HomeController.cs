using QueryDesigner;
using RealEstate.Common;
using RealEstate.Common.Entities.Common;
using RealEstate.Common.Entities.Property;
using RealEstate.DataAccess;
using RealEstate.Web.Models.Common;
using RealEstate.Web.Models.Property;
using RealEstate.Web.Models.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static RealEstate.Common.AppConstants;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Controllers.View
{
    public class HomeController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
			HomeVM homeInitData = new HomeVM();
			using(var uow=new AppUnitOfWork())
			{
				var propertyTypeQuery = uow.Repository<PropertyType>().Queryable();
				var documentTypeQuery = uow.Repository<DocumentType>().Queryable();
				var propertyQuery = uow.Repository<PropertyInfo>().Queryable();
				var regionQuery = from state in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State)
								  join city in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City) on state.ID equals city.ParentId
								  join region in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region) on city.ID equals region.ParentId
								  select new { state, city, region };

				var query = from property in propertyQuery
							join propType in propertyTypeQuery on property.PropertyTypeId equals propType.ID
							join docType in documentTypeQuery on property.DocumentTypeId equals docType.ID
							join region in regionQuery on property.RegionId equals region.region.ID
							where property.Status==PropertyStatus.Approved
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
								RegionName = region.state.Name + DbFunctions.AsUnicode(" ") + region.city.Name + DbFunctions.AsUnicode(" ") + region.region.Name,
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
								ReagentFullName = property.ReagentFirstName + DbFunctions.AsUnicode(" ") + property.ReagentLastName,
								WelfaresString = property.Welfares,
								CoverImage = property.CoverImage,
								SlideImage = property.SlideImage,
								Status = property.Status,
								RegisterDate = property.InsertDateTime,
								IsDeleted = property.IsDeleted,
								
							};

				homeInitData.LatestProperties = query.OrderByDescending(e => e.RegisterDate).Take(AppConstants.LatestTakeProperty).ToList();
				homeInitData.LuxuryProperties= query.Where(e=>e.SlideImage!=null).OrderByDescending(e => e.RegisterDate).Take(AppConstants.LatestTakeProperty).ToList();
				homeInitData.LatestProperties?.ForEach(prop => {
					if (prop.Type == TransactionType.Rent)
					{
						prop.TransactionTypeName = "اجاره";
					}
					else if (prop.Type == TransactionType.Sale)
					{
						prop.TransactionTypeName = "فروش";
					}
					else if (prop.Type == TransactionType.PreSel)
					{
						prop.TransactionTypeName = "پیش فروش";
					}
					else if (prop.Type == TransactionType.Exchange)
					{
						prop.TransactionTypeName = "معاوضه";
					}
					if (!string.IsNullOrWhiteSpace(prop.CoverImage))
						prop.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/{prop.HashKey}/CoverImage_{prop.CoverImage}";
					else
						prop.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/default.jpg";
				});
				homeInitData.LuxuryProperties?.ForEach(prop => {
					if (prop.Type == TransactionType.Rent)
					{
						prop.TransactionTypeName = "اجاره";
					}
					else if (prop.Type == TransactionType.Sale)
					{
						prop.TransactionTypeName = "فروش";
					}
					else if (prop.Type == TransactionType.PreSel)
					{
						prop.TransactionTypeName = "پیش فروش";
					}
					else if (prop.Type == TransactionType.Exchange)
					{
						prop.TransactionTypeName = "معاوضه";
					}
					if (!string.IsNullOrWhiteSpace(prop.SlideImage))
						prop.SlideImagePath = $"{AppConfigurations.PropertyImageFolder}/{prop.HashKey}/SlideImage_{prop.SlideImage}";
				});
				homeInitData.States = uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State).ToList();
				homeInitData.Cities = uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City).ToList();
				homeInitData.Regions = uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region).ToList();
				homeInitData.PropertyTypes = propertyTypeQuery.ToList().Select(e => new PropertyTypeVM { ID = e.ID, Name = e.Name }).ToList();
			}
            return View(homeInitData);
			
        }
		[HttpPost]
		public JsonResult Search(PropertyFilterVM filter, int? page,int? take)
		{
			List<PropertyInfoVM> result = null;
			using (var uow = new AppUnitOfWork())
			{
				var propertyTypeQuery = uow.Repository<PropertyType>().Queryable();
				var documentTypeQuery = uow.Repository<DocumentType>().Queryable();
				var propertyQuery = uow.Repository<PropertyInfo>().Queryable();
				var regionQuery = from state in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State)
								  join city in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City) on state.ID equals city.ParentId
								  join region in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region) on city.ID equals region.ParentId
								  select new { state, city, region };

				var query = from property in propertyQuery
							join propType in propertyTypeQuery on property.PropertyTypeId equals propType.ID
							join docType in documentTypeQuery on property.DocumentTypeId equals docType.ID
							join region in regionQuery on property.RegionId equals region.region.ID
							where property.Status == PropertyStatus.Approved
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
								RegionName = region.state.Name + DbFunctions.AsUnicode(" ") + region.city.Name + DbFunctions.AsUnicode(" ") + region.region.Name,
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
								ReagentFullName = property.ReagentFirstName + DbFunctions.AsUnicode(" ") + property.ReagentLastName,
								WelfaresString = property.Welfares,
								CoverImage = property.CoverImage,
								SlideImage = property.SlideImage,
								Status = property.Status,
								RegisterDate = property.InsertDateTime,
								IsDeleted = property.IsDeleted,
								FullText = region.state.Name + DbFunctions.AsUnicode(" ") + region.city.Name + DbFunctions.AsUnicode(" ") + region.region.Name + DbFunctions.AsUnicode(" ") + property.Title + DbFunctions.AsUnicode(" ") + propType.Name,
							};

				if (filter.Type.HasValue)
				{
					query = query.Where(e => e.Type == filter.Type);
				}
				var filterContainer = new FilterContainer();
				filterContainer.OrderBy = new List<OrderFilter>
				{
					new OrderFilter
					{
						Field="ID",
						Order=OrderFilterType.Desc
					}
				};
				filterContainer.Take = take.HasValue ? take.Value : AppConstants.PropertySearchDefaultTake;
				filterContainer.Skip = page.HasValue ? (page.Value -1) * filterContainer.Take : 0;
				
				var operands = new List<TreeFilter>();
				if (!string.IsNullOrWhiteSpace(filter.RegionName))
				{
					operands.Add(new TreeFilter { Field = "RegionName", Value = filter.RegionName.Trim(), FilterType = WhereFilterType.Contains });
				}
				if (filter.PropertyType.HasValue && filter.PropertyType.Value>0)
				{
					operands.Add(new TreeFilter { Field = "PropertyTypeId", Value = filter.PropertyType, FilterType = WhereFilterType.Equal });
				}
				if (!string.IsNullOrWhiteSpace(filter.FullText))
				{
					operands.Add(new TreeFilter { Field = "FullText", Value = filter.FullText, FilterType = WhereFilterType.Contains });
				}
				if (filter.IsAdvanced)
				{
					if (filter.Type.HasValue)
					{
						if (filter.Type == TransactionType.Rent)
						{
							
							if (filter.MortgagePrice != null)
							{
								decimal minValue = 0;
								decimal maxValue = 0;
								if (Convert.ToDecimal(filter.MortgagePrice.MinValue) > 0  && Convert.ToDecimal(filter.MortgagePrice.MaxValue) > 0)
								{
									minValue = Convert.ToDecimal(filter.MortgagePrice.MinValue);
									maxValue = Convert.ToDecimal(filter.MortgagePrice.MaxValue);
									query = query.Where(e => e.MortgagePrice.Value >= minValue && e.MortgagePrice.Value <= maxValue);
								}
								else if(Convert.ToDecimal(filter.MortgagePrice.MinValue) > 0 && Convert.ToDecimal(filter.MortgagePrice.MaxValue) <= 0)
								{
									minValue = Convert.ToDecimal(filter.MortgagePrice.MinValue);
									query = query.Where(e => e.MortgagePrice.Value >= minValue);
								}
								else if(Convert.ToDecimal(filter.MortgagePrice.MinValue) <=0 && Convert.ToDecimal(filter.MortgagePrice.MaxValue) > 0)
								{
									maxValue = Convert.ToDecimal(filter.MortgagePrice.MaxValue);
									query = query.Where(e => e.MortgagePrice.Value <= maxValue);
								}
							}
							if (filter.RentalPrice != null)
							{
								decimal minValue = 0;
								decimal maxValue = 0;
								if (Convert.ToDecimal(filter.RentalPrice.MinValue) > 0 && Convert.ToDecimal(filter.RentalPrice.MaxValue) > 0)
								{
									minValue = Convert.ToDecimal(filter.RentalPrice.MinValue);
									maxValue = Convert.ToDecimal(filter.RentalPrice.MaxValue);
									query = query.Where(e => e.RentalPrice.Value >= minValue && e.RentalPrice.Value <= maxValue);
								}
								else if (Convert.ToDecimal(filter.RentalPrice.MinValue) > 0 && Convert.ToDecimal(filter.RentalPrice.MaxValue) <= 0)
								{
									minValue = Convert.ToDecimal(filter.RentalPrice.MinValue);
									query = query.Where(e => e.RentalPrice.Value >= minValue);
								}
								else if (Convert.ToDecimal(filter.RentalPrice.MinValue) <= 0 && Convert.ToDecimal(filter.RentalPrice.MaxValue) > 0)
								{
									maxValue = Convert.ToDecimal(filter.RentalPrice.MaxValue);
									query = query.Where(e => e.RentalPrice.Value <= maxValue);
								}
							}
						}
						else if(filter.Type==TransactionType.Sale || filter.Type == TransactionType.PreSel)
						{
							if (filter.SalePrice != null)
							{
								decimal minValue = 0;
								decimal maxValue = 0;
								if (Convert.ToDecimal(filter.SalePrice.MinValue) > 0 && Convert.ToDecimal(filter.SalePrice.MaxValue) > 0)
								{
									minValue = Convert.ToDecimal(filter.SalePrice.MinValue);
									maxValue = Convert.ToDecimal(filter.SalePrice.MaxValue);
									query = query.Where(e => e.SalePrice.Value >= minValue && e.SalePrice.Value <= maxValue);
								}
								else if (Convert.ToDecimal(filter.SalePrice.MinValue) > 0 && Convert.ToDecimal(filter.SalePrice.MaxValue) <= 0)
								{
									minValue = Convert.ToDecimal(filter.SalePrice.MinValue);
									query = query.Where(e => e.SalePrice.Value >= minValue);
								}
								else if (Convert.ToDecimal(filter.SalePrice.MinValue) <= 0 && Convert.ToDecimal(filter.SalePrice.MaxValue) > 0)
								{
									maxValue = Convert.ToDecimal(filter.SalePrice.MaxValue);
									query = query.Where(e => e.SalePrice.Value <= maxValue);
								}
							}
						}
					}
					
					if (filter.LandingArea != null)
					{
						int minValue = 0;
						int maxValue = 0;
						if (Convert.ToDecimal(filter.LandingArea.MinValue) > 0 && Convert.ToDecimal(filter.LandingArea.MaxValue) > 0)
						{
							minValue = Convert.ToInt32(filter.LandingArea.MinValue);
							maxValue = Convert.ToInt32(filter.LandingArea.MaxValue);
							query = query.Where(e => e.LandingArea.Value >= minValue && e.LandingArea.Value <= maxValue);
						}
						else if (Convert.ToDecimal(filter.LandingArea.MinValue) > 0 && Convert.ToDecimal(filter.LandingArea.MaxValue) <= 0)
						{
							minValue = Convert.ToInt32(filter.LandingArea.MinValue);
							query = query.Where(e => e.LandingArea.Value >= minValue);
						}
						else if (Convert.ToDecimal(filter.LandingArea.MinValue) <= 0 && Convert.ToDecimal(filter.LandingArea.MaxValue) > 0)
						{
							maxValue = Convert.ToInt32(filter.LandingArea.MaxValue);
							query = query.Where(e => e.LandingArea.Value <= maxValue);
						}
					}
					if (filter.BuildingArea != null)
					{
						int minValue = 0;
						int maxValue = 0;
						if (Convert.ToDecimal(filter.BuildingArea.MinValue) > 0 && Convert.ToDecimal(filter.BuildingArea.MaxValue) > 0)
						{
							minValue = Convert.ToInt32(filter.BuildingArea.MinValue);
							maxValue = Convert.ToInt32(filter.BuildingArea.MaxValue);
							query = query.Where(e => e.BuildingArea.Value >= minValue && e.BuildingArea.Value <= maxValue);
						}
						else if (Convert.ToDecimal(filter.BuildingArea.MinValue) > 0 && Convert.ToDecimal(filter.BuildingArea.MaxValue) <= 0)
						{
							minValue = Convert.ToInt32(filter.BuildingArea.MinValue);
							query = query.Where(e => e.BuildingArea.Value >= minValue);
						}
						else if (Convert.ToDecimal(filter.BuildingArea.MinValue) <= 0 && Convert.ToDecimal(filter.BuildingArea.MaxValue) > 0)
						{
							maxValue = Convert.ToInt32(filter.BuildingArea.MaxValue);
							query = query.Where(e => e.BuildingArea.Value <= maxValue);
						}
					}
				}
				//if (filter.Type.HasValue && filter.Type.Value > 0)
				//{
				//	TransactionType type = TransactionType.Rent;
				//	operands.Add(new TreeFilter { Field = "Type", Value = type, FilterType = WhereFilterType.Equal });
				//}
				if (filter.Bedroom.HasValue && filter.Bedroom.Value > 0)
				{
					operands.Add(new TreeFilter { Field = "BedRoom", Value = filter.Bedroom, FilterType = WhereFilterType.Equal });
				}
				if (operands.Any())
				{
					filterContainer.Where = new TreeFilter
					{
						OperatorType = TreeFilterType.And,
						Operands = operands
					};
				}
				
				query = query.Request(filterContainer);
				
				result = query.ToList();
				result?.ForEach(prop => {
					if (prop.Type == TransactionType.Rent)
					{
						prop.TransactionTypeName = "اجاره";
					}
					else if (prop.Type == TransactionType.Sale)
					{
						prop.TransactionTypeName = "فروش";
					}
					else if (prop.Type == TransactionType.PreSel)
					{
						prop.TransactionTypeName = "پیش فروش";
					}
					else if (prop.Type == TransactionType.Exchange)
					{
						prop.TransactionTypeName = "معاوضه";
					}
					if (!string.IsNullOrWhiteSpace(prop.CoverImage))
						prop.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/{prop.HashKey}/CoverImage_{prop.CoverImage}";
					else
						prop.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/default.jpg";
				});
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}
        public ActionResult AboutMe()
        {
            return View();
        }
		[Route("property/{hashkey}/{title}")]
		public ActionResult PropertyDetail(string hashkey,string title)
		{
			PropertyInfoVM result = null;
			using (var uow = new AppUnitOfWork())
			{
				var propertyTypeQuery = uow.Repository<PropertyType>().Queryable();
				var documentTypeQuery = uow.Repository<DocumentType>().Queryable();
				var propertyQuery = uow.Repository<PropertyInfo>().Queryable();
				var regionQuery = from state in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State)
								  join city in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City) on state.ID equals city.ParentId
								  join region in uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region) on city.ID equals region.ParentId
								  select new { state, city, region };

				var query = from property in propertyQuery
							join propType in propertyTypeQuery on property.PropertyTypeId equals propType.ID
							join docType in documentTypeQuery on property.DocumentTypeId equals docType.ID
							join region in regionQuery on property.RegionId equals region.region.ID
							where property.HashKey == hashkey
							where property.Status == PropertyStatus.Approved
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
								RegionName = region.state.Name + DbFunctions.AsUnicode(" ") + region.city.Name + DbFunctions.AsUnicode(" ") + region.region.Name,
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
								ReagentFullName = property.ReagentFirstName + DbFunctions.AsUnicode(" ") + property.ReagentLastName,
								WelfaresString = property.Welfares,
								CoverImage = property.CoverImage,
								SlideImage = property.SlideImage,
								Status = property.Status,
								RegisterDate = property.InsertDateTime,
								IsDeleted = property.IsDeleted,

							};
				result = query.FirstOrDefault();
				List<Attachment> attachments = null;
				if (result != null)
				{
					result.Welfares = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WelfareVM>>(result.WelfaresString);

					attachments =uow.Repository<Attachment>().Queryable().Where(e => e.ObjectType == ObjectType.Property && e.ObjectId == result.ID).ToList();
					result.Attachments = attachments.Select(e => new AttachmentVM
					{
						FileName = e.FileName,
						HashKey = e.HashKey,
						ObjectId = e.ObjectId,
						FileSize = e.FileSize,
						ContentType = e.ContentType,
						Path = $"{AppConfigurations.PropertyImageFolder}/{result.HashKey}/Attachment_{e.FileName}",
					}).ToList();
					if (!string.IsNullOrWhiteSpace(result.CoverImage))
						result.CoverImagePath = $"{AppConfigurations.PropertyImageFolder}/{result.HashKey}/CoverImage_{result.CoverImage}";
					if (!string.IsNullOrWhiteSpace(result.SlideImage))
						result.SlideImagePath = $"{AppConfigurations.PropertyImageFolder}/{result.HashKey}/SlideImage_{result.SlideImage}";

					if (result.Type == TransactionType.Rent)
					{
						result.TransactionTypeName = "اجاره";
					}
					else if (result.Type == TransactionType.Sale)
					{
						result.TransactionTypeName = "فروش";
					}
					else if (result.Type == TransactionType.PreSel)
					{
						result.TransactionTypeName = "پیش فروش";
					}
					else if (result.Type == TransactionType.Exchange)
					{
						result.TransactionTypeName = "معاوضه";
					}

				}
			}
			return View("~/ClientApp/Views/Home/PropertyDetails.cshtml", result);
		}
		public ActionResult PropertyDemand()
		{
			var model = new PropertyDemandCreateVM();

			using (var uow = new AppUnitOfWork())
			{
				var propertyTypes = uow.Repository<PropertyType>().Queryable()
				   .Where(c => !c.IsDeleted)
					 .Select(c => new PropertyTypeVM
					 {
						 ID = c.ID,
						 Name = c.Name,
					 }).ToList();

				var transactionTypes = new List<TransactionTypeVM>
				{
					new TransactionTypeVM{ID=1,Name="اجاره"},
					new TransactionTypeVM{ID=2,Name="فروش"},
					new TransactionTypeVM{ID=3,Name="پیش فروش"},
					new TransactionTypeVM{ID=4,Name="معاوضه"}
				};

				//var provinces = uow.Repository<State>().Queryable()
				//   .Where(c => c.Level == (int)Level.State && !c.IsDeleted)
				//	 .Select(c => new State
				//	 {
				//		 ID = c.ID,
				//		 Name = c.Name,
				//	 }).ToList();

				//var cities = uow.Repository<State>().Queryable()
				//   .Where(c => c.Level == (int)Level.City && !c.IsDeleted)
				//	 .Select(c => new StateVM
				//	 {
				//		 ID = c.ID,
				//		 Name = c.Name,
				//	 }).ToList();

				//var states = uow.Repository<State>().Queryable()
				//   .Where(c => c.Level == (int)Level.Region && !c.IsDeleted)
				//	 .Select(c => new StateVM
				//	 {
				//		 ID = c.ID,
				//		 Name = c.Name,
				//	 }).ToList();

				model.States = uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State).ToList();
				model.Cities = uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City).ToList();
				model.Regions = uow.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region).ToList();


				ViewData["PropertyTypesSelectList"] = new SelectList(propertyTypes, "ID", "Name");
				ViewData["TransactionTypesSelectList"] = new SelectList(transactionTypes, "ID", "Name");
				//ViewData["ProvinceSelectList"] = new SelectList(provinces, "ID", "Name");
				//ViewData["CitySelectList"] = new SelectList(cities, "ID", "Name");
				//ViewData["StateSelectList"] = new SelectList(states, "ID", "Name");
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult GetCasCadeCities(int a)
		{
			using (var uow = new AppUnitOfWork())
			{
				var cities = uow.Repository<State>().Queryable()
				   .Where(c => c.ParentId == a && !c.IsDeleted)
					 .Select(c => new StateVM
					 {
						 ID = c.ID,
						 Name = c.Name,
					 }).ToList();


				return Json(new { success = true, da = cities });
			}

		}

		[HttpPost]
		public ActionResult GetCasCadeStates(int b)
		{
			using (var uow = new AppUnitOfWork())
			{
				var states = uow.Repository<State>().Queryable()
				   .Where(c => c.ParentId == b && !c.IsDeleted)
					 .Select(c => new StateVM
					 {
						 ID = c.ID,
						 Name = c.Name,
					 }).ToList();

				return Json(new { success = true, da = states });
			}

		}


		[HttpPost]
		public ActionResult InsertPropertyDemand(PropertyDemandCreateVM model)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(model.FullName))
				errors.Add(string.Format(MessageTemplate.Required, " نام و نام خانوادگی "));
			if (string.IsNullOrWhiteSpace(model.MobileNumber))
				errors.Add(string.Format(MessageTemplate.Required, " شماره تماس "));
			if (model.PropertyType == 0)
				errors.Add(string.Format(MessageTemplate.Required, " نوع ملک "));
			if (model.TransactionType == 0)
				errors.Add(string.Format(MessageTemplate.Required, " نوع معامله "));
			if (model.MinimumPrice == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداقل قیمت به تومان "));
			if (model.MaximumPrice == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداکثر قیمت به تومان "));
			if (model.MaximumArea == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداکثر متراژ(متر مربع) "));
			if (model.MinimumArea == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداقل متراژ(متر مربع) "));
			if ((model.StateId == 0 || model.StateId == null) && !model.CityId.HasValue && !model.ProvinceId.HasValue)
				errors.Add(string.Format(MessageTemplate.Required, " منطقه "));



			if (errors != null && errors.Any())
			{
				return Json(new { success = false, errors = errors });
			}
			else
			{
				var item = new PropertyDemand
				{
					MobileNumber = model.MobileNumber,
					FullName = model.FullName,
					MaximumArea = model.MaximumArea,
					MinimumArea = model.MinimumArea,
					MaximumPrice = model.MaximumPrice,
					MinimumPrice = model.MinimumPrice,
					TransactionType = model.TransactionType,
					PropertyType = model.PropertyType,
					IsDeleted = false,
					InserDateTime = DateTime.Now,
					StateId = model.StateId.HasValue ? model.StateId.Value :
						   model.CityId.HasValue ? model.CityId.Value : model.ProvinceId.Value

				};

				using (var uow = new AppUnitOfWork())
				{
					var query = uow.Repository<PropertyDemand>();
					query.Insert(item);

					uow.SaveChange();
				}

				return Json(new { success = true });
			}
		}
	}
}