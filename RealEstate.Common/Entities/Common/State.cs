using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Entities.Common
{
	public class State : BaseEntity,ILogicalDeletable
	{
		public string Name { get; set; }
		public int? ParentId { get; set; }
		public int Level { get; set; }
		public bool IsDeleted { get; set; }
	}
}
