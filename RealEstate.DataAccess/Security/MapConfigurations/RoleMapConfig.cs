using RealEstate.Common.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppConstants;

namespace RealEstate.DataAccess.Security.MapConfigurations
{
	public class RoleMapConfig : EntityMapConfig<Role>
	{
		public RoleMapConfig()
		{
			Property(e => e.Name);
			Property(e => e.IsDeleted);

			ToTable("Roles");
		}
	}
}
