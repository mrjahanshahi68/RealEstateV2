using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Entities
{
	public  class LoggableEntity : BaseEntity,ILoggableEntity
	{
		public int InsertUserId { get; set; }
		public DateTime InsertDateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public DateTime? UpdateDateTime { get; set; }
	}
}
