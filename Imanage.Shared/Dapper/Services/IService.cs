using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imanage.Shared.Dapper.Interfaces;
using Dapper;

namespace Imanage.Shared.Dapper.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        string this[string columnName] { get; }

        string Error { get; }
        bool HasError { get; }
        IUnitOfWork UnitOfWork { get; }

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Dispose();
        void Dispose(bool disposing);
        IEnumerable<TEntity> Find(string sql = null, IDictionary<string, object> parameters = null);
        Task<IEnumerable<DTO>> ExecuteStoredProcedure<DTO>(string sql, DynamicParameters parameters);
        TEntity FindById(Guid id);
        IEnumerable<Dto> SqlQuery<Dto>(string sql, object paramaters);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }
}