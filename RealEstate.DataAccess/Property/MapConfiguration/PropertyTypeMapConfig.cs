using RealEstate.Common.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Property.MapConfiguration
{
	public class PropertyTypeMapConfig : EntityMapConfig<PropertyType>
	{
		public PropertyTypeMapConfig()
		{
			Property(e => e.Name);
			Property(e => e.IsDeleted);

			ToTable("PropertyType");
		}
	}
}
