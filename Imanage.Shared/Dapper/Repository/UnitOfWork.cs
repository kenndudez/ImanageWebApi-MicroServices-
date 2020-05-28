using Imanage.Shared.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _dbTransaction;
        private readonly IDbConnection _connection;

        public UnitOfWork(IDbConnection connectiom)
        {
            _connection = connectiom;
        }

        public IDapperRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new DapperRepository<TEntity>(_connection);
        }

        public void BeginTransaction()
        {
            _dbTransaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_dbTransaction == null)
                return;

            _dbTransaction.Commit();
        }

        public void Rollback()
        {
            if (_dbTransaction == null)
                return;

            _dbTransaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
        }
    }
}
