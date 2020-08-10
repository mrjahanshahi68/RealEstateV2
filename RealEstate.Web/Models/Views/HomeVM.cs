using RealEstate.Common.Entities.Common;
using RealEstate.Web.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models.Views
{
	public class HomeVM
	{
		public List<PropertyInfoVM> LatestProperties { get; set; }
		public List<PropertyInfoVM> LuxuryProperties { get; set; }
		public List<State> States { get; set; }
		public List<State> Cities { get; set; }
		public List<State> Regions { get; set; }
		public List<PropertyTypeVM> PropertyTypes { get; set; }
		public MinMaxValue MinMaxPropertySalePrice { get; set; }
		public MinMaxValue MinMaxPropertyMortgagePrice { get; set; }
		public MinMaxValue MinMaxPropertyRentalPrice { get; set; }
		public MinMaxValue MinMaxPropertyLandArea { get; set; }
		public MinMaxValue MinMaxPropertyBuildingArea { get; set; }

	}
}