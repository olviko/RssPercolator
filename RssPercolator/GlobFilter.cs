using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RssPercolator
{
    /// <summary>
    /// Filter using wilcards.
    /// <remarks>This filter is case insensitive.</remarks>
    /// </summary>
    internal sealed class GlobFilter : Filter
    {
        private readonly string[] patterns;

        public GlobFilter(FilterAction action, FeedField scope, IEnumerable<string> patterns)
            : base(action, scope)
        {
            if (patterns == null)
                throw new ArgumentNullException("patterns");

            this.patterns = patterns.ToArray();
        }

        protected override bool IsMatch(string text)
        {
            return text.WildcardMatch(patterns);
        }
    }
}
