﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Filters a sequence of entities using a predicate
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IFindSql<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Filters a collection of entities using a predicate
        /// </summary>
        /// <param name="sql">SQL containing named parameter placeholders. For example: SELECT * FROM Customer WHERE Id = @Id</param>
        /// <param name="parameters">Named parameters</param>
        /// <param name="parameterPattern">Parameter Regex pattern, Defualts to @(\w+)</param>
        /// <returns>Filtered collection of entities</returns>
        IEnumerable<TEntity> Find(
          string sql,
          IDictionary<string, object> parameters = null,
          string parameterPattern = @"@(\w+)");
    }
}
