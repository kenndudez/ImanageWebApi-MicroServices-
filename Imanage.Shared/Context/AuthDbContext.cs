using Imanage.Shared.EF;
using Imanage.Shared.Extensions;
using Imanage.Shared.Models;
using Imanage.Shared.Models.Map;
using Imanage.Shared.Timing;
using Imanage.Shared.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Imanage.Shared.Context
{
    public class AuthDbContext : IdentityDbContext<ImanageUser, ImanageRole, Guid, ImanageUserClaim, ImanageUserRole, ImanageUserLogin, ImanageRoleClaim, ImanageUserToken>, IDbContext
    {
        private IDbContextTransaction _transaction;

        #region update
        public IGuidGenerator GuidGenerator { get; set; }

        private void SetNullsForInjectedProperties()
        {
            GuidGenerator = SequentialGuidGenerator.Instance;
        }

        public long BulkInsert(DataTable Data, string TableName, int BatchSize, int NotifySize)
        {
            SqlConnection sqlCon = (SqlConnection)Database.GetDbConnection();
            long insertedCount = 0;
            using (sqlCon)
            {
                try
                {
                    sqlCon.Open();
                    var transaction = sqlCon.BeginTransaction();
                    using (SqlBulkCopy sqlCopy = new SqlBulkCopy(sqlCon))
                    {

                        Data.Columns.Cast<DataColumn>().ToList().ForEach(f =>
                        {
                            SqlBulkCopyColumnMapping bccm = new SqlBulkCopyColumnMapping();
                            bccm.DestinationColumn = f.ColumnName;
                            bccm.SourceColumn = f.ColumnName;
                            sqlCopy.ColumnMappings.Add(bccm);
                        });
                        sqlCopy.NotifyAfter = NotifySize;
                        sqlCopy.SqlRowsCopied += (sender, eventArgs) => insertedCount = eventArgs.RowsCopied;
                        sqlCopy.DestinationTableName = TableName;
                        sqlCopy.BatchSize = BatchSize;
                        sqlCopy.WriteToServer(Data);
                    }
                    return insertedCount;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

        private static MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(AuthDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null && ShouldFilterEntity<TEntity>(entityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }


            return false;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
         where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted;
            expression = softDeleteFilter;

            return expression;
        }

        protected virtual void ApplyAbpConcepts(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyAbpConceptsForAddedEntity(entry);
                    break;
                case EntityState.Modified:
                    ApplyAbpConceptsForModifiedEntity(entry);
                    break;
                case EntityState.Deleted:
                    ApplyAbpConceptsForDeletedEntity(entry);
                    break;
            }

        }

        protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
        {
            CheckAndSetId(entry);
            SetCreationAuditProperties(entry.Entity);
        }

        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry)
        {
            SetModificationAuditProperties(entry.Entity);
            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                SetDeletionAuditProperties(entry.Entity);
            }
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
        {
            CancelDeletionForSoftDelete(entry);
            SetDeletionAuditProperties(entry.Entity);
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            //Set GUID Ids
            var entity = entry.Entity as IEntity;
            if (entity != null && entity.Id == Guid.Empty)
            {
                var dbGeneratedAttr = ReflectionHelper
                    .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                    entry.Property("Id").Metadata.PropertyInfo
                    );

                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    entity.Id = GuidGenerator.Create();
                }
            }
        }

        protected virtual void SetCreationAuditProperties(object entityAsObj)
        {
            if (!(entityAsObj is IDateAudit entityWithCreationTime))
            {
                //Object does not implement IHasCreationTime
                return;
            }

            if (entityWithCreationTime.CreatedOn == default(DateTime))
            {
                entityWithCreationTime.CreatedOn = Clock.Now;
            }
        }

        protected virtual void SetModificationAuditProperties(object entityAsObj)
        {
            if (entityAsObj is IDateAudit)
            {
                entityAsObj.As<IDateAudit>().ModifiedOn = Clock.Now;
            }
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
        }

        protected virtual void SetDeletionAuditProperties(object entityAsObj)
        {
            if (entityAsObj is ISoftDelete)
            {
                var entity = entityAsObj.As<ISoftDelete>();

                if (entity.DeletedOn == null)
                {
                    entity.DeletedOn = Clock.Now;
                }
            }
        }

        public override int SaveChanges()
        {
            try
            {
                ApplyAbpConcepts();
                var result = base.SaveChanges();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                ApplyAbpConcepts();
                var result = await base.SaveChangesAsync(cancellationToken);
                //await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        protected virtual void ApplyAbpConcepts()
        {
            //var userId = GetAuditUserId();

            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                ApplyAbpConcepts(entry);
            }

        }


        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }

                return base.Visit(node);
            }
        }

        public AuthDbContext(DbContextOptions options) : base(options)
        {
            SetNullsForInjectedProperties();
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfiguration(new MyAppRoleMap());
            modelBuilder.ApplyConfiguration(new MyAppUserMap());
            modelBuilder.ApplyConfiguration(new MyAppUserRoleMap());
            modelBuilder.ApplyConfiguration(new MyAppUserLoginMap());
            modelBuilder.ApplyConfiguration(new MyAppUserTokenMap());
            modelBuilder.ApplyConfiguration(new MyAppUserClaimMap());

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        public static string GetFileWithName(string filePath)
        {
            var baseDir = $@"{AppDomain.CurrentDomain.BaseDirectory}";
            //Enum.Parse<GCCollectionMode>()
            Debug.WriteLine(baseDir);

            if (Directory.Exists($"{baseDir}\bin"))
                return $@"{baseDir}\\bin{filePath}";
            else
                return $@"{baseDir}\{filePath}";
        }


        public new DbSet<TEntity> Set<TEntity>() where TEntity : AuditedEntity
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : AuditedEntity
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : AuditedEntity
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : AuditedEntity
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }
        public void SetAsDetached<TEntity>(TEntity entity) where TEntity : AuditedEntity
        {
            UpdateEntityState(entity, EntityState.Detached);
        }
        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }
        public int Commit()
        {
            var saveChanges = SaveChanges();
            _transaction.Commit();
            return saveChanges;
        }
        public void Rollback()
        {
            _transaction.Rollback();
        }
        public async Task<int> CommitAsync()
        {
            var saveChangesAsync = await SaveChangesAsync();
            _transaction.Commit();
            return saveChangesAsync;
        }
        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : AuditedEntity
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }
        private EntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : AuditedEntity
        {
            var dbEntityEntry = Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return string.Empty;
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : AuditedEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : AuditedEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    if (!(parameters[i] is DbParameter p))
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = SqlQuery<TEntity>(commandText, parameters).ToList();

            for (int i = 0; i < result.Count; i++)
                result[i] = AttachEntityToContext(result[i]);

            return result;
        }
        /// <summary>
        /// Use when Model isn't attach in context
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> ExecuteSqlQuery<TElement>(string sql, params object[] parameters) where TElement :  new()
        {
            return Database.GetModelFromQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IQueryable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : AuditedEntity
        {
            return base.Set<TElement>().FromSql<TElement>(sql, parameters);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            var result = this.Database.ExecuteSqlCommand(sql, parameters);
            return result;
        }

        async Task<int> IDbContext.SaveChangesAsync()
        {
            return await SaveChangesAsync();
        }
    }
}
