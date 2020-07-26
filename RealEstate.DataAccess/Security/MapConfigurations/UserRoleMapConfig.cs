using RealEstate.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppConstants;

namespace RealEstate.DataAccess.Security.MapConfigurations
{
	public class UserRoleMapConfig : EntityMapConfig<UserRole>
	{
		public UserRoleMapConfig()
		{
			Property(e => e.UserId);
			Property(e => e.RoleId);

			ToTable("UserRoles");
		}
	}
}
