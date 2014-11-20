RssPercolator
=============

RssPercolator is a .NET library used for downloading, aggregating, and filtering RSS feeds. Developed out of frustration with [Yahoo Pipes](https://pipes.yahoo.com/pipes/).

Features
--

- RSS and Atom feed formats
- Asynchronous multi-feed download
- Multiple sources and multiple destinations
- Feed filters (string match, wildcards, and Regex)

Example
--
```c#
PercolatorSettings pipelineSettings = new PercolatorSettings
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
            Output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "github_feed.xml"),
            Title = "Github - Project releases",
            Description = "Aggregated feed of multiple projects"
        }
    },
    // Filters are always evaluated from top to bottom
    Filters = new[]
    {
        // First, exclude all
        new FilterSettings
        {
            Action = FilterAction.Exclude,
            Field = FeedField.Any,
            PatternType = PatternType.Glob,
            Patterns = new [] { "*" }
        },
    
        // Include posts with "Release" or "Version" keywords
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

// Create filters
IFilter[] filters = percolatorSettings.Filters.Select(filterSettings => Filter.Create(filterSettings)).ToArray();

// Create pipeline evaluator
IPipelineEvaluator pipeline = PipelineEvaluator.Create();

// Execute each pipeline
foreach (var pipelineSettings in percolatorSettings.Pipelines)
{
    pipeline.Execute(filters, pipelineSettings);
}
```

