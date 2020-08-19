using RealEstate.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Views
{
    public class PropertyDemandCreateVM
	{
		public int PropertyType { get; set; }
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
		public DateTime InserDateTime { get; set; }
		public bool IsDeleted { get; set; }
		public int? InsertUserId { get; set; }
		public string InsertUserName { get; set; }
		public List<State> States { get; set; }
		public List<State> Cities { get; set; }
		public List<State> Regions { get; set; }
	}
}