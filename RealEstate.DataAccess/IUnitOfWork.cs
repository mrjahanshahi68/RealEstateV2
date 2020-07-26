using RealEstate.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public interface IUnitOfWork
	{
		IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
		Task<int> SaveChangeAsync();
		int SaveChange();
		void BeginTransaction();
		void Commit();
		void Rollback();
	}
}
