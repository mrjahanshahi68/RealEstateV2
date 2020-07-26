using RealEstate.Common;
using RealEstate.Common.Entities;
using RealEstate.Web.Cache;
using RealEstate.Web.Infrastrcuture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using static RealEstate.Common.AppConstants;

namespace RealEstate.Web.Security.Filters
{
	public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
	{
		public bool AllowMultiple => false;
		public string Realm { get; set; }
		public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
            var request = context.Request;
			var authorization = request.Headers.Authorization;
			if (authorization == null || authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult(MessageTemplate.InvalidToken, context.Request);
				return;
            }
            if (string.IsNullOrWhiteSpace(authorization.Parameter))
			{
				context.ErrorResult = new AuthenticationFailureResult(MessageTemplate.MissingToken, context.Request);
				return;
			}
			var token = authorization.Parameter;
			var principal = await AutheticateJwtToken(token);
			if (principal == null)
				context.ErrorResult = new AuthenticationFailureResult(MessageTemplate.InvalidToken, context.Request);
			else
				context.Principal = principal;
		}

		private static bool ValidateToken(string token,out string username)
		{
			username = null;
			var principle = SecurityManager.GetPrinciple(token);
			var identity = principle?.Identity as ClaimsIdentity;
			if (identity == null)
				return false;
			if (!identity.IsAuthenticated)
				return false;
			var usernameClaim = identity.FindFirst(ClaimTypes.Name);
			username = usernameClaim?.Value;
			if (string.IsNullOrWhiteSpace(username)) return false;

			return true;
		}
		private  Task<IPrincipal> AutheticateJwtToken(string token)
		{
			if(ValidateToken(token, out string username) && CacheManager.Conatins(username))
			{
                var appUserInfo=CacheManager.GetValue(username) as AppUserInfo;
                if (SessionStorage.Instance[token] == null)
                {
                    SessionStorage.Instance[token] = appUserInfo;
                }
                    
                var currentUser = appUserInfo.Context;
                var identity = new ClaimsIdentity("JWT");
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
                identity
                    .AddClaims(Enumerable
                    .Range(0, currentUser.Roles.Count)
                    .Select(i => new Claim(ClaimTypes.Role, currentUser.Roles[i].Name)));

                IPrincipal user = new ClaimsPrincipal(identity);
				return Task.FromResult(user);
			}
			return Task.FromResult<IPrincipal>(null);
		}

		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			Challenge(context);
			return Task.FromResult(0);
		}

		private void Challenge(HttpAuthenticationChallengeContext context)
		{
			string parameter = null;

			if (!string.IsNullOrEmpty(Realm))
				parameter = "realm=\"" + Realm + "\"";

			context.ChallengeWith("Bearer", parameter);
		}
	}
}