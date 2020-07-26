using RealEstate.Common.Entities.Security;
using RealEstate.DataAccess.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace RealEstate.Web.Security
{
	public interface ISecurityProvider
	{
		bool IsAuthenticated(string token);
		bool SignIn(string userName, string password);
		void SignOut(string token);
        string GenerateToken(string userName, int expireMinutes = 20);
        ClaimsPrincipal GetPrinciple(string token);
		DateTime GetRestOfExpiryDate(string token);


	}
}