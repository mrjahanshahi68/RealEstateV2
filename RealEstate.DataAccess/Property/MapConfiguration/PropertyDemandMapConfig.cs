using RealEstate.Common.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Property.MapConfiguration
{
	public class PropertyDemandMapConfig : EntityMapConfig<PropertyDemand>
	{
		public PropertyDemandMapConfig()
		{
			Property(e => e.PropertyType);
			Property(e => e.TransactionType);
			Property(e => e.MaximumArea);
			Property(e => e.MinimumArea);
			Property(e => e.MinimumPrice);
			Property(e => e.MaximumPrice);
			Property(e => e.FullName);
			Property(e => e.MobileNumber);
			Property(e => e.StateId);
			Property(e => e.InsertUserId);
			Property(e => e.IsDeleted);

			ToTable("PropertyDemands");
		}
	}
}
