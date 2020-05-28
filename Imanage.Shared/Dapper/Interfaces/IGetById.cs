﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Gets an entity by id.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IGetById<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">Filter to find a single item</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id);
        IEnumerable<TEntity> GetLById(object id);
    }
}