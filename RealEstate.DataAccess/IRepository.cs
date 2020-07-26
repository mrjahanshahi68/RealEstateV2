using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess
{
	public interface IRepository<TEntity> where TEntity : class
	{
		void Delete(TEntity entityToDelete);
		//void Delete(int id);
		void Insert(TEntity entity);
		void Update(TEntity entityToUpdate);
		TEntity Find(int id);
		IQueryable<TEntity> Queryable(bool cotainsDeleted=false);
	}
}
