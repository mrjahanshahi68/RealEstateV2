using RealEstate.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public class LoggableEntityMapConfig<TEntity> : EntityMapConfig<TEntity> where TEntity : class, ILoggableEntity
	{
		public LoggableEntityMapConfig() : base()
		{
			Property(e => e.InsertDateTime).IsRequired();
			Property(e => e.InsertUserId).IsRequired();
			Property(e => e.UpdateDateTime);
			Property(e => e.UpdateUserId);
		}
	}
}
