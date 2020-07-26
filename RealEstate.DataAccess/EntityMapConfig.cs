using RealEstate.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public class EntityMapConfig<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : class, IEntity
	{
		public EntityMapConfig()
		{
			Ignore(e => e.ObjectState);

			Property(e => e.ID).IsRequired();
		}
	}
}
