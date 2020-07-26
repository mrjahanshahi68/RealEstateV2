using RealEstate.Common.Entities;
using RealEstate.Common.Entities.Security;
using RealEstate.Common.Exceptions;
using RealEstate.Domain;
using RealEstate.Domain.Security;
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

                var user = Rule.FindUserByUserName("developer");
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

	}
}
