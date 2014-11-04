using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Filter using regular expressions.
    /// <remarks>This filter is case insensitive.</remarks>
    /// </summary>
    internal sealed class RegexFilter : Filter
    {
        private readonly Regex[] regularExpressions;

        public RegexFilter(FilterAction action, FeedField scope, IEnumerable<string> patterns)
            : base(action, scope)
        {
            if (patterns == null)
                throw new ArgumentNullException("patterns");

            this.regularExpressions = patterns
                .Select(pattern => new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToArray();
        }

        protected override bool IsMatch(string text)
        {
            return !Array.TrueForAll(regularExpressions, regex => !regex.Match(text).Success);
        }
    }
}
