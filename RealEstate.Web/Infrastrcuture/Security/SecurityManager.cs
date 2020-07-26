using RealEstate.Common;
using RealEstate.Common.Entities.Security;
using RealEstate.Domain.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using static RealEstate.Common.AppConstants;
using RealEstate.Common.Exceptions;

namespace RealEstate.Web.Security
{
	public static class SecurityManager 
	{
        private static ISecurityProvider _provider;
		public static void SetProvider(ISecurityProvider provider)
		{
			_provider = provider;
		}
        public static bool SignIn(string userName, string password)
		{
			if (_provider == null) throw new NullReferenceException("Security provider not set");
			return _provider.SignIn(userName, password);
		}

		public static void SignOut(string token)
		{
			if (_provider == null) throw new NullReferenceException("Security provider not set");
			_provider.SignOut(token);
		}
        public static string GenerateToken(string userName, int expireMinutes = 20)
        {
            if (_provider == null) throw new NullReferenceException("Security provider not set");
            return _provider.GenerateToken(userName,expireMinutes);
        }
        public static ClaimsPrincipal GetPrinciple(string token)
        {
            if (_provider == null) throw new NullReferenceException("Security provider not set");
            return _provider.GetPrinciple(token);
        }
		public static DateTime GetRestOfExpiryDate(string token)
		{
			if (_provider == null) throw new NullReferenceException("Security provider not set");
			return _provider.GetRestOfExpiryDate(token);
		}
		public static double GetRestOfExpiryAsMinute(string token)
		{
			if (_provider == null) throw new NullReferenceException("Security provider not set");
			var tokenExpiryDate = GetRestOfExpiryDate(token);
			TimeSpan timeSpan = tokenExpiryDate - DateTime.Now;
			return timeSpan.TotalMinutes;
		}
		public static bool IsAuthenticated(string token)
		{
			if (_provider == null) throw new NullReferenceException("Security provider not set");
			return _provider.IsAuthenticated(token);
		}
		public static string GetToken(HttpRequestMessage request)
		{
			return request?.Headers?.Authorization?.Parameter;
		}
	}
}