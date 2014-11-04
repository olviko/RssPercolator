RssPercolator
=============

RssPercolator is a .NET library used for downloading, aggregating, and filtering RSS feeds.

Features
--

- Support for RSS 2.0 and Atom for input and output
- Asynchronous feed download
- Suport for multiple sources and multiple destinations
- Filters (string match, wildcards, and Regex)

Example
--

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

            // Build filters
            IFilter[] filters = percolatorSettings.Filters != null ?
                percolatorSettings.Filters.Select(filterSettings => Filter.Create(filterSettings)).ToArray() :
                new IFilter[0];

            // Create pipeline evaluator
            IPipelineEvaluator pipeline = PipelineEvaluator.Create();

            // Execute each pipeline
            foreach (var pipelineSettings in percolatorSettings.Pipelines)
            {
                pipeline.Execute(filters, pipelineSettings);
            }


