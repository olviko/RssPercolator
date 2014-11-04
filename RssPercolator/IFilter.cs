using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    /// <summary>
    /// Interface for all rss filters
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Executes filter on rss item.
        /// </summary>
        /// <param name="item">RSS feed item</param>
        /// <returns>Action that needs to be performed on the provided item.</returns>
        FilterAction Apply(SyndicationItem item);
    }
}
