using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Type of the pattern
    /// </summary>
    public enum PatternType
    {
        /// <summary>
        /// Regular string.
        /// </summary>
        String = 1,

        /// <summary>
        /// Wildcard.
        /// </summary>
        Glob = 2,

        /// <summary>
        /// Regular expression.
        /// </summary>
        Regex = 3
    }
}
