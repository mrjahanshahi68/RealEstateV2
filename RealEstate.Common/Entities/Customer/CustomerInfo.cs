using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Common.Entities.Customer
{
	public class CustomerInfo : LoggableEntity, ILogicalDeletable
	{
		public string HashKey { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Tel { get; set; }
		public string Mobile { get; set; }
		public CustomerType CustomerType { get; set; }
		public bool IsDeleted { get; set; }
	}
}
