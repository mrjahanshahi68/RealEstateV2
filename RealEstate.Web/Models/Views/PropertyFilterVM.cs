using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Views
{
	public class PropertyFilterVM
	{
		public string RegionName { get; set; }
		public int? PropertyType { get; set; }
		public TransactionType? Type { get; set; }
		public int? Bedroom { get; set; }
	}
}