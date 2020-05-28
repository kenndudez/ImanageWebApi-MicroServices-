using Imanage.Shared.EF;
using Imanage.Shared.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.EF.Services
{
    public interface IService<TEntity> : IDisposable where TEntity : AuditedEntity
    {
        IEnumerable<T>  SqlQuery<T>(string sql, params object[] parameters) where T: new();
        IQueryable<TEntity> SqlRawQuery(string sql, params object[] parameters);
        TEntity SingleOrDefault(Func<TEntity, bool> predicate);
        TEntity SingleOrDefault();
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Func<TEntity, bool> predicate);

        IUnitOfWork UnitOfWork { get; }
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize);
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        PaginatedList<T> ApplyPagination<T>(IQueryable<T> query, int pageSize, int pageIndex);

        TEntity GetById(Guid id);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<Int32> AddAsync(TEntity entity);
        Task<Int32> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<Int32> UpdateAsync(TEntity entity);
        Task<Int32> DeleteAsync(TEntity entity);
    }
}
