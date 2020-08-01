using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Entities.Property
{
	public class Welfare : BaseEntity, ILogicalDeletable
	{
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
	}
}
