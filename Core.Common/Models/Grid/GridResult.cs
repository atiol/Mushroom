using System.Collections.Generic;

namespace Core.Common.Models.Grid
{
    /// <summary>
    /// Represents the default implementation of the <see cref="GridResult{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the data to page</typeparam>
    public sealed class GridResult<T>
    {
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; set; }
        
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IEnumerable<T> Items { get; set; }
    }
}