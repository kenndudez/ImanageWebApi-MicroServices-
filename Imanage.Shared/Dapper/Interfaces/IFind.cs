using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Finds a list of entites.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IFind<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Get a list of entities
        /// </summary>
        /// <returns>Query result</returns>
        IEnumerable<TEntity> Find();
    }
}
