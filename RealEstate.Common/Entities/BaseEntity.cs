using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Common.Entities
{
	public class BaseEntity : IEntity
	{
		public int ID { get; set; }
		public ObjectState ObjectState { get; set; }
	}
}
