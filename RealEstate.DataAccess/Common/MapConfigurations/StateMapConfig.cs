using RealEstate.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Common.MapConfigurations
{
	public class StateMapConfig : EntityMapConfig<State>
	{
		public StateMapConfig()
		{
			Property(e => e.Name);
			Property(e => e.ParentId);
			Property(e => e.Level);
			Property(e => e.IsDeleted);

			ToTable("States");
		}
	}
}
