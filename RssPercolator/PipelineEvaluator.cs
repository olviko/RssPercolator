using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http;
using System.IO;

namespace RssPercolator
{
    /// <summary>
    /// Executes a pipeline.
    /// <remarks>
    /// Filters are always executed in a sequence from top to bottom. Make sure to put broad filters 
    /// in the beginning.
    /// </remarks>
    /// </summary>
    public sealed class PipelineEvaluator : IPipelineEvaluator
    {
        public static IPipelineEvaluator Create()
        {
            return new PipelineEvaluator();
        }

        private PipelineEvaluator()
        {
            this.httpClient = new HttpClient();
        }

        public void Execute(IList<IFilter> filters, PipelineSettings pipelineSettings)
        {
            IEnumerable<SyndicationItem> items;

            if (pipelineSettings.Inputs != null)
            {
                // Get a list of feed items
                items = from feed in ParallelCrawl(pipelineSettings.Inputs)
                                                        from i in feed.Items
                                                        select i;
            }
            else
            {
                items = new SyndicationItem[0];
            }

            // Filter
            IEnumerable<SyndicationItem> filtered = items
                .Where(x => ApplyFilters(filters, x) == FilterAction.Include);

            // Remove duplicates
            IEnumerable<SyndicationItem> merged = Dedup(filtered);

            if (pipelineSettings.Output != null)
            {
                // Save results
                var newFeed = new SyndicationFeed(merged.OrderBy(i => i.PublishDate));

                newFeed.Title = SyndicationContent.CreatePlaintextContent(pipelineSettings.Title);
                newFeed.Description = SyndicationContent.CreatePlaintextContent(pipelineSettings.Description);
                newFeed.LastUpdatedTime = DateTimeOffset.Now;

                using (XmlWriter writer = XmlWriter.Create(pipelineSettings.Output))
                {
                    newFeed.SaveAsAtom10(writer);
                }
            }
        }

        private IList<SyndicationFeed> ParallelCrawl(IList<string> urls)
        {
            var tasks = urls.Select(url =>
            {
                return httpClient.GetStreamAsync(url)
                    .ContinueWith(task =>
                    {
                        using (Stream responseStream = task.Result)
                        using (XmlReader reader = XmlReader.Create(responseStream))
                        {
                            return SyndicationFeed.Load(reader);
                        }
                    });
            });

            return Task.WhenAll(tasks).Result;
        }

        private static FilterAction ApplyFilters(IEnumerable<IFilter> filters, SyndicationItem item)
        {
            // By default all items are included
            FilterAction result = FilterAction.Include;

            if (filters != null)
            {
                // All filters are executed in order of definition
                foreach (var filter in filters)
                {
                    FilterAction action = filter.Apply(item);
                    if (action != FilterAction.None)
                    {
                        // The result is only changed when the filter "applies"
                        result = action;
                    }
                }
            }

            return result;
        }

        private static IEnumerable<SyndicationItem> Dedup(IEnumerable<SyndicationItem> items)
        {
            // Dedup feed items using Id, Title, and Link

            HashSet<string> titles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<Uri> links = new HashSet<Uri>();

            foreach (SyndicationItem item in items)
            {
                if (ids.Add(item.Id) && titles.Add(item.Title.Text))
                {
                    SyndicationLink link = item.Links.SingleOrDefault(x => x.RelationshipType == "alternate");

                    if (link == null || links.Add(link.Uri))
                    {
                        yield return item;
                    }
                }
            }
        }

        private readonly HttpClient httpClient;
    }
}
