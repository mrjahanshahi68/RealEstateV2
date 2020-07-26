using RealEstate.Common;
using RealEstate.Common.Entities;
using RealEstate.Common.Entities.Security;
using RealEstate.Web.Infrastrcuture;
using RealEstate.Domain.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using RealEstate.Web.Cache;
using RealEstate.Common.Exceptions;
using static RealEstate.Common.AppConstants;

namespace RealEstate.Web.Security
{
	public class JwtSecurityProvider : ISecurityProvider
	{
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
        public bool SignIn(string userName, string password)
		{
			var userRule = new UserBusinessRule();
			var user= userRule.FindUserByUserName(userName);
			if (user == null) return false;
			if (user.Password != password.ToMd5().ToBase64()) return false;
			var roles = new RoleBusinessRule().FindRolesByUserId(user.ID);
			user.Roles = roles;
			var appUserInfo = new AppUserInfo
			{
				Context = user,
				UserId = user.ID,
			};
            CacheManager.SetValue(userName, appUserInfo);
            return true;
		}
        public void SignOut(string token)
		{
			var principle = GetPrinciple(token);
			var userName = principle.Identity?.Name;
			CacheManager.Remove(userName);
		}
		
        public  string GenerateToken(string userName, int expireMinutes = AppConstants.ExpireMinutes)
        {
           
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(symmetricKey);
            var appUserInfo = CacheManager.GetValue(userName) as AppUserInfo;
            var currentUser = appUserInfo.Context;
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("iss", AppConfigurations.Issuer));
			var role =string.Join(",", currentUser.Roles.Select(e => e.Name).ToList());
			identity.AddClaim(new Claim(ClaimTypes.Role,role));
			

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject= identity,
                Expires = now.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
			
            return jwtToken;
        }
        public  ClaimsPrincipal GetPrinciple(string token)
        {
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null) return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var securityKey = new SymmetricSecurityKey(symmetricKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateAudience = false,
                    IssuerSigningKey = securityKey,
                    ValidIssuer = AppConfigurations.Issuer,
					
                };
				
				var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }
            catch
            {
                return null;
            }

        }
		public  DateTime GetRestOfExpiryDate(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
			var tokenExpiryDate = jwtToken.ValidTo.ToLocalTime();
			return tokenExpiryDate;
			
		}

		public bool IsAuthenticated(string token)
		{
			var tokenExpiryDate = GetRestOfExpiryDate(token);
			TimeSpan timeSpan = tokenExpiryDate - DateTime.Now;
			var restOfExpireMinute = timeSpan.TotalMinutes;
			if (restOfExpireMinute < 0)
				return true;
			return false;
		}
		
	}
}