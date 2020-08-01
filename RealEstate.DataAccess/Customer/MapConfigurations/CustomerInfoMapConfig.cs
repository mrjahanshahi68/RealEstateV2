using RealEstate.Common.Entities.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Customer.MapConfigurations
{
	public class CustomerInfoMapConfig : LoggableEntityMapConfig<CustomerInfo>
	{
		public CustomerInfoMapConfig()
		{
			Property(e => e.HashKey);
			Property(e => e.FirstName);
			Property(e => e.LastName);
			Property(e => e.Tel);
			Property(e => e.Mobile);
			Property(e => e.CustomerType);
			Property(e => e.IsDeleted);

			ToTable("CustomerInfos");
		}
	}
}
