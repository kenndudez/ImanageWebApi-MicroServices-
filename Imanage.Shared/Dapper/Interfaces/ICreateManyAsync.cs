using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Creates a list of new entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface ICreateManyAsync<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Create a list of new entities
        /// </summary>
        /// <param name="entities">List of entities</param>
        /// <returns>Task</returns>
        Task CreateManyAsync(IEnumerable<TEntity> entities);
    }
}
