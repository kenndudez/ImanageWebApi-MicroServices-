using Imanage.Shared.Dapper.Interfaces;


namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Abstracts data access for en entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public interface IRepository<TEntity> :
      ICreate<TEntity>,
      ICreateAsync<TEntity>,
      ICreateMany<TEntity>,
      ICreateManyAsync<TEntity>,
      IDelete<TEntity>,
      IDeleteAsync<TEntity>,
      IDeleteMany<TEntity>,
      IDeleteManyAsync<TEntity>,
      IGetById<TEntity>,
      IGetByIdAsync<TEntity>,
      IFind<TEntity>,
      IFindAsync<TEntity>,
      IUpdate<TEntity>,
      IUpdateAsync<TEntity>
      where TEntity : class
    {
    }
}