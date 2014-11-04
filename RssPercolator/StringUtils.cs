using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    internal static class StringUtils
    {
        /// <summary>
        /// Wildcard pattern match.
        /// </summary>
        [DebuggerStepThrough]
        public static bool WildcardMatch(this string source, params string[] patterns)
        {
            if (source == null)
                return false;

            if (patterns == null || patterns.Length == 0)
                return true;

            return !Array.TrueForAll(patterns, p => !WildcardMatch(source, p));
        }

        /// <summary>
        /// Wildcard pattern match.
        /// </summary>
        [DebuggerStepThrough]
        public static bool WildcardMatch(this string source, string pattern)
        {
            if (source == null)
                return false;

            if (pattern == null)
                return true;

            pattern = pattern.ToUpperInvariant();
            source = source.ToUpperInvariant();

            int i, star, ii = 0, pi = 0;

        new_segment:

            star = 0;
            if (pattern[pi] == '*')
            {
                star = 1;
                while (++pi < pattern.Length && pattern[pi] == '*') ;
            }

        test_match:

            for (i = 0; i + pi < pattern.Length && (pattern[i + pi] != '*'); i++)
            {
                if (i + ii == source.Length)
                    return false;

                if (source[i + ii] != pattern[i + pi])
                {
                    if ((pattern[i + pi] == '?') && (source[i + ii] != '.'))
                        continue;

                    if (star == 0)
                        return false;

                    ii++;
                    goto test_match;
                }
            }

            if (i + pi < pattern.Length && pattern[i + pi] == '*')
            {
                ii += i;
                pi += i;
                goto new_segment;
            }

            if (i + ii == source.Length)
                return true;

            if (i != 0 && pattern[pi + i - 1] == '*')
                return true;

            if (star == 0)
                return false;

            ii++;

            goto test_match;
        }

    }
}
