using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //string settingsFile = "";
            //github.SaveToFile(settingsFile);
            //PercolatorSettings github = PercolatorSettings.LoadFromFile(settingsFile);

            PercolatorSettings github = GitHub();
            PercolatorSettings jobs = JobOpenings();

            Percolate(github);
            Percolate(jobs);
        }

        private static void Percolate(PercolatorSettings percolatorSettings)
        {
            IFilter[] filters = percolatorSettings.Filters != null ?
                percolatorSettings.Filters.Select(filterSettings => Filter.Create(filterSettings)).ToArray() :
                new IFilter[0];

            IPipelineEvaluator pipeline = PipelineEvaluator.Create();

            foreach (var pipelineSettings in percolatorSettings.Pipelines)
            {
                if (!Path.IsPathRooted(pipelineSettings.Output))
                {
                    pipelineSettings.Output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pipelineSettings.Output);
                }

                pipeline.Execute(filters, pipelineSettings);
            }
        }

        private static PercolatorSettings JobOpenings()
        {
            var pipelineSettings = new PercolatorSettings
            {
                Pipelines = new[]
                { 
                    new PipelineSettings
                    {
                        Inputs = new []
                        { 
                            "http://rss.indeed.com/rss?q=machine+learning&l=San+Fransisco%2C+CA&sort=date",
                            "http://rss.indeed.com/rss?q=data+mining&l=San+Fransisco%2C+CA&sort=date",
                        },

                        Output = "job_feed.xml",
                        Title = "Jobs",
                        Description = "Aggregated & filtered feed of ML job openings"
                    }
                },
                Filters = new[]
                {
                    // First, exlude all
                    new FilterSettings
                    {
                        Action = FilterAction.Exclude,
                        Field = FeedField.Any,
                        PatternType = PatternType.Glob,
                        Patterns = new [] { "*" }
                    },
                    
                    // Only include posts with the following keywords
                    new FilterSettings
                    {
                        Action = FilterAction.Include,
                        Field = FeedField.Any,
                        PatternType = PatternType.String,
                        Patterns = new [] 
                        { 
                            ".net","sql","c#","java","python","data mining","machine learning" 
                        }
                    },

                    // Remove irrelevant posts & spam
                    new FilterSettings
                    {
                        Action = FilterAction.Exclude,
                        Field = FeedField.Title,
                        PatternType = PatternType.String,
                        Patterns = new [] 
                        { 
                            "associate","clerk","webmaster","specialist","sales","insurance","junior","entry","jr.","intern"
                        }
                    },

                }
            };

            return pipelineSettings;
        }

        private static PercolatorSettings GitHub()
        {
            var pipelineSettings = new PercolatorSettings
            {
                Pipelines = new[]
                { 
                    new PipelineSettings
                    {
                        Inputs = new []
                        { 
                            "https://github.com/StackExchange/dapper-dot-net/commits.atom",
                            "https://github.com/JamesNK/Newtonsoft.Json/commits/master.atom",
                            "https://github.com/StackExchange/StackExchange.Redis/commits/master.atom",
                            "https://github.com/olviko/RssPercolator/commits/master.atom"
                        },
                        Output = "github_feed.xml",
                        Title = "Github - Project releases",
                        Description = "Aggregated feed of multiple projects"
                    }
                },
                Filters = new[]
                {
                    // First, exlude all
                    new FilterSettings
                    {
                        Action = FilterAction.Exclude,
                        Field = FeedField.Any,
                        PatternType = PatternType.Glob,
                        Patterns = new [] { "*" }
                    },
                
                    // Include activity related to the specific project
                    new FilterSettings
                    {
                        Action = FilterAction.Include,
                        Field = FeedField.Any,
                        PatternType = PatternType.String,
                        Patterns = new [] 
                        { 
                            "Release", "Version"
                        }
                    }
                }
            };

            return pipelineSettings;
        }

    }
}
