using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Common.Entities.Property
{
	public class PropertyDemand : BaseEntity, ILogicalDeletable
	{
		public int PropertyType { get; set; }
		public TransactionType TransactionType { get; set; }
		public int MinimumArea { get; set; }
		public int MaximumArea { get; set; }
		public int MinimumPrice { get; set; }
		public int MaximumPrice { get; set; }
		public string MobileNumber { get; set; }
		public string FullName { get; set; }
		public int StateId { get; set; }
		public DateTime InserDateTime { get; set; }
		public int? InsertUserId { get; set; }
		public bool IsDeleted { get; set; }
	}
}
