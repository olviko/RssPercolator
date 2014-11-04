using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Filter actions.
    /// </summary>
    public enum FilterAction
    {
        /// <summary>
        /// Skip filter.
        /// </summary>
        None = 0,
        /// <summary>
        /// Include item into results.
        /// </summary>
        Include = 1,
        /// <summary>
        /// Exclude item from results.
        /// </summary>
        Exclude = 2
    }
}
