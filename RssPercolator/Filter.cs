using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Base class of all filters
    /// </summary>
    public abstract class Filter : IFilter
    {
        /// <summary>
        /// Action returned when the filter finds a match.
        /// </summary>
        public FilterAction Action { get; private set; }

        /// <summary>
        /// Rss field used by the filter.
        /// </summary>
        public FeedField Field { get; private set; }

        public Filter(FilterAction action, FeedField field)
        {
            this.Action = action;
            this.Field = field;
        }

        /// <summary>
        /// Factory method for creating specific filters.
        /// </summary>
        /// <param name="settings">Filter configuration.</param>
        /// <returns>IFilter implementaiton.</returns>
        public static IFilter Create(FilterSettings settings)
        {
            switch (settings.PatternType)
            {
                case PatternType.String:
                    return new StringFilter(settings.Action, settings.Field, settings.Patterns);

                case PatternType.Glob:
                    return new GlobFilter(settings.Action, settings.Field, settings.Patterns);

                case PatternType.Regex:
                    return new RegexFilter(settings.Action, settings.Field, settings.Patterns);

                default:
                    throw new NotImplementedException();
            }
        }

        public FilterAction Apply(SyndicationItem item)
        {
            bool result;

            TextSyndicationContent content = item.Content as TextSyndicationContent;
            TextSyndicationContent summary = item.Summary;

            switch (Field)
            {
                case FeedField.Any:
                    result = IsMatch(item.Title.Text) || (summary != null && IsMatch(summary.Text)) || (content != null && IsMatch(content.Text));
                    break;

                case FeedField.Title:
                    result = IsMatch(item.Title.Text);
                    break;

                case FeedField.Description:
                    result = (summary != null && IsMatch(summary.Text)) || (content != null && IsMatch(content.Text));
                    break;

                default:
                    throw new NotImplementedException();
            }

            return result ? this.Action : FilterAction.None;
        }

        protected abstract bool IsMatch(string text);
    }
}
