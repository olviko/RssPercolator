using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Filter settings
    /// </summary>
    public sealed class FilterSettings
    {
        /// <summary>
        /// Filter action.
        /// </summary>
        public FilterAction Action { get; set; }

        /// <summary>
        /// Rss field used by the filter.
        /// </summary>
        public FeedField Field { get; set; }

        /// <summary>
        /// Filter match type.
        /// </summary>
        public PatternType PatternType { get; set; }

        /// <summary>
        /// Patterns.
        /// </summary>
        public IList<string> Patterns { get; set; }
    }
}
