﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Parameterizes queries
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public interface IParameterizedRepository<TEntity> : IRepository<TEntity>
      where TEntity : class
    {
        /// <summary>
        /// Gets a list of reference properties to include
        /// </summary>
        IDictionary<string, object> Parameters { get; }

        /// <summary>
        /// Adds a parameter to queries
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Current instance</returns>
        IParameterizedRepository<TEntity> SetParameter(string name, object value);

        /// <summary>
        /// Gets parameter value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Parameter value</returns>
        object GetParameter(string name);

        /// <summary>
        /// Clears parameters
        /// </summary>
        /// <returns>Current instance</returns>
        IParameterizedRepository<TEntity> ClearParameters();
    }
}
