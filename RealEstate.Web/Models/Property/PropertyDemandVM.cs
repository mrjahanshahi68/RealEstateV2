using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Property
{
    public class PropertyDemandVM
    {
		public int? ID { get; set; }
		public int PropertyType { get; set; }
		public string PropertyTypeName { get; set; }
		public TransactionType TransactionType { get; set; }
		public int MinimumArea { get; set; }
		public int MaximumArea { get; set; }
		public int MinimumPrice { get; set; }
		public int MaximumPrice { get; set; }
		public string MobileNumber { get; set; }
		public string FullName { get; set; }
		public int? ProvinceId { get; set; }
		public int? CityId { get; set; }
		public int? StateId { get; set; }
		public string RegionName { get; set; }
		public string CityName { get; set; }
		public DateTime InserDateTime { get; set; }
		public bool IsDeleted { get; set; }
		public int? InsertUserId { get; set; }
		public string InsertUserName { get; set; }
	}
}