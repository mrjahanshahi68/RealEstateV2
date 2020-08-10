using RealEstate.Common.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Property.MapConfiguration
{
	public class PropertyInfoMapConfig : LoggableEntityMapConfig<PropertyInfo>
	{
		public PropertyInfoMapConfig()
		{
			Property(e => e.PropertyTypeId);
			Property(e => e.DocumentTypeId);
			//Property(e => e.OwnerId);
			Property(e => e.HashKey);
			Property(e => e.PropertyCode);
			Property(e => e.Title);
			Property(e => e.UrlTitle);
			Property(e => e.Status);
			Property(e => e.Type);
			Property(e => e.BuildingArea);
			Property(e => e.LandingArea);
			Property(e => e.BathRoom);
			Property(e => e.BedRoom);
			Property(e => e.Floor);
			Property(e => e.FloorNumber);
			Property(e => e.Apartment);
			Property(e => e.Position);
			Property(e => e.PropertyView);
			Property(e => e.ConstructorYear);
			Property(e => e.Welfares);
			Property(e => e.RegionId);
			Property(e => e.Address);
			Property(e => e.Description);
			Property(e => e.SalePrice);
			Property(e => e.MortgagePrice);
			Property(e => e.RentalPrice);
			Property(e => e.FirstName);
			Property(e => e.LastName);
			Property(e => e.Tel);
			Property(e => e.ReagentFirstName);
			Property(e => e.ReagentLastName);
			Property(e => e.ReagentTel);
			//Property(e => e.IsLuxury);
			//Property(e => e.IsShowInSlide);
			Property(e => e.IsDeleted);

			Property(e => e.SlideImage);
			Property(e => e.CoverImage);

			ToTable("Properties");
		}
	}
}
