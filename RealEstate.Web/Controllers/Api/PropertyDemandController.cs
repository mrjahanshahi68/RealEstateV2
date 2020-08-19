using RealEstate.Common.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Property;
using System.Threading.Tasks;
using RealEstate.Web.Security.Filters;
using RealEstate.Web.Models.Property;
using static RealEstate.Common.AppConstants;
using QueryDesigner;
using System.Data.Entity;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Models;
using RealEstate.Web.Security;
using RealEstate.Web.Cache;
using RealEstate.Common.Entities;
using RealEstate.Common.Entities.Common;
using static RealEstate.Common.AppEnums;
using RealEstate.Common.Entities.Security;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
	public class PropertyDemandController : BaseApiController<PropertyDemand, PropertyDemandVM>
	{
		protected override IBusinessRule<PropertyDemand> CreateRule() => new PropertyDemandBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetPropertyDemands(FilterContainer filter)
		{
			try
			{
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var propertyTypeQuery = BusinessRule.UnitOfWork.Repository<PropertyType>().Queryable();
				var userQuery = BusinessRule.UnitOfWork.Repository<User>().Queryable();
				var propertyDemandQuery = BusinessRule.Queryable();
				var regionQuery = from state in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.State)
								  join city in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.City) on state.ID equals city.ParentId
								  join region in BusinessRule.UnitOfWork.Repository<State>().Queryable().Where(e => e.Level == (int)Level.Region) on city.ID equals region.ParentId
								  select new { state, city, region };
				//var regions = regionQuery.ToList();

				var count = propertyDemandQuery.Count();
				var query = from d in propertyDemandQuery
							join propType in propertyTypeQuery on d.PropertyType equals propType.ID
							join region in regionQuery on d.StateId equals region.region.ID
							//join user in userQuery on d.InsertUserId equals user.ID
							select new PropertyDemandVM
							{
								ID = d.ID,
								PropertyType = d.PropertyType,
								PropertyTypeName = propType.Name,
								TransactionType = d.TransactionType,
								MaximumArea = d.MaximumArea,
								MinimumArea = d.MinimumArea,
								MaximumPrice = d.MaximumPrice,
								MinimumPrice = d.MinimumPrice,
								InserDateTime = d.InserDateTime,
								//InsertUserId = d.InsertUserId,
								//InsertUserName = user.FirstName + user.LastName,
								MobileNumber = d.MobileNumber,
								FullName = d.FullName,
								StateId = region.region.ID,
								CityId = region.city.ID,
								ProvinceId = region.state.ID,
								CityName = region.city.Name,
								RegionName = region.region.Name,
								IsDeleted = d.IsDeleted
							};

				var count2 = query.Count();
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
		#endregion

		#region Validations
		protected override List<string> InsertValidation(PropertyDemandVM viewmodel)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(viewmodel.FullName))
				errors.Add(string.Format(MessageTemplate.Required, " نام و نام خانوادگی "));
			if (string.IsNullOrWhiteSpace(viewmodel.MobileNumber))
				errors.Add(string.Format(MessageTemplate.Required, " شماره تماس "));
			if (viewmodel.PropertyType == 0)
				errors.Add(string.Format(MessageTemplate.Required, " نوع ملک "));
			if (viewmodel.TransactionType == 0)
				errors.Add(string.Format(MessageTemplate.Required, " نوع معامله "));
			if (viewmodel.StateId == 0)
				errors.Add(string.Format(MessageTemplate.Required, " منطقه "));
			if (viewmodel.MinimumPrice == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداقل قیمت به تومان "));
			if (viewmodel.MaximumPrice == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداکثر قیمت به تومان "));
			if (viewmodel.MaximumArea == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداکثر متراژ(متر مربع) "));
			if (viewmodel.MinimumArea == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداقل متراژ(متر مربع) "));
			if (viewmodel.StateId == 0)
				errors.Add(string.Format(MessageTemplate.Required, " منطقه "));

			var securityManager = SecurityManager.GetPrinciple(Token);
			var username = securityManager.Identity.Name;
			var userInfo = CacheManager.GetValue(username) as AppUserInfo;

			viewmodel.InsertUserId = userInfo.UserId;
			viewmodel.InserDateTime = DateTime.Now;

			return errors;

		}

		protected override List<string> UpdateValidation(PropertyDemandVM viewmodel)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(viewmodel.FullName))
				errors.Add(string.Format(MessageTemplate.Required, " نام و نام خانوادگی "));
			if (string.IsNullOrWhiteSpace(viewmodel.MobileNumber))
				errors.Add(string.Format(MessageTemplate.Required, " شماره تماس "));
			if (viewmodel.PropertyType == 0)
				errors.Add(string.Format(MessageTemplate.Required, " نوع ملک "));
			if (viewmodel.TransactionType == 0)
				errors.Add(string.Format(MessageTemplate.Required, " نوع معامله "));
			if (viewmodel.StateId == 0)
				errors.Add(string.Format(MessageTemplate.Required, " منطقه "));
			if (viewmodel.MinimumPrice == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداقل قیمت به تومان "));
			if (viewmodel.MaximumPrice == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداکثر قیمت به تومان "));
			if (viewmodel.MaximumArea == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداکثر متراژ(متر مربع) "));
			if (viewmodel.MinimumArea == 0)
				errors.Add(string.Format(MessageTemplate.Required, " حداقل متراژ(متر مربع) "));
			if (viewmodel.StateId == 0)
				errors.Add(string.Format(MessageTemplate.Required, " منطقه "));

			var securityManager = SecurityManager.GetPrinciple(Token);
			var username = securityManager.Identity.Name;
			var userInfo = CacheManager.GetValue(username) as AppUserInfo;

			viewmodel.InsertUserId = userInfo.UserId;
			viewmodel.InserDateTime = DateTime.Now;

			return errors;

		}
		#endregion
	}
}
