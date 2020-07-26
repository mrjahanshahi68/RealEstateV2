using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Common.Entities
{
	public interface IEntity
	{
		int ID { get; set; }
		ObjectState ObjectState { get; set; }
	}
}
