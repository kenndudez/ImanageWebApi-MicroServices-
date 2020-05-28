using Microsoft.EntityFrameworkCore;
using Dafmis.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace Imanage.Shared
{
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> ExecuteSqlQuery<TElement>(string sql, params object[] parameters) where TElement : new();
        string CreateDatabaseScript();
        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : AuditedEntity, new();

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        IQueryable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : AuditedEntity;

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        Int32 ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters);

        DbSet<TEntity> Set<TEntity>() where TEntity : AuditedEntity;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : AuditedEntity;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : AuditedEntity;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : AuditedEntity;
        void SetAsDetached<TEntity>(TEntity entity) where TEntity : AuditedEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> CommitAsync();

        long BulkInsert(DataTable Data, string TableName, int BatchSize, int NotifySize);
    }
}