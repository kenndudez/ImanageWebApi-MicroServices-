using Imanage.Shared.EF.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Imanage.Shared.EF.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : AuditedEntity
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        private readonly IRepository<TEntity> _repository;
        private bool _disposed;
        protected List<ValidationResult> results = new List<ValidationResult>();

        public Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _repository = UnitOfWork.Repository<TEntity>();
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : new()
        {
            return _repository.SqlQuery<T>(sql, parameters);
        }


        public IQueryable<TEntity> SqlRawQuery(string sql, params object[] parameters)
        {
            return _repository.SqlRawQuery(sql, parameters);
        }

        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null),
              results, false);
        }

        public TEntity SingleOrDefault(Func<TEntity, bool> predicate)
        {
            return _repository.GetAll().SingleOrDefault(predicate);
        }

        public TEntity SingleOrDefault()
        {
            return _repository.GetAll().SingleOrDefault();
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return _repository.GetAll().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> All { get { return _repository.Query(); } }

        public TEntity FirstOrDefault()
        {
            return _repository.GetAll().FirstOrDefault();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize)
        {
            return _repository.GetAll(pageIndex, pageSize);
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return _repository.GetAll(pageIndex, pageSize, keySelector, orderBy);
        }

        public IQueryable<TEntity> GetAllString(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties)
        {

            return _repository.GetAllString(pageIndex, pageSize, keySelector, predicate, orderBy, includeProperties);
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetAll(pageIndex, pageSize, keySelector, predicate, orderBy, includeProperties);
        }

        public TEntity GetById(Guid id)
        {
            return _repository.GetSingle(id);
        }

        public TEntity GetById(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.GetSingleIncluding(id, includeProperties);
        }
        

        public void Add(TEntity entity)
        {
            _repository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _repository.InsertRange(entities);
            UnitOfWork.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity is AuditedEntity)
                _repository.Remove(entity);
            else
                _repository.Delete(entity);
            UnitOfWork.SaveChanges();
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _repository.GetAllAsync(pageIndex, pageSize);
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return await _repository.GetAllAsync(pageIndex, pageSize, keySelector, orderBy);
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _repository.GetAllAsync(pageIndex, pageSize, keySelector, predicate, orderBy, includeProperties);
        }


        public PaginatedList<T> ApplyPagination<T>(IQueryable<T> query, int pageSize, int pageIndex)
        {
            if (typeof(IOrderedQueryable<T>).IsAssignableFrom(query.Expression.Type))
                throw new Exception("Query not ordered");

            var totalRecordsQuery = query.Select(x => 1).DeferredCount().FutureValue();
            var dataFuture = query.Future();
            //var dataFuture = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Future();
            var data = dataFuture.ToList();
            var totalRecords = totalRecordsQuery.Value;

            return new PaginatedList<T>(data, pageIndex, pageSize, totalRecords);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Int32> AddAsync(TEntity entity)
        {
            _repository.Insert(entity);
            return await UnitOfWork.SaveChangesAsync();
        }

        public async Task<Int32> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _repository.InsertRange(entities);
            return await UnitOfWork.SaveChangesAsync();
        }

        public async Task<Int32> UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            return await UnitOfWork.SaveChangesAsync();
        }

        public async Task<Int32> DeleteAsync(TEntity entity)
        {
            _repository.Delete(entity);
            return await UnitOfWork.SaveChangesAsync();
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                UnitOfWork.Dispose();
            }
            _disposed = true;
        }

        public IEnumerable<ValidationResult> Errors
        {
            get
            {
                if (results.Count > 0)
                {
                    return results;
                }
                return Enumerable.Empty<ValidationResult>();
            }
        }

        public string this[string columnName]
        {
            get
            {
                var validatioResult = results.FirstOrDefault(r => r.MemberNames.FirstOrDefault() == columnName);
                return validatioResult == null ? string.Empty : validatioResult.ErrorMessage;
            }
        }

        public Boolean HasError
        {
            get
            {
                if (results.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
