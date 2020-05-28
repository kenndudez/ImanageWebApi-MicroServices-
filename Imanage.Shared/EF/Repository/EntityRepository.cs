using Dafmis.Shared;
using Imanage.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Imanage.Shared.EF.Repository
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : AuditedEntity
    {
        private IDbContext context;
        private DbSet<TEntity> _dbSet;

        public EntityRepository(IDbContext context)
        {
            this.context = context;
        }

        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = context.Set<TEntity>();
                }

                return _dbSet;
            }
        }

        //public virtual IEnumerable<T> ExecuteSqlQuery<T>(string query, params object[] parameters)
        //     where T : BaseEntity, new()
        //{
        //    return context.ExecuteSqlQuery<T>(query, parameters);
        //}

        public virtual IQueryable<TEntity> SqlRawQuery(string query, params object[] parameters)
        {
            return context.SqlQuery<TEntity>(query, parameters);
        }

        public virtual IEnumerable<T> SqlQuery<T>(String sql, params object[] parameters) where T: new()
        {
            return context.ExecuteSqlQuery<T>(sql, parameters);
        }

        public TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize)
        {
            return GetAll(pageIndex, pageSize, x => x.Id);
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return GetAll(pageIndex, pageSize, keySelector, null, orderBy);
        }

        public IQueryable<TEntity> GetAllString(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties)
        {
            var entities = FilterQueryString(keySelector, predicate, orderBy, includeProperties);
            return entities.Paginate(pageIndex, pageSize);
        }

        public IQueryable<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            return entities.Paginate(pageIndex, pageSize);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entities;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities;
        }

        public TEntity GetSingle(Guid id)
        {
            return Entities.SingleOrDefault(t => t.Id == id);
        }

        public TEntity GetSingleIncluding(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.FirstOrDefault(x => x.Id == id);
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return Entities.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> GetByIdIncludingAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return await entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await GetAllAsync(pageIndex, pageSize, x => x.Id);
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return await GetAllAsync(pageIndex, pageSize, keySelector, null, orderBy);
        }

        public async Task<PaginatedList<TEntity>> GetAllStringAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector,
           Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params string[] includeProperties)
        {
            var entities = FilterQueryString(keySelector, predicate, orderBy, includeProperties);
            var total = await entities.CountAsync();// entities.CountAsync() is different than pageSize
            entities = entities.Paginate(pageIndex, pageSize);
            var list = await entities.ToListAsync();
            return list.ToPaginatedList(pageIndex, pageSize, total);
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, Guid>> keySelector,
            Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            var total = await entities.CountAsync();// entities.CountAsync() is different than pageSize
            entities = entities.Paginate(pageIndex, pageSize);
            var list = await entities.ToListAsync();
            return list.ToPaginatedList(pageIndex, pageSize, total);
        }

        public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, string>>[] includeProperties)
        {
            return GetAllIncludingAsync(includeProperties);
        }

        public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.ToListAsync();
        }

        private IQueryable<TEntity> FilterQueryString(Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
         string[] includeProperties)
        {
            var entities = IncludeStringProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return entities;
        }

        private IQueryable<TEntity> FilterQuery(Expression<Func<TEntity, Guid>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
         Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return entities;
        }

        private IQueryable<TEntity> IncludeStringProperties(string[] includeProperties)
        {
            IQueryable<TEntity> entities = null ;
            foreach (var includeProperty in includeProperties)
            {
                if (entities == null)
                    entities = Entities.Include(includeProperty);
                else
                    entities = entities.Include(includeProperty);
            }
            return entities;
        }


        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = Entities;
            //Todo: a better way to write this
            //http://appetere.com/post/Passing-Include-statements-into-a-Repository
            //not tested before will do that later looks shorter
            //entities  = includeProperties.Aggregate(entities, (current, include) => current.Include(include));

            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
        }

        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = Entities;
            return query.Any(filter);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            context.SetAsModified<TEntity>(entityToUpdate);
        }

        public void Insert(TEntity entity)
        {
            Entities.Add(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        //public void InsertOrUpdate(TEntity entity)
        //{
        //    dbSet.AddOrUpdate(entity);
        //}

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return Entities.Where(where).ToList();
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return Entities.Where(where).AsQueryable();
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = Entities;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entities;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(filter);
        }

        public void Remove(TEntity entityToDelete)
        {
            context.SetAsDeleted<TEntity>(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            context.SetAsDetached<TEntity>(entityToDelete);
        }

        public long BulkInsert<T>(IEnumerable<T> Data, string TableName, int BatchSize = 500, int NotifySize = 500)
        {
            var dataTable = Data.ToDataTable();
            return context.BulkInsert(dataTable, TableName, BatchSize, NotifySize);
        }

        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
