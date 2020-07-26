using RealEstate.Common.Entities;
using RealEstate.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RealEstate.Common.AppEnums;

namespace RealEstate.DataAccess
{
	internal class RemoveCastsVisitor : ExpressionVisitor
	{
		private static readonly ExpressionVisitor Default = new RemoveCastsVisitor();

		private RemoveCastsVisitor()
		{
		}

		public new static Expression Visit(Expression node)
		{
			return Default.Visit(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.NodeType == ExpressionType.Convert
				&& node.Type.IsAssignableFrom(node.Operand.Type))
			{
				return base.Visit(node.Operand);
			}
			return base.VisitUnary(node);
		}
	}
	public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
	{
		
		private IDataContext _dbContext;
		private readonly DbSet<TEntity> _dbSet;

		public BaseRepository(IDataContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<TEntity>();
		}
		#region Operations
		public virtual void Delete(TEntity entityToDelete)
		{
            try
            {
                if (entityToDelete is ILogicalDeletable)
                {
                    ILogicalDeletable logicalDeletedEntity = entityToDelete as ILogicalDeletable;
                    logicalDeletedEntity.IsDeleted = true;
                    //if (logicalDeletedEntity is ILoggableEntity)
                    //{
                    //    ILoggableEntity loggableEntity = logicalDeletedEntity as ILoggableEntity;
                    //    loggableEntity.UpdateUserId = -1;
                    //    loggableEntity.UpdateDateTime = DateTime.Now;
                    //}
                    _dbSet.Attach(entityToDelete);
                    _dbContext.Entry(entityToDelete).State = EntityState.Modified;
                }
                else
                {
                    if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                    {
                        _dbSet.Attach(entityToDelete);
                    }
                    _dbSet.Remove(entityToDelete);
                }
                entityToDelete.ObjectState = ObjectState.Deleted;
            }
            catch(Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
		}
		//public virtual void Delete(int id)
		//{
		//	TEntity entityToDelete = Find(id);
		//	if (entityToDelete != null)
		//		Delete(entityToDelete);
		//}
		public virtual void Insert(TEntity entity)
		{
            try
            {
                //if (entity is ILoggableEntity)
                //{
                //    ILoggableEntity loggableEntity = entity as ILoggableEntity;
                //    loggableEntity.InsertDateTime = DateTime.Now;
                //    loggableEntity.InsertUserId = -1;//get from security;
                //}
                _dbSet.Add(entity);
                _dbContext.Entry(entity).State = EntityState.Added;
                entity.ObjectState = ObjectState.Added;
            }
            catch(Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
			
		}
		public virtual void Update(TEntity entityToUpdate)
		{
            try
            {
                TEntity entity = Find(entityToUpdate.ID);
                if (entity != null)
                {
                    _dbSet.Attach(entityToUpdate);
                    _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
                    entityToUpdate.ObjectState = ObjectState.Modified;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
		}
		#endregion

		#region Query
		public virtual IQueryable<TEntity> Queryable(bool containsDeleted = false)
		{
            try
            {
                IQueryable<TEntity> query = _dbSet.AsNoTracking();

                if (containsDeleted)
                    return query;
                else
                {
                    if (query is IQueryable<ILogicalDeletable>)
                    {
                        Expression<Func<TEntity, bool>> deleteClause = x => !((ILogicalDeletable)x).IsDeleted;
                        deleteClause = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(deleteClause);
                        return query.Where(deleteClause);
                    }
                    return _dbSet.AsNoTracking().AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
		}
		public virtual TEntity Find(int id)
		{
            try
            {
                IQueryable<TEntity> query = _dbSet.AsNoTracking();
                if (query is IQueryable<ILogicalDeletable>)
                {
                    var logicalDeletedQuery = query as IQueryable<ILogicalDeletable>;
                    Expression<Func<TEntity, bool>> deleteClause = x => !((ILogicalDeletable)x).IsDeleted;
                    deleteClause = (Expression<Func<TEntity, bool>>)RemoveCastsVisitor.Visit(deleteClause);
                    return query.Where(deleteClause).Where(e => e.ID == id).SingleOrDefault();
                }
                return query.Where(e => e.ID == id).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, ex);
            }
            
		}
		#endregion
	}
}
