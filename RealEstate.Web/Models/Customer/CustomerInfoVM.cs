using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Customer
{
	public class CustomerInfoVM
	{
		public int ID { get; set; }
		public string HashKey { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Tel { get; set; }
		public string Mobile { get; set; }
		public CustomerType CustomerType { get; set; }
		public bool IsDeleted { get; set; }
	}
}