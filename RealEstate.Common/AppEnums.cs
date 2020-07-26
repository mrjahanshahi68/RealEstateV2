using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common
{
	public static class AppEnums
	{
		public enum ObjectState
		{
			Unchanged = 2,
			Added = 4,
			Deleted = 8,
			Modified = 16
		}
		public enum UserTypes
		{
			Administrator = 1,
			User=2,
			Developer=1000,
			//Students = 2,
			//Teachers = 3,
			//Advisers = 4,
			//Supporters = 5,
			//Developers = 1000,
		}
		public enum ResultCode
		{
			Success = 1,
            ValidationError = 2,
            InternalServerError = 3,
			UnAuthenticated = 4,
			UnAuthorized = 5,
		}

		public enum CustomerType
		{
			PropertyOwner=1,
			PropertyApplicant=2,
		}
		public enum ObjectType
		{
			Property=1,
		}
		public enum DocumentType
		{
			Registered=1,

		}
		public enum TransactionType
		{
			Rent =1,
			Sale =2,
			PreSel=3,
			Exchange=4,
		}
		public enum PropertyStatus
		{
			Submitted=1,
			Approved=2,
			Sold=3,
			Leased= 4,
			Rejected=5,
		}
	}
}
