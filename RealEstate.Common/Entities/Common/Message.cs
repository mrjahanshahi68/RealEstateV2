using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Entities.Common
{
	public class Message : LoggableEntity, ILogicalDeletable
	{
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
		public string Text { get; set; }
		public bool IsDeleted { get; set; }
	}
}
