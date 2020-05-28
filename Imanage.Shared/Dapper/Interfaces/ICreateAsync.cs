using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface ICreateAsync<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Task</returns>
        Task CreateAsync(TEntity entity);
    }
}
