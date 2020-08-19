﻿using RealEstate.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Security;
using System.Threading.Tasks;
using RealEstate.Web.Security.Filters;
using RealEstate.Web.Models.Security;
using static RealEstate.Common.AppConstants;
using QueryDesigner;
using System.Data.Entity;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Models;
using RealEstate.Common;
using RealEstate.Web.Security;
using RealEstate.Web.Cache;
using RealEstate.Common.Entities;

namespace RealEstate.Web.Controllers.Api
{
	[JwtAuthentication]
    public class AccountController : BaseApiController<User,UserVM>
    {
		protected override IBusinessRule<User> CreateRule() => new UserBusinessRule();

		#region Operations
		[HttpPost]
		public async Task<HttpResponseMessage> GetUsers(FilterContainer filter)
		{
			try
			{
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				var query = BusinessRule.Queryable();
				var totalCount = await query.CountAsync();
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
		[HttpPost]
		public async Task<HttpResponseMessage> GetInfoUsers()
		{
			try
			{
				var securityManager = SecurityManager.GetPrinciple(Token);
				var username = securityManager.Identity.Name;
				var userInfo = CacheManager.GetValue(username) as AppUserInfo;

				var result = new ChangePassWordVM { UserId = userInfo.UserId };

				return Success(result);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		public async Task<HttpResponseMessage> ChangePass(ChangePassWordVM model)
		{
			try
			{
				var user = await BusinessRule.Queryable().Where(c => c.ID == model.UserId).SingleAsync();

				user.Password = model.NewPass.ToMd5().ToBase64();

				BusinessRule.UpdateEntity(user);

				return Success();
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion

		#region Validations
		protected override List<string> InsertValidation(UserVM viewmodel)
		{
			var errors = new List<string>();
			if (string.IsNullOrWhiteSpace(viewmodel.FirstName))
				errors.Add(string.Format(MessageTemplate.Required, "نام"));
			if (string.IsNullOrWhiteSpace(viewmodel.LastName))
				errors.Add(string.Format(MessageTemplate.Required, "نام خانوادگی"));
			if (string.IsNullOrWhiteSpace(viewmodel.UserName))
				errors.Add(string.Format(MessageTemplate.Required, "نام کاربری"));
			if (string.IsNullOrWhiteSpace(viewmodel.Password))
				errors.Add(string.Format(MessageTemplate.Required, "کلمه عبور"));

			viewmodel.RegisterDate = DateTime.Now;
			viewmodel.Password = viewmodel.Password.ToMd5().ToBase64();
			return errors;

		}
		protected override List<string> UpdateValidation(UserVM viewmodel)
		{
			var errors = new List<string>();
			if (viewmodel.ID <= 0)
				errors.Add("کد شناسته نامعتبر است");
			if (string.IsNullOrWhiteSpace(viewmodel.FirstName))
				errors.Add(string.Format(MessageTemplate.InvalidIdentity));
			if (string.IsNullOrWhiteSpace(viewmodel.LastName))
				errors.Add(string.Format(MessageTemplate.Required, "نام خانوادگی"));
			if (string.IsNullOrWhiteSpace(viewmodel.UserName))
				errors.Add(string.Format(MessageTemplate.Required, "نام کاربری"));
			if (string.IsNullOrWhiteSpace(viewmodel.Password))
				errors.Add(string.Format(MessageTemplate.Required, "کلمه عبور"));

			var entity=BusinessRule.FindEntity(viewmodel.ID);
			if (entity == null) throw new Exception("رکورد مورد نظر یافت نشد");
			viewmodel.Password = viewmodel.Password.ToMd5().ToBase64();
			viewmodel.RegisterDate = entity.RegisterDate;


			return errors;
		}
		#endregion
	}
}
