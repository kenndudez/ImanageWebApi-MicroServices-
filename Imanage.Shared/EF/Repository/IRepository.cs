using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.EF.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : AuditedEntity
    {
        IQueryable<TEntity> SqlRawQuery(string query, params object[] parameters);
        IEnumerable<T> SqlQuery<T>(String sql, params object[] parameters) where T : new();
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize);
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending);

        IQueryable<TEntity> GetAllString(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties);

        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        TEntity GetSingle(Guid id);
        TEntity GetSingleIncluding(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetAllAsync();
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PaginatedList<TEntity>> GetAllStringAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector,
          Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties);

        Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByIdIncludingAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate);


        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Get single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert multiple entities to db
        /// </summary>
        /// <param name="entities"></param>
        void InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert Or Update entity to db
        /// </summary>
        /// <param name="entity"></param>
        //void InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        void Delete(TEntity entity);

        void Remove(TEntity entityToDelete);

        long BulkInsert<T>(IEnumerable<T> Data, string TableName, int BatchSize = 500, int NotifySize = 500);
        
    }
}
