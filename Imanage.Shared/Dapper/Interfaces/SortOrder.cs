using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Dapper.Interfaces
{
    /// <summary>
    /// Specifies how rows of data are sorted.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// The default. No sort order is specified.
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// Rows are sorted in ascending order.
        /// </summary>
        Ascending = 0,

        /// <summary>
        /// Rows are sorted in descending order.
        /// </summary>
        Descending = 1
    }
}
