using RealEstate.Common.Entities;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain
{
	public class BaseBusinessRule<TEntity> : IBusinessRule<TEntity> where TEntity : class, IEntity 
	{
        public AppUserInfo UserContext { get; set; } 
		public IUnitOfWork UnitOfWork { get; set; }
		public IRepository<TEntity> Repository
		{
			get
			{
				return UnitOfWork.Repository<TEntity>();
			}
		}
		public BaseBusinessRule(IUnitOfWork unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
		}
		public BaseBusinessRule()
		{   
            
		}
		public virtual TEntity FindEntity(int id)
		{
			return Repository.Find(id);
		}
		public virtual IQueryable<TEntity> Queryable(bool containsDeletedData = false)
		{
			return Repository.Queryable(containsDeletedData);
		}
		public virtual void InsertEntity(TEntity entity)
		{
			Repository.Insert(entity);
            UnitOfWork.SaveChange();
		}
		public virtual void UpdateEntity(TEntity entity)
		{
			Repository.Update(entity);
            UnitOfWork.SaveChange();
        }
        public virtual void DeleteEntity(TEntity entity)
        {
            Repository.Delete(entity);
            UnitOfWork.SaveChange();
        }
	}
}
