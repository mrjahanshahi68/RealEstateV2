using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Entities
{
	public interface ILoggableEntity :IEntity	{
		int InsertUserId { get; set; }
		DateTime InsertDateTime { get; set; }
		int? UpdateUserId { get; set; }
		DateTime? UpdateDateTime { get; set; }
	}
}
