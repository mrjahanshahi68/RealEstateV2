using RealEstate.Common.Entities;
using RealEstate.Common.Entities.Security;
using RealEstate.Common.Exceptions;
using RealEstate.Domain;
using RealEstate.Domain.Security;
using RealEstate.Web.Cache;
using RealEstate.Web.Infrastrcuture;
using RealEstate.Web.Infrastrcuture.Filters;
using RealEstate.Web.Models.Security;
using RealEstate.Web.Security;
using RealEstate.Web.Security.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static RealEstate.Common.AppConstants;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Controllers.Api
{
	
    public class SecurityController : BaseApiController<User,UserVM>
	{
        public  UserBusinessRule Rule => BusinessRule as UserBusinessRule;
        protected override IBusinessRule<User> CreateRule()=> new UserBusinessRule();
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Login(AuthenticateRequest parameters)
		{
            try
			{
                #region Validations
                if (parameters == null) throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(parameters.UserName))
                    errors.Add(string.Format(MessageTemplate.Required, nameof(parameters.UserName)));
                if (string.IsNullOrWhiteSpace(parameters.Password))
                    errors.Add(string.Format(MessageTemplate.Required, nameof(parameters.Password)));
                if (errors.Any()) throw new ValidationModelException(errors);
                #endregion

                //var user = Rule.FindUserByUserName(parameters.UserName);
                if (!SecurityManager.SignIn(parameters.UserName, parameters.Password))
                    throw new AuthenticationException("Username or password was wrong");
                
				var jwtToken= SecurityManager.GenerateToken(parameters.UserName);

				return Success(jwtToken);
			}
			catch (Exception ex)
			{
				return await  HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		[JwtAuthentication]
		public async Task<HttpResponseMessage> Loggout()
		{
			try
			{
				if (IsAuthenticated)
				{
					SecurityManager.SignOut(Token);
				}
				return Success();
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		[JwtAuthentication]
		public async Task<HttpResponseMessage> RefereshToken()
		{
			try
			{
				if (IsAuthenticated)
				{
					//var restOfExpireMinute = SecurityManager.GetRestOfExpiryAsMinute(Token);
					//if(restOfExpireMinute<0)
					//var jwtToken = SecurityManager.GenerateToken(CurrentUser.Context.UserName);
					//return Success(jwtToken);
				}
				return Success(null);
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		[JwtAuthentication]
		public async Task<HttpResponseMessage> GetCurrentUser()
		{
			try
			{
				if (!IsAuthenticated) throw new AuthenticationException("کاربر فعلی نامعتبر است");
				
				return Success(new {
					UserName=CurrentUser.Context.UserName,
					FullName=$"{CurrentUser.Context.FirstName} {CurrentUser.Context.LastName}",
					Roles=CurrentUser.Context.Roles.Select(e=>e.Name).ToList(),
					Email = CurrentUser.Context.Email,
					NationalCode = CurrentUser.Context.NationalCode,
					UserType = CurrentUser.Context.UserType,
					Mobile = CurrentUser.Context.Mobile,
					RegisterDate = CurrentUser.Context.RegisterDate,
					Address = CurrentUser.Context.Address,
				});
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

		[HttpPost]
		[JwtAuthentication]
		public async Task<HttpResponseMessage> GetMenus()
		{
			try
			{
				MenuItemResponse menu = new MenuItemResponse
				{
					Guid = Guid.NewGuid().ToString(),
					Name = "ریشه",
					Icon = "fa fa-sitemap",
					MenuItems = new List<MenuItemResponse>
					{
						new MenuItemResponse
						{
							Guid = Guid.NewGuid().ToString(),
							Name="داشبورد",
							Link="/dashboard",
							Icon="fa fa-dashboard",
						},
						new MenuItemResponse
						{
							Guid = Guid.NewGuid().ToString(),
							Name="اطلاعات پایه",
							Icon="fa fa-cube",
							MenuItems=new List<MenuItemResponse>
							{
								new MenuItemResponse
								{
									Guid = Guid.NewGuid().ToString(),
									Name="رشته ها",
									Icon="fa fa-cogs",
									Link="/baseinfo/courses"
									
								},
							}
						},
						new MenuItemResponse
						{
							Guid = Guid.NewGuid().ToString(),
							Name="آزمون",
							Icon="fa fa-test",
							MenuItems=new List<MenuItemResponse>
							{
								new MenuItemResponse
								{
									Guid = Guid.NewGuid().ToString(),
									Name="ساخت آزمون",
									Icon="fa fa-cogs",
									Link="/test/create"
								},
								new MenuItemResponse
								{
									Guid = Guid.NewGuid().ToString(),
									Name="مدیریت",
									Icon="fa fa-cogs",
									Link="/test/manage"
								}
							}
						},
						new MenuItemResponse
						{
							Guid = Guid.NewGuid().ToString(),
							Name="کاربران",
							Icon="fa fa-users",
							MenuItems=new List<MenuItemResponse>
							{
								new MenuItemResponse
								{
									Guid = Guid.NewGuid().ToString(),
									Name="ثبت کاربر",
									Icon="fa fa-user",
									Link="/user/create"
								},
								new MenuItemResponse
								{
									Guid = Guid.NewGuid().ToString(),
									Name="مدیریت",
									Icon="fa fa-cogs",
									Link="/user/manage"
								}
							}
						},
					}
				};
				return Success(menu);
			}
			catch(Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
	}
}
