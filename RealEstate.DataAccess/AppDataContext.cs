using RealEstate.DataAccess.Common.MapConfigurations;
using RealEstate.DataAccess.Customer.MapConfigurations;
using RealEstate.DataAccess.Property.MapConfiguration;
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

			#region Common
			modelBuilder.Configurations.Add(new AttachmentMapConfig());
			modelBuilder.Configurations.Add(new ContactUsMapConfig());
			modelBuilder.Configurations.Add(new BlogMapConfig());
			modelBuilder.Configurations.Add(new MessageMapConfig());
			modelBuilder.Configurations.Add(new StateMapConfig());
			#endregion

			#region Customer
			modelBuilder.Configurations.Add(new CustomerInfoMapConfig());
			#endregion

			#region Property
			modelBuilder.Configurations.Add(new PropertyInfoMapConfig());
			modelBuilder.Configurations.Add(new PropertyTypeMapConfig());
			modelBuilder.Configurations.Add(new DocumentTypeMapConfig());
			modelBuilder.Configurations.Add(new WelfareMapConfig());
			modelBuilder.Configurations.Add(new PropertyDemandMapConfig());
			#endregion




			base.OnModelCreating(modelBuilder);
		}
	}
}
