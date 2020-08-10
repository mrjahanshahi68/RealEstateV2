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

				decimal maxValue =uow.Repository<PropertyInfo>().Queryable().Max(e => e.SalePrice) ?? 0;
				decimal minValue = uow.Repository<PropertyInfo>().Queryable().Min(e => e.SalePrice) ?? 0;
				homeInitData.MinMaxPropertySalePrice = new MinMaxValue
				{
					MaxValue = maxValue.ToString(),
					MinValue = minValue.ToString(),
				};
				maxValue = uow.Repository<PropertyInfo>().Queryable().Max(e => e.MortgagePrice) ?? 0;
				minValue = uow.Repository<PropertyInfo>().Queryable().Min(e => e.MortgagePrice) ?? 0;
				homeInitData.MinMaxPropertyMortgagePrice = new MinMaxValue
				{
					MaxValue = maxValue.ToString(),
					MinValue = minValue.ToString(),
				};
				maxValue = uow.Repository<PropertyInfo>().Queryable().Max(e => e.RentalPrice) ?? 0;
				minValue = uow.Repository<PropertyInfo>().Queryable().Min(e => e.RentalPrice) ?? 0;
				homeInitData.MinMaxPropertyRentalPrice = new MinMaxValue
				{
					MaxValue = maxValue.ToString(),
					MinValue = minValue.ToString(),
				};
				double landMaxValue = uow.Repository<PropertyInfo>().Queryable().Max(e => e.LandingArea) ?? 0;
				double landMinValue = uow.Repository<PropertyInfo>().Queryable().Min(e => e.LandingArea) ?? 0;
				homeInitData.MinMaxPropertyLandArea = new MinMaxValue
				{
					MaxValue = landMaxValue.ToString(),
					MinValue = landMinValue.ToString(),
				};
				landMaxValue = uow.Repository<PropertyInfo>().Queryable().Max(e => e.BuildingArea) ?? 0;
				landMinValue = uow.Repository<PropertyInfo>().Queryable().Min(e => e.BuildingArea) ?? 0;
				homeInitData.MinMaxPropertyBuildingArea = new MinMaxValue
				{
					MaxValue = landMaxValue.ToString(),
					MinValue = landMinValue.ToString(),
				};
			}
            return View(homeInitData);
			
        }
		[HttpPost]
		public JsonResult Search(PropertyFilterVM filter)
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
				var filterContainer = new FilterContainer();
				filterContainer.OrderBy = new List<OrderFilter>
				{
					new OrderFilter
					{
						Field="ID",
						Order=OrderFilterType.Desc
					}
				};
				filterContainer.Skip = 0;
				filterContainer.Take = 100;
				var operands = new List<TreeFilter>();
				if (!string.IsNullOrWhiteSpace(filter.RegionName))
				{
					operands.Add(new TreeFilter { Field = "RegionName", Value = filter.RegionName, FilterType = WhereFilterType.Contains });
				}
				if (filter.PropertyType.HasValue && filter.PropertyType.Value>0)
				{
					operands.Add(new TreeFilter { Field = "PropertyTypeId", Value = filter.PropertyType, FilterType = WhereFilterType.Equal });
				}
				if (filter.Type.HasValue && filter.Type.Value > 0)
				{
					operands.Add(new TreeFilter { Field = "Type", Value = filter.Type, FilterType = WhereFilterType.Equal });
				}
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
				
				query= query.Request(filterContainer);
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
				});
			}
			return Json(result, JsonRequestBehavior.AllowGet);
		}
        public ActionResult AboutMe()
        {
            return View();
        }
      
    }
}