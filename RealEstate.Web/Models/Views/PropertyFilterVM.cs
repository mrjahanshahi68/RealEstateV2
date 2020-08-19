using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Views
{
	public class PropertyFilterVM
	{
		public string FullText { get; set; }
		public string RegionName { get; set; }
		public int? PropertyType { get; set; }
		public TransactionType? Type { get; set; }
		public int? Bedroom { get; set; }
		public MinMaxValue SalePrice { get; set; }
		public MinMaxValue MortgagePrice { get; set; }
		public MinMaxValue RentalPrice { get; set; }
		public MinMaxValue LandingArea { get; set; }
		public MinMaxValue BuildingArea { get; set; }
		public bool IsAdvanced { get; set; }
	}
}