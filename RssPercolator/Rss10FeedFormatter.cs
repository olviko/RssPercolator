using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RssPercolator
{
    /// <summary>
    /// RSS 1.0 formatter
    /// </summary>
    internal class Rss10FeedFormatter : SyndicationFeedFormatter
    {
        public override string Version
        {
            get { return "Rss10"; }
        }

        public override bool CanRead(XmlReader reader)
        {
            return reader.IsStartElement("RDF", rdfNs);
        }

        protected override SyndicationFeed CreateFeedInstance()
        {
            return new SyndicationFeed();
        }

        public override void ReadFrom(XmlReader reader)
        {
            this.SetFeed(this.CreateFeedInstance());

            reader.ReadStartElement(); // <RDF>

            // process <channel>

            reader.ReadStartElement("channel"); // <channel>   
            while (reader.IsStartElement())         
            {
                if (reader.IsStartElement("title"))
                    Feed.Title = new TextSyndicationContent(reader.ReadElementString());
                else if (reader.IsStartElement("link"))
                    Feed.Links.Add(new SyndicationLink(new Uri(reader.ReadElementString())));
                else if (reader.IsStartElement("description"))
                    Feed.Description = new TextSyndicationContent(reader.ReadElementString());
                else if (reader.IsStartElement("updateBase", synNs))
                    Feed.LastUpdatedTime = DateTimeOffset.Parse(reader.ReadElementString());
                else if (reader.IsStartElement("language", dcNs))
                    Feed.Language = reader.ReadElementString();
                else
                    reader.Skip();
            }
            reader.ReadEndElement(); // </channel>   

            while (reader.IsStartElement())
            {
                if (reader.IsStartElement("item"))
                {
                    var items = new List<SyndicationItem>();

                    while (reader.IsStartElement("item"))
                    {
                        SyndicationItem item = new SyndicationItem();

                        reader.ReadStartElement(); // <item>
                        while (reader.IsStartElement())
                        {
                            if (reader.IsStartElement("title"))
                                item.Title = new TextSyndicationContent(reader.ReadElementString());
                            else if (reader.IsStartElement("link"))
                                item.Links.Add(new SyndicationLink(new Uri(reader.ReadElementString())));
                            else if (reader.IsStartElement("description"))
                                item.Summary = new TextSyndicationContent(reader.ReadElementString());
                            else if (reader.IsStartElement("source", dcNs))
                                item.Id = reader.ReadElementString();
                            else if (reader.IsStartElement("date", dcNs))
                                item.PublishDate = item.LastUpdatedTime = DateTimeOffset.Parse(reader.ReadElementString());
                            else
                                reader.Skip();
                        }
                        reader.ReadEndElement(); // </item>
                        items.Add(item);
                    }

                    Feed.Items = items;
                    break;
                }

                reader.Skip();
            }
        }

        public override void WriteTo(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        const string rdfNs = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        const string dcNs = "http://purl.org/dc/elements/1.1/";
        const string synNs = "http://purl.org/rss/1.0/modules/syndication/";
    }
}
