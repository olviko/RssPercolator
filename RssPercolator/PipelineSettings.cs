using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator
{
    public sealed class PipelineSettings
    {
        /// <summary>
        /// List of input rss feeds
        /// </summary>
        public IList<string> Inputs { get; set; }

        /// <summary>
        /// Output feed file path
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Output feed title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Output feed description
        /// </summary>
        public string Description { get; set; }
    }
}
