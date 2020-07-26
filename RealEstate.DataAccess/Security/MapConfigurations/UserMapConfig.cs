using RealEstate.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppConstants;

namespace RealEstate.DataAccess.Security.MapConfigurations
{
	public class UserMapConfig : LoggableEntityMapConfig<User>
	{
		public UserMapConfig()
		{
			Ignore(e => e.Roles);

			Property(e => e.FirstName);
			Property(e => e.LastName);
			Property(e => e.NationalCode);
			Property(e => e.Mobile);
			Property(e => e.Address);
			Property(e => e.UserName);
			Property(e => e.Password);
			Property(e => e.UserType);
			Property(e => e.IsActive);
			Property(e => e.IsDeleted);
			Property(e => e.RegisterDate);

			ToTable("Users");
		}
	}
}
