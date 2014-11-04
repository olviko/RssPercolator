using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// RSS feed item fields.
    /// </summary>
    public enum FeedField
    {
        /// <summary>
        /// Apply filter to all available fields.
        /// </summary>
        Any = 0,
        /// <summary>
        /// Apply filter to a title.
        /// </summary>
        Title = 1,
        /// <summary>
        /// Apply filter to a description.
        /// </summary>
        Description = 2
    }
}
