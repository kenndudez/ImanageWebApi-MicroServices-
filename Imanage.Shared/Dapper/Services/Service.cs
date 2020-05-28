using Imanage.Shared.Dapper.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imanage.Shared.Dapper.Services;

namespace Dafmis.Shared.Dapper.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        protected readonly IDapperRepository<TEntity> _dapperRepository;
        private bool _disposed;
        protected Dictionary<String, String> _errors = new Dictionary<String, String>();
        protected List<ValidationResult> results = new List<ValidationResult>();

        public Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _dapperRepository = UnitOfWork.Repository<TEntity>();
        }

        public async Task<IEnumerable<DTO>> ExecuteStoredProcedure<DTO>(string sql, DynamicParameters parameters)
        {
            return await  _dapperRepository.ExecuteStoredProcedure<DTO>(sql, parameters);
        }
        public IEnumerable<Dto> SqlQuery<Dto>(string sql, object paramaters)
        {
            return _dapperRepository.Connection.Query<Dto>(sql, paramaters);
        }

        public TEntity FindById(Guid id)
        {
            return _dapperRepository.GetById(id);
        }

        public IEnumerable<TEntity> Find(string sql = null, IDictionary<string, object> parameters = null)
        {
            return _dapperRepository.Find(sql, parameters);
        }

        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null),
              results, false);
        }

        public void Add(TEntity entity)
        {
            _dapperRepository.Create(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dapperRepository.CreateMany(entities);
        }

        public void Update(TEntity entity)
        {
            _dapperRepository.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dapperRepository.Delete(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dapperRepository.CreateAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dapperRepository.CreateManyAsync(entities);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _dapperRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await _dapperRepository.DeleteAsync(entity);
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

        public string Error
        {
            get
            {
                if (_errors.Count > 0)
                {
                    return _errors.FirstOrDefault().Value;
                }
                return String.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                {
                    return _errors[columnName];
                }

                return String.Empty;
            }
        }

        public Boolean HasError
        {
            get
            {
                if (_errors.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
