using RealEstate.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public class UnitOfWork<TContext> : IUnitOfWork, IDisposable where TContext : IDataContext, new()
	{
		private TContext _dbContext;
		private readonly Dictionary<Type, object> _repositories;
		public UnitOfWork()
		{
			_dbContext = new TContext();
			_repositories = new Dictionary<Type, object>();
			disposedValue = false;
		}
		public UnitOfWork(TContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Dictionary<Type, object>();
			disposedValue = false;
		}

		public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
		{
			if (_repositories.Keys.Contains(typeof(TEntity)))
			{
				return _repositories[typeof(TEntity)] as IRepository<TEntity>;
			}
			var repositoryType = typeof(BaseRepository<>);

			var repositoryInstance =
			Activator.CreateInstance(repositoryType
				.MakeGenericType(typeof(TEntity)), _dbContext);


			_repositories.Add(typeof(TEntity), repositoryInstance);

			return (IRepository<TEntity>)_repositories[typeof(TEntity)];
		}
		public void BeginTransaction()
		{
			_dbContext.Database.BeginTransaction();
		}

		public void Commit()
		{
			_dbContext.Database.CurrentTransaction.Commit();
		}

		public void Rollback()
		{
			_dbContext.Database.CurrentTransaction.Rollback();
		}

		public int SaveChange()
		{
			return _dbContext.SaveChanges();
		}

		public Task<int> SaveChangeAsync()
		{
			return _dbContext.SaveChangesAsync();
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
				disposedValue = true;
			}
		}
		void IDisposable.Dispose()
		{
			Dispose(true);
			System.GC.SuppressFinalize(this);
		}
		#endregion
	}
}
