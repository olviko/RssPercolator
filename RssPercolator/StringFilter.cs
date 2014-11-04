using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Filter using exact match.
    /// <remarks>This filter is case insensitive.</remarks>
    /// </summary>
    internal sealed class StringFilter : Filter
    {
        private readonly string[] patterns;

        public StringFilter(FilterAction action, FeedField scope, IEnumerable<string> patterns)
            : base(action, scope)
        {
            if (patterns == null)
                throw new ArgumentNullException("patterns");

            this.patterns = patterns.ToArray();
        }

        protected override bool IsMatch(string text)
        {
            return !Array.TrueForAll(patterns, pattern => text.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) < 0);
        }
    }
}
