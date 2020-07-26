using RealEstate.DataAccess.Security.MapConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public class AppDataContext : DataContext
	{
		public AppDataContext() : base("RealEstateConnectionString")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			#region Security
			modelBuilder.Configurations.Add(new UserMapConfig());
			modelBuilder.Configurations.Add(new RoleMapConfig());
			modelBuilder.Configurations.Add(new UserRoleMapConfig());
			#endregion

			base.OnModelCreating(modelBuilder);
		}
	}
}
